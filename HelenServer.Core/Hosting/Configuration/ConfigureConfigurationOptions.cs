namespace HelenServer.Core;

internal class ConfigureConfigurationOptions : IPostConfigureOptions<ConfigurationOptions>
{
    private readonly IServiceProvider _provider;

    public ConfigureConfigurationOptions(IServiceProvider provider)
    {
        _provider = provider;
    }

    public void PostConfigure(string name, ConfigurationOptions options)
    {
        if (!TryConfigure(options, name, ConfigurationOptionsProvider.Grpc))
        {
            if (!TryConfigure(options, name, ConfigurationOptionsProvider.Cache))
            {
                if (!TryConfigure(options, name, ConfigurationOptionsProvider.Database))
                {

                }
            }
        }
    }

    private bool TryConfigure(ConfigurationOptions options, string name, string providerName)
    {
        var provider = _provider.GetService<ConfigurationOptionsProvider>(providerName);

        return provider is not null && provider.TryConfigure(name, options);
    }
}
