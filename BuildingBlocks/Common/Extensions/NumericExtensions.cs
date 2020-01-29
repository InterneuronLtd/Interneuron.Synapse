using System;

namespace Interneuron.Common.Extensions
{
    public static class NumericExtensions
    {
        public static int Ceil(this double val)
        {
            return Convert.ToInt32(Math.Ceiling(val));
        }

		public static T ToEnum<T>(this int enumVal)
		{
			if (Enum.IsDefined(typeof(T), enumVal))
				return (T)Enum.ToObject(typeof(T), enumVal);
			
			return default(T);
		}

        public static T ToEnum<T>(this short enumVal)
        {
            if (Enum.IsDefined(typeof(T), enumVal))
                return (T)Enum.ToObject(typeof(T), enumVal);

            return default(T);
        }
        
        public static TDestination? ConvertTo<TSource, TDestination>(this TSource? input) where TSource:struct where TDestination:struct
        {
            if (!input.HasValue)
                return null;

            return (TDestination)Convert.ChangeType(input, typeof (TDestination));
        }

        public static TDestination ConvertTo<TDestination>(this object input)
            where TDestination : struct
        {
            return (TDestination)Convert.ChangeType(input, typeof(TDestination));
        } 
    }
}
