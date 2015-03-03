using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Model.Filters
{
    public class CommentSearchOption
    {
            [Display(Name = "发布人名字")]
            public string CreateUserName { get; set; }
            [Display(Name = "发布人代码")]
            public int? CreateUserId { get; set; }
            [Display(Name = "评论人名字")]
            public string CommentUserName { get; set; }
            [Display(Name = "产品代码")]
            public int? ProductId { get; set; }
            [Display(Name = "活动代码")]
            public int? PromotionId { get; set; }
            [Display(Name = "排序")]
            public GenericOrder? OrderBy { get; set; }



            public int CurrentUser { get; set; }

            public int CurrentUserRole { get; set; }
    }
}
