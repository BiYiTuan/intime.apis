using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;
using com.intime.fashion.common;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    public class RMAInfoResponse:BaseResponse
    {
        [DataMember(Name="rmano")]
        public string RMANo { get; set; }
        [DataMember(Name = "orderno")]
        public string OrderNo { get; set; }
        [DataMember(Name = "rmaamount")]
        public decimal RMAAmount { get; set; }
        [DataMember(Name = "createdate")]
        public System.DateTime CreateDate { get; set; }
        public int UpdateUser { get; set; }
        public System.DateTime UpdateDate { get; set; }
        [DataMember(Name = "bankname")]
        public string BankName { get; set; }
        [DataMember(Name = "bankaccount")]
        public string BankAccount { get; set; }
        [DataMember(Name = "bankcard")]
        public string BankCard { get; set; }
        [DataMember(Name="contactphone")]
        public string ContactPhone{get;set;}
        [DataMember(Name="shipvia")]
        public int ShipVia {get;set;}
        [DataMember(Name="shipviano")]
        public string ShipViaNo {get;set;}
        [DataMember(Name = "reason")]
        public string Reason { get; set; }
        [DataMember(Name = "rejectreason")]
        public string RejectReason { get; set; }

        [DataMember(Name = "statust")]
        public int Status { get; set; }

        [DataMember(Name = "status")]
        public string StatusF
        {
            get
            {
                return ((RMAStatus)Status).ToFriendlyString();
            }
        }
      
        [DataMember(Name = "products")]
        public IEnumerable<RMAItemInfoResponse> Products { get; set; }
        [DataMember(Name="logs")]
        public IEnumerable<RMALogResponse> Logs {get;set;}
        [DataMember(Name="mailaddress")]
        public string MailAddress {get;set;}
        [DataMember(Name = "canvoid")]
        public bool CanVoid
        {
            get;
            set;
        }
    }

    public class RMAItemInfoResponse : BaseResponse
    { 
         [DataMember(Name = "productid")]
        public string ProductId { get; set; }
        [DataMember(Name = "productname")]
         public string ProductName { get { return ProductDesc; } }
        [IgnoreDataMember]
        public string ProductDesc { get; set; }
        [DataMember(Name = "itemno")]
        public string StoreItem { get; set; }
        [DataMember(Name = "itemdesc")]
        public string StoreDesc { get { return ProductDesc; } }
        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }
        [DataMember(Name = "price")]
        public decimal ItemPrice { get; set; }
        [DataMember(Name="resource")]
         public ResourceInfoResponse ProductResource { get; set; }
        [DataMember(Name = "skucode")]
        public string SkuCode { get { return StoreItem; } }
        [DataMember(Name = "brandname")]
        public string BrandName { get; set; }
        [DataMember(Name = "brand2name")]
        public string Brand2Name { get; set; }


        [DataMember(Name = "properties")]
        public IEnumerable<TagPropertyDetailResponse> Properties { get; set; }
    }

    public class RMALogResponse:BaseResponse
    {
        [DataMember(Name="createddate")]
        public DateTime CreatedDate {get;set;}
        [DataMember(Name="memo")]
        public string Memo {get;set;}
    }
}
