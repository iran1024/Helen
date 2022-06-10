namespace HelenServer.Core;

public interface IServiceMetadata<TService>
    where TService : class
{
    IReadOnlyDictionary<string, IReadOnlyDictionary<string, object>> Metadatas { get; }

    TService GetService(IServiceProvider provider, string name);
}
