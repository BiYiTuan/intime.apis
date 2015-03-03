using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class AdminAccessRightEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public AdminAccessRightEntity()
        {
            this.RoleAccessRights = new List<RoleAccessRightEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ControllName { get; set; }
        public string ActionName { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> InDate { get; set; }
        public Nullable<int> InUser { get; set; }
        public virtual ICollection<RoleAccessRightEntity> RoleAccessRights { get; set; }

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
