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

                KQInfo DAccess = new KQInfo(Common.GetDataConnection(Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));


                // 获取设备信息
                CopyDataFun.CopyDevData();

                // 获取用户信息
                CopyDataFun.CopyUserData();

                // 获取班次信息
                CopyDataFun.CopyRostingData();


                //    System.Data.DataTable dtRosting = DAccess.GetAllRostInfo();
                //    if (dtRosting != null)
                //    {
                //        foreach (System.Data.DataRow row in dtRosting.Rows)
                //        {
                //            RostingData rosteData = new RostingData();
                //            rosteData.ID = row["ID"].ToString();
                //            rosteData.bcName = row["bcName"].ToString();
                //            rosteData.startTime = row["startTime"].ToString();
                //            rosteData.endTime = row["endTime"].ToString();
                //            rosteData.earlyRange = row["earlyRange"] == null ? string.Empty : row["earlyRange"].ToString();
                //            rosteData.lateRange = row["lateRange"] == null ? string.Empty : row["lateRange"].ToString();
                //            rosteData.realStartTime = row["realStartTime"] == null ? string.Empty : row["realStartTime"].ToString();
                //            rosteData.realStartTime = row["realEndTime"] == null ? string.Empty : row["realEndTime"].ToString();
                //            rosteData.realStartTime = row["flag"] == null ? string.Empty : row["flag"].ToString();
                //            rosteData.realStartTime = row["multType"] == null ? string.Empty : row["multType"].ToString();
                //            rosteData.realStartTime = row["nextID"] == null ? string.Empty : row["nextID"].ToString();
                //            rosteData.realStartTime = row["mulripleDur"] == null ? string.Empty : row["mulripleDur"].ToString();
                //            rosteData.realStartTime = row["nightWork"] == null ? string.Empty : row["nightWork"].ToString();
                //            rosteData.realStartTime = row["bak1"] == null ? string.Empty : row["bak1"].ToString();
                //            rosteData.realStartTime = row["bak2"] == null ? string.Empty : row["bak2"].ToString();
                //            rosteData.realStartTime = row["bak3"] == null ? string.Empty : row["bak3"].ToString();
                //            rosteData.realStartTime = row["bak4"] == null ? string.Empty : row["bak4"].ToString();
                //            RostingList.Add(System.Convert.ToInt32(rosteData.ID), rosteData);
                //        }
                //    }
            }
            catch (Exception ex)
            {
                LogManager.LogSys(ex.Message);
            }
        }
    }
}
