using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.common.Wxpay
{
   public class WxMenu
    {
       [JsonProperty("button")]
       public IEnumerable<WxMenuItemBase> Buttons { get; set; }
    }

   public class WxMenuItemBase
   {
       [JsonProperty("name")]
       public string Name { get; set; }
   }
   public class WxMenuItemClick:WxMenuItemBase
   {
       [JsonProperty("type")]
       public string Type { get; set; }
       [JsonProperty("key")]
       public string Key { get; set; }
   }
   public class WxMenuItemView : WxMenuItemBase
   {
       [JsonProperty("type")]
       public string Type { get; set; }
       [JsonProperty("url")]
       public string Url { get; set; }
   }
   public class WxMenuItemSub : WxMenuItemBase
   {
       [JsonProperty("sub_button")]
       public IEnumerable<WxMenuItemBase> SubButtons { get; set; }
   }

}

