using System.Reflection;
using System.Text;
using System.Threading.RateLimiting;
using Asp.Versioning.ApiExplorer;
using FluentValidation;
using HealthChecks.UI.Client;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Interfaces.Services;
using Quill.Application.Mappings;
using Quill.Application.Options;
using Quill.Application.Services;
using Quill.Application.Validators.User;
using Quill.Infrastructure.Options;
using Quill.Infrastructure.Persistence;
using Quill.Infrastructure.Persistence.Repositories;
using Quill.Presentation.Middleware;
using Quill.Presentation.Swagger;
using Serilog;
using Serilog.Events;
using Swashbuckle.AspNetCore.SwaggerGen;


var builder = WebApplication.CreateBuilder(args);

// Serilog
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.ReadFrom.Configuration(context.Configuration);

    loggerConfig.Filter.ByExcluding(logEvent =>
    {
        if (logEvent.Properties.TryGetValue("RequestPath", out var requestPathValue))
        {
            if (requestPathValue is ScalarValue scalarValue && scalarValue.Value is string path)
            {
                return path.StartsWith("/health", StringComparison.OrdinalIgnoreCase);
            }
        }
        return false;
    });
});

// Health Check
builder.Services.AddHealthChecks()
    .AddNpgSql(
        connectionString: builder.Configuration.GetConnectionString(DatabaseOptions.ConnectionStringName)!,
        name: "PostgreSQL Database",
        failureStatus: Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy,
        tags: new string[] { "database", "postgres" }
    );

// Health Check UI
builder.Services
    .AddHealthChecksUI()
    .AddInMemoryStorage();


// Rate Limiting Policies

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter(policyName: "fixed", fixedWindowOptions =>
    {
        fixedWindowOptions.PermitLimit = 20;
        fixedWindowOptions.Window = TimeSpan.FromSeconds(10);
    });

    options.AddFixedWindowLimiter(policyName: "auth", fixedWindowOptions =>
    {
        fixedWindowOptions.PermitLimit = 5;
        fixedWindowOptions.Window = TimeSpan.FromMinutes(1);
    });

    options.AddFixedWindowLimiter(policyName: "admin", fixedWindowOptions =>
    {
        fixedWindowOptions.PermitLimit = 30;
        fixedWindowOptions.Window = TimeSpan.FromSeconds(10);
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

        if (context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
        {
            context.HttpContext.Response.Headers.RetryAfter = retryAfter.TotalSeconds.ToString("0");

            await context.HttpContext.Response.WriteAsync(
                $"Too many requests. Please try again after {retryAfter.TotalSeconds:0} second(s).", cancellationToken: token);
        }
        else
        {
            await context.HttpContext.Response.WriteAsync(
                "Too many requests. Please try again later.", cancellationToken: token);
        }
    };
});

// --- Configure Services ---

// Options Pattern Registration
builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));


// Authentication & Authorization Services
var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()!;
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});
builder.Services.AddAuthorization();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0); // Default 1.0

    // If the client does not specify a version, assume that the default version will be used.
    options.AssumeDefaultVersionWhenUnspecified = true;

    // In the response header, send information indicating which versions of the API are supported.
    options.ReportApiVersions = true;

    // We will use the URL segment (e.g., /api/v1/posts).
    options.ApiVersionReader = new Asp.Versioning.UrlSegmentApiVersionReader();
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";

    // In Swagger UI, if no version is specified for an endpoint, ensure it uses the default version.
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

// Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);

    options.IncludeXmlComments(xmlPath);
});

// Project Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Project Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ITagService, TagService>();
builder.Services.AddScoped<IUserService, UserService>();

// AutoMapper Resolvers
builder.Services.AddScoped<CategoryPostCountResolver>();
builder.Services.AddScoped<TagPostCountResolver>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Other Application Services
builder.Services.AddValidatorsFromAssembly(typeof(UserRegisterDtoValidator).Assembly);
builder.Services.AddFluentValidationRulesToSwagger();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString(DatabaseOptions.ConnectionStringName)));
builder.Services.AddControllers();

// --- Build The App ---
var app = builder.Build();

// --- Configure HTTP Request Pipeline ---

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseRateLimiter();

app.UseSerilogRequestLogging();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,

    ResultStatusCodes =
    {
        [HealthStatus.Healthy] = StatusCodes.Status200OK,
        [HealthStatus.Degraded] = StatusCodes.Status200OK,
        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
    },
    AllowCachingResponses = false
});

app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui";
});

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var dbContext = services.GetRequiredService<AppDbContext>();
        
        if (dbContext.Database.IsRelational())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
    }
}


app.Run();