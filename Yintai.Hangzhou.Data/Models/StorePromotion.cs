using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class StorePromotionEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> ActiveStartDate { get; set; }
        public Nullable<System.DateTime> ActiveEndDate { get; set; }
        public Nullable<int> PromotionType { get; set; }
        public Nullable<int> AcceptPointType { get; set; }
        public string Notice { get; set; }
        public Nullable<System.DateTime> CouponStartDate { get; set; }
        public Nullable<System.DateTime> CouponEndDate { get; set; }
        public Nullable<int> MinPoints { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public string UsageNotice { get; set; }
        public string InScopeNotice { get; set; }
        public Nullable<int> UnitPerPoints { get; set; }

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
