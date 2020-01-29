using System;

namespace Interneuron.Caching
{
    public class NullCacheProvider : ICacheProvider
    {
        public void Set(string key, object value, TimeSpan cacheDuration)
        {

        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public bool Remove(string key)
        {
            return false;
        }

        public void FlushAll()
        {

        }
    }
}
