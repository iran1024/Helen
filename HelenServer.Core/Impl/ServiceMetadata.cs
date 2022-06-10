namespace HelenServer.Core;

internal abstract class ServiceMetadata
{
    protected struct ServiceMetadataDescriptor
    {
        public ServiceMetadataDescriptor(
            Type serviceType, ServiceProviderAttribute attribute)
        {
            ServiceType = serviceType;

            var metadata = new Dictionary<string, object>
                {
                    { nameof(attribute.Description), attribute.Description }
                };

            if (attribute.HasMetadata)
            {
                var type = attribute.GetType();

                var properties = type.GetProperties(
                    BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty | BindingFlags.SetProperty);

                foreach (var property in properties)
                {
                    metadata.Add(
                        property.Name, property.GetValue(attribute));
                }
            }

            Metadata = metadata;
        }

        public Type ServiceType { get; }
        public IReadOnlyDictionary<string, object> Metadata { get; }
    }

    protected readonly IDictionary<string, ServiceMetadataDescriptor> _maps;
    public ServiceMetadata()
    {
        _maps = new Dictionary<string, ServiceMetadataDescriptor>(StringComparer.OrdinalIgnoreCase);
    }

    public void Add(Type serviceType, ServiceProviderAttribute attribute)
    {
        _maps[attribute.Name] = new ServiceMetadataDescriptor(serviceType, attribute);
    }

    public object GetService(IServiceProvider provider, string name)
    {
        if (_maps.TryGetValue(name, out var metadata))
        {
            return provider.GetService(metadata.ServiceType);
        }

        return default;
    }
}

internal class ServiceMetadata<TService>
    : ServiceMetadata, IServiceMetadata<TService> where TService : class
{
    public IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> Metadatas
    {
        get
        {
            var result = new Dictionary<string, IReadOnlyDictionary<string, object>>();

            foreach (string? key in _maps.Keys)
            {
                var d = _maps[key];

                result.Add(key, d.Metadata);
            }

            return result;
        }
    }

    TService IServiceMetadata<TService>.GetService(IServiceProvider provider, string name)
    {
        return (TService)GetService(provider, name);
    }
}
