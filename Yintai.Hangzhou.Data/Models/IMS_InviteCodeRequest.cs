using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_InviteCodeRequestEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public string ContactMobile { get; set; }
        public string Name { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public string OperatorCode { get; set; }
        public int StoreId { get; set; }
        public int DepartmentId { get; set; }
        public int RequestType { get; set; }
        public int CreateUser { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int UpdateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public Nullable<bool> Approved { get; set; }
        public Nullable<int> ApprovedBy { get; set; }
        public Nullable<System.DateTime> ApprovedDate { get; set; }
        public string RejectReason { get; set; }
        public string IdCard { get; set; }
        public Nullable<int> ApprovedNotificationTimes { get; set; }
        public Nullable<int> DemotionNotificationTimes { get; set; }

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
