using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
    public class StorePromotionSearchOption
    {
        [Display(Name="编码")]
        public int? SId { get; set; }
        [Display(Name = "兑换开始时间")]
        [DataType(DataType.DateTime)]
        public DateTime? ActiveStartDate { get; set; }
        [Display(Name = "兑换结束时间")]
        [DataType(DataType.DateTime)]
        public DateTime? ActiveEndDate { get; set; }
        [Display(Name = "门店代码")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "store")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        public int? StoreId { get; set; }
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Display(Name = "状态")]
        public DataStatus? Status { get; set; }
        [Display(Name = "排序")]
        public GenericOrder? SortBy { get; set; }
    }
}
