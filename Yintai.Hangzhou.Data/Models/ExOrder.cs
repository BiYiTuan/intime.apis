using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class ExOrderEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string ExOrderNo { get; set; }
        public decimal Amount { get; set; }
        public string PaymentCode { get; set; }
        public System.DateTime PaidDate { get; set; }
        public Nullable<bool> IsShipped { get; set; }
        public Nullable<System.DateTime> ShipDate { get; set; }
        public Nullable<int> OrderType { get; set; }

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
