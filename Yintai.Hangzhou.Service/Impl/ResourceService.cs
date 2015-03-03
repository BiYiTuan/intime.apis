
﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Web;
using Yintai.Architecture.Common.Web;
using Yintai.Hangzhou.Contract.Images;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;
using Yintai.Hangzhou.Repository.Contract;
using Yintai.Hangzhou.Service.Manager;

namespace Yintai.Hangzhou.Service.Impl
{
    public class ResourceService : BaseService, Contract.IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }

        #region methods

        /// <summary>
        /// 获取指定文件夹
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        private static string[] GetFolder(int sourceId,
                                       SourceType sourceType)
        {
            switch (sourceType)
            {
                case SourceType.BrandLogo:
                case SourceType.CustomerPortrait:
                case SourceType.CustomerThumbBackground:
                case SourceType.StoreLogo:
                    var useridstr = sourceId.ToString(CultureInfo.InvariantCulture);
                    var n = String.Empty;
                    if (useridstr.Length < 6)
                    {
                        for (var i = 0; i < (6 - useridstr.Length); i++)
                        {
                            n += "0";
                        }

                        useridstr = n + useridstr;
                    }

                    var folders = new string[2];
                    folders[0] = useridstr.Substring(0, 3);
                    folders[1] = useridstr.Substring(3, 3);

                    return folders;
                default:
                    return new string[0];
            }
        }

        #endregion


        private ResourceEntity InnerSave(HttpPostedFileBase files, string key, int sourceId, SourceType sourceType, 
            int createdUid, int count, int colorPId)
        {
            FileInfor fileInfor;
            var fileuploadResult = FileUploadServiceManager.UploadFile(files, key, out fileInfor, GetFolder(sourceId, sourceType));
            if (fileuploadResult == FileMessage.Success)
            {
                var entity = InnerSave(createdUid, colorPId, fileInfor, count, sourceId, sourceType);
                //set whether current image is product dimension
                if (sourceType == SourceType.Product &&
                    files.FileName.Contains("@cc."))
                    entity.IsDimension = true;
                return entity;
            }
            else
            {
                Logger.Error(fileuploadResult);

                return null;
            }
        }

        private ResourceEntity InnerSave(int userid, int  colorPId, FileInfor fileInfor, int sortOrder, int sourceId, SourceType sourceType)
        {

            //存储到数据库
            var entity = new ResourceEntity
            {
                CreatedDate = DateTime.Now,
                CreatedUser = userid,
                IsDefault = false,
                Name = fileInfor.FileName, //+ "." + fileInfor.FileExtName,//这里要注意
                ExtName = fileInfor.FileExtName,
                Width = fileInfor.Width,
                Height = fileInfor.Height,
                ContentSize = fileInfor.FileSize,
                Size = fileInfor.Width.ToString(CultureInfo.InvariantCulture) + "x" + fileInfor.Height.ToString(CultureInfo.InvariantCulture),
                SortOrder = sortOrder,
                SourceId = sourceId,
                SourceType = (int)sourceType,
                Status = (int)DataStatus.Normal,
                Type = (int)fileInfor.ResourceType,
                UpdatedDate = DateTime.Now,
                UpdatedUser = userid,
                Domain = String.Empty,
                IsDimension = false,
                ColorId = colorPId
            };

            return entity;
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="files">文件流</param>
        /// <param name="createdUid">created userid</param>
        /// <param name="colorPId"></param>
        /// <param name="sourceId">来源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        public List<ResourceEntity> Save(HttpFileCollectionBase files, int createdUid, int colorPId, int sourceId,
                                         SourceType sourceType)
        {
            if (files == null || files.Count == 0)
            {
                return new List<ResourceEntity>(0);
            }

            var list = new List<ResourceEntity>(files.Count);
            var count = files.Count;
            //检查扩展名
            foreach (string upload in files)
            {
                if (!files[upload].HasFile())
                {
                    Logger.Debug(string.Format("{0} no file data",upload));
                    continue;
                }

                /*
                 *  1.获取存储的文件名
                 *  2.存储文件
                 *  3.存储到数据库
                 *  4.返回
                //*/
                FileInfor fileInfor;
                FileMessage fileuploadResult;
                
                
                var fileExt = files[upload].FileName.Substring(files[upload].FileName.LastIndexOf(".", System.StringComparison.Ordinal) + 1).ToLower();

                switch (sourceType)
                {
                    case SourceType.Product:
                    case SourceType.Promotion:
                        //Logger.Warn("switch:" + upload + " ext:" + fileExt + ",ct:" + files[upload].ContentType);
                        if (files[upload].ContentType.IndexOf("audio/x-m4a", StringComparison.OrdinalIgnoreCase) > -1 || fileExt.IndexOf("m4a", System.StringComparison.OrdinalIgnoreCase) > -1 || upload.LastIndexOf("audio", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            var entity2 = InnerSave(files[upload], (sourceType.ToString() + "audio").ToLower(), sourceId, sourceType, createdUid,
          count, colorPId);

                            if (entity2 != null)
                                list.Add(entity2);

                            continue;
                        }
                        break;
                    default:

                        break;
                }

                var entity = InnerSave(files[upload], sourceType.ToString().ToLower(), sourceId, sourceType, createdUid,
count--, colorPId);

                if (entity != null)
                    list.Add(entity);

            }

            if (list.Count > 0)
            {
                return _resourceRepository.Insert(list);
            }

            return new List<ResourceEntity>(0);
        }
        /// <summary>
        /// implement the saveStage
        /// </summary>
        /// <param name="file"></param>
        /// <param name="sourceType"></param>
        /// <returns></returns>
        public FileInfor SaveStage(FileInfo file, int createUser, SourceType sourceType)
        {
            if (file == null)
                return null;


            FileInfor fileInfor;

            var fileuploadResult = FileUploadServiceManager.UploadFile(file, sourceType.ToString().ToLower(),
                                                        out fileInfor, string.Empty);

            if (fileuploadResult == FileMessage.Success)
            {

                return fileInfor;
            }
            else
                return null;


        }

        public ResourceEntity Get(int resourceId)
        {
            return _resourceRepository.GetItem(resourceId);
        }

        public List<ResourceEntity> Get(int sourceId, SourceType sourceType)
        {
            return _resourceRepository.GetList((int)sourceType, sourceId);
        }

        public ResourceEntity Del(int resourceId)
        {
            var entity = Get(resourceId);
            if (entity == null)
            {
                return null;
            }

            var s = (SourceType)entity.SourceType;

            String[] uf = GetFolder(entity.SourceId, s);

            //图片没有做物理删除
            FileUploadServiceManager.DeletedFile(s.ToString().ToLower(), new string[] { entity.Name }, uf);

            _resourceRepository.Delete(entity);

            return entity;
        }

        /*

        public List<ResourceEntity> Save(HttpFileCollectionBase files, int userid, int defaultNum, int sourceId, SourceType sourceType)
        {
            if (files == null || files.Count == 0)
            {
                return new List<ResourceEntity>(0);
            }

            var list = new List<ResourceEntity>(files.Count);

            var count = 0;
            //检查扩展名
            foreach (string upload in files)
            {
                if (!files[upload].HasFile()) continue;

                var type = files[upload].ContentType;
                //var mimeType = files[upload].ContentType;
                var fileStream = files[upload].InputStream;
                //var fileName = Path.GetFileName(files[upload].FileName);
                //bytes
                var fileLength = files[upload].ContentLength;
                var fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);

                var fileInfo = new FileInfo(files[upload].FileName);
                ResourceEntity entity = null;
                switch (fileInfo.Extension.ToLower())
                {
                    case ".jpg":
                    case ".jpe":
                    case ".gif":
                    case ".jpeg":
                    case ".png":
                        entity = SaveEntity(ResourceType.Image, userid, defaultNum == count, count, sourceId,
                                                sourceType, files[upload], fileInfo.Extension);
                        break;
                    default:
                        entity = SaveEntity(userid, defaultNum == count, count, sourceId, sourceType, files[upload]);
                        break;
                }
                if (entity != null)
                {
                    list.Add(entity);
                }
                count++;
            }

            if (list.Count > 0)
            {
                return _resourceRepository.Insert(list);
            }

            return new List<ResourceEntity>();
        }

        //*/
    }
}

