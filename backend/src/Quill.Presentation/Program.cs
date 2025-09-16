using Quill.Application.Interfaces;
using Quill.Application.Interfaces.Repositories;
using Quill.Infrastructure.Persistence;
using Quill.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<AppDbContext>();

var app = builder.Build();

app.Run();