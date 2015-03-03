using Yintai.Architecture.Common.Models;

namespace Yintai.Hangzhou.Contract.Images
{
    /// <summary>
    /// 文件对象
    /// </summary>
    public class FileInfor
    {
        /// <summary>
        /// 扩展名
        /// </summary>
        public string FileExtName { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get;
            set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public int FileSize
        {
            get;
            set;
        }

        /// <summary>
        /// 宽
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// 资源类型
        /// </summary>
        public ResourceType ResourceType { get; set; }

        /// <summary>
        /// 长度（秒为单位）
        /// </summary>
        public long Length { get; set; }
    }
}