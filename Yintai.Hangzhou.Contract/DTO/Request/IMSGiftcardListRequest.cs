using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Yintai.Hangzhou.Contract.DTO.Request
{
    public class IMSGiftcardListRequest:PagerInfoRequest
    {
        private int page = 1;
        private int pageSize = 20;
        public override int Page
        {
            get
            {
                return page;
            }
            set
            {
                page = value;
            }
        }

        public override int Pagesize {
            get { return pageSize; }
            set
            {
                if (value > 40 || value <= 0)
                {
                    pageSize = 20;
                }
                else
                    pageSize = value;
            } 
        }
    }
}
