namespace HelenServer.Core;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class ServiceFactoryAttribute : InjectionAttribute
{
    public ServiceFactoryAttribute(Type serviceType, ServiceLifetime lifetime = ServiceLifetime.Scoped)
        : base(serviceType, lifetime)
    {

    }
}
