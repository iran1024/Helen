using Microsoft.Extensions.Hosting;

namespace HelenServer.Core
{
    public static class ServiceCollectionExtensions
    {
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();

            if (hostBuilderContext?.Configuration is not null)
            {
                return (IConfigurationRoot)hostBuilderContext.Configuration;
            }

            return services.GetSingletonInstance<IConfiguration>();
        }

        public static IHostEnvironment GetHostEnvironment(this IServiceCollection services)
        {
            var hostBuilderContext = services.GetSingletonInstanceOrNull<HostBuilderContext>();

            if (hostBuilderContext?.HostingEnvironment is not null)
            {
                return hostBuilderContext.HostingEnvironment;
            }

            return services.GetSingletonInstance<IHostEnvironment>();
        }

        public static T? GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T?)services.FirstOrDefault(n => n.ServiceType == typeof(T))?.ImplementationInstance;
        }

        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstanceOrNull<T>();

            if (service is null)
            {
                throw new InvalidOperationException("无法找到单例服务: " + typeof(T).AssemblyQualifiedName);
            }

            return service;
        }
    }
}
