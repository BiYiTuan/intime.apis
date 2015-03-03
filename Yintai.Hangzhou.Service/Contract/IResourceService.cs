using System.Collections.Generic;
using System.IO;
using System.Web;
using Yintai.Hangzhou.Contract.Images;
using Yintai.Hangzhou.Data.Models;
using Yintai.Hangzhou.Model.Enums;

namespace Yintai.Hangzhou.Service.Contract
{
    public interface IResourceService
    {
        /// <summary>
        /// 存储
        /// </summary>
        /// <param name="files">文件流</param>
        /// <param name="createdUid">created userid</param>
        /// <param name="colorPid"></param>
        /// <param name="sourceId">来源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        List<ResourceEntity> Save(HttpFileCollectionBase files, int createdUid, int colorPid, int sourceId, SourceType sourceType);

        /// <summary>
        /// upload resources to stage folder
        /// </summary>
        /// <param name="files">文件流</param>
        /// <param name="createdUid">created userid</param>
        /// <param name="defaultNum"></param>
        /// <param name="sourceId">来源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        FileInfor SaveStage(FileInfo file, int createdUid, SourceType sourceType);

        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <param name="resourceId">资源ID</param>
        /// <returns></returns>
        ResourceEntity Get(int resourceId);

        /// <summary>
        /// 获取指定资源
        /// </summary>
        /// <param name="sourceId">来源ID</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns></returns>
        List<ResourceEntity> Get(int sourceId, SourceType sourceType);

        /// <summary>
        /// 删除指定资源
        /// </summary>
        /// <param name="resourceId"></param>
        ResourceEntity Del(int resourceId);
    }
}
