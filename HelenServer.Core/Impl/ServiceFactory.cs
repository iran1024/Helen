namespace HelenServer.Core;

public abstract class ServiceFactory<TService> : IServiceFactory
    where TService : class
{
    protected ServiceFactory(IServiceMetadata<TService> provider)
    {
        Provider = provider;
    }

    public IServiceMetadata<TService> Provider { get; }

    public abstract object GetService(IServiceProvider provider);
}

public abstract class DalServiceFactory<TService> : ServiceFactory<TService> where TService : class
{
    private readonly IConfigurationOptions _options;
    protected DalServiceFactory(
        IServiceMetadata<TService> provider,
        IConfigurationOptions options) : base(provider)
    {
        _options = options;
    }

    public override object GetService(IServiceProvider provider)
    {
        return Provider.GetService(provider, _options.ConnectionName);
    }
}
