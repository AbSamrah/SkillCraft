using BuissnessLogicLayer.Profiles;
using BuissnessLogicLayer.Services;
using DataAccessLayer.Auth;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ProfilesManagement.BuisnessLogicLayer.Profiles;
using ProfilesManagement.BuisnessLogicLayer.Services;
using ProfilesManagement.DataAccessLayer.Data;
using ProfilesManagement.DataAccessLayer.Interfaces;
using ProfilesManagement.DataAccessLayer.Repositories;
using QuizesManagement.BuisnessLogicLayer.Profiles;
using QuizesManagement.BuisnessLogicLayer.Services;
using QuizesManagement.DataAccessLayer.Data;
using QuizesManagement.DataAccessLayer.Interfaces;
using QuizesManagement.DataAccessLayer.Repositories;
using RoadmapMangement.BuisnessLogicLayer.Profiles;
using RoadmapMangement.BuisnessLogicLayer.Services;
using RoadmapMangement.DataAccessLayer.Data;
using RoadmapMangement.DataAccessLayer.Interfaces;
using RoadmapMangement.DataAccessLayer.Repositories;
using SkillCraft.Api.Middleware;
using System;
using System.Collections.Generic;
using System.Text;
using UsersManagement.BuissnessLogicLayer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    var serializerOptions = opt.JsonSerializerOptions;
                    serializerOptions.IgnoreReadOnlyProperties = false;
                    serializerOptions.WriteIndented = true;
                });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("UsersMangement"));
});
builder.Services.AddScoped<IRoadmapDbContext, RoadmapDbContext>();
builder.Services.AddScoped<IQuizDbContext, QuizDbContext>();
builder.Services.AddScoped<IProfileDbContext, ProfileDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});

// Register HttpClient for DI
builder.Services.AddHttpClient();

// Business Services & Repositories
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRoadmapRepository, RoadmapRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMilestoneRepository, MilestoneRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IMultipleChoicesQuizRepository, MultipleChoicesQuizRepository>();
builder.Services.AddScoped<ITrueOrFalseQuizRepository, TrueOrFalseQuizRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StepsService>();
builder.Services.AddScoped<MilestonesService>();
builder.Services.AddScoped<IRoadmapsService, RoadmapsService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IMultipleChoisesQuizService, MultipleChoisesQuizService>();
builder.Services.AddScoped<ITrueOrFalseQuizService, TrueOrFlaseQuizService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<RoadmapMangement.BuisnessLogicLayer.Services.IAiGenerator, RoadmapMangement.BuisnessLogicLayer.Services.GeminiAiAdapter>();
builder.Services.AddScoped<QuizesManagement.BuisnessLogicLayer.Services.IAiGenerator, QuizesManagement.BuisnessLogicLayer.Services.GeminiAiAdapter>();

builder.Services.AddScoped<RoadmapMangement.BuisnessLogicLayer.Services.IStrategyFactory, RoadmapMangement.BuisnessLogicLayer.Services.StrategyFactory>();
builder.Services.AddScoped<QuizesManagement.BuisnessLogicLayer.Services.IStrategyFactory, QuizesManagement.BuisnessLogicLayer.Services.StrategyFactory>();


// Register Roadmap Creation Strategies
builder.Services.AddScoped<ManualRoadmapCreationStrategy>();
builder.Services.AddScoped<AiRoadmapCreationStrategy>();

builder.Services.AddScoped<ManualQuizCreationStrategy>();
builder.Services.AddScoped<AiQuizCreationStrategy>();

//// Register the factory delegate to resolve strategies by key
//builder.Services.AddTransient<Func<string, IRoadmapCreationStrategy>>(serviceProvider => key =>
//{
//    switch (key.ToLowerInvariant())
//    {
//        case "manual":
//            return serviceProvider.GetRequiredService<ManualRoadmapCreationStrategy>();
//        case "ai":
//            return serviceProvider.GetRequiredService<AiRoadmapCreationStrategy>();
//        default:
//            throw new KeyNotFoundException($"Strategy '{key}' not found.");
//    }
//});

// Units of Work
builder.Services.AddScoped<RoadmapMangement.DataAccessLayer.Interfaces.IUnitOfWork, RoadmapMangement.DataAccessLayer.Uow.UnitOfWork>();
builder.Services.AddScoped<QuizesManagement.DataAccessLayer.Interfaces.IUnitOfWork, QuizesManagement.DataAccessLayer.Uow.UnitOfWork>();

// AutoMapper Profiles
builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);
builder.Services.AddAutoMapper(typeof(StepsProfile).Assembly);
builder.Services.AddAutoMapper(typeof(QuizesProfile).Assembly);
builder.Services.AddAutoMapper(typeof(ProfilesProfile).Assembly);

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
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
