using CLAP;
using com.intime.fashion.common.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Hangzhou.Data.Models;

namespace com.intime.fashion.console.onetime
{
    partial class OneTimeCommand
    {
        [Verb(IsDefault = false, Description = "auto generate brand's group name from chinese name",Aliases="brand_gen_group")]
       static void Brand_Gen_Group(
            [Aliases("bid")]
            [Description("single brand id")]
            int? id)
        {
            using (var db = new YintaiHangzhouContext())
            {
                var linq = db.Set<BrandEntity>().AsQueryable();
                if (id.HasValue)
                    linq = linq.Where(b => b.Id == id.Value);
                var brands = linq.ToList();
                foreach (var brand in brands)
                { 
                    if (brand.Group!=null && brand.Group.Trim()!="" 
                        && brand.Group.Trim()!="0")
                        continue;
                    var inputName = brand.EnglishName;
                    if (string.IsNullOrEmpty(inputName))
                        inputName = brand.Name;
                    brand.Group = ChineseUtil.FirstPinYin(inputName, '0').ToString();
                    brand.UpdatedDate = DateTime.Now;
                    db.Entry(brand).State = EntityState.Modified;

                    db.SaveChanges();
                }
            }
        }
    }
}
