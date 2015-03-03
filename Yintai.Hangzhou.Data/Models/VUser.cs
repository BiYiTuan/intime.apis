using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class VUserEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public System.DateTime UpdatedDate { get; set; }
        public System.DateTime LastLoginDate { get; set; }
        public string Mobile { get; set; }
        public string EMail { get; set; }
        public int Status { get; set; }
        public int UserLevel { get; set; }
        public int Store_Id { get; set; }
        public int Region_Id { get; set; }
        public string Logo { get; set; }
        public string Description { get; set; }
        public byte Gender { get; set; }
        public int Role_Id { get; set; }
        public string Role_Name { get; set; }
        public string Role_Description { get; set; }
        public int Role_Val { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return new Dictionary<string, object> (22){
                {"User_Id",User_Id}, {"Name",Name}, {"Password",Password}, {"Nickname",Nickname}, {"CreatedUser",CreatedUser}, {"CreatedDate",CreatedDate}, {"UpdatedUser",UpdatedUser}, {"UpdatedDate",UpdatedDate}, {"LastLoginDate",LastLoginDate}, {"Mobile",Mobile}, {"EMail",EMail}, {"Status",Status}, {"UserLevel",UserLevel}, {"Store_Id",Store_Id}, {"Region_Id",Region_Id}, {"Logo",Logo}, {"Description",Description}, {"Gender",Gender}, {"Role_Id",Role_Id}, {"Role_Name",Role_Name}, {"Role_Description",Role_Description}, {"Role_Val",Role_Val} 
                };}
 
        }

        #endregion
    }
}
