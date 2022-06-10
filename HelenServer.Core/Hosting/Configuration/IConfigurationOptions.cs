namespace HelenServer.Core;

public interface IConfigurationOptions
{
    string ConnectionName { get; }

    string? ConnectionString { get; }

    bool IsCacheEnabled { get; }
}
