namespace HelenServer.Core;

internal class ServiceProviderAnalyzer : ServiceAnalyzer<ServiceProviderAttribute>
{
    private readonly Dictionary<Type, ServiceMetadata> _maps;
    public ServiceProviderAnalyzer()
    {
        _maps = new Dictionary<Type, ServiceMetadata>();
    }

    protected override bool TryAnalyze(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        ServiceProviderAttribute attribute)
    {
        if (attribute != null)
        {
            switch (attribute.Lifetime)
            {
                case ServiceLifetime.Scoped:
                    {
                        services.AddScoped(implementationType);
                    }

                    break;
                case ServiceLifetime.Singleton:
                    {
                        services.AddSingleton(implementationType);
                    }

                    break;
                case ServiceLifetime.Transient:
                    {
                        services.AddTransient(implementationType);
                    }

                    break;
            }

            if (!_maps.TryGetValue(serviceType, out var value))
            {
                var type = typeof(ServiceMetadata<>);

                type = type.MakeGenericType(serviceType);

                value = (ServiceMetadata)Activator.CreateInstance(type)!;

                _maps.Add(serviceType, value);

                var stype = typeof(IServiceMetadata<>);

                stype = stype.MakeGenericType(serviceType);

                services.AddSingleton(stype, value);
            }

            value.Add(implementationType, attribute);

            return true;
        }

        return false;
    }
}
