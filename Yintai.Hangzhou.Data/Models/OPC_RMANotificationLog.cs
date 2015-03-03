using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_RMANotificationLogEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string RMANo { get; set; }
        public int Status { get; set; }
        public string Message { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }

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
