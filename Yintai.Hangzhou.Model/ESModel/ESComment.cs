using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Yintai.Hangzhou.Model.ES
{
    public class ESComment
    {
        public int Id { get; set; }
        public int SourceType { get; set; }
        public int SourceId { get; set; }
        public IEnumerable<ESResource> Resource { get; set; }
        public int CreateUserId { get; set; }
        public DateTime CreatedDate {get;set;}
        public string TextMsg { get; set; }
        public int Status { get; set; }
        public int ParentCommentId { get; set; }
        public int ParentCommentUserId { get; set; }

    }
}
