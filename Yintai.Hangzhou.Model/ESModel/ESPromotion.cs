using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Yintai.Hangzhou.Model.ES
{
    public class ESPromotion
    {
        private DateTime _createdDate;
        private DateTime _startDate;
        private DateTime _endDate;

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedDate
        {
            get
            {
                return  _createdDate.ToUniversalTime();
            }
            set
            {
                _createdDate = value;
            }
        }
        public System.DateTime StartDate
        {
            get
            {
                return  _startDate.ToUniversalTime();
            }
            set
            {
                _startDate = value;
            }
        }
        public System.DateTime EndDate
        {
            get
            {
                return  _endDate.ToUniversalTime();
            }
            set
            {
                _endDate = value;
            }
        }
        public int Status { get; set; }
        public int FavoriteCount { get; set; }
        public ESStore Store { get; set; }
        public bool IsTop { get; set; }
        public virtual IEnumerable<ESResource> Resource { get; set; }
        public int CreateUserId { get; set; }
        public bool ShowInList { get; set; }
        public string PublicCode { get; set; }
        public int LikeCount { get; set; }
        public int ShareCount { get; set; }
        public int InvolvedCount { get; set; }
        public bool IsProdBindable { get; set; }
        public int PublicationLimit { get; set; }
    }
}
