using BuissnessLogicLayer.Models;
using BuissnessLogicLayer.Profiles;
using BuissnessLogicLayer.Services;
using DataAccessLayer.Auth;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("UsersMangement"));
});
builder.Services.AddScoped<IPasswordHasher,  PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();

builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); 
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
