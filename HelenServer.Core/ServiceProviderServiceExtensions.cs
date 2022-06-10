namespace HelenServer.Core;

public static class ServiceProviderServiceExtensions
{
    public static T GetRequiredService<T>(this IServiceProvider provider, string name)
        where T : class
    {
        var metadata = provider.GetRequiredService<IServiceMetadata<T>>();

        var service = metadata.GetService(provider, name);

        if (service is not null)
        {
            return service;
        }

        throw new NotImplementedException(name);
    }

    public static T GetService<T>(this IServiceProvider provider, string name)
        where T : class
    {
        var metadata = provider.GetRequiredService<IServiceMetadata<T>>();

        return metadata.GetService(provider, name);
    }
}
