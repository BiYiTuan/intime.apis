using System;
namespace com.intime.fashion.service.contract
{
    public interface IAuthKeysService
    {
        Yintai.Hangzhou.Model.AliPayKey GetAlipayKey(int groupId);
        Yintai.Hangzhou.Model.WeixinPayKey GetWeixinPayKey(int groupId);
    }
}
