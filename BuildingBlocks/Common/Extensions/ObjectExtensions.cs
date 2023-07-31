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

using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Interneuron.Common.Extensions
{
    public static class ObjectExtensions
    {
        public static bool IsNotNull<T>(this T obj) where T : class
        {
            return obj != null;
        }

        public static bool IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        public static T Clone<T>(this T obj) where T : class
        {
            return obj;
        }

		public static byte[] SerializeEntity<T>(this T obj)
		{
			var typeAttr = obj.GetType().Attributes;

			if ((obj.GetType().Attributes & TypeAttributes.Serializable) == TypeAttributes.Serializable)
			{
				byte[] bytes;
				IFormatter formatter = new BinaryFormatter();
				using (var stream = new MemoryStream())
				{
					formatter.Serialize(stream, obj);
					stream.Position = 0;
					bytes = stream.ToArray();
				}
				return bytes;
			}
			return null;
		}

		public static T DeSerializeEntity<T>(this byte[] bytesObj)
		{
			IFormatter formatter = new BinaryFormatter();

			using (var stream = new MemoryStream(bytesObj))
			{
				return (T)formatter.Deserialize(stream);
			}
		}
    }

}
