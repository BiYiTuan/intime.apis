using System.Collections.Generic;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
    public class PromotionFilter
    {
        public Timestamp Timestamp { get; set; }

        public PromotionFilterMode? FilterMode { get; set; }

        public DataStatus? DataStatus { get; set; }

        public bool? HasBanner { get; set; }

        public bool? HasProduct { get; set; }

        public int? RecommendUser { get; set; }

        public DateTimeRangeInfo DateTimeRangeInfo { get; set; }

        public List<int> TagIds { get; set; }

        public List<int> Ids { get; set; }
    }
}
