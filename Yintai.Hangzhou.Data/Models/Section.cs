using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class SectionEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string Location { get; set; }
        public string ContactPhone { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> BrandId { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> UpdateUser { get; set; }
        public string ContactPerson { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public Nullable<int> ChannelSectionId { get; set; }
        public string SectionCode { get; set; }
        public Nullable<int> DepartmentId { get; set; }

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
