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
        [Verb(IsDefault = false, Description = "reindex types", Aliases = "reindex_type")]
        static void Reindex_Types(
            [Aliases("re_index_type")]
            [Description("p:product,b:brand,s:store,it:imstags,g:group")]
            [Required]
            string type)
        {
            var indexType = IndexSourceType.Product;
            IEnumerable<int> idList = null;
            if (type == "p")
            {
                indexType = IndexSourceType.Product;
                idList = allActiveProducts();
            }
            else if (type == "b")
            {
                indexType = IndexSourceType.Brand;
                idList = allActiveBrands();
            }
            else if (type == "s")
            {
                indexType = IndexSourceType.Store;
                idList = allActiveStores();
            }
            else if (type == "it")
            {
                indexType = IndexSourceType.IMSTag;
                idList = allActiveIMSTags();
            }
            else if (type == "g")
            {
                indexType = IndexSourceType.Group;
                idList = allActiveGroups();
            }
            else if (type == "t")
            {
                indexType = IndexSourceType.Tag;
                idList = allActiveTags();
            }
            else
            {
                Console.WriteLine("not support type");
                return;
            }

            var indexService = SearchLogic.GetService(indexType);


            foreach (var iid in idList)
            {
                try
                {
                    indexService.IndexSingle(iid);
                    Console.WriteLine(string.Format("{0} indexed", iid));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Console.WriteLine(string.Format("error: {0} not indexed", iid));
                }
            }
        }

        private static IEnumerable<int> allActiveGroups()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<GroupEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                   .Select(p => p.Id).ToList();
        }

        private static IEnumerable<int> allActiveIMSTags()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<IMS_TagEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                   .Select(p => p.Id).ToList();
        }

        private static IEnumerable<int> allActiveStores()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<StoreEntity>().Where(p => p.Status == (int)DataStatus.Normal && p.IsOnLine.HasValue
                && p.IsOnLine.Value == 1
                )
                   .Select(p => p.Id).ToList();
        }

        private static IEnumerable<int> allActiveBrands()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<BrandEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                   .Select(p => p.Id).ToList();
        }

        private static IEnumerable<int> allActiveProducts()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<ProductEntity>().Where(p => p.Status == (int)DataStatus.Normal)
                   .Select(p => p.Id).ToList();

        }

        private static IEnumerable<int> allActiveTags()
        {
            var db = ServiceLocator.Current.Resolve<DbContext>();
            return db.Set<TagEntity>().Where(t => t.Status == (int) DataStatus.Normal).Select(t => t.Id);
        }
    }
}
