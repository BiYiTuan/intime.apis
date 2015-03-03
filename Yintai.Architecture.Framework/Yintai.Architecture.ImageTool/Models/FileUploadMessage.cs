using System.IO;
using System.ServiceModel;

namespace Yintai.Architecture.ImageTool.Models
{
    /// <summary>
    /// 文件传送对象
    /// </summary>
    [MessageContract]
    public class FileUploadMessage
    {
        /// <summary>
        /// 文件名
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public string FileName;

        /// <summary>
        /// 文件类型
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public string FileExt;

        /// <summary>
        /// 项目键名
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public string KeyName;

        /// <summary>
        /// 是否保存原始文件
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public bool SaveOrigin;

        /// <summary>
        /// 是否转换为JPG文件
        /// </summary>
        [MessageHeader(MustUnderstand = true)]
        public bool ConvertToJPG;

        /// <summary>
        /// 文件流
        /// </summary>
        [MessageBodyMember(Order = 1)]
        public Stream FileData;

    }

}
