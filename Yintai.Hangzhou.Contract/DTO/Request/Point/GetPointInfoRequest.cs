using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Contract.DTO.Request.Point
{
    public class GetPointInfoRequest : AuthRequest
    {
        public int PointId { get; set; }
    }

    public class GetListPointCollectionRequest : AuthPagerInfoRequest
    {
        public int Sort { get; set; }

        public PointSortOrder SortOrder
        {
            get { return (PointSortOrder)Sort; }
        }
    }
}
