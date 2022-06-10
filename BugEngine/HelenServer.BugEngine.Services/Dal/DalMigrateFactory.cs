using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using Microsoft.Extensions.Options;

namespace HelenServer.BugEngine.Services
{
    [ServiceFactory(typeof(IDalMigrateService))]
    internal class DalMigrateFactory : DalServiceFactory<IDalMigrateService>
    {
        public DalMigrateFactory(
            IServiceMetadata<IDalMigrateService> provider,
            IOptionsMonitor<ConfigurationOptions> opts)
                : base(provider, opts.CurrentValue)
        {

        }
    }
}