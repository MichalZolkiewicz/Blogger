using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Application.Services;

public class ResponseCacheService : IResponseCacheService
{
    private readonly IDistributedCache _distributedCache;

    public ResponseCacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }
    public async Task CacheResponseAsync(string cacheKey, object response, TimeSpan timelive)
    {
        if(response == null)
        {
            return;
        }

        var serializedResponse = JsonConvert.SerializeObject(response);

        await _distributedCache.SetStringAsync(cacheKey, serializedResponse, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = timelive
        });
    }

    public async Task<string> GetCacheResponseAsync(string cacheKey)
    {
        var cachedResponse = await _distributedCache.GetStringAsync(cacheKey);

        return string.IsNullOrEmpty(cachedResponse) ? null : cachedResponse;
    }
}
