
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.Framework.ServiceLocation;
﻿using Yintai.Hangzhou.Contract.Images;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Yintai.Hangzhou.Service.Manager
{
    public class ImageServiceClientProxy : IImageService
    {
        private readonly IImageService _imageService;

        public ImageServiceClientProxy()
            : this(new ImageServiceClient("ImageServiceEndpoint"))
        {
        }

        public ImageServiceClientProxy(IImageService imageService)
        {
            this._imageService = imageService;
        }

        [System.Obsolete("该方法已经过期")]
        public void UploadFile(FileUploadMessage request)
        {
            _imageService.UploadFile(request);
        }

        public ThumbnailInfo UploadFileAndReturnInfo(FileUploadMessage request)
        {
            return _imageService.UploadFileAndReturnInfo(request);
        }

        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, out string fileKey, out string fileExt)
        {
            return _imageService.GetFileName(key, fileSize, clientFilePath, out fileKey, out fileExt);
        }

        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, string contentType, out string fileKey, out string fileExt)
        {
            return _imageService.GetFileName(key, fileSize, clientFilePath, contentType, out fileKey, out fileExt);
        }

        public FileMessage GetFileNameByUser(string key, int fileSize, string clientFilePath, string contentType, out string fileKey, out string fileExt,
                                             string[] userFolders)
        {
            return _imageService.GetFileNameByUser(key, fileSize, clientFilePath, contentType, out fileKey, out fileExt,
                                                   userFolders);
        }

        public bool IsExistingFile(string key, string fileName, string fileExt)
        {
            return
                _imageService.IsExistingFile(key, fileName, fileExt);
        }

        public bool HandleGroupPortrait(string fileName, string url)
        {
            return _imageService.HandleGroupPortrait(fileName, url);
        }

        public bool DeleteFile(string key, string[] fileNameList, string[] date, string[] userFolders)
        {
            return _imageService.DeleteFile(key, fileNameList, date, userFolders);
        }

        public bool DeleteNormalFile(string[] filePaths)
        {
            return _imageService.DeleteNormalFile(filePaths);
        }
    }

    public class FileUploadServiceManager
    {
        #region fields

        private static readonly IImageService _imageService;

        private static readonly FileUploadServiceManager _instance;

        private static readonly ILog _logger;

        //private static readonly object _syncObj = new object();

        #endregion

        #region .ctor

        private FileUploadServiceManager()
        {
            //if (_imageService == null)
            //{
            //    lock (_syncObj)
            //    {
            //        if (_imageService == null)
            //        {
            //            _imageService = new ImageServiceClient();
            //        }
            //    }
            //}
        }

        static FileUploadServiceManager()
        {
            _instance = new FileUploadServiceManager();
            _imageService = new ImageServiceClientProxy();
            _logger = ServiceLocator.Current.Resolve<ILog>();
        }

        #endregion

        #region properties

        public static FileUploadServiceManager Instance
        {
            get { return _instance; }
        }

        #endregion

        #region methods

        private static string ContentTypeGetExt(string contentType)
        {
            return ContentType.GetExt(contentType);
        }

        private static ResourceType ExtGetResourceType(string ext)
        {
            return ContentType.GetResourceType(ext);
        }

        private static void GetSize(HttpPostedFileBase postedFile, out int width, out int height)
        {
            postedFile.InputStream.Position = 0L;

            using (var image = System.Drawing.Image.FromStream(postedFile.InputStream))
            {
                width = image.Width;
                height = image.Height;
            }

            postedFile.InputStream.Position = 0L;
        }

        /// <summary>
        /// 存储图片
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="fileUploadMessage"></param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        private static FileMessage SaveImage(HttpPostedFileBase postedFile, FileUploadMessage fileUploadMessage, out int width, out int height)
        {
            var client = _imageService;
            width = height = 0;
            try
            {
                if (postedFile.InputStream.CanRead)
                {
                    System.Drawing.Image image = System.Drawing.Image.FromStream(postedFile.InputStream);
                    width = image.Width;
                    height = image.Height;
                    //byte[] content = new byte[postedFile.ContentLength + 1];
                    //postedFile.InputStream.Read(content, 0, postedFile.ContentLength);
                    //inValue.FileData = new MemoryStream(content);
                    fileUploadMessage.FileData = postedFile.InputStream;
                    client.UploadFileAndReturnInfo(fileUploadMessage);
                    fileUploadMessage.FileData.Close();
                    fileUploadMessage.FileData.Dispose();
                }
                else
                {
                    _logger.Warn("图片文件不能读取");

                    return FileMessage.UnknowError;
                }

                return FileMessage.Success;
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return FileMessage.UnknowError;
            }
        }

        /// <summary>
        /// 存储声音文件
        /// </summary>

        /// <param name="fileUploadMessage"></param>
        /// <returns></returns>
        private static FileMessage SaveSound(FileUploadMessage fileUploadMessage, out long Length)
        {
            var client = _imageService;
            var t = client.UploadFileAndReturnInfo(fileUploadMessage);
            Length = 0;
            if (t != null)
            {
                if (t.Sizes != null)
                {
                    ImageSize v;
                    if (t.Sizes.TryGetValue(fileUploadMessage.KeyName, out v))
                    {
                        Length = v.Length;
                    }
                }
            }

            fileUploadMessage.FileData.Close();
            fileUploadMessage.FileData.Dispose();

            return FileMessage.Success;
        }

        #endregion

        #region 文件上传


        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="postedFile"></param>
        /// <param name="key"></param>
        /// <param name="fileInfor"></param>
        /// <param name="userFolder"></param>
        /// <returns></returns>
        public static FileMessage UploadFile(HttpPostedFileBase postedFile, string key, out FileInfor fileInfor,
                                             string[] userFolder)
        {
            var client = _imageService;
            string fileKey;
            string fileExt;
            fileInfor = new FileInfor();

            FileMessage f = client.GetFileNameByUser(key, postedFile.ContentLength, postedFile.FileName, postedFile.ContentType, out fileKey, out fileExt, userFolder);

            if (f == FileMessage.Success)
            {
                fileInfor.FileName = fileKey;
                fileInfor.FileSize = postedFile.ContentLength;
                fileInfor.FileExtName = fileExt;
                fileInfor.ResourceType = ContentType.GetResourceType(fileExt);

                try
                {
                    if (postedFile.InputStream.CanRead)
                    {
                        FileUploadMessage inValue = new FileUploadMessage();
                        inValue.FileName = fileKey;
                        inValue.KeyName = key;
                        inValue.FileExt = fileExt;
                        inValue.SaveOrigin = false;
                        inValue.FileData = postedFile.InputStream;

                        switch (fileInfor.ResourceType)
                        {
                            case ResourceType.Sound:
                                long length;
                                var t = SaveSound(inValue, out  length);
                                fileInfor.Length = length;
                                fileInfor.Width = (int)length;
                                return t;
                        }
                        //byte[] content = new byte[postedFile.ContentLength + 1];
                        //postedFile.InputStream.Read(content, 0, postedFile.ContentLength);
                        //inValue.FileData = new MemoryStream(content);
                        //获取
                        int width, height;
                        GetSize(postedFile, out width, out height);
                        fileInfor.Width = width;
                        fileInfor.Height = height;

                        //client.UploadFileAndReturnInfo(inValue);
                        client.UploadFile(inValue);
                        inValue.FileData.Close();
                        inValue.FileData.Dispose();
                    }
                    else
                    {
                        return FileMessage.UnknowError;
                    }

                    return FileMessage.Success;
                }
                catch
                {
                    return FileMessage.UnknowError;
                }
            }

            return f;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="postedFile">上传的文件对象</param>
        /// <param name="key">服务端配置索引名</param>
        /// <param name="fileInfor">文件上传后返回的信息对象</param>
        /// <returns></returns>
        public static FileMessage UploadFile(HttpPostedFileBase postedFile, string key, out FileInfor fileInfor)
        {
            return UploadFile(postedFile, key, out fileInfor, new string[0]);
        }

        /// <summary>
        /// 上传文件并返回缩略图大小a
        /// </summary>
        /// <param name="postedFile">上传的文件对象</param>
        /// <param name="key">服务端配置索引名</param>
        /// <param name="fileInfor">文件上传后返回的信息对象</param>
        /// <param name="thumbnailInfo">缩略图信息</param>
        /// <returns></returns>
        public static FileMessage UploadFileAndReturnInfo(HttpPostedFileBase postedFile, string key, out FileInfor fileInfor, string[] userFolder, out ThumbnailInfo thumbnailInfo)
        {
            var client = _imageService;
            string fileKey;
            string fileExt;
            fileInfor = new FileInfor();
            thumbnailInfo = new ThumbnailInfo();

            FileMessage f = client.GetFileNameByUser(key, postedFile.ContentLength, postedFile.FileName, postedFile.ContentType, out fileKey, out fileExt, userFolder);

            if (f == FileMessage.Success)
            {
                fileInfor.FileName = fileKey;
                fileInfor.FileSize = postedFile.ContentLength;
                fileInfor.FileExtName = fileExt;
                fileInfor.ResourceType = ContentType.GetResourceType(fileExt);

                try
                {
                    FileUploadMessage inValue = new FileUploadMessage();
                    inValue.FileName = fileKey;
                    inValue.KeyName = key;
                    inValue.FileExt = fileExt;
                    inValue.SaveOrigin = false;

                    if (postedFile.InputStream.CanRead)
                    {
                        //byte[] content = new byte[postedFile.ContentLength + 1];
                        //postedFile.InputStream.Read(content, 0, postedFile.ContentLength);
                        //inValue.FileData = new MemoryStream(content);
                        inValue.FileData = postedFile.InputStream;

                        switch (fileInfor.ResourceType)
                        {
                            case ResourceType.Sound:
                                long length;
                                var t = SaveSound(inValue, out  length);
                                fileInfor.Length = length;
                                fileInfor.Width = (int)length;

                                LoggerManager.Current().Warn(fileInfor.Width);

                                return t;
                        }

                        int width, height;
                        GetSize(postedFile, out width, out height);
                        fileInfor.Width = width;
                        fileInfor.Height = height;

                        thumbnailInfo = client.UploadFileAndReturnInfo(inValue);
                        inValue.FileData.Close();
                        inValue.FileData.Dispose();
                    }
                    else
                    {
                        return FileMessage.UnknowError;
                    }

                    return FileMessage.Success;
                }
                catch
                {
                    return FileMessage.UnknowError;
                }
            }
            else
            {
                return f;
            }
        }

        /// <summary>
        /// 文件集合上传
        /// </summary>
        /// <param name="key">服务端索配置引名</param>
        /// <param name="fileCollection">文件集合</param>
        /// <param name="AutoSize">允许上传的集合大小</param>
        /// <returns></returns>
        public static List<FileInfor> UploadFileCollection(string key, HttpFileCollection fileCollection, int autoSize)
        {
            HttpFileCollectionBase obj = new HttpFileCollectionWrapper(fileCollection);

            return UploadFileCollection(key, obj, autoSize);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileCollection"></param>
        /// <param name="autoSize"></param>
        /// <param name="thumbnailInfoes"></param>
        /// <returns></returns>
        public static List<FileInfor> UploadFileCollectionAndReturnInfo(string key, HttpFileCollection fileCollection, int autoSize, out IList<ThumbnailInfo> thumbnailInfoes)
        {
            HttpFileCollectionBase obj = new HttpFileCollectionWrapper(fileCollection);

            return UploadFileCollectionAndReturnInfo(key, obj, autoSize, out thumbnailInfoes);
        }

        /// <summary>
        /// 文件集合上传
        /// </summary>
        /// <param name="key">服务端索配置引名</param>
        /// <param name="fileCollection">文件集合</param>
        /// <param name="AutoSize">允许上传的集合大小</param>
        /// <returns></returns>
        public static List<FileInfor> UploadFileCollection(string key, HttpFileCollectionBase fileCollection, int autoSize)
        {
            // TODO: AutoSize应使用Camel命名法

            List<FileInfor> fileList = new List<FileInfor>();

            for (int i = 0; i < fileCollection.Count; i++)
            {
                if (fileCollection[i].FileName.Length < 5)
                    continue;
                if (fileCollection[i].ContentLength > autoSize)
                {
                    continue;
                }

                FileInfor fileInfor;
                var f = UploadFile(fileCollection[i], key, out fileInfor);

                if (f == FileMessage.Success)
                {
                    autoSize -= fileCollection[i].ContentLength;
                    fileList.Add(fileInfor);
                }
            }
            return fileList;
        }

        /// <summary>
        /// 批量文件上传并返回缩略图信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileCollection"></param>
        /// <param name="autoSize"></param>
        /// <param name="thumbnailInfoes"></param>
        /// <returns></returns>
        public static List<FileInfor> UploadFileCollectionAndReturnInfo(string key, HttpFileCollectionBase fileCollection, int autoSize, out IList<ThumbnailInfo> thumbnailInfoes)
        {
            // TODO: AutoSize应使用Camel命名法

            List<FileInfor> fileList = new List<FileInfor>();
            thumbnailInfoes = new List<ThumbnailInfo>();

            for (int i = 0; i < fileCollection.Count; i++)
            {
                if (fileCollection[i].FileName.Length < 5)
                {
                    continue;
                }
                if (fileCollection[i].ContentLength > autoSize)
                {
                    continue;
                }

                FileInfor fileInfor;
                ThumbnailInfo info;
                var f = UploadFileAndReturnInfo(fileCollection[i], key, out fileInfor, new string[0], out info);
                if (f == FileMessage.Success)
                {
                    autoSize -= fileCollection[i].ContentLength;
                    fileList.Add(fileInfor);
                    thumbnailInfoes.Add(info);
                }
            }
            return fileList;
        }

        #endregion
        /// <summary>
        /// restricted to upload image file , for staging purpose
        /// </summary>
        /// <param name="file"></param>
        /// <param name="key"></param>
        /// <param name="fileInfor"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static FileMessage UploadFile(FileInfo file, string key, out FileInfor fileInfor, string p2)
        {
            var client = _imageService;
            string fileKey;
            string fileExt;
            fileInfor = new FileInfor();
            FileMessage f = client.GetFileName(key, (int)file.Length, file.Name, file.Extension, out fileKey, out fileExt);

            if (f == FileMessage.Success)
            {
                fileInfor.FileName = fileKey;
                fileInfor.FileSize = (int)file.Length;
                fileInfor.FileExtName = fileExt;
                fileInfor.ResourceType = ContentType.GetResourceType(fileExt);


                try
                {
                    FileUploadMessage inValue = new FileUploadMessage();
                    inValue.FileName = fileKey;
                    inValue.KeyName = key;
                    inValue.FileExt = fileExt;
                    inValue.SaveOrigin =false;


                    using (var fileHR = file.OpenRead())
                    {
                        inValue.FileData = fileHR;

                        int width, height;
                        using (var image = System.Drawing.Image.FromStream(fileHR))
                        {
                            width = image.Width;
                            height = image.Height;
                        }
                        fileHR.Seek(0, SeekOrigin.Begin);
                        fileInfor.Width = width;
                        fileInfor.Height = height;

                        client.UploadFileAndReturnInfo(inValue);
                        fileHR.Close();
                    }


                    return FileMessage.Success;
                }
                catch
                {
                    return FileMessage.UnknowError;
                }
            }
            else
            {
                return f;
            }
        }

        internal static bool DeletedFile(string key, string[] fileNameList, string[] userFolders)
        {
            var client = _imageService;

            return client.DeleteFile(key, fileNameList, null, userFolders);
        }

    }

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

