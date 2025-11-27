using Microsoft.Extensions.Caching.Memory;

namespace Mix.Service.Interfaces
{
    public interface IMixMemoryCacheService
    {
        public ICacheEntry CreateEntry(object key);

        public void Remove(object key);

        public Task<T?> TryGetValueAsync<T>(object key, Func<ICacheEntry, Task<T>> factory);
    }
}
