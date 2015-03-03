#region License
//  Copyright (c) 2009 Lev Danielyan
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy of 
//  this software and associated documentation files (the "Software"), to deal in the 
//  Software without restriction, including without limitation the rights to use, copy, 
//  modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
//  and to permit persons to whom the Software is furnished to do so, subject to the 
//  following conditions:
//
//  The above copyright notice and this permission notice shall be included in all copies 
//  or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//  PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
//  FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//  ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//  THE SOFTWARE.

//Note:代码有修改，勿覆盖

#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;

namespace Yintai.Architecture.ImageTool.Models.Exif
{
    #region ExifTag

    public sealed class ExifTag
    {
        public int Id { get; set; }
        public string FieldName { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }

        public ExifTag(int id, string fieldName, string description)
        {
            Id = id;
            Description = description;
            FieldName = fieldName;
        }

        public override string ToString()
        {
            return String.Format("Field Id:\t{0}\nField Name:\t{1}\nDescription:\t{2}\nField Value:\t{3}",
                                 Id, FieldName, Description, Value);
        }
    }

    #endregion

    #region SupportedTags

    internal class SupportedTags : List<ExifTag>
    {
        private static SupportedTags _inst;

        private SupportedTags()
        {
            Add(new ExifTag(0x100, "ImageWidth", "Image width"));
            Add(new ExifTag(0x101, "ImageHeight", "Image height"));
            Add(new ExifTag(0x0, "GPSVersionID", "GPS tag version"));
            Add(new ExifTag(0x5, "GPSAltitudeRef", "Altitude reference"));
            Add(new ExifTag(0x111, "StripOffsets", "Image data location"));
            Add(new ExifTag(0x116, "RowsPerStrip", "Number of rows per strip"));
            Add(new ExifTag(0x117, "StripByteCounts", "Bytes per compressed strip"));
            Add(new ExifTag(0xA002, "PixelXDimension", "Valid image width"));
            Add(new ExifTag(0xA003, "PixelYDimension", "Valid image height"));
            Add(new ExifTag(0x102, "BitsPerSample", "Number of bits per component"));
            Add(new ExifTag(0x103, "Compression", "Compression scheme"));
            Add(new ExifTag(0x106, "PhotometricInterpretation", "Pixel composition"));
            Add(new ExifTag(0x112, "Orientation", "Orientation of image"));
            Add(new ExifTag(0x115, "SamplesPerPixel", "Number of components"));
            Add(new ExifTag(0x11C, "PlanarConfiguration", "Image data arrangement"));
            Add(new ExifTag(0x212, "YCbCrSubSampling", "Subsampling ratio of Y to C"));
            Add(new ExifTag(0x213, "YCbCrPositioning", "Y and C positioning"));
            Add(new ExifTag(0x128, "ResolutionUnit", "Unit of X and Y resolution"));
            Add(new ExifTag(0x12D, "TransferFunction", "Transfer function"));
            Add(new ExifTag(0xA001, "ColorSpace", "Color space information"));
            Add(new ExifTag(0x8822, "ExposureProgram", "Exposure program"));
            Add(new ExifTag(0x8827, "ISOSpeedRatings", "ISO speed rating"));
            Add(new ExifTag(0x9207, "MeteringMode", "Metering mode"));
            Add(new ExifTag(0x9208, "LightSource", "Light source"));
            Add(new ExifTag(0x9209, "Flash", "Flash"));
            Add(new ExifTag(0x9214, "SubjectArea", "Subject area"));
            Add(new ExifTag(0xA210, "FocalPlaneResolutionUnit", "Focal plane resolution unit"));
            Add(new ExifTag(0xA214, "SubjectLocation", "Subject location"));
            Add(new ExifTag(0xA217, "SensingMethod", "Sensing method"));
            Add(new ExifTag(0xA401, "CustomRendered", "Custom image processing"));
            Add(new ExifTag(0xA402, "ExposureMode", "Exposure mode"));
            Add(new ExifTag(0xA403, "WhiteBalance", "White balance"));
            Add(new ExifTag(0xA405, "FocalLengthIn35mmFilm", "Focal length in 35 mm film"));
            Add(new ExifTag(0xA406, "SceneCaptureType", "Scene capture type"));
            Add(new ExifTag(0xA408, "Contrast", "Contrast"));
            Add(new ExifTag(0xA409, "Saturation", "Saturation"));
            Add(new ExifTag(0xA40A, "Sharpness", "Sharpness"));
            Add(new ExifTag(0xA40C, "SubjectDistanceRange", "Subject distance range"));
            Add(new ExifTag(0x1E, "GPSDifferential", "GPS differential correction"));
            Add(new ExifTag(0x9201, "ShutterSpeedValue", "Shutter speed"));
            Add(new ExifTag(0x9203, "BrightnessValue", "Brightness"));
            Add(new ExifTag(0x9204, "ExposureBiasValue", "Exposure bias"));
            Add(new ExifTag(0x201, "JPEGInterchangeFormat", "Offset to JPEG SOI"));
            Add(new ExifTag(0x202, "JPEGInterchangeFormatLength", "Bytes of JPEG data"));
            Add(new ExifTag(0x11A, "XResolution", "Image resolution in width direction"));
            Add(new ExifTag(0x11B, "YResolution", "Image resolution in height direction"));
            Add(new ExifTag(0x13E, "WhitePoint", "White point chromaticity"));
            Add(new ExifTag(0x13F, "PrimaryChromaticities", "Chromaticities of primaries"));
            Add(new ExifTag(0x211, "YCbCrCoefficients", "Color space transformation matrix coefficients"));
            Add(new ExifTag(0x214, "ReferenceBlackWhite", "Pair of black and white reference values"));
            Add(new ExifTag(0x9102, "CompressedBitsPerPixel", "Image compression mode"));
            Add(new ExifTag(0x829A, "ExposureTime", "Exposure time"));
            Add(new ExifTag(0x829D, "FNumber", "F number"));
            Add(new ExifTag(0x9202, "ApertureValue", "Aperture"));
            Add(new ExifTag(0x9205, "MaxApertureValue", "Maximum lens aperture"));
            Add(new ExifTag(0x9206, "SubjectDistance", "Subject distance"));
            Add(new ExifTag(0x920A, "FocalLength", "Lens focal length"));
            Add(new ExifTag(0xA20B, "FlashEnergy", "Flash energy"));
            Add(new ExifTag(0xA20E, "FocalPlaneXResolution", "Focal plane X resolution"));
            Add(new ExifTag(0xA20F, "FocalPlaneYResolution", "Focal plane Y resolution"));
            Add(new ExifTag(0xA215, "ExposureIndex", "Exposure index"));
            Add(new ExifTag(0xA404, "DigitalZoomRatio", "Digital zoom ratio"));
            Add(new ExifTag(0xA407, "GainControl", "Gain control"));
            Add(new ExifTag(0x2, "GPSLatitude", "Latitude"));
            Add(new ExifTag(0x4, "GPSLongitude", "Longitude"));
            Add(new ExifTag(0x6, "GPSAltitude", "Altitude"));
            Add(new ExifTag(0x7, "GPSTimeStamp", "GPS time (atomic clock)"));
            Add(new ExifTag(0xB, "GPSDOP", "Measurement precision"));
            Add(new ExifTag(0xD, "GPSSpeed", "Speed of GPS receiver"));
            Add(new ExifTag(0xF, "GPSTrack", "Direction of movement"));
            Add(new ExifTag(0x11, "GPSImgDirection", "Direction of image"));
            Add(new ExifTag(0x14, "GPSDestLatitude", "Latitude of destination"));
            Add(new ExifTag(0x16, "GPSDestLongitude", "Longitude of destination"));
            Add(new ExifTag(0x18, "GPSDestBearing", "Bearing of destination"));
            Add(new ExifTag(0x1A, "GPSDestDistance", "Distance to destination"));
            Add(new ExifTag(0x132, "DateTime", "File change date and time"));
            Add(new ExifTag(0x10E, "ImageDescription", "Image title"));
            Add(new ExifTag(0x10F, "Make", "Image input equipment manufacturer"));
            Add(new ExifTag(0x110, "Model", "Image input equipment model"));
            Add(new ExifTag(0x131, "Software", "Software used"));
            Add(new ExifTag(0x13B, "Artist", "Person who created the image"));
            Add(new ExifTag(0x8298, "Copyright", "Copyright holder"));
            Add(new ExifTag(0xA004, "RelatedSoundFile", "Related audio file"));
            Add(new ExifTag(0x9003, "DateTimeOriginal", "Date and time of original data generation"));
            Add(new ExifTag(0x9004, "DateTimeDigitized", "Date and time of digital data generation"));
            Add(new ExifTag(0x9290, "SubSecTime", "DateTime subseconds"));
            Add(new ExifTag(0x9291, "SubSecTimeOriginal", "DateTimeOriginal subseconds"));
            Add(new ExifTag(0x9292, "SubSecTimeDigitized", "DateTimeDigitized subseconds"));
            Add(new ExifTag(0xA420, "ImageUniqueID", "Unique image ID"));
            Add(new ExifTag(0x8824, "SpectralSensitivity", "Spectral sensitivity"));
            Add(new ExifTag(0x1, "GPSLatitudeRef", "North or South Latitude"));
            Add(new ExifTag(0x3, "GPSLongitudeRef", "East or West Longitude"));
            Add(new ExifTag(0x8, "GPSSatellites", "GPS satellites used for measurement"));
            Add(new ExifTag(0x9, "GPSStatus", "GPS receiver status"));
            Add(new ExifTag(0xA, "GPSMeasureMode", "GPS measurement mode"));
            Add(new ExifTag(0xC, "GPSSpeedRef", "Speed unit"));
            Add(new ExifTag(0xE, "GPSTrackRef", "Reference for direction of movement"));
            Add(new ExifTag(0x10, "GPSImgDirectionRef", "Reference for direction of image"));
            Add(new ExifTag(0x12, "GPSMapDatum", "Geodetic survey data used"));
            Add(new ExifTag(0x13, "GPSDestLatitudeRef", "Reference for latitude of destination"));
            Add(new ExifTag(0x15, "GPSDestLongitudeRef", "Reference for longitude of destination"));
            Add(new ExifTag(0x17, "GPSDestBearingRef", "Reference for bearing of destination"));
            Add(new ExifTag(0x19, "GPSDestDistanceRef", "Reference for distance to destination"));
            Add(new ExifTag(0x1D, "GPSDateStamp", "GPS date"));
            Add(new ExifTag(0x8828, "OECF", "Optoelectric conversion factor"));
            Add(new ExifTag(0xA20C, "SpatialFrequencyResponse", "Spatial frequency response"));
            Add(new ExifTag(0xA300, "FileSource", "File source"));
            Add(new ExifTag(0xA301, "SceneType", "Scene type"));
            Add(new ExifTag(0xA302, "CFAPattern", "CFA pattern"));
            Add(new ExifTag(0xA40B, "DeviceSettingDescription", "Device settings description"));
            Add(new ExifTag(0x9000, "ExifVersion", "Exif version"));
            Add(new ExifTag(0xA000, "FlashpixVersion", "Supported Flashpix version"));
            Add(new ExifTag(0x9101, "ComponentsConfiguration", "Meaning of each component"));
            Add(new ExifTag(0x927C, "MakerNote", "Manufacturer notes"));
            Add(new ExifTag(0x9286, "UserComment", "User comments"));
            Add(new ExifTag(0x1B, "GPSProcessingMethod", "Name of GPS processing method"));
            Add(new ExifTag(0x1C, "GPSAreaInformation", "Name of GPS area"));
        }

        public static SupportedTags GetInstance()
        {
            return _inst ?? (_inst = new SupportedTags());
        }
    }

    #endregion

    #region ExifTags

    public sealed class ExifTags : Dictionary<string, ExifTag>
    {
        #region Fields

        private static readonly List<ExifTag> SupportedTags = Exif.SupportedTags.GetInstance();
        private static readonly Encoding Ascii = Encoding.ASCII;

        #endregion

        #region Property

        public static IEnumerable<string> SupportedTagNames
        {
            get
            {
                var tags = from tag in SupportedTags
                           select tag.FieldName;
                return tags;
            }
        }

        #endregion

        #region Constructor

        public ExifTags(string fileName)
            : this(fileName, true, false)
        {
        }

        public ExifTags(Image image)
        {
            ReadTags(image);
        }

        public ExifTags(string fileName, bool useEmbeddedColorManagement, bool validateImageData)
        {
            using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                var image = Image.FromStream(stream,
                    useEmbeddedColorManagement,
                    validateImageData);

                ReadTags(image);
            }
        }

        #endregion

        #region Public Method

        public static string ReadTag(Image image, ExifFileds exifFiledType)
        {
            return ReadTag(image, (int)exifFiledType);
        }

        #endregion

        #region Helper

        private static string ReadTag(Image image, int propId)
        {
            string value = string.Empty;
            PropertyItem pitem;

            try
            {
                pitem = image.GetPropertyItem(propId);
            }
            catch (Exception ex)
            {


                throw ex;
            }
            

            switch (pitem.Type)
            {
                case 0x1:
                    if (pitem.Value.Length == 4)
                        value = "Version " + pitem.Value[0].ToString() + "." + pitem.Value[1].ToString();
                    else if (pitem.Id == 0x5 && pitem.Value[0] == 0)
                        value = "Sea level";
                    else
                        value = pitem.Value[0].ToString();
                    break;
                case 0x2:
                    value = Ascii.GetString(pitem.Value).Trim('\0');
                    if (pitem.Id == 0x1 || pitem.Id == 0x13)
                        if (value == "N") value = "North latitude";
                        else if (value == "S") value = "South latitude";
                        else value = "reserved";
                    if (pitem.Id == 0x3 || pitem.Id == 0x15)
                        if (value == "E") value = "East longitude";
                        else if (value == "W") value = "West longitude";
                        else value = "reserved";
                    if (pitem.Id == 0x9)
                        if (value == "A") value = "Measurement in progress";
                        else if (value == "V") value = "Measurement Interoperability";
                        else value = "reserved";
                    if (pitem.Id == 0xA)
                        if (value == "2") value = "2-dimensional measurement";
                        else if (value == "3") value = "3-dimensional measurement";
                        else value = "reserved";
                    if (pitem.Id == 0xC || pitem.Id == 0x19)
                        if (value == "K") value = "Kilometers per hour";
                        else if (value == "M") value = "Miles per hour";
                        else if (value == "N") value = "Knots";
                        else value = "reserved";
                    if (pitem.Id == 0xE || pitem.Id == 0x10 || pitem.Id == 0x17)
                        if (value == "T") value = "True direction";
                        else if (value == "M") value = "Magnetic direction";
                        else value = "reserved";
                    break;
                case 0x3:
                    {
                        #region 3 = SHORT (16-bit unsigned int)

                        UInt16 uintval = BitConverter.ToUInt16(pitem.Value, 0);

                        // orientation // lookup table					
                        switch (pitem.Id)
                        {
                            case 0x8827: // ISO speed rating
                                value = "ISO-" + uintval.ToString();
                                break;
                            case 0xA217: // sensing method
                                {
                                    switch (uintval)
                                    {
                                        case 1: value = "Not defined"; break;
                                        case 2: value = "One-chip color area sensor"; break;
                                        case 3: value = "Two-chip color area sensor"; break;
                                        case 4: value = "Three-chip color area sensor"; break;
                                        case 5: value = "Color sequential area sensor"; break;
                                        case 7: value = "Trilinear sensor"; break;
                                        case 8: value = "Color sequential linear sensor"; break;
                                        default: value = " reserved"; break;
                                    }
                                }
                                break;
                            case 0x8822: // 曝光程序
                                switch (uintval)
                                {
                                    case 0: value = "未定义"; break;
                                    case 1: value = "手工"; break;
                                    case 2: value = "标准程序"; break;
                                    case 3: value = "光圈优先"; break;
                                    case 4: value = "快门优先"; break;
                                    case 5: value = "景深优先"; break;
                                    case 6: value = "运动模式"; break;
                                    case 7: value = "肖像模式"; break;
                                    case 8: value = "风景模式"; break;
                                    default: value = "保留"; break;
                                }
                                break;
                            case 0x9207: // 测光模式
                                switch (uintval)
                                {
                                    case 0: value = "未知"; break;
                                    case 1: value = "平均"; break;
                                    case 2: value = "中央重点平均"; break;
                                    case 3: value = "点测"; break;
                                    case 4: value = "多点"; break;
                                    case 5: value = "评估"; break;
                                    case 6: value = "局部"; break;
                                    case 255: value = "其他"; break;
                                    default: value = "保留"; break;
                                }
                                break;
                            case 0x9208: // Light source
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "unknown"; break;
                                        case 1: value = "Daylight"; break;
                                        case 2: value = "Fluorescent"; break;
                                        case 3: value = "Tungsten (incandescent light)"; break;
                                        case 4: value = "Flash"; break;
                                        case 9: value = "Fine weather"; break;
                                        case 10: value = "Cloudy weather"; break;
                                        case 11: value = "Shade"; break;
                                        case 12: value = "Daylight fluorescent (D 5700 – 7100K)"; break;
                                        case 13: value = "Day white fluorescent (N 4600 – 5400K)"; break;
                                        case 14: value = "Cool white fluorescent (W 3900 – 4500K)"; break;
                                        case 15: value = "White fluorescent (WW 3200 – 3700K)"; break;
                                        case 17: value = "Standard light A"; break;
                                        case 18: value = "Standard light B"; break;
                                        case 19: value = "Standard light C"; break;
                                        case 20: value = "D55"; break;
                                        case 21: value = "D65"; break;
                                        case 22: value = "D75"; break;
                                        case 23: value = "D50"; break;
                                        case 24: value = "ISO studio tungsten"; break;
                                        case 255: value = "ISO studio tungsten"; break;
                                        default: value = "other light source"; break;
                                    }
                                }
                                break;
                            case 0x9209: // Flash
                                {
                                    switch (uintval)
                                    {
                                        case 0x0: value = "关闭"; break;
                                        case 0x1: value = "Flash fired"; break;
                                        case 0x5: value = "Strobe return light not detected"; break;
                                        case 0x7: value = "Strobe return light detected"; break;
                                        case 0x9: value = "Flash fired, compulsory flash mode"; break;
                                        case 0xD: value = "Flash fired, compulsory flash mode, return light not detected"; break;
                                        case 0xF: value = "Flash fired, compulsory flash mode, return light detected"; break;
                                        case 0x10: value = "Flash did not fire, compulsory flash mode"; break;
                                        case 0x18: value = "Flash did not fire, auto mode"; break;
                                        case 0x19: value = "Flash fired, auto mode"; break;
                                        case 0x1D: value = "Flash fired, auto mode, return light not detected"; break;
                                        case 0x1F: value = "Flash fired, auto mode, return light detected"; break;
                                        case 0x20: value = "No flash function"; break;
                                        case 0x41: value = "Flash fired, red-eye reduction mode"; break;
                                        case 0x45: value = "Flash fired, red-eye reduction mode, return light not detected"; break;
                                        case 0x47: value = "Flash fired, red-eye reduction mode, return light detected"; break;
                                        case 0x49: value = "Flash fired, compulsory flash mode, red-eye reduction mode"; break;
                                        case 0x4D: value = "Flash fired, compulsory flash mode, red-eye reduction mode, return light not detected"; break;
                                        case 0x4F: value = "Flash fired, compulsory flash mode, red-eye reduction mode, return light detected"; break;
                                        case 0x59: value = "Flash fired, auto mode, red-eye reduction mode"; break;
                                        case 0x5D: value = "Flash fired, auto mode, return light not detected, red-eye reduction mode"; break;
                                        case 0x5F: value = "Flash fired, auto mode, return light detected, red-eye reduction mode"; break;
                                        default: value = "reserved"; break;
                                    }
                                }
                                break;
                            case 0x0128: //ResolutionUnit
                                {
                                    switch (uintval)
                                    {
                                        case 2: value = "Inch"; break;
                                        case 3: value = "Centimeter"; break;
                                        default: value = "No Unit"; break;
                                    }
                                }
                                break;
                            case 0xA409: // Saturation
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Normal"; break;
                                        case 1: value = "Low saturation"; break;
                                        case 2: value = "High saturation"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;

                            case 0xA40A: // Sharpness
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Normal"; break;
                                        case 1: value = "Soft"; break;
                                        case 2: value = "Hard"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA408: // Contrast
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Normal"; break;
                                        case 1: value = "Soft"; break;
                                        case 2: value = "Hard"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0x103: // Compression
                                {
                                    switch (uintval)
                                    {
                                        case 1: value = "Uncompressed"; break;
                                        case 6: value = "JPEG compression (thumbnails only)"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0x106: // PhotometricInterpretation
                                {
                                    switch (uintval)
                                    {
                                        case 2: value = "RGB"; break;
                                        case 6: value = "YCbCr"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0x112: // Orientation
                                {
                                    switch (uintval)
                                    {
                                        case 1: value = "The 0th row is at the visual top of the image, and the 0th column is the visual left-hand side."; break;
                                        case 2: value = "The 0th row is at the visual top of the image, and the 0th column is the visual right-hand side."; break;
                                        case 3: value = "The 0th row is at the visual bottom of the image, and the 0th column is the visual right-hand side."; break;
                                        case 4: value = "The 0th row is at the visual bottom of the image, and the 0th column is the visual left-hand side."; break;
                                        case 5: value = "The 0th row is the visual left-hand side of the image, and the 0th column is the visual top."; break;
                                        case 6: value = "The 0th row is the visual right-hand side of the image, and the 0th column is the visual top."; break;
                                        case 7: value = "The 0th row is the visual right-hand side of the image, and the 0th column is the visual bottom."; break;
                                        case 8: value = "The 0th row is the visual left-hand side of the image, and the 0th column is the visual bottom."; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0x213: // YCbCrPositioning
                                {
                                    switch (uintval)
                                    {
                                        case 1: value = "centered"; break;
                                        case 6: value = "co-sited"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA001: // ColorSpace
                                {
                                    switch (uintval)
                                    {
                                        case 1: value = "sRGB"; break;
                                        case 0xFFFF: value = "Uncalibrated"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA401: // CustomRendered
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Normal process"; break;
                                        case 1: value = "Custom process"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA402: // ExposureMode
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Auto exposure"; break;
                                        case 1: value = "Manual exposure"; break;
                                        case 2: value = "Auto bracket"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA403: // WhiteBalance
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Auto white balance"; break;
                                        case 1: value = "Manual white balance"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA406: // SceneCaptureType
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Standard"; break;
                                        case 1: value = "Landscape"; break;
                                        case 2: value = "Portrait"; break;
                                        case 3: value = "Night scene"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;

                            case 0xA40C: // SubjectDistanceRange
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "unknown"; break;
                                        case 1: value = "Macro"; break;
                                        case 2: value = "Close view"; break;
                                        case 3: value = "Distant view"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0x1E: // GPSDifferential
                                {
                                    switch (uintval)
                                    {
                                        case 0: value = "Measurement without differential correction"; break;
                                        case 1: value = "Differential correction applied"; break;
                                        default: value = "Reserved"; break;
                                    }
                                }
                                break;
                            case 0xA405: // FocalLengthIn35mmFilm
                                value = uintval + " mm";
                                break;
                            default://
                                value = uintval.ToString();
                                break;
                        }
                        #endregion
                    }
                    break;
                case 0x4:
                    value = BitConverter.ToUInt32(pitem.Value, 0).ToString();
                    break;
                case 0x5:
                    {
                        #region 5 = RATIONAL (Two LONGs, unsigned)

                        var rat = new URational(pitem.Value);

                        switch (pitem.Id)
                        {
                            case 0x9202: // ApertureValue
                                value = "F/" + Math.Round(Math.Pow(Math.Sqrt(2), rat.ToDouble()), 2).ToString();
                                break;
                            case 0x9205: // MaxApertureValue
                                value = "F/" + Math.Round(Math.Pow(Math.Sqrt(2), rat.ToDouble()), 2).ToString();
                                break;
                            case 0x920A: // FocalLength
                                value = rat.ToDouble().ToString() + " mm";
                                break;
                            case 0x829D: // F-number
                                value = "F/" + rat.ToDouble().ToString();
                                break;
                            case 0x11A: // Xresolution
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x11B: // Yresolution
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x829A: // ExposureTime
                                value = rat.ToString() + " sec";
                                break;
                            case 0x2: // GPSLatitude                                
                                value = new GPSRational(pitem.Value).ToString();
                                break;
                            case 0x4: // GPSLongitude
                                value = new GPSRational(pitem.Value).ToString();
                                break;
                            case 0x6: // GPSAltitude
                                value = rat.ToDouble() + " meters";
                                break;
                            case 0xA404: // Digital Zoom Ratio
                                value = rat.ToDouble().ToString();
                                if (value == "0") value = "none";
                                break;
                            case 0xB: // GPSDOP
                                value = rat.ToDouble().ToString();
                                break;
                            case 0xD: // GPSSpeed
                                value = rat.ToDouble().ToString();
                                break;
                            case 0xF: // GPSTrack
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x11: // GPSImgDir
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x14: // GPSDestLatitude
                                value = new GPSRational(pitem.Value).ToString();
                                break;
                            case 0x16: // GPSDestLongitude
                                value = new GPSRational(pitem.Value).ToString();
                                break;
                            case 0x18: // GPSDestBearing
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x1A: // GPSDestDistance
                                value = rat.ToDouble().ToString();
                                break;
                            case 0x7: // GPSTimeStamp                                
                                value = new GPSRational(pitem.Value).ToString(":");
                                break;

                            default:
                                value = rat.ToString();
                                break;
                        }

                        #endregion
                    }
                    break;
                case 0x7:
                    switch (pitem.Id)
                    {
                        case 0xA300: //FileSource
                            {
                                value = pitem.Value[0] == 3 ? "DSC" : "reserved";
                                break;
                            }
                        case 0xA301: //SceneType
                            value = pitem.Value[0] == 1 ? "A directly photographed image" : "reserved";
                            break;
                        case 0x9000:// Exif Version
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            break;
                        case 0xA000: // Flashpix Version
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            value = value == "0100" ? "Flashpix Format Version 1.0" : "reserved";
                            break;
                        case 0x9101: //ComponentsConfiguration
                            value = GetComponentsConfig(pitem.Value);
                            break;
                        case 0x927C: //MakerNote
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            break;
                        case 0x9286: //UserComment
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            break;
                        case 0x1B: //GPS Processing Method
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            break;
                        case 0x1C: //GPS Area Info
                            value = Ascii.GetString(pitem.Value).Trim('\0');
                            break;
                        default:
                            value = "-";
                            break;
                    }
                    break;
                case 0x9:
                    value = BitConverter.ToInt32(pitem.Value, 0).ToString();
                    break;
                case 0xA:
                    {
                        #region 10 = SRATIONAL (Two SLONGs, signed)

                        var rat = new Rational(pitem.Value);

                        switch (pitem.Id)
                        {
                            case 0x9201: // ShutterSpeedValue
                                value = "1/" + Math.Round(Math.Pow(2, rat.ToDouble()), 2).ToString();
                                break;
                            case 0x9203: // BrightnessValue
                                value = Math.Round(rat.ToDouble(), 4).ToString();
                                break;
                            case 0x9204: // ExposureBiasValue
                                value = Math.Round(rat.ToDouble(), 2).ToString() + " eV";
                                break;
                            default:
                                value = rat.ToString();
                                break;
                        }
                        #endregion
                    }
                    break;
            }

            return value;
        }

        private void ReadTags(Image image)
        {
            var tagsInFile = from tag in SupportedTags
                             join id in image.PropertyIdList on tag.Id equals id
                             select tag;

            foreach (var tag in tagsInFile)
            {
                tag.Value = ReadTag(image, tag.Id);
                Add(tag.FieldName, tag);
            }
        }

        private static string GetComponentsConfig(byte[] bytes)
        {
            var s = string.Empty;
            var vals = new[] { "", "Y", "Cb", "Cr", "R", "G", "B" };

            foreach (var b in bytes)
                s += vals[b];

            return s;
        }

        #endregion


    }

    #endregion
}
