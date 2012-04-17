using System;
using System.Collections.Generic;
using System.Text;
using SND.KQ.DAL.FRAS;
using SND.DA.DataAccessHelper;
using System.Data;
using SND.KQ.BL.EntityData;
using SND.KQ.Machines.DevInterface;
using System.IO;

namespace SND.KQ.BL
{
    public class InputUserFeatureBL
    {
        private string mstrConnectionString;
        private DataSourceType mDataSourceType;
        private KQInfo DAccess = null;
        private IMachine Machine = null;
        private string TemplateFeaturePath = @"C:\Program Files\SND\FRAS\Feature";
        private string TemplatePhotoPath = @"C:\Program Files\SND\FRAS\\Picture";
        public delegate void CompleteProcess(bool result,string msg);

        public CompleteProcess CompleteHandler=null;

        public InputUserFeatureBL(String strConnectionString)
        {
            mstrConnectionString = strConnectionString;
            mDataSourceType = SND.DA.DataAccessHelper.DataSourceType.SqlClient;
            Common.Initialize(mstrConnectionString, mDataSourceType);

            DAccess = new KQInfo(Common.GetDataConnection(strConnectionString, SND.DA.DataAccessHelper.DataSourceType.SqlClient));
        }
        public int InitialMachine(string ip, string port, string userName, string password, string machineNum)
        {
            Machine = MachineFactory.GetMachine("MACHINE", ip, port, userName, password, machineNum);
            Machine.OnEInputFeaturer = new EventHandler(OnCompleteTemplate);
            return Machine.Connect();
        }

        public DataTable GetUserInfoByName(string userName)
        {
            return DAccess.GetUserInfoByName(userName);
        }

        public DataTable GetUserInfoById(string userId)
        {
            return DAccess.GetUserInfoById(userId);
        }

        public DataTable GetDeptInfo()
        {
            return DAccess.GetDeptInfo();
        }

        public DataTable GetRankInfo()
        {
            return DAccess.GetRankInfo();
        }

        public int AddCardNo(string cardId,string userId)
        {
            DataTable dt = DAccess.GetCardInfoByUserId(userId);
            if (dt != null && dt.Rows.Count > 0)
            {
                return DAccess.UpdateUserCardInfo(userId,cardId);
            }
            else
            {
                return DAccess.CreatUserCard(cardId, userId);
            }
        }

        public int AddUserToMachine(UserInfo UserInfo)
        {
            if(Machine!=null)
            {
                EnrollUserEventArgs arg = new EnrollUserEventArgs();
                arg.UserId = System.Convert.ToInt32(UserInfo.UserId);
                arg.UserName = UserInfo.UserName;
                arg.UserType = UserInfo.Type;
                arg.CardNo = UserInfo.CardNo;
                arg.DeptId = System.Convert.ToInt32(UserInfo.DeptId);
                arg.lPhotoType = 0;
                arg.lPhotoLen=0;
                arg.strBase64PhotoData=string.Empty;
                return Machine.ModifyUser(arg);
            }
            return 0;
        }
        public bool SetTemplate(int userId)
        {
            Machine.InputFeature(userId, 1);
            return true;
        }

        private void OnCompleteTemplate(object sender, EventArgs e)
        {
            EInputUserFeatuer arg = e as EInputUserFeatuer;
            bool isSucc=false;
            // 录入成功
            if (arg.lOpCode == 0)
            {
               isSucc=true;
               if (!Directory.Exists(TemplateFeaturePath))
               {
                   Directory.CreateDirectory(TemplateFeaturePath);
               }
               if (!Directory.Exists(TemplatePhotoPath))
               {
                   Directory.CreateDirectory(TemplatePhotoPath);
               }
               string featurePath=Path.Combine(TemplateFeaturePath,ComFunc.GetPhotoPath(arg.lUserID.ToString(),"fea","dat"));
               string photoPath=Path.Combine(TemplatePhotoPath,ComFunc.GetPhotoPath(arg.lUserID.ToString(),"Picture","jpg"));
               ComFunc.Base64StringToImage(arg.strBase64PhotoData,photoPath);
               ComFunc.Base64StringToFile(arg.strBase64FeatureData,featurePath);
               int featureId = 0;
               int ret =DAccess.CreatUserFeature(out featureId,featurePath,photoPath);
               if(ret==0)
               {
                   isSucc=false;
               }
               if (featureId != 0)
               {
                  ret = DAccess.UpdateUserInfoFeatureInfo(arg.lUserID.ToString(), featureId);
                  if(ret==0)
                  {
                      isSucc=false;
                  }
               }

               
            }

            if (CompleteHandler != null)
            {
                CompleteHandler(isSucc, "");
            }
        }
    }
}
