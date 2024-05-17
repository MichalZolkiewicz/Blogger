namespace Application.Interfaces;

public interface IResponseCacheService
{
    Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeleave);

    Task<string> GetCacheResponseAsync(string cacheKey);
}
