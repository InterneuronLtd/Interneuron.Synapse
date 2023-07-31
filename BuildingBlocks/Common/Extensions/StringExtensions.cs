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
using System.ComponentModel;
using System.Globalization;

namespace Interneuron.Common.Extensions
{
    public static class StringExtensions
	{
		/// <summary>
		/// A better(?) way of calling string.format
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string ToFormat(this string format, params object[] args)
		{
			return string.Format(format, args);
		}

		/// <summary>
		/// Converts the string to Title Case
		/// </summary>
		public static string Capitalize(this string stringValue)
		{
			return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(stringValue);
		}

		/// <summary>
		/// Performs a case-insensitive comparison of strings
		/// </summary>
		public static bool EqualsIgnoreCase(this string thisString, string otherString)
		{
			return thisString.Equals(otherString, StringComparison.InvariantCultureIgnoreCase);
		}

        /// <summary>
        /// Performs a case-insensitive contains operation
        /// </summary>
        public static bool ContainsIgnoreCase(this string thisString, string otherString)
        {
            return thisString.IndexOf(otherString, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

		/// <summary>
		/// Returns true if the string is null or String.Empty
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static bool IsEmpty(this string stringValue)
		{
		    stringValue = stringValue.Clean();
			return string.IsNullOrEmpty(stringValue);
		}

		/// <summary>
		/// Returns true if the string is not null and not String.Empty
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static bool IsNotEmpty(this string stringValue)
		{
			return !string.IsNullOrEmpty(stringValue) && stringValue.Trim().Length > 0;
		}

		/// <summary>
		/// Converts string to enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="enumString"></param>
		/// <returns></returns>
		public static T ToEnum<T>(this string enumString)
		{
			if (enumString.IsNotEmpty())
				return (T)Enum.Parse(typeof(T), enumString);
			else
				return default(T);
		}

        public static string Clean(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;
            return input.Trim();
        }

		/// <summary>
		/// Perform some action if the string is not empty
		/// </summary>
		/// <param name="stringValue"></param>
		/// <param name="action"></param>
		public static void IsNotEmpty(this string stringValue, Action<string> action)
		{
			if (stringValue.IsNotEmpty())
				action(stringValue);
		}

		/// <summary>
		/// Converts the string to bool
		/// </summary>
		/// <param name="stringValue"></param>
		/// <returns></returns>
		public static bool ToBool(this string stringValue)
		{
			if (string.IsNullOrEmpty(stringValue)) return false;

			return bool.Parse(stringValue);
		}

		public static int AsInt(this string value)
		{
			return As<int>(value);
		}

        public static short AsShort(this string value)
        {
            return As<short>(value);
        }

		public static int AsInt(this string value, int defaultValue)
		{
			return As<int>(value, defaultValue);
		}

		public static decimal AsDecimal(this string value)
		{
			return As<decimal>(value);
		}

		public static decimal AsDecimal(this string value, decimal defaultValue)
		{
			return As<decimal>(value, defaultValue);
		}

		public static float AsFloat(this string value)
		{
			return As<float>(value);
		}

		public static float AsFloat(this string value, float defaultValue)
		{
			return As<float>(value, defaultValue);
		}

		public static DateTime AsDateTime(this string value)
		{
			return As<DateTime>(value);
		}

		public static DateTime AsDateTime(this string value, DateTime defaultValue)
		{
			return As<DateTime>(value, defaultValue);
		}

		public static TValue As<TValue>(this string value)
		{
			return As<TValue>(value, default(TValue));
		}

		public static bool AsBool(this string value)
		{
			return As<bool>(value, false);
		}

		public static bool AsBool(this string value, bool defaultValue)
		{
			if (value.IsEmpty())
				return defaultValue;

			return As<bool>(value, defaultValue);
		}

		public static TValue As<TValue>(this string value, TValue defaultValue)
		{
			try
			{
				TypeConverter converter = TypeDescriptor.GetConverter(typeof(TValue));
				if (converter.CanConvertFrom(typeof(string)))
				{
					return (TValue)converter.ConvertFrom(value);
				}
				// try the other direction 
				converter = TypeDescriptor.GetConverter(typeof(string));
				if (converter.CanConvertTo(typeof(TValue)))
				{
					return (TValue)converter.ConvertTo(value, typeof(TValue));
				}
			}
			catch (Exception)
			{
				// eat all exceptions and return the defaultValue, assumption is that its always a parse/format exception
			}
			return defaultValue;
		}
	}
}
