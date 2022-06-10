using Dapr.Client;
using Microsoft.Extensions.Caching.Distributed;

namespace HelenServer.Dapr;

internal class DaprDistributedCache : IAdvancedDistributedCache
{
    internal readonly DaprClient _client;
    internal readonly DaprDistributedCacheOptions _options;

    private readonly SemaphoreSlim _lock = new(1, 1);

    private bool IsFirst { get; set; } = true;

    public DaprDistributedCache(DaprClient client, IOptions<DaprDistributedCacheOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public byte[] Get(string key)
    {
        var waitTask = _client.WaitForSidecarAsync();

        Task.WaitAll(waitTask);

        var task = _client.GetStateAsync<byte[]>(_options.StoreName, key);

        return task.Result;
    }

    public TValue? Get<TValue>(string key)
    {
        this.WaitCacheService().Wait();

        var task = this.GetAsync<TValue?>(key, default);

        return task.Result;
    }

    public async Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        await this.WaitCacheService();

        return await _client.GetStateAsync<byte[]>(_options.StoreName, key, cancellationToken: token);
    }

    public async Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default)
    {
        await this.WaitCacheService();

        return await _client.GetStateAsync<TValue?>(_options.StoreName, key, null, cancellationToken: cancellationToken);
    }

    public async Task<TValue> GetOrCreateAsync<TValue>(
        string key, Func<string, CancellationToken, Task<TValue>> factory,
        DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        var value = await this.GetAsync<TValue>(key, cancellationToken);

        if (value is null)
        {
            value = await factory(key, cancellationToken);

            await this.SetAsync(key, value, options, cancellationToken);

            return value;
        }

        return value;
    }

    [Obsolete("Dapr do not support refresh.")]
    public void Refresh(string key)
    {
        throw new NotImplementedException();
    }

    [Obsolete("Dapr do not support refresh.")]
    public Task RefreshAsync(string key, CancellationToken token = default)
    {
        throw new NotImplementedException();
    }

    public void Remove(string key)
    {
        this.WaitCacheService().Wait();

        _client.DeleteStateAsync(_options.StoreName, key).Wait();
    }

    public async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await this.WaitCacheService();

        await _client.DeleteStateAsync(_options.StoreName, key, cancellationToken: token);
    }

    public void Set<TValue>(string key, TValue value, DistributedCacheEntryOptions? options = null)
    {
        this.WaitCacheService().Wait();

        this.SetAsync(key, value, options).Wait();
    }

    public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        this.WaitCacheService().Wait();

        var metadata = new Dictionary<string, string>();

        if (options.AbsoluteExpirationRelativeToNow != null)
        {
            double uniTimeStamp = ((TimeSpan)options.AbsoluteExpirationRelativeToNow).TotalSeconds;

            metadata.Add("ttlInSeconds", uniTimeStamp.ToString());
        }

        _client.SaveStateAsync(_options.StoreName, key, value, metadata: metadata).Wait();
    }

    public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions? options = null, CancellationToken token = default)
    {
        await this.WaitCacheService();

        var metadata = new Dictionary<string, string>();

        if (options?.AbsoluteExpiration != null)
        {
            double uniTimeStamp = (options.AbsoluteExpiration.Value - DateTimeOffset.UtcNow).TotalSeconds;

            metadata.Add("ttlInSeconds", uniTimeStamp.ToString());
        }
        else if (options?.AbsoluteExpirationRelativeToNow != null)
        {
            double uniTimeStamp = options.AbsoluteExpirationRelativeToNow.Value.TotalSeconds;

            metadata.Add("ttlInSeconds", uniTimeStamp.ToString());
        }
        else if (options?.SlidingExpiration != null)
        {

        }

        await _client.SaveStateAsync(_options.StoreName, key, value, metadata: metadata, cancellationToken: token);
    }

    public async Task SetAsync<TValue>(
        string key, TValue value, DistributedCacheEntryOptions? options = null, CancellationToken cancellationToken = default)
    {
        await this.WaitCacheService();

        var metadata = new Dictionary<string, string>();

        if (options?.AbsoluteExpiration != null)
        {
            double uniTimeStamp = (options.AbsoluteExpiration.Value - DateTimeOffset.UtcNow).TotalSeconds;

            metadata.Add("ttlInSeconds", uniTimeStamp.ToString());
        }
        else if (options?.AbsoluteExpirationRelativeToNow != null)
        {
            double uniTimeStamp = options.AbsoluteExpirationRelativeToNow.Value.TotalSeconds;

            metadata.Add("ttlInSeconds", uniTimeStamp.ToString());
        }
        else if (options?.SlidingExpiration != null)
        {

        }

        await _client.SaveStateAsync(_options.StoreName, key, value, null, metadata, cancellationToken);
    }

    private async Task WaitCacheService()
    {
        if (IsFirst)
        {
            await _lock.WaitAsync();

            if (IsFirst)
            {
                try
                {
                    await _client.WaitForSidecarAsync();

                    IsFirst = false;
                }
                finally
                {
                    _lock.Release();
                }
            }
        }
    }
}