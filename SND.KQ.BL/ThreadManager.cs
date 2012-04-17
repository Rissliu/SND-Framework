using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.BL.EntityData;
using SND.KQ.Machines.DevInterface;
using SND.KQ.Log;

namespace SND.KQ.BL
{
    /// <summary>
    /// 线程管理类，对系统线程运行状态进行管理
    /// </summary>
    public class ThreadManager
    {
        private ConfigEntity mconfig = null;
        // 检测设备是否处于连接状态
        private System.Timers.Timer DevTimer = null;
        // 处理解禁用户
        private System.Timers.Timer PassUserTimer = null;
        // 处理自动签退用户
        private System.Timers.Timer AutoLeaveTimer = null;
        // 同步内存数据到DB 
        private System.Timers.Timer CopyDataToDBTimer = null;
        // 同步DB数据到Dev 
        private System.Timers.Timer CopyDataToDevTimer = null;
        // 同步DB数据到内存（每天晚上执行）
        private System.Timers.Timer CopyDataToMemorTimer = null;
        private ThreadProcess mprocess = new ThreadProcess();

        public void Initialize(ConfigEntity config)
        {
            mconfig = config;

            foreach (string antNum in DataCollection.DevInfos.Keys)
            {
                DevInfo dev = DataCollection.DevInfos[antNum];
                
                if (!DataCollection.MachineList.ContainsKey(antNum))
                {

                    IMachine m =MachineFactory.GetMachine(DataCollection.Config.MachineType,dev.DevIp,dev.DevPort,dev.DevUserName,dev.DevPassWord,dev.AntNo);
                    m.EventVerifyUser += new EventHandler(mprocess.VerifyUserCallBack);
                    DataCollection.MachineList.Add(antNum,m);
                }
                
            }

            // 初始化线程处理类

            // 一分钟执行一次
            DevTimer = new System.Timers.Timer();
            DevTimer.Interval = 60000;
            DevTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.MonitorDevConnectStatus);

            // 5秒执行一次
            PassUserTimer = new System.Timers.Timer();
            PassUserTimer.Interval = 5000*20;
            PassUserTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.PassUser);

            // 1分执行一次
            AutoLeaveTimer = new System.Timers.Timer();
            AutoLeaveTimer.Interval = 1000*30;
            AutoLeaveTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.AutoLeaveUser);

            // 5分钟执行一次
            CopyDataToDBTimer = new System.Timers.Timer();
            CopyDataToDBTimer.Interval = 1000 * 60*5;
            CopyDataToDBTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.CopyDataToDB);

            // 10分钟执行一次
            CopyDataToMemorTimer = new System.Timers.Timer();
            CopyDataToMemorTimer.Interval = 1000 * 60*10;
            CopyDataToMemorTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.CopyDataToMemor);

            // 10分钟执行一次
            CopyDataToDevTimer = new System.Timers.Timer();
            CopyDataToDevTimer.Interval = 1000 * 60 * 10;
            CopyDataToDevTimer.Elapsed += new System.Timers.ElapsedEventHandler(mprocess.CopyDataToDev);

        }

        public void OnStart()
        {
            LogManager.LogSys("ThreadManager.OnStart 开始连接设备...");
            // 开始连接设备
            foreach (string key in DataCollection.MachineList.Keys)
            {
                int ret = DataCollection.MachineList[key].Connect();
                if (ret == 0)
                {
                    LogManager.LogSys(string.Format("第[{0}]通道监控【开启失败】 错误码：{1},稍后将重新启动",
                        key, DataCollection.MachineList[key].LastError));
                }
                else
                {
                    LogManager.LogSys(string.Format("第[{0}]通道已经开启，监听中...", key));
                }
            }

            LogManager.LogSys("ThreadManager.OnStart 启动线程...");
            // 启动线程
            DevTimer.Enabled = true;
            PassUserTimer.Enabled = true;
            AutoLeaveTimer.Enabled = true;
            CopyDataToDBTimer.Enabled = true;
            CopyDataToMemorTimer.Enabled = true;
            CopyDataToDevTimer.Enabled = true;
        }


        public void OnStop()
        {
            LogManager.LogSys("ThreadManager.OnStart 停止线程...");
            // 停止线程
            DevTimer.Enabled = false;
            PassUserTimer.Enabled = false;
            AutoLeaveTimer.Enabled = false;
            CopyDataToDBTimer.Enabled = false;
            CopyDataToMemorTimer.Enabled = false;
            CopyDataToDevTimer.Enabled = false;
            LogManager.LogSys("ThreadManager.OnStart 线程停止完成");

            LogManager.LogSys("ThreadManager.OnStart 开始关闭设备连接...");
            // 开始关闭设备连接
            foreach (string key in DataCollection.MachineList.Keys)
            {
                int ret = DataCollection.MachineList[key].DisConnect();
                if (ret == 0)
                {
                    LogManager.LogSys(string.Format("第[{0}]通道监控【关闭失败】 错误码：{1}",
                        key, DataCollection.MachineList[key].LastError));
                }
                else
                {
                    LogManager.LogSys(string.Format("第[{0}]通道已经关闭", key));
                }
            }
            LogManager.LogSys("ThreadManager.OnStart 关闭设备完成");
        }

    }
}
