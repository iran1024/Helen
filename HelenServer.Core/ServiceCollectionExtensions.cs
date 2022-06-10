using System.Runtime.Loader;

namespace Microsoft.Extensions.DependencyInjection;

public static partial class ServiceCollectionExtensions
{
    public static IServiceCollection AddCloudServiceCore(
        this IServiceCollection services, Action<IServiceOptions>? optionsAction = null)
    {
        var options = new ServiceOptions(services);

        optionsAction?.Invoke(options);

        var definedTypes = Directory
            .GetFiles(AppContext.BaseDirectory, $"HelenServer.*.dll")
            .SelectMany(x => AssemblyLoadContext.Default.LoadFromAssemblyPath(x).DefinedTypes);

        foreach (var definedType in definedTypes)
        {
            if (definedType.IsClass &&
                !definedType.IsAbstract &&
                definedType.GetCustomAttribute<InjectionAttribute>() is InjectionAttribute attribute &&
                !options.Analyzers.Any(x => x.TryAnalyze(services, definedType, attribute)))
            {
                var serviceType = attribute.ServiceType ?? definedType;

                _ = attribute.Lifetime switch
                {
                    ServiceLifetime.Singleton => services.AddScoped(serviceType, definedType),
                    ServiceLifetime.Transient => services.AddTransient(serviceType, definedType),
                    _ => services.AddScoped(serviceType, definedType),
                };
            }
        }

        return services;
    }
}
