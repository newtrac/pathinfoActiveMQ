//get mac address
#include <stdio.h>
#include <WinSock2.h>
#include <Iphlpapi.h>
#include <iostream>
using namespace std;
#pragma comment(lib,"Iphlpapi.lib") //add lib


// typedef struct _IP_ADAPTER_INFO {
//     struct _IP_ADAPTER_INFO* Next;
//     DWORD ComboIndex;
//     char AdapterName[MAX_ADAPTER_NAME_LENGTH + 4];
//     char Description[MAX_ADAPTER_DESCRIPTION_LENGTH + 4];
//     UINT AddressLength;
//     BYTE Address[MAX_ADAPTER_ADDRESS_LENGTH];
//     DWORD Index;
// UINT Type;//type of adapters

// *   MIB_IF_TYPE_OTHER     1
// *   MIB_IF_TYPE_ETHERNET     6
// *   MIB_IF_TYPE_TOKENRING     9
// *   MIB_IF_TYPE_FDDI     15
// *   MIB_IF_TYPE_PPP     23
// *   MIB_IF_TYPE_LOOPBACK      24
// *   MIB_IF_TYPE_SLIP      28

// UINT DhcpEnabled;
// PIP_ADDR_STRING CurrentIpAddress;
// IP_ADDR_STRING IpAddressList;
// IP_ADDR_STRING GatewayList;
// IP_ADDR_STRING DhcpServer;
// BOOL HaveWins;
// IP_ADDR_STRING PrimaryWinsServer;
// IP_ADDR_STRING SecondaryWinsServer;
// time_t LeaseObtained;
// time_t LeaseExpires;
// } IP_ADAPTER_INFO,*PIP_ADAPTER_INFO;

// typedef struct _IP_ADDR_STRING
// {
// struct _IP_ADDR_STRING* Next;  //net IP
// IP_ADDRESS_STRING IpAddress;  //IP
// IP_MASK_STRING IpMask; //subnet mask
// DWORD Context;// net table entrance
// } IP_ADDR_STRING;

int main(int argc, char* argv[])
{
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
            cout<<"number of network cards:"<<++netCardNum<<endl;
            cout<<"name of network cards:"<<pIpAdapterInfo->AdapterName<<endl;
            cout<<"Description of network cards"<<pIpAdapterInfo->Description<<endl;
            switch(pIpAdapterInfo->Type)
            {
                case MIB_IF_TYPE_OTHER:
                cout<<"network card type:"<<"OTHER"<<endl;
                break;
                case MIB_IF_TYPE_ETHERNET:
                cout<<"network card type:"<<"ETHERNET"<<endl;
                break;
                case MIB_IF_TYPE_TOKENRING:
                cout<<"network card type:"<<"TOKENRING"<<endl;
                break;
                case MIB_IF_TYPE_FDDI:
                cout<<"network card type:"<<"FDDI"<<endl;
                break;
                case MIB_IF_TYPE_PPP:
                printf("PP\n");
                cout<<"network card type:"<<"PPP"<<endl;
                break;
                case MIB_IF_TYPE_LOOPBACK:
                cout<<"network card type:"<<"LOOPBACK"<<endl;
                break;
                case MIB_IF_TYPE_SLIP:
                cout<<"network card type:"<<"SLIP"<<endl;
                break;
                default:

                break;
            }
            cout<<"MAC address:";
            for (DWORD i = 0; i < pIpAdapterInfo->AddressLength; i++)
                if (i < pIpAdapterInfo->AddressLength-1)
                {
                    printf("%02X-", pIpAdapterInfo->Address[i]);
                }
                else
                {
                    printf("%02X\n", pIpAdapterInfo->Address[i]);
                }
                cout<<"IP address as following:"<<endl;

                IP_ADDR_STRING *pIpAddrString =&(pIpAdapterInfo->IpAddressList);
                do 
                {
                    cout<<"number of ip:"<<++IPnumPerNetCard<<endl;
                    cout<<"IP address:"<<pIpAddrString->IpAddress.String<<endl;
                    cout<<"subnet address:"<<pIpAddrString->IpMask.String<<endl;
                    cout<<"gateway sddress:"<<pIpAdapterInfo->GatewayList.IpAddress.String<<endl;
                    pIpAddrString=pIpAddrString->Next;
                } while (pIpAddrString);
                pIpAdapterInfo = pIpAdapterInfo->Next;
                cout<<"--------------------------------------------------------------------"<<endl;
            }

        }
//release
        if (pIpAdapterInfo)
        {
            delete pIpAdapterInfo;
        }

        return 0;
    }

