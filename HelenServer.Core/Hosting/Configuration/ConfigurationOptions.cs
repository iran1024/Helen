namespace HelenServer.Core;

public class ConfigurationOptions : IConfigurationOptions
{
    public string ConnectionName { get; set; } = "Microsoft SQL Server";

    public string? ConnectionString { get; set; }

    public bool IsCacheEnabled { get; set; }
}
