using System;
using System.Runtime.Caching;

namespace Interneuron.Caching
{
    public class InMemoryCacheProvider : ICacheProvider
    {
        static MemoryCache cacheInstance = CreateMemoryCache();

        private static MemoryCache CreateMemoryCache()
        {
            return new MemoryCache(AppDomain.CurrentDomain.FriendlyName);
        }

        public void Set(string key, object value, TimeSpan cacheDuration)
        {
            cacheInstance.Set(key, value, DateTimeOffset.Now.Add(cacheDuration));
        }

        public T Get<T>(string key)
        {
            object val = cacheInstance.Get(key);
            return val == null ? default(T) : (T)val;
        }

        public bool Remove(string key)
        {
            return cacheInstance.Remove(key) != null;
        }

        public void FlushAll()
        {
            cacheInstance.Dispose();
            cacheInstance = CreateMemoryCache();
        }
    }
}
