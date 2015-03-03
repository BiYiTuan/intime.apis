using com.intime.fashion.service.contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.Common.Logger;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model;
using Yintai.Hangzhou.Model.Enums;

namespace com.intime.fashion.service
{
    public class AuthKeysService : BusinessServiceBase, IAuthKeysService
    {
        private ICacheService _cacheService;
        public AuthKeysService(ICacheService cacheService)
            : base()
        {
            _cacheService = cacheService;
        }
        public Yintai.Hangzhou.Model.AliPayKey GetAlipayKey(int groupId)
        {
            IEnumerable<AliPayKey> keys = _cacheService.GetList<AliPayKey>("ali_pay_key", () => {
                var linq = _db.Set<Group_AliKeysEntity>().Where(ga => ga.Status == (int)DataStatus.Normal)
                            .ToList()
                            .Select(l=> AliPayKey.FromEntity<AliPayKey>(l));
                return linq;
            });
            if (keys != null)
                return keys.Where(k => k.GroupId == groupId).FirstOrDefault();
            _log.Info(string.Format("alipay key not get:{0}", groupId));
            return null;
        }

        public Yintai.Hangzhou.Model.WeixinPayKey GetWeixinPayKey(int groupId)
        {
            IEnumerable<WeixinPayKey> keys = _cacheService.GetList<WeixinPayKey>("weixin_pay_key", () =>
            {
                var linq = _db.Set<Group_WeixinKeysEntity>().Where(ga => ga.Status == (int)DataStatus.Normal)
                            .ToList()
                            .Select(l => WeixinPayKey.FromEntity<WeixinPayKey>(l));
                return linq;
            });
            if (keys != null)
                return keys.Where(k => k.GroupId == groupId).FirstOrDefault();
            _log.Info(string.Format("weixin key not get:{0}", groupId));
            return null;
        }

    }
}
