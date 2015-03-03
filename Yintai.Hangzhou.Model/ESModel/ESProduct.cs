using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Model.ESModel;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESProduct
    {
        private DateTime _createdDate;
        private DateTime _updatedDate;
        public int Id { get; set; }
        public string Name {get;set;}
        public string Description { get; set; }
        public DateTime CreatedDate
        {
            get {
                return _createdDate.ToUniversalTime();
            }
            set
            {
            _createdDate = value;
        } }

        public DateTime UpdatedDate
        {
            get
            {
                return _updatedDate.ToUniversalTime();
            }
            set
            {
                _updatedDate = value;
            }
        }

        public decimal Price { get; set; }
        public string RecommendedReason { get; set; }
        public int Status { get; set; }
        public ESStore Store { get; set; }  
        public ESTag Tag { get; set; }
        public ESBrand Brand { get; set; }
        public int SortOrder { get; set; }
        public IEnumerable<ESResource> Resource { get; set; }
        public IEnumerable<ESPromotion> Promotion { get; set; }
        public IEnumerable<ESSpecialTopic> SpecialTopic { get; set; }
        public IEnumerable<ESProductPropertyValue> PropertyValues { get; set; }
        public IEnumerable<ESStockPropertyValue> StockPropertyValues { get; set; } 
        public int CreateUserId { get; set; }
        public bool Is4Sale { get; set; }
        public decimal? UnitPrice { get; set; }
        public int FavoriteCount { get; set; }
        public int ShareCount { get; set; }
        public int InvolvedCount { get; set; }
        public int RecommendUserId { get; set; }
        public ESSection Section { get; set; }
        public string UpcCode { get; set; }
        public bool IsSystem { get; set; }
        public decimal DiscountRate { get{
            if (!UnitPrice.HasValue || UnitPrice.Value <= 0m)
                return 0;
            return (int)((UnitPrice-Price) / UnitPrice * 100);
        } }
        public int CategoryId { get; set; }
    }
}
