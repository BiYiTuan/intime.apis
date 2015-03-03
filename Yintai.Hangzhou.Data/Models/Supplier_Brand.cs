using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Supplier_BrandEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }
        public int Supplier_Id { get; set; }
        public int Brand_Id { get; set; }

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
