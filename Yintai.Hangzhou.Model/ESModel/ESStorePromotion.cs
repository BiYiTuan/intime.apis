using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.ES
{
    public class ESStorePromotion
    {
        private DateTime _activeStartDate;
        private DateTime _activeEndDate;
        private DateTime _couponEndDate;
        private DateTime _couponStartDate;
        public System.DateTime ActiveStartDate
        {
            get
            {
                return _activeStartDate.ToUniversalTime();
            }
            set
            {
                _activeStartDate = value;
            }
        }
        public System.DateTime ActiveEndDate
        {
            get
            {
                return _activeEndDate.ToUniversalTime();
            }
            set
            {
                _activeEndDate = value;
            }
        }
        public System.DateTime CouponEndDate
        {
            get
            {
                return _couponEndDate.ToUniversalTime();
            }
            set
            {
                _couponEndDate = value;
            }
        }
        public System.DateTime CouponStartDate
        {
            get
            {
                return _couponStartDate.ToUniversalTime();
            }
            set
            {
                _couponStartDate = value;
            }
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> PromotionType { get; set; }
        public Nullable<int> AcceptPointType { get; set; }
        public string Notice { get; set; }
        public Nullable<int> MinPoints { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<int> CreateUser { get; set; }
        public string UsageNotice { get; set; }
        public string InScopeNotice { get; set; }
        public string ExchangeRule { get; set; }
        public int UnitPerPoints { get; set; }

        public  T FromEntity<T>(dynamic entity) where T : class
        {
            return FromEntity<T>(entity, null) as T;
        }

        public  T FromEntity<T>(dynamic entity, Action<T> moreMapping) where T : class
        {
            if (entity == null)
                return default(T);
            T model = AutoMapper.Mapper.DynamicMap(entity, entity.GetType(), typeof(T)) as T;
            if (moreMapping != null)
            {
                moreMapping(model);
            }
            return model;
        }
    }
}
