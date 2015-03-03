using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_ComboEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public string Private2Name { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public System.DateTime OnlineDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int UpdateUser { get; set; }
        public Nullable<int> ProductType { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<bool> IsInPromotion { get; set; }
        public Nullable<decimal> DiscountAmount { get; set; }
        public Nullable<bool> IsPublic { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }

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
