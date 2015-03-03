using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class tb_SeedEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int id { get; set; }
        public Nullable<int> value { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return id; }
 
        }

        #endregion
    }
}
