namespace HelenServer.Core;

public interface IServiceOptions
{
    IServiceCollection Services { get; }

    IEnumerable<IServiceAnalyzer> Analyzers { get; }

    void AddAnalyzer(IServiceAnalyzer analyzer);
}
