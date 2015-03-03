using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_OrgInfoEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int ID { get; set; }
        public string OrgID { get; set; }
        public string OrgName { get; set; }
        public string ParentID { get; set; }
        public Nullable<int> StoreOrSectionID { get; set; }
        public string StoreOrSectionName { get; set; }
        public Nullable<int> OrgType { get; set; }
        public bool IsDel { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return ID; }
 
        }

        #endregion
    }
}
