using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class CategoryEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int ExCatId { get; set; }
        public string Name { get; set; }
        public string ExCatCode { get; set; }
        public int Status { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int TagId { get; set; }

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
