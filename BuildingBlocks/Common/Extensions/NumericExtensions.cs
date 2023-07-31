//BEGIN LICENSE BLOCK 
//Interneuron Synapse

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
//END LICENSE BLOCK 
﻿//Interneuron Synapse

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
