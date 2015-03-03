using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_SaleDetailEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string SaleOrderNo { get; set; }
        public Nullable<int> OrderItemID { get; set; }
        public string SectionCode { get; set; }
        public int Status { get; set; }
        public int StockId { get; set; }
        public int SaleCount { get; set; }
        public decimal Price { get; set; }
        public Nullable<int> BackNumber { get; set; }
        public string ProdSaleCode { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> RemarkDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedUser { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> UpdatedUser { get; set; }

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
