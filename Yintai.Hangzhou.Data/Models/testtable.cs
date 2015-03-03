using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class testtableEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public int number { get; set; }
        public string name { get; set; }
        public string content { get; set; }
        public Nullable<System.DateTime> time { get; set; }
        public Nullable<decimal> money { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<decimal> height { get; set; }
        public Nullable<int> more { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return number; }
 
        }

        #endregion
    }
}
