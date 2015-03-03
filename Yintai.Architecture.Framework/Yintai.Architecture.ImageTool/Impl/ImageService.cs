using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.Common.Models;
using Yintai.Architecture.ImageTool.Configurations;
using Yintai.Architecture.ImageTool.Contract;
using Yintai.Architecture.ImageTool.Core;
using Yintai.Architecture.ImageTool.Models;

namespace Yintai.Architecture.ImageTool.Impl
{
    public class ImageService : IImageService
    {
        private static readonly ILog Log = LoggerManager.Current();

        private static readonly HashSet<string> Pexts = new HashSet<string>(new[] { "bmp", "pcx", "tiff", "gif", "jpeg", "jpg", "tga", "exif", "fpx", "svg", "psd", "cdr", "pcd", "dxf", "ufo", "eps", "png" });

        private ImageSettingConfig _imageSetting;
        private ImageElement _imageElement;
        private string _fileFolder = String.Empty;

        private const string FileTempExt = "_original";

        #region methods

        //private void SaveAudio(string oName)
        //{
        //    SaveAudioAndReturnDuration(oName, false);
        //}

        private TimeSpan SaveAudioAndReturnDuration(string oName, bool isReturnDuration)
        {
            var oFullName = oName;
            var tFullName = oName.Substring(0, oName.IndexOf(FileTempExt, System.StringComparison.Ordinal)) + ".mp3";

            AudioService.Current.Compression(oFullName, tFullName);

            if (isReturnDuration)
            {
                try
                {
                    var d = AudioService.Current.GetDuration(tFullName);
                    Log.Warn("n:" + tFullName + ",d:" + d.TotalSeconds);
                    return d;
                }
                catch (Exception ex)
                {
                    Log.Error(ex);

                    throw;
                }

            }

            return TimeSpan.Zero;
        }


        private FileMessage GetFileName(string key, int fileSize, string clientFilePath, string contentType, out string fileKey, out string fileExt, string[] userFolders)
        {
            Log.Debug(String.Format("Input parameters:\n key:{0}\n fileSize:{1}\n clientFilePath:{2}", key, fileSize, clientFilePath));

            fileKey = string.Empty;
            _imageSetting = (ImageSettingConfig)ConfigurationManager.GetSection("imageSetting");
            _imageElement = _imageSetting.ImageCollection[key];

            _fileFolder = String.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

            fileExt = clientFilePath.Substring(clientFilePath.LastIndexOf(".", System.StringComparison.Ordinal) + 1).ToLower();

            if (_imageElement.Suffix.IndexOf(fileExt, System.StringComparison.Ordinal) < 0)
            {
                if (!String.IsNullOrWhiteSpace(contentType))
                {
                    fileExt = ContentType.GetExt(contentType);
                }

                if (_imageElement.Suffix.IndexOf(fileExt, System.StringComparison.Ordinal) < 0)
                {
                    return FileMessage.ExtError;
                }
            }

            if (fileSize > _imageElement.Size * 1024 || fileSize <= 0)
            {
                return FileMessage.SizeError;//ImageMsg.SizeError;
            }

            string tfileFolder;

            if (userFolders == null || userFolders.Length == 0)
            {
                tfileFolder = _imageElement.Folder.Replace("{Year}", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)).Replace("{Month}", ToLongTime(DateTime.Now.Month)).Replace("{Day}", ToLongTime(DateTime.Now.Day));
            }
            else
            {
                tfileFolder = String.Format(_imageElement.Folder, userFolders);
            }

            _fileFolder = Path.Combine(_fileFolder, tfileFolder);

            if (!Directory.Exists(_fileFolder))
            {
                Directory.CreateDirectory(_fileFolder);
            }

            fileKey = String.Format("{0}{1}", tfileFolder, Guid.NewGuid().ToString());
            //Console.WriteLine(clientFilePath + "验证成功");

            Log.Debug(String.Format("Create file and generate key successful.fileFolder: {0},filekey: {1}", _fileFolder, fileKey));

            return FileMessage.Success;
        }

        #endregion

        #region GetFileName


        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, out string fileKey, out string fileExt)
        {
            return GetFileName(key, fileSize, clientFilePath, null, out  fileKey, out fileExt, new string[0]);
        }

        public FileMessage GetFileName(string key, int fileSize, string clientFilePath, string contentType, out string fileKey,
                                       out string fileExt)
        {
            return GetFileName(key, fileSize, clientFilePath, contentType, out  fileKey, out fileExt, new string[0]);
        }


        public FileMessage GetFileNameByUser(string key, int fileSize, string clientFilePath, string contentType, out string fileKey, out string fileExt, string[] userFolders)
        {
            return GetFileName(key, fileSize, clientFilePath, contentType, out  fileKey, out fileExt, userFolders);
        }

        #endregion

        #region UploadFile


        [Obsolete("此方法已经过期")]
        public void UploadFile(FileUploadMessage request)
        {
            Log.Debug(string.Format("Upload file:\n filename:{0}\n fileext:{1}\n keyname:{2}\n saveorigin:{3}\n converttojpg:{4}\n", request.FileName, request.FileExt, request.KeyName, request.SaveOrigin, request.ConvertToJPG));


            if (request.FileData.CanRead == false)
            {
                Log.Debug(string.Format("\"{0}\"文件不可读", request.FileName));

                request.FileData.Close();
                request.FileData.Dispose();

                return;
            }

            _imageSetting = (ImageSettingConfig)ConfigurationManager.GetSection("imageSetting");
            _imageElement = _imageSetting.ImageCollection[request.KeyName];


            _fileFolder = string.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

            var filePath = Path.Combine(_fileFolder, request.FileName + (_imageElement.AsNormalFile ? String.Empty : FileTempExt) + "." + request.FileExt);

            Log.Debug(string.Format("filepath:{0}", filePath));

            var fileInfo = new FileInfo(filePath);

            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            try
            {
                using (var sourceStream = request.FileData)
                {
                    using (var targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {

                        const int bufferLen = 4096;
                        var buffer = new byte[bufferLen];
                        int count = 0;

                        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                        {
                            targetStream.Write(buffer, 0, count);
                        }

                        targetStream.Close();
                        sourceStream.Close();
                        request.FileData.Close();
                        request.FileData.Dispose();
                    }
                }

                if (_imageElement.Type == 2)
                {
                    var timeSpan = SaveAudioAndReturnDuration(filePath, true);

                    var t = new ThumbnailInfo
                    {
                        Sizes =
                            new Dictionary<string, ImageSize>(1)
                                    {
                                        {
                                            request.KeyName, new ImageSize(0, 0, Int64.Parse(timeSpan.TotalSeconds.ToString("F0")))
                                        }
                                    }
                    };

                    return;
                    //return t;
                }

                if (Pexts.Contains(request.FileExt, StringComparer.OrdinalIgnoreCase) && !_imageElement.AsNormalFile)
                {

                    foreach (ThumbElement thumb in _imageElement.ThumbCollection)
                    {
                        string thumbPath = filePath.Substring(0, filePath.IndexOf(FileTempExt, System.StringComparison.Ordinal)) + "_" + thumb.Width.ToString(CultureInfo.InvariantCulture) + "x" + thumb.Height.ToString(CultureInfo.InvariantCulture) + ".jpg";
                        try
                        {
                            Thumbnail.Instance.MakeThumbnailPic(filePath, thumbPath, thumb.Width, thumb.Height, thumb.Mode, _imageSetting.ImageQuality);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                else if (Pexts.Contains(request.FileExt, StringComparer.OrdinalIgnoreCase) && request.ConvertToJPG && _imageElement.AsNormalFile)
                {
                    using (var bitmap = Image.FromFile(filePath))
                    {

                        bitmap.Save(filePath.Substring(0, filePath.IndexOf("." + request.FileExt, System.StringComparison.Ordinal)) + ".jpg", ImageFormat.Jpeg);
                    }

                }

                if (!request.SaveOrigin)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("上传文件\"{0}\"发生错误：{1}", request.FileName, ex.Message));
            }

            Log.Debug(string.Format("Upload file successful.file: {0}, filekey: {1}", request.FileName, request.KeyName));
        }

        #endregion

        #region UploadFileAndReturnInfo

        public ThumbnailInfo UploadFileAndReturnInfo(FileUploadMessage request)
        {
            Log.Debug(string.Format("Upload file:\n filename:{0}\n fileext:{1}\n keyname:{2}", request.FileName, request.FileExt, request.KeyName));


            if (request.FileData.CanRead == false)
            {
                Log.Debug(string.Format("\"{0}\"文件不可读", request.FileName));

                request.FileData.Close();
                request.FileData.Dispose();

                return null;
            }


            _imageSetting = (ImageSettingConfig)ConfigurationManager.GetSection("imageSetting");


            _imageElement = _imageSetting.ImageCollection[request.KeyName];


            _fileFolder = String.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

            string filePath = Path.Combine(_fileFolder, request.FileName + (_imageElement.AsNormalFile ? string.Empty : FileTempExt) + "." + request.FileExt);

            var fileInfo = new FileInfo(filePath);


            if (fileInfo.Directory != null && !fileInfo.Directory.Exists)
            {
                Directory.CreateDirectory(fileInfo.Directory.FullName);
            }

            try
            {
                using (var sourceStream = request.FileData)
                {
                    using (var targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {

                        const int bufferLen = 4096;
                        byte[] buffer = new byte[bufferLen];
                        int count = 0;
                        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                        {
                            targetStream.Write(buffer, 0, count);
                        }

                        targetStream.Close();
                        sourceStream.Close();
                        request.FileData.Close();
                        request.FileData.Dispose();
                    }

                }

                if (_imageElement.Type == 2)
                {
                    var timeSpan = SaveAudioAndReturnDuration(filePath, true);

                    Log.Warn(timeSpan.TotalSeconds);
                    var t = new ThumbnailInfo
                        {
                            Sizes =
                                new Dictionary<string, ImageSize>(1)
                                    {
                                        {
                                            request.KeyName, new ImageSize(0, 0, Int64.Parse(timeSpan.TotalSeconds.ToString("F0")))
                                        }
                                    }
                        };

                    return t;
                }

                if (Pexts.Contains(request.FileExt, StringComparer.OrdinalIgnoreCase) && !_imageElement.AsNormalFile)
                {
                    ThumbnailInfo thumbnailInfoes = new ThumbnailInfo();
                    thumbnailInfoes.Info = new Dictionary<string, long>();
                    thumbnailInfoes.Sizes = new Dictionary<string, ImageSize>();

                    foreach (ThumbElement thumb in _imageElement.ThumbCollection)
                    {
                        string thumbPath = filePath.Substring(0, filePath.IndexOf(FileTempExt)) + "_" + thumb.Width.ToString() + "x" + thumb.Height.ToString() + ".jpg";
                        try
                        {
                            int realWidth;
                            int realHeight;

                            Log.Debug("thumbPath:" + thumbPath);
                            long size = Thumbnail.Instance.MakeThumbnailPicAndReturnSize(filePath, thumbPath, thumb.Width, thumb.Height, thumb.Mode, _imageSetting.ImageQuality, out realWidth, out realHeight, thumbnailInfoes.ExifInfos);

                            thumbnailInfoes.Info.Add(thumb.Key, size);
                            thumbnailInfoes.Sizes.Add(thumb.Key, new ImageSize(realWidth, realHeight));
                        }
                        catch (Exception ex)
                        {
                            Log.Warn(ex);
                            continue;
                        }
                    }

                    return thumbnailInfoes;
                }


                else if (Pexts.Contains(request.FileExt, StringComparer.OrdinalIgnoreCase) && request.ConvertToJPG && _imageElement.AsNormalFile)
                {
                    using (Image bitmap = Image.FromFile(filePath))
                    {

                        bitmap.Save(filePath.Substring(0, filePath.IndexOf("." + request.FileExt)) + ".jpg", ImageFormat.Jpeg);
                    }

                }


                if (!request.SaveOrigin)
                {
                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("上传文件\"{0}\"发生错误：{1}", request.FileName, ex.Message));
            }



            Log.Debug(string.Format("Upload file successful.file: {0}, filekey: {1}", request.FileName, request.KeyName));

            return new ThumbnailInfo();
        }

        #endregion

        #region IsExistingFile

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="fileName"></param>
        /// <param name="fileExt"></param>
        /// <returns></returns>
        public bool IsExistingFile(string key, string fileName, string fileExt)
        {
            _imageSetting = ConfigurationManager.GetSection("imageSetting") as ImageSettingConfig;

            if (_imageSetting != null)
            {
                _imageElement = _imageSetting.ImageCollection[key];


                _fileFolder = string.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

                string filePath = Path.Combine(_fileFolder + _imageElement.Folder.Replace("/", @"\"), fileName + "." + fileExt);
                Log.Debug(string.Format("Is Existing File :\n key:{0}\n fileName:{1}\n fileExt:{2}\n filepath:{3}", key, fileName, fileExt, filePath));

                return File.Exists(filePath);
            }

            return false;
        }

        #endregion

        #region HandleGroupPortrait
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool HandleGroupPortrait(string fileName, string url)
        {
            string key = "GroupPortrait";
            string fileExt = "jpg";
            _imageSetting = ConfigurationManager.GetSection("imageSetting") as ImageSettingConfig;

            if (_imageSetting != null)
            {
                _imageElement = _imageSetting.ImageCollection[key];


                _fileFolder = string.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

                string filePath = Path.Combine(_fileFolder + _imageElement.Folder.Replace("/", @"\"), fileName + "." + fileExt);

                //log.Error(string.Format("HandleGroupPortrait :\n fileName:{0}\n url:{1}\n filePath:{2}", fileName, url, filePath));

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                FileInfo fileInfo = new FileInfo(filePath);
                if (!fileInfo.Directory.Exists)
                {
                    Directory.CreateDirectory(fileInfo.Directory.FullName);
                }

                try
                {
                    WebResponse response = null;
                    try
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Accept = "*/*";
                        response = request.GetResponse();
                        if (response.ContentType.ToLower().StartsWith("image/"))
                        {
                            if (SaveBinaryFile(response, filePath))
                            {
                                return true;
                            }
                        }

                        Log.Error(string.Format("HandleGroupPortrait：从核心服务拷贝图片到文件服务器时出错：fileName:\"{0}\" url:{1} ", fileName, url));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(string.Format("HandleGroupPortrait：从核心服务获取头像时出错：fileName:\"{0}\" url:{1} 错误信息：{2}", fileName, url, ex.Message));
                        return false;
                    }
                    finally
                    {
                        if (response != null) response.Close();
                    }

                    //using (Stream sourceStream = request.FileData)
                    //{
                    //    using (FileStream targetStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    //    {
                    //        //从文件流中读取4k
                    //        const int bufferLen = 4096;
                    //        byte[] buffer = new byte[bufferLen];
                    //        int count = 0;

                    //        while ((count = sourceStream.Read(buffer, 0, bufferLen)) > 0)
                    //        {
                    //            targetStream.Write(buffer, 0, count);
                    //        }

                    //        targetStream.Close();
                    //        sourceStream.Close();
                    //        request.FileData.Close();
                    //        request.FileData.Dispose();
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("HandleGroupPortrait：fileName:\"{0}\" url:{1} 错误信息：{2}", fileName, url, ex.Message));
                }
            }

            Log.Error(string.Format("HandleGroupPortrait :\n fileName:{0}\n url:{1}\n", fileName, url));

            return false;
        }

        #endregion

        #region DeleteFile

        public bool DeleteFile(string key, string[] fileNameList, string[] dateList, string[] userFolders)
        {
            try
            {
                if (dateList == null)
                {
                    return DeleteFileInternal(key, fileNameList, userFolders);
                }

                return DeleteFileInternal(key, fileNameList, dateList, userFolders);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
            }

            return false;
        }

        public bool DeleteNormalFile(string[] filePaths)
        {
            try
            {
                return DeleteNormalFileInternal(filePaths);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message + ex.StackTrace);
            }

            return false;
        }

        private bool DeleteFileInternal(string key, string[] fileNameList, string[] userFolders)
        {
            //整理成文件名，

            List<string> dateList = new List<string>(fileNameList.Length);
            List<string> fileNames = new List<string>(fileNameList.Length);

            foreach (var fileName in fileNameList)
            {
                var pos = fileName.LastIndexOf(@"\");
                var f = fileName.Substring(pos + 1);
                fileNames.Add(f);
                var posi = fileName.IndexOf(@"\");
                var d = fileName.Substring(posi + 1, 8);
                dateList.Add(d);
            }

            return DeleteFileInternal(key, fileNames.ToArray(), dateList.ToArray(), userFolders);
        }

        private bool DeleteFileInternal(string key, string[] fileNameList, string[] dateList, string[] userFolders)
        {
            if (string.IsNullOrEmpty(key) || fileNameList == null || fileNameList.Length < 1)
            {
                return true;
            }


            bool isSpecialDate = false;

            if (dateList != null && dateList.Length > 0)
            {
                isSpecialDate = true;

                if (dateList.Length != fileNameList.Length)
                {

                    Log.Error(string.Format("Delete File Error :\n key:{0}\n 指定的flieNameList个数和date个数不符合", key));
                    return false;
                }
            }

            _imageSetting = ConfigurationManager.GetSection("imageSetting") as ImageSettingConfig;

            if (_imageSetting != null)
            {
                _imageElement = _imageSetting.ImageCollection[key];


                string backupRootDirectory = _imageSetting.BackupFolder;

                _fileFolder = string.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;

                string fileSubFolder = _imageElement.Folder;

                if (userFolders != null && userFolders.Length > 0)
                {
                    fileSubFolder = string.Format(fileSubFolder, userFolders);
                }

                string currentDirectory = string.Empty;

                for (int i = 0; i < fileNameList.Length; i++)
                {
                    if (isSpecialDate && dateList[i].Length != 8)
                    {
                        Log.Error(string.Format("Delete File Error :\n key:{0}\n 指定的日期：{1}不合法", key, dateList[i]));
                        continue;
                    }

                    if (isSpecialDate)
                    {
                        fileSubFolder = fileSubFolder.Replace("{Year}", dateList[i].Substring(0, 4)).Replace("{Month}", ToLongTime(Int32.Parse(dateList[i].Substring(4, 2)))).Replace("{Day}", ToLongTime(Int32.Parse(dateList[i].Substring(6, 2))));
                    }
                    //else
                    //{
                    //    fileSubFolder = fileSubFolder.Replace("{Year}", DateTime.Now.Year.ToString()).Replace("{Month}", ToLongTime(DateTime.Now.Month)).Replace("{Day}", ToLongTime(DateTime.Now.Day));
                    //}


                    currentDirectory = Path.Combine(_fileFolder, fileSubFolder);


                    string backupDirectory = currentDirectory.Replace(_fileFolder, backupRootDirectory);

                    if (!Directory.Exists(backupDirectory))
                    {
                        Directory.CreateDirectory(backupDirectory);
                    }

                    string searchPattern = string.Format(fileNameList[i] + "*.*");

                    string[] fileNames = Directory.GetFiles(currentDirectory, searchPattern, SearchOption.TopDirectoryOnly);

                    foreach (string name in fileNames)
                    {

                        File.Copy(name, name.Replace(_fileFolder, backupRootDirectory), true);


                        File.Delete(name);
                    }


                    Log.Info(string.Format("Backup File :\n key:{0}\n fileName:{1}\n filepath:{2}", key, fileNameList[i], backupDirectory));

                    Log.Info(string.Format("Delete File :\n key:{0}\n fileName:{1}\n filepath:{2}", key, fileNameList[i], currentDirectory));
                }

                return true;
            }

            return false;
        }


        private bool DeleteNormalFileInternal(string[] filePaths)
        {

            if (filePaths == null || filePaths.Length < 1)
            {
                return false;
            }

            _imageSetting = ConfigurationManager.GetSection("imageSetting") as ImageSettingConfig;

            if (_imageSetting != null)
            {
                foreach (string fileName in filePaths)
                {


                    string key = fileName.Split(new char[] { '/' })[0] + "/";

                    foreach (ImageElement item in _imageSetting.ImageCollection)
                    {
                        if (item.Folder.StartsWith(key))
                        {
                            _imageElement = item;
                        }
                    }

                    if (_imageElement == null)
                    {
                        Log.Error(string.Format("删除文件{0}发生错误,找不到key:{1}的配置节点", fileName, key));
                        return false;
                    }


                    _fileFolder = string.IsNullOrEmpty(_imageElement.RootFolder) ? _imageSetting.Folder : _imageElement.RootFolder;


                    _imageSetting = (ImageSettingConfig)ConfigurationManager.GetSection("imageSetting");
                    string backupRootDirectory = _imageSetting.BackupFolder;


                    string backupDirectory = _fileFolder.Replace(_fileFolder, backupRootDirectory);

                    string backupFullDirectory = backupDirectory;


                    Log.Info(string.Format("backupFullDirectory:{0} fileName:{1},fileFolder:{2}", backupFullDirectory, fileName, _fileFolder));

                    if (fileName.Contains(@"\"))
                    {
                        backupFullDirectory = Path.Combine(backupFullDirectory, fileName.Substring(0, fileName.LastIndexOf(@"\")));
                    }
                    else if (fileName.Contains("/"))
                    {
                        backupFullDirectory = Path.Combine(backupFullDirectory, fileName.Substring(0, fileName.LastIndexOf("/")));
                    }
                    else
                    {
                        backupFullDirectory = Path.Combine(backupFullDirectory, fileName);
                    }

                    if (!Directory.Exists(backupFullDirectory))
                    {
                        Directory.CreateDirectory(backupFullDirectory);
                    }

                    string searchPattern = fileName + "*.*";

                    string[] fileNames = Directory.GetFiles(_fileFolder, searchPattern, SearchOption.TopDirectoryOnly);

                    foreach (string name in fileNames)
                    {

                        File.Copy(name, name.Replace(_fileFolder, backupRootDirectory), true);


                        File.Delete(name);
                    }


                    Log.Info(string.Format("Backup File :\n fileName:{0}\n filepath:{1}", fileName, backupDirectory));

                    Log.Info(string.Format("Delete File :\n fileName:{0}\n filepath:{1}", fileName, _fileFolder));
                }
                return true;
            }
            return false;
        }

        #endregion

        #region Helper



        private static string ToLongTime(int times)
        {
            return times < 10 ? "0" + times.ToString(CultureInfo.InvariantCulture) : times.ToString(CultureInfo.InvariantCulture);
        }

        private static bool SaveBinaryFile(WebResponse response, string path)
        {
            var buffer = new byte[1024];
            //string dir = Path.GetDirectoryName(path);
            //if (!Directory.Exists(dir))
            //    Directory.CreateDirectory(dir);
            //else
            //{
            //    /// 移除过期的头像
            //    string endFile = path.Substring(path.LastIndexOf('_'));
            //    string[] files = Directory.GetFiles(dir, "*" + endFile);
            //    foreach (string file in files)
            //    {
            //        File.Delete(file);
            //    }
            //}

            try
            {
                using (Stream outStream = File.Create(path))
                {
                    using (var inStream = response.GetResponseStream())
                    {

                        int l;
                        do
                        {
                            l = inStream.Read(buffer, 0, buffer.Length);
                            if (l > 0)
                                outStream.Write(buffer, 0, l);
                        }
                        while (l > 0);

                        inStream.Close();
                    }
                    outStream.Close();
                }
            }
            catch (Exception ex)
            {
                Log.Error(string.Format("HandleGroupPortrait：保存群组头像 path:\"{0}\" 错误信息：{1}", path, ex.Message));

                return false;
            }

            return true;
        }

        #endregion
    }
}
