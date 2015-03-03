using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Model.Filters
{
    public class OrderSearchOption
    {
        [Display(Name = "订单号")]
        public string OrderNo { get; set; }
        [Display(Name = "订购人编号")]
        public int? CustomerId { get; set; }
        [Display(Name = "开始时间")]
        public DateTime? FromDate { get; set; }
        [Display(Name = "结束时间")]
        public DateTime? ToDate { get; set; }
        [Display(Name="订单状态")]
        public OrderStatus? Status { get; set; }
        [UIHint("Association")]
        [AdditionalMetadata("controller", "store")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        [Display(Name = "门店")]
        public int? Store { get; set; }

        [UIHint("Association")]
        [AdditionalMetadata("controller", "brand")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("valuefield", "Id")]
        [Display(Name = "品牌")]
        public int? Brand { get; set; }

        public int CurrentUser { get; set; }

        public int CurrentUserRole { get; set; }

    }
}
