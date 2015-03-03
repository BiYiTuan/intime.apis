using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Model.ES;

namespace Yintai.Hangzhou.Model.ESModel
{
    public class ESCombo
    {
        public int Id { get; set; }
        public string Desc { get; set; }
        public decimal Price { get; set; }
        public string Private2Name { get; set; }
        public int Status { get; set; }
        public int UserId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int CreateUser { get; set; }
        public System.DateTime OnlineDate { get; set; }
        public System.DateTime UpdateDate { get; set; }
        public int UpdateUser { get; set; }
        public Nullable<int> ProductType { get; set; }
        public DateTime? ExpireDate { get; set; }
        public IEnumerable<ESResource> Resources { get; set; }
        public int StoreId { get; set; }
        public IEnumerable<ESBrand> Brands { get; set; }
        public int AssociateId { get; set; }
        public bool? IsInPromotion { get; set; }
        public decimal? DiscountAmount { get; set; }
        public string AssociateName { get; set; }
        public decimal OriginPrice { get; set; }
        public IEnumerable<ESIMSTag> Tags { get; set; }
        public ESGroup Group { get; set; }
    }
}
