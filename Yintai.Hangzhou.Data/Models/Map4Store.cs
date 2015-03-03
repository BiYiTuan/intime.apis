using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Map4StoreEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ChannelStoreId { get; set; }
        public string Channel { get; set; }
        public Nullable<int> ChannelId { get; set; }

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
