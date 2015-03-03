using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ProductPropertyEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string PropertyDesc { get; set; }
        public int Status { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int UpdateUser { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<bool> IsColor { get; set; }
        public Nullable<bool> IsSize { get; set; }
        public Nullable<int> ChannelPropertyId { get; set; }

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
