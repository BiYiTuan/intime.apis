using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ProductPropertyStageEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string ItemCode { get; set; }
        public string PropertyDesc { get; set; }
        public string ValueDesc { get; set; }
        public int SortOrder { get; set; }
        public int UploadGroupId { get; set; }
        public Nullable<int> Status { get; set; }

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
