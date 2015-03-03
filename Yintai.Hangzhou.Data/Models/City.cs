using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class CityEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsProvince { get; set; }
        public Nullable<int> ParentId { get; set; }
        public int Status { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string ZipCode { get; set; }
        public Nullable<bool> IsCity { get; set; }

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
