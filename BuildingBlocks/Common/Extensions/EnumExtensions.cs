using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Interneuron.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string DisplayName(this Enum value)
        {
            Type enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            MemberInfo member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            var outString = attrs.IsCollectionValid()?((DisplayAttribute)attrs[0]).Name : value.ToString();
            
            return outString;
        }
    }
}
