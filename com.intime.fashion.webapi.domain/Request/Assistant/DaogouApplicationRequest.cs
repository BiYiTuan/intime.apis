
namespace com.intime.fashion.webapi.domain.Request.Assistant
{
    public class DaogouApplicationRequest : AuthRequest
    {
        /// <summary>
        /// 申请类型, 0 表示普通用户， 1表示导购
        /// </summary>
        public int ApplyType { get; set; }
        public int StoreId { get; set; }

        public string SectionName { get; set; }

        public string SectionCode { get; set; }

        public string OperatorCode { get; set; }

        public string Phone { get; set; }

        public string Name { get; set; }

        public string IdCard { get; set; }

    }
}
