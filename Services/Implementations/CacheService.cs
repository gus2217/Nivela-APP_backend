using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NivelaService.Services.Interface;
using System.Text.Json;

namespace NivelaService.Services.Implementations
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache? _distributedCache;
        private readonly IMemoryCache? _memoryCache;
        private readonly ILogger<CacheService> _logger;
        private readonly bool _useDistributedCache;

        public CacheService(IServiceProvider serviceProvider, ILogger<CacheService> logger)
        {
            _logger = logger;
            
            // Try to get distributed cache (Redis), fallback to memory cache
            _distributedCache = serviceProvider.GetService<IDistributedCache>();
            _memoryCache = serviceProvider.GetService<IMemoryCache>();
            _useDistributedCache = _distributedCache != null;

            if (_useDistributedCache)
            {
                _logger.LogInformation("Using distributed cache (Redis)");
            }
            else
            {
                _logger.LogInformation("Using in-memory cache");
            }
        }

        public async Task<T?> GetAsync<T>(string key) where T : class
        {
            try
            {
                if (_useDistributedCache)
                {
                    var cachedValue = await _distributedCache!.GetStringAsync(key);
                    return cachedValue == null ? null : JsonSerializer.Deserialize<T>(cachedValue);
                }
                else
                {
                    return _memoryCache!.Get<T>(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cached item with key: {Key}", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null) where T : class
        {
            try
            {
                var defaultExpiration = TimeSpan.FromMinutes(30);
                var cacheExpiration = expiration ?? defaultExpiration;

                if (_useDistributedCache)
                {
                    var serializedValue = JsonSerializer.Serialize(value);
                    var options = new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cacheExpiration
                    };
                    await _distributedCache!.SetStringAsync(key, serializedValue, options);
                }
                else
                {
                    var options = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = cacheExpiration
                    };
                    _memoryCache!.Set(key, value, options);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cached item with key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                if (_useDistributedCache)
                {
                    await _distributedCache!.RemoveAsync(key);
                }
                else
                {
                    _memoryCache!.Remove(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cached item with key: {Key}", key);
            }
        }

        public async Task RemovePatternAsync(string pattern)
        {
            try
            {
                if (_useDistributedCache)
                {
                    // For Redis, you'd need to implement pattern-based deletion
                    // This is a simplified version - in production, consider using Redis SCAN
                    _logger.LogWarning("Pattern-based cache removal not fully implemented for Redis");
                }
                else
                {
                    // Memory cache doesn't support pattern removal easily
                    _logger.LogWarning("Pattern-based cache removal not supported for MemoryCache");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cached items with pattern: {Pattern}", pattern);
            }
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> getItem, TimeSpan? expiration = null) where T : class
        {
            var cachedValue = await GetAsync<T>(key);
            if (cachedValue != null)
            {
                return cachedValue;
            }

            var item = await getItem();
            await SetAsync(key, item, expiration);
            return item;
        }
    }
}