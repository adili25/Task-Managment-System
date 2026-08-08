using Task_Manager.Models;
using Task_managment_system.Repositries;
using Task_managment_system.Services;

var builder = WebApplication.CreateBuilder();

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