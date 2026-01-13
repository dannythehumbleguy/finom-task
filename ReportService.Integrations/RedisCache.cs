using System;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using ReportService.BusinessLogic.Abstractions;

namespace ReportService.Integrations;

/// <summary>
/// Реализация кэша на основе Redis через IDistributedCache.
/// </summary>
public class RedisCache(IDistributedCache distributedCache) : ICache
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public async Task<T> GetAsync<T>(string key)
    {
        var cachedValue = await distributedCache.GetStringAsync(key);
        
        if (string.IsNullOrEmpty(cachedValue))
        {
            return default(T);
        }

        try
        {
            return JsonSerializer.Deserialize<T>(cachedValue, JsonOptions);
        }
        catch
        {
            return default(T);
        }
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
    {
        var serializedValue = JsonSerializer.Serialize(value, JsonOptions);
        var options = new DistributedCacheEntryOptions();

        if (expiration.HasValue)
        {
            options.AbsoluteExpirationRelativeToNow = expiration;
        }

        await distributedCache.SetStringAsync(key, serializedValue, options);
    }

    public async Task RemoveAsync(string key)
    {
        await distributedCache.RemoveAsync(key);
    }
}
