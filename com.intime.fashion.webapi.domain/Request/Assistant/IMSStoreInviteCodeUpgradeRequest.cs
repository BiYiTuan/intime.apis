using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.intime.fashion.webapi.domain.Request.Assistant
{
    public class IMSStoreInviteCodeUpgradeRequest
    {
        [Required(ErrorMessage = "门店必选")]
        [Range(1, int.MaxValue)]
        public int StoreId { get; set; }
        [Required(ErrorMessage = "专柜编码必填")]
        [MinLength(1, ErrorMessage = "专柜编码不能为空")]
        public string SectionCode { get; set; }

        [Required(ErrorMessage = "导购编码必填")]
        [MinLength(1, ErrorMessage = "导购编码不能为空")]
        public string OperatorCode { get; set; }
        [Required(ErrorMessage = "营业部门必选")]
        [Range(1, int.MaxValue)]
        public int DepartId { get; set; }
        [Required(ErrorMessage = "联系手机必填")]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }
    }
}
