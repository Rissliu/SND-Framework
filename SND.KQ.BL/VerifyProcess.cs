using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.Machines.DevInterface;
using SND.KQ.Log;
using SND.KQ.BL.EntityData;
using System.IO;
using System.Data;
using SND.KQ.DAL.FRAS;

namespace SND.KQ.BL
{
    /// <summary>
    /// 刷卡处理类
    /// </summary>
    public class VerifyProcess
    {
        public static void VerifyUser(TrapEventArgs arg)
        {

            // 操作成功
            if (arg.lOpCode == SysData.S_OK)
            {
                if (arg.lUserID == 0)
                {
                    LogManager.LogSys("用户Id返回值不存在");
                    return;
                }

                UserInfo uinfo = DataCollection.UserInfos[ ComFunc.GetUserIdString(arg.lUserID.ToString())];
                // 识别成功
                if (arg.lVerifyResult == 1)
                {

                    int userId = arg.lUserID;
                    //因为人脸识别设备的USERID，10以为被设定为管理员ID，因此在10以内的用户，前面加621，凑7位
                    //如userid为0008，则录入机器中的模板ID为6210008即可；读出时需要减去6210000；
                    if (userId > SysData.USERID_OFFSET_COUNT)
                    {
                        userId = userId - SysData.USERID_OFFSET_COUNT;
                    }

                    if (!DataCollection.UserInfos.ContainsKey(userId.ToString()))
                    {
                        LogManager.LogSys("用户 " + userId + " 在通道【" + arg.lUserData + "】查询失败，处理终止。");
                        return;
                    }


                    LogManager.LogSys("用户 " + uinfo.UserName + " 在通道【" + arg.lUserData + "】识别");

                    //控制同一类机器，同一人半小时内多次刷卡无效
                    if (DataCollection.ForbindList.ContainsKey(userId.ToString() + arg.lUserData.ToString()))
                    {
                        TimeSpan span = DateTime.Now.Subtract(DataCollection.ForbindList[userId.ToString() + arg.lUserData.ToString()]);
                        if (span.Minutes <= 30)
                        {
                            string msg = String.Format("用户【{0}】半小时内重复刷卡，不做考勤记录。", uinfo.UserName);
                            LogManager.LogSys(msg);
                            LogManager.LogInfo(msg);
                            return;

                        }
                    }
                    else
                    {
                        //禁刷集合中没有，则为第一次刷卡，加入集合中
                        DataCollection.ForbindList.Add(userId.ToString() + arg.lUserData.ToString(), DateTime.Now);
                        LogManager.LogSys("用户【" + uinfo.UserName + "】已经加入禁刷集合。");
                    }

                }

                LogManager.LogSys("处理识别结果信息...");
                string photoName = GetPhotoName(arg.lUserID.ToString());
                if (arg.lVerifyResult == 1)
                {
                    photoName = Path.Combine(GetSavPath(Dir_Type.SavePic, arg.lUserData), photoName);
                }
                else
                {
                    photoName = Path.Combine(GetSavPath(Dir_Type.ErrPic, arg.lUserData), photoName);
                }
                // 保存照片
                ComFunc.Base64StringToImage(arg.strBase64PhotoData, photoName);


                int inOroOut = GetInOrOut(DataCollection.DevInfos[arg.lUserData.ToString()], arg.lUserID);
                int rostId = 0;
                RostInfo userRostInfo = new  RostInfo();
                DevInfo dev = DataCollection.DevInfos[arg.lUserData.ToString()];
                //================================
                //参数传递有错误-------sayid
                //================================
                int state = GetState(arg.lUserID, inOroOut, Convert.ToInt32(dev.DevType), Convert.ToInt32(dev.AntNo), uinfo.CopyType, ref rostId, ref userRostInfo);

                ////进行工时统计,生活区只记录流水，不做工时统计
                if (arg.lVerifyResult == 1 && dev.DevType != "18")
                {
                    SetWorkDuration(dev, uinfo, inOroOut, state, rostId, userRostInfo);
                }


                //识别结果入库
                InserLog(uinfo.UserId, inOroOut, arg.lVerifyResult, state, arg.lUserData, photoName);


                //记录日志+
                SetCueInfo(uinfo.UserName, inOroOut, state, arg.lVerifyResult, arg.lUserData, arg.lUserID.ToString());


                // 如果刷卡失败，进行统计失败次数，并决定是否启动模板录入功能
                if (arg.lVerifyResult != 1)
                {
                    SwipeFaileHandle.Failed(uinfo, dev);
                }
            }
        }

        private static void InserLog(string strUserId, int dwInOrOut, int dwAccess, int dwState, int dwDevNum, string strPhotoPath)
        {
            string time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
            DAccess.AddAccessLog(strUserId, time, date, dwInOrOut, dwAccess, dwState, dwDevNum, strPhotoPath);


        }
        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="strUserName">用户名</param>
        /// <param name="dwInOrOut">签到/签退</param>
        /// <param name="dwState">班次状态</param>
        /// <param name="dwAntNum">通道</param>
        /// <param name="dwResult">成功/失败</param>
        /// <param name="strUserId">用户ID</param>
        private static void SetCueInfo(string strUserName, int dwInOrOut, int dwState, int dwResult, int dwAntNum, string strUserId)
        {
            string strInOutInfo = string.Empty;
            string strStateInfo = string.Empty;

            string strAntInfo;

            switch (dwInOrOut)
            {
                case (int)Dev_InOrOut.In:
                    strInOutInfo = "签到";
                    break;
                case (int)Dev_InOrOut.Out:
                    strInOutInfo = "签退";
                    break;
                default:
                    break;
            }

            switch (dwState)
            {
                case 0:
                    strStateInfo = "正常";
                    break;
                case 1:
                    strStateInfo = "迟到";
                    break;
                case 2:
                    strStateInfo = "早退";
                    break;
                case 3:
                    strStateInfo = "超班次时间";
                    break;
                case -1:
                    strStateInfo = "未指定班次";
                    break;
                case 4:
                    strStateInfo = "旷工";
                    break;
                case 101:
                    strStateInfo = "签退时间签到，视为签退";
                    break;
                default:
                    strStateInfo = "未知";
                    break;
            }

            //生活区不足5小时考勤
            if (dwInOrOut == 99)
            {
                strInOutInfo = "不足5小时签退";
                strStateInfo = "无效";
            }

            //井口1对多为1号通道，井口入2为2号通道，井口入1为3号通道，井口出为4号通道
            switch (dwAntNum)
            {
                case 1:
                    strAntInfo = "井口3号通道";
                    break;
                case 2:
                    strAntInfo = "井口2号通道";
                    break;
                case 3:
                    strAntInfo = "井口4号通道";
                    break;
                case 4:
                    strAntInfo = "井口1号通道";
                    break;
                case 5:
                    strAntInfo = "联建楼";
                    break;
                case 6:
                    strAntInfo = "办公楼";
                    break;
                case 7:
                    strAntInfo = "生活区";
                    break;
                //增加新的通道，即井口5号通道，用于出井刷卡（黑脸）
                case 8:
                    strAntInfo = "井口5号通道";
                    break;
                default:
                    strAntInfo = "未知";
                    break;
            }

            string msg;

            if (dwResult == 1)
            {
                //识别成功
                msg = string.Format("用户【{0}({1})】已在【{2}】{3}，状态为【{4}】", strUserName, strUserId, strAntInfo, strInOutInfo, strStateInfo);
            }
            else
            {
                msg = string.Format("用户【{0}({1})】在【{2}】{3}，识别失败！", strUserName, strUserId, strAntInfo, strInOutInfo);
            }

            //在日志里也写入
            LogManager.LogSys(msg);
            LogManager.LogInfo(msg);

        }
        private static void SetWorkDuration(DevInfo dev, UserInfo user, int inorout, int state, int rostId, RostInfo userRostInfo)
        {
            try
            {
                //若出入标志为99，则代表生活区不符合考勤时间，不做工时统计
                if (inorout == 99)
                {
                    LogManager.LogSys(string.Format("用户[{0}]生活区考勤间隔不足5小时，不做工时统计!", user.UserName));
                    return;
                }

                //签退时间签到的记录，不做工时统计
                if (state == 101)
                {
                    LogManager.LogSys(string.Format("用户[{0}]签退时间签到，视为签退，不做工时统计!", user.UserName));
                    return;
                }

                if (state == 3)
                {
                    //加入不做工时统计的日志
                    LogManager.LogSys(string.Format("用户[{0}]超班次时间，不做工时统计!", user.UserName));
                    return;
                }

                //无班次设置，不做工时统计
                if (state == -1)
                {
                    //加入不做工时统计的日志
                    LogManager.LogSys(string.Format("用户[{0}]未设置班次，不做工时统计!", user.UserName));

                    return;
                }
                bool hasDure = true;
                DataTable dt = null;

                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));

                //入口1,2，出口3，出口8计算上次入井都是找1,2
                if (dev.AntNo == "1" || dev.AntNo == "2" || dev.AntNo == "3" || dev.AntNo == "8")
                {
                    dt = DAccess.GetAccessLogByUserId(user.UserId);
                }
                else
                {
                    dt = DAccess.GetAccessLogByUserIdAndDevNum(user.UserId, dev.AntNo);
                }

                if (dt == null)
                {
                    return;
                }
                DateTime lastTime = DateTime.Now;
                if (dt.Rows.Count == 0)
                {
                    hasDure = false;
                }
                else
                {
                    lastTime = System.Convert.ToDateTime(dt.Rows[0]["datetime"]);
                }
                LogManager.LogSys(string.Format("用户[{0}]上次签到时间为[{1}]", user.UserName, lastTime.ToString()));

                DateTime now = DateTime.Now;
                TimeSpan timsSpan = now - lastTime;
                //若为签退，则查询之前的工时记录信息
                if (inorout == (int)Dev_InOrOut.Out)
                {
                    //因为入井时有多个通通，因此要特殊处理
                    DataTable dtDur = null;
                    if (dev.AntNo == "3" || dev.AntNo == "8")
                    {
                        dtDur = DAccess.GetWorkDurationByUserId(user.UserId);
                    }
                    else
                    {
                        dtDur = DAccess.GetWorkDurationByUserIdAndDevNum(user.UserId, dev.AntNo);
                    }

                    if (dtDur == null || dtDur.Rows.Count == 0)
                    {
                        LogManager.LogSys("未找到签到时的工时记录，无法做签退处理!");
                        return;
                    }

                    string durId = dtDur.Rows[0]["ID"].ToString();
                    //取得签到时的状态
                    string strLastState = dtDur.Rows[0]["bak1"].ToString();

                    //生活区签退时，更新工时记录
                    if (dev.AntNo == "7")
                    {
                        string strDuration = string.Format("{0}:{1}:{2}", timsSpan.Hours, timsSpan.Minutes, timsSpan.Seconds);

                        DAccess.UpdateWorkDurationRecord(strDuration, System.Convert.ToInt64(timsSpan.TotalSeconds), now.ToString("yyyy-MM-dd HH:mm:ss"), System.Convert.ToInt32(durId), 1);

                        LogManager.LogSys(string.Format("用户[{0}]生活区出勤时长[{1}]，更新工时记录！", user.UserName, timsSpan.TotalSeconds));
                        return;
                    }


                    //判断入井是否满足时间
                    bool isFullDur = true;

                    //走井口1对多的是领导，满足3小时；走井口1对1的都是工人，需要满足7小时下井；
                    //但对队长，副队长，技术员的角色单独判断，需要满足4小时下井
                    //井口1对多
                    if (dev.DevType == "4")
                    {
                        if (timsSpan.TotalMinutes < 60 * 3)
                        {
                            LogManager.LogSys(string.Format("用户[{0}]为办公楼人员，下井不足3小时，不做工时统计!", user.UserName));
                            isFullDur = false;
                        }

                    }
                    if (dev.DevType == "3") //井口1对1
                    {

                        if (user.CopyType == 4 || user.CopyType == 3)
                        {
                            if (timsSpan.TotalMinutes < 60 * 3)
                            {
                                LogManager.LogSys(string.Format("用户[{0}]为队长/副队长/技术员，下井不足3小时，不做工时统计!", user.UserName));
                                isFullDur = false;
                            }
                        }
                        else
                        {
                            //======================================
                            //正常工人为7个小时，不是3个小时---------------sayid
                            //=======================================
                            if (timsSpan.TotalMinutes < 60 * 7)
                            {
                                LogManager.LogSys(string.Format("用户[{0}]为下井工人，下井不足7小时，不做工时统计!", user.UserName));
                                isFullDur = false;
                            }
                        }

                    }

                    //若为井口出井，并且未早退，并且下井足时，则判断是否有井下定位记录，若无，则置工时记录为无效
                    int dwIfInWell = 1;
                    //配置文件中增加是否验证井下定位的参数(1-验证；0-不验证)
                    int ifCheckWell = DataCollection.Config.CheckInWell;
                    if (ifCheckWell == 1 && (dev.AntNo == "3" || dev.AntNo == "4" || dev.AntNo == "8") && state != 4 && isFullDur)
                    {
                        LogManager.LogSys("查询井下定位记录...");

                        SND.KQ.DAL.Well.WellInfo DAccessWell = new SND.KQ.DAL.Well.WellInfo(SND.KQ.DAL.Well.Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                        DataTable dtWell = DAccessWell.GetWellInfo(user.UserId, System.Convert.ToDateTime(strLastState), DateTime.Now);
                        if (dtWell == null || dt.Rows.Count == 0)
                        {
                            dwIfInWell = 0;
                        }
                        else
                        {
                            dwIfInWell = 1;
                        }
                        //加入井下定位无记录的界面提示
                        if (dwIfInWell < 1)
                        {

                            string msg = string.Format("工号[{0}]员工未查询到井下定位记录！", user.UserId);
                            LogManager.LogSys(msg);
                            LogManager.LogInfo(msg);
                        }

                    }

                    //提前出井算旷工，或下井不足时间
                    //对无井下定位记录的处理，也置为无效工时记录
                    if (state == 4 || !isFullDur || dwIfInWell < 1)
                    {

                        string strDuration = string.Format("{0}:{1}:{2}", timsSpan.Hours, timsSpan.Minutes, timsSpan.Seconds);

                        //==========================================
                        //不计入工时Duratio应该为-2-------------------sayid
                        //==========================================
                        DAccess.UpdateWorkDurationRecord(strDuration, -2, now.ToString("yyyy-MM-dd HH:mm:ss"), System.Convert.ToInt32(durId), 1);
                        LogManager.LogSys(string.Format("用户[{0}]提前签退旷工，下井不足时，或无井下定位记录，工时记录无效！", user.UserName));
                    }
                    else
                    {

                        //更新工时表记录
                        string strDuration = string.Format("{0}:{1}:{2}", timsSpan.Hours, timsSpan.Minutes, timsSpan.Seconds);
                        double dwDurations = timsSpan.TotalSeconds;
                        int lsState = System.Convert.ToInt32(strLastState);
                        //若上次为正常，不做处理，若上次为迟到，那么这次正常则状态依然为迟到，若这次为早退，则为迟到加早退
                        if (state == 2 && lsState == 1)
                        {
                            state = 3;
                        }
                        else if (state == 0)
                        {
                            state = lsState;
                        }

                        // 更新表
                        if ((dev.AntNo == "3" && (user.CopyType == 3 || user.CopyType == 4))
                        || (dev.AntNo == "4" && user.CopyType == 6)
                        || (dev.AntNo == "8" && user.CopyType == 9)
                        )
                        {

                            DAccess.UpdateWorkDurationRecordAndBak1(strDuration, System.Convert.ToInt64(dwDurations), state.ToString(),
                                  now.ToString("yyyy-MM-dd HH:mm:ss"),
                                 System.Convert.ToInt32(durId),
                                 1);
                        }
                        else
                        {
                            DAccess.UpdateWorkDurationRecordAndBak1(strDuration, System.Convert.ToInt64(dwDurations), state.ToString(),
                                  now.ToString("yyyy-MM-dd HH:mm:ss"),
                                 System.Convert.ToInt32(durId), 0);

                        }

                        LogManager.LogSys(string.Format("用户[{0}]出勤时长[{1}]，更新工时记录！", user.UserName, strDuration));

                    }
                }
                else if (inorout == (int)Dev_InOrOut.In)
                {
                    LogManager.LogSys("【签到处理】");
                    string strDurDate = DateTime.Now.ToString("yyyy-MM-dd");
                    if (dev.DevType == "3" || dev.DevType == "4")
                    {
                        if (hasDure)
                        {
                            //时间判断
                            //算两个时间差的总分钟数需要用GetTotalMinutes,而GetMinutes只能得到60以内的尾数，故改正
                            if (timsSpan.TotalMinutes < 60 * 8)		//两次入井相隔不足八小时，不另算一次入井
                            {
                                //加入不做工时统计的日志
                                LogManager.LogSys(string.Format("用户[{0}]两次下井时间间隔不足8小时，不另做入井统计!", user.UserName));
                                return;
                            }
                        }
                    }
                    //增加不为生活区设备的条件
                    if (dev.AntNo != "7")
                    {
                        //若签到时间即将跨天，则工时表按后一天的日期登记为考勤日期
                        DateTime nextDate = now.Date.AddDays(1);
                        TimeSpan nextTimsSpan = nextDate - now;
                        //strDurDate = now.Date.ToString("yyyy-MM-dd");
                        //算两个时间差的总分钟数需要用GetTotalMinutes,而GetMinutes只能得到60以内的尾数，故改正
                        if (nextTimsSpan.TotalMinutes < 60)		//距后一日零时一小时内，记录第二天工作
                        {
                            strDurDate = nextDate.ToString("yyyy-MM-dd");
                        }
                        // 新增工时记录

                        DAccess.AddWorkDurationRecord(user.UserId, strDurDate, 0, "NULL", 0, System.Convert.ToInt32(dev.AntNo),
                           0, userRostInfo.dwNightWork, state.ToString(), now.ToString("yyyy-MM-dd HH:mm:sss"), "NULL", rostId.ToString());

                    }
                    else
                    {
                        //专门为生活区处理，签到时，插入工长为0.5的工时记录
                        DAccess.AddWorkDurationRecord(user.UserId, strDurDate, 0, "NULL", 0, System.Convert.ToInt32(dev.AntNo),
                        System.Convert.ToInt64(0.5), 0, "0", now.ToString("yyyy-MM-dd HH:mm:sss"), "NULL", "NULL");
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==SetWorkDuration==" + ex.Message);
            }

        }
        private static int GetState(int userId, int inorout, int devType, int devNum, int copytype, ref int rostId, ref RostInfo rostInfo)
        {
            //对井口1对多，以及生活区1对多的机器，刷卡不看班次时间，均为正常考勤
            if (devType == 4 || devType == 18)
            {
                return 0;
            }

            //对于生活区的设备，状态即为正常
            if (devNum == 7)
            {
                return 0;
            }

            if ((copytype == 3 || copytype == 4) && devType == 3)
            {
                return 0;
            }
            RostInfo userRost = null;
            rostId = GetRostInfo(userId, inorout, ref userRost);
            if (userRost != null)
                rostInfo = userRost;
            if (rostId < 0)
            {
                return -1;
            }


            //获取当前时间
            DateTime nowTime = DateTime.Now;
            int h = nowTime.Hour;
            int m = nowTime.Minute;
            int dwNowTime = h * 60 + m;

            //跨天时间增加24*60分钟
            if (userRost.dwStartTime < userRost.dwRealStartTime)
            {
                userRost.dwStartTime += 24 * 60;
                userRost.dwEndTime += 24 * 60;
                userRost.dwRealEndTime += 24 * 60;
            }
            else if (userRost.dwEndTime < userRost.dwRealStartTime)
            {
                userRost.dwEndTime += 24 * 60;
                userRost.dwRealEndTime += 24 * 60;
            }
            else if (userRost.dwRealEndTime < userRost.dwRealStartTime)
            {
                userRost.dwRealEndTime += 24 * 60;
            }

            //第二时间段跨天时间增加24*60分钟
            if (userRost.dwMult == 2)
            {
                if (userRost.dwStartTime2 < userRost.dwRealStartTime2)
                {
                    userRost.dwStartTime2 += 24 * 60;
                    userRost.dwEndTime2 += 24 * 60;
                    userRost.dwRealEndTime2 += 24 * 60;
                }
                else if (userRost.dwEndTime2 < userRost.dwRealStartTime2)
                {
                    userRost.dwEndTime2 += 24 * 60;
                    userRost.dwRealEndTime2 += 24 * 60;
                }
                else if (userRost.dwRealEndTime2 < userRost.dwRealStartTime2)
                {
                    userRost.dwRealEndTime2 += 24 * 60;
                }
            }

            //判断刷卡状态
            string strDur = userRost.strMulDur;
            //初始为第一时段的工时倍数
            int dwNowTemp = dwNowTime;

            if (dwNowTemp < userRost.dwRealStartTime)
            {
                dwNowTemp = dwNowTime + 24 * 60;
            }

            //1.判断是否超出时间范围
            if (dwNowTemp > userRost.dwRealEndTime)
            {
                //第一时段超时，判断是否在另一时段
                if (userRost.dwMult == 1)
                {
                    return 3;		//若单时段，则已超出时间段，返回班次外考勤状态：3
                }

                //对第二时间段进行判断
                strDur = userRost.strMulDur2;

                dwNowTemp = dwNowTime;	//回归原始时间

                if (dwNowTemp < userRost.dwRealStartTime2)
                {
                    dwNowTemp = dwNowTime + 24 * 60;
                }

                //1-判断是否超出时间范围
                if (dwNowTemp > userRost.dwRealEndTime2)
                {
                    return 3;		//第二时段超出，返回班次外考勤状态:3
                }

                //2-判断是否在正常考勤区间内
                if (inorout == (int)Dev_InOrOut.In)
                {

                    if (userRost.dwRealStartTime2 <= dwNowTemp && dwNowTemp <= userRost.dwStartTime2)
                    {
                        return 0;		//正常考勤
                    }
                    //add by v2.1
                    //若在下班签退时间内签到，视为签退
                    else if (userRost.dwEndTime2 <= dwNowTemp && dwNowTemp <= userRost.dwRealEndTime2)
                    {
                        return 101;		//下班时间签到，视为签退
                    }

                    else
                    {
                        return 1;		//在时段内，超出正常签到时间段，则均为迟到
                    }
                }
                else
                {
                    if (userRost.dwEndTime2 <= dwNowTemp && dwNowTemp <= userRost.dwRealEndTime2)
                    {
                        return 0;		//正常考勤
                    }

                    //2-若在时间内，最后两个小时内为早退
                    if ((userRost.dwEndTime2 - dwNowTemp) < 2 * 60)
                    {
                        return 2;		//早退
                    }
                    else
                    {
                        return 4;		//异常，不予处理（旷工）
                    }
                }

            }

            //2.判断是否在正常考勤区间内
            if (inorout == (int)Dev_InOrOut.In)
            {
                if (userRost.dwRealStartTime <= dwNowTemp && dwNowTemp <= userRost.dwStartTime)
                {
                    return 0;		//正常考勤
                }
                //add by v2.1
                //若在下班签退时间内签到，视为签退
                else if (userRost.dwEndTime <= dwNowTemp && dwNowTemp <= userRost.dwRealEndTime)
                {
                    return 101;		//下班时间签到，视为签退
                }
                else
                {
                    return 1;		//在时段内，超出正常签到时间段，则均为迟到
                }
            }
            else
            {
                if (userRost.dwEndTime <= dwNowTemp && dwNowTemp <= userRost.dwRealEndTime)
                {
                    return 0;		//正常考勤
                }

                //2-若在时间内，最后两个小时内为早退
                if ((userRost.dwEndTime - dwNowTemp) < 2 * 60)
                {
                    return 2;		//早退
                }
                else
                {
                    return 4;		//异常，不予处理（旷工）
                }
            }

        }

        /// <summary>
        /// 增加排班预设功能后，读取当前预设的班次信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="inorout">出入状态</param>
        /// <returns>班次信息</returns>
        private static int GetRostInfo(int userId, int inorout, ref RostInfo userRost)
        {

            try
            {
                LogManager.LogSys("获取排班信息");

                DataTable dt;
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                //先根据出入状态找出当前匹配的班次ID
                if (inorout == (int)Dev_InOrOut.In)
                {
                    //若签到时间即将跨天，则工时表按后一天的日期登记为考勤日期
                    DateTime now = DateTime.Now;
                    DateTime nextDay = now.Date.AddDays(1);
                    TimeSpan timespan = nextDay - now;
                    int day = now.Day;
                    //=====================================
                    //应该是小于60，也就是在当天的夜里11点到12点之间打卡上班的，将工时统计在下一天-------sayid
                    //=====================================
                    if (timespan.TotalMinutes < 60)
                    {
                        day = nextDay.Day;
                    }

                    //===========================================================
                    //sql.Format("select d%d as rostID from PreRostering where userid='%s' ", day, strUserId);---原有的sql
                    //在KQInfo中调用此的command赋值给错了
                    //===========================================================

                    dt = DAccess.GetPreRostInfoByUserId(userId.ToString(), "d" + day);

                }
                else
                {
                    //签退时，根据签到时工时记录表中记录的班次ID为准。
                    dt = DAccess.GetWorkDurationByUserId2(userId.ToString());
                }

                if (dt == null)
                {
                    LogManager.LogSys("获取排班信息失败。");
                    return -1;
                }

                if (dt.Rows.Count == 0)
                {
                    if (inorout == (int)Dev_InOrOut.In)
                    {
                        return GetRostInfo(userId, ref  userRost);
                    }
                    return -2;
                }
                int rostId = Convert.ToInt32(dt.Rows[0]["rostID"]);
                //若无班次，则取原用户信息表中的班次ID（该条是为了满足刚开始启用时，部分员工的签到记录还没有记录班次ID）
                if (rostId == 0)
                {
                    return GetRostInfo(userId, ref userRost);
                }

                //根据班次ID读取当前班次的具体信息
                //从DB读数据
                DataTable rostDetail = DAccess.GetRostingById(rostId);
                if (rostDetail == null)
                {
                    LogManager.LogSys("获取排班信息失败。");
                    return -1;
                }
                if (rostDetail.Rows.Count == 0)
                {
                    return -2;
                }

                userRost = new RostInfo();
                userRost.strUserId = userId.ToString();
                userRost.dwMult = 1;
                userRost.strRealStartTime = rostDetail.Rows[0]["realStartTime"].ToString();
                userRost.strStartTime = rostDetail.Rows[0]["startTime"].ToString();
                userRost.strEndTime = rostDetail.Rows[0]["endTime"].ToString();
                userRost.strRealEndTime = rostDetail.Rows[0]["realEndTime"].ToString();
                userRost.strMulDur = rostDetail.Rows[0]["mulripleDur"].ToString();

                userRost.dwRealStartTime = ComFunc.TimeStringToInt(userRost.strRealStartTime);
                userRost.dwStartTime = ComFunc.TimeStringToInt(userRost.strStartTime);
                userRost.dwEndTime = ComFunc.TimeStringToInt(userRost.strEndTime);
                userRost.dwRealEndTime = ComFunc.TimeStringToInt(userRost.strRealEndTime);

                int next=0;
                if(!( rostDetail.Rows[0]["nextID"] is DBNull))
                    next = System.Convert.ToInt32(rostDetail.Rows[0]["nextID"]);
                userRost.dwNightWork = (int)rostDetail.Rows[0]["nightWork"];

                //将第一时段的读取内容提前，并查询完后关闭结果集，为第二时段准备
                if (next > 0)
                {
                    DataTable nextTable = DAccess.GetRostingById(next);
                    if (nextTable == null)
                    {
                        LogManager.LogSys("获取排班信息失败。");
                        return -1;

                    }
                    if (nextTable.Rows.Count == 0)
                    {
                        return -3;
                    }

                    userRost.dwMult = 2;
                    userRost.strRealStartTime2 = nextTable.Rows[0]["realStartTime"].ToString();
                    userRost.strStartTime2 = nextTable.Rows[0]["startTime"].ToString();
                    userRost.strEndTime2 = nextTable.Rows[0]["endTime"].ToString();
                    userRost.strRealEndTime2 = nextTable.Rows[0]["realEndTime"].ToString();
                    userRost.strMulDur2 = nextTable.Rows[0]["mulripleDur"].ToString();

                    userRost.dwRealStartTime = ComFunc.TimeStringToInt(userRost.strRealStartTime2);
                    userRost.dwStartTime2 = ComFunc.TimeStringToInt(userRost.strStartTime2);
                    userRost.dwEndTime2 = ComFunc.TimeStringToInt(userRost.strEndTime2);
                    userRost.dwRealEndTime = ComFunc.TimeStringToInt(userRost.strRealEndTime2);
                }
                return rostId;

            }
            catch (Exception ex)
            {
                LogManager.LogSys("==GetRostInfo(int userId,int inorout,ref RostInfo userRost)==" + ex.Message);
            }
            return -1;

        }
        /// <summary>
        /// 增加排班预设功能后，读取当前预设的班次信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="inorout">出入状态</param>
        /// <returns>班次信息</returns>
        private static int GetRostInfo(int userId, ref RostInfo userRost)
        {
            try
            {
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                //从DB获取数据
                DataTable rostDetail = DAccess.GetUserRostingByUserId(userId.ToString());
                if (rostDetail == null)
                {
                    LogManager.LogSys("获取排班信息失败。");
                    return -1;
                }

                //========================================================
                //如果没有记录则返回-2,并不是记录数为1返回-2-----------------sayid
                //========================================================
                if (rostDetail.Rows.Count == 0)
                {
                    return -2;
                }

                int rostId = (int)rostDetail.Rows[0]["ID"];
                userRost = new RostInfo();
                userRost.strUserId = userId.ToString();
                userRost.dwMult = 1;
                userRost.strRealStartTime = rostDetail.Rows[0]["realStartTime"].ToString();
                userRost.strStartTime = rostDetail.Rows[0]["startTime"].ToString();
                userRost.strEndTime = rostDetail.Rows[0]["endTime"].ToString();
                userRost.strRealEndTime = rostDetail.Rows[0]["realEndTime"].ToString();
                userRost.strMulDur = rostDetail.Rows[0]["mulripleDur"].ToString();

                userRost.dwRealStartTime = ComFunc.TimeStringToInt(userRost.strRealStartTime);
                userRost.dwStartTime = ComFunc.TimeStringToInt(userRost.strStartTime);
                userRost.dwEndTime = ComFunc.TimeStringToInt(userRost.strEndTime);
                userRost.dwRealEndTime = ComFunc.TimeStringToInt(userRost.strRealEndTime);

                int next = (int)rostDetail.Rows[0]["nextID"];
                userRost.dwNightWork = (int)rostDetail.Rows[0]["nightWork"];

                //将第一时段的读取内容提前，并查询完后关闭结果集，为第二时段准备
                if (next > 0)
                {
                    DataTable nextTable = DAccess.GetRostingById(next);
                    if (nextTable == null)
                    {
                        LogManager.LogSys("获取排班信息失败。");
                        return -1;

                    }
                    if (nextTable.Rows.Count == 0)
                    {
                        return -3;
                    }

                    userRost.dwMult = 2;
                    userRost.strRealStartTime2 = rostDetail.Rows[0]["realStartTime"].ToString();
                    userRost.strStartTime2 = rostDetail.Rows[0]["startTime"].ToString();
                    userRost.strEndTime2 = rostDetail.Rows[0]["endTime"].ToString();
                    userRost.strRealEndTime2 = rostDetail.Rows[0]["realEndTime"].ToString();
                    userRost.strMulDur2 = rostDetail.Rows[0]["mulripleDur"].ToString();

                    userRost.dwRealStartTime = ComFunc.TimeStringToInt(userRost.strRealStartTime2);
                    userRost.dwStartTime2 = ComFunc.TimeStringToInt(userRost.strStartTime2);
                    userRost.dwEndTime2 = ComFunc.TimeStringToInt(userRost.strEndTime2);
                    userRost.dwRealEndTime = ComFunc.TimeStringToInt(userRost.strRealEndTime2);
                }
                return rostId;

            }
            catch (Exception ex)
            {
                LogManager.LogSys("==GetRostInfo(int userId,ref RostInfo userRost)==" + ex.Message);
            }
            return -1;

        }
        /// <summary>
        /// 获取出入状态
        /// </summary>
        /// <param name="dev">设备</param>
        /// <param name="userId">用户ID</param>
        /// <returns>设备出入状态</returns>
        private static int GetInOrOut(DevInfo dev, int userId)
        {
            try
            {
                if (System.Convert.ToInt32(dev.AccessFlag) != (int)Dev_InOrOut.InAndOut)
                {
                    return System.Convert.ToInt32(dev.AccessFlag);
                }
                KQInfo DAccess = new KQInfo(Common.GetDataConnection(DataCollection.Config.FRASConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
                DataSet ds = DAccess.GetFirstWorkLog(userId.ToString(), dev.AntNo);

                //==============================================
                //此处进行了修改,详细看整理的逻辑------------------------------sayid
                //==============================================

                // 生活区
                #region 生活区的考勤不记录工时表，因此需要在流水表中查询
                if (dev.DevType == "18" || dev.AntNo == "7")
                {
                    if (ds == null || ds.Tables.Count == 0)
                    {
                        return -1;
                    }

                    DataTable dacclog = ds.Tables[0];

                    // 查询失败
                    if (dacclog == null)
                    {
                        return -1;
                    }
                    // 如果流水中没有记录
                    if (dacclog.Rows.Count == 0)
                    {
                        // 从工时表中查询
                        string date = DateTime.Now.ToString("yyyy-MM-dd");


                        DataTable d = DAccess.GetFirstWorkDuration(userId.ToString(), dev.AntNo, date);


                        // 没有记录,则为签到
                        if (d.Rows.Count == 0)
                        {
                            return (int)Dev_InOrOut.In;
                        }
                        else
                            return 99;
                    }

                    else
                    {
                        string lastDate = dacclog.Rows[0]["date"].ToString();
                        string lasttime = dacclog.Rows[0]["datetime"].ToString();
                        string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                        // 当天没有记录，为签到
                        if (lastDate != nowdate)
                        {
                            return (int)Dev_InOrOut.In;
                        }
                        else
                        {
                            DateTime datetemp = System.Convert.ToDateTime(lasttime);
                            TimeSpan span =  DateTime.Now-datetemp ;
                            // 判断比签到时间是否超过5小时，若超过，则为签退
                            if (span.Hours >= 5)
                            {
                                return (int)Dev_InOrOut.Out;
                            }
                            else
                            {
                                return 99;
                            }
                        }
                    }
                }
                //else
                //{

                    //    // 如果最后一天记录是出，则当前为入
                //    if (dacclog.Rows[0]["accessFlag"].ToString() == "1")
                //    {
                //        return (int)Dev_InOrOut.In;
                //    }
                //    else
                //    {
                //        return (int)Dev_InOrOut.Out;
                //    }
                //}
                //}
                #endregion

                #region 非生活区看workduration表中最后一条纪录是否duration=null和duration=0存在，如果存在则是出，否则是进
                else // 非生活区
                {
                    if (ds.Tables.Count < 2)
                    {
                        return -1;
                    }
                    DataTable dworkDur = ds.Tables[1];
                    // 查询失败
                    if (dworkDur == null)
                    {
                        return -1;
                    }
                    // 没有记录，代表是签到(入井）
                    if (dworkDur.Rows.Count == 0)
                    {
                        return (int)Dev_InOrOut.In;
                    }
                    return (int)Dev_InOrOut.Out;
                }
                #endregion

            }
            catch (Exception ex)
            {
                LogManager.LogSys("==GetInOrOut==" + ex.Message);
            }
            return (int)Dev_InOrOut.Out;

        }


        /// <summary>
        /// 获取图片保存路径
        /// </summary>
        /// <param name="dirtype">目录类型</param>
        /// <param name="antNum">设备号</param>
        /// <returns>目录路径</returns>
        public static string GetSavPath(Dir_Type dirtype, int antNum)
        {
            try
            {
                string appPath = AppDomain.CurrentDomain.BaseDirectory;
                appPath = Path.Combine(appPath, dirtype.ToString());
                if (dirtype == Dir_Type.SavePic || dirtype == Dir_Type.ErrPic)
                {
                    appPath = Path.Combine(appPath, DateTime.Now.ToString("yyyy-MM-dd"));
                    appPath = Path.Combine(appPath, antNum.ToString());


                }
                if (!Directory.Exists(appPath))
                {
                    Directory.CreateDirectory(appPath);
                }
                return appPath;
            }
            catch (Exception ex)
            {
                LogManager.LogSys("==GetSavPath==" + ex.Message);
            }
            return string.Empty;

        }
        /// <summary>
        /// 获取照片名称
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static string GetPhotoName(string userId)
        {
            DateTime d = DateTime.Now;
            return string.Format("{0}_vPhoto_{1}{2}{3}{4}{5}{6}.jpg", userId, d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }





    }
}
