using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class SectionBrandImportStageOutputEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return new Dictionary<string, object> (2){
                {"Code",Code}, {"Name",Name} 
                };}
 
        }

        #endregion
    }
}
