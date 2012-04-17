using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SND.KQ.Machines.DevInterface
{
    public partial class DevControl : Form
    {
        private string mIp = string.Empty;
        private string mLastError = string.Empty;
        private string mPort = string.Empty;
        private string mUserName = string.Empty;
        private string mPassword = string.Empty;
        // 通道编号
        private string mMachineNum = string.Empty;
        private int mDevHandle = 0;

        public EventHandler UserEventTrap;
        public EventHandler EInputUserFeature;
        public DevControl()
        {
            InitializeComponent();
        }

        public void InitialData(string ip, string port, string userName, string password, string machineNum)
        {
            mIp = ip;
            mPort = port;
            mUserName = userName;
            mPassword = password;
            mMachineNum = machineNum;
        }
        private void axFirsFaceSdk1_OnEventTrap(object sender, AxFirsFaceSdkLib._DFirsFaceSdkEvents_OnEventTrapEvent e)
        {
            TrapEventArgs arg =new TrapEventArgs();
            arg.lDevHandle = e.lDevHandle;
            arg.lExtendParam = e.lExtendParam;
            //操作标识，OP_SUCC=0成功，OP_FAIL=1失败
            arg.lOpCode = e.lOpCode;
            arg.lPhotoLen = e.lPhotoLen;
            arg.lPhotoType = e.lPhotoType;
            arg.lScore = e.lScore;
            arg.lUserData = e.lUserData;
            arg.lUserID = e.lUserID;
            // 识别结果，1通过，0失败
            arg.lVerifyResult = e.lVerifyResult;
            arg.strBase64PhotoData = e.strBase64PhotoData;

            if (UserEventTrap != null)
            {
                UserEventTrap(null, arg);
            }

        }

        public int Connect()
        {
            axFirsFaceSdk1.FirsSdkInit();

            int port = string.IsNullOrEmpty(mPort) ? 33302 : System.Convert.ToInt32(mPort);
             mDevHandle = axFirsFaceSdk1.Connect(mIp, port, mUserName, mPassword, 2, System.Convert.ToInt32(mMachineNum));
             return mDevHandle;
        }
        public int DisConnect()
        {
             axFirsFaceSdk1.FirsSdkUnit();
             return axFirsFaceSdk1.Disconnect(mDevHandle);
        }

        public string GetLastError()
        {
            return axFirsFaceSdk1.SdkGetLastError().ToString("x");
        }

        public int GetDeviceTime()
        {
            return axFirsFaceSdk1.GetDeviceTime(mDevHandle);
        }

        public int ModifyUser(int lUserType, int lUserID, string strCardNo, int lPower, string strUserName, int lDeptID, int lUserStatus, int lRegStatus, int lPhotoType, int lPhotoLen, string strBase64PhotoData)
        {
            return axFirsFaceSdk1.ModifyUser(mDevHandle,0,lUserID,strCardNo,lPower,strUserName,lDeptID,lUserStatus,lRegStatus,lPhotoType,lPhotoLen,strBase64PhotoData);
        }
        public int ModifyUserFeature(int lUserID,string strBase64Featrue)
        {
            return axFirsFaceSdk1.ModifyUserFeature(mDevHandle, lUserID, strBase64Featrue);
        }

        public int InputUserFeature(int lUserID, int lCardType)
        {
            return axFirsFaceSdk1.EnrollUser(mDevHandle, lUserID, 0);
        }
        private void axFirsFaceSdk1_OnEventEnrollUser(object sender, AxFirsFaceSdkLib._DFirsFaceSdkEvents_OnEventEnrollUserEvent e)
        {
            EInputUserFeatuer arg = new EInputUserFeatuer();
            arg.lDevHandle = e.lDevHandle;
            arg.lExtendParam = e.lExtendParam;
            arg.lFeatureLen = e.lFeatureLen;
            arg.lOpCode = e.lOpCode;
            arg.lPhotoLen = e.lPhotoLen;
            arg.lPhotoType = e.lPhotoType;
            arg.lUserData = e.lUserData;
            arg.lUserID = e.lUserID;
            arg.strBase64FeatureData = e.strBase64FeatureData;
            arg.strBase64PhotoData = e.strBase64PhotoData;
            arg.strCardNo = e.strCardNo;

            if (EInputUserFeature != null)
            {
                EInputUserFeature(null, arg);
            }
        }
    }
}
