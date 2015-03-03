using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class OPC_CategoryMapEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public string ChannelCategoryCode { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public string Channel { get; set; }

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
