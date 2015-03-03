using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Ref_EnumerationEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string Name { get; set; }
        public string BaseType { get; set; }
        public bool HasFlagsAttribute { get; set; }
        public string Description { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return Name; }
 
        }

        #endregion
    }
}
