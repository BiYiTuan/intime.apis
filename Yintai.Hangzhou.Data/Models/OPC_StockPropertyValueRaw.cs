using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_StockPropertyValueRawEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public string SourceStockId { get; set; }
        public string PropertyData { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string Channel { get; set; }
        public string BrandSizeName { get; set; }
        public string BrandSizeCode { get; set; }

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
