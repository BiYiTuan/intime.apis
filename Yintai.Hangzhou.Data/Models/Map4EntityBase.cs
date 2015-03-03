using System;
using System.ComponentModel.DataAnnotations;

namespace Yintai.Hangzhou.Data.Models
{
    public abstract class Map4EntityBase : Architecture.Common.Models.BaseEntity
    {
        public int Id { get; set; }

        public int? ChannelId { get; set; }

        [MaxLength(50)]
        public string Channel { get; set; }

        public DateTime UpdateDate { get; set; }

        public DateTime CreateDate { get; set; }

        public override object EntityId
        {
            get { return this.Id; }
        }
    }
}