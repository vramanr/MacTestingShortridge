
#define TWSCAN_VERSION 0x300

#define STR_BI_WINDOW_MESSAGE	"szBiTwainWindowMsg"

#define  TWSCAN_ACQUIRE          	 2
#define  TWSCAN_ACQUIRETOCLIPBOARD    4
#define  TWSCAN_SELECT               1
#define  TWSCAN_TERMINATE            3
#define  TWSTAT_BATCHSCAN            5 

#define GETCONT    0
#define SETCONT    1

#define O_GET    0
#define O_SET    1

#define  MSG_GET          		    1
#define  MSG_GETCURRENT         	2
#define  MSG_GETDEFAULT          	3
#define  MSG_RESET          		7
#define  MSG_SET         			6

#define TWTY_INT8        0    
#define TWTY_INT16       1    
#define TWTY_INT32       2    
#define TWTY_UINT8       3    
#define TWTY_UINT16      4    
#define TWTY_UINT32      5    
#define TWTY_BOOL        6    
#define TWTY_FIX32       7    
#define TWTY_FRAME       8    
#define TWTY_STR32       9   
#define TWTY_STR64       10    
#define TWTY_STR128      11    
#define TWTY_STR255      12    
#define TWTY_STR1024     13    
#define TWTY_UNI512      14   

#define C_ONEVALUE				1
#define C_ENUM					2
#define C_ARRAY					4
#define C_RANGE					8 

#define C_LEFT					0
#define C_TOP					1
#define C_RIGHT					2
#define C_BOTTOM				3

#define  TW_OK                  0
#define  TW_GENERAL_ERROR       1
#define  TW_NOTOPEN             2
#define  TW_TRANSFERFAILURE  	3
#define  TW_OPENDSMFAILURE      4
#define  TW_COMPATIBILITY   	5
#define  TW_OPENDSFAILURE       6
#define  TW_CLOSEDSFAILURE  	7
#define  TW_SELECTDSFAILURE 	8
#define  TW_NOTWAINDLL      	9
#define  TW_LOADED              10
#define  TW_INVALIDPARAMETER	11
#define  TW_NOTTIFFDLL			12
#define	 TW_FILESAVEERROR		13
#define  TW_NOTDLGDLL			14
#define  TW_SUPPORT_OPERATION	15
#define  TW_HWND_ERROR			16
#define	 TW_LAST				17



#define WM_BI_SCANNING_DOCUMENT_FINISHED			WM_USER+100
#define WM_BI_TWAIN_CLOSE							WM_USER+101
#define WM_BI_SCANNING_ERROR						WM_USER+102
#define WM_BI_FEEDER_EMPTY							WM_USER+103
#define WM_BI_PAGE_FINISHED							WM_USER+104


#define TCOMP_NOCOMP     201  ** Nocompression **
#define TCOMP_PACKBITS   202  ** Packbit **
#define TCOMP_LZW        203  ** LZW **
#define TCOMP_LZW_DIFF   204  ** LZW with differentiation **
#define TCOMP_CCITTG2    205  ** CCITT Group 3 1D NO EOL **
#define TCOMP_CCITTG31D  206  ** CCITT Group 3 1D **
#define TCOMP_CCITTG32D  207  ** CCITT Group 3 2D **
#define TCOMP_CCITTG4    208  ** CCITT Group 4 **
#define TCOMP_JPEG       209  ** JPEG **


#define TW_BW					0
#define TW_GRAY					1
#define TW_RGB					2
#define TW_PALETTE 				3
#define TW_CMY					4
#define TW_CMYK					5
#define TW_YUV					6
#define TW_YUVK					7
#define TW_CIEXYZ				8

#define TW_INCHES      			0
#define TW_CENTIMETERS 			1
#define TW_PICAS       			2
#define TW_POINTS      			3
#define TW_TWIPS       			4
#define TW_PIXELS      			5

#define TW_DISUI				0
#define TW_ENBSCANNERUI 		1
#define TW_ENBBIUI				2

#define ICAP_AUTOBRIGHT                   0x1100
#define ICAP_BRIGHTNESS                   0x1101
#define ICAP_CONTRAST                     0x1103
#define ICAP_CUSTHALFTONE                 0x1104
#define ICAP_EXPOSURETIME                 0x1105
#define ICAP_FILTER                       0x1106
#define ICAP_FLASHUSED                    0x1107
#define ICAP_GAMMA                        0x1108
#define ICAP_HALFTONES                    0x1109
#define ICAP_HIGHLIGHT                    0x110a
#define ICAP_IMAGEFILEFORMAT              0x110c
#define ICAP_LAMPSTATE                    0x110d
#define ICAP_LIGHTSOURCE                  0x110e
#define ICAP_ORIENTATION                  0x1110
#define ICAP_PHYSICALWIDTH                0x1111
#define ICAP_PHYSICALHEIGHT               0x1112
#define ICAP_SHADOW                       0x1113
#define ICAP_FRAMES                       0x1114
#define ICAP_XNATIVERESOLUTION            0x1116
#define ICAP_YNATIVERESOLUTION            0x1117
#define ICAP_XRESOLUTION                  0x1118
#define ICAP_YRESOLUTION                  0x1119
#define ICAP_MAXFRAMES                    0x111a
#define ICAP_TILES                        0x111b
#define ICAP_BITORDER                     0x111c
#define ICAP_CCITTKFACTOR                 0x111d
#define ICAP_LIGHTPATH                    0x111e
#define ICAP_PIXELFLAVOR                  0x111f
#define ICAP_PLANARCHUNKY                 0x1120
#define ICAP_ROTATION                     0x1121
#define ICAP_SUPPORTEDSIZES               0x1122
#define ICAP_THRESHOLD                    0x1123
#define ICAP_XSCALING                     0x1124
#define ICAP_YSCALING                     0x1125
#define ICAP_BITORDERCODES                0x1126
#define ICAP_PIXELFLAVORCODES             0x1127
#define ICAP_JPEGPIXELTYPE                0x1128
#define ICAP_TIMEFILL                     0x112a
#define ICAP_BITDEPTH                     0x112b
#define ICAP_BITDEPTHREDUCTION            0x112c  
#define ICAP_UNDEFINEDIMAGESIZE           0x112d  
#define ICAP_IMAGEDATASET                 0x112e  
#define ICAP_EXTIMAGEINFO                 0x112f  
#define ICAP_MINIMUMHEIGHT                0x1130  
#define ICAP_MINIMUMWIDTH                 0x1131  
#define ICAP_FLIPROTATION                 0x1136  
#define ICAP_BARCODEDETECTIONENABLED      0x1137  
#define ICAP_SUPPORTEDBARCODETYPES        0x1138  
#define ICAP_BARCODEMAXSEARCHPRIORITIES   0x1139  
#define ICAP_BARCODESEARCHPRIORITIES      0x113a  
#define ICAP_BARCODESEARCHMODE            0x113b  
#define ICAP_BARCODEMAXRETRIES            0x113c 
#define ICAP_BARCODETIMEOUT               0x113d  
#define ICAP_ZOOMFACTOR                   0x113e  
#define ICAP_PATCHCODEDETECTIONENABLED    0x113f  
#define ICAP_SUPPORTEDPATCHCODETYPES      0x1140  
#define ICAP_PATCHCODEMAXSEARCHPRIORITIES 0x1141  
#define ICAP_PATCHCODESEARCHPRIORITIES    0x1142  
#define ICAP_PATCHCODESEARCHMODE          0x1143  
#define ICAP_PATCHCODEMAXRETRIES          0x1144  
#define ICAP_PATCHCODETIMEOUT             0x1145  
#define ICAP_FLASHUSED2                   0x1146  
#define ICAP_IMAGEFILTER                  0x1147  
#define ICAP_NOISEFILTER                  0x1148  
#define ICAP_OVERSCAN                     0x1149  
#define ICAP_AUTOMATICBORDERDETECTION     0x1150  
#define ICAP_AUTOMATICDESKEW              0x1151  
#define ICAP_AUTOMATICROTATE              0x1152  
#define ICAP_JPEGQUALITY                  0x1153  
#define ICAP_UNITS                        0x102 


#define CAP_AUTHOR                  0x1000
#define CAP_CAPTION                 0x1001
#define CAP_FEEDERENABLED           0x1002
#define CAP_FEEDERLOADED            0x1003
#define CAP_TIMEDATE                0x1004
#define CAP_SUPPORTEDCAPS           0x1005
#define CAP_EXTENDEDCAPS            0x1006
#define CAP_AUTOFEED                0x1007
#define CAP_CLEARPAGE               0x1008
#define CAP_FEEDPAGE                0x1009
#define CAP_REWINDPAGE              0x100a
#define CAP_INDICATORS              0x100b   
#define CAP_SUPPORTEDCAPSEXT        0x100c   
#define CAP_PAPERDETECTABLE         0x100d   
#define CAP_UICONTROLLABLE          0x100e   
#define CAP_DEVICEONLINE            0x100f   
#define CAP_AUTOSCAN                0x1010   
#define CAP_THUMBNAILSENABLED       0x1011   
#define CAP_DUPLEX                  0x1012   
#define CAP_DUPLEXENABLED           0x1013   
#define CAP_ENABLEDSUIONLY          0x1014   
#define CAP_CUSTOMDSDATA            0x1015   
#define CAP_ENDORSER                0x1016  
#define CAP_JOBCONTROL              0x1017   
#define CAP_ALARMS                  0x1018   
#define CAP_ALARMVOLUME             0x1019   
#define CAP_AUTOMATICCAPTURE        0x101a   
#define CAP_TIMEBEFOREFIRSTCAPTURE  0x101b   
#define CAP_TIMEBETWEENCAPTURES     0x101c   
#define CAP_CLEARBUFFERS            0x101d   
#define CAP_MAXBATCHBUFFERS         0x101e   
#define CAP_DEVICETIMEDATE          0x101f   
#define CAP_POWERSUPPLY             0x1020   
#define CAP_CAMERAPREVIEWUI         0x1021   
#define CAP_DEVICEEVENT             0x1022   
#define CAP_SERIALNUMBER            0x1024   
#define CAP_PRINTER                 0x1026   
#define CAP_PRINTERENABLED          0x1027   
#define CAP_PRINTERINDEX            0x1028   
#define CAP_PRINTERMODE             0x1029   
#define CAP_PRINTERSTRING           0x102a   
#define CAP_PRINTERSUFFIX           0x102b   
#define CAP_LANGUAGE                0x102c   
#define CAP_FEEDERALIGNMENT         0x102d   
#define CAP_FEEDERORDER             0x102e   
#define CAP_REACQUIREALLOWED        0x1030   
#define CAP_BATTERYMINUTES          0x1032   
#define CAP_BATTERYPERCENTAGE       0x1033   



