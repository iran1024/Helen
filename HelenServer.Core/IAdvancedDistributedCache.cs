using Microsoft.Extensions.Caching.Distributed;

namespace HelenServer.Core;

public interface IAdvancedDistributedCache : IDistributedCache
{
    TValue? Get<TValue>(string key);
    Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default);
    void Set<TValue>(string key, TValue value, DistributedCacheEntryOptions? options = null);
    Task SetAsync<TValue>(string key, TValue value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default);
    Task<TValue> GetOrCreateAsync<TValue>(string key, Func<string, CancellationToken, Task<TValue>> factory, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default);
}