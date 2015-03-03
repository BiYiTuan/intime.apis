using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
    public class ProductSearchOption
    {
        [Display(Name = "商品代码")]
        public int? PId { get; set; }
        [Display(Name = "商品名称")]
        public string Name { get; set; }
        [Display(Name = "状态")]
        public DataStatus? Status { get; set; }
        [Display(Name = "排序")]
        public ProductSortOrder? OrderBy { get; set; }
        [Display(Name = "创建人代码")]
        public int? User { get; set; }
        [UIHint("Association")]
        [AdditionalMetadata("controller", "store")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Name")]
        [Display(Name = "门店")]
        public string Store { get; set; }
        [UIHint("Association")]
        [AdditionalMetadata("controller", "tag")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Name")]
        [Display(Name = "分类")]
        public string Tag { get; set; }
        [UIHint("Association")]
        [AdditionalMetadata("controller", "brand")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Name")]
        [Display(Name = "品牌")]
        public string Brand { get; set; }
        [UIHint("Association")]
        [AdditionalMetadata("controller", "specialtopic")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Name")]
        [Display(Name = "专题")]
        public string Topic { get; set; }

        [UIHint("Association")]
        [AdditionalMetadata("controller", "promotion")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Name")]
        [Display(Name = "促销")]
        public string Promotion { get; set; }


        public int CurrentUser { get; set; }

        public int CurrentUserRole { get; set; }
    }
}
