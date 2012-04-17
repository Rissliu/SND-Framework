using System;
using System.Collections.Generic;
using System.Text;


namespace SND.KQ.Machines.DevInterface
{
    /// <summary>
    /// 考勤机对象
    /// </summary>
    public class EACMMachine : IMachine
    {
        private string mIp = string.Empty;
        private string mLastError = string.Empty;

        public string MIp
        {
            get { return mIp; }
            set { mIp = value; }
        }
        private string mPort = string.Empty;
        private string mUserName = string.Empty;
        private string mPassword = string.Empty;
        // 通道编号
        private string mMachineNum = string.Empty;

        // 设备SDK对象
        private EACM mDevObj = null;
        private IntPtr mDevHandle = IntPtr.Zero;

        public string LastError
        {
            get { return mLastError; }
            set { this.mLastError = value; }
        }

        // 用户识别
        private EventHandler mEventVerifyUser;
        public EventHandler EventVerifyUser
        {
            get { return mEventVerifyUser; }
            set { mEventVerifyUser = value; }
        }

        /// <summary>
        /// 录入模板回调
        /// </summary>
        /// <param name="arg"></param>
        private EventHandler mOnEInputFeaturer;
        public EventHandler OnEInputFeaturer
        {
            get { return mOnEInputFeaturer; }
            set { mOnEInputFeaturer = value; }
        }

        public EACMMachine(string ip, string port, string userName, string password, string machineNum)
        {
            mIp = ip;
            mPort = port;
            mUserName = userName;
            mPassword = password;
            mMachineNum = machineNum;
            EACM ctr = new EACM();
            mDevObj = ctr;
            ctr.EventTrapUser = new EventHandler(OnEventTrap);

        }
        private void OnEventTrap(object sender, EventArgs e)
        {
            OnVerifyUser((TrapEventArgs)e);
        }
        ///// <summary>
        ///// 考勤机自动响应事件（终端有事件时，就触发该事件）
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void mDevObj_OnEventTrap(object sender, _DFirsFaceSdkEvents_OnEventTrapEvent e)
        //{
        //    TrapEventArgs arg = new TrapEventArgs();

        //    arg.lDevHandle=e.lDevHandle;
        //    arg.lExtendParam=e.lExtendParam;
        //    //操作标识，OP_SUCC=0成功，OP_FAIL=1失败
        //    arg.lOpCode=e.lOpCode;
        //    arg.lPhotoLen=e.lPhotoLen;
        //    arg.lPhotoType=e.lPhotoType;
        //    arg.lScore=e.lScore;
        //    arg.lUserData=e.lUserData;
        //    arg.lUserID=e.lUserID;
        //    // 识别结果，1通过，0失败
        //    arg.lVerifyResult=e.lVerifyResult;
        //    arg.strBase64PhotoData=e.strBase64PhotoData;
        //    OnVerifyUser(arg);
        //}



        /// <summary>
        /// 连接考勤机
        /// </summary>
        public int Connect()
        {
            // 考勤机初始化
            try
            {
                mDevHandle = mDevObj.DEV_Init(0, mDevObj.Trapdata, mMachineNum);

                int port = string.IsNullOrEmpty(mPort) ? 33302 : System.Convert.ToInt32(mPort);
                int ret = mDevObj.DEV_Connect(mDevHandle, mIp, port);

                if (ret < 0)
                {
                    LastError = "连接失败";
                    return -1;
                }
                DeletGetAutData auth = new DeletGetAutData(GetAuthData);

                long a = mDevObj.DEV_Auth(mDevHandle, mUserName, mPassword, auth, mMachineNum);

                if (a < 0)
                {
                    LastError = "授权失败";
                    return -1;
                }

            }
            catch (Exception ex)
            {
                mDevHandle = IntPtr.Zero;
                LastError = ex.Message;
            }

            return 1;
        }



        public long GetAuthData(TERMINAL_PROTOCOL_INFO result, long lDataLen, long lIndex, string pUserData)
        {
            return 0;
        }
        /// <summary>
        /// 断开考勤机
        /// </summary>
        /// <returns></returns>
        public int DisConnect()
        {
            int ret = 1;
            if (mDevObj != null)
            {
                mDevObj.DEV_UnInit(mDevHandle);
                mDevObj.DEV_DisConnect(mDevHandle);

            }
            return ret;
        }
        /// <summary>
        /// 获取设备时间，用来判断设备是否处于连接状态
        /// </summary>
        /// <returns></returns>
        public int GetDeviceTime()
        {
            return mDevObj.DEV_GetTime(mDevHandle, 101);
        }

        /// <summary>
        /// 用户识别
        /// </summary>
        /// <param name="arg"></param>
        private void OnVerifyUser(TrapEventArgs arg)
        {
            if (EventVerifyUser != null)
            {
                EventVerifyUser(this, arg);
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="arg"></param>
        public void EnrollUser(EnrollUserEventArgs arg)
        {
            USER_INFO userIfo = new USER_INFO();
            userIfo.cardno = arg.CardNo;
            userIfo.deptname = arg.DeptId;
            userIfo.id = arg.UserId;
            userIfo.name = arg.UserName;
            userIfo.photo = arg.strBase64PhotoData;
            userIfo.photolen = arg.lPhotoLen;
            userIfo.phototype = (byte)arg.lPhotoType;
            userIfo.power = 0;
            userIfo.regstatus = 0;
            userIfo.status = 0;
            userIfo.type = (byte)arg.UserType;

            // 修改用户数据
            long ret = mDevObj.DEV_PutUser(mDevHandle, 105, userIfo, false);
            if (ret == 0)
            {
                FEATURE_INFO featuerInfo = new FEATURE_INFO();
                featuerInfo.feature = arg.strBase64FeatureData;
                featuerInfo.featurelen = arg.lFeatureLen;
                featuerInfo.method = 0;
                featuerInfo.version = 0;
                //修改特征数据
                long result = mDevObj.DEV_PutFeature(mDevHandle, 112, arg.UserId, featuerInfo);
            }
        }

        public int InputFeature(int lUserID, int lCardType)
        {
            return 0;
        }
        public int ModifyUser(EnrollUserEventArgs arg)
        {
            return 0;
        }

    }

}
