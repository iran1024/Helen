using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Logging.Abstractions;

namespace HelenServer.Dapr.DataProtection
{
    public static class DaprDataProtectionBuilderExtensions
    {
        private const string _dataProtectionKeysName = "DataProtection-Keys";

        public static IDataProtectionBuilder PersistKeysToDaprState(this IDataProtectionBuilder builder, Func<IAdvancedDistributedCache> cacheFactory, string key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (cacheFactory == null)
            {
                throw new ArgumentNullException(nameof(cacheFactory));
            }

            return PersistKeysToDaprStateInternal(builder, key, cacheFactory);
        }

        public static IDataProtectionBuilder PersistKeysToDaprState(this IDataProtectionBuilder builder)
        {
            return PersistKeysToDaprState(builder, _dataProtectionKeysName);
        }

        public static IDataProtectionBuilder PersistKeysToDaprState(this IDataProtectionBuilder builder, string key)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return PersistKeysToDaprStateInternal(builder, key);
        }

        private static IDataProtectionBuilder PersistKeysToDaprStateInternal(IDataProtectionBuilder builder, string key, Func<IAdvancedDistributedCache>? cacheFactory = null)
        {
            builder.Services.AddSingleton<IConfigureOptions<KeyManagementOptions>>(services =>
            {
                var loggerFactory = services.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;
                var distributedCacheService = cacheFactory is not null ? cacheFactory() : services.GetRequiredService<IAdvancedDistributedCache>();

                return new ConfigureOptions<KeyManagementOptions>(options =>
                {
                    options.XmlRepository = new DaprXmlRepository(distributedCacheService, key);
                });
            });

            return builder;
        }
    }
}