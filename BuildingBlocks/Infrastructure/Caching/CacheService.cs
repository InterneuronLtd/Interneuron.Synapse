using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Text;

namespace Interneuron.Caching
{
    public class CacheService
    {
        static readonly Lazy<ICacheProvider> CacheProvider = new Lazy<ICacheProvider>(GetCacheProvider, true);

        public static Func<ICacheProvider> CacheProviderDelegate;

        private static TimeSpan defaultCacheDuration = TimeSpan.MinValue;

        static TimeSpan DefaultCacheDuration
        {
            get
            {
                if(defaultCacheDuration == TimeSpan.MinValue)
                {
                    var cacheMinutes = CacheSettings.Instance.CacheDurationInMinutes;
                    defaultCacheDuration = TimeSpan.FromMinutes(cacheMinutes);
                }
                return defaultCacheDuration;
            }
        }

        static ICacheProvider Cache { get { return CacheProvider.Value; } }

        static CacheService()
        {
            CacheProviderDelegate = BuildCacheProvider;
        }

        static ICacheProvider GetCacheProvider()
        {
            return CacheProviderDelegate();
        }

        private static ICacheProvider BuildCacheProvider()
        {
            var cacheEnabled = CacheSettings.Instance.Enabled;

            if(cacheEnabled)
            {
                var cacheProviderType = CacheSettings.Instance.Provider;

                if (!string.IsNullOrWhiteSpace(cacheProviderType))
                {
                    var type = Type.GetType(cacheProviderType); 
                    
                    if(type == null)
                        throw new Exception(string.Format("Unable to find caching provider of type : {0}",cacheProviderType));

                    return BuildCacheProvider(type);
                }

                return new InMemoryCacheProvider();
            }

            return new NullCacheProvider();
        }

        private static ICacheProvider BuildCacheProvider(Type provider)
        {
            var obj = Activator.CreateInstance(provider) as ICacheProvider;
            if (obj == null)
            {
                throw new InvalidOperationException("Unable to create cache provider from type " + provider.FullName);
            }
            return obj;
        }

        public static void Set(string key, object value)
        {
            Cache.Set(key,value,DefaultCacheDuration);
        }

        public static void Set(string key, object value, TimeSpan cacheDuration)
        {
            Cache.Set(key, value, cacheDuration);
        }

        public static T Get<T>(string key)
        {
            return Cache.Get<T>(key);
        }

        public static bool Remove(string key)
        {
            return Cache.Remove(key);
        }

        public static void FlushAll()
        {
            Cache.FlushAll();
        }
    }
}
