using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Data.Models;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IShareService
    {
        /// <summary>
        /// 创建一个分享
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ShareHistoryEntity Create(ShareHistoryEntity entity);
    }
}
