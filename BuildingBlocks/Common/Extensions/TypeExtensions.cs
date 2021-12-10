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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Interneuron.Common.Extensions
{
    public static class TypeExtensions
    {
        public static bool IsString(this Type type)
        {
            return type == typeof(string);
        }

        public static bool IsPrimitive(this Type type)
        {
            return type.IsPrimitive && !IsString(type) && type != typeof(IntPtr);
        }

        public static bool IsSimple(this Type type)
        {
            return type.IsPrimitive || IsString(type) || type.IsEnum;
        }

        public static bool IsConcrete(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool IsNotConcrete(this Type type)
        {
            return !type.IsConcrete();
        }

        /// <summary>
        /// Returns true if the type is a DateTime or nullable DateTime
        /// </summary>
        /// <param name="typeToCheck"></param>
        /// <returns></returns>
        public static bool IsDateTime(this Type typeToCheck)
        {
            return typeToCheck == typeof(DateTime) || typeToCheck == typeof(DateTime?);
        }

        public static bool IsBoolean(this Type typeToCheck)
        {
            return typeToCheck == typeof(bool) || typeToCheck == typeof(bool?);
        }

        /// <summary>
        /// Displays type names using CSharp syntax style. Supports funky generic types.
        /// </summary>
        /// <param name="type">Type to be pretty printed</param>
        /// <returns></returns>
        public static string PrettyPrint(this Type type)
        {
            return type.PrettyPrint(t => t.Name);
        }

        /// <summary>
        /// Displays type names using CSharp syntax style. Supports funky generic types.
        /// </summary>
        /// <param name="type">Type to be pretty printed</param>
        /// <param name="selector">Function determining the name of the type to be displayed. Useful if you want a fully qualified name.</param>
        /// <returns></returns>
        public static string PrettyPrint(this Type type, Func<Type, string> selector)
        {
            string typeName = selector(type) ?? string.Empty;
            if (!type.IsGenericType)
            {
                return typeName;
            }

            Func<Type, string> genericParamSelector = type.IsGenericTypeDefinition ? t => t.Name : selector;
            string genericTypeList = String.Join(",", type.GetGenericArguments().Select(genericParamSelector).ToArray());
            int tickLocation = typeName.IndexOf('`');
            if (tickLocation >= 0)
            {
                typeName = typeName.Substring(0, tickLocation);
            }
            return string.Format("{0}<{1}>", typeName, genericTypeList);
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// int, long, decimal, short, float, or double
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is numeric</returns>
        public static bool IsNumeric(this Type type)
        {
            return type.IsFloatingPoint() || type.IsIntegerBased();
        }


        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// int, long or short
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is integer based</returns>
        public static bool IsIntegerBased(this Type type)
        {
            return _integerTypes.Contains(type);
        }

        private static readonly IList<Type> _integerTypes = new List<Type>
                                    {
                                        typeof (byte),
                                        typeof (short),
                                        typeof (int),
                                        typeof (long),
                                        typeof (sbyte),
                                        typeof (ushort),
                                        typeof (uint),
                                        typeof (ulong),
                                        typeof (byte?),
                                        typeof (short?),
                                        typeof (int?),
                                        typeof (long?),
                                        typeof (sbyte?),
                                        typeof (ushort?),
                                        typeof (uint?),
                                        typeof (ulong?)
                                    };

        /// <summary>
        /// Returns a boolean value indicating whether or not the type is:
        /// decimal, float or double
        /// </summary>
        /// <param name="type"></param>
        /// <returns>Bool indicating whether the type is floating point</returns>
        public static bool IsFloatingPoint(this Type type)
        {
            return type == typeof(decimal) || type == typeof(float) || type == typeof(double);
        }

        public static string GetPropertyName<TSource, TProperty>(this TSource source,
    Expression<Func<TSource, TProperty>> propertyLambda)
        {
            Type type = typeof(TSource);

            var member = propertyLambda.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expresion '{0}' refers to a property that is not from type {1}.",
                    propertyLambda,
                    type));

            return propInfo.IsNotNull() ? propInfo.Name : string.Empty;
        }

    }
}
