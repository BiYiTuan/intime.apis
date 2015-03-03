using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_AssociateIncomeTransferEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public int TotalCount { get; set; }
        public int TotalFee { get; set; }
        public bool IsSuccess { get; set; }
        public string TransferRetCode { get; set; }
        public string TransferRetMsg { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int Status { get; set; }
        public string QueryRetCode { get; set; }
        public string QueryRetMsg { get; set; }
        public int SerialNo { get; set; }
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
