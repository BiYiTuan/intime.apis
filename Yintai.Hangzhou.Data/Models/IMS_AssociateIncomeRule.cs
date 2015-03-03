using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_AssociateIncomeRuleEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public System.DateTime FromDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int RuleType { get; set; }
        public int Status { get; set; }
        public Nullable<int> GroupId { get; set; }
        public Nullable<int> ComputeType { get; set; }

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
