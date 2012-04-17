using System;
using System.Collections.Generic;
using System.Text;

namespace SND.KQ.BL.EntityData
{
    /// <summary>
    /// 设备信息
    /// </summary>
    public class DevInfo
    {
        private int mId;

        public int ID
        {
            get { return mId; }
            set { mId = value; }
        }
        private string mdevIp;

        public string DevIp
        {
            get { return mdevIp; }
            set { mdevIp = value; }
        }
        private string mdevPort;

        public string DevPort
        {
            get { return mdevPort; }
            set { mdevPort = value; }
        }
        private string mdevType;

        public string DevType
        {
            get { return mdevType; }
            set { mdevType = value; }
        }
        private string mdevUserName;

        public string DevUserName
        {
            get { return mdevUserName; }
            set { mdevUserName = value; }
        }
        private string mdevPassWord;

        public string DevPassWord
        {
            get { return mdevPassWord; }
            set { mdevPassWord = value; }
        }
        private string mantNo;

        public string AntNo
        {
            get { return mantNo; }
            set { mantNo = value; }
        }
        private string maccessFlag;

        public string AccessFlag
        {
            get { return maccessFlag; }
            set { maccessFlag = value; }
        }
        private string mflag;

        public string Flag
        {
            get { return mflag; }
            set { mflag = value; }
        }
    }
}
