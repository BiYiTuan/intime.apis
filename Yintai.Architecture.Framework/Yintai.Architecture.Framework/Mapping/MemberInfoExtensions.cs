using System.Reflection;
using Yintai.Architecture.Framework.Extension;

namespace Yintai.Architecture.Framework.Mapping
{
    public static class MemberInfoExtensions
    {
        public static MappingAttribute GetAttribute(this MemberInfo member)
        {
            MappingAttribute[] attributes = member.GetCustomAttributes<MappingAttribute>();

            MappingAttribute attribute = null;
            if (attributes.Length != 0)
                attribute = attributes[0];

            return attribute;
        }
    }
}
