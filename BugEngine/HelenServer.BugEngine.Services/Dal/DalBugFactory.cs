using HelenServer.BugEngine.Contracts;
using HelenServer.Core;
using Microsoft.Extensions.Options;

namespace HelenServer.BugEngine.Services
{
    [ServiceFactory(typeof(IDalBugService))]
    internal class DalBugFactory : DalServiceFactory<IDalBugService>
    {
        public DalBugFactory(
            IServiceMetadata<IDalBugService> provider,
            IOptionsMonitor<ConfigurationOptions> opts)
                : base(provider, opts.CurrentValue)
        {

        }
    }
}