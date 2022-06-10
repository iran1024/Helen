namespace HelenServer.Core;

internal class ServiceFactoryAnalyzer : ServiceAnalyzer<ServiceFactoryAttribute>
{
    protected override bool TryAnalyze(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceFactoryAttribute attribute)
    {
        if (attribute != null)
        {
            switch (attribute.Lifetime)
            {
                case ServiceLifetime.Scoped:
                    {
                        services.AddScoped(implementationType);

                        services.AddScoped(
                            serviceType,
                            m => GetRealService(m, implementationType));
                    }

                    break;
                case ServiceLifetime.Singleton:
                    {
                        services.AddSingleton(implementationType);

                        services.AddSingleton(
                            serviceType,
                            m => GetRealService(m, implementationType));
                    }

                    break;
                case ServiceLifetime.Transient:
                    {
                        services.AddTransient(implementationType);

                        services.AddTransient(
                            serviceType,
                            m => GetRealService(m, implementationType));
                    }

                    break;
            }

            return true;
        }

        return false;
    }

    private object GetRealService(IServiceProvider provider, Type implementationType)
    {
        object? obj = provider.GetService(implementationType);

        if (obj is IServiceFactory factory)
        {
            return factory.GetService(provider);
        }

        return obj;
    }
}
