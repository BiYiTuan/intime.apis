using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Contract.Response;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Response
{
    [DataContract]
    public class StoreCouponDetailResponse : BaseResponse
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }
        [DataMember(Name = "code")]
        public string Code { get; set; }
        [IgnoreDataMember]
        public Nullable<System.DateTime> ValidStartDate { get; set; }
        [IgnoreDataMember]
        public Nullable<System.DateTime> ValidEndDate { get; set; }

        [DataMember(Name = "validstartdate")]
        public string ValidStartDate_S { get {
            return ValidStartDate.ToClientTimeFormat();
        }
            set { }
        }
        [DataMember(Name = "validenddate")]
        public string ValidEndDate_S { get {
            return ValidEndDate.ToClientTimeFormat();
        }
            set { }
        }
        [IgnoreDataMember]
        public Nullable<int> Status { get; set; }

        [DataMember(Name = "status")]
        public Nullable<int> Status_s { get {
            if (Status == (int)CouponStatus.Normal
                && ValidEndDate.HasValue
                && ValidEndDate.Value < DateTime.Now)
                return (int)CouponStatus.Expired;
            else
                return Status;
        } set { } }

        [IgnoreDataMember]
        public Nullable<System.DateTime> CreateDate { get; set; }

        [DataMember(Name = "createddate")]
        public string CreateDate_S
        {
            get
            {
                return CreateDate.ToClientTimeFormat();
            }
            set { }
        }
        [DataMember(Name = "amount")]
        public Nullable<decimal> Amount { get; set; }

        [DataMember(Name = "points")]
        public Nullable<int> Points { get; set; }
        [DataMember(Name = "exclude")]
        public string Exclude { get; set; }
        [DataMember(Name = "storename")]
        public string StoreName { get; set; }

        [DataMember(Name = "promotion")]
        public StorePromotionDetailResponse Promotion { get; set; }

    }
    [DataContract]
    public class StorePromotionDetailResponse : BaseResponse
    {
        private IEnumerable<StorePromotionScopeDetailResponse> _inScopeS;
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "description")]
        public string Description { get; set; }
        [IgnoreDataMember]
        public Nullable<System.DateTime> ActiveStartDate { get; set; }
        [IgnoreDataMember]
        public Nullable<System.DateTime> ActiveEndDate { get; set; }


        [DataMember(Name = "activestartdate")]
        public string ActiveStartDate_S
        {
            get
            {
                return ActiveStartDate.ToClientTimeFormat();
            }
            set { }
        }
        [DataMember(Name = "activeenddate")]
        public string ActiveEndDate_S
        {
            get
            {
                return ActiveEndDate.ToClientTimeFormat();
            }
            set { }
        }
        [DataMember(Name = "notice")]
        public string Notice { get; set; }
        [DataMember(Name = "usagenotice")]
        public string UsageNotice { get; set; }
        [IgnoreDataMember]
        public string InScopeNotice { get; set; }
        [DataMember(Name = "inscopenotice")]
        public IEnumerable<StorePromotionScopeDetailResponse> InScopeNotice_S
        {
            get
            {
                if (_inScopeS != null)
                {
                    foreach (var scope in _inScopeS)
                        yield return scope;
                    yield break;
                }
                else
                {
                    IEnumerable<dynamic> jsons = JsonConvert.DeserializeObject(InScopeNotice) as IEnumerable<dynamic>;
                    foreach (var json in jsons)
                    {
                        yield return new StorePromotionScopeDetailResponse
                        {
                            StoreId = json.storeid,
                            StoreName = json.storename,
                            Excludes = json.excludes
                        };
                    }
                }

        } set {
            _inScopeS = value;
        } }
    }
    [DataContract]
    public class StorePromotionScopeDetailResponse : BaseResponse
    {
        [DataMember(Name="storeid")]
        public int StoreId { get; set; }
         [DataMember(Name = "storename")]
        public string StoreName { get; set; }
         [DataMember(Name = "excludes")]
        public string Excludes { get; set; }
    }

}
