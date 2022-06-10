using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace HelenServer.Core;

public static class ApplicationExtensions
{
    public static WebApplication UseCloudService(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {

        }

        app.UseExceptionMiddleware();

        app.UseForwardedHeaders();

        app.MapCloudHealthChecks();

        return app;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app)
    {
        return app.UseMiddleware<HttpExceptionMiddleware>();
    }

    public static IEndpointRouteBuilder MapCloudHealthChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/health");

        endpoints.MapHealthChecks("/health-liveness", new HealthCheckOptions()
        {
            Predicate = m => m.Tags.Contains("liveness")
        });

        endpoints.MapHealthChecks("/health-readiness", new HealthCheckOptions()
        {
            Predicate = m => m.Tags.Contains("readiness")
        });

        return endpoints;
    }
}