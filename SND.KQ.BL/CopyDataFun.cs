using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.BL.EntityData;
using SND.KQ.Machines.DevInterface;
using System.Data;
using SND.KQ.DAL.FRAS;
using SND.KQ.Log;

namespace SND.KQ.BL
{
    public class CopyDataFun
    {
        public static void CopyDevData()
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                object lockObj = new object();
                // 获取设备信息
                System.Data.DataTable dt = DAccess.GetDevInfo();
                if (dt != null)
                {
                    foreach (System.Data.DataRow row in dt.Rows)
                    {
                        if (!DataCollection.DevInfos.ContainsKey(row["AntNo"].ToString()))
                        {
                            DevInfo devinfo = new DevInfo();
                            devinfo.AntNo = row["AntNo"] is DBNull ? "0" : row["AntNo"].ToString().Trim();
                            devinfo.DevIp = row["devIp"].ToString().Trim();
                            devinfo.DevPort = row["devPort"].ToString().Trim();
                            devinfo.DevType = row["devType"].ToString().Trim();
                            devinfo.DevUserName = row["devUserName"].ToString().Trim();
                            devinfo.DevPassWord = row["devPassWord"].ToString().Trim();
                            devinfo.AccessFlag = row["accessFlag"] is DBNull ? "0" : row["accessFlag"].ToString().Trim();
                            devinfo.Flag = row["flag"] is DBNull ? "0" : row["flag"].ToString().Trim();
                            lock (lockObj)
                            {
                                if (!DataCollection.DevInfos.ContainsKey(row["AntNo"].ToString()))
                                {
                                    DataCollection.DevInfos.Add(devinfo.AntNo, devinfo);
                                }

                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==CopyDevData==" + ex.Message);
            }

        }
        public static void CopyUserData()
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                object lockObj = new object();
                // 获取用户信息
                System.Data.DataTable dtUser = DAccess.GetUserInfo();
                if (dtUser != null)
                {
                    foreach (System.Data.DataRow row in dtUser.Rows)
                    {
                        if (!DataCollection.DevInfos.ContainsKey(row["userId"].ToString().Trim()))
                        {
                            UserInfo userInfo = new UserInfo();
                            userInfo.UserId = row["userId"].ToString().Trim();
                            userInfo.UserName = row["userName"].ToString();
                            userInfo.DeptId = row["deptId"].ToString();
                            userInfo.FeatureId = row["featureId"].ToString();
                            userInfo.RankId = row["rankId"].ToString();
                            userInfo.SenderId = row["senderId"] == null ? string.Empty : row["senderId"].ToString();
                            userInfo.CopyType = row["copyType"] is DBNull ? 0 : System.Convert.ToInt32(row["copyType"].ToString());
                            userInfo.CardNo = row["cardNo"] == null ? string.Empty : row["cardNo"].ToString();
                            userInfo.FeatureId = row["featureId"].ToString();
                            userInfo.FeaPath = row["featurePath"].ToString();
                            userInfo.PhtPath = row["photoPath"].ToString();

                            lock (lockObj)
                            {
                                if (!DataCollection.UserInfos.ContainsKey(row["userId"].ToString().Trim()))
                                {
                                    DataCollection.UserInfos.Add(userInfo.UserId, userInfo);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==CopyUserData==" + ex.Message);
            }
        }

        /// <summary>
        /// 获取班次信息
        /// </summary>
        public static void CopyRostingData()
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                object lockObj = new object();

                System.Data.DataTable dtRosting = DAccess.GetAllRostInfo();
                if (dtRosting != null)
                {
                    foreach (System.Data.DataRow row in dtRosting.Rows)
                    {
                        if (!DataCollection.RostingList.ContainsKey(System.Convert.ToInt32(row["ID"])))
                        {
                            RostingData rosteData = new RostingData();
                            rosteData.ID = row["ID"].ToString();
                            rosteData.bcName = row["bcName"].ToString();
                            rosteData.startTime = row["startTime"].ToString();
                            rosteData.endTime = row["endTime"].ToString();
                            rosteData.earlyRange = row["earlyRange"] == null ? string.Empty : row["earlyRange"].ToString();
                            rosteData.lateRange = row["lateRange"] == null ? string.Empty : row["lateRange"].ToString();
                            rosteData.realStartTime = row["realStartTime"] == null ? string.Empty : row["realStartTime"].ToString();
                            rosteData.realStartTime = row["realEndTime"] == null ? string.Empty : row["realEndTime"].ToString();
                            rosteData.realStartTime = row["flag"] == null ? string.Empty : row["flag"].ToString();
                            rosteData.realStartTime = row["multType"] == null ? string.Empty : row["multType"].ToString();
                            rosteData.realStartTime = row["nextID"] == null ? string.Empty : row["nextID"].ToString();
                            rosteData.realStartTime = row["mulripleDur"] == null ? string.Empty : row["mulripleDur"].ToString();
                            rosteData.realStartTime = row["nightWork"] == null ? string.Empty : row["nightWork"].ToString();
                            rosteData.realStartTime = row["bak1"] == null ? string.Empty : row["bak1"].ToString();
                            rosteData.realStartTime = row["bak2"] == null ? string.Empty : row["bak2"].ToString();
                            rosteData.realStartTime = row["bak3"] == null ? string.Empty : row["bak3"].ToString();
                            rosteData.realStartTime = row["bak4"] == null ? string.Empty : row["bak4"].ToString();


                            lock (lockObj)
                            {
                                if (!DataCollection.RostingList.ContainsKey(System.Convert.ToInt32(row["ID"])))
                                {
                                    DataCollection.RostingList.Add(System.Convert.ToInt32(rosteData.ID), rosteData);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==CopyRostingData==" + ex.Message);
            }
        }


        /// <summary>
        /// 同步数据从DB到设备
        /// </summary>
        public static void CopyDataToDev()
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                DataTable dt = DAccess.GetCopyData();
                List<CopyData> list = new List<CopyData>();
                if (dt != null && dt.Rows.Count > 0)
                {

                    foreach (DataRow row in dt.Rows)
                    {
                        int dwFlag = row["flag"] is DBNull ? 0 : System.Convert.ToInt32(row["flag"]);
                        int dwCopyType = System.Convert.ToInt32(row["copyType"]);
                        int dwAntNum = 0;
                        dwAntNum = GetDevNum(dwFlag, dwCopyType);
                        CopyData cdata = new CopyData();
                        cdata.StrDevNum = dwAntNum.ToString();
                        cdata.StrUserId = row["userid"].ToString();
                        cdata.StrUserName = row["username"].ToString();
                        cdata.StrFeaturePath = row["featurePath"].ToString();

                        cdata.StrPicturePath = row["photoPath"].ToString();
                        cdata.StrCardId = row["cardId"].ToString();
                        cdata.StrCopyType = row["copyType"].ToString();
                        cdata.StrFlag = dwFlag.ToString();
                        list.Add(cdata);

                    }
                }

                // 开始同步数据
                foreach (CopyData data in list)
                {

                    if (DataCollection.MachineList.ContainsKey(data.StrDevNum))
                    {
                        EnrollUserEventArgs arg = new EnrollUserEventArgs();
                        arg.UserId = System.Convert.ToInt32(data.StrUserId);
                        arg.UserName = data.StrUserName;
                        arg.strCardNo = string.IsNullOrEmpty(data.StrCardId) ? "0000000000" : data.StrCardId;
                        arg.lPhotoType = 1;

                        arg.strBase64FeatureData = ComFunc.FileToBase64String(ref arg.lFeatureLen, data.StrFeaturePath);
                        arg.strBase64PhotoData = ComFunc.FileToBase64String(ref arg.lPhotoLen, data.StrPicturePath);

                        IMachine m = DataCollection.MachineList[data.StrDevNum];
                        m.EnrollUser(arg);

                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==CopyDataToDev==" + ex.Message);
            }

        }

        private static int GetDevNum(int dwFlag, int dwCopyType)
        {
            // 同步设备号
            int dwAntNum = 0;
            switch (dwCopyType)
            {
                case 1:
                    dwAntNum = dwFlag + 1;
                    break;
                case 2:
                    dwAntNum = 5;
                    break;
                case 3:
                    //同步类型3同同步类型4，联建楼考勤，井口1对1下井
                    dwAntNum = dwFlag + 1;
                    if (dwFlag == 3)
                    {
                        dwAntNum = 5;
                    }

                    break;
                case 4:
                    //队长，副队长，技术员在联建楼考勤，井口1对1下井
                    dwAntNum = dwFlag + 1;
                    if (dwFlag == 3)
                    {
                        dwAntNum = 5;
                    }

                    break;
                case 5:
                    dwAntNum = 6;
                    break;
                case 6:
                    if (dwFlag == 0)
                    {
                        dwAntNum = 4;
                    }
                    else
                    {
                        dwAntNum = 6;
                    }
                    break;
                case 7:
                    dwAntNum = 7;
                    break;
                //新增井口出口刷卡通道，则同步需考虑，若为同步类型8，则同步井口入口1，2和出口刷卡8
                case 8:
                    dwAntNum = dwFlag + 1;
                    if (dwFlag == 2)
                    {
                        dwAntNum = 8;
                    }
                    break;
                //新增井口出口刷卡通道，若为联建楼的出井刷卡，则同步至1，2，5，8
                case 9:
                    dwAntNum = dwFlag + 1;
                    if (dwFlag == 2)
                    {
                        dwAntNum = 5;
                    }
                    else if (dwFlag == 5)
                    {
                        dwAntNum = 8;
                    }
                    break;
                default:
                    break;
            }
            return dwAntNum;
        }
    }
}
