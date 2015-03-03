namespace Yintai.Architecture.ImageTool.Models.Exif
{
    /// <summary>
    /// Exif 枚举
    /// </summary>
    public enum ExifFileds
    {
        /// <summary>
        /// Image width
        /// </summary>
        ImageWidth = 0x100,

        /// <summary>
        /// Image height
        /// </summary>
        ImageHeight = 0x101,

        /// <summary>
        /// GPS tag version
        /// </summary>
        GPSVersionID = 0x0,

        /// <summary>
        /// Altitude reference
        /// </summary>
        GPSAltitudeRef = 0x5,

        /// <summary>
        /// Image data location
        /// </summary>
        StripOffsets = 0x111,

        /// <summary>
        /// Number of rows per strip
        /// </summary>
        RowsPerStrip = 0x116,

        /// <summary>
        /// Bytes per compressed strip
        /// </summary>
        StripByteCounts = 0x117,

        /// <summary>
        /// Valid image height
        /// </summary>
        PixelXDimension = 0xA003,

        /// <summary>
        /// Number of bits per component
        /// </summary>
        BitsPerSample = 0x102,

        /// <summary>
        /// Compression scheme
        /// </summary>
        Compression = 0x103,

        /// <summary>
        /// Pixel composition
        /// </summary>
        PhotometricInterpretation = 0x106,

        /// <summary>
        /// Orientation of image
        /// </summary>
        Orientation = 0x112,

        /// <summary>
        /// Number of components
        /// </summary>
        SamplesPerPixel = 0x115,

        /// <summary>
        /// Image data arrangement
        /// </summary>
        PlanarConfiguration = 0x11C,

        /// <summary>
        /// Subsampling ratio of Y to C
        /// </summary>
        YCbCrSubSampling = 0x212,

        /// <summary>
        /// Y and C positioning
        /// </summary>
        YCbCrPositioning = 0x213,

        /// <summary>
        /// Unit of X and Y resolution
        /// </summary>
        ResolutionUnit = 0x128,

        /// <summary>
        /// Transfer function
        /// </summary>
        TransferFunction = 0x12D,

        /// <summary>
        /// Color space information
        /// </summary>
        ColorSpace = 0xA001,

        /// <summary>
        /// Exposure program
        /// </summary>
        ExposureProgram = 0x8822,

        /// <summary>
        /// ISO speed rating
        /// </summary>
        ISOSpeedRatings = 0x8827,

        /// <summary>
        /// Metering mode
        /// </summary>
        MeteringMode = 0x9207,

        /// <summary>
        /// Light source
        /// </summary>
        LightSource = 0x9208,

        /// <summary>
        /// Flash
        /// </summary>
        Flash = 0x9209,

        /// <summary>
        /// Subject area
        /// </summary>
        SubjectArea = 0x9214,

        /// <summary>
        /// Focal plane resolution unit
        /// </summary>
        FocalPlaneResolutionUnit = 0xA210,

        /// <summary>
        /// Subject location
        /// </summary>
        SubjectLocation = 0xA214,

        /// <summary>
        /// Sensing method
        /// </summary>
        SensingMethod = 0xA217,

        /// <summary>
        /// Custom image processing
        /// </summary>
        CustomRendered = 0xA401,

        /// <summary>
        /// Exposure mode
        /// </summary>
        ExposureMode = 0xA402,

        /// <summary>
        /// White balance
        /// </summary>
        WhiteBalance = 0xA403,

        /// <summary>
        /// Focal length in 35 mm film
        /// </summary>
        FocalLengthIn35mmFilm = 0xA405,

        /// <summary>
        /// Scene capture type
        /// </summary>
        SceneCaptureType = 0xA406,

        /// <summary>
        /// Contrast
        /// </summary>
        Contrast = 0xA408,

        /// <summary>
        /// Saturation
        /// </summary>
        Saturation = 0xA409,

        /// <summary>
        /// Sharpness
        /// </summary>
        Sharpness = 0xA40A,

        /// <summary>
        /// Subject distance range
        /// </summary>
        SubjectDistanceRange = 0xA40C,

        /// <summary>
        /// GPS differential correction
        /// </summary>
        GPSDifferential = 0x1E,

        /// <summary>
        /// Shutter speed
        /// </summary>
        ShutterSpeedValue = 0x9201,

        /// <summary>
        /// Brightness
        /// </summary>
        BrightnessValue = 0x9203,

        /// <summary>
        /// Exposure bias
        /// </summary>
        ExposureBiasValue = 0x9204,

        /// <summary>
        /// Offset to JPEG SOI
        /// </summary>
        JPEGInterchangeFormat = 0x201,

        /// <summary>
        /// Bytes of JPEG data
        /// </summary>
        JPEGInterchangeFormatLength = 0x202,

        /// <summary>
        /// Image resolution in width direction
        /// </summary>
        XResolution = 0x11A,

        /// <summary>
        /// Image resolution in height direction
        /// </summary>
        YResolution = 0x11B,

        /// <summary>
        /// White point chromaticity
        /// </summary>
        WhitePoint = 0x13E,

        /// <summary>
        /// Chromaticities of primaries
        /// </summary>
        PrimaryChromaticities = 0x13F,

        /// <summary>
        /// Color space transformation matrix coefficients
        /// </summary>
        YCbCrCoefficients = 0x211,

        /// <summary>
        /// Pair of black and white reference values
        /// </summary>
        ReferenceBlackWhite = 0x214,

        /// <summary>
        /// Image compression mode
        /// </summary>
        CompressedBitsPerPixel = 0x9102,

        /// <summary>
        /// Exposure time
        /// </summary>
        ExposureTime = 0x829A,

        /// <summary>
        /// F number
        /// </summary>
        FNumber = 0x829D,

        /// <summary>
        /// Aperture
        /// </summary>
        ApertureValue = 0x9202,

        /// <summary>
        /// Maximum lens aperture
        /// </summary>
        MaxApertureValue = 0x9205,

        /// <summary>
        /// Subject distance
        /// </summary>
        SubjectDistance = 0x9206,

        /// <summary>
        /// Lens focal length
        /// </summary>
        FocalLength = 0x920A,

        /// <summary>
        /// Flash energy
        /// </summary>
        FlashEnergy = 0xA20B,

        /// <summary>
        /// Focal plane X resolution
        /// </summary>
        FocalPlaneXResolution = 0xA20E,

        /// <summary>
        /// Focal plane Y resolution
        /// </summary>
        FocalPlaneYResolution = 0xA20F,

        /// <summary>
        /// Exposure index
        /// </summary>
        ExposureIndex = 0xA215,

        /// <summary>
        /// Digital zoom ratio
        /// </summary>
        DigitalZoomRatio = 0xA404,

        /// <summary>
        /// Gain control
        /// </summary>
        GainControl = 0xA407,

        /// <summary>
        /// Latitude
        /// </summary>
        GPSLatitude = 0x2,

        /// <summary>
        /// Longitude
        /// </summary>
        GPSLongitude = 0x4,

        /// <summary>
        /// Altitude
        /// </summary>
        GPSAltitude = 0x6,

        /// <summary>
        /// GPS time (atomic clock)
        /// </summary>
        GPSTimeStamp = 0x7,

        /// <summary>
        /// Measurement precision
        /// </summary>
        GPSDOP = 0xB,

        /// <summary>
        /// Speed of GPS receiver
        /// </summary>
        GPSSpeed = 0xD,

        /// <summary>
        /// Direction of movement
        /// </summary>
        GPSTrack = 0xF,

        /// <summary>
        /// Direction of image
        /// </summary>
        GPSImgDirection = 0x11,

        /// <summary>
        /// Latitude of destination
        /// </summary>
        GPSDestLatitude = 0x14,

        /// <summary>
        /// Longitude of destination
        /// </summary>
        GPSDestLongitude = 0x16,

        /// <summary>
        /// Bearing of destination
        /// </summary>
        GPSDestBearing = 0x18,

        /// <summary>
        /// Distance to destination
        /// </summary>
        GPSDestDistance = 0x1A,

        /// <summary>
        /// File change date and time
        /// </summary>
        DateTime = 0x132,

        /// <summary>
        /// Image title
        /// </summary>
        ImageDescription = 0x10E,

        /// <summary>
        /// Image input equipment manufacturer
        /// </summary>
        Make = 0x10F,

        /// <summary>
        /// Image input equipment model
        /// </summary>
        Model = 0x110,

        /// <summary>
        /// Software used
        /// </summary>
        Software = 0x131,

        /// <summary>
        /// Person who created the image
        /// </summary>
        Artist = 0x13B,

        /// <summary>
        /// Copyright holder
        /// </summary>
        Copyright = 0x8298,

        /// <summary>
        /// Related audio file
        /// </summary>
        RelatedSoundFile = 0xA004,

        /// <summary>
        /// Date and time of original data generation
        /// </summary>
        DateTimeOriginal = 0x9003,

        /// <summary>
        /// /Date and time of digital data generation
        /// </summary>
        DateTimeDigitized = 0x9004,

        /// <summary>
        /// DateTime subseconds
        /// </summary>
        SubSecTime = 0x9290,

        /// <summary>
        /// DateTimeOriginal subseconds
        /// </summary>
        SubSecTimeOriginal = 0x9291,

        /// <summary>
        /// DateTimeDigitized subseconds
        /// </summary>
        SubSecTimeDigitized = 0x9292,

        /// <summary>
        /// Unique image ID
        /// </summary>
        ImageUniqueID = 0xA420,

        /// <summary>
        /// Spectral sensitivity
        /// </summary>
        SpectralSensitivity = 0x8824,

        /// <summary>
        /// North or South Latitude
        /// </summary>
        GPSLatitudeRef = 0x1,

        /// <summary>
        /// East or West Longitude
        /// </summary>
        GPSLongitudeRef = 0x3,

        /// <summary>
        /// GPS satellites used for measurement
        /// </summary>
        GPSSatellites = 0x8,

        /// <summary>
        /// GPS receiver status
        /// </summary>
        GPSStatus = 0x9,

        /// <summary>
        /// GPS measurement mode
        /// </summary>
        GPSMeasureMode = 0xA,

        /// <summary>
        /// Speed unit
        /// </summary>
        GPSSpeedRef = 0xC,

        /// <summary>
        /// Reference for direction of movement
        /// </summary>
        GPSTrackRef = 0xE,

        /// <summary>
        /// Reference for direction of image
        /// </summary>
        GPSImgDirectionRef = 0x10,

        /// <summary>
        /// Geodetic survey data used
        /// </summary>
        GPSMapDatum = 0x12,

        /// <summary>
        /// Reference for latitude of destination
        /// </summary>
        GPSDestLatitudeRef = 0x13,

        /// <summary>
        /// Reference for longitude of destination
        /// </summary>
        GPSDestLongitudeRef = 0x15,

        /// <summary>
        /// Reference for bearing of destination
        /// </summary>
        GPSDestBearingRef = 0x17,

        /// <summary>
        /// Reference for distance to destination
        /// </summary>
        GPSDestDistanceRef = 0x19,

        /// <summary>
        /// "GPS date
        /// </summary>
        GPSDateStamp = 0x1D,

        /// <summary>
        /// Optoelectric conversion factor
        /// </summary>
        OECF = 0x8828,

        /// <summary>
        /// Spatial frequency response
        /// </summary>
        SpatialFrequencyResponse = 0xA20C,

        /// <summary>
        /// File source
        /// </summary>
        FileSource = 0xA300,

        /// <summary>
        /// Scene type"
        /// </summary>
        SceneType = 0xA301,

        /// <summary>
        /// CFA pattern
        /// </summary>
        CFAPattern = 0xA302,

        /// <summary>
        /// Device settings description
        /// </summary>
        DeviceSettingDescription = 0xA40B,

        /// <summary>
        /// Exif version
        /// </summary>
        ExifVersion = 0x9000,

        /// <summary>
        /// Supported Flashpix version
        /// </summary>
        FlashpixVersion = 0xA000,

        /// <summary>
        /// Meaning of each component
        /// </summary>
        ComponentsConfiguration = 0x9101,

        /// <summary>
        /// Manufacturer notes
        /// </summary>
        MakerNote = 0x927C,

        /// <summary>
        /// User comments
        /// </summary>
        UserComment = 0x9286,

        /// <summary>
        /// Name of GPS processing method
        /// </summary>
        GPSProcessingMethod = 0x1B,

        /// <summary>
        /// Name of GPS area
        /// </summary>
        GPSAreaInformation = 0x1C
    }
}