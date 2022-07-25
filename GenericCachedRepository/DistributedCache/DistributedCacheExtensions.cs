using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace GenericCachedRepository.DistributedCache;

internal static class DistributedCacheExtensions
{
    internal static async Task SetEntry<T>(this IDistributedCache cache, string key, T value,
        TimeSpan? absoluteExpirationTime = null, TimeSpan? slidingExpirationTime = null,
        CancellationToken token = default)
    {
        var option = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = absoluteExpirationTime ?? TimeSpan.FromMinutes(5),
            SlidingExpiration = slidingExpirationTime,
        };

        var jasonString = JsonSerializer.Serialize(value);

        await cache.SetStringAsync(key, jasonString, option, token);

    }


    internal static async Task<T> GetEntry<T>(this IDistributedCache cache, string key,
        CancellationToken token = default)
    {
        var jasonString = await cache.GetStringAsync(key, token);

        return jasonString is null ? default(T) : JsonSerializer.Deserialize<T>(jasonString);
    }

}
