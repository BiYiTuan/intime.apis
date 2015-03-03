using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class RoleAccessRightEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int AccessRightId { get; set; }
        public virtual AdminAccessRightEntity AdminAccessRight { get; set; }
        public virtual RoleEntity Role { get; set; }

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
