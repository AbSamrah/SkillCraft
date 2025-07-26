using BuissnessLogicLayer.Profiles;
using BuissnessLogicLayer.Services;
using DataAccessLayer.Auth;
using DataAccessLayer.Data;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
using System.Text;
using UsersManagement.BuissnessLogicLayer.Services;
using SkillCraft.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    var serializerOptions = opt.JsonSerializerOptions;
                    serializerOptions.IgnoreReadOnlyProperties = false;
                    serializerOptions.WriteIndented = true;
                });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<UsersDbContext>(options =>
{
    options.UseSqlServer(connectionString: builder.Configuration.GetConnectionString("UsersMangement"));
});
builder.Services.AddScoped<IRoadmapDbContext, RoadmapDbContext>();
builder.Services.AddScoped<IQuizDbContext, QuizDbContext>();


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

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRoadmapRepository, RoadmapRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMilestoneRepository, MilestoneRepository>();
builder.Services.AddScoped<IQuizRepository, QuizRepository>();
builder.Services.AddScoped<IMultipleChoicesQuizRepository, MultipleChoicesQuizRepository>();


builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<UsersService>();
builder.Services.AddScoped<RolesService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<StepsService>();
builder.Services.AddScoped<MilestonesService>();
builder.Services.AddScoped<RoadmapsService>();
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<IMultipleChoisesQuizService, MultipleChoisesQuizService>();


builder.Services.AddScoped<RoadmapMangement.DataAccessLayer.Interfaces.IUnitOfWork, RoadmapMangement.DataAccessLayer.Uow.UnitOfWork>();
builder.Services.AddScoped<QuizesManagement.DataAccessLayer.Interfaces.IUnitOfWork, QuizesManagement.DataAccessLayer.Uow.UnitOfWork>();

builder.Services.AddAutoMapper(typeof(UsersProfile).Assembly);
builder.Services.AddAutoMapper(typeof(StepsProfile).Assembly);
builder.Services.AddAutoMapper(typeof(QuizesProfile).Assembly);

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
