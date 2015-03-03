using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model
{
    public class UserAccountModel : DomainModel
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public int User_Id { get; set; }

        public int CreatedUser { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int UpdatedUser { get; set; }
        public System.DateTime UpdatedDate { get; set; }

        [System.Obsolete("注意这个")]
        public int AccountType {
            get { return (int) AType; }
            set { AType = (AccountType) value; }
        }
        public int AccountId { get; set; }

        public int Status { get; set; }

        /// <summary>
        /// 用户账户类型
        /// </summary>
        public AccountType AType { get; set; }
    }
}