using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class IMS_GiftCardUserEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string GiftCardAccount { get; set; }
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
