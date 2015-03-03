using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.config
{
    public class Weixin_IMSConfiguration:CommonConfigurationBase
    {
        protected override string SectionName
        {
            get { return "weixin_ims"; }
        }
        public string Paid4DaoGou_Template_Id { get { return GetItem("paid_daogou_template_id"); } }
        public string Paid4User_Template_Id { get { return GetItem("paid_user_template_id"); } }
        public string StoreApply_Template_Id { get { return GetItem("store_apply_template_id"); } }
        public string App_Id { get { return GetItem("app_id"); } }
        public string App_Secret { get { return GetItem("app_secret"); } }

    }
}
