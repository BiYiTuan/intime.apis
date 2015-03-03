using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class WX_MenuEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string AppId { get; set; }
        public int ActionType { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? Pos { get; set; }
        public string WKey { get; set; }
       

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
