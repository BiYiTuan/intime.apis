using CLAP;
using com.intime.fashion.service.search;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        [Verb(IsDefault = false, Description = "reindex escombos", Aliases = "reindex_combo")]
        static void Reindex_Combo(
            [Aliases("cid")]
            [Description("single combo id")]
            int? id)
        {
            var indexService = SearchLogic.GetService(IndexSourceType.Combo);
            if (id.HasValue)
            {
                indexService.IndexSingle(id.Value);
                Console.WriteLine(string.Format("{0} indexed",id.Value));
            }
            else
            {
                var db = ServiceLocator.Current.Resolve<DbContext>();
                foreach (var combo in db.Set<IMS_ComboEntity>().Where(ic => ic.Status == (int)DataStatus.Normal && ic.ExpireDate > DateTime.Now))
                {
                    indexService.IndexSingle(combo.Id);
                    Console.WriteLine(string.Format("{0} indexed", combo.Id));
                }
            }
        }
    }
}
