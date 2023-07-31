 //Interneuron synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

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
﻿using Interneuron.Common.Extensions;
using Interneuron.Terminology.API.AppCode.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Interneuron.Terminology.API.AppCode.Extensions
{
    public static class CommonExtensions
    {
        public static bool HasSamePropertyValues<T>(this object A, object B, HashSet<string> propNamesToCheck = null)
        {
            if (A == null || B == null) return false;

            var type = typeof(T);
            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allSimpleProperties = allProperties.Where(pi => pi.PropertyType.IsSimpleType());
            var unequalProperties =
                   from pi in allSimpleProperties
                   where (propNamesToCheck == null) || (propNamesToCheck.IsCollectionValid() && propNamesToCheck.Contains(pi.Name))
                   let AValue = type.GetProperty(pi.Name).GetValue(A, null)
                   let BValue = type.GetProperty(pi.Name).GetValue(B, null)
                   where AValue != BValue && (AValue == null || !AValue.Equals(BValue))
                   select pi.Name;
            return !unequalProperties.IsCollectionValid();
        }

        public static bool HasAllProperties<T>(this object obj, HashSet<string> propNamesToCheck)
        {
            var type = typeof(T);
            var allProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var allSimpleProperties = allProperties.Where(pi => pi.PropertyType.IsSimpleType());
            var matchedProperties =
                   from pi in allSimpleProperties
                   where (propNamesToCheck.IsCollectionValid() && propNamesToCheck.Contains(pi.Name))
                   select pi.Name;
            return (matchedProperties.IsCollectionValid() && matchedProperties.Count() == propNamesToCheck.Count);
        }

        public static IEnumerable<string> GetPropertyResult<T>(this IEnumerable<T> list, string propName)
        {
            var retList = new List<string>();

            var prop = typeof(T).GetProperty(propName);
            if (prop == null) return null;

            retList = list.Select(c => prop.GetValue(c).ToString()).ToList();
            return retList;
        }

        public static bool IsSimpleType(this Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return type.GetGenericArguments()[0].IsSimpleType();
            }
            return type.IsPrimitive
              || type.IsEnum
              || type.Equals(typeof(string))
              || type.Equals(typeof(decimal));
        }

        public static bool HasProperty(this ExpandoObject obj, string propertyName)
        {
            //return ((IDictionary<String, object>)obj).ContainsKey(propertyName);
            if (obj is ExpandoObject)
                return ((IDictionary<string, object>)obj).ContainsKey(propertyName);

            return obj.GetType().GetProperty(propertyName) != null;
        }

        public static IApplicationBuilder UseMaximumRequestTimeout(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<MaximumRequestTimeoutMiddleware>();

            return builder;
        }
    }
}
