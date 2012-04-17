using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace SND.KQ.BL
{
    public class SysConfig
    {
        /// <summary>
        /// 从配置文件中读取数据
        /// </summary>
        /// <returns></returns>
        public static ConfigEntity LoadConfig()
        {
            ConfigEntity config = new ConfigEntity();

            config.DBName = ConfigurationSettings.AppSettings["DBname"];
            config.DBServer = ConfigurationSettings.AppSettings["DBServer"];
            config.DBUserName= ConfigurationSettings.AppSettings["DBUserName"];
            config.DBPassword = ConfigurationSettings.AppSettings["DBPassword"];
            config.SysLogPath = ConfigurationSettings.AppSettings["SysLogPath"];
            config.InfoLogPath = ConfigurationSettings.AppSettings["InfoLogPath"];
            config.ReportConnectionString = ConfigurationSettings.AppSettings["ReportConnectionString"];
            config.CheckInWell = string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CheckInWell"]) ? 0 : System.Convert.ToInt32(ConfigurationSettings.AppSettings["CheckInWell"]);

            config.CopyDataNow = string.IsNullOrEmpty(ConfigurationSettings.AppSettings["CopyDataNow"]) ? 0 : System.Convert.ToInt32(ConfigurationSettings.AppSettings["CopyDataNow"]);
            config.MachineType = ConfigurationSettings.AppSettings["MachineType"];

            return config;
        }
    }
    /// <summary>
    /// 此类存放从配置文件中读出的信息
    /// </summary>
    public class ConfigEntity
    {
        // 数据库名称
        private string mdbname = string.Empty;

        public string DBName
        {
            get { return mdbname; }
            set { mdbname = value; }
        }
        // 数据库服务器
        private string mdbserver = string.Empty;

        public string DBServer
        {
            get { return mdbserver; }
            set { mdbserver = value; }
        }
        //数据库用户名
        private string mdbuserId = string.Empty;

        public string DBUserName
        {
            get { return mdbuserId; }
            set { mdbuserId = value; }
        }

        //数据库密码
        private string mdbpassword = string.Empty;

        public string DBPassword
        {
            get { return mdbpassword; }
            set { mdbpassword = value; }
        }

        private string mSysLogPath = string.Empty;

        // 系统日志路径
        public string SysLogPath
        {
            get { return mSysLogPath; }
            set { mSysLogPath = value; }
        }
        private string mInfoLogPath = string.Empty;

        // 考勤信息日志路径
        public string InfoLogPath
        {
            get { return mInfoLogPath; }
            set { mInfoLogPath = value; }
        }
        /// <summary>
        /// 报表数据库连接字符串
        /// </summary>
        private string mReportConnectionString = string.Empty;
        public string ReportConnectionString
        {
            get { return mReportConnectionString; }
            set { mReportConnectionString = value; }
        }

        public string FRASConnectionString
        {
            get {

                string connStr = "Server={0};Initial Catalog={1};Uid={2};Pwd={3}";
                return string.Format(connStr, DBServer, DBName, DBUserName, DBPassword);
            }
        }

        //是否验证井下定位(1-验证；0-不验证)
        private int checkInWell;

        public int CheckInWell
        {
            get { return checkInWell; }
            set { checkInWell = value; }
        }

        // 立马同步数据到设备（1-立马同步;0-固定时间同步)
        private int copyDataNow;

        public int CopyDataNow
        {
            get { return copyDataNow; }
            set { copyDataNow = value; }
        }

        // 设备类别(Machine,EACM....)
        private string machineType;

        public string MachineType
        {
            get { return machineType; }
            set { machineType = value; }
        }
    }
}
