using System.Collections.Generic;
using Yintai.Hangzhou.Model.ES;

namespace Yintai.Hangzhou.WebApiCore.Areas.Gg.ViewModels
{
    public class ESProducts : ESProduct
    {
        public IEnumerable<ESStocks> Items { get; set; }
    }
}
