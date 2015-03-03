using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OutboundItemEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string OutboundNo { get; set; }
        public int ProductId { get; set; }
        public string ProductDesc { get; set; }
        public string StoreItemNo { get; set; }
        public string StoreItemDesc { get; set; }
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal ExtendPrice { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public decimal UnitPrice { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<int> ColorId { get; set; }

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
