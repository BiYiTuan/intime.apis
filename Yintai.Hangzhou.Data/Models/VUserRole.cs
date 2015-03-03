using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class VUserRoleEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Role_Id { get; set; }
        public int User_Id { get; set; }
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
        public int Role_Val { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return new Dictionary<string, object> (5){
                {"Role_Id",Role_Id}, {"User_Id",User_Id}, {"Role_Name",Role_Name}, {"Role_Description",Role_Description}, {"Role_Val",Role_Val} 
                };}
 
        }

        #endregion
    }
}
