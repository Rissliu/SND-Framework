using System;
using System.Collections.Generic;
using System.Text;

namespace SND.KQ.BL
{
    /// <summary>
    /// 系统常量以及枚举定义
    /// </summary>
    public class SysData
    {
        // 设备返回结果 成功
        public static int S_OK=0;
        // 设备返回结果 失败
        public static int S_FAIL = 1;

        // 用户ID偏移量
        //因为人脸识别设备的USERID，10以为被设定为管理员ID，因此在10以内的用户，前面加621，凑7位
        //如userid为0008，则录入机器中的模板ID为6210008即可；读出时需要减去6210000；
        public static int USERID_OFFSET_COUNT= 6210000;


       

    }

    /// <summary>
    /// 设备类型
    /// </summary>
    public enum Dev_Type
    {
        //非井口1对1
        NoWellOneToOne = 1,
        //非井口1对多
        NoWellOneToMutile = 2,
        //井口1对1
        WellOneToOne = 3,
        //井口1对多
        WellOneToMutile = 4,
        //普通即生活区
        NormalLive = 18,

    }

    /// <summary>
    /// 出入类别
    /// </summary>
    public enum Dev_InOrOut
    {
        In = 0,//入
        Out = 1,//出
        InAndOut = 2//既出又入
    }

    public enum Dir_Type
    {
        Feature = 0,
        Picture = 1,
        SavePic = 2,
        ErrPic = 3,
    }
}
