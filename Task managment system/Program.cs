using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Task_managment_system.Repositries;
using Task_managment_system.Services;

var builder = WebApplication.CreateBuilder();

builder.Services.AddAuthentication(options =>
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
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"])) ?? throw new ArgumentNullException("missing the Secret key in config file")
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", Policy =>
    {
        Policy.RequireRole("Admin");
    });
});


//singleton DI lifetime so the repo hold data through the whole app lifetime
builder.Services.AddSingleton<UserRepository>();
builder.Services.AddSingleton<TaskRepository>();

builder.Services.AddScoped<AuthServices>();
builder.Services.AddScoped<TaskServices>();
builder.Services.AddScoped<ReportServices>();
builder.Services.AddScoped<UserServices>();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

/*
---THE ORDER OF MIDDLEWARE PIPELINE---

1- Development Tools (Swagger): You configure the UI documentation to only load if the application is in a development environment.
2- HTTPS Redirection: You force any unencrypted HTTP traffic to securely bounce to an encrypted HTTPS port.
3- Authentication (Who are you?): The application intercepts the request, reads the JWT from the HTTP headers, validates the signature, and extracts the claims.
4- Authorization (What are you allowed to do?): Now that the app knows who the user is, it checks if they have the specific Roles or Policies required to move forward.
5- Controller Mapping: The secure, validated request is finally routed to the correct endpoint in your TaskController or ReportsController.
*/

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();




