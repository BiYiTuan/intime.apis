using com.intime.fashion.common.config;
using com.intime.fashion.service.analysis.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Yintai.Architecture.Framework.ServiceLocation;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Model.ESModel;

namespace com.intime.fashion.service.analysis
{
    public class AnalysisService:BusinessServiceBase
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private HttpClient _client;

        public AnalysisService()
        {
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        
        }
        public void SetDate(DateTime startDate,DateTime endDate)
        {
            _startDate = startDate;
            _endDate = endDate;
        }
        

        public ESAnalysisSummary GetSummary()
        {
            ESAnalysisSummary summary = new ESAnalysisSummary();
            summary.App = CommonConfiguration<AnalysisConfiguration>.Current.AppName;
            summary.StarDate = _startDate;
            summary.EndDate = _endDate;
            var requests = new Dictionary<SummaryRequestBase,Action<SummaryResponse>>();
            requests.Add(new SummaryNewUserRequest() { StartDate = _startDate,EndDate = _endDate}, res => summary.NewUsers = res.Count);
            requests.Add(new SummaryActiveUserRequest() { StartDate = _startDate, EndDate = _endDate }, res => summary.ActiveUsers = res.Count);
            requests.Add(new SummarySessionsRequest() { StartDate = _startDate, EndDate = _endDate }, res => summary.Sessions = res.Count);
            foreach (var request in requests)
            {
                SendHttpSummaryRequest(request.Key)
                   .ContinueWith(response => {
                       request.Value(response.Result);
                   })
                   .Wait();
                ThreholdControl();

            }
            return summary;
        }

        public IEnumerable<ESAnalysisEvent> GetComboEvent()
        {
            var request = new ComboDetailEventRequest() { StartDate = _startDate, EndDate = _endDate };
            var response = new List<ESAnalysisEvent>();
            SendHttpEventRequest<ComboDetailEventResponse>(request)
                .ContinueWith(res=>{
                    var items = res.Result.Items;
                    if (items == null)
                        return;
                    foreach (var item in res.Result.Items)
                    {
                        int comboId = int.Parse(item.Id);
                        var associateEntity = _db.Set<IMS_AssociateItemsEntity>()
                            .Where(ia => ia.ItemId == comboId && ia.ItemType == (int)ComboType.Product)
                            .Join(_db.Set<IMS_AssociateEntity>(), o => o.AssociateId, i => i.Id, (o, i) => i)
                            .FirstOrDefault();
                        if (associateEntity == null)
                        {
                            _log.Info(string.Format("anaysis combo event not found:{0}", comboId));
                            continue;
                        }
                        response.Add(new ESAnalysisEvent()
                        {
                            EventId = item.Id,
                            Count = item.Count,
                            SectionId = associateEntity.SectionId,
                            StoreId = associateEntity.StoreId,
                            AssociateId = associateEntity.Id,
                            StartDate = _startDate,
                            EndDate = _endDate,
                            EventType = (int)ESEventType.Combo
                        });
                    }
                })
                .Wait();
            return response;
        }

        public IEnumerable<ESAnalysisEvent> GetGiftCardEvent()
        {

            var request = new GiftCardDetailEventRequest() { StartDate = _startDate, EndDate = _endDate };
            var response = new List<ESAnalysisEvent>();
            SendHttpEventRequest<GiftCardDetailEventResponse>(request)
                .ContinueWith(res =>
                {
                    var items = res.Result.Items;
                    if (items == null)
                        return;
                    foreach (var item in res.Result.Items)
                    {
                        int giftId = int.Parse(item.Id);
                        var associateEntity = _db.Set<IMS_AssociateEntity>()
                            .Find(giftId);
                        if (associateEntity == null)
                        {
                            _log.Info(string.Format("anaysis giftcard event not found:{0}", giftId));
                            continue;
                        }
                        response.Add(new ESAnalysisEvent()
                        {
                            EventId = item.Id,
                            Count = item.Count,
                            SectionId = associateEntity.SectionId,
                            StoreId = associateEntity.StoreId,
                            AssociateId = associateEntity.Id,
                            StartDate = _startDate,
                            EndDate = _endDate,
                            EventType = (int)ESEventType.GiftCard
                        });
                    }
                })
                .Wait();
            return response;
        }

        public IEnumerable<ESAnalysisEvent> GetStoreEvent()
        {
            var request = new StoreDetailEventRequest() { StartDate = _startDate, EndDate = _endDate };
            var response = new List<ESAnalysisEvent>();
            SendHttpEventRequest<StoreDetailEventResponse>(request)
                .ContinueWith(res =>
                {
                    var items = res.Result.Items;
                    if (items == null)
                        return;
                    foreach (var item in res.Result.Items)
                    {
                        int storeId = int.Parse(item.Id);
                        var associateEntity = _db.Set<IMS_AssociateEntity>().Find(storeId);
                        if (associateEntity == null)
                        {   
                            _log.Info(string.Format("anaysis store event not found:{0}", storeId));
                            continue;
                        }
                        response.Add(new ESAnalysisEvent()
                        {
                            EventId = item.Id,
                            Count = item.Count,
                            SectionId = associateEntity.SectionId,
                            StoreId = associateEntity.StoreId,
                            AssociateId = associateEntity.Id,
                            StartDate = _startDate,
                            EndDate = _endDate,
                            EventType = (int)ESEventType.Store
                        });
                    }
                })
                .Wait();
            return response;
        }

        public void ThreholdControl()
        {
            Thread.Sleep(TimeSpan.FromSeconds(5));
        }

        private async Task<SummaryResponse> SendHttpSummaryRequest(SummaryRequestBase request) 
        {
            var response = _client.GetStringAsync(request.Url);
            var strObject = await response;
            return new SummaryResponse(JsonConvert.DeserializeObject<dynamic>(strObject));
                            
           
        }

        private async Task<T> SendHttpEventRequest<T>(EventRequestBase request) where T:EventResponse,new()
        {
            var response = _client.GetStringAsync(request.Url);
            var strObject = await response;
            return new T().SetContent(JsonConvert.DeserializeObject<dynamic>(strObject));

        }
    }
}
