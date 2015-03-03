using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class RoleEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public RoleEntity()
        {
            this.RoleAccessRights = new List<RoleAccessRightEntity>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedUser { get; set; }
        public int Status { get; set; }
        public int Val { get; set; }
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
