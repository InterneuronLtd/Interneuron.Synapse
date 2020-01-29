
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
