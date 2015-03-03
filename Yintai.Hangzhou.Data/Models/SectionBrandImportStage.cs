using System;
using System.Collections.Generic;

namespace Yintai.Hangzhou.Data.Models
{
    public partial class SectionBrandImportStageEntity : Yintai.Architecture.Common.Models.BaseEntity
    {
        public string StoreCode { get; set; }
        public string Department { get; set; }
        public string SupplyCode { get; set; }
        public string ContractCode { get; set; }
        public string CompanyName { get; set; }
        public string CompanyContactPerson { get; set; }
        public string CompanyContactPhone { get; set; }
        public string SectionName { get; set; }
        public string SectionBrandName { get; set; }
        public string SectonBrandEName { get; set; }
        public string SectionCode { get; set; }
        public string OperatorCode { get; set; }

        #region Overrides of BaseEntity

        /// <summary>
        /// KeyMemberId
        /// </summary>
        public override object EntityId
        {       
                get { return new Dictionary<string, object> (7){
                {"StoreCode",StoreCode}, {"SupplyCode",SupplyCode}, {"ContractCode",ContractCode}, {"CompanyName",CompanyName}, {"CompanyContactPerson",CompanyContactPerson}, {"SectionName",SectionName}, {"SectionCode",SectionCode} 
                };}
 
        }

        #endregion
    }
}
