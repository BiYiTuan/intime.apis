using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Yintai.Hangzhou.Model.Filters
{
    public class UserAuthSearchOption
    {
        [Display(Name = "门店代码")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "store")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        public int? StoreId { get; set; }
        [Display(Name = "授权数据")]
        public int? Type { get; set; }
        [Display(Name = "用户代码")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "customer")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        public int? UserId { get; set; }
        [Display(Name = "品牌代码")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "brand")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        public int? BrandId { get; set; }
        [Display(Name = "排序")]
        public GenericOrder? OrderBy { get; set; }
    }
}
