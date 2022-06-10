using HelenServer.FileStorage.Contracts;

namespace HelenServer.FileStorage.Services
{
    [ServiceFactory(typeof(IDalFileStorageService))]
    public class DalFileStorageFactory : DalServiceFactory<IDalFileStorageService>
    {
        public DalFileStorageFactory(
        IServiceMetadata<IDalFileStorageService> provider,
        IOptionsMonitor<ConfigurationOptions> options)
        : base(provider, options.CurrentValue)
        {

        }
    }
}