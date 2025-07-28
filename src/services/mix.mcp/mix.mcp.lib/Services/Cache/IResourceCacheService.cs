using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mix.MCP.Lib.Services.Cache
{
    /// <summary>
    /// Interface for enhanced resource caching service
    /// </summary>
    public interface IResourceCacheService
    {
        /// <summary>
        /// Gets a cached resource
        /// </summary>
        /// <typeparam name="T">Type of the cached resource</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached resource or default if not found</returns>
        T? Get<T>(string key);

        /// <summary>
        /// Gets a cached resource asynchronously
        /// </summary>
        /// <typeparam name="T">Type of the cached resource</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="factory">Factory function to create the resource if not cached</param>
        /// <param name="expiry">Cache expiry time</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Cached or newly created resource</returns>
        Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiry = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sets a cached resource
        /// </summary>
        /// <typeparam name="T">Type of the resource</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="value">Value to cache</param>
        /// <param name="expiry">Cache expiry time</param>
        void Set<T>(string key, T value, TimeSpan? expiry = null);

        /// <summary>
        /// Removes a cached resource
        /// </summary>
        /// <param name="key">Cache key</param>
        void Remove(string key);

        /// <summary>
        /// Removes cached resources by pattern
        /// </summary>
        /// <param name="pattern">Pattern to match keys</param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// Gets cache statistics
        /// </summary>
        /// <returns>Cache statistics</returns>
        CacheStatistics GetStatistics();

        /// <summary>
        /// Clears all cached resources
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Cache statistics information
    /// </summary>
    public class CacheStatistics
    {
        public int TotalKeys { get; set; }
        public long Hits { get; set; }
        public long Misses { get; set; }
        public double HitRatio => Hits + Misses == 0 ? 0 : (double)Hits / (Hits + Misses);
        public DateTime LastAccessed { get; set; }
    }

    /// <summary>
    /// Enhanced resource caching service implementation
    /// </summary>
    public class ResourceCacheService : IResourceCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly ILogger<ResourceCacheService> _logger;
        private readonly object _lockObject = new();
        private readonly HashSet<string> _cacheKeys;
        private long _hits = 0;
        private long _misses = 0;
        private DateTime _lastAccessed = DateTime.UtcNow;

        private static readonly TimeSpan DefaultExpiry = TimeSpan.FromMinutes(30);
        private const string CACHE_PREFIX = "resource_";

        public ResourceCacheService(IMemoryCache cache, ILogger<ResourceCacheService> logger)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cacheKeys = new HashSet<string>();
        }

        public T? Get<T>(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return default;

            var fullKey = GetFullKey(key);
            _lastAccessed = DateTime.UtcNow;

            if (_cache.TryGetValue(fullKey, out var value))
            {
                Interlocked.Increment(ref _hits);
                _logger.LogDebug("Cache hit for key: {Key}", key);
                return (T)value;
            }

            Interlocked.Increment(ref _misses);
            _logger.LogDebug("Cache miss for key: {Key}", key);
            return default;
        }

        public async Task<T> GetOrCreateAsync<T>(
            string key,
            Func<CancellationToken, Task<T>> factory,
            TimeSpan? expiry = null,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be null or empty", nameof(key));

            if (factory == null)
                throw new ArgumentNullException(nameof(factory));

            var fullKey = GetFullKey(key);
            _lastAccessed = DateTime.UtcNow;

            if (_cache.TryGetValue(fullKey, out var cachedValue))
            {
                Interlocked.Increment(ref _hits);
                _logger.LogDebug("Cache hit for key: {Key}", key);
                return (T)cachedValue;
            }

            Interlocked.Increment(ref _misses);
            _logger.LogDebug("Cache miss for key: {Key}, creating new value", key);

            try
            {
                var value = await factory(cancellationToken);
                Set(key, value, expiry);
                return value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating cached resource for key: {Key}", key);
                throw;
            }
        }

        public void Set<T>(string key, T value, TimeSpan? expiry = null)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var fullKey = GetFullKey(key);
            var cacheExpiry = expiry ?? DefaultExpiry;

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = cacheExpiry,
                Priority = CacheItemPriority.Normal
            };

            // Add removal callback to track keys
            options.RegisterPostEvictionCallback((k, v, reason, state) =>
            {
                lock (_lockObject)
                {
                    _cacheKeys.Remove(k.ToString()!);
                }
                _logger.LogDebug("Cache entry evicted: {Key}, Reason: {Reason}", k, reason);
            });

            _cache.Set(fullKey, value, options);

            lock (_lockObject)
            {
                _cacheKeys.Add(fullKey);
            }

            _logger.LogDebug("Cached resource with key: {Key}, Expiry: {Expiry}", key, cacheExpiry);
        }

        public void Remove(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return;

            var fullKey = GetFullKey(key);
            _cache.Remove(fullKey);

            lock (_lockObject)
            {
                _cacheKeys.Remove(fullKey);
            }

            _logger.LogDebug("Removed cached resource with key: {Key}", key);
        }

        public void RemoveByPattern(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                return;

            var keysToRemove = new List<string>();

            lock (_lockObject)
            {
                foreach (var key in _cacheKeys)
                {
                    if (key.Contains(pattern, StringComparison.OrdinalIgnoreCase))
                    {
                        keysToRemove.Add(key);
                    }
                }
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
                lock (_lockObject)
                {
                    _cacheKeys.Remove(key);
                }
            }

            _logger.LogInformation("Removed {Count} cached resources matching pattern: {Pattern}", keysToRemove.Count, pattern);
        }

        public CacheStatistics GetStatistics()
        {
            lock (_lockObject)
            {
                return new CacheStatistics
                {
                    TotalKeys = _cacheKeys.Count,
                    Hits = _hits,
                    Misses = _misses,
                    LastAccessed = _lastAccessed
                };
            }
        }

        public void Clear()
        {
            var keysToRemove = new List<string>();

            lock (_lockObject)
            {
                keysToRemove.AddRange(_cacheKeys);
                _cacheKeys.Clear();
            }

            foreach (var key in keysToRemove)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Cleared all cached resources ({Count} items)", keysToRemove.Count);
        }

        private static string GetFullKey(string key)
        {
            return $"{CACHE_PREFIX}{key}";
        }
    }
}