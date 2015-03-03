using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class Ref_EnumerationMemberEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string EnumerationName { get; set; }
        public string Name { get; set; }
        public long Value { get; set; }
        public string Description { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return new Dictionary<string, object> (2){
                {"EnumerationName",EnumerationName}, {"Name",Name} 
                };}
 
        }

        #endregion
    }
}
