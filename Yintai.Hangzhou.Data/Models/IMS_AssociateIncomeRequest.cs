using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_AssociateIncomeRequestEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string BankName { get; set; }
        public string BankNo { get; set; }
        public decimal Amount { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public string BankCode { get; set; }
        public string BankAccountName { get; set; }
        public string TransferErrorCode { get; set; }
        public string TransferErrorMsg { get; set; }
        public Nullable<decimal> TransferFee { get; set; }
        public string IDCard { get; set; }
        public Nullable<int> GroupId { get; set; }

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
