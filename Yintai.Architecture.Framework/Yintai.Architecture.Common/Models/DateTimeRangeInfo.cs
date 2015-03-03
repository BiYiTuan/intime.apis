using System;

namespace Yintai.Architecture.Common.Models
{
    [Serializable]
    public class DateTimeRangeInfo
    {
        public DateTime? StartDateTime { get; set; }

        public DateTime? EndDateTime { get; set; }
    }
}