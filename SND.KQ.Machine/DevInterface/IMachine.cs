using System;
using System.Collections.Generic;
using System.Text;

namespace SND.KQ.Machines.DevInterface
{
    /// <summary>
    /// 考勤机借口
    /// </summary>
    public interface  IMachine
    {
        // 用户识别代理
        EventHandler EventVerifyUser { get; set; }
        string MIp { get; set; }
        string LastError { get; }
        /// <summary>
        /// 连接考勤机
        /// </summary>
        int Connect();

        /// <summary>
        /// 断开考勤机
        /// </summary>
        /// <returns></returns>
        int DisConnect();

        /// <summary>
        /// 获取设备时间，用来判断设备是否处于连接状态
        /// </summary>
        /// <returns></returns>
        int GetDeviceTime();

        /// <summary>
        /// 录入模板
        /// </summary>
        /// <returns></returns>
        int InputFeature(int lUserID, int lCardType);

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="arg"></param>
        void EnrollUser(EnrollUserEventArgs arg);

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="arg"></param>
        int ModifyUser(EnrollUserEventArgs arg);

        /// <summary>
        /// 录入模板回调
        /// </summary>
        /// <param name="arg"></param>
        EventHandler OnEInputFeaturer { get; set; }


    }

    public class TrapEventArgs : EventArgs
    {
        public int lDevHandle;
        public int lExtendParam;
        public int lOpCode;
        public int lPhotoLen;
        public int lPhotoType;
        public int lScore;
        public int lUserData;
        public int lUserID;
        public int lVerifyResult;
        public string strBase64PhotoData;

    }

    public class SetUserEventArgs : EventArgs
    {
        public int lDevHandle;
        public int lExtendParam;
        public int lOpCode;
        public int lUserData;
        public int lUserID;

    }

    public class EnrollUserEventArgs : EventArgs
    {
        public int UserId;
        public string UserName;
        public string CardNo;
        public int DeptId;
        public int UserStatus;
        public int UserType;
        public int lFeatureLen;
        public int lPhotoLen;
        public int lPhotoType;
        public string strBase64FeatureData;
        public string strBase64PhotoData;
        public string strCardNo;
    }

    public class EInputUserFeatuer : EventArgs
    {
        public int lDevHandle;
        public int lExtendParam;
        public int lFeatureLen;
        public int lOpCode;
        public int lPhotoLen;
        public int lPhotoType;
        public int lUserData;
        public int lUserID;
        public string strBase64FeatureData;
        public string strBase64PhotoData;
        public string strCardNo;
    }

}
