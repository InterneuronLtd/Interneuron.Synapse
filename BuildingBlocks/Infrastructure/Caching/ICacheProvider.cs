using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Interneuron.Caching
{
    public interface ICacheProvider
    {
        void Set(string key, object value, TimeSpan cacheDuration);
        T Get<T>(string key);
        bool Remove(string key);
        void FlushAll();
    }
}
