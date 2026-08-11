using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Task_managment_system.Extentions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerWithJwt(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            // Swagger document
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Task Management API",
                Version = "v1",
                Description = "Task Management System API"
            });

            // JWT Bearer authentication
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "Enter ONLY the raw JWT token, without the 'Bearer ' prefix " +
                    "(Swagger adds it automatically).\n\n" +
                    "Example: eyJhbGciOiJIUzI1NiIs...",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT"
            });

            // Apply JWT authentication to Swagger endpoints
            options.AddSecurityRequirement(document =>
                new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            "Bearer",
                            document)
                    ] = []
                });
        });

        return services;
    }

    public static WebApplication UseSwaggerWithJwt(
        this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Task Management API v1");
            options.RoutePrefix = "swagger";
        });

        return app;
    }
}