using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_AssociateIncomeRuleFlexEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int RuleId { get; set; }
        public decimal BenchFrom { get; set; }
        public decimal BenchTo { get; set; }
        public Nullable<decimal> Percentage { get; set; }

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
