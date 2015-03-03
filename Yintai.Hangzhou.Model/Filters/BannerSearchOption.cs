using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
   public class BannerSearchOption
    {
        [Display(Name = "活动代码")]
        public int? PromotionId { get; set; }
        [Display(Name = "状态")]
        public DataStatus? Status { get; set; }
        [Display(Name = "排序")]
        public GenericOrder? OrderBy { get; set; }
    }
}
