using Microsoft.Extensions.Caching.Memory;

namespace HelenServer.Dapr
{
    public class DaprDistributedCacheOptions : MemoryCacheOptions
    {
        public const string Position = "Dapr:Cache";

        public string StoreName { get; set; } = string.Empty;
    }
}