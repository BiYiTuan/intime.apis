using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Promotion2ProductEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public Nullable<int> ProdId { get; set; }
        public Nullable<int> ProId { get; set; }
        public Nullable<int> Status { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return Id; }
 
        }

        #endregion
    }
}
