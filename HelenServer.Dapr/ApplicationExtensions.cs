using HelenServer.Dapr;

namespace Microsoft.AspNetCore.Builder;

public static class ApplicationExtensions
{
    public static WebApplication UseCloudDaprService(this WebApplication app)
    {
        app.MapGrpcService<DaprGrpcService>();

        return app;
    }
}