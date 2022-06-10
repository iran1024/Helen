namespace HelenServer.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class InjectionAttribute : Attribute
{
    public InjectionAttribute() : this(null)
    {

    }

    public InjectionAttribute(
        Type serviceType,
        ServiceLifetime lifetime = ServiceLifetime.Scoped)
    {
        ServiceType = serviceType;

        Lifetime = lifetime;
    }

    public Type ServiceType { get; }

    public ServiceLifetime Lifetime { get; set; }
}