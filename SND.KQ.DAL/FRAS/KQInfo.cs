
namespace SND.KQ.DAL.FRAS
{
    using SND.DA.CommandBuilder;
    using SND.DA.DataAccessHelper;
    using System.Data;
    using System.Collections.Generic;


    public partial class KQInfo
    {

        private DataConnection mConnection;



        public KQInfo(DataConnection conn)
        {
            this.mConnection = ((DataConnection)(conn));
        }
        public virtual System.Data.DataTable GetDevInfo()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetDevInfo);
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "DevInfo");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["DevInfo"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetUserInfo()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetUserInfo);
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "UserInfo");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["UserInfo"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetCopyData()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetCopyData);
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "CopyData");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["CopyData"];
            }
            return null;

        }

        public virtual System.Data.DataTable GetAccessLogByUserId(string userId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetAccessLogByUserId);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "AccessLog");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["AccessLog"];
            }
            return null;

        }

        public virtual System.Data.DataSet GetFirstWorkLog(string userId, string devNum)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetFirstWorkLog);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["DevNum"].Value = devNum;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            return ds;

        }
        public virtual System.Data.DataTable GetFirstWorkDuration(string userId, string devNum, string date)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetFirstWorkDuration);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["DevNum"].Value = devNum;
            cmdBuilder.Parameters["Date"].Value = date;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }


        public virtual System.Data.DataTable GetAccessLogByUserIdAndDevNum(string userId, string devNum)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetAccessLogByUserIdAndDevNum);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["DevNum"].Value = devNum;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "AccessLog");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["AccessLog"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetWorkDurationByUserId(string userId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetWorkDurationByUserId);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetWorkDurationByUserId2(string userId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetWorkDurationByUserId2);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetRostingById(int rostId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetRostingById);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["Id"].Value = rostId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetUserRostingByUserId(string userId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetUserRostingByUserId);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetWorkDurationByRosteId(string rosteId)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetWorkDurationByRosteId);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["Id"].Value = rosteId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual DataTable GetUserWorkDuration()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetUserWorkDuration);
            System.Data.Common.DataTableMappingCollection tableMappings = null;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }

        public virtual System.Data.DataTable GetAllRostInfo()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetAllRostInfo);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetWorkDurationByUserIdAndDevNum(string userId, string devNum)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetWorkDurationByUserIdAndDevNum);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["DevNum"].Value = devNum;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "Table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["Table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetPreRostInfoByUserId(string userId, string columName)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetPreRostInfoByUserId);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            //cmdBuilder.Parameters["ColumID"].Value = columName;
            cmdBuilder.Parameters["UserId"].Value = userId;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "Table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);

            if (ds.Tables.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("rostID", typeof(string));
                if (ds.Tables["Table1"].Rows.Count > 0)
                {
                    DataRow row = dt.NewRow();
                    row["rostID"] = ds.Tables["Table1"].Rows[0][columName].ToString();
                    dt.Rows.Add(row);
                }
                return dt;
            }
            return null;

        }

        public virtual int UpdateWorkDurationRecord(string durations, long duration, string bak3, int id, int mulDur)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.UpdateWorkDurationRecord);
            cmdBuilder.Parameters["Durations"].Value = durations;
            cmdBuilder.Parameters["duration"].Value = duration;
            cmdBuilder.Parameters["Bak3"].Value = bak3;
            cmdBuilder.Parameters["ID"].Value = id;
            if (mulDur != 0)
            {
                cmdBuilder.Parameters["MulDur"].Value = mulDur;
            }
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }

        public virtual int UpdateWorkDuration(string id)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.UpdateWorkDurations);
            cmdBuilder.Parameters["ID"].Value = id;

            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }


        public virtual int UpdateWorkDurationRecordAndBak1(string durations, long duration, string bak1, string bak3, int id, int mulDur)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.UpdateWorkDurationRecordAndBak1);
            cmdBuilder.Parameters["Durations"].Value = durations;
            cmdBuilder.Parameters["duration"].Value = duration;
            cmdBuilder.Parameters["Bak1"].Value = bak1;
            cmdBuilder.Parameters["Bak3"].Value = bak3;
            cmdBuilder.Parameters["ID"].Value = id;
            if (mulDur != 0)
            {
                cmdBuilder.Parameters["MulDur"].Value = mulDur;
            }
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }

        public virtual int AddWorkDurationRecord(string userId, string date, int accessNum, string durations, long duration, int devNum, long MulDur, int nightwork, string bak1, string bak2, string bak3, string bak4)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.AddWorkDurationRecord);
            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["Date"].Value = date;
            cmdBuilder.Parameters["DevNum"].Value = devNum;
            cmdBuilder.Parameters["MulDur"].Value = MulDur;
            cmdBuilder.Parameters["NightWork"].Value = nightwork;
            cmdBuilder.Parameters["Bak1"].Value = bak1;
            cmdBuilder.Parameters["Bak2"].Value = bak2;
            cmdBuilder.Parameters["Bak4"].Value = bak4;
            cmdBuilder.Parameters["AccessNum"].Value = accessNum;
            cmdBuilder.Parameters["Duration"].Value = duration;
            if (!string.IsNullOrEmpty(durations))
            {
                cmdBuilder.Parameters["Durations"].Value = durations;
            }
            if (!string.IsNullOrEmpty(bak3))
            {
                cmdBuilder.Parameters["Bak3"].Value = bak3;
            }
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }

        public virtual int AddAccessLog(string userId, string dateTime, string date, int inorout, int accessNum, int state, int devNum, string photoPath)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.AddAccessLog);
            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["DateTime"].Value = dateTime;
            cmdBuilder.Parameters["Date"].Value = date;
            cmdBuilder.Parameters["InOrOut"].Value = inorout;
            cmdBuilder.Parameters["Access"].Value = accessNum;
            cmdBuilder.Parameters["State"].Value = state;
            cmdBuilder.Parameters["DevNum"].Value = devNum;
            cmdBuilder.Parameters["PhotoPath"].Value = photoPath;

            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }

        public virtual int UpdateUserInfoFeatureInfo(string userId, int featureId)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.UpdateUserInfoFeatureInfo);
            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["FeatureId"].Value = featureId;
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }

        public virtual int UpdateUserCardInfo(string userId, string CardId)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.UpdateUserCardInfo);
            cmdBuilder.Parameters["UserId"].Value = userId;
            cmdBuilder.Parameters["CardId"].Value = CardId;
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            return retval;

        }


        /// <summary>
        /// </summary>
        public virtual int CreatUserFeature(out int featureId, string featurePath, string photoPath)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.CreatUserFeature);
            cmdBuilder.Parameters["featurePath"].Value = featurePath;
            cmdBuilder.Parameters["photoPath"].Value = photoPath;
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            int retval = cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
            featureId = ((int)(cmdBuilder.Parameters["ID"].Value));
            return retval;

        }

        /// <summary>
        /// </summary>
        public virtual int CreatUserInfo(string userId, string userName, int deptId, int featureId, int rankId, string senderId, int rosteringId, int type, int copyType, int flag)
        {

            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.CreatUserInfo);
            cmdBuilder.Parameters["userId"].Value = userId;
            cmdBuilder.Parameters["userName"].Value = userName;
            cmdBuilder.Parameters["deptId"].Value = deptId;
            cmdBuilder.Parameters["featureId"].Value = featureId;
            cmdBuilder.Parameters["rankId"].Value = rankId;
            cmdBuilder.Parameters["senderId"].Value = senderId;
            cmdBuilder.Parameters["rosteringId"].Value = rosteringId;
            cmdBuilder.Parameters["type"].Value = type;
            cmdBuilder.Parameters["copyType"].Value = copyType;
            cmdBuilder.Parameters["flag"].Value = flag;
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            return cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);

        }

        /// <summary>
        /// </summary>
        public virtual System.Data.DataTable GetCardInfoByUserId(string userId)
        {
            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetCardInfoByUserId);
            cmdBuilder.Parameters["UserId"].Value = userId;
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "Table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["Table1"];
            }
            return null;
        }

        /// <summary>
        /// </summary>
        public virtual int CreatUserCard(string cardId, string userId)
        {
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.CreatUserCard);
            cmdBuilder.Parameters["cardId"].Value = cardId;
            cmdBuilder.Parameters["userId"].Value = userId;
            if (mConnection.GetConnection().State == ConnectionState.Closed)
            {
                mConnection.Open();
            }
            return cmdBuilder.ExecuteNonQuery(mConnection.GetConnection(), mConnection.Transaction);
        }

        public virtual System.Data.DataTable GetUserInfoByName(string userName)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetUserInfoByName);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserName"].Value = userName;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "Table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["Table1"];
            }
            return null;

        }
        public virtual System.Data.DataTable GetUserInfoById(string userName)
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetUserInfoById);
            System.Data.Common.DataTableMappingCollection tableMappings = null;


            cmdBuilder.Parameters["UserId"].Value = userName;

            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "Table1");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["Table1"];
            }
            return null;

        }

        public virtual System.Data.DataTable GetDeptInfo()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetDeptInfo);
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "DeptInfo");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["DeptInfo"];
            }
            return null;

        }

        public virtual System.Data.DataTable GetRankInfo()
        {

            DataSet ds = new DataSet();
            Command cmdBuilder = mConnection.Commands.GetCommand(CommandEnumeration.GetRankInfo);
            System.Data.Common.DataTableMappingCollection tableMappings = null;
            tableMappings = new System.Data.Common.DataTableMappingCollection();
            tableMappings.Add("Table", "RankInfo");
            cmdBuilder.ExecuteAndFillDataSet(mConnection.GetConnection(), mConnection.Transaction, ds, tableMappings);
            if (ds.Tables.Count > 0)
            {
                return ds.Tables["RankInfo"];
            }
            return null;

        }

    }
}
