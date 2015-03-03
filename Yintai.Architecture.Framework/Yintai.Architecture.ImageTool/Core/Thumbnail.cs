using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Text;
using Yintai.Architecture.Common.Logger;
using Yintai.Architecture.ImageTool.Models.Exif;
using Color = System.Drawing.Color;
using Encoder = System.Drawing.Imaging.Encoder;
using Image = System.Drawing.Image;

namespace Yintai.Architecture.ImageTool.Core
{
    #region 缩略图生成模式枚举
    /// <summary>
    /// 
    /// </summary>
    public enum ThumbMode
    {
        /// <summary>
        /// 
        /// </summary>
        NHW,
        /// <summary>
        /// 
        /// </summary>
        HW,
        /// <summary>
        /// 
        /// </summary>
        H,
        /// <summary>
        /// 
        /// </summary>
        W,
        /// <summary>
        /// 
        /// </summary>
        Cut
    }

    #endregion

    internal class ThumbnailRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int? Width;
        public int? Height;

        public string OriginalImageFullName { get; set; }

        public Dictionary<int, string> ExifInfos { get; set; }

        public ThumbMode ThumbMode { get; set; }

        public int ImageQuality { get; set; }

        public string SaveImageFullName { get; set; }
    }

    internal class ThumbnailResult
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public long ContentSize { get; set; }
    }

    internal interface IThumbnailGeneraterProvider
    {
        ThumbnailResult Generater(ThumbnailRequest info);
    }

    internal class Net4ThumbnailGeneraterProvider : IThumbnailGeneraterProvider
    {
        //public ThumbnailResult Generater(ThumbnailRequest request)
        //{
        //    int towidth = request.Width;
        //    int toheight = request.Height;
        //    int x = 0;
        //    int y = 0;

        //    var exifInfos = request.ExifInfos;
        //    var result = new ThumbnailResult { Width = towidth, Height = toheight };

        //    using (var originalImage = Image.FromFile(request.OriginalImageFullName, true))
        //    {
        //        if (exifInfos == null)
        //        {
        //            exifInfos = new Dictionary<int, string>();
        //            try
        //            {
        //                ProcessExifInfo(originalImage, exifInfos);
        //            }
        //            catch
        //            {

        //            }
        //        }

        //        int ow = originalImage.Width;
        //        int oh = originalImage.Height;
        //        switch (request.ThumbMode)
        //        {
        //            case ThumbMode.NHW:
        //                if (toheight > originalImage.Height && towidth > originalImage.Width)
        //                {
        //                    towidth = originalImage.Width;
        //                    toheight = originalImage.Height;
        //                }
        //                else
        //                {
        //                    if ((double)originalImage.Width / (double)towidth > (double)originalImage.Height / (double)toheight)
        //                    {
        //                        toheight = originalImage.Height * request.Width / originalImage.Width;
        //                    }
        //                    else
        //                    {
        //                        towidth = originalImage.Width * request.Height / originalImage.Height;
        //                    }
        //                }
        //                break;
        //            case ThumbMode.HW:
        //                break;
        //            case ThumbMode.W:
        //                toheight = originalImage.Height * request.Width / originalImage.Width;
        //                break;
        //            case ThumbMode.H:
        //                towidth = originalImage.Width * request.Height / originalImage.Height;
        //                break;
        //            case ThumbMode.Cut:
        //                if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
        //                {
        //                    oh = originalImage.Height;
        //                    ow = originalImage.Height * towidth / toheight;
        //                    y = 0;
        //                    x = (originalImage.Width - ow) / 2;
        //                }
        //                else
        //                {
        //                    ow = originalImage.Width;
        //                    oh = originalImage.Width * request.Height / towidth;
        //                    x = 0;
        //                    y = (originalImage.Height - oh) / 2;
        //                }
        //                break;
        //            default:
        //                break;
        //        }

        //        using (Image bitmap = new System.Drawing.Bitmap(towidth, toheight))
        //        {
        //            var wrapMode = new ImageAttributes();
        //            wrapMode.SetWrapMode(WrapMode.TileFlipXY);


        //            using (var g = System.Drawing.Graphics.FromImage(bitmap))
        //            {
        //                g.Clear(Color.White);
        //                g.CompositingQuality = CompositingQuality.HighQuality;
        //                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        //                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
        //                g.SmoothingMode = SmoothingMode.HighQuality;

        //                g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), x, y, ow, oh, GraphicsUnit.Pixel, wrapMode);

        //                var encoderParms = new EncoderParameters(1);

        //                encoderParms.Param[0] = new EncoderParameter(Encoder.Quality, request.ImageQuality);

        //                bitmap.Save(request.SaveImageFullName, JpegFormat, encoderParms);

        //                result.ContentSize = new FileInfo(request.SaveImageFullName).Length;
        //            }
        //        }
        //    }

        //    return result;
        //}
        public ThumbnailResult Generater(ThumbnailRequest info)
        {
            throw new NotImplementedException();
        }
    }

    internal class ImageMagick4ThumbnailGeneraterProvider : IThumbnailGeneraterProvider
    {
        private readonly string _pathImageMagick;
        private readonly ILog _logger;

        public ImageMagick4ThumbnailGeneraterProvider(ILog log)
        {
            _logger = log;
            _pathImageMagick = Path.GetFullPath(ConfigurationManager.AppSettings["imagemegickExePath"]);
        }

        public ThumbnailResult Generater(ThumbnailRequest info)
        {
            _logger.Debug("begin image ");
            try
            {
                info.OriginalImageFullName = System.IO.Path.GetFullPath(info.OriginalImageFullName);
                info.SaveImageFullName = System.IO.Path.GetFullPath(info.SaveImageFullName);

                if (info.Height == null || info.Height.Value == 0)
                {
                    Make(info.ImageQuality, info.OriginalImageFullName, info.SaveImageFullName, info.Width ?? 0);
                }
                else
                {
                    Make(info.ImageQuality, info.OriginalImageFullName, info.SaveImageFullName, info.Width ?? 0, info.Height.Value);
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
            _logger.Debug("end image");

            return new ThumbnailResult { ContentSize = 0, Height = info.Height ?? 0, Width = info.Width ?? 0 };
        }

        private void Make(int quality, string originalImagePath, string thumbnailPath, int width, int height)
        {
            var sbFileArgs = new StringBuilder()
                        .Append(String.Format("{0}", originalImagePath))
                  .Append(@" -intent relative")
                       .AppendFormat(@" -resize {0}x{1} ", width, height)
                         .Append(@" -unsharp .5x.5+.5+0 ")
                //        .Append(@" -depth 8 ")
                //     .Append(@" -strip")
                        .Append(String.Format(" -quality {0} ", quality.ToString(CultureInfo.InvariantCulture)))
                        .Append(thumbnailPath);
            var fileArgs = sbFileArgs.ToString();
            _logger.Debug(fileArgs);
            CallImageMagick(fileArgs);
        }

        private void Make(int quality, string originalImagePath, string thumbnailPath, int width)
        {
            var sbFileArgs = new StringBuilder()
                        .Append(String.Format("{0}", originalImagePath))
                  .Append(@" -intent relative")
                       .AppendFormat(@" -resize {0}x ", width)
                         .Append(@" -unsharp .5x.5+.5+0 ")
                //        .Append(@" -depth 8 ")
                //     .Append(@" -strip")
                        .Append(String.Format(" -quality {0} ", quality.ToString(CultureInfo.InvariantCulture)))
                        .Append(thumbnailPath);
            var fileArgs = sbFileArgs.ToString();
            _logger.Debug(fileArgs);
            CallImageMagick(fileArgs);
            /*
           ImageMagickNET.Image tempImage = new ImageMagickNET.Image();
           tempImage.Read(originalImagePath);
           Geometry geo = new Geometry();
           geo.Width((uint)width);
           geo.Aspect();
           tempImage.Resize(geo);
           tempImage.Unsharpmask(1.5, 1, 0.7, 0.02);   //must keep value like this
           tempImage.Write(thumbnailPath);
           return;
             * */
        }

        private void CallImageMagick(string fileArgs)
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = fileArgs,
                FileName = _pathImageMagick,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true
            };

            using (var exeProcess = Process.Start(startInfo))//Process.Start(System.IO.Path.Combine(pathImageMagick,appImageMagick),fileArgs))
            {
                exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }
    }

    #region 生成缩略图

    //[System.Obsolete("目前只能切横向图")]
    public class Thumbnail
    {
        #region Private

        private static ImageCodecInfo _jpegFormat;
        protected static readonly ILog Logger = LoggerManager.Current();

        private static ImageMagick4ThumbnailGeneraterProvider _generater;

        /// <summary>
        /// 
        /// </summary>
        protected static ImageCodecInfo JpegFormat
        {
            get
            {
                if (_jpegFormat != null)
                {
                    return _jpegFormat;
                }

                var infos = ImageCodecInfo.GetImageEncoders();

                for (int i = 0, length = infos.Length; i < length; i++)
                {
                    var info = infos[i];

                    if (info.FormatID == ImageFormat.Jpeg.Guid)
                    {
                        _jpegFormat = info;

                        return info;
                    }
                }

                return null;
            }
        }

        public static Thumbnail _thumbnail = new Thumbnail();

        public static Thumbnail Instance
        {
            get { return _thumbnail; }
        }

        #endregion

        private Thumbnail()
        {
            _generater = new ImageMagick4ThumbnailGeneraterProvider(Logger);
        }

        public void MakeThumbnailPic(string originalImagePath, string thumbnailPath, int width, int height, ThumbMode mode, int imageQuality)
        {
            MakeThumbnailPicAndReturnSize(originalImagePath, thumbnailPath, width, height, mode, imageQuality);
        }

        public long MakeThumbnailPicAndReturnSize(string originalImagePath, string thumbnailPath, int width, int height, ThumbMode mode, int imageQuality)
        {
            int realHeight;
            int realWidht;
            IDictionary<int, string> exifs = new Dictionary<int, string>();

            return MakeThumbnailPicAndReturnSize(originalImagePath, thumbnailPath, width, height, mode, imageQuality, out realWidht, out realHeight, exifs);
        }

        public long MakeThumbnailPicAndReturnSize(string originalImagePath, string thumbnailPath, int width, int height, ThumbMode mode, int imageQuality,
            out int realWidth, out int realHeight, IDictionary<int, string> exifInfos)
        {
            int towidth = width;
            int toheight = height;
            int x = 0;
            int y = 0;

            using (Image originalImage = Image.FromFile(originalImagePath, true))
            {

                if (exifInfos == null)
                {
                    exifInfos = new Dictionary<int, string>();
                    try
                    {
                        ProcessExifInfo(originalImage, exifInfos);
                    }
                    catch
                    {

                    }
                }

                int ow = originalImage.Width;
                int oh = originalImage.Height;
                switch (mode)
                {
                    case ThumbMode.NHW:
                        if (toheight > originalImage.Height && towidth > originalImage.Width)
                        {
                            towidth = originalImage.Width;
                            toheight = originalImage.Height;
                        }
                        else
                        {
                            if ((double)originalImage.Width / (double)towidth > (double)originalImage.Height / (double)toheight)
                            {
                                toheight = originalImage.Height * width / originalImage.Width;
                            }
                            else
                            {
                                towidth = originalImage.Width * height / originalImage.Height;
                            }
                        }
                        break;
                    case ThumbMode.HW:
                        break;
                    case ThumbMode.W:
                        toheight = originalImage.Height * width / originalImage.Width;
                        break;
                    case ThumbMode.H:
                        towidth = originalImage.Width * height / originalImage.Height;
                        break;
                    case ThumbMode.Cut:
                        if ((double)originalImage.Width / (double)originalImage.Height > (double)towidth / (double)toheight)
                        {
                            oh = originalImage.Height;
                            ow = originalImage.Height * towidth / toheight;
                            y = 0;
                            x = (originalImage.Width - ow) / 2;
                        }
                        else
                        {
                            ow = originalImage.Width;
                            oh = originalImage.Width * height / towidth;
                            x = 0;
                            y = (originalImage.Height - oh) / 2;
                        }
                        break;
                    default:
                        break;
                }

                realWidth = towidth;
                realHeight = toheight;

                //TODO:edit这里
                if (_generater != null)
                {
                    Logger.Debug("执行imagemagic");
                    _generater.Generater(new ThumbnailRequest
                    {
                        Height = toheight < 1 ? new int?() : toheight,
                        Width = realWidth,
                        ImageQuality = imageQuality,
                        OriginalImageFullName = originalImagePath,
                        SaveImageFullName = thumbnailPath,
                        ThumbMode = mode
                    });

                    return new FileInfo(thumbnailPath).Length;
                }
                else
                {
                    Logger.Debug("imagemagic = null");
                }

                using (Image bitmap = new System.Drawing.Bitmap(towidth, toheight))
                {
                    var wrapMode = new ImageAttributes();
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);


                    using (var g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.Clear(Color.White);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), x, y, ow, oh, GraphicsUnit.Pixel, wrapMode);

                        var encoderParms = new EncoderParameters(1);

                        encoderParms.Param[0] = new EncoderParameter(Encoder.Quality, imageQuality);

                        bitmap.Save(thumbnailPath, JpegFormat, encoderParms);

                        return new FileInfo(thumbnailPath).Length;
                    }
                }
            }
        }

        #region Helper

        protected static void ProcessExifInfo(Image image, IDictionary<int, string> exifInfos)
        {
            ReadExif(exifInfos, image, ExifFileds.ImageWidth);
            ReadExif(exifInfos, image, ExifFileds.ImageHeight);
            ReadExif(exifInfos, image, ExifFileds.DateTimeOriginal);
            ReadExif(exifInfos, image, ExifFileds.Software);
            ReadExif(exifInfos, image, ExifFileds.ExposureTime);
            ReadExif(exifInfos, image, ExifFileds.ExposureProgram);
            ReadExif(exifInfos, image, ExifFileds.Make);
            ReadExif(exifInfos, image, ExifFileds.Model);
            ReadExif(exifInfos, image, ExifFileds.FocalLength);
            ReadExif(exifInfos, image, ExifFileds.ApertureValue);
            ReadExif(exifInfos, image, ExifFileds.MeteringMode);
            ReadExif(exifInfos, image, ExifFileds.ISOSpeedRatings);

            exifInfos.Add((int)ExifFileds.Flash, ExifTags.ReadTag(image, ExifFileds.Flash) == "关闭" ? "关闭" : "开启");
        }

        protected static void ReadExif(IDictionary<int, string> exifInfos, Image image, ExifFileds exifType)
        {
            exifInfos.Add((int)exifType, ExifTags.ReadTag(image, exifType));
        }

        #endregion

    }
    #endregion
}
