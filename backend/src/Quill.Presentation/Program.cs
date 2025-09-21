using Microsoft.EntityFrameworkCore;
using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Application.Mappings;
using Quill.Infrastructure.Options;
using Quill.Infrastructure.Persistence;
using Quill.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Quill.Application.Validators.User;
using Quill.Application.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<DatabaseOptions>(builder.Configuration.GetSection(DatabaseOptions.SectionName));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddValidatorsFromAssembly(typeof(UserRegisterDtoValidator).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString(DatabaseOptions.ConnectionStringName)));

var app = builder.Build();

app.Run();