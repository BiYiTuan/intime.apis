using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Yintai.Hangzhou.Model.Filters
{
    public class CardSearchOption
    {
        [Display(Name = "用户代码")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "customer")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        public int? UserId { get; set; }

        [Display(Name = "排序")]
        public GenericOrder? OrderBy { get; set; }
    }
}
