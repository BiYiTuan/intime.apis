using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ShippingAddressEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ShippingZipCode { get; set; }
        public string ShippingAddress1 { get; set; }
        public string ShippingContactPerson { get; set; }
        public string ShippingContactPhone { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> ShippingProvinceId { get; set; }
        public Nullable<int> ShippingCityId { get; set; }
        public string ShippingProvince { get; set; }
        public string ShippingCity { get; set; }
        public Nullable<int> ShippingDistrictId { get; set; }
        public string ShippingDistrictName { get; set; }

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
