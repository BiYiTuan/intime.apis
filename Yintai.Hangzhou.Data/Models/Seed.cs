using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class SeedEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string Name { get; set; }
        public Nullable<int> Id { get; set; }
        public Nullable<int> Value { get; set; }

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
