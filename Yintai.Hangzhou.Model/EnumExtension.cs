using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public static class Enum2Extension
    {
        public static string FriendString(this Enum proStatus)
        {
            FieldInfo EnumInfo = proStatus.GetType().GetField(proStatus.ToString());
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.
                GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }
            return proStatus.ToString();
        }
    }
}
