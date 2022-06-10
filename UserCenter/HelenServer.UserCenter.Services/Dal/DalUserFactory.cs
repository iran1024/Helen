using HelenServer.Core;
using HelenServer.UserCenter.Contracts;
using Microsoft.Extensions.Options;

namespace HelenServer.UserCenter.Services
{
    [ServiceFactory(typeof(IDalUserService))]
    internal class DalUserFactory : DalServiceFactory<IDalUserService>
    {
        public DalUserFactory(IServiceMetadata<IDalUserService> provider, IOptionsMonitor<ConfigurationOptions> options)
            : base(provider, options.CurrentValue)
        {

        }
    }
}
