using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using Microsoft.Extensions.Options;

namespace HelenServer.BugEngine.Services
{
    [ServiceFactory(typeof(IDalProductService))]
    internal class DalProductFactory : DalServiceFactory<IDalProductService>
    {
        public DalProductFactory(
            IServiceMetadata<IDalProductService> provider,
            IOptionsMonitor<ConfigurationOptions> opts)
                : base(provider, opts.CurrentValue)
        {

        }
    }
}