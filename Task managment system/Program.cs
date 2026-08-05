using Task_Manager.Models;
using Task_managment_system.Repositries;

var builder = WebApplication.CreateBuilder();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<TaskRepository>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", Policy =>
            {
                Policy.RequireRole("Admin");
            });
});

var app = builder.Build();
app.MapControllers();
app.Run();