using System;
using System.Collections.Generic;
using System.Text;


namespace SND.KQ.Machines.DevInterface
{
    /// <summary>
    /// 考勤机对象
    /// </summary>
    public class Machine : IMachine
    {
        private string mIp = string.Empty;
        private string mLastError = string.Empty;
        public string MIp
        {
            get { return mIp; }
            set { mIp = value; }
        }


        // 设备SDK对象
        private DevControl mDevObj = null;
        public string LastError
        {
            get { return mLastError; }
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
        // 用户注册
        public EventHandler EventEnrollUser;


        public Machine(string ip, string port, string userName, string password, string machineNum)
        {
            mIp = ip;
            DevControl ctr = new DevControl();
            ctr.InitialData(ip, port, userName, password, machineNum);
            mDevObj = ctr;
            mDevObj.UserEventTrap = new EventHandler(mDevObj_OnEventTrap);
            mDevObj.EInputUserFeature = new EventHandler(OnEventInputFeaturer);
        }
        /// <summary>
        /// 考勤机自动响应事件（终端有事件时，就触发该事件）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mDevObj_OnEventTrap(object sender, EventArgs e)
        {
            TrapEventArgs arg = e as TrapEventArgs;


            OnVerifyUser(arg);
        }

        /// <summary>
        /// 连接考勤机
        /// </summary>
        public int Connect()
        {
            // 考勤机初始化
            try
            {
                int handle = mDevObj.Connect();
                if (handle == 0)
                {
                    this.mLastError = mDevObj.GetLastError();
                }

            }
            catch (Exception ex)
            {
                this.mLastError = ex.Message;
            }

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
                ret = mDevObj.DisConnect();
                if (ret == 0)
                {
                    ret = 1;
                    mLastError = string.Empty;
                }
                else
                {
                    mLastError = mDevObj.GetLastError();
                }


            }
            return ret;
        }
        /// <summary>
        /// 获取设备时间，用来判断设备是否处于连接状态
        /// </summary>
        /// <returns></returns>
        public int GetDeviceTime()
        {
            return mDevObj.GetDeviceTime();
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
            // 考勤机初始化
            try
            {
                // 修改用户数据
                int ret = mDevObj.ModifyUser(arg.UserType, arg.UserId, arg.CardNo, 0, arg.UserName, 0, 0, 0, arg.lPhotoType, arg.lPhotoLen, arg.strBase64PhotoData);
                if (ret == 0)
                {
                    //修改特征数据
                    mDevObj.ModifyUserFeature(arg.UserId, arg.strBase64FeatureData);
                }
            }
            catch (Exception ex)
            {
                mLastError = ex.Message;
            }
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="arg"></param>
        public int ModifyUser(EnrollUserEventArgs arg)
        {
            // 修改用户数据
            return mDevObj.ModifyUser(arg.UserType, arg.UserId, arg.CardNo, 0, arg.UserName, arg.DeptId, 0, 0, arg.lPhotoType, arg.lPhotoLen, arg.strBase64PhotoData);
        }
        /// <summary>
        /// 用户录入模板
        /// </summary>
        /// <param name="lDevHandle"></param>
        /// <param name="lUserID"></param>
        /// <param name="lCardType"></param>
        /// <returns></returns>
        public int InputFeature(int lUserID, int lCardType)
        {
            return mDevObj.InputUserFeature(lUserID, lCardType);
        }
        /// <summary>
        /// 录入模板回调
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public void OnEventInputFeaturer(object obj, EventArgs e)
        {
            if (OnEInputFeaturer != null)
            {
                OnEInputFeaturer(obj, e);
            }
        }
    }

}
