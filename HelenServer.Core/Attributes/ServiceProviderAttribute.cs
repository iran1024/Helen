using System.Resources;

namespace HelenServer.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceProviderAttribute : InjectionAttribute
{
    public ServiceProviderAttribute(
        Type serviceType,
        Type resourceType,
        string nameKey,
        string descriptionKey = "",
        ServiceLifetime lifetime = ServiceLifetime.Scoped) : base(serviceType, lifetime)
    {
        var resource = new ResourceManager(resourceType);

        Name = resource.GetString(nameKey) ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(descriptionKey))
        {
            Description = resource.GetString(descriptionKey) ?? string.Empty;
        }
        else
        {
            Description = string.Empty;
        }
    }

    public ServiceProviderAttribute(
        Type serviceType,
        string name,
        string description = "",
        ServiceLifetime lifetime = ServiceLifetime.Scoped) : base(serviceType, lifetime)
    {
        Name = name;

        Description = description;
    }

    public string Name { get; }
    public string Description { get; }
    public bool HasMetadata { get; protected set; }
}
