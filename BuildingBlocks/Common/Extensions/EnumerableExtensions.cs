using System;
using System.Collections.Generic;
using System.Linq;

namespace Interneuron.Common.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if(source!=null)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
            return source;
        }

		public static bool IsCollectionValid<T>(this ICollection<T> source)
		{
			return source != null && source.Count > 0;
		}

        public static bool IsCollectionValid<T>(this IEnumerable<T> source)
        {
            return source != null && source.Any();
        }

        public static IEnumerable<T> Distinct<T,TK>(this IEnumerable<T> source, Func<T,TK> keySelector)
        {
            var dict = new HashSet<TK>();

            foreach (var item in source)
            {
                var key = keySelector(item);

                if(!dict.Contains(key))
                {
                    dict.Add(key);
                    yield return item;
                }
            }
        } 

        public static List<T> ToListOrDefault<T>(this IEnumerable<T> source)
        {
            return (source != null && source.Any()) ? source.ToList() : null;
        }
    }
}
