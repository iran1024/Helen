using HelenServer.Dapr;

namespace Microsoft.AspNetCore.Builder;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCloudDaprService(this WebApplicationBuilder builder)
    {
        builder.ConfigureDaprService();

        builder.Services.AddCloudDaprService();

        return builder;
    }

    public static IServiceCollection AddCloudDaprService(this IServiceCollection services)
    {
        services.AddGrpc();
        services.AddDaprClient();

        services.AddSingleton<IGrpcClientFactory, DaprGrpcClientFactory>();
        services.AddSingleton<IAdvancedDistributedCache, DaprDistributedCache>();

        services.AddSingleton<IEventMessageConverter, DaprEventMessageConverter>();
        services.AddSingleton<IEventPublisher, DaprEventPublisher>();

        return services;
    }

    private static WebApplicationBuilder ConfigureDaprService(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<DaprEventBusOptions>(
           builder.Configuration.GetSection(DaprEventBusOptions.Position));

        builder.Services.Configure<DaprDistributedCacheOptions>(
            builder.Configuration.GetSection(DaprDistributedCacheOptions.Position));

        return builder;
    }
}