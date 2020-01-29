using System.Collections.Generic;

namespace Interneuron.Common.Extensions
{
    public static class DictionaryExtensions
    {
        public static TValue SafeGet<TKey,TValue>(this IDictionary<TKey,TValue> dict, TKey key, TValue defaultValue = default(TValue))
        {
            TValue val = defaultValue;
            if(dict.IsCollectionValid() && key != null)
            {
                dict.TryGetValue(key, out val);
            }
            return val;
        }
    }
}
