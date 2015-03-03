using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Yintai.Hangzhou.Data.Models
{
    public enum ProUploadStatus
    {
        [Description("已上传到服务器本地")]
        ProductsOnDisk = 0,
        [Description("已上传到数据库")]
        ProductsOnStage = 1,
        [Description("数据校验失败")]
        ProductsValidateFailed = 2,
        [Description("数据校验通过(无图片)")]
        ProductsValidateNoImageSuccess = 3,
        [Description("数据校验通过")]
        ProductsValidateWithImageSuccess = 4,
        [Description("发布成功")]
        ProductsOnLive = 5
    }

    public class ProductUploadInfo
    {
        [DisplayName("商品编码")]
        public string ItemCode { get; set; }
        [DisplayName("商品名称")]
        public string Title { get; set; }
        [DisplayName("商品描述")]
        public string Descrip { get; set; }
        [DisplayName("促销描述")]
        public string DescripOfPromotion { get; set; }
        [DisplayName("促销开始时间")]
        [DataType(DataType.DateTime)]
        public DateTime? DescripOfPromotionBeginDate { get; set; }
        [DisplayName("促销结束时间")]
        [DataType(DataType.DateTime)]
        public DateTime? DescripOfPromotionEndDate { get; set; }
        [DisplayName("品牌名")]
        [UIHint("Association")]
        [AdditionalMetadata("controller","brand")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        public string Brand { get; set; }
        [DisplayName("Tag名")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "tag")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        public string Tag { get; set; }
        [DisplayName("销售价")]
        public decimal? Price { get; set; }
        [DisplayName("实体店名")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "store")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("searchfield", "name")]
        public string Store { get; set; }
        [DisplayName("绑定促销IDs")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "promotion")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("valuefield", "Id")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("multiple", "true")]
        public string PromotionIds { get; set; }
        [DisplayName("绑定主题IDs")]
        [UIHint("Association")]
        [AdditionalMetadata("controller", "specialtopic")]
        [AdditionalMetadata("displayfield", "Name")]
        [AdditionalMetadata("valuefield", "Id")]
        [AdditionalMetadata("searchfield", "name")]
        [AdditionalMetadata("multiple","true")]
        public string SubjectIds { get; set; }
        [DisplayName("商品货号")]
        public string UPCCode { get; set; }
        public int Id { get; set; }
        public int GroupId { get; set; }
        public IEnumerable<ProductPropertyStageEntity> Properties { get; set; }
        public string PropertiesDisplay { get {
            if (Properties == null)
                return string.Empty;
           return Properties.OrderBy(p => p.PropertyDesc).Aggregate(new StringBuilder(),
                (s, v) => s.AppendFormat("{0}:{1} ", v.PropertyDesc, v.ValueDesc),
                s => s.ToString());

        } }

    }
    public class ProductUploadJob
    {
        public int JobId { get; set; }
        public DateTime? InDate { get; set; }
        public ProUploadStatus Status { get; set; }
    }
    public class ImageUploadInfo
    {
        public string ItemCode { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string FileName { get; set; }
        public string fileSize { get; set; }

    }
    public class ProductValidateResult
    {
        public string ItemCode { get; set; }
        public string ValidateResult { get; set; }
        public ProUploadStatus ResultStatus { get; set; }
    }
    public class ProductPublishResult
    {
        public string ItemCode { get; set; }
        public ProUploadStatus Status { get; set; }
        public string PublishMemo { get; set; }

    }
}
