using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.DTO.Response.Resources;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;
using com.intime.fashion.common;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    public class MyOrderDetailResponse:BaseResponse
    {
        [DataMember(Name="orderno")]
        public string OrderNo { get; set; }
        [DataMember(Name = "totalamount")]
        public decimal TotalAmount { get; set; }

        [DataMember(Name="totalpoints")]
        public int TotalPoints {get;set;}
        [DataMember(Name = "totalfee")]
        public decimal TotalFee {get;set;}
        [DataMember(Name = "extendprice")]
        public decimal ExtendPrice {get{
            return TotalAmount;
        }}

        [DataMember(Name="statust")]
        public int Status { get; set; }
         [DataMember(Name = "createdate")]
        public System.DateTime CreateDate { get; set; }
        [DataMember(Name="status")]
         public string StatusF { get {
             return ((OrderStatus)Status).ToFriendlyString();
         } }
        [DataMember(Name="paymentcode")]
        public string PaymentMethodCode { get; set; }
         [DataMember(Name="paymentname")]
         public string PaymentMethodName { get; set; }
        [DataMember(Name = "shippingzipcode")]
         public string ShippingZipCode { get; set; }
        [DataMember(Name = "shippingaddress")]
         public string ShippingAddress { get; set; }
        [DataMember(Name = "shippingcontactperson")]
         public string ShippingContactPerson { get; set; }
        [DataMember(Name = "shippingcontactphone")]
         public string ShippingContactPhone { get; set; }
        [DataMember(Name = "needinvoice")]
         public Nullable<bool> NeedInvoice { get; set; }
        [DataMember(Name = "invoicetitle")]
         public string InvoiceSubject { get; set; }
        [DataMember(Name = "invoicedetail")]
         public string InvoiceDetail { get; set; }
        [DataMember(Name = "shippingfee")]
         public Nullable<decimal> ShippingFee { get; set; }
        [DataMember(Name = "shippingvianame")]
         public string ShippingViaName { get; set; }
        [DataMember(Name = "memo")]
         public string Memo { get; set; }
        [DataMember(Name="ships")]
        public IEnumerable<MyShipResponse> Outbounds { get; set; }
        
        [DataMember(Name="products")]   
        public IEnumerable<MyOrderItemDetailResponse> Products { get; set; }
        [DataMember(Name = "rmas")]
        public IEnumerable<MyRMAResponse> RMAs { get; set; }

     
        [DataMember(Name="canvoid")]
        public bool CanVoid { get {
            return (!IsDaoGou) &&
               new int[]{(int)OrderStatus.Create}.Any(status => status == Status);
        } }
        [DataMember(Name = "canrma")]
        public bool CanRMA {
            get {
                return  (!IsDaoGou) &&
                        Status == (int)OrderStatus.Shipped &&
                        (RMAs==null || !RMAs.Any(r=>new int[]{(int)RMAStatus.Created,(int)RMAStatus.CustomerConfirmed,(int)RMAStatus.PackageReceived,(int)RMAStatus.PassConfirmed}.Any(status=>status==r.Status)));
            }
        }
        [DataMember(Name = "canEditPro")]
        public bool CanEditPro {
            get {
                return Promotion_Flag &&
                        (Status == (int)OrderStatus.Paid || Status == (int)OrderStatus.AgentConfirmed)
                        && IsDaoGou;
            }
        }
        [DataMember(Name="is_owner")]
          public bool IsOwner { get; set; }
        [DataMember(Name = "has_promotion")]
        public bool Promotion_Flag { get; set; }
         [DataMember(Name = "promotion_desc")]
        public string PromotionDesc { get; set; }
         [DataMember(Name = "promotion_rules")]
        public string PromotionRules { get; set; }
        [DataMember(Name="can_like_miniyin")]
         public bool CanLikeMiniYin { get {
             return !string.IsNullOrWhiteSpace(LikeRedirectUrl);
         } }
        [DataMember(Name="liked_redirect_url")]
        public string LikeRedirectUrl { get; set; }

        [IgnoreDataMember]
        public bool IsDaoGou { get; set; }
        [IgnoreDataMember]
        public bool? PromotionFlag { get; set; }

    }
    [DataContract]
    public class MyOrderItemDetailResponse : BaseResponse
    {

        [DataMember(Name = "productid")]
        public string ProductId { get; set; }
        [DataMember(Name = "productname")]
        public string ProductName { get; set; }
         [DataMember(Name = "productdesc")]
        public string ProductDesc { get; set; }
        [DataMember(Name = "itemno")]
        public string StoreItemNo { get; set; }
        [DataMember(Name = "itemdesc")]
        public string StoreItemDesc { get; set; }
        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }
        [DataMember(Name = "price")]
        public decimal ItemPrice { get; set; }
        [DataMember(Name="resource")]
         public ResourceInfoResponse ProductResource { get; set; }
        [DataMember(Name = "skucode")]
        public string SkuCode { get { return StoreItemNo; } }
        [DataMember(Name = "colorvalue")]
        public string ColorValueName { get; set; }
        [DataMember(Name = "colorvalueid")]
        public string ColorValueId { get; set; }
        [DataMember(Name = "sizevalue")]
        public string SizeValueName { get; set; }
        [DataMember(Name = "sizevalueid")]
        public string SizeValueId { get; set; }
        [DataMember(Name = "brandname")]
        public string BrandName { get; set; }
        [DataMember(Name = "brand2name")]
        public string Brand2Name { get; set; }
        [DataMember(Name = "properties")]
        public IEnumerable<TagPropertyDetailResponse> Properties { get; set; }
        [DataMember(Name="sales_code")]
        public string StoreSalesCode { get; set; }
    }
     [DataContract]
    public class MyRMAResponse : BaseResponse
    {
        [DataMember(Name = "rmano")]
        public string RMANo { get; set; }
        [IgnoreDataMember]
        public int RMAType { get; set; }
        [DataMember(Name = "rmatype")]
        public string RMATypeF
        {
            get
            {
                return ((Yintai.Hangzhou.Model.Enums.RMAType)RMAType).ToFriendlyString();
            }
        }
        [DataMember(Name="statust")]
        public int Status { get; set; }
        [DataMember(Name = "status")]
        public string RMAF
        {
            get
            {
                return ((RMAStatus)Status).ToFriendlyString();
            }
        }
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
        [DataMember(Name = "reason")]
        public string Reason { get; set; }
        [DataMember(Name = "rejectreason")]
        public string RejectReason { get; set; }
        [DataMember(Name = "rebatepostfee")]
        public Nullable<decimal> rebatepostfee { get; set; }
        [DataMember(Name = "chargepostfee")]
        public Nullable<decimal> chargepostfee { get; set; }
        [DataMember(Name = "actualamount")]
        public Nullable<decimal> ActualAmount { get; set; }
        [DataMember(Name = "chargegiftfee")]
        public decimal? ChargeGiftFee { get; set; }
        [DataMember(Name = "mailaddress")]
        public string MailAddress { get; set; }
        [DataMember(Name = "canvoid")]
        public bool CanVoid { get; set; }

    }
    [DataContract]
     public class MyShipResponse : BaseResponse {
        [DataMember(Name="shipvianame")]
        public string ShipViaName { get; set; }
        [DataMember(Name = "shipno")]
        public string ShipNo { get; set; }
    }
}
