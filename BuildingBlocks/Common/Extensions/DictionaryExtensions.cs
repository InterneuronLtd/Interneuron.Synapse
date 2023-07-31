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
