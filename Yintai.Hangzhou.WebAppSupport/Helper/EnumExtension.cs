using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;

namespace Yintai.Hangzhou.WebSupport.Helper
{
    public static class EnumExtension
    {
        /// <summary>
        /// 用的时候       @Html.DropDownListFor(model => model.ExpertIsWork, Consts.YesNo.是.ToSelectList(0))
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <param name="selected"></param>
        /// <returns></returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj, int selected)
        {
            try
            {
                var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                             select
                                 new SelectListItem { Value = (Convert.ToInt32(e).ToString(CultureInfo.InvariantCulture)), Text = e.ToString() };
                return new SelectList(values, "Value", "Text", selected);
            }
            catch
            {
                return new SelectList(new List<string> { "转换枚举类型到列表时出现错误" }, selected);
            }
        }

        /// <summary>
        /// 用的时候       @Html.DropDownListFor(model => model.ExpertIsWork, Consts.YesNo.是.ToSelectList(0))
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="enumObj"></param>
        /// <returns></returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj)
        {
            try
            {
                var name = Architecture.Framework.Extension.EnumExtension.GetEnumName(typeof(TEnum), enumObj);


                var values = from TEnum e in Enum.GetValues(typeof(TEnum))
                             select
                                 new SelectListItem { Value = (Convert.ToInt32(e).ToString(CultureInfo.InvariantCulture)), Text = e.ToString(), Selected = String.Compare(e.ToString(), name, StringComparison.OrdinalIgnoreCase) == 0 };

                return new SelectList(values, "Value", "Text");
            }
            catch
            {
                return new SelectList(new List<string> { "转换枚举类型到列表时出现错误" });
            }
        }
    }
}
