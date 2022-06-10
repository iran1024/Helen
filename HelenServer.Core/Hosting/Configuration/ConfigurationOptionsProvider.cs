namespace HelenServer.Core;

public abstract class ConfigurationOptionsProvider
{
    public const string Grpc = nameof(Grpc);
    public const string Cache = nameof(Cache);
    public const string Database = nameof(Database);

    public abstract bool TryConfigure(string name, ConfigurationOptions options);
}
