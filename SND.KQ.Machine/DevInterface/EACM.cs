using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SND.KQ.Machines.DevInterface
{
    public class EACM
    {
        public delegate long DeletGetData(USER_RESULT result, long lDataLen, long lIndex, long lType, string pUserData);

        public DeletGetData Trapdata;
        public EACM()
        {
            Trapdata = new DeletGetData(GetData);
        }
        public EventHandler EventTrapUser;

        [DllImport("EACM.dll")]
        public static extern IntPtr FIRS_DEV_Init(long lDevType, DeletGetData pOnGetData, string pUserData);


        [DllImport("EACM.dll")]
        public static extern void FIRS_DEV_UnInit(IntPtr hDev);

        [DllImport("EACM.dll")]
        public static extern int FIRS_DEV_Connect(IntPtr hDev, string id, long port);

        [DllImport("EACM.dll")]
        public static extern void FIRS_DEV_DisConnect(IntPtr hDev);

        [DllImport("EACM.dll")]
        public static extern int FIRS_DEV_Auth(IntPtr hDev, long index, string userName, string password);

        [DllImport("EACM.dll")]
        public static extern long FIRS_DEV_Auth(IntPtr hDev, string szUserName, string szPassword, DeletGetAutData getAuthData, string pUserData);

        [DllImport("EACM.dll")]
        public static extern int FIRS_DEV_GetTime(IntPtr hDev, long lIndex);

        [DllImport("EACM.dll")]
        public static extern int FIRS_DEV_GetTime(IntPtr hDev, ref TIME_AREA time);

        [DllImport("EACM.dll")]
        public static extern long FIRS_DEV_PutUser(IntPtr hDev, long lIndex, USER_INFO userInfo, bool delete);

        [DllImport("EACM.dll")]
        public static extern long FIRS_DEV_PutFeature(IntPtr hDev, long lIndex, long userId, FEATURE_INFO featureInfo);

        [DllImport("EACM.dll")]
        public static extern int FIRS_DEV_GetLastError(IntPtr hDev, string szErrorCode, string szErrorDesc);


        public IntPtr DEV_Init(long lDevType, DeletGetData mEventVerifyUser, string userData)
        {
            return EACM.FIRS_DEV_Init(lDevType, mEventVerifyUser, userData);
        }
        public void DEV_UnInit(IntPtr hDev)
        {
            EACM.FIRS_DEV_UnInit(hDev);
        }
        public int DEV_Connect(IntPtr hDev, string id, long port)
        {
            return EACM.FIRS_DEV_Connect(hDev, id, port);
        }
        public void DEV_DisConnect(IntPtr hDev)
        {
            EACM.FIRS_DEV_DisConnect(hDev);
        }
        public long DEV_Auth(IntPtr hDev, string szUserName, string szPassword, DeletGetAutData getAuthData, string pUserData)
        {
            return EACM.FIRS_DEV_Auth(hDev, szUserName, szPassword, getAuthData, pUserData);
        }
        public int DEV_FIRS_DEV_AuthAuth(IntPtr hDev, long index, string userName, string password)
        {
            return EACM.FIRS_DEV_Auth(hDev, index, userName, password);
        }
        public int DEV_GetTime(IntPtr hDev, long lIndex)
        {
            return EACM.FIRS_DEV_GetTime(hDev, lIndex);
        }
        public long DEV_PutUser(IntPtr hDev, long lIndex, USER_INFO userInfo, bool delete)
        {
            return EACM.FIRS_DEV_PutUser(hDev, lIndex, userInfo, delete);
        }
        public long DEV_PutFeature(IntPtr hDev, long lIndex, long userId, FEATURE_INFO featureInfo)
        {
            return EACM.FIRS_DEV_PutFeature(hDev, lIndex, userId, featureInfo);
        }
        public long DEV_GetLastError(IntPtr hDev, string szErrorCode, string szErrorDesc)
        {
            return EACM.FIRS_DEV_GetLastError(hDev, szErrorCode, szErrorDesc);
        }

        private long GetData(USER_RESULT pData, long lDataLen, long lIndex, long lType, string pUserData)
        {
            if (lType == 2)
            {
                TrapEventArgs arg = new TrapEventArgs();
                arg.lUserData = System.Convert.ToInt32(pUserData);
                arg.lVerifyResult = pData.result;
                arg.lUserID = (int)pData.userid;
                arg.lScore = pData.score;
                arg.lPhotoLen = (int)pData.photolen;
                arg.lPhotoType = pData.phototype;
                arg.strBase64PhotoData = pData.photo;


                if (EventTrapUser != null)
                {
                    EventTrapUser(null, arg);
                }
                return 0;

            }

            return -1;
        }

    }

    //	响应 - 终端时间信息
    public class TIME_AREA
    {
        string area;			//时区
        string datatime;		//时间
        string ntptype;			//NTP-TYPE
        string ntpserver;		//NTP-SERVER
    }

    public delegate long DeletGetAutData(TERMINAL_PROTOCOL_INFO result, long lDataLen, long lIndex, string pUserData);
    //	响应 - 终端版本信息
    [StructLayout(LayoutKind.Sequential)]
    public class TERMINAL_PROTOCOL_INFO
    {
        Int16 version;		//协议版本号
        Int16 type;			//终端类型
        char name;			//终端名称
        char typeno;		//型号
    }

    [StructLayout(LayoutKind.Sequential)]
    public class USER_INFO
    {
        public byte type;			//用户类型
        public long id;				//用户ID
        public string cardno;		//卡号
        public long power;			//权限
        public string name;			//名称
        public long deptname;		//部门ID
        public byte status;			//用户状态
        public byte regstatus;		//注册状态  1-y 0-n
        public byte phototype;		//照片类型
        public long photolen;		//照片长度
        public string photo;		//照片数据
    }

    [StructLayout(LayoutKind.Sequential)]
    public class FEATURE_INFO
    {
        public short version;			//算法版本
        public short method;			//算法
        public long featurelen;			//特征长度
        public string feature;			//特征数据第一位
    }

    [StructLayout(LayoutKind.Sequential)]
    public class USER_RESULT
    {
        public long userid;
        public byte result;
        public byte score;
        public byte phototype;
        public long photolen;
        public string photo;
    }

}
