using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using SND.KQ.BL;
using System.Configuration;
using SND.KQ.BL.EntityData;

namespace InputUserFeature
{
    public partial class MainForm : Form
    {
        private InputUserFeatureBL BL = null;
        private UserInfo UserInfo = null;
        private DataTable BakDataTable = null;
        public MainForm()
        {
            InitializeComponent();
            InitialData();

            this.FormBorderStyle = FormBorderStyle.FixedDialog;
        }
        private void InitialData()
        {
            string connStr = ConfigurationManager.AppSettings["ConnectionString"];
            if (!string.IsNullOrEmpty(connStr))
            {
                BL = new InputUserFeatureBL(connStr);
                BL.CompleteHandler = new InputUserFeatureBL.CompleteProcess(CompleteHandler);
                string ip = ConfigurationManager.AppSettings["ip"];
                string port = ConfigurationManager.AppSettings["port"];
                string machineNum = ConfigurationManager.AppSettings["machineNum"];
                string userName = ConfigurationManager.AppSettings["userName"];
                string password = ConfigurationManager.AppSettings["password"];
                BL.InitialMachine(ip, port, userName, password, machineNum);
            }

            PopulateCopyType();
            PopulateDetp();
            PopulateRankInfo();
        }

        private void PopulateCopyType()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Name"));
            dt.Columns.Add(new DataColumn("Value"));

            DataRow row = dt.NewRow();
            row["Name"] = "-请选择同步类型-";
            row["Value"] = "0";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "下井员工";
            row["Value"] = "1";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "联建楼(地面工种)";
            row["Value"] = "2";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "联建楼(地面工种,需要下井)";
            row["Value"] = "3";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "队长,副队长,技术员";
            row["Value"] = "4";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "办公楼(不下井)";
            row["Value"] = "5";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "办公楼(需要下井)";
            row["Value"] = "6";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "生活区";
            row["Value"] = "7";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "下井员工(出井刷卡)";
            row["Value"] = "8";
            dt.Rows.Add(row);

            row = dt.NewRow();
            row["Name"] = "联建楼(出井刷卡)";
            row["Value"] = "9";
            dt.Rows.Add(row);

            this.mCombCopyType.DataSource = dt;
            this.mCombCopyType.DisplayMember = "Name";
            this.mCombCopyType.ValueMember = "Value";
        }

        public void PopulateDetp()
        {
            if (BL != null)
            {
                DataTable dt = BL.GetDeptInfo();
                if (dt != null)
                {
                    DataRow row = dt.NewRow();
                    row["deptName"] = "--请选择部门--";
                    row["deptId"] = "0";
                    dt.Rows.InsertAt(row, 0);
                    this.mCombDept.DataSource = dt;
                    this.mCombDept.DisplayMember = "deptName";
                    this.mCombDept.ValueMember = "deptId";

                }
            }
        }

        public void PopulateRankInfo()
        {
            if (BL != null)
            {
                DataTable dt = BL.GetRankInfo();
                if (dt != null)
                {
                    DataRow row = dt.NewRow();
                    row["Rank"] = "--请选择职务--";
                    row["ID"] = "0";
                    dt.Rows.InsertAt(row, 0);
                    this.mCombRank.DataSource = dt;
                    this.mCombRank.DisplayMember = "Rank";
                    this.mCombRank.ValueMember = "ID";
                }
            }
        }

        private void mBtnConfirm_Click(object sender, EventArgs e)
        {
            if (UserInfo == null)
            {
                MessageBox.Show(this, "请先选择用户！");
                return;
            }

            if (!CheckInput())
            {
                return;
            }
            UserInfo.UserName = this.mTxtUserName.Text.Trim();

            UserInfo.CardNo = this.mTxtCardNum.Text.Trim();

            SetPageState(false);

            // 添加卡号到DB
            BL.AddCardNo(UserInfo.CardNo, UserInfo.UserId);

            // 添加用户到机器
            if (Convert.ToInt32(UserInfo.UserId)<10)
                UserInfo.UserId = (SysData.USERID_OFFSET_COUNT + Convert.ToInt32(UserInfo.UserId)).ToString();
            int ret = BL.AddUserToMachine(UserInfo);
            if (ret == 1)
            {
                MessageBox.Show(this, "用户无法添加到机器,请检查用户信息是否正确或是设备连接正常。", "提示消息", MessageBoxButtons.OK);
                SetPageState(true);
                return;
            }
            // 模板录入
            BL.SetTemplate(System.Convert.ToInt32(UserInfo.UserId));
        }

        private void CompleteHandler(bool restult, string msg)
        {
            if (!restult)
            {
                MessageBox.Show(this, "模板录入失败,请重新录入。", "提示消息", MessageBoxButtons.OK);

            }
            else
            {
                MessageBox.Show(this, "模板录入成功。", "提示消息", MessageBoxButtons.OK);
                ClearFields();
                UserInfo = null;
            }

            SetPageState(true);
        }
        private void SetPageState(bool able)
        {
            this.mUserInfogroupBox.Enabled = able;
            this.mBtnConfirm.Enabled = able;
            this.minfoLable.Visible = !able;
        }
        private void ClearFields()
        {
            this.mTxtUserName.Text = string.Empty;
            this.mTextUserId.Text = string.Empty;
            this.mTxtCardNum.Text = string.Empty;
            this.mCombCopyType.SelectedIndex = 0;
            this.mCombDept.SelectedIndex = 0;
            this.mCombRank.SelectedIndex = 0;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BakDataTable = null;
            if (string.IsNullOrEmpty(this.mTxtUserName.Text.Trim()) & string.IsNullOrEmpty(this.mTextUserId.Text.Trim()))
            {
                MessageBox.Show(this, "请输入用户名或工号，然后点击查询。", "提示消息", MessageBoxButtons.OK);
                return;
            }
            if (BL != null)
            {
                DataTable dt = null;

                if (!string.IsNullOrEmpty(this.mTextUserId.Text.Trim()))
                    dt = BL.GetUserInfoById(this.mTextUserId.Text.Trim());
                else
                    dt = BL.GetUserInfoByName(this.mTxtUserName.Text.Trim());

                if (dt != null && dt.Rows.Count > 0)
                {
                    BakDataTable = dt;
                    this.mTxtUserName.Text = dt.Rows[0]["userName"].ToString().Trim();
                    this.mTextUserId.Text = dt.Rows[0]["userId"].ToString().Trim();
                    string deptId = dt.Rows[0]["deptId"].ToString().Trim();
                    this.mCombDept.SelectedValue = deptId;
                    string rankId = dt.Rows[0]["rankId"].ToString().Trim();
                    this.mCombRank.SelectedValue = rankId;
                    this.mTxtCardNum.Text = dt.Rows[0]["cardNo"] == null ? string.Empty : dt.Rows[0]["cardNo"].ToString().Trim();
                    this.mCombCopyType.SelectedValue = dt.Rows[0]["copyType"].ToString().Trim();

                    UserInfo = new UserInfo();
                    UserInfo.UserId = dt.Rows[0]["userId"].ToString().Trim();
                    UserInfo.UserName = dt.Rows[0]["userName"].ToString().Trim();
                    UserInfo.DeptId = dt.Rows[0]["deptId"].ToString().Trim();
                    UserInfo.RankId = dt.Rows[0]["rankId"].ToString().Trim();
                    UserInfo.CopyType = System.Convert.ToInt32(dt.Rows[0]["copyType"].ToString().Trim());
                    UserInfo.Type = System.Convert.ToInt32(dt.Rows[0]["type"].ToString().Trim());

                }
                else
                {
                    MessageBox.Show(this, "没有查询到该用户,请确认用户名是否输入正确。", "错误消息", MessageBoxButtons.OK);
                }
            }
        }
        private bool CheckInput()
        {
            if (UserInfo == null) //(string.IsNullOrEmpty(this.mTextUserId.Text.Trim()))
            {
                MessageBox.Show(this, "请先查询一个用户，然后进行模板录入", "提示消息", MessageBoxButtons.OK);
                return false;
            }

            //if (string.IsNullOrEmpty(this.mTxtCardNum.Text.Trim()))
            //{
            //    MessageBox.Show(this, "请输入卡号，如果没有卡号,请输入 0000000000 ", "提示消息", MessageBoxButtons.OK);
            //    return false;
            //}
            //bool ret =  Regex.IsMatch(this.mTxtCardNum.Text, "/[0-9]/");
            //if (!ret)
            //{

            //    MessageBox.Show(this, "卡号格式为10位数字.如：0000110000 ", "提示消息", MessageBoxButtons.OK);
            //    return false;

            //}



            return true;
        }
    }
}
