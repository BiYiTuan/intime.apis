using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Architecture.Common.Models;
using Yintai.Hangzhou.Contract.DTO.Request.Share;

namespace Yintai.Hangzhou.Contract.Share
{
    public interface IShareDataService
    {
        ExecuteResult Create(ShareCreateRequest request);
    }
}
