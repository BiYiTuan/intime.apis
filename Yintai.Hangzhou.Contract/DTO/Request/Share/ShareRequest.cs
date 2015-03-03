using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Contract.Request;

namespace Yintai.Hangzhou.Contract.DTO.Request.Share
{
    public class ShareCreateRequest : AuthRequest
    {
        public int SourceId { get; set; }

        public int SourceType { get; set; }

        /// <summary>
        /// 分享理由
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 分享类型
        /// </summary>
        public int ShareType { get; set; }

        /// <summary>
        /// 外站类型
        /// </summary>
        public int OutSiteType { get; set; }
    }

    public class ShareListRequest : AuthRequest
    {
    }
}
