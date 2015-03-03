using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yintai.Hangzhou.Model.Enums
{
    public enum SpecialTopicType
    {
        [Description("默认")]
        Default = 0,
        [Description("商品列表")]
        Products = 1,
        [Description("促销详情")]
        Promotion = 2,
        [Description("商品详情")]
        Product =3,
        [Description("只做为显示")]
        DisplayOnly = 4,
        [Description("跳转链接")]
        Url = 5,
        [Description("积点列表")]
        PointList =6
    }
}
