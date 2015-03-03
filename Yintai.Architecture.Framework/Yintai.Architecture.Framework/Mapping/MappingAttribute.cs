using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.Framework.Mapping
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field,
        AllowMultiple = false, Inherited = true)]
    public class MappingAttribute : Attribute
    {
        public MappingAttribute(string name)
            : this(name, false)
        { }

        public MappingAttribute(string name, bool ignored)
        {
            this.Name = name;
            this.Ignored = ignored;
        }

        public string Name { get; set; }

        public bool Ignored { get; set; }
    }
}
