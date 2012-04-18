using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.BL.EntityData;
using SND.KQ.Machines.DevInterface;
using SND.KQ.DAL.FRAS;
using SND.KQ.Log;

namespace SND.KQ.BL
{
    /// <summary>
    /// 该类定义一些数据集合，用来存放临时数据
    /// </summary>
    public class DataCollection
    {
        public static ConfigEntity Config = null;


        public static SND.KQ.DAL.Well.WellInfo DAccessWell = null;

        // 打卡计数器，统计一个人连续刷卡次数
        public static Dictionary<string, List<DateTime>> SwipeCardCount = new Dictionary<string, List<DateTime>>();

        // 用户信息集合
        public static Dictionary<string, UserInfo> UserInfos = new Dictionary<string, UserInfo>();

        // 设备信息集合
        public static Dictionary<string, DevInfo> DevInfos = new Dictionary<string, DevInfo>();

        // 设备集合
        public static Dictionary<string, IMachine> MachineList = new Dictionary<string, IMachine>();

        // 禁止刷卡列表
        public static Dictionary<string, DateTime> ForbindList = new Dictionary<string, DateTime>();

        // 签退列表
        //班次信息集合？？？----------sayid
        public static Dictionary<int, RostingData> RostingList = new Dictionary<int, RostingData>();

        // 从数据库加载数据放入内存，以后直接使用，提高效率
        public static void LoadDBData()
        {
            LogManager.LogSys("==LoadDBData 加载初始化数据==");

            try
            {
                SND.KQ.DAL.Well.Common.Initialize(Config.ReportConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient);


                Common.Initialize(Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient);


                // 获取设备信息
                CopyDataFun.CopyDevData();

                // 获取用户信息
                CopyDataFun.CopyUserData();

                // 获取班次信息
                CopyDataFun.CopyRostingData();

            }
            catch (Exception ex)
            {
                LogManager.LogSys(ex.Message);
            }
        }
    }
}
