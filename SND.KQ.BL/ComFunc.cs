using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Drawing;

namespace SND.KQ.BL
{
    public class ComFunc
    {
        public static void Base64StringToImage(string base64str, string imagefilename)  
        {  
            try  
            {  
                byte[] arr = Convert.FromBase64String(base64str);  
                MemoryStream ms = new MemoryStream(arr);  
                Bitmap bmp = new Bitmap(ms);
                bmp.Save(imagefilename, System.Drawing.Imaging.ImageFormat.Jpeg);  
                ms.Close();  
     
            }  
            catch
            {  

            }  
        }
       public static string ImgToBase64String(string Imagefilename)  
       {  
           try  
           {  
               Bitmap bmp = new Bitmap(Imagefilename);    
               MemoryStream ms = new MemoryStream();  
               bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);  
               byte[] arr = new byte[ms.Length];  
               ms.Position = 0;  
               ms.Read(arr, 0, (int)ms.Length);  
               ms.Close();  
               return  Convert.ToBase64String(arr);  
               
           }  
           catch
           {  
              
           }
           return string.Empty;
       }
       public static string ImgToBase64String(ref int length,string Imagefilename)
       {
           try
           {
               Bitmap bmp = new Bitmap(Imagefilename);
               MemoryStream ms = new MemoryStream();
               bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
               byte[] arr = new byte[ms.Length];
               length = arr.Length;
               ms.Position = 0;
               ms.Read(arr, 0, (int)ms.Length);
               ms.Close();
               return Convert.ToBase64String(arr);

           }
           catch
           {

           }
           return string.Empty;
       }

       public static string FileToBase64String(ref int length,string filename)
       {
           try
           {
               FileStream fs = File.OpenRead(filename);
               byte[] arr = new byte[fs.Length];
               length = arr.Length;
               fs.Position = 0;
               fs.Read(arr, 0, (int)fs.Length);
               fs.Close();
               return Convert.ToBase64String(arr);

           }
           catch
           {

           }
           return string.Empty;
       }


       public static void Base64StringToFile(string base64string, string filename)
       {
           try
           {
               using (FileStream stream = File.Create(filename))
               {
                   StreamWriter sw = new StreamWriter(stream);
                   sw.Write(base64string);
                   sw.Flush();
                   sw.Close();
               }

           }
           catch
           {

           }
         
       }

        public static int TimeStringToInt(string strTime)
       {
           return (Convert.ToInt32(strTime.Substring(0, 2)) * 60 + Convert.ToInt32(strTime.Substring(3,2)));
        }

        public static string GetPhotoPath(string userId, string type, string ext)
        {
            System.DateTime date = DateTime.Now;
            return string.Format("{0}_{1}_{2}{3}{4}{5}{6}{7}.{8}", userId, type, date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second,ext);
        }

        public static string GetUserIdString(string id)
        {
            string UserId = "";
            if (id.Length >= 4)
                return id;
            else
            {
                for (int i = 0; i < 4 - id.Length; i++)
                {
                    UserId += "0";
                }
                UserId += id;
                return UserId;
            }
        }
    }
}
