using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class RMAItemEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string RMANo { get; set; }
        public int ProductId { get; set; }
        public string ProductDesc { get; set; }
        public string StoreItem { get; set; }
        public string StoreDesc { get; set; }
        public decimal ItemPrice { get; set; }
        public int Quantity { get; set; }
        public decimal ExtendPrice { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int Status { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> SizeValueId { get; set; }
        public Nullable<int> ColorValueId { get; set; }
        public string ColorValueName { get; set; }
        public string SizeValueName { get; set; }
        public Nullable<int> BrandId { get; set; }

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
