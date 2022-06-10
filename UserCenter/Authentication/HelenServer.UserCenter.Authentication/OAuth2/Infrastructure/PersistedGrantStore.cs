using HelenServer.Core;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace HelenServer.Authentication.OAuth2
{
    public class PersistedGrantStore : IPersistedGrantStore
    {
        private readonly ILogger<PersistedGrantStore> _logger;
        private readonly IAdvancedDistributedCache _cache;

        protected ISystemClock _clock;

        public PersistedGrantStore(
        IAdvancedDistributedCache cache,
        ILogger<PersistedGrantStore> logger,
        ISystemClock clock)
        {
            _cache = cache;
            _logger = logger;
            _clock = clock;
        }
        public async Task StoreAsync(PersistedGrant grant)
        {
            try
            {
                if (grant.Type == "refresh_token")
                {
                    var expirationTimeSpan = (TimeSpan)(grant.Expiration! - _clock.UtcNow);

                    await _cache.SetAsync(grant.Key, grant, new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = expirationTimeSpan
                    }, default);
                }
                else
                {
                    await _cache.SetAsync(grant.Key, grant, new DistributedCacheEntryOptions(), default);
                }
                _logger.LogDebug("尝试在数据库中保存或更新 {persistedGrantKey}", grant.Key);
            }
            catch (Exception ex)
            {
                _logger.LogError(0, ex, "存储持久授权数据时发生异常");
                throw;
            }
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            throw new NotImplementedException();
        }

        public async Task<PersistedGrant> GetAsync(string key)
        {
            var model = await _cache.GetAsync<PersistedGrant>(key, default);

            _logger.LogDebug("Key：{persistedGrantKey} 是否在数据库中找到数据：{persistedGrantKeyFound}", key, model != null);

            return model;
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task RemoveAsync(string key)
        {
            _logger.LogDebug("从数据库中移除持久存储的Key：{persistedGrantKey}", key);

            return _cache.RemoveAsync(key);
        }
    }
}