using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
    public class ProductFilter
    {
        public DataStatus? DataStatus { get; set; }

        public Timestamp Timestamp { get; set; }

        public string ProductName { get; set; }

        public int? RecommendUser { get; set; }

        public List<int> TagIds { get; set; }

        public int? BrandId { get; set; }

        public int? TopicId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public int? PromotionId { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.Append("[productfilter:");

            if (DataStatus != null)
            {
                sb.AppendFormat("ds_{0}|", DataStatus);
            }

            if (!String.IsNullOrWhiteSpace(ProductName))
            {
                sb.AppendFormat("pn_{0}|", ProductName);
            }

            if (RecommendUser != null)
            {
                sb.AppendFormat("ru_{0}|", RecommendUser);
            }

            if (TagIds != null)
            {
                var t = String.Join(",", TagIds);
                sb.AppendFormat("tag_{0}|", t);
            }

            if (BrandId != null)
            {
                sb.AppendFormat("b_{0}|", BrandId);
            }

            if (TopicId != null)
            {
                sb.AppendFormat("top_{0}|", TopicId);
            }


            if (Timestamp != null)
            {
                sb.AppendFormat("ts_{0}|", Timestamp.ToString());
            }

            if (PromotionId != null)
            {
                sb.AppendFormat("pro_{0}", PromotionId);
            }

            sb.Append("]");

            return sb.ToString();
        }
    }
}
