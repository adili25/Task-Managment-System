using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task_managment_system.Database;
using Task_managment_system.Extentions;
using Task_managment_system.Interfaces;
using Task_managment_system.Middlewares;
using Task_managment_system.Repositries;
using Task_managment_system.Services;
using Task_managment_system.Models;


var builder = WebApplication.CreateBuilder(args);

// JWT Authentication

var issuer = builder.Configuration["JwtSettings:Issuer"]
    ?? throw new InvalidOperationException(
        "Missing JwtSettings:Issuer in configuration.");

var audience = builder.Configuration["JwtSettings:Audience"]
    ?? throw new InvalidOperationException(
        "Missing JwtSettings:Audience in configuration.");

var secret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException(
        "Missing JwtSettings:Secret in configuration.");

var connectinoString = builder.Configuration["DbConnectionString"]
    ?? throw new InvalidOperationException(
        "missing User secrets: DbConnectionsString in configurations ");

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = issuer,
            ValidAudience = audience,

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(secret))
        };

        //logs the real reason a token was rejected
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                Console.WriteLine($"---> JWT MESSAGE RECEIVED. Token present: {!string.IsNullOrEmpty(context.Token)}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("---> JWT TOKEN VALIDATED SUCCESSFULLY");
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"---> JWT VALIDATION FAILED: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                Console.WriteLine($"---> JWT AUTH CHALLENGE. Error: '{context.Error}', Description: '{context.ErrorDescription}', AuthFailure: '{context.AuthenticateFailure?.Message}'");
                return Task.CompletedTask;
            },
            OnForbidden = context =>
            {
                Console.WriteLine("---> JWT FORBIDDEN (authenticated but role/policy check failed)");
                return Task.CompletedTask;
            }
        };
    });


// Authorization
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("Admin", policy =>
    {
        policy.RequireRole("Admin");
    });

// Dependency Injection

// Singleton repositories because they hold application data
// for the lifetime of the application.

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectinoString));

//---> here DbContext will crash if AddSingleton
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Scoped services
builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<TaskServices>();
builder.Services.AddScoped<ReportServices>();
builder.Services.AddScoped<UserServices>();

// Controllers
builder.Services.AddControllers();

// Swagger
builder.Services.AddSwaggerWithJwt();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

// Build application
var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerWithJwt();
}

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseExceptionHandler();

app.UseCors("AllowAll");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();