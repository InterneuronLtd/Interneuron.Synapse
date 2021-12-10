//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.


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
