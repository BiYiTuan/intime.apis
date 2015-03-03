using System;
//using com.intime.fashion.data.sync.Taobao.Factory;
//using com.intime.fashion.data.sync.Taobao.Fetcher;
//using com.intime.fashion.data.sync.Taobao.Request;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nest;
using Yintai.Hangzhou.Model.ES;
using Yintai.Hangzhou.Model.ESModel;
using Yintai.Hangzhou.WebApiCore.Areas.Gg.Controllers;

namespace Yintai.Hangzhou.WebApiCore.Test
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public void Test()
        {
            var result = ElasticClient.Search<ESProduct>(body =>
                // return first 5 results, default is 10
                body.Size(5).Query(query =>
                    query.Term(p => p.IsSystem, false)));
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void TestStocks()
        {
            var result = ElasticClient.Search<ESStock>(body =>
                // return first 5 results, default is 10
               body.SortDescending(b=>b.UpdateDate).Size(15).Query(query =>
                   query.Term(p => p, false)));
            Assert.IsNotNull(result);
        }

        private static ElasticClient ElasticClient
        {
            get
            {
                var uriString = "http://218.244.139.79:9200/";
                var searchBoxUri = new Uri(uriString);
                var settings = new ConnectionSettings(searchBoxUri);
                settings.SetDefaultIndex("intime");
                return new ElasticClient(settings);
            }
        }

        [TestMethod]
        public void Test4Facet()
        {
            //FetchRequest request = new FetchRequest()
            //{
            //    Channel = "IMS",
            //    LastUpdateTime = DateTime.Now.AddMonths(-3),
            //    PageSize = 10,
            //    PageIndex = 1
            //};

            //var result = new IMSProductFetcher(ElasticClient).Fetch(request);

            //Assert.IsNotNull(result);
        }
    }
}
