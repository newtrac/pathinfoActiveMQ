//
//  main.cpp
//  slideConvert
//
//  Created by Quan Yuan on 5/24/16.
//  Copyright © 2016 Quan Yuan. All rights reserved.
//

#include <iostream>
#include <windows.h>
#include <stdio.h>
#include <getopt.h>
#include <string.h>
#include <vector>
#include <WinSock2.h>
#include <Iphlpapi.h>
using namespace std;
#pragma comment(lib,"Iphlpapi.lib") //add lib

typedef struct IMAGE_INFO_STRUCT
{
    LPARAM DataFilePTR;
}ImageInfoStruct;

typedef struct IMAGE_HEADER
{
    int khiImageHeight;
    int	khiImageWidth;
    int	khiScanScale;
    float khiSpendTime;
    double khiScanTime;
    float khiImageCapRes;
    int khiImageBlockSize;
}ImageHeaderInfo;

typedef bool (CALLBACK* LPFNDLLFUNC0)(ImageInfoStruct&, const char*);
typedef bool (CALLBACK* LPFNDLLFUNC1)(ImageInfoStruct&);
typedef bool (CALLBACK* LPFNDLLFUNC2)(ImageInfoStruct, int&, int&, int&, float&, double&, float&, int&);
typedef char* (__stdcall* LPFNDLLFUNC3)(ImageInfoStruct&, float, int, int, int&, unsigned char**);
//bool GetPriviewInfoPathFunc( constchar* szFilePath, unsignedchar** ImageData, int& nDataLength, int& nPriviewWidth, int& nPriviewHeight );
typedef bool (__stdcall* LPFNDLLFUNC4)(const char*, unsigned char**, int&, int&, int& );
//boolGetImageDataRoiFunc( ImageInfoStruct sImageInfo, float fScale, int sp_x, int sp_y, int nWidth, int nHeight,BYTE** pBuffer, int&DataLength, bool flag);
typedef bool (__stdcall* LPFNDLLFUNC5)( ImageInfoStruct, float, int, int, int, int,BYTE** , int&, bool);
typedef bool (__stdcall* LPFNDLLFUNC6)(LPVOID);

int write2BMP(const uint8_t* data,int w,int h, const char* bmpFileName){
    
    int filesize = 54 + 3*w*h;  //w is your image width, h is image height, both int
    unsigned char bmpfileheader[14] = {'B','M', 0,0,0,0, 0,0, 0,0, 54,0,0,0};
    unsigned char bmpinfoheader[40] = {40,0,0,0, 0,0,0,0, 0,0,0,0, 1,0, 24,0};
    unsigned char bmppad[3] = {0,0,0};
    
    bmpfileheader[ 2] = (unsigned char)(filesize    );
    bmpfileheader[ 3] = (unsigned char)(filesize>> 8);
    bmpfileheader[ 4] = (unsigned char)(filesize>>16);
    bmpfileheader[ 5] = (unsigned char)(filesize>>24);
    
    bmpinfoheader[ 4] = (unsigned char)(       w    );
    bmpinfoheader[ 5] = (unsigned char)(       w>> 8);
    bmpinfoheader[ 6] = (unsigned char)(       w>>16);
    bmpinfoheader[ 7] = (unsigned char)(       w>>24);
    bmpinfoheader[ 8] = (unsigned char)(       h    );
    bmpinfoheader[ 9] = (unsigned char)(       h>> 8);
    bmpinfoheader[10] = (unsigned char)(       h>>16);
    bmpinfoheader[11] = (unsigned char)(       h>>24);
    
    FILE *f = fopen(bmpFileName,"wb");
    if(f == 0){
        printf("can't open bmp file to write!\n");
        return -1;
    }
    
    fwrite(bmpfileheader,1,14,f);
    fwrite(bmpinfoheader,1,40,f);
    for(unsigned i=0; i<h; i++)
    {
        fwrite(data+(w*(h-i-1)*3),3,w,f);
        fwrite(bmppad,1,(4-(w*3)%4)%4,f);
    }
    fclose(f);
    
    return 0;
}

void write2BMP3(const uint8_t* data, int width, int height, const char* bmpFile){
    uint8_t * bmpData = (uint8_t* )malloc(width*height*3);
    for(size_t i=0;i<width*height;i++){
        for(size_t k=0;k<3;k++){
            bmpData[i*3+k] = data[i];
        }
    }
    write2BMP(bmpData, width, height, bmpFile);
    free(bmpData);
    
}

std::vector<std::string> getMacAddressAsUUID(){
//PIP_ADAPTER_INFO store info
    PIP_ADAPTER_INFO pIpAdapterInfo = new IP_ADAPTER_INFO();
//get structure size
    unsigned long stSize = sizeof(IP_ADAPTER_INFO);
//call GetAdaptersInfo,stSize pass in and out
    int nRel = GetAdaptersInfo(pIpAdapterInfo,&stSize);
//number of adapter
    int netCardNum = 0;
//number of IP on each adapter
    int IPnumPerNetCard = 0;

    std::vector< std::string > macUUIDs;

    if (ERROR_BUFFER_OVERFLOW == nRel)
    {
//if returns ERROR_BUFFER_OVERFLOW
//GetAdaptersInfo does not get enough memory,send stSize,meaning the space needed

//释放原来的内存空间
        delete pIpAdapterInfo;
//re-allocate memory of adapters
        pIpAdapterInfo = (PIP_ADAPTER_INFO)new BYTE[stSize];
//call again
        nRel=GetAdaptersInfo(pIpAdapterInfo,&stSize);    
    }
    if (ERROR_SUCCESS == nRel)
    {
//output info of cards
//may have multiple cards
        while (pIpAdapterInfo)
        {
            //cout<<"number of network cards:"<<++netCardNum<<endl;
            //cout<<"name of network cards:"<<pIpAdapterInfo->AdapterName<<endl;
            //cout<<"Description of network cards"<<pIpAdapterInfo->Description<<endl;
            macUUIDs.push_back(std::string(pIpAdapterInfo->AdapterName));
            pIpAdapterInfo = pIpAdapterInfo->Next;
            //cout<<"--------------------------------------------------------------------"<<endl;
        }

    }
//release
    if (pIpAdapterInfo)
    {
        delete pIpAdapterInfo;
    }

    return macUUIDs;
}


int main(int argc, const char* argv[]){
	std::vector<std::string> macAddresses = getMacAddressAsUUID();
    const std::string targetMAC = "{B70737F4-D7D7-4617-97F5-7EBC42E31CE6}";
    bool matchTarget = false;
    for(size_t i=0;i<macAddresses.size();i++){
        //cout<<"mac address:"<<macAddresses[i]<<std::endl;
        //cout<<"target address"<<targetMAC<<std::endl;
        if(macAddresses[i].compare(targetMAC) == 0){
            matchTarget = true;
            break;
        }
    }
    if(matchTarget==false){
        cout<<"This program is binded with Luman server only."<<std::endl;
        return -1;
    }


    int nextOption;
    // A string listing valid short options letters.
    const char *const shortOptions = "i:m";
    
    // An array describing valid long options.
    const struct option longOptions[] = {
        {"inputFilename", required_argument, NULL, 'i'},
        {"modelFile", optional_argument, NULL, 'm'},
    };
    if(argc < 2){
        std::cout<<"need input file path!"<<std::endl;
        return 0;
    }
    
    
    HINSTANCE hDLL = LoadLibrary("ImageOperationLib.dll");               // Handle to DLL
    
    LPFNDLLFUNC0 lpfnDllFuncInitImageFileFunc;
    LPFNDLLFUNC1 lpfnDllFuncUnInitImageFileFunc;    // Function pointers
    LPFNDLLFUNC2 lpfnDllFuncGetHeaderInfoFunc;
    LPFNDLLFUNC3 lpfnDllFuncGetImageStreamFunc;
    LPFNDLLFUNC4 lpfnDllFuncGetPriviewInfoPathFunc;
    LPFNDLLFUNC5 lpfnDllFuncGetImageDataRoiFunc;
    LPFNDLLFUNC6 lpfnDllFuncDeleteImageDataFunc;
    
    lpfnDllFuncInitImageFileFunc = (LPFNDLLFUNC0)GetProcAddress(hDLL, "InitImageFileFunc");
    lpfnDllFuncUnInitImageFileFunc = (LPFNDLLFUNC1)GetProcAddress(hDLL, "UnInitImageFileFunc");
    lpfnDllFuncGetHeaderInfoFunc = (LPFNDLLFUNC2)GetProcAddress(hDLL, "GetHeaderInfoFunc");
    lpfnDllFuncGetImageStreamFunc = (LPFNDLLFUNC3)GetProcAddress(hDLL, "GetImageStreamFunc");
    lpfnDllFuncGetPriviewInfoPathFunc = (LPFNDLLFUNC4)GetProcAddress(hDLL, "GetPriviewInfoPathFunc");
    lpfnDllFuncGetImageDataRoiFunc = (LPFNDLLFUNC5)GetProcAddress(hDLL, "GetImageDataRoiFunc");
    lpfnDllFuncDeleteImageDataFunc = (LPFNDLLFUNC6)GetProcAddress(hDLL, "DeleteImageDataFunc");

    
    ImageInfoStruct imageInfo;
    ImageHeaderInfo imageHeader;
    std::string input_file_name(argv[1]);
    std::string output_file_name, previewJpgFile;
    if(argc==3){
        output_file_name = std::string(argv[2]);
    }
    else{
        output_file_name = input_file_name+".jpg";
        previewJpgFile = input_file_name+"_preview.jpg";
    }
    
    if(lpfnDllFuncInitImageFileFunc( imageInfo, input_file_name.c_str() )){
        bool b0 = lpfnDllFuncGetHeaderInfoFunc(imageInfo, imageHeader.khiImageHeight,
                                               imageHeader.khiImageWidth,
                                               imageHeader.khiScanScale,
                                               imageHeader.khiSpendTime,
                                               imageHeader.khiScanTime,
                                               imageHeader.khiImageCapRes,
                                               imageHeader.khiImageBlockSize);
        if(b0){
            std::cout<<"image height: "<<imageHeader.khiImageHeight<<std::endl
            <<"image width: "<<imageHeader.khiImageWidth<<std::endl
			<<"block size: "<<imageHeader.khiImageBlockSize<<std::endl
            <<"image scan scale: "<<imageHeader.khiScanScale<<std::endl
            <<"image capture resolution: "<<imageHeader.khiImageCapRes<<std::endl;
            unsigned char* imageData;
            
             //preview image
             int previewDataLength, previewWidth, previewHeight;
             bool b1 = lpfnDllFuncGetPriviewInfoPathFunc( input_file_name.c_str(), &imageData, previewDataLength, previewWidth, previewHeight );
             std::cout<<"preview image height: "<<previewHeight<<std::endl
             <<"preview image width: "<<previewWidth<<std::endl
             <<"preview image data length: "<<previewDataLength<<std::endl;
             //write2BMP(imageData,previewWidth,previewHeight, previewBmpFile.c_str());
             FILE * fp = fopen(previewJpgFile.c_str(), "wb");
             fwrite(imageData, previewDataLength, 1, fp);
             fclose(fp);
             lpfnDllFuncDeleteImageDataFunc(imageData);

            
            int dataLength, r=2;
            //image roi
            //GetImageDataRoiFunc( ImageInfoStruct sImageInfo, float fScale, int sp_x, int sp_y, int nWidth, int nHeight,BYTE** pBuffer, int&DataLength, bool flag);
            bool b2 = lpfnDllFuncGetImageDataRoiFunc( imageInfo, (float)imageHeader.khiScanScale/2, 0, 0,
                                                     (int)(imageHeader.khiImageWidth/2),
													 (int)(imageHeader.khiImageHeight/2),
                                                     &imageData, dataLength, true);
            std::cout<<"load image data length: "<<dataLength<<std::endl;
            fp = fopen(output_file_name.c_str(), "wb");
            fwrite(imageData, dataLength, 1, fp);
            fclose(fp);
            lpfnDllFuncDeleteImageDataFunc(imageData);
            /*
             //preview image
            int previewDataLength, previewWidth, previewHeight;
            bool b1 = lpfnDllFuncGetPriviewInfoPathFunc( input_file_name.c_str(), &imageData, previewDataLength, previewWidth, previewHeight );
            std::cout<<"preview image height: "<<previewHeight<<std::endl
            <<"preview image width: "<<previewWidth<<std::endl
            <<"preview image data length: "<<previewDataLength<<std::endl;
            //write2BMP(imageData,previewWidth,previewHeight, previewBmpFile.c_str());
            FILE * fp = fopen(output_file_name.c_str(), "wb");
            fwrite(imageData, previewDataLength, 1, fp);
            fclose(fp);
            */
            /* // jpeg stream
            unsigned char** jpegStream;
            int jpegDataLength;
            lpfnDllFuncGetImageStreamFunc(imageInfo, (float)imageHeader.khiScanScale, imageHeader.khiImageWidth/2,
                                          imageHeader.khiImageHeight/2, jpegDataLength, jpegStream);
            FILE * fp = fopen(output_file_name.c_str(), "wb");
            fwrite(jpegStream, jpegDataLength, 1, fp);
            fclose(fp);
            */
        }
        else{
            std::cout<<"failed to load image header. quit."<<std::endl;
            lpfnDllFuncUnInitImageFileFunc( imageInfo);
            return -1;
        }
        lpfnDllFuncUnInitImageFileFunc( imageInfo);
    }
    else{
        std::cout<<"cannot open file:"<<input_file_name<<std::endl;
    }
    
    return 0;
}








