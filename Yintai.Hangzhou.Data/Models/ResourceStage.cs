using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ResourceStageEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExtName { get; set; }
        public Nullable<int> Width { get; set; }
        public Nullable<int> Height { get; set; }
        public Nullable<int> ContentSize { get; set; }
        public string Size { get; set; }
        public Nullable<int> SortOrder { get; set; }
        public Nullable<int> InUser { get; set; }
        public string ItemCode { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public int Status { get; set; }
        public Nullable<int> UploadGroupId { get; set; }
        public Nullable<bool> IsDimension { get; set; }

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
