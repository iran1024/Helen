namespace HelenServer.Core;

internal class ServiceOptions : IServiceOptions
{
    private readonly List<IServiceAnalyzer> _ananlyzers;

    public IServiceCollection Services { get; }

    public IEnumerable<IServiceAnalyzer> Analyzers => _ananlyzers;

    public ServiceOptions(IServiceCollection services)
    {
        _ananlyzers = new List<IServiceAnalyzer>
        {
            new ServiceFactoryAnalyzer(),
            new ServiceProviderAnalyzer(),
        };

        Services = services;
    }

    public void AddAnalyzer(IServiceAnalyzer analyzer)
    {
        _ananlyzers.Add(analyzer);
    }
}
