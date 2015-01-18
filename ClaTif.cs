/////////////////////////////////////////////////////////////////////////////////////////
// ClaTif Class Library
//
//  Copyright (c) 2015  Masashi Sonobe
//
//  This class library is released under the MIT License. 
//
//  http://opensource.org/licenses/MIT
/////////////////////////////////////////////////////////////////////////////////////////


using System;
using System.IO;   //フォルダ取得，ファイル存在確認のために追加

namespace Snb
{
namespace Image
{
    #region ImgSet Class
    /// <summary>
    ///  ImgSetクラスで使用している画像を示すための列挙型
    ///  Show the image type using at ImgSet Class
    /// </summary>
    public enum ImgType : int
    {
        notInitialized = 0,

        bilevelImg,
        gryImg_8,
        gryImg_16,
        gryImg_32,
        gryImg_64,

        bilevelImg_CompMH_CCITT1D, // Tif
        bilevelImg_CompMR_G3Fax,   // Tif
        bilevelImg_CompMMR_G4Fax,  // Tif

        gryImg_Sgl, //32bit 浮動小数点 float  Tif
        gryImg_Dbl, //64bit 浮動小数点 double Tif

        rgbImg_24,
        rgbImg_48int,
        rgbImg_96sgl,

        notImplement,
    }

    /// <summary>
    /// 画像変数をまとめて扱うためのクラス
    /// Class for manupulating many different type image as one variable
    /// </summary>
    public class ImgSet : IDisposable
    {
        public ImgType iImgType;       // 使用中の画像タイプを示す変数, show using type
        public int compression;    // 圧縮を示す変数 
        public int photometric;    // 白黒の値を示す変数
        public int sampleFormat;   // 同じ32bitでも, singleで32bitか, UInt32で32bitかを見分ける場合に必要


        public UInt32 imageWidth;
        public UInt32 imageLength;
        public UInt32 buffSize;

        public Byte[] blvImg;

        public Byte[] gryImg_8;
        public UInt16[] gryImg_16;
        public UInt32[] gryImg_32;
        public Single[] gryImg_Sgl; //拡張予定
        public Double[] gryImg_Dbl; //拡張予定

        public Byte[] rgbImg24_R;
        public Byte[] rgbImg24_G;
        public Byte[] rgbImg24_B;


        public ImgSet()
        {
            iImgType     = ImgType.notInitialized;
            compression  = 1; //UnCompressed- for Tif
            photometric  = 0;
            sampleFormat = 1; //SampleFormatUnsignedInteger

            imageWidth  = 0;
            imageLength = 0;
            buffSize    = 0;

            blvImg      = null;

            gryImg_8    = null;
            gryImg_16   = null;
            gryImg_32   = null;
            gryImg_Sgl  = null; //拡張予定
            gryImg_Dbl  = null; //拡張予定


            rgbImg24_R  = null;
            rgbImg24_G  = null;
            rgbImg24_B  = null;

        }

        public void Dispose()
        {
            iImgType     = ImgType.notInitialized;
            compression  = 1; //UnCompressed- for Tif
            photometric  = 0;
            sampleFormat = 1; //SampleFormatUnsignedInteger

            imageWidth  = 0;
            imageLength = 0;
            buffSize    = 0;

            #region Clear Img

            blvImg      = null;

            gryImg_8    = null;
            gryImg_16   = null;
            gryImg_32   = null;
            gryImg_Sgl  = null; //拡張予定
            gryImg_Dbl  = null; //拡張予定


            rgbImg24_R  = null;
            rgbImg24_G  = null;
            rgbImg24_B  = null;

            #endregion Clear Img

        }
    }
    #endregion ImgSet Class

namespace TifRW
{

    #region enums for Tif

    /// <summary>
    /// Field Type 
    ///     _ Implementation of Tiff v.6.0 p15-p16  
    /// </summary>
    public enum TifFieldType : ushort
    {
        BYTE       = 1, //  8bit(1byte) unsigned integer
        ASCII     =  2,
        SHORT     =  3, // 16bit(2byte) unsigned integer
        LONG      =  4, // 32bit(4byte) unsigned integer
        RATIONAL  =  5, // Two LONGs    the first :fraction , the second: the denominator
        SBYTE     =  6, // an 8-bit signed (twos-complement) integer.
        UNDEFINED =  7, // an 8-bit byte that may contain anything, depending on the definition of the field.
        SSHORT    =  8, // a 16-bit (2-byte) signed (twos-complement) integer.
        SLONG     =  9, // a 32-bit (4-byte) signed (twos-complement) integer.
        SRATIONAL = 10, // Two SLONGs
        FLOAT     = 11, // Single precision(4-byte) IEEE format.
        DOUBLE    = 12, // Double precision(8-byte) IEEE format.



        BYTE_OR_SHORT = 100,
        SHORT_OR_LONG = 101,
        ANY           = 200,
    }

    /// <summary>
    /// Tag Name (Tifタグの番号)
    ///    _ Implementation of Tiff v.6.0 p117-p118
    /// </summary>
    public enum TagName : ushort
    {
        NewSubfileType = 254,
        SubfileType = 255,
        ImageWidth = 256,
        ImageLength = 257,
        BitsPerSample = 258,
        Compression = 259,
        PhotometricInterpretation = 262,
        Threshholding = 263,
        CellWidth = 264,
        CellLength = 265,
        FillOrder = 266,
        DocumentName = 269,
        ImageDescription = 270,
        Make = 271,
        Model = 272,
        StripOffsets = 273,
        Orientation = 274,
        SamplesPerPixel = 277,
        RowsPerStrip = 278,
        StripByteCounts = 279,
        MinSampleValue = 280,
        MaxSampleValue = 281,
        XResolution = 282,
        YResolution = 283,
        PlanarConfiguration = 284,
        PageName = 285,
        XPosition = 286,
        YPosition = 287,
        FreeOffsets = 288,
        FreeByteCounts = 289,
        GrayResponseUnit = 290,

        GrayResponseCurve = 291,
        T4Options = 292,
        T6Options = 293,
        ResolutionUnit = 296,
        PageNumber = 297,
        TransferFunction = 301,
        Software = 305,
        DateTime = 306,
        Artist = 315,
        HostComputer = 316,
        Predictor = 317,
        WhitePoint = 318,
        PrimaryChromaticities = 319,
        ColorMap = 320,
        HalftoneHints = 321,
        TileWidth = 322,
        TileLength = 323,
        TileOffsets = 324,
        TileByteCounts = 325,
        InkSet = 332,
        InkNames = 333,

        NumberOfInks = 334,
        DotRange = 336,
        TargetPrinter = 337,
        ExtraSamples = 338,
        SampleFormat = 339,
        SMinSampleValue = 340,
        SMaxSampleValue = 341,
        TransferRange = 342,


        JPEGProc = 512,
        JPEGInterchangeFormat = 513,
        JPEGInterchangeFormatLngth = 514,
        JPEGRestartInterval = 515,
        JPEGLosslessPredictors = 517,
        JPEGPointTransforms = 518,
        JPEGQTables = 519,
        JPEGDCTables = 520,
        JPEGACTables = 521,
        YCbCrCoefficients = 529,
        YCbCrSubSampling = 530,
        YCbCrPositioning = 531,
        ReferenceBlackWhite = 532,
        Copyright = 33432,

    }



    /// <summary>
    /// Tag Data Type (Tifタグのデータ型)
    ///    _ Implementation of Tiff v.6.0 p117-p118
    /// </summary>
    public enum TagType : ushort
    {
        NewSubfileType = TifFieldType.LONG,
        SubfileType = TifFieldType.SHORT,
        ImageWidth = TifFieldType.SHORT_OR_LONG,
        ImageLength = TifFieldType.SHORT_OR_LONG,
        BitsPerSample = TifFieldType.SHORT,
        Compression = TifFieldType.SHORT,

        PhotometricInterpretation = TifFieldType.SHORT,

        Threshholding = TifFieldType.SHORT,
        CellWidth = TifFieldType.SHORT,
        CellLength = TifFieldType.SHORT,
        FillOrder = TifFieldType.SHORT,

        DocumentName = TifFieldType.ASCII,
        ImageDescription = TifFieldType.ASCII,
        Make = TifFieldType.ASCII,
        Model = TifFieldType.ASCII,

        StripOffsets = TifFieldType.SHORT_OR_LONG,

        Orientation = TifFieldType.SHORT,
        SamplesPerPixel = TifFieldType.SHORT,
        RowsPerStrip = TifFieldType.SHORT_OR_LONG,
        StripByteCounts = TifFieldType.SHORT_OR_LONG,

        MinSampleValue = TifFieldType.SHORT,
        MaxSampleValue = TifFieldType.SHORT,
        XResolution = TifFieldType.RATIONAL,
        YResolution = TifFieldType.RATIONAL,

        PlanarConfiguration = TifFieldType.SHORT,
        PageName = TifFieldType.ASCII,
        XPosition = TifFieldType.RATIONAL,
        YPosition = TifFieldType.RATIONAL,

        FreeOffsets = TifFieldType.LONG,
        FreeByteCounts = TifFieldType.LONG,
        GrayResponseUnit = TifFieldType.SHORT,


        GrayResponseCurve = TifFieldType.SHORT,
        T4Options = TifFieldType.LONG,
        T6Options = TifFieldType.LONG,
        ResolutionUnit = TifFieldType.SHORT,
        PageNumber = TifFieldType.SHORT,
        TransferFunction = TifFieldType.SHORT,

        Software = TifFieldType.ASCII,
        DateTime = TifFieldType.ASCII,
        Artist = TifFieldType.ASCII,
        HostComputer = TifFieldType.ASCII,

        Predictor = TifFieldType.SHORT,
        WhitePoint = TifFieldType.RATIONAL,
        PrimaryChromaticities = TifFieldType.RATIONAL,

        ColorMap = TifFieldType.SHORT,
        HalftoneHints = TifFieldType.SHORT,

        TileWidth = TifFieldType.SHORT_OR_LONG,
        TileLength = TifFieldType.SHORT_OR_LONG,
        TileOffsets = TifFieldType.LONG,
        TileByteCounts = TifFieldType.SHORT_OR_LONG,

        InkSet = TifFieldType.SHORT,
        InkNames = TifFieldType.ASCII,

        NumberOfInks = TifFieldType.SHORT,
        DotRange = TifFieldType.BYTE_OR_SHORT,

        TargetPrinter = TifFieldType.ASCII,
        ExtraSamples = TifFieldType.BYTE,

        SampleFormat = TifFieldType.SHORT,
        SMinSampleValue = TifFieldType.ANY,
        SMaxSampleValue = TifFieldType.ANY,
        TransferRange = TifFieldType.SHORT,


        JPEGProc = TifFieldType.SHORT,
        JPEGInterchangeFormat = TifFieldType.LONG,
        JPEGInterchangeFormatLngth = TifFieldType.LONG,
        JPEGRestartInterval = TifFieldType.SHORT,
        JPEGLosslessPredictors = TifFieldType.SHORT,
        JPEGPointTransforms = TifFieldType.SHORT,

        JPEGQTables = TifFieldType.LONG,
        JPEGDCTables = TifFieldType.LONG,
        JPEGACTables = TifFieldType.LONG,

        YCbCrCoefficients = TifFieldType.RATIONAL,
        YCbCrSubSampling = TifFieldType.SHORT,
        YCbCrPositioning = TifFieldType.SHORT,
        ReferenceBlackWhite = TifFieldType.LONG,

        Copyright = 33432,

    }


    /// <summary>
    /// PhotometricInterpretation Enum (ピクセルの白黒定義方式)
    ///  _ Implementation of Tiff v.6.0 p117-p118
    /// </summary>
    public enum PhotometricInterpretation : int
    {
        WhiteIsZero = 0,
        BlackIsZero = 1,
        RGB = 2,
        RGBPalette = 3,
        TransparencyMask = 4,
        CMYK = 5,
        YCbCr = 6,
        CIELab = 7,

    }

    /// <summary>
    /// Compression Method Enum (圧縮方式の定義)
    ///  _ Implementation of Tiff v.6.0 p117-p118
    /// </summary>
    public enum TifCompression : int
    {
        Uncompressed = 1,
        CCITT1D = 2,
        Group3Fax = 3,
        Group4Fax = 4,
        LZW = 5,
        JPEG = 6,
        PackBits = 32773,
    }

    /// <summary>
    /// Specifies how to interpret each data sample in a pixel (１ピクセルの中身が整数型か，浮動少数型かを見分ける)
    ///  _ Implementation of Tiff v.6.0 p80
    /// </summary>
    public enum SampleFormat : int
    {
        UnsignedInteger = 1,
        TwosComlementSignedInteger = 2,//2の補数からなる符号を持つ整数
        IEEEfloatingPoint = 3,
        UndefinedDataFormat = 4,
    }

    #endregion enums for Tif

    #region Elemental Class for ClaTiff



    /// <summary>
    /// Image File Header
    ///     _ Implementation of Tiff v.6.0 p13-p14  
    /// </summary>
    public class IFH
    {
        public Byte[] byteOrder;       // II or MM                 (2-byte)
        public UInt16 tiffID;          // 42                       (2-byte)
        public UInt32 firstIFDOffset;  // File Offset of first IFD (4-byte)

        public string byteOrderStr;    // string of byteOreder-Field
        public string tiffIDStr;       // string of tiffID-Field

        public IFH()
        {
            byteOrder = new Byte[2];
            tiffID = 0;
            firstIFDOffset = 0;

            byteOrderStr = "";
            tiffIDStr = "";
        }
    }


    /// <summary>
    /// Image File Directory  
    ///     _ Implementation of Tiff v.6.0 p14  
    /// </summary>
    public class IFD
    {
        public UInt16 numOfDirectoryEntries; // (2-byte)
        public IFDEntry[] directoryEntry;
        public UInt32 offsetOfNextIFD;       // (4-byte)

        public IFD()
        {
            numOfDirectoryEntries = 0;
            directoryEntry = null;
            offsetOfNextIFD = 0;
        }

    }


    /// <summary>
    /// IFD Entry
    ///    _ Implementation of Tiff v6.0 p 14
    /// </summary>
    public class IFDEntry
    {

        public UInt16 tagNumber;      // Identify Tag Number  (2-byte)
        public UInt16 tagType;        // field Type           (2-byte)
        public UInt32 count;          // the number of values (4-byte)
        public UInt32 value_offset;   // value or file offset for the value field (4-byte) 

        public string tagNameStr;     // Name Of Tag Number
        public string tagTypeStr;     // Name Of Tag Type 

        public IFDEntry()
        {
            tagNumber = 0;
            tagType = 0;
            count = 0;
            value_offset = 0;

            tagNameStr = "";
            tagTypeStr = "";
        }

        public void Reset()
        {
            tagNumber = 0;
            tagType = 0;
            count = 0;
            value_offset = 0;

            tagNameStr = "";
            tagTypeStr = "";
        }

    }


    public class OffsetValues
    {

        public UInt32 xResolution1;
        public UInt32 xResolution2;
        public UInt32 yResolution1;
        public UInt32 yResolution2;

        public UInt32[] stripOffsets;
        public UInt32[] stripByteCounts;

        public Byte[] softwareName;
        public Byte[] dateTime;

        public OffsetValues()
        {
            xResolution1 = 0;
            xResolution2 = 0;
            yResolution1 = 0;
            yResolution2 = 0;

            stripOffsets = null;
            stripByteCounts = null;

            softwareName = null;
            dateTime = null;
        }
    }

    public class RequiredValues
    {
        public UInt32[] bitsPerSample;
        public UInt32 photometricInterpretation;
        public UInt32 samplesPerPixel;

        public RequiredValues()
        {
            bitsPerSample = new UInt32[3];

            for (int i = 0; i < 3; ++i) bitsPerSample[i] = 0;

            photometricInterpretation = 0;
            samplesPerPixel = 0;
        }
    }


    #endregion Elemental Class for ClaTiff


    #region ClaTiffErr-enum
    /// <summary>
    /// エラーを扱うためのenum
    /// </summary>
    public enum ClaTiffErr : int
    {
        NoError = 0,

        Err_ReadTif_BufAllocException,

        Err_ReadTif_ArgumentException,
        Err_ReadTif_PathTooLongException,
        Err_ReadTif_NoDirectory,
        Err_ReadTif_NoFile,

        NoError_ReadTif_CheckDirectoryAndFile_End_Suspned,


        Err_ReadTif_ReadImageFileHeader,
        Err_ReadTif_ReadImageFileHeader_ByteOrder,
        Err_ReadTif_ReadImageFileHeader_AbnormalByteOrder,
        Err_ReadTif_CheckTiffID_UndifinedID,

        NoError_ReadTif_ReadImageHeader_End_Suspned,


        Err_ReadTif_ReadIFD_ReadImageFileDirectories,

        NoError_ReadTif_ReadIFD_End_Suspned,


        Err_ReadTif_Alloc_IFD_and_ImgSet,

        NoError_ReadTif_AllocIFDAndImgSet_End_Suspned,


        Err_ReadTif_ReadImaegDirectoryEntry_ReadImageDirectoryEntries,
        Err_ReadTif_ReadImaegDirectoryEntry_AllocDirectoryEntries,

        NoError_ReadTif_ReadImaegDirectoryEntry_End_Suspned,


        Err_ReadTif_NameTypeInterpreterError,
        Err_ReadTif_NameTypeSelectionError,


        Err_ReadTif_ReadStripOffsets,
        Err_ReadTif_ReadStripByteCounts,
        Err_ReadTif_ReadValueSetLongerThan4Bytes,

        NoError_ReadTif_ReadValueSetLongerThan4Bytes_End_Suspned,



        Err_ReadTif_ZeroSizeImageBuffer,
        Err_ReadTif_GetImageDataFromRequiredFiled,


        Err_ReadTif_CheckAndSetImageTypeForGray,
        Err_ReadTif_CheckAndSetImageTypeForColor,

        NoError_ReadTif_CheckAndSetImageType_End_Suspned,

        Err_ReadTif_AllocImageSet_NotImplement,
        Err_ReadTif_AllocImageSet,

        NoError_ReadTif_AllocImageSet_End_Suspned,


        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_bilevel_gray,
        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_8bit_gray,
        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_16bit_gray,
        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_32bit_gray_int,
        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_32bit_gray_float,
        Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_24bit_rgb,
        Err_ReadTif_CheckStripByteCounts_NotImplement,
        Err_ReadTif_CheckStripByteCounts_NotInitialized,
        Err_ReadTif_CheckStripByteCounts,

        NoError_ReadTif_CheckStripByteCounts_End_Suspned,


        Err_ReadTif_ReadImageDataFromFile,
        Err_ReadTif_ReadImageDataFromFile_NotImplementTypeImage,
        Err_ReadTif_ReadImageDataFromFile_NotImplementCompress,

        NoError_ReadTif_ReadImageData_End_Suspned,


        Err_ReadAndGet2ByteValue_FileRewindError,
        Err_ReadAndGet2ByteValue_FileRewindCheckError,
        Err_ReadAndGet2ByteValue_BuffAllocException,
        Err_ReadAndGet2ByteValue_SetFilePositionException,
        Err_ReadAndGet2ByteValue_SetFilePositionCheck,
        Err_ReadAndGet2ByteValue_ConvertByteArgumentNullException,
        Err_ReadAndGet2ByteValue_ConvertByteArgumentOutOfRangeException,

        Err_ReadAndGet4ByteValue_FileRewindError,
        Err_ReadAndGet4ByteValue_FileRewindCheckError,
        Err_ReadAndGet4ByteValue_BuffAllocException,
        Err_ReadAndGet4ByteValue_SetFilePositionException,
        Err_ReadAndGet4ByteValue_SetFilePositionCheck,
        Err_ReadAndGet4ByteValue_ConvertByteArgumentNullException,
        Err_ReadAndGet4ByteValue_ConvertByteArgumentOutOfRangeException,

        Err_ChangeEndian_InputDataNull,
        Err_ChangeEndian_OuputDataNull,
        Err_ChangeEndian_BufferSizeUnMatch,
        Err_ChangeEndian_RearrengeUnknownError,

        Err_InitializeByteBuffer_InitializeErr,

        Err_CopyBuffer_ManipulateErr,
    }
    #endregion ClaTiffErr-enum

    #region ClaTiffSuspendFlag - enum
    /// <summary>
    /// 処理を途中で停止するためのフラグ(画像のタグのみ解析したい場合に使用)
    /// </summary>
    public enum ClaTiffSuspendFlag : int
    {
        ReadTif_No_Suspend = 0,

        ReadTif_CheckDirectoryAndFile_End_Suspend,
        ReadTif_ReadImageHeader_End_Suspend,
        ReadTif_ReadIFD_End_Suspend,
        ReadTif_AllocIFDAndImgSet_End_Suspend,
        ReadTif_ReadImaegDirectoryEntry_End_Suspend,
        ReadTif_ReadValueSetLongerThan4Bytes_End_Suspend,
        ReadTif_GetImageDataFromRequiredFiled,
        ReadTif_CheckAndSetImageType_End_Suspend,
        ReadTif_AllocImgset_End_Suspend,
        ReadTif_CheckStripByteCounts_End_Suspend,
        ReadTif_ReadImageData_End_Suspend,
    }
    #endregion ClaTiffSuspendFlag - enum

    /// <summary>
    /// Tiffを扱うためのクラス
    /// Class for manupulating Tiff Image
    /// </summary>
    public class ClaTif : IDisposable
    {

        public string mFileNameFullPath;
        public IFH mImageFileHeader;
        public IFD[] mImageFileDirectory;


        public ImgSet[] imgData;
        public Int32 numOfImgData;

        public OffsetValues[] offsetValue;
        public RequiredValues[] requiredValue;

        public string optionForTagInterpreter = ""; // if = "UseInterpreter", convert int to IFDEntry NameStr

        string strError;
        string strWarning;

        //ファイルチェック・フォルダチェック用変数
        public bool fileCheck = false;
        public bool folderCheck = false;

        //データの内容を表示 デバッグ用
        public bool showDataForDebug = false;

        /// <summary>
        /// ClaTiffコンストラクタ
        /// </summary>
        public ClaTif()
        {
            mFileNameFullPath   = "";
            mImageFileHeader    = new IFH();
            mImageFileDirectory = null;

            imgData             = null;
            numOfImgData        = 0;

            offsetValue         = null;
            requiredValue       = null;

            strError            = "";
            strWarning          = "";
        }

        /// <summary>
        /// Disposer
        /// </summary>
        public void Dispose()
        {
            #region mImageFileDirectory, imgData, offsetValue, requiredValue

            if (mImageFileDirectory != null)
            {
                for (int i = 0; i < mImageFileDirectory.Length; ++i)
                {
                    if (mImageFileDirectory[i] != null)
                    {
                        mImageFileDirectory[i] = null; // Fobid reusing
                    }
                }
            }

            if (imgData != null)
            {
                for (int i = 0; i < imgData.Length; ++i)
                {
                    if (imgData[i] != null)
                    {
                        imgData[i].Dispose();
                        imgData[i] = null;    // Fobid reusing
                    }
                }
            }

            if (offsetValue != null)
            {
                for (int i = 0; i < offsetValue.Length; ++i)
                {
                    if (offsetValue[i] != null)
                    {
                        offsetValue[i] = null; // Fobid reusing
                    }
                }
            }

            if (requiredValue != null)
            {
                for (int i = 0; i < requiredValue.Length; ++i)
                {
                    if (requiredValue[i] != null)
                    {
                        requiredValue[i] = null; // Fobid reusing
                    }
                }
            }

            #endregion mImageFileDirectory, imgData, offsetValue, requiredValue

            mFileNameFullPath = "";
            strError          = "";
            strWarning        = "";
        }

        /// <summary>
        /// 初期化
        /// </summary>
        public void Initialize()
        {
            mFileNameFullPath   = "";
            mImageFileHeader    = new IFH();
            mImageFileDirectory = null;

            imgData             = null;
            numOfImgData        = 0;

            offsetValue         = null;
            requiredValue       = null;

            strError            = "";
            strWarning          = "";
        }


        /// <summary>
        /// Tif画像の読み込み
        /// </summary>
        /// <param name="mFileNameTmp">読み込むファイル名</param>
        /// <param name="suspendFlag">途中で読み込み処理を停止するかのフラグ</param>
        /// <returns>エラーの有無</returns>
        public Int32 ReadTiff(string mFileNameTmp, Int32 suspendFlag)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            mFileNameFullPath = mFileNameTmp;

            string folderPath = "";
            string byteOrderStrTmp = "";

            Byte[] tmpBuf_size2_in = null;
            Byte[] tmpBuf_size2_out = null;
            Byte[] tmpBuf_size4_in = null;
            Byte[] tmpBuf_size4_out = null;


            //00. New and Check buffer
            #region New and Check Buffer

            try
            {
                tmpBuf_size2_in = new Byte[2];
                tmpBuf_size2_out = new Byte[2];
                tmpBuf_size4_in = new Byte[4];
                tmpBuf_size4_out = new Byte[4];
            }
            catch (Exception ex_Buf)
            {
                strError = ex_Buf.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_BufAllocException;
            }

            //Initilize buffer before use
            resVal = InitializeByteBuffer(ref tmpBuf_size2_in, 2, "ReadTif_BufAlloc_After New Byte[]");
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            resVal = InitializeByteBuffer(ref tmpBuf_size2_out, 2, "ReadTif_BufAlloc_After New Byte[]");
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            resVal = InitializeByteBuffer(ref tmpBuf_size4_in, 4, "ReadTif_BufAlloc_After New Byte[]");
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            resVal = InitializeByteBuffer(ref tmpBuf_size4_out, 4, "ReadTif_BufAlloc_After New Byte[]");
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            #endregion New and Check Buffer

            //01. Check directory and file existance
            #region Check directory and file existance

            if (true == folderCheck)
            {
                // Get Folder Path
                try
                {
                    folderPath = Path.GetDirectoryName(mFileNameTmp);
                }
                catch (ArgumentException argExcp)
                {
                    strError = argExcp.ToString();
                    return (Int32)ClaTiffErr.Err_ReadTif_ArgumentException;
                }
                catch (PathTooLongException ptlExcp)
                {
                    strError = ptlExcp.ToString();
                    return (Int32)ClaTiffErr.Err_ReadTif_PathTooLongException;
                }

                //  Folder Check
                if (Directory.Exists(folderPath) == false)
                {
                    strError = "No such directory" + folderPath;
                    return (Int32)ClaTiffErr.Err_ReadTif_NoDirectory;
                }
            }


            //  File Check
            if (true == fileCheck)
            {
                if (File.Exists(mFileNameFullPath) == false)
                {
                    strError = "No such a file" + mFileNameFullPath;
                    return (Int32)ClaTiffErr.Err_ReadTif_NoFile;
                }
            }

            #endregion

            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_CheckDirectoryAndFile_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After CheckDirectoryAndFile";
                return (Int32)ClaTiffErr.NoError_ReadTif_CheckDirectoryAndFile_End_Suspned;
            }

            //02. Read Image File Header
            //    Image File Headerの読込
            #region Read Image File Header

            try
            {
                using (FileStream fileSTRM = new FileStream(mFileNameTmp, FileMode.Open, FileAccess.Read))
                {
                    //Read ByteOrder
                    #region Read ByteOrder
                    try
                    {
                        fileSTRM.Read(mImageFileHeader.byteOrder, 0, 2);
                    }
                    catch (Exception exHeader1)
                    {
                        strError = exHeader1.ToString();
                        return (Int32)ClaTiffErr.Err_ReadTif_ReadImageFileHeader_ByteOrder;
                    }

                    //Check Byte Order 
                    if (mImageFileHeader.byteOrder[0] == 73 && mImageFileHeader.byteOrder[1] == 73)
                    {
                        //II (I=73 @ascii)
                        mImageFileHeader.byteOrderStr = "II";
                    }
                    else if (mImageFileHeader.byteOrder[0] == 77 && mImageFileHeader.byteOrder[1] == 77)
                    {
                        //MM (M=77 @ascii)
                        mImageFileHeader.byteOrderStr = "MM";
                    }
                    else
                    {
                        strError = "Abnormal Byte Order error at Image File Header";
                        return (Int32)ClaTiffErr.Err_ReadTif_ReadImageFileHeader_AbnormalByteOrder;
                    }
                    #endregion Read ByteOrder

                    byteOrderStrTmp = mImageFileHeader.byteOrderStr;


                    //Read TiffID
                    resVal = ReadAndGet2ByteValue(fileSTRM, 2, byteOrderStrTmp, "ReadImageHeader_Reading_TiffID", ref mImageFileHeader.tiffID);

                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error

                    //Check Tiff ID
                    if (mImageFileHeader.tiffID != 42)
                    {
                        strError = "Seq02_Read Tiff ID in not equal 42";
                        return (Int32)ClaTiffErr.Err_ReadTif_CheckTiffID_UndifinedID;
                    }


                    //Read first IFD
                    resVal = ReadAndGet4ByteValue(fileSTRM, 4, byteOrderStrTmp, "ReadImageHeader_Reading_firstIFD", ref mImageFileHeader.firstIFDOffset);

                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error
                }
            }
            catch (Exception ex)
            {
                strError = "Error occupied in Read Image File Haeeaders\r\n" + ex.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_ReadImageFileHeader;
            }

            #endregion Read Image File Header


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_ReadImageHeader_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After ReadImageHeader";
                return (Int32)ClaTiffErr.NoError_ReadTif_ReadImageHeader_End_Suspned;
            }


            //03. Read Image File Directories And Count up the number of images for Multi-image
            //    IFDのnextIFDのみ順次読込み，画像枚数を数える。
            #region Read Image File Directories

            try
            {
                using (FileStream fileSTRM = new FileStream(mFileNameTmp, FileMode.Open, FileAccess.Read))
                {
                    //Rewind file pos <念のため読み取り開始位置を先頭にまき戻す。>
                    fileSTRM.Seek(0, SeekOrigin.Begin);

                    //Initialize valiable <変数初期化>
                    bool bEndOfIFD = false;
                    int numOfImages = 0;
                    UInt32 curPosOfFile = mImageFileHeader.firstIFDOffset;
                    UInt16 numOfDirectoryEntriesTmp = 0;
                    UInt32 nextPos = 0;

                    while (bEndOfIFD == false)
                    {

                        //Read Num Of Directory Entries <IFD中のタグ数を読み取る>
                        resVal = ReadAndGet2ByteValue(fileSTRM, curPosOfFile, byteOrderStrTmp, "ReadIFD_Reading_NumOfDirectoryEntries", ref numOfDirectoryEntriesTmp);

                        if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                        //Read Num Of Directory Entries <>
                        resVal = ReadAndGet4ByteValue(fileSTRM,
                                                        curPosOfFile + 2 + (UInt32)numOfDirectoryEntriesTmp * 12,
                                                        byteOrderStrTmp,
                                                        "ReadIFD_Reading_NextIFDoffset",
                                                        ref nextPos);

                        if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                        //Count up the number of image in a tif file. <画像枚数を数える>
                        ++numOfImages;


                        //Determine whether move to next IFD or End here.
                        if (nextPos != 0)
                        {
                            curPosOfFile = nextPos;
                        }
                        else if (nextPos == 0)
                        {
                            //End of search for IFD.
                            bEndOfIFD = true;

                            //Set num of image
                            numOfImgData = numOfImages;
                        }


                    }


                }

            }
            catch (Exception ex)
            {
                strError = "Error occupied in Read Image File Directories\r\n" + ex.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_ReadIFD_ReadImageFileDirectories;
            }

            #endregion Read Image File Directories


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_ReadIFD_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After ReadIFD(Count images)";
                return (Int32)ClaTiffErr.NoError_ReadTif_ReadIFD_End_Suspned;
            }

            //04. Allocate IFD , ImgSet , offsetValue and required Value
            //    IFDとImgSetを画像分割り当てる。

            #region Allocate IFD , ImgSet
            try
            {
                mImageFileDirectory = new IFD[numOfImgData];
                imgData = new ImgSet[numOfImgData];

                offsetValue = new OffsetValues[numOfImgData];
                requiredValue = new RequiredValues[numOfImgData];

                for (Int32 i = 0; i < numOfImgData; ++i)
                {
                    mImageFileDirectory[i] = new IFD();
                    imgData[i] = new ImgSet();

                    offsetValue[i] = new OffsetValues();
                    requiredValue[i] = new RequiredValues();
                }
            }
            catch (Exception exAlloc)
            {
                strError = "Error occupied in Alloc IFD and ImgSet\r\n" + exAlloc.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_Alloc_IFD_and_ImgSet;
            }
            #endregion Allocate IFD and ImgSet

            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_AllocIFDAndImgSet_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After Alloc IFD and Imgset";
                return (Int32)ClaTiffErr.NoError_ReadTif_AllocIFDAndImgSet_End_Suspned;
            }


            //05. Read Image Directory Entries (Read tags)

            #region Read Image Directory Entries (Read tags)

            try
            {
                using (FileStream fileSTRM = new FileStream(mFileNameTmp, FileMode.Open, FileAccess.Read))
                {
                    //Rewind file pos, 念のため読み取り開始位置を先頭にまき戻す。
                    fileSTRM.Seek(0, SeekOrigin.Begin);

                    UInt32 curPosOfFile = mImageFileHeader.firstIFDOffset;
                    UInt16 numOfDirectoryEntriesTmp = 0;
                    UInt32 nextPos = 0;

                    UInt32 posOfTagNumber = 0;
                    UInt32 posOfTagType = 0;
                    UInt32 posOfTagCount = 0;
                    UInt32 posOfTagOffset = 0;

                    IFDEntry tmpIFDEntry = new IFDEntry();

                    for (Int32 i = 0; i < numOfImgData; ++i)
                    {
                        //Read Num Of Directory Entries <i番目のIFD中のタグ数を読み取る>
                        resVal = ReadAndGet2ByteValue(fileSTRM, curPosOfFile, byteOrderStrTmp,
                                                        "ReadImaegDirectoryEntry_Reading_NumOfDirectoryEntries at" + i.ToString() + "th image",
                                                        ref numOfDirectoryEntriesTmp);

                        if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                        //Alloc field of Directory entries
                        #region Alloc field of Directory entries
                        try
                        {
                            mImageFileDirectory[i].numOfDirectoryEntries = numOfDirectoryEntriesTmp;

                            mImageFileDirectory[i].directoryEntry = new IFDEntry[numOfDirectoryEntriesTmp];

                            for (Int32 j = 0; j < numOfDirectoryEntriesTmp; ++j)
                            {
                                mImageFileDirectory[i].directoryEntry[j] = new IFDEntry();
                            }

                        }
                        catch (Exception exAllocDirecEntries)
                        {
                            strError = "ReadImaegDirectoryEntry_AllocDirectoryEntries_AfterReadingNum at" + i.ToString() + "th image"
                                        + exAllocDirecEntries.ToString();
                            return (Int32)ClaTiffErr.Err_ReadTif_ReadImaegDirectoryEntry_AllocDirectoryEntries;
                        }
                        #endregion Alloc field of Directory entries


                        //Read All Tag data
                        #region Read All Tag data

                        for (Int32 j = 0; j < mImageFileDirectory[i].numOfDirectoryEntries; ++j)
                        {



                            posOfTagNumber = (UInt32)(curPosOfFile + 2 + j * 12); //  2 byte for Num of directory entry,  12 byte is the size of one directory entry 
                            posOfTagType = (UInt32)(curPosOfFile + 2 + 2 + j * 12); //+ 2 byte for tag number
                            posOfTagCount = (UInt32)(curPosOfFile + 2 + 2 + 2 + j * 12); //+ 2 byte for tag type
                            posOfTagOffset = (UInt32)(curPosOfFile + 2 + 2 + 2 + 4 + j * 12); //+ 4 byte for tag count


                            //Read Tag Type <タグの番号を読み取る>
                            resVal = ReadAndGet2ByteValue(fileSTRM, posOfTagNumber, byteOrderStrTmp,
                                                            "ReadImaegDirectoryEntry_Reading_Identify Tag Number at" + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                            ref tmpIFDEntry.tagNumber);

                            if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error



                            //Read Tag Type <タグのデータ型を読み取る>
                            resVal = ReadAndGet2ByteValue(fileSTRM, posOfTagType, byteOrderStrTmp,
                                                            "ReadImaegDirectoryEntry_Reading_Identify Tag Type at" + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                            ref tmpIFDEntry.tagType);

                            if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                            //Read Tag Count <タグの中のデータ個数を読み取る>
                            resVal = ReadAndGet4ByteValue(fileSTRM, posOfTagCount, byteOrderStrTmp,
                                                            "ReadImaegDirectoryEntry_Reading_Data count at" + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                            ref tmpIFDEntry.count);

                            if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                            //Read Tag Count <タグの中のデータ個数を読み取る>
                            resVal = ReadAndGet4ByteValue(fileSTRM, posOfTagOffset, byteOrderStrTmp,
                                                            "ReadImaegDirectoryEntry_Reading_Data offset at" + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                            ref tmpIFDEntry.value_offset);

                            if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                            //読み取ったデータを代入
                            mImageFileDirectory[i].directoryEntry[j].tagNumber = tmpIFDEntry.tagNumber;
                            mImageFileDirectory[i].directoryEntry[j].tagType = tmpIFDEntry.tagType;
                            mImageFileDirectory[i].directoryEntry[j].count = tmpIFDEntry.count;
                            mImageFileDirectory[i].directoryEntry[j].value_offset = tmpIFDEntry.value_offset;


                            //Reset temporary valuable
                            tmpIFDEntry.Reset();

                        }

                        #endregion Read All Tag data


                        //Read Next IFD and Set Next IFD
                        //Read Num Of Directory Entries <>
                        resVal = ReadAndGet4ByteValue(fileSTRM,
                                                        curPosOfFile + 2 + (UInt32)numOfDirectoryEntriesTmp * 12,
                                                        byteOrderStrTmp,
                                                        "ReadImaegDirectoryEntry_Reading_NextIFDoffset",
                                                        ref nextPos);

                        //UpDate Next IDF position
                        curPosOfFile = nextPos;

                        if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error

                    }
                }
            }
            catch (Exception exReadImgEntry)
            {
                strError = "Error occupied in  Read Image Directory Entries (Read tags)\r\n" + exReadImgEntry.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_ReadImaegDirectoryEntry_ReadImageDirectoryEntries;
            }

            #endregion Read Image Directory Entries (Read tags)

            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_ReadImaegDirectoryEntry_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend Aftre ReadImaegDirectoryEntry";
                return (Int32)ClaTiffErr.NoError_ReadTif_ReadImaegDirectoryEntry_End_Suspned;
            }

            #region tag interpreter option <タグ解析オプション>

            //05.01 tag Name and Type interpreter option  enum to string
            //      <Tifタグ番号と型式を文字列に変換するオプション>

            if (optionForTagInterpreter == "UseTagInterpreter")
            {
                try
                {

                    UInt16 tagNameIntTmp = 0;
                    UInt16 tagTypeIntTmp = 0;


                    for (Int32 i = 0; i < numOfImgData; ++i)
                    {
                        for (Int32 j = 0; j < mImageFileDirectory[i].numOfDirectoryEntries; ++j)
                        {
                            tagNameIntTmp = (UInt16)mImageFileDirectory[i].directoryEntry[j].tagNumber;

                            mImageFileDirectory[i].directoryEntry[j].tagNameStr = Enum.GetName(typeof(TagName), tagNameIntTmp);

                            tagTypeIntTmp = (UInt16)mImageFileDirectory[i].directoryEntry[j].tagType;

                            mImageFileDirectory[i].directoryEntry[j].tagTypeStr = Enum.GetName(typeof(TifFieldType), tagTypeIntTmp);
                        }
                    }
                }
                catch (Exception exOptionError)
                {
                    strError = "Error occupied in NameTypeInterpreter option\r\n" + exOptionError.ToString();
                    return (Int32)ClaTiffErr.Err_ReadTif_NameTypeInterpreterError;
                }
            }
            else if (optionForTagInterpreter == "")
            {
                //do nothing <何もしない>
            }
            else
            {
                strError = "NameTypeInterpreter option select Error\r\n";
                return (Int32)ClaTiffErr.Err_ReadTif_NameTypeSelectionError;
            }

            #endregion tag interpreter option <タグ解析オプション>


            //06. Read Value set (longer than 4 byte) - Required Field

            #region Read Value set (longer than 4 byte) - Required Field

            try
            {
                using (FileStream fileSTRM = new FileStream(mFileNameTmp, FileMode.Open, FileAccess.Read))
                {
                    //Rewind file pos, 念のため読み取り開始位置を先頭にまき戻す。
                    fileSTRM.Seek(0, SeekOrigin.Begin);


                    for (Int32 i = 0; i < numOfImgData; ++i)
                    {
                        for (Int32 j = 0; j < mImageFileDirectory[i].numOfDirectoryEntries; ++j)
                        {
                            UInt16 numberTmp = mImageFileDirectory[i].directoryEntry[j].tagNumber;
                            UInt32 fileOffset = mImageFileDirectory[i].directoryEntry[j].value_offset;
                            UInt32 countTmp = mImageFileDirectory[i].directoryEntry[j].count;

                            switch (numberTmp)
                            {
                                case (UInt16)TagName.XResolution:
                                    #region XResolution

                                    UInt32 xResolution1 = 0;
                                    UInt32 xResolution2 = 0;

                                    //Get xResolution from file offset  < 指定されたオフセットにあるXResolutionデータを読み取る>
                                    resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset, byteOrderStrTmp,
                                                                    "ReadValueSetLongerThan4Bytes_Reading_xResolution1 data  " + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                                    ref xResolution1);

                                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                                    //Get xResolution from file offset  < 指定されたオフセットにあるXResolutionデータを読み取る>
                                    resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset + 4, byteOrderStrTmp,
                                                                    "ReadValueSetLongerThan4Bytes_Reading_xResolution2 data  " + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                                    ref xResolution2);

                                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error

                                    //Set
                                    offsetValue[i].xResolution1 = xResolution1;
                                    offsetValue[i].xResolution2 = xResolution2;

                                    #endregion XResolution
                                    break;

                                case (UInt16)TagName.YResolution:
                                    #region YResolution

                                    UInt32 yResolution1 = 0;
                                    UInt32 yResolution2 = 0;

                                    //Get xResolution from file offset  < 指定されたオフセットにあるXResolutionデータを読み取る>
                                    resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset, byteOrderStrTmp,
                                                                    "ReadValueSetLongerThan4Bytes_Reading_yResolution1 data  " + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                                    ref yResolution1);

                                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error


                                    //Get xResolution from file offset  < 指定されたオフセットにあるXResolutionデータを読み取る>
                                    resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset + 4, byteOrderStrTmp,
                                                                    "ReadValueSetLongerThan4Bytes_Reading_yResolution2 data  " + i.ToString() + "th image -" + j.ToString() + "th tag",
                                                                    ref yResolution2);

                                    if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error

                                    //Set
                                    offsetValue[i].yResolution1 = yResolution1;
                                    offsetValue[i].yResolution2 = yResolution2;

                                    #endregion YResolution
                                    break;

                                case (UInt16)TagName.StripOffsets:
                                    #region  StripOffsets

                                    try
                                    {
                                        offsetValue[i].stripOffsets = new UInt32[countTmp];

                                        for (int k = 0; k < countTmp; ++k) offsetValue[i].stripOffsets[k] = 0;


                                        if (countTmp == 1)
                                        {
                                            offsetValue[i].stripOffsets[0] = fileOffset; // this type isnot file offset, actually value
                                        }
                                        else
                                        {
                                            for (int k = 0; k < countTmp; ++k)
                                            {
                                                //Get StripOffsets from file offset  < 指定されたオフセットにあるStripOffsetsデータを読み取る>
                                                resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset + (UInt32)(4 * k), byteOrderStrTmp,
                                                                                "ReadValueSetLongerThan4Bytes_Reading_StripOffsets data  " + i.ToString() + "th image -" + j.ToString() + "th tag -" + k.ToString() + "th strip offsets",
                                                                                ref offsetValue[i].stripOffsets[k]);

                                                if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error
                                            }
                                        }
                                    }
                                    catch (Exception exStripOffsets)
                                    {
                                        strError = "ReadValueSetLongerThan4Bytes_Reading_StripOffsetsData" + exStripOffsets.ToString();
                                        return (Int32)ClaTiffErr.Err_ReadTif_ReadStripOffsets;
                                    }

                                    #endregion  StripOffsets
                                    break;

                                case (UInt16)TagName.StripByteCounts:
                                    #region  StripByteCounts

                                    try
                                    {
                                        offsetValue[i].stripByteCounts = new UInt32[countTmp];

                                        for (int k = 0; k < countTmp; ++k) offsetValue[i].stripByteCounts[k] = 0;

                                        if (countTmp == 1)
                                        {
                                            offsetValue[i].stripByteCounts[0] = fileOffset; // this type is not fileoffset, actually value
                                        }
                                        else
                                        {
                                            for (int k = 0; k < countTmp; ++k)
                                            {
                                                //Get StripByteCounts from file offset  < 指定されたオフセットにあるStripByteCountsデータを読み取る>
                                                resVal = ReadAndGet4ByteValue(fileSTRM, fileOffset + (UInt32)(4 * k), byteOrderStrTmp,
                                                                                "ReadValueSetLongerThan4Bytes_Reading_StripByteCounts data  " + i.ToString() + "th image -" + j.ToString() + "th tag -" + k.ToString() + "th strip offsets",
                                                                                ref offsetValue[i].stripByteCounts[k]);

                                                if (resVal != (Int32)ClaTiffErr.NoError) return resVal; // Check error
                                            }
                                        }
                                    }
                                    catch (Exception exStripByteCounts)
                                    {
                                        strError = "ReadValueSetLongerThan4Bytes_Reading_StripByteCountsData" + exStripByteCounts.ToString();
                                        return (Int32)ClaTiffErr.Err_ReadTif_ReadStripByteCounts;
                                    }

                                    #endregion  StripByteCounts
                                    break;

                                case (UInt16)TagName.BitsPerSample:

                                    if (countTmp > 1)
                                    {
                                        //読み込み
                                        //requiredValue[i].bitsPerSample[0] = fileOffset;

                                    }
                                    break;

                                case (UInt16)TagName.ColorMap:
                                    break;

                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                strError = "Read Value set" + ex.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_ReadValueSetLongerThan4Bytes;
            }

            #endregion Read Value set (longer than 4 byte) - Required Field


            //07. Get Image Data from Required Field ( imageLength/imageWidth/imageSize/ imgType ) 
            //     ref TIFF varsion6 p17-25

            #region Get Image Data from Required Field ( imageLength/imageWidth/imageSize/ imgType )

            try
            {

                for (Int32 i = 0; i < numOfImgData; ++i)
                {
                    for (Int32 j = 0; j < mImageFileDirectory[i].numOfDirectoryEntries; ++j)
                    {
                        UInt16 numberTmp = mImageFileDirectory[i].directoryEntry[j].tagNumber;
                        UInt32 valueTmp = mImageFileDirectory[i].directoryEntry[j].value_offset;
                        UInt32 countTmp = mImageFileDirectory[i].directoryEntry[j].count;

                        switch (numberTmp)
                        {
                            case (UInt16)TagName.ImageLength:
                                imgData[i].imageLength = valueTmp;
                                break;

                            case (UInt16)TagName.ImageWidth:
                                imgData[i].imageWidth = valueTmp;
                                break;

                            case (UInt16)TagName.PhotometricInterpretation:
                                requiredValue[i].photometricInterpretation = valueTmp;
                                imgData[i].photometric = (int)valueTmp;
                                break;

                            case (UInt16)TagName.BitsPerSample:
                                if (countTmp == 1)
                                    requiredValue[i].bitsPerSample[0] = valueTmp;
                                break;

                            case (UInt16)TagName.SamplesPerPixel:
                                requiredValue[i].samplesPerPixel = valueTmp;
                                break;

                            case (UInt16)TagName.Compression:
                                imgData[i].compression = (int)valueTmp;
                                break;

                            case (UInt16)TagName.SampleFormat:
                                imgData[i].sampleFormat = (int)valueTmp;
                                break;
                        }

                    }

                    imgData[i].buffSize = imgData[i].imageLength * imgData[i].imageWidth;

                    if (imgData[i].buffSize == 0)
                    {
                        //Eror is occupied, エラー
                        strError = "Buffer is zero at Getting Image Data from Required Field";
                        return (Int32)ClaTiffErr.Err_ReadTif_ZeroSizeImageBuffer;
                    }

                    //16bitグレー 対応 (for Image-J)
                    if (requiredValue[i].samplesPerPixel > 0)
                    {
                        imgData[i].photometric = imgData[i].photometric / (int)requiredValue[i].samplesPerPixel;
                    }
                }


            }
            catch (Exception ex)
            {
                strError = "Error occupied in Getting Image Data from Required Field:" + ex.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_GetImageDataFromRequiredFiled;
            }

            #endregion Get Image Data from Required Field ( imageLength/imageWidth/imageSize/ imgType )


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_ReadImaegDirectoryEntry_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend Aftre ReadImaegDirectoryEntry";
                return (Int32)ClaTiffErr.NoError_ReadTif_ReadImaegDirectoryEntry_End_Suspned;
            }



            //08  Check and Set ImageType for each image

            #region Check and Set ImageType for each image

            for (UInt32 i = 0; i < numOfImgData; ++i)
            {
                if (requiredValue[i].samplesPerPixel == 1)
                {
                    //bilevel, Gray

                    if (requiredValue[i].bitsPerSample[0] == 1)
                    {
                        imgData[i].iImgType = ImgType.bilevelImg;
                    }
                    else if (requiredValue[i].bitsPerSample[0] == 8)
                    {
                        imgData[i].iImgType = ImgType.gryImg_8;
                    }
                    else if (requiredValue[i].bitsPerSample[0] == 16)
                    {
                        imgData[i].iImgType = ImgType.gryImg_16;
                    }
                    else if (requiredValue[i].bitsPerSample[0] == 32)
                    {
                        if (imgData[i].sampleFormat == (int)SampleFormat.UnsignedInteger)
                        {
                            imgData[i].iImgType = ImgType.gryImg_32;

                            //Not Implemented Error, 未実装のためエラーとする
                            strError = "Error occupiked in Checking and Setting ImageType for Gray";
                            return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForGray;
                        }
                        else if (imgData[i].sampleFormat == (int)SampleFormat.IEEEfloatingPoint)
                        {
                            imgData[i].iImgType = ImgType.gryImg_Sgl;
                        }
                        else
                        {
                            imgData[i].iImgType = ImgType.notImplement;


                            //Not Implemented Error, 未実装のためエラーとする
                            strError = "Error occupiked in Checking and Setting ImageType for Gray";
                            return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForGray;
                        }
                    }
                    else
                    {
                        //未実装
                        imgData[i].iImgType = ImgType.notImplement;

                        //Not Implemented Error, 未実装のためエラーとする
                        strError = "Error occupiked in Checking and Setting ImageType for Gray";
                        return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForGray;
                    }

                }
                else if (requiredValue[i].samplesPerPixel == 3)
                {
                    //color 24bit int
                    if (requiredValue[i].bitsPerSample[0] == 8
                            && requiredValue[i].bitsPerSample[1] == 8
                            && requiredValue[i].bitsPerSample[2] == 8)
                    {
                        imgData[i].iImgType = ImgType.notImplement;
                        //imgData[i].iImgType = ImgType.rgbImg_24;

                        //Not Implemented Error, 未実装のためエラーとする
                        strError = "Error occupiked in Checking and Setting ImageType for Color";
                        return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForColor;
                    }
                    else
                    {
                        //拡張機能,ないし,未実装
                        imgData[i].iImgType = ImgType.notImplement;

                        //Not Implemented Error, 未実装のためエラーとする
                        strError = "Error occupiked in Checking and Setting ImageType for Color";
                        return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForColor;
                    }

                }
                // Image-J対応 
                else if (requiredValue[i].samplesPerPixel == 65536)
                {
                    if (requiredValue[i].bitsPerSample[0] == 16 * 65536)
                    {
                        imgData[i].iImgType = ImgType.gryImg_16;
                    }
                    else if (requiredValue[i].bitsPerSample[0] == 32 * 65536)
                    {
                        imgData[i].iImgType = ImgType.gryImg_Sgl;
                    }
                    else
                    {
                        //拡張機能,ないし,未実装
                        imgData[i].iImgType = ImgType.notImplement;

                        //Not Implemented Error, 未実装のためエラーとする
                        strError = "Error occupiked in Checking and Setting ImageType for Color";
                        return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForColor;
                    }

                }
                else
                {
                    //拡張機能,ないし,未実装
                    imgData[i].iImgType = ImgType.notImplement;

                    //Not Implemented Error, 未実装のためエラーとする
                    strError = "Error occupiked in Checking and Setting ImageType for Color";
                    return (Int32)ClaTiffErr.Err_ReadTif_CheckAndSetImageTypeForColor;
                }

            }

            #endregion Check and Set ImageType for each image


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_CheckAndSetImageType_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend Aftre CheckAndSetImageType";
                return (Int32)ClaTiffErr.NoError_ReadTif_CheckAndSetImageType_End_Suspned;
            }


            //09 Alloc Image

            #region Alloc Image
            try
            {

                for (UInt32 i = 0; i < numOfImgData; ++i)
                {
                    bool resNotImplement = false;

                    switch (imgData[i].iImgType)
                    {
                        case ImgType.bilevelImg:
                            imgData[i].blvImg = new Byte[imgData[i].buffSize];
                            break;

                        case ImgType.gryImg_8:
                            imgData[i].gryImg_8 = new Byte[imgData[i].buffSize];
                            break;

                        case ImgType.gryImg_16:
                            imgData[i].gryImg_16 = new UInt16[imgData[i].buffSize];
                            break;

                        case ImgType.gryImg_32:
                            imgData[i].gryImg_32 = new UInt32[imgData[i].buffSize];
                            break;

                        case ImgType.gryImg_Sgl:
                            imgData[i].gryImg_Sgl = new Single[imgData[i].buffSize];
                            break;

                        default:
                            resNotImplement = true;
                            break;
                    }

                    if (resNotImplement == true)
                    {
                        //Not Implemented Error, 未実装のためエラーとする
                        strError = "Error occupied in Allocating Imgset";
                        return (Int32)ClaTiffErr.Err_ReadTif_AllocImageSet_NotImplement;
                    }

                }
            }
            catch (Exception exAllocImage)
            {
                strError = "Error occupied in Allocating Imgset:" + exAllocImage.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_AllocImageSet;
            }
            #endregion Alloc Image


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_AllocImgset_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After AllocImgset";
                return (Int32)ClaTiffErr.NoError_ReadTif_AllocImageSet_End_Suspned;
            }



            //10 Check StripByteCounts (matching between Byte Counts and total size of image)

            #region Check StripByteCounts (matching between Byte Counts and total size of image)

            try
            {
                UInt32 totalCountFromFile = 0;
                Int32 cntLength = 0;

                for (UInt32 i = 0; i < numOfImgData; ++i)
                {
                    cntLength = offsetValue[i].stripByteCounts.Length;
                    totalCountFromFile = 0;

                    for (UInt32 j = 0; j < cntLength; ++j)
                    {
                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                    }


                    ImgType curImgType = imgData[i].iImgType;
                    TifCompression imgComp = (TifCompression)imgData[i].compression;

                    ClaTiffErr result = ClaTiffErr.NoError;

                    if (imgComp == TifCompression.Uncompressed)
                    {

                        switch (curImgType)
                        {
                            case ImgType.bilevelImg:
                                if (totalCountFromFile * 8 < imgData[i].buffSize)
                                {
                                    //エラー
                                    strError = "Image size in file is smaller than calculate image at " + i.ToString() + "th image.";
                                    return (Int32)ClaTiffErr.Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_bilevel_gray;
                                }
                                else if (totalCountFromFile * 8 > imgData[i].buffSize)
                                {
                                    //ワーニング
                                    //問題はないが，注意が必要。過剰な部分は読み込まないこと。
                                    strWarning = "Image size in file is larger than calculate image at " + i.ToString() + "th image.";
                                }
                                //else if (totalCountFromFile * 8 == imgData[i].buffSize)
                                //{
                                //    //問題なし
                                //}

                                break;

                            case ImgType.gryImg_8:
                                if (totalCountFromFile < imgData[i].buffSize)
                                {
                                    //Error occupied, エラー発生 
                                    strError = "Image size in file is smaller than calculate image at " + i.ToString() + "th image.";
                                    return (Int32)ClaTiffErr.Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_8bit_gray;
                                }
                                else if (totalCountFromFile > imgData[i].buffSize)
                                {
                                    //Warning , ワーニング
                                    //問題はないが，注意が必要。過剰な部分は読み込まないこと。
                                    strWarning = "Image size in file is larger than calculate image at " + i.ToString() + "th image.";
                                }

                                break;

                            case ImgType.gryImg_16:
                                if (totalCountFromFile < imgData[i].buffSize * 2)
                                {
                                    //Error occupied, エラー発生 
                                    strError = "Image size in file is smaller than calculate image at " + i.ToString() + "th image.";
                                    return (Int32)ClaTiffErr.Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_16bit_gray;
                                }
                                else if (totalCountFromFile > imgData[i].buffSize * 2)
                                {
                                    //Warning , ワーニング
                                    //問題はないが，注意が必要。過剰な部分は読み込まないこと。
                                    strWarning = "Image size in file is larger than calculate image at " + i.ToString() + "th image.";
                                }

                                break;

                            case ImgType.gryImg_32:
                                if (totalCountFromFile < imgData[i].buffSize * 4)
                                {
                                    //Error occupied, エラー発生 
                                    strError = "Image size in file is smaller than calculate image at " + i.ToString() + "th image.";
                                    return (Int32)ClaTiffErr.Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_32bit_gray_float;
                                }
                                else if (totalCountFromFile > imgData[i].buffSize * 4)
                                {
                                    //Warning , ワーニング
                                    //問題はないが，注意が必要。過剰な部分は読み込まないこと。
                                    strWarning = "Image size in file is larger than calculate image at " + i.ToString() + "th image.";
                                }

                                break;


                            case ImgType.gryImg_Sgl:
                                if (totalCountFromFile < imgData[i].buffSize * 4)
                                {
                                    //Error occupied, エラー発生 
                                    strError = "Image size in file is smaller than calculate image at " + i.ToString() + "th image.";
                                    return (Int32)ClaTiffErr.Err_ReadTif_FileImageSizeIsLagerThanCalclulateImageSize_32bit_gray_float;
                                }
                                else if (totalCountFromFile > imgData[i].buffSize * 4)
                                {
                                    //Warning , ワーニング
                                    //問題はないが，注意が必要。過剰な部分は読み込まないこと。
                                    strWarning = "Image size in file is larger than calculate image at " + i.ToString() + "th image.";
                                }

                                break;

                            case ImgType.notInitialized:
                                //Error occupied, エラー発生 
                                strError = "Err Check Strip Byte Counts because image type is not Initialized  at " + i.ToString() + "th image.";
                                result = ClaTiffErr.Err_ReadTif_CheckStripByteCounts_NotInitialized;
                                break;

                            case ImgType.notImplement:
                                //Error occupied, エラー発生 
                                strError = "Err Check Strip Byte Counts because this image type is not Implemented at " + i.ToString() + "th image.";
                                result = ClaTiffErr.Err_ReadTif_CheckStripByteCounts_NotImplement;
                                break;

                            default:
                                //Error occupied, エラー発生 
                                strError = "Err Check Strip Byte Counts because image type is not Initialized  at " + i.ToString() + "th image.";
                                result = ClaTiffErr.Err_ReadTif_CheckStripByteCounts_NotInitialized;
                                break;

                        }

                        //エラー
                        if (result != ClaTiffErr.NoError)
                        {
                            return (Int32)result;
                        }
                    }


                }
            }
            catch (Exception exCheckStripByteCountSize)
            {
                strError = "Error occupied in CheckingByteCounts:" + exCheckStripByteCountSize.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_CheckStripByteCounts;
            }

            #endregion Check StripByteCounts (matching between Byte Counts and total size of image)


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_CheckStripByteCounts_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After AllocImgset";
                return (Int32)ClaTiffErr.NoError_ReadTif_CheckStripByteCounts_End_Suspned;
            }




            //11. Read Image Data

            #region Read Image Data

            try
            {

                using (FileStream fileSTRM = new FileStream(mFileNameTmp, FileMode.Open, FileAccess.Read))
                {
                    //Rewind file pos, 念のため読み取り開始位置を先頭にまき戻す。
                    fileSTRM.Seek(0, SeekOrigin.Begin);


                    for (UInt32 i = 0; i < numOfImgData; ++i)
                    {
                        ImgType curImgType = imgData[i].iImgType;

                        TifCompression imgComp = (TifCompression)imgData[i].compression;


                        //By Compress,Uncompress, control case.
                        if (imgComp == TifCompression.Uncompressed)
                        {
                            //Uncompressed-圧縮なし

                            Int32 result = (Int32)ClaTiffErr.NoError;

                            Byte[] imgLoadFromFile = null;

                            switch (curImgType)
                            {
                                case ImgType.bilevelImg:

                                    #region 非圧縮・1bitモノクロ読み取り / Uncompressed 1bit mono

                                    //Photometricに従って展開
                                    // ImgSet monoImgにByteとして展開

                                    Int32 cntLength = offsetValue[i].stripByteCounts.Length;
                                    Int64 setFilePos = 0;
                                    UInt32 curPosOfFile = 0;
                                    UInt32 curPosOfCount = 0;
                                    UInt32 totalCount = 0;
                                    UInt32 totalCountFromFile = 0;


                                    for (UInt32 j = 0; j < cntLength; ++j)
                                    {
                                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                                    }

                                    imgLoadFromFile = new Byte[totalCountFromFile];
                                    
                                    //関数で読み取り
                                    for (Int32 j = 0; j < cntLength; ++j)
                                    {

                                        curPosOfFile = offsetValue[i].stripOffsets[j];
                                        curPosOfCount = offsetValue[i].stripByteCounts[j];

                                        //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                                        setFilePos = fileSTRM.Seek(curPosOfFile, SeekOrigin.Begin);

                                        fileSTRM.Read(imgLoadFromFile, (int)totalCount, (int)curPosOfCount);

                                        totalCount += curPosOfCount;
                                    }

                                    #region Convert 1bit->1byte / 1bit->1byte変換
                                    //Phtometricにあわせて，1byteの中身を1bitずつ取り出し，その1bitを1byteへ変換
                                    // パレットの中身は無視して，とりあえず展開(後ほど)
                                    if (imgData[i].photometric == (int)PhotometricInterpretation.BlackIsZero | imgData[i].photometric == (int)PhotometricInterpretation.RGBPalette)
                                    {
                                        //1byteの中に入っている8つの1bit変数を取り出し。
                                        Byte[][] convertTableBIZ = new Byte[256][];

                                        //Byte[] bit = new Byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };
                                        Byte[] bit = new Byte[8] { 128, 64, 32, 16, 8, 4, 2, 1 };
                                        Byte bitTmp = 0;

                                        //convertTableBIZの作成
                                        for (Int32 j = 0; j < 256; ++j)
                                        {
                                            convertTableBIZ[j] = new Byte[8];

                                            for (int k = 0; k < 8; ++k)
                                            {
                                                bitTmp = (Byte)(bit[k] & j);

                                                if (bitTmp == bit[k])
                                                {
                                                    convertTableBIZ[j][k] = 255; // white is 255
                                                }
                                                else
                                                {
                                                    convertTableBIZ[j][k] = 0; // black is zero
                                                }
                                            }
                                        }

                                        //8の倍数の場合・ imageWidth is in multiples of 8
                                        if (imgData[i].imageWidth % 8 == 0)
                                        {
                                            Byte tmpCompBright = 0;

                                            for (Int32 k = 0; k < totalCount; ++k)
                                            {
                                                tmpCompBright = imgLoadFromFile[k];
                                                Buffer.BlockCopy(convertTableBIZ[tmpCompBright], 0, imgData[i].blvImg, k * 8, 8);
                                            }
                                        }
                                        //8の倍数で無い場合・imageWidth isn't in multiples of 8
                                        else
                                        {
                                            Byte tmpCompBright = 0;

                                            int modNumber = (int)(imgData[i].imageWidth - (imgData[i].imageWidth / 8) * 8);

                                            int curPosCopy = 0;

                                            for (int n = 0; n < imgData[i].imageLength; ++n)
                                            {
                                                // from 0 to width -2 / 0 ～ width-2
                                                for (int m = 0; m < imgData[i].imageWidth / 8; ++m)
                                                {
                                                    tmpCompBright = imgLoadFromFile[m + n * (imgData[i].imageWidth / 8 + 1)];

                                                    Buffer.BlockCopy(convertTableBIZ[tmpCompBright], 0, imgData[i].blvImg, curPosCopy, 8);

                                                    curPosCopy += 8;
                                                }

                                                // width -1
                                                int mPlus = (int)(imgData[i].imageWidth / 8);

                                                tmpCompBright = imgLoadFromFile[mPlus + n * (imgData[i].imageWidth / 8 + 1)];

                                                Buffer.BlockCopy(convertTableBIZ[tmpCompBright], 0, imgData[i].blvImg, curPosCopy, modNumber);

                                                curPosCopy += modNumber;

                                            }
                                        }
                                    }
                                    else if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                    {
                                        //1byteの中に入っている8つの1bit変数を取り出し。
                                        Byte[][] convertTableWIZ = new Byte[256][];

                                        //Byte[] bit = new Byte[8] { 1, 2, 4, 8, 16, 32, 64, 128 };
                                        Byte[] bit = new Byte[8] { 128, 64, 32, 16, 8, 4, 2, 1 };

                                        Byte bitTmp = 0;

                                        for (Int32 j = 0; j < 256; ++j)
                                        {
                                            convertTableWIZ[j] = new Byte[8];

                                            for (int k = 0; k < 8; ++k)
                                            {
                                                bitTmp = (Byte)(bit[k] & j);

                                                if (bitTmp == bit[k])
                                                {
                                                    convertTableWIZ[j][k] = 0; //white is zero
                                                }
                                                else
                                                {
                                                    convertTableWIZ[j][k] = 255; //Black is 255
                                                }
                                            }
                                        }


                                        //8の倍数の場合・ imageWidth is in multiples of 8
                                        if (imgData[i].imageWidth % 8 == 0)
                                        {
                                            Byte tmpCompBright = 0;

                                            for (Int32 k = 0; k < totalCount; ++k)
                                            {
                                                tmpCompBright = imgLoadFromFile[k];
                                                Buffer.BlockCopy(convertTableWIZ[tmpCompBright], 0, imgData[i].blvImg, k * 8, 8);
                                            }
                                        }
                                        //8の倍数で無い場合・imageWidth isn't in multiples of 8
                                        else
                                        {
                                            Byte tmpCompBright = 0;

                                            int modNumber = (int)(imgData[i].imageWidth - (imgData[i].imageWidth / 8) * 8);

                                            int curPosCopy = 0;

                                            for (int n = 0; n < imgData[i].imageLength; ++n)
                                            {
                                                // from 0 to width -2 / 0 ～ width-2
                                                for (int m = 0; m < imgData[i].imageWidth / 8; ++m)
                                                {
                                                    tmpCompBright = imgLoadFromFile[m + n * (imgData[i].imageWidth / 8 + 1)];

                                                    Buffer.BlockCopy(convertTableWIZ[tmpCompBright], 0, imgData[i].blvImg, curPosCopy, 8);

                                                    curPosCopy += 8;
                                                }

                                                // width -1
                                                int mPlus = (int)(imgData[i].imageWidth / 8);

                                                tmpCompBright = imgLoadFromFile[mPlus + n * (imgData[i].imageWidth / 8 + 1)];

                                                Buffer.BlockCopy(convertTableWIZ[tmpCompBright], 0, imgData[i].blvImg, curPosCopy, modNumber);

                                                curPosCopy += modNumber;

                                            }
                                        }



                                    }
                                    #endregion Convert 1bit->1byte / 1bit->1byte変換


                                    #endregion 非圧縮・1bitモノクロ読み取り / Uncompressed 1bit mono

                                    break;

                                case ImgType.gryImg_8:

                                    #region 非圧縮・8bitグレー読み取り  / Uncompressed 8 bit gray
                                    cntLength = offsetValue[i].stripByteCounts.Length;
                                    setFilePos = 0;
                                    curPosOfFile = 0;
                                    curPosOfCount = 0;
                                    totalCount = 0;
                                    totalCountFromFile = 0;


                                    for (UInt32 j = 0; j < cntLength; ++j)
                                    {
                                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                                    }

                                    imgLoadFromFile = new Byte[totalCountFromFile];

                                    //関数で読み取り
                                    for (Int32 j = 0; j < cntLength; ++j)
                                    {

                                        curPosOfFile = offsetValue[i].stripOffsets[j];
                                        curPosOfCount = offsetValue[i].stripByteCounts[j];

                                        //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                                        setFilePos = fileSTRM.Seek(curPosOfFile, SeekOrigin.Begin);


                                        fileSTRM.Read(imgLoadFromFile, (int)totalCount, (int)curPosOfCount);

                                        totalCount += curPosOfCount;
                                    }

                                    for (Int32 k = 0; k < totalCount; ++k)
                                    {
                                        Buffer.BlockCopy(imgLoadFromFile, k, imgData[i].gryImg_8, k, 1);
                                    }
                                    #endregion 非圧縮・8bitグレー読み取り  / Uncompressed 8 bit gray

                                    break;

                                case ImgType.gryImg_16:

                                    #region 非圧縮・16bitグレー読み取り / Uncompressed 16 bit gray
                                    cntLength = offsetValue[i].stripByteCounts.Length;
                                    setFilePos = 0;
                                    curPosOfFile = 0;
                                    curPosOfCount = 0;
                                    totalCount = 0;
                                    totalCountFromFile = 0;


                                    for (UInt32 j = 0; j < cntLength; ++j)
                                    {
                                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                                    }

                                    imgLoadFromFile = new Byte[totalCountFromFile];

                                    //関数で読み取り
                                    for (Int32 j = 0; j < cntLength; ++j)
                                    {

                                        curPosOfFile = offsetValue[i].stripOffsets[j];
                                        curPosOfCount = offsetValue[i].stripByteCounts[j];

                                        //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                                        setFilePos = fileSTRM.Seek(curPosOfFile, SeekOrigin.Begin);


                                        fileSTRM.Read(imgLoadFromFile, (int)totalCount, (int)curPosOfCount);

                                        totalCount += curPosOfCount;
                                    }


                                    //色変換テーブル作成 ・ make convert color table
                                    UInt16[] convertTable = new UInt16[65536];
                                    if (imgData[i].photometric == (int)PhotometricInterpretation.BlackIsZero)
                                    {
                                        for (int cnv = 0; cnv < 65536; ++cnv)
                                        {
                                            convertTable[cnv] = (UInt16)cnv;
                                        }
                                    }
                                    else if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                    {
                                        for (int cnv = 0; cnv < 65536; ++cnv)
                                        {
                                            convertTable[cnv] = (UInt16)(65535 - cnv);
                                        }
                                    }


                                    if (mImageFileHeader.byteOrderStr == "II")
                                    {
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            Buffer.BlockCopy(imgLoadFromFile, 2 * k, imgData[i].gryImg_16, 2 * k, 2);

                                            imgData[i].gryImg_16[k] = convertTable[imgData[i].gryImg_16[k]];
                                        }
                                    }
                                    else if (mImageFileHeader.byteOrderStr == "MM")
                                    {
                                        Byte[] tmp = new Byte[2];

                                        //一応，用意しておく。
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            //バイトを入れ替える
                                            tmp[0] = imgLoadFromFile[2 * k + 1];
                                            tmp[1] = imgLoadFromFile[2 * k];

                                            //バイト配列を変換
                                            imgData[i].gryImg_16[k] = convertTable[(UInt16)BitConverter.ToUInt16(tmp, 0)];
                                        }
                                    }



                                    #endregion 非圧縮・16bitグレー読み取り / Uncompressed 16 bit gray

                                    break;

                                case ImgType.gryImg_32:

                                    #region 非圧縮・32bitグレー読み取り / Uncompressed 32 bit gray
                                    cntLength = offsetValue[i].stripByteCounts.Length;
                                    setFilePos = 0;
                                    curPosOfFile = 0;
                                    curPosOfCount = 0;
                                    totalCount = 0;
                                    totalCountFromFile = 0;


                                    for (UInt32 j = 0; j < cntLength; ++j)
                                    {
                                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                                    }

                                    imgLoadFromFile = new Byte[totalCountFromFile];

                                    //関数で読み取り
                                    for (Int32 j = 0; j < cntLength; ++j)
                                    {

                                        curPosOfFile = offsetValue[i].stripOffsets[j];
                                        curPosOfCount = offsetValue[i].stripByteCounts[j];

                                        //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                                        setFilePos = fileSTRM.Seek(curPosOfFile, SeekOrigin.Begin);


                                        fileSTRM.Read(imgLoadFromFile, (int)totalCount, (int)curPosOfCount);

                                        totalCount += curPosOfCount;
                                    }


                                    //色変換テーブル作成 ・ make convert color table
                                    UInt32 convertValueInt32 = 0;
                                    if (imgData[i].photometric == (int)PhotometricInterpretation.BlackIsZero)
                                    {
                                        convertValueInt32 = 0;
                                    }
                                    else if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                    {
                                        convertValueInt32 = System.UInt32.MaxValue;
                                    }


                                    if (mImageFileHeader.byteOrderStr == "II")
                                    {
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            Buffer.BlockCopy(imgLoadFromFile, 4 * k, imgData[i].gryImg_32, 4 * k, 4);

                                            if (convertValueInt32 == 0)
                                            {
                                                imgData[i].gryImg_32[k] = imgData[i].gryImg_32[k];
                                            }
                                            else
                                            {
                                                imgData[i].gryImg_32[k] = convertValueInt32 - imgData[i].gryImg_32[k];
                                            }
                                        }
                                    }
                                    else if (mImageFileHeader.byteOrderStr == "MM")
                                    {
                                        Byte[] tmp = new Byte[4];

                                        //一応，用意しておく。
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            //バイトを入れ替える
                                            tmp[0] = imgLoadFromFile[4 * k + 3];
                                            tmp[1] = imgLoadFromFile[4 * k + 2];
                                            tmp[2] = imgLoadFromFile[4 * k + 1];
                                            tmp[3] = imgLoadFromFile[4 * k];

                                            //バイト配列を変換
                                            imgData[i].gryImg_32[k] = (UInt32)BitConverter.ToInt32(tmp, 0);


                                            if (convertValueInt32 == 0)
                                            {
                                                imgData[i].gryImg_32[k] = imgData[i].gryImg_32[k];
                                            }
                                            else
                                            {
                                                imgData[i].gryImg_32[k] = convertValueInt32 - imgData[i].gryImg_32[k];
                                            }
                                        }
                                    }



                                    #endregion 非圧縮・32bitグレー読み取り / Uncompressed 32 bit gray

                                    break;


                                case ImgType.gryImg_Sgl:

                                    #region 非圧縮・32bit(浮動小数点)グレー読み取り / Uncompressed 32 bit float gray
                                    cntLength = offsetValue[i].stripByteCounts.Length;
                                    setFilePos = 0;
                                    curPosOfFile = 0;
                                    curPosOfCount = 0;
                                    totalCount = 0;
                                    totalCountFromFile = 0;


                                    for (UInt32 j = 0; j < cntLength; ++j)
                                    {
                                        totalCountFromFile += offsetValue[i].stripByteCounts[j];
                                    }

                                    imgLoadFromFile = new Byte[totalCountFromFile];

                                    //関数で読み取り
                                    for (Int32 j = 0; j < cntLength; ++j)
                                    {

                                        curPosOfFile = offsetValue[i].stripOffsets[j];
                                        curPosOfCount = offsetValue[i].stripByteCounts[j];

                                        //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                                        setFilePos = fileSTRM.Seek(curPosOfFile, SeekOrigin.Begin);


                                        fileSTRM.Read(imgLoadFromFile, (int)totalCount, (int)curPosOfCount);

                                        totalCount += curPosOfCount;
                                    }

                                    //色変換テーブル作成 ・ make convert color table
                                    Single convertValueFloat = 0;
                                    if (imgData[i].photometric == (int)PhotometricInterpretation.BlackIsZero)
                                    {
                                        convertValueFloat = 0.0F;
                                    }
                                    else if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                    {
                                        convertValueFloat = 1.0F;
                                    }


                                    if (mImageFileHeader.byteOrderStr == "II")
                                    {
                                        Byte[] tmp = new Byte[4];

                                        //一応，用意しておく。
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            //バイトを入れ替える
                                            tmp[0] = imgLoadFromFile[4 * k];
                                            tmp[1] = imgLoadFromFile[4 * k + 1];
                                            tmp[2] = imgLoadFromFile[4 * k + 2];
                                            tmp[3] = imgLoadFromFile[4 * k + 3];

                                            //バイト配列を変換
                                            imgData[i].gryImg_Sgl[k] = (Single)BitConverter.ToSingle(tmp, 0);


                                            if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                            {
                                                imgData[i].gryImg_Sgl[k] = convertValueFloat - imgData[i].gryImg_Sgl[k];
                                            }
                                        }
                                    }
                                    else if (mImageFileHeader.byteOrderStr == "MM")
                                    {
                                        Byte[] tmp = new Byte[4];

                                        //一応，用意しておく。
                                        for (Int32 k = 0; k < imgData[i].buffSize; ++k)
                                        {
                                            //バイトを入れ替える

                                            tmp[0] = imgLoadFromFile[4 * k + 3];
                                            tmp[1] = imgLoadFromFile[4 * k + 2];
                                            tmp[2] = imgLoadFromFile[4 * k + 1];
                                            tmp[3] = imgLoadFromFile[4 * k];

                                            //バイト配列を変換
                                            imgData[i].gryImg_Sgl[k] = (Single)BitConverter.ToSingle(tmp, 0);

                                            if (imgData[i].photometric == (int)PhotometricInterpretation.WhiteIsZero)
                                            {
                                                imgData[i].gryImg_Sgl[k] = convertValueFloat - imgData[i].gryImg_Sgl[k];
                                            }
                                        }
                                    }



                                    #endregion 非圧縮・32bit(浮動小数点)グレー読み取り / Uncompressed 32 bit float gray

                                    break;


                                default:
                                    //別途実装
                                    strError = "Error occupied in ReadImageDataFromFile, Because this type image are not implemented.";
                                    result = (Int32)ClaTiffErr.Err_ReadTif_ReadImageDataFromFile_NotImplementTypeImage;
                                    break;
                            }

                            //エラーが発生していた場合
                            if ((Int32)ClaTiffErr.NoError != result)
                            {
                                return result;
                            }

                        }
                        else
                        {
                            //Compressed-圧縮あり

                            Int32 result = (Int32)ClaTiffErr.NoError;

                            //Byte[] imgLoadFromFile = null;

                            switch (curImgType)
                            {
                                default:
                                    //拡張機能
                                    strError = "Error occupied in ReadImageDataFromFile, Because Compressions are not implemented.";
                                    result = (Int32)ClaTiffErr.Err_ReadTif_ReadImageDataFromFile_NotImplementCompress;
                                    break;
                            }

                            //エラーが発生していた場合
                            if ((Int32)ClaTiffErr.NoError != result)
                            {
                                return result;
                            }

                        }
                    }
                }

            }
            catch (Exception exReadImageDataFromFile)
            {
                strError = "Error occupied in ReadImageDataFromFile:" + exReadImageDataFromFile.ToString();
                return (Int32)ClaTiffErr.Err_ReadTif_ReadImageDataFromFile;

            }

            #endregion Read Image Data


            if (suspendFlag == (Int32)ClaTiffSuspendFlag.ReadTif_ReadImageData_End_Suspend)
            {
                strError = "No Error In ReadTif(), Suspend After AllocImgset";
                return (Int32)ClaTiffErr.NoError_ReadTif_ReadImageData_End_Suspned;
            }


            //12 Read Value set (longer than 4 byte) - Additional Required Field, option



            return resVal;
        }



        #region  ReadAndGet2ByteValue()

        private Int32 ReadAndGet2ByteValue(FileStream fileSTRMTmp, UInt32 curPosOfFileTmp,
                                            string byteOrderStrTmp, string sequenceStr,
                                            ref UInt16 getValueTmp)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            Byte[] tmpBuf_size2_in = null;
            Byte[] tmpBuf_size2_out = null;

            Int64 setFilePos = 0;

            //Rewind file pos, 念のため読み取り開始位置を先頭にまき戻す。
            #region File Rewind And Check
            try
            {
                setFilePos = fileSTRMTmp.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception exStream)
            {
                strError = "File Rewind Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exStream.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_FileRewindError;
            }

            if (setFilePos != 0)
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_FileRewindCheckError;

            #endregion File Rewind And Check


            #region new and initialize buffer
            try
            {
                tmpBuf_size2_in = new Byte[2];
                tmpBuf_size2_out = new Byte[2];
            }
            catch (Exception ex_Buf)
            {
                strError = "New Byte[2] Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + ex_Buf.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_BuffAllocException;
            }

            //Initilize buffer before use
            resVal = InitializeByteBuffer(ref tmpBuf_size2_in, 2, sequenceStr);
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            resVal = InitializeByteBuffer(ref tmpBuf_size2_out, 2, sequenceStr);
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            #endregion new and initialize buffer


            //Set File Position
            #region Set File Position
            try
            {
                //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                setFilePos = fileSTRMTmp.Seek(curPosOfFileTmp, SeekOrigin.Begin);
            }
            catch (Exception exLoop)
            {
                strError = "Set File Position Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exLoop.ToString();

                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_SetFilePositionException;
            }

            if (setFilePos != curPosOfFileTmp)
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_SetFilePositionCheck;

            #endregion Set File Position


            //Read 2Byte Value
            #region  Read 2Byte Value

            try
            {
                fileSTRMTmp.Read(tmpBuf_size2_in, 0, 2);
            }
            catch (Exception exReadValFromFile)
            {
                strError = "Read Value Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exReadValFromFile.ToString();

                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_SetFilePositionException;
            }

            if (byteOrderStrTmp == "MM")
            {
                resVal = ChangeEndian(ref tmpBuf_size2_in, 2, ref tmpBuf_size2_out, sequenceStr);
                if (resVal != (Int32)ClaTiffErr.NoError) return resVal;
            }
            else if (byteOrderStrTmp == "II")
            {
                resVal = CopyBuffer(ref tmpBuf_size2_in, ref tmpBuf_size2_out, 2, sequenceStr);
                if (resVal != (Int32)ClaTiffErr.NoError) return resVal;
            }

            #endregion  Read 2Byte Value

            //Convert Byte buffer to UInt16
            #region Convert Byte buffer to UInt16
            try
            {
                //Convert Byte buffer to UInt16
                getValueTmp = (UInt16)BitConverter.ToUInt16(tmpBuf_size2_out, 0);
            }
            catch (ArgumentNullException argNullExcp)
            {
                strError = "Convert Byte Argument Null Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + argNullExcp.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_ConvertByteArgumentNullException;
            }
            catch (ArgumentOutOfRangeException argOORExcp)
            {
                strError = "Convert Byte Argument Out Of Range Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + argOORExcp.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_ConvertByteArgumentOutOfRangeException;
            }
            #endregion Convert Byte buffer to UInt16


            //後処理
            tmpBuf_size2_in = null;
            tmpBuf_size2_out = null;

            return resVal;
        }

        #endregion  ReadAndGet2ByteValue()

        #region  ReadAndGet4ByteValue()

        private Int32 ReadAndGet4ByteValue(FileStream fileSTRMTmp, UInt32 curPosOfFileTmp,
                                            string byteOrderStrTmp, string sequenceStr,
                                            ref UInt32 getValueTmp)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            Byte[] tmpBuf_size4_in = null;
            Byte[] tmpBuf_size4_out = null;

            Int64 setFilePos = 0;


            //Rewind file pos, 念のため読み取り開始位置を先頭にまき戻す。
            #region File Rewind And Check
            try
            {
                setFilePos = fileSTRMTmp.Seek(0, SeekOrigin.Begin);
            }
            catch (Exception exStream)
            {
                strError = "File Rewind Error in ReadAndGet2ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exStream.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_FileRewindError;
            }

            if (setFilePos != 0)
                return (Int32)ClaTiffErr.Err_ReadAndGet2ByteValue_FileRewindCheckError;

            #endregion File Rewind And Check


            #region new and initialize buffer
            try
            {
                tmpBuf_size4_in = new Byte[4];
                tmpBuf_size4_out = new Byte[4];
            }
            catch (Exception ex_Buf)
            {
                strError = "New Byte[4] Error in ReadAndGet4ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + ex_Buf.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_BuffAllocException;
            }

            //Initilize buffer before use
            resVal = InitializeByteBuffer(ref tmpBuf_size4_in, 4, sequenceStr);
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            resVal = InitializeByteBuffer(ref tmpBuf_size4_out, 4, sequenceStr);
            if (resVal != (Int32)ClaTiffErr.NoError) return resVal;

            #endregion new and initialize buffer


            //Set File Position
            #region Set File Position
            try
            {
                //Set curPosOfFile , 開始位置からcurPosOfFileへ移動
                setFilePos = fileSTRMTmp.Seek(curPosOfFileTmp, SeekOrigin.Begin);
            }
            catch (Exception exLoop)
            {
                strError = "Set File Position Error in ReadAndGet4ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exLoop.ToString();

                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_SetFilePositionException;
            }

            if (setFilePos != curPosOfFileTmp)
                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_SetFilePositionCheck;

            #endregion Set File Position


            //Read 4Byte Value
            #region  Read 4Byte Value

            try
            {
                fileSTRMTmp.Read(tmpBuf_size4_in, 0, 4);
            }
            catch (Exception exReadValFromFile)
            {
                strError = "Read Value Error in ReadAndGet4ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + exReadValFromFile.ToString();

                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_SetFilePositionException;
            }

            if (byteOrderStrTmp == "MM")
            {
                resVal = ChangeEndian(ref tmpBuf_size4_in, 4, ref tmpBuf_size4_out, sequenceStr);
                if (resVal != (Int32)ClaTiffErr.NoError) return resVal;
            }
            else if (byteOrderStrTmp == "II")
            {
                resVal = CopyBuffer(ref tmpBuf_size4_in, ref tmpBuf_size4_out, 4, sequenceStr);
                if (resVal != (Int32)ClaTiffErr.NoError) return resVal;
            }

            #endregion  Read 4Byte Value

            //Convert Byte buffer to UInt32
            #region Convert Byte buffer to UInt32
            try
            {
                //Convert Byte buffer to UInt32
                getValueTmp = (UInt32)BitConverter.ToUInt32(tmpBuf_size4_out, 0);
            }
            catch (ArgumentNullException argNullExcp)
            {
                strError = "Convert Byte Argument Null Error in ReadAndGet4ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + argNullExcp.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_ConvertByteArgumentNullException;
            }
            catch (ArgumentOutOfRangeException argOORExcp)
            {
                strError = "Convert Byte Argument Out Of Range Error in ReadAndGet4ByteValue()\r\n"
                            + "Phase:" + sequenceStr + "\r\n"
                            + "Detail:" + argOORExcp.ToString();
                return (Int32)ClaTiffErr.Err_ReadAndGet4ByteValue_ConvertByteArgumentOutOfRangeException;
            }
            #endregion Convert Byte buffer to UInt32


            //後処理
            tmpBuf_size4_in = null;
            tmpBuf_size4_out = null;

            return resVal;
        }

        #endregion  ReadAndGet4ByteValue()

        //
        //例えば、32bitで処理される環境で int a=0x00000001 としたとき、
        //メモリ上で 01,00,00,00 と並ぶのが「little-endian」
        //メモリ上で 00,00,00,01 と並ぶのが「big-endian」
        //

        #region ChangeEndian()

        private Int32 ChangeEndian(ref Byte[] inData, int dataSize, ref Byte[] outData, string phaseStr)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            //0. Check Input/Output Data for null

            //if input buffer is null,
            if (inData == null)
            {
                resVal = (Int32)ClaTiffErr.Err_ChangeEndian_InputDataNull;
                return resVal;
            }

            //if output buffer is null,
            if (outData == null)
            {
                resVal = (Int32)ClaTiffErr.Err_ChangeEndian_OuputDataNull;
                return resVal;
            }

            //1. Check Input/Output buffer size
            if (inData.Length != outData.Length)
            {
                resVal = (Int32)ClaTiffErr.Err_ChangeEndian_BufferSizeUnMatch;
                return resVal;
            }


            //2. Initialize output data
            for (int i = 0; i < dataSize; ++i)
                outData[i] = 0;


            //3. Change Endian
            try
            {
                for (int i = 0; i < dataSize; ++i)
                {
                    outData[i] = inData[dataSize - 1 - i];
                }
            }
            catch (Exception ex)
            {
                strError = "Change Endian Error\r\n"
                            + "phase:" + phaseStr + "\r\n"
                            + "Err detail:" + ex.ToString();
                return (Int32)ClaTiffErr.Err_ChangeEndian_RearrengeUnknownError;
            }

            return resVal;
        }


        #endregion ChangeEndian()

        #region Function InitializeByteBuffer()
        /// <summary>
        ///  Initialize input buffer (zero)
        /// </summary>
        /// <param name="buf">all elements are set to zero</param>
        /// <param name="size">buffer size</param>
        /// <param name="size">phase string (where to use in code)</param>
        /// <returns></returns>
        private Int32 InitializeByteBuffer(ref Byte[] buf, int size, string phaseStr)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            try
            {
                //Initialize
                for (int i = 0; i < size; ++i)
                    buf[i] = 0;
            }
            catch (Exception ex)
            {
                strError = "InitializeByteBuffer() at" + phaseStr + "\r\n" + ex.ToString() + "\r\n";
                return (Int32)ClaTiffErr.Err_InitializeByteBuffer_InitializeErr;
            }

            return resVal;
        }
        #endregion Function InitializeByteBuffer()

        #region CopyBuffer()

        /// <summary>
        /// Copy buffer from inBuf to outBuf
        /// </summary>
        /// <param name="inBuf">origin</param>
        /// <param name="outBuf">destination</param>
        /// <param name="size">buf size</param>
        /// <param name="phaseStr">phase string (where to use in code)</param>
        /// <returns></returns>
        private Int32 CopyBuffer(ref Byte[] inBuf, ref Byte[] outBuf, int size, string phaseStr)
        {
            Int32 resVal = (Int32)ClaTiffErr.NoError;

            try
            {
                //Copy
                for (int i = 0; i < size; ++i)
                    outBuf[i] = inBuf[i];
            }
            catch (Exception ex)
            {
                strError = "CopyBuffer() at" + phaseStr + "\r\n" + ex.ToString() + "\r\n";
                return (Int32)ClaTiffErr.Err_CopyBuffer_ManipulateErr;
            }


            return resVal;
        }

        #endregion CopyBuffer()




    }


}//namespace TifRW
}//namespace Image
}//namespace Snb