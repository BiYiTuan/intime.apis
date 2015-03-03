using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Group_WeixinKeysEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public string AppId { get; set; }
        public string AppSecret { get; set; }
        public string PaySignKey { get; set; }
        public string ParterId { get; set; }
        public int Status { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string ParterKey { get; set; }
        public string PaidLikeUrl { get; set; }
        public string Outcome_OperatorId { get; set; }
        public string Outcome_OperatorPwd { get; set; }
        public string Outcome_ParterId { get; set; }
        public string Outcome_ParterKey { get; set; }
        public string Outcome_CARelativeFilePath { get; set; }
        public string Outcome_CertRelativeFilePath { get; set; }
        public string Outcome_CertFilePwd { get; set; }

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
