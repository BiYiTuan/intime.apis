using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Yintai.Architecture.ImageTool.Contract;
using Yintai.Architecture.ImageTool.Models;

namespace Yintai.Architecture.ImageClient
{
    public partial class ImageServiceClient : ClientBase<IImageService>, IImageService
    {
        #region .ctor

        public ImageServiceClient()
            : this("ImageServiceEndpoint")
        {
        }

        public ImageServiceClient(string endpointConfigurationName)
            : base(endpointConfigurationName)
        {
        }

        public ImageServiceClient(string endpointConfigurationName, string remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ImageServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
            : base(endpointConfigurationName, remoteAddress)
        {
        }

        public ImageServiceClient(Binding binding, EndpointAddress remoteAddress)
            : base(binding, remoteAddress)
        {
        }

        #endregion

        public void UploadFile(FileUploadMessage request)
        {
            base.Channel.UploadFile(request);
        }

        public ThumbnailInfo UploadFileAndReturnInfo(FileUploadMessage request)
        {
            return base.Channel.UploadFileAndReturnInfo(request);
        }

        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, out string fileKey,
                                       out string fileExt)
        {
            return base.Channel.GetFileName(key, fileSize, clientFilePath, out fileKey, out fileExt);
        }

        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, string contentType,
                                       out string fileKey,
                                       out string fileExt)
        {
            return base.Channel.GetFileName(key, fileSize, clientFilePath, contentType, out  fileKey, out  fileExt);
        }

        public FileMessage GetFileNameByUser(string key, int fileSize, string clientFilePath, string contentType,
                                             out string fileKey,
                                             out string fileExt, string[] userFolders)
        {
            return base.Channel.GetFileNameByUser(key, fileSize, clientFilePath, contentType,
                                                  out fileKey,
                                                  out fileExt, userFolders);
        }

        public bool IsExistingFile(string key, string fileName, string fileExt)
        {
            return base.Channel.IsExistingFile(key, fileName, fileExt);
        }

        public bool HandleGroupPortrait(string fileName, string url)
        {
            return base.Channel.HandleGroupPortrait(fileName, url);
        }

        public bool DeleteFile(string key, string[] fileNameList, string[] date, string[] userFolders)
        {
            return base.Channel.DeleteFile(key, fileNameList, date, userFolders);
        }

        public bool DeleteNormalFile(string[] filePaths)
        {
            return base.Channel.DeleteNormalFile(filePaths);
        }
    }
}
