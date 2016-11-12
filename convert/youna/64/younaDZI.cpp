//
//  youna.cpp
//  slideConvert
//
//  Created by Quan Yuan on 5/26/16.
//  Copyright © 2016 Quan Yuan. All rights reserved.
//
//


#include <iostream>
#include <WinSock2.h>
#include <windows.h>
#include <stdio.h>
#include <getopt.h>
#include <string.h>
#include <vector>

#include <Iphlpapi.h>
#include "TMAPConvert.h"
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
    const std::vector<std::string> targetMACs = {"{65032085-2E37-493F-BEC6-AA8F468BE713}", 
												"{45756D51-98A8-42BB-8F47-9B53A7A8C89B}",
												"{7437785D-B6D9-478B-89B0-6E153FF9A640}",
												"{25643C52-FB59-4B70-8BC5-840E3242935F}",
												"{FB9FB65F-CF6A-49D6-B79A-F9C52FF348FE}",
												"{231FB1DE-B64C-4ADC-839F-67D616866C75}"
												}; // {45756D51-98A8-42BB-8F47-9B53A7A8C89B} //awk ={65032085-2E37-493F-BEC6-AA8F468BE713}
	bool matchTarget = false;
    for(size_t i=0;i<macAddresses.size();i++){
        //cout<<"mac address:"<<macAddresses[i]<<std::endl;
        //cout<<"target address"<<targetMAC<<std::endl;
		for(size_t j=0;j<targetMACs.size();j++){
			if(macAddresses[i].compare(targetMACs[j]) == 0){
				matchTarget = true;
				break;
			}
		}
		if(matchTarget){
			break;
		}
    }
    if(matchTarget==false){
        cout<<"This program is bound with the network adaptor of Anhui AWK server only."<<std::endl;
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
    
    //HINSTANCE hDLL = LoadLibrary("iViewerSDK.dll");               // Handle to DLL
    
    // LPFNDLLFUNC0 lpfnDllFuncInitImageFileFunc;
    // LPFNDLLFUNC1 lpfnDllFuncUnInitImageFileFunc;    // Function pointers
    // LPFNDLLFUNC2 lpfnDllFuncGetHeaderInfoFunc;
    // LPFNDLLFUNC3 lpfnDllFuncGetImageStreamFunc;
    // LPFNDLLFUNC4 lpfnDllFuncGetPriviewInfoPathFunc;
    // LPFNDLLFUNC5 lpfnDllFuncGetImageDataRoiFunc;
    // LPFNDLLFUNC6 lpfnDllFuncDeleteImageDataFunc;
    
    // lpfnDllFuncInitImageFileFunc = (LPFNDLLFUNC0)GetProcAddress(hDLL, "InitImageFileFunc");
    // lpfnDllFuncUnInitImageFileFunc = (LPFNDLLFUNC1)GetProcAddress(hDLL, "UnInitImageFileFunc");
    // lpfnDllFuncGetHeaderInfoFunc = (LPFNDLLFUNC2)GetProcAddress(hDLL, "GetHeaderInfoFunc");
    // lpfnDllFuncGetImageStreamFunc = (LPFNDLLFUNC3)GetProcAddress(hDLL, "GetImageStreamFunc");
    // lpfnDllFuncGetPriviewInfoPathFunc = (LPFNDLLFUNC4)GetProcAddress(hDLL, "GetPriviewInfoPathFunc");
    // lpfnDllFuncGetImageDataRoiFunc = (LPFNDLLFUNC5)GetProcAddress(hDLL, "GetImageDataRoiFunc");
    // lpfnDllFuncDeleteImageDataFunc = (LPFNDLLFUNC6)GetProcAddress(hDLL, "DeleteImageDataFunc");
    
    
    ImageInfoStruct imageInfo;
    ImageHeaderInfo imageHeader;
    std::string input_file_name(argv[1]);
    std::string output_file_name, labelJpgFile, macroJpgFile;
    if(argc==3){
        output_file_name = std::string(argv[2]);
    }
    else{
        output_file_name = input_file_name+".jpg";
        labelJpgFile = input_file_name+"_label.jpg";
		macroJpgFile = input_file_name+"_macro.jpg";
    }
    
    if(GetTMAPImageInfo(input_file_name.c_str(),imageHeader.khiImageWidth, 
                        imageHeader.khiImageHeight,imageHeader.khiScanScale,imageHeader.khiImageCapRes)){
        // bool b0 = lpfnDllFuncGetHeaderInfoFunc(imageInfo, imageHeader.khiImageHeight,
        //                                        imageHeader.khiImageWidth,
        //                                        imageHeader.khiScanScale,
        //                                        imageHeader.khiSpendTime,
        //                                        imageHeader.khiScanTime,
        //                                        imageHeader.khiImageCapRes,
        //                                        imageHeader.khiImageBlockSize);
        bool b0 = true;
        if(b0){
            std::cout<<"image height: "<<imageHeader.khiImageHeight<<std::endl
            <<"image width: "<<imageHeader.khiImageWidth<<std::endl
            //<<"block size: "<<imageHeader.khiImageBlockSize<<std::endl
            <<"image scan scale: "<<imageHeader.khiScanScale<<std::endl
            <<"image capture pixel size: "<<imageHeader.khiImageCapRes<<std::endl;
            //assume jpeg data length <= full data length / 10
			//size_t jpegDataSize = (long long)imageHeader.khiImageHeight*(long long)imageHeader.khiImageWidth*2;
			size_t jpegDataSize =40960000;
			std::cout<<"allocate size="<<jpegDataSize<<std::endl;
            //unsigned char* imageData=new unsigned char[(long long)imageHeader.khiImageHeight*(long long)imageHeader.khiImageWidth/10];
            unsigned char* imageData=new unsigned char[jpegDataSize];
            
            //label image
            int labelDataLength=0, labelWidth, labelHeight;
            labelDataLength = GetLabelImgData( input_file_name.c_str(), imageData);
            if(labelDataLength>0){
              std::cout<<"label image loaded length "<<labelDataLength<<std::endl;   
            }
            // std::cout<<"label image height: "<<labelHeight<<std::endl
            // <<"label image width: "<<labelWidth<<std::endl
            // <<"label image data length: "<<labelDataLength<<std::endl;
            //write2BMP(imageData,labelWidth,labelHeight, labelBmpFile.c_str());
            FILE * fp = fopen(labelJpgFile.c_str(), "wb");
            fwrite(imageData, labelDataLength, 1, fp);
            fclose(fp);
            //lpfnDllFuncDeleteImageDataFunc(imageData);
            
			//Macro image
             int macroDataLength, macroWidth, macroHeight;
             //bool b1 = lpfnDllFuncGetPriviewInfoPathFunc( input_file_name.c_str(), &imageData, macroDataLength, macroWidth, macroHeight );
             macroDataLength = GetMacroImgData(input_file_name.c_str(), imageData);
			 std::cout<<"macro image data length: "<<macroDataLength<<std::endl;
             //write2BMP(imageData,macroWidth,macroHeight, macroBmpFile.c_str());
             fp = fopen(macroJpgFile.c_str(), "wb");
             fwrite(imageData, macroDataLength, 1, fp);
             fclose(fp);
            
            int dataLength, r=1;
            double zoom = imageHeader.khiScanScale;
            //image roi
            //GetImageDataRoiFunc( ImageInfoStruct sImageInfo, float fScale, int sp_x, int sp_y, int nWidth, int nHeight,BYTE** pBuffer, int&DataLength, bool flag);
            try{
				std::cout<<"load image roi. "<<std::endl;
                dataLength =  GetRoiImage( input_file_name.c_str(), 
                                        zoom/r, 0, 0,
                                                         (int)(2560),
                                                         (int)(2560),
                                                         imageData);
				fp = fopen("0.jpg", "wb");
                fwrite(imageData, dataLength, 1, fp);
                fclose(fp); 
                std::cout<<"load image data length: "<<dataLength<<std::endl;
				std::string cmd = "jpegtran.exe -maxmemory 5000M -crop "+std::to_string(imageHeader.khiImageWidth) +"x"
													+std::to_string(imageHeader.khiImageHeight)+"+0+0 -outfile "
													+output_file_name + " 0.jpg";
				std::cout<<cmd<<std::endl;
				system(cmd.c_str());
               /*  fp = fopen(output_file_name.c_str(), "wb");
                fwrite(imageData, dataLength, 1, fp);
                fclose(fp); */
				delete [] imageData;
                //lpfnDllFuncDeleteImageDataFunc(imageData);
            }
            catch(std::exception e){
                std::cout<<input_file_name<<": failed to read slide at scan scale "<<imageHeader.khiScanScale/r<<std::endl;
            }
            
             
             
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
            //lpfnDllFuncUnInitImageFileFunc( imageInfo);
            return -1;
        }
       // lpfnDllFuncUnInitImageFileFunc( imageInfo);
    }
    else{
        std::cout<<"cannot open file:"<<input_file_name<<std::endl;
    }
    
    return 0;
}









