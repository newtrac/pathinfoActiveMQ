/**
* @date         2016-01-18
* @filename     iViewerC41.h
* @purpose      iViewer C interface for Henan province.
* @version      1.0
* @history      initial draft
* @author       Morgan Lei, UNIC, Beijing, China
* @copyright    Morgan.Lei@unic-tech.cn, UNIC Technologies Inc, 2005-2016. All rights reserved.
*/

#ifndef __IVIEWER_C_41_H__
#define __IVIEWER_C_41_H__

#ifdef IVIEWERC41_EXPORTS
#define IVIEWERC41_API __declspec(dllexport)
#else
#define IVIEWERC41_API __declspec(dllimport)
#endif

typedef long INT32;
typedef void* LPVOID;

#ifdef __cplusplus
extern "C" {
#endif

    typedef struct IMAGE_INFO_STRUCT
    {
        int DataFilePTR;
    } ImageInfoStruct;

    // 参数：
    // 1.sImageInfo：返回数字图像文件指针
    // 2.path：数字图像路径
    IVIEWERC41_API bool InitImageFileFunc(ImageInfoStruct &sImageInfo, const char *Path);

    // 参数：
    // 1.sImageInfo：传入数字图像文件指针
    IVIEWERC41_API bool UnInitImageFileFunc(ImageInfoStruct &sImageInfo);

    // 参数：
    // 1.sImageInfo：传入数字图像文件指针
    // 2.fScale：传入倍率
    // 3.nImagePosX：传入X坐标
    // 4.nImagePosY：传入Y坐标
    // 5.nDataLength：返回图像数据长度
    // 6.ImageStream：返回图像数据指针
    // 备注：每一块图像大小必须是256*256
    IVIEWERC41_API unsigned char* GetImageStreamFunc(ImageInfoStruct &sImageInfo,
        float fScale,
        INT32 nImagePosX,
        INT32 nImagePosY,
        INT32 &nDataLength,
        unsigned char **ImageStream);

    // 参数：
    // 1.pImageData：传入图像数据指针
    IVIEWERC41_API bool DeleteImageDataFunc(LPVOID pImageData);

    // 参数：
    // 1.szFilePath：传入数字文件路径
    // 2.ImageData：返回图像数据指针
    // 3.nDataLength：返回长度
    // 4.nThumWidth：返回宽度
    // 5.nThumHeght：返回高度
    IVIEWERC41_API bool GetThumnailImagePathFunc(const char *szFilePath,
        unsigned char **ImageData,
        INT32 &nDataLength,
        INT32 &nThumWidth,
        INT32 &nThumHeght);

    // 参数：
    // 1.szFilePath：传入数字文件路径
    // 2.ImageData：返回图像数据指针
    // 3.nDataLength：返回长度
    // 4.nPriviewWidth：返回宽度
    // 5.nPriviewHeight：返回高度
    IVIEWERC41_API bool GetPriviewInfoPathFunc(const char *szFilePath,
        unsigned char **ImageData,
        INT32 &nDataLength,
        INT32 &nPriviewWidth,
        INT32 &nPriviewHeight);

    // 参数：
    // 1.szFilePath：传入数字文件路径
    // 2.ImageData：返回图像数据指针
    // 3.nDataLength：返回长度
    // 4.nLabelWidth：返回宽度
    // 5.nLabelHeight：返回高度
    IVIEWERC41_API bool GetLableInfoPathFunc(const char *szFilePath,
        unsigned char **ImageData,
        INT32 &nDataLength,
        INT32 &nLabelWidth,
        INT32 &nLabelHeight);

    // 参数：
    // 1.ImageInfo：传入图像数据指针
    // 2.khiImageHeight：返回扫描高度
    // 3.khiImageWidth：返回扫描宽度
    // 4.khiScanScale：返回扫描倍率
    // 5.khiSpendTime：返回扫描时间
    // 6.khiImageCapRes：返回图像比例
    // 7.khiImageBlockSize：返回图像块大小
    IVIEWERC41_API bool GetHeaderInfoFunc(ImageInfoStruct sImageInfo,
        long &khiImageHeight,
        long &khiImageWidth,
        long &khiScanScale,
        float &khiSpendTime,
        double &khiScanTime,	
        float &khiImageCapRes, 
        long &khiImageBlockSize);

    // 参数
    // 1.ImageInfo：传入图像数据指针
    // 2.fScale：传入倍率
    // 3.sp_x：左上角X坐标
    // 4.sp_y：右上角Y坐标
    // 5.nWidth：宽度
    // 6.nHeight：高度
    // 7.pBuffer：返回图像数据指针
    // 8.DataLength：返回图像字节长度
    // 9.flag：true
    IVIEWERC41_API bool GetImageDataRoiFunc(ImageInfoStruct sImageInfo,
        float fScale,
        INT32 sp_x,
        INT32 sp_y,
        INT32 nWidth,
        INT32 nHeight,
        unsigned char **pBuffer,
        INT32 &DataLength,
        bool flag);

#ifdef __cplusplus
};
#endif

#endif // __IVIEWER_C_41_H__
