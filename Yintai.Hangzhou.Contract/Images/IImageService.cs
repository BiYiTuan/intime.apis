using System;
using System.ServiceModel;

namespace Yintai.Hangzhou.Contract.Images
{
    /// <summary>
    /// 文件上传服务协议接口
    /// </summary>
    [ServiceContract]
    public interface IImageService
    {
        /// <summary>
        /// 文件上传方法
        /// </summary>
        /// <param name="request"></param>
        [Obsolete("此方法已经过期")]
        [OperationContract(IsOneWay = true)]
        void UploadFile(FileUploadMessage request);

        [OperationContract]
        ThumbnailInfo UploadFileAndReturnInfo(FileUploadMessage request);

        /// <summary>
        /// getname
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileSize"></param>
        /// <param name="clientFilePath"></param>
        /// <param name="fileKey"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetFileName1")]
        FileMessage GetFileName(string key, int fileSize, string clientFilePath, out string fileKey, out string fileExt);

        /// <summary>
        /// getname
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileSize"></param>
        /// <param name="clientFilePath"></param>
        /// <param name="contentType"></param>
        /// <param name="fileKey"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        [OperationContract(Name = "GetFileName2")]
        FileMessage GetFileName(string key, int fileSize, string clientFilePath, string contentType, out string fileKey,
                                out string fileExt);
        /// <summary>
        /// getname folder
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileSize"></param>
        /// <param name="clientFilePath"></param>
        /// <param name="contentType"></param>
        /// <param name="fileKey"></param>
        /// <param name="fileExt"></param>
        /// <param name="userFolders"></param>
        /// <returns></returns>
        [OperationContract]
        FileMessage GetFileNameByUser(string key, int fileSize, string clientFilePath, string contentType, out string fileKey, out string fileExt, string[] userFolders);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        [OperationContract]
        bool IsExistingFile(string key, string fileName, string fileExt);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        [OperationContract]
        bool HandleGroupPortrait(string fileName, string url);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileNameList"></param>
        /// <param name="date"></param>
        /// <param name="userFolders"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteFile(string key, string[] fileNameList, string[] date, string[] userFolders);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePaths"></param>
        /// <returns></returns>
        [OperationContract]
        bool DeleteNormalFile(string[] filePaths);
    }
}
