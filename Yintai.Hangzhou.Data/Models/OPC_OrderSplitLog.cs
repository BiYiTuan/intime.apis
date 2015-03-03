using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_OrderSplitLogEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int ID { get; set; }
        public string OrderNo { get; set; }
        public int Status { get; set; }
        public string Reason { get; set; }
        public System.DateTime CreateDate { get; set; }

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
