using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SND.KQ.Log
{
    public static class  LogManager
    {
        //系统日志路径
        private static string mSysLogPath;
        //考勤信息日志路径
        private static string mInfoLogPath;
        private static object lockobj = new object();
        private static object lockobjInfo = new object();

        public static void InitialLog(string sysLogPath,string infoLogPath)
        {
            if (!Directory.Exists(sysLogPath))
            {
                Directory.CreateDirectory(sysLogPath);
            }
            if (!Directory.Exists(infoLogPath))
            {
                Directory.CreateDirectory(infoLogPath);
            }

            mSysLogPath = sysLogPath;
            mInfoLogPath = infoLogPath;
        }

        public static void LogSys(String msg)
        {
            try
            {
                lock (lockobj)
                {
                    string fileName = Path.Combine(mSysLogPath, "FRAS_" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                    if (!File.Exists(fileName))
                    {
                      using (FileStream stream= File.Create(fileName))
                      {
                          stream.Close();
                      }
                    }
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(@"/-----------------------------begin---------------------------------\");
                        sw.WriteLine(System.DateTime.Now);
                        sw.Write(Environment.NewLine);
                        sw.Write(msg);
                        sw.Write(Environment.NewLine);
                        sw.WriteLine(@"\-----------------------------end-----------------------------------/");
                        sw.Flush();
                        sw.Close();
                    }
                }


            }
            finally
            { ;}
        }

        public static void LogInfo(String msg)
        {
            try
            {
                lock (lockobjInfo)
                {
                    string fileName = Path.Combine(mInfoLogPath, "FRAS_Info" + DateTime.Now.ToString("yyyy-MM-dd") + ".log");
                    if (!File.Exists(fileName))
                    {
                        using (FileStream stream = File.Create(fileName))
                        {
                            stream.Close();
                        }
                    }
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.WriteLine(msg);
                        sw.Flush();
                        sw.Close();
                    }
                }

               
            }
            finally
            { ;}
        }
    }
}
