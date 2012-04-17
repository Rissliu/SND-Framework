using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.Machines.DevInterface;
using SND.KQ.Log;
using System.Data;
using SND.DA.DataAccessHelper;
using SND.KQ.DAL.FRAS;

namespace SND.KQ.BL
{
    /// <summary>
    /// 线程处理类，系统所有的线程处理类都定义在这里
    /// </summary>
    public class ThreadProcess
    {
        /// <summary>
        /// 检测设备连接状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void MonitorDevConnectStatus(object sender, System.Timers.ElapsedEventArgs e)
        {
            foreach (string antNum in DataCollection.MachineList.Keys)
            {
                IMachine machine = DataCollection.MachineList[antNum];
                if (machine.GetDeviceTime() != SysData.S_OK)
                {
                    LogManager.LogSys(string.Format("第[{0}]通道设备断开，IP={1} 将要进行重新连接", antNum, machine.MIp));
                    machine.DisConnect();
                    int ret = machine.Connect();
                    if (ret == 0)
                    {
                        LogManager.LogSys(string.Format("第[{0}]通道设备连接失败,IP={1} 错误码：{2},稍后将重新连接",
                            antNum, machine.MIp, machine.LastError));
                    }
                    else
                    {
                        LogManager.LogSys(string.Format("第[{0}]通道设备连接成功，IP={1} 监听中...", antNum, machine.MIp));
                    }
                }
            }
        }

        /// <summary>
        /// 处理解禁用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PassUser(object sender, System.Timers.ElapsedEventArgs e)
        {
            LogManager.LogSys("处理禁刷用户");
            if (DataCollection.ForbindList.Count == 0)
            {
                return;
            }
            List<string> list = new List<string>();
            foreach (string key in DataCollection.ForbindList.Keys)
            {
                DateTime date = DataCollection.ForbindList[key];
                DateTime now = DateTime.Now;
                TimeSpan span = now - date;
                if (span.TotalMinutes >=30)
                {
                    list.Add(key);
                }
            }
            object lockobj = new object();
            foreach (string key in list)
            {
                lock (lockobj)
                {
                    DataCollection.ForbindList.Remove(key);
                }
                
            }

            LogManager.LogSys("处理禁刷用户完成");
        }

        /// <summary>
        /// 处理自动签退
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void AutoLeaveUser(object sender, System.Timers.ElapsedEventArgs e)
        {
           
            LogManager.LogSys("处理自动签退...");
            List<string> list = new List<string>();
            string nowTime = string.Format("{0}:{1}", DateTime.Now.Hour, DateTime.Now.Minute);
            foreach (int key in DataCollection.RostingList.Keys)
            {
                string strId = key.ToString();

                if (DataCollection.RostingList[key].multType == 0)
                {
                    strId = (key - 1).ToString();
                }

                if (nowTime == DataCollection.RostingList[key].realEndTime)
                {
                    list.Add(strId);
                }
            }

            foreach (string id in list)
            {
                UpdateDuration(id);
            }

            UpdateDurationWithoutBC();

            LogManager.LogSys("自动签退处理结束...");


        }

        private void UpdateDuration(string strRostId)
        {
            try
            {

                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                DataTable dt = DAccess.GetWorkDurationByRosteId(strRostId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                List<string> list = new List<string>();
                foreach (DataRow row in dt.Rows)
                {
                    string copyType = row["copyType"].ToString();
                    string devNum = row["devNum"].ToString();
                    if (copyType == "3" || copyType == "4" && (devNum == "1" || devNum == "2"))
                    {
                        //此时为队长副队长技术员下井，则无班次限制，不在此处做自动签退
                        continue;
                    }

                    list.Add(row["ID"].ToString());

                }

                LogManager.LogSys(string.Format("==有[{0}]条记录需要自动签退==", list.Count));

                for (int i = 0; i < list.Count; i++)
                {
                    DAccess.UpdateWorkDuration(list[i]);
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==UpdateDurationWithoutBC== " + ex.Message);
            }

        }

        private void UpdateDurationWithoutBC()
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                DataTable dt = DAccess.GetUserWorkDuration();

                if (dt == null || dt.Rows.Count == 0)
                {
                    return;
                }
                List<string> list = new List<string>();
                DateTime now = DateTime.Now.AddHours(-1);
                foreach (DataRow row in dt.Rows)
                {
                    DateTime startTime = System.Convert.ToDateTime(row["bak2"]);
                    if (startTime <= now)
                    {
                        list.Add(row["ID"].ToString());
                    }
                }


                LogManager.LogSys(string.Format("==有[{0}]条记录需要自动签退==", list.Count));

                for (int i = 0; i < list.Count; i++)
                {
                   DAccess.UpdateWorkDuration(list[i]);
                }
            }
            catch(Exception ex)
            {
                LogManager.LogSys("==UpdateDurationWithoutBC== " + ex.Message);
            }
        }

        /// <summary>
        ///同步内存数据到DB
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyDataToDB(object sender, System.Timers.ElapsedEventArgs e)
        {
            DateTime date = DateTime.Now;
             //晚上8点，早上六点之间可以同步
            if (date.Hour > 20 || date.Hour < 6)
            {

            }
        }
        /// <summary>
        ///同步数据到设备
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyDataToDev(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (DataCollection.Config.CopyDataNow == 1)
                {
                    CopyDataFun.CopyDataToDev();
                }
                else
                {
                    DateTime date = DateTime.Now;
                    //晚上12点，早上六点之间可以同步
                    if (date.Hour >= 0 || date.Hour < 6)
                    {
                        CopyDataFun.CopyDataToDev();
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==CopyDataToDev同步数据到设备==" + ex.Message);
            }
        }
        /// <summary>
        ///同步DB数据到内存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CopyDataToMemor(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                if (DataCollection.Config.CopyDataNow == 1)
                {
                    CopyDataFun.CopyDevData();
                    CopyDataFun.CopyUserData();
                }
                else
                {
                    DateTime date = DateTime.Now;
                    //晚上8点，早上六点之间可以同步
                    if (date.Hour > 20 || date.Hour < 6)
                    {
                        CopyDataFun.CopyDevData();
                        CopyDataFun.CopyUserData();
                    }
                }
            }
            catch(Exception ex)
            {
                LogManager.LogSys("==CopyDataToMemor/同步DB数据到内存==" + ex.Message);
            }

        }

        /// <summary>
        /// 识别用户回调函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="arg"></param>
        public void VerifyUserCallBack(object obj, EventArgs eArg)
        {
            TrapEventArgs arg = (TrapEventArgs)eArg;
            try
            {
                VerifyProcess.VerifyUser(arg);
            }
            catch(Exception ex)
            {
                LogManager.LogSys("==LoadDBData识别用户==" + ex.Message);
            }

        }
        /// <summary>
        /// 设置用户信息回调函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eArg"></param>
        public void SetUserCallBack(object obj, EventArgs eArg)
        {
            SetUserEventArgs arg= (SetUserEventArgs)eArg;
        }

        /// <summary>
        /// 注册用户回调函数
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="eArg"></param>
        public void EnrollUserCallBack(object obj, EventArgs eArg)
        {
            EnrollUserEventArgs arg = (EnrollUserEventArgs)eArg;
        }

        private DataConnection GetConnection()
        {
            //
            // Run some commands
            //
            return SND.KQ.DAL.FRAS.Common.GetDataConnection(DataCollection.Config.FRASConnectionString,
                DataSourceType.SqlClient);
        }
    }
}
