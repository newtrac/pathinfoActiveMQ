/**
* @date         2016-03-12
* @filename     TMAPConvert.h
* @purpose      interface for reading TMAP 7 and above using HTTP method
* @version      2.3
* @history      initial draft
* @author       Morgan Lei, UNIC, Beijing, China
* @copyright    Morgan.Lei@unic-tech.cn, UNIC Technologies Inc, 2005-2016. All rights reserved.
*/

#ifndef __HTTP_TMAP_H__
#define __HTTP_TMAP_H__

#ifdef ICONVERTOR_LIB
# define HTTPTMAP_API __declspec(dllexport)
#else
# define HTTPTMAP_API __declspec(dllimport)
#endif

#ifdef __cplusplus
extern "C"
{
#endif

    // get information of TMAP 7.0 or above
    // pcFileName could start with http:// or local file
    HTTPTMAP_API bool GetTMAPImageInfo(const char* pcFileName, int& nWidth, int& nHeight,
        int &nScanScale, float &fPixelSizeOf100X);

    // pcFileName could start with http:// or local file
    // nLayer starts from 0, nPosX and nPosY means the tile's index, not the pixel position
    // pucData should be allocated by user and make sure it's large enough
    HTTPTMAP_API int DecodeTileStream(const char* pcFileName, const int nLayer,
        const int nPosX, const int nPosY, unsigned char* pucData,
        const int nJpegQuality = 85);

    // pcFileName could start with http:// or local file
    // pucData should be allocated by user and make sure it's large enough
    HTTPTMAP_API int GetNaviImgData(const char* pcFileName, unsigned char* pucData,
        const int nJpegQuality = 85);

    // pcFileName could start with http:// or local file
    // pucData should be allocated by user and make sure it's large enough
    HTTPTMAP_API int GetMacroImgData(const char* pcFileName, unsigned char* pucData,
        const int nJpegQuality = 85);

    // pcFileName could start with http:// or local file
    // pucData should be allocated by user and make sure it's large enough
    HTTPTMAP_API int GetLabelImgData(const char* pcFileName, unsigned char* pucData,
        const int nJpegQuality = 85);

    // pcFileName could start with http:// or local file
    // pucData should be allocated by user and make sure it's large enough
    HTTPTMAP_API int GetThumbnailImgData(const char* pcFileName, unsigned char* pucData,
        const int nJpegQuality = 85);

    // pcFileName could start with http:// or local file
    // dZoomRate, current scale
    // (nPixelX, nPixelY) start point of ROI in current scale
    // (nPixelW, nPixelH) size of ROI in current scale
    HTTPTMAP_API int GetRoiImage(const char* pcFileName, const double dZoomRate,
        const int nPixelX, const int nPixelY, const int nPixelW, const int nPixelH,
        unsigned char* pucData, int nJpegQuality = 85);
 
    // pcFileName could start with http:// or local file
    // get extent information in TMAP
    // *pData will be allocated inside and it should be released using ReleaseMemory
    // paras not used yet
    HTTPTMAP_API bool GetExtentFileData(const char *pcFileName, const int nDataType,
        int *pDataLen, void **pData, void *paras);

    // release memory allocated in the above function
    HTTPTMAP_API void ReleaseMemory(void **pData);

    // release resources
    HTTPTMAP_API void ReleaseResources();

#ifdef __cplusplus
};
#endif

#endif
