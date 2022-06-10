namespace HelenServer.Core;

public abstract class ServiceAnalyzer<TAttribute> : IServiceAnalyzer where TAttribute : InjectionAttribute
{
    public virtual bool TryAnalyze(
        IServiceCollection services,
        Type type,
        InjectionAttribute attribute)
    {
        var serviceType = type;

        if (attribute != null)
        {
            if (attribute.ServiceType != null)
            {
                serviceType = attribute.ServiceType;
            }

            if (attribute is TAttribute attr)
            {
                return TryAnalyze(
                    services, serviceType, type, attr);
            }
        }
        else
        {
            return TryAnalyze(
                services, serviceType, type, default);
        }

        return false;
    }

    protected abstract bool TryAnalyze(
        IServiceCollection services,
        Type serviceType,
        Type implementationType,
        TAttribute attribute);
}
