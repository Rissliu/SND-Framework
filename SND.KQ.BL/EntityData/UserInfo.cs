using System;
using System.Collections.Generic;
using System.Text;

namespace SND.KQ.BL.EntityData
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo
    {

        string userId;//工号

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        string userName;//用户名

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        string deptId;//部门号

        public string DeptId
        {
            get { return deptId; }
            set { deptId = value; }
        }
        string deptName;//部门名称

        public string DeptName
        {
            get { return deptName; }
            set { deptName = value; }
        }
        string rankId;//职位号

        public string RankId
        {
            get { return rankId; }
            set { rankId = value; }
        }
        string rankName;//职位名称

        public string RankName
        {
            get { return rankName; }
            set { rankName = value; }
        }
        string senderId;//射频号

        public string SenderId
        {
            get { return senderId; }
            set { senderId = value; }
        }
        string cardNo; //卡号

        public string CardNo
        {
            get { return cardNo; }
            set { cardNo = value; }
        }
        private int copyType;
        public int CopyType
        {
            get { return copyType; }
            set { copyType = value; }
        }

        private int type;
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        string featureId;//模板编号

        public string FeatureId
        {
            get { return featureId; }
            set { featureId = value; }
        }
        string feaPath;//模板文件路径

        public string FeaPath
        {
            get { return feaPath; }
            set { feaPath = value; }
        }
        string phtPath;	//采集图像文件路径

        public string PhtPath
        {
            get { return phtPath; }
            set { phtPath = value; }
        }
    }
}
