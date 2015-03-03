using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Filters
{
    public class HotwordSearchOption
    {
        [Display(Name = "关键词")]
        public string Word { get; set; }
        [Display(Name = "类型")]
        public int? Type { get; set; }
        [Display(Name = "排序")]
        public GenericOrder? OrderBy { get; set; }
    }
}
