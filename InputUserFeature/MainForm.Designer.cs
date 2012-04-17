namespace InputUserFeature
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.mBtnConfirm = new System.Windows.Forms.Button();
            this.mUserInfogroupBox = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.mCombRank = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mCombDept = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.mCombCopyType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mTxtCardNum = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mTextUserId = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.mTxtUserName = new System.Windows.Forms.TextBox();
            this.minfoLable = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.mUserInfogroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.minfoLable);
            this.panel1.Controls.Add(this.mBtnConfirm);
            this.panel1.Controls.Add(this.mUserInfogroupBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 345);
            this.panel1.TabIndex = 0;
            // 
            // mBtnConfirm
            // 
            this.mBtnConfirm.Location = new System.Drawing.Point(464, 264);
            this.mBtnConfirm.Name = "mBtnConfirm";
            this.mBtnConfirm.Size = new System.Drawing.Size(75, 45);
            this.mBtnConfirm.TabIndex = 1;
            this.mBtnConfirm.Text = "确定";
            this.mBtnConfirm.UseVisualStyleBackColor = true;
            this.mBtnConfirm.Click += new System.EventHandler(this.mBtnConfirm_Click);
            // 
            // mUserInfogroupBox
            // 
            this.mUserInfogroupBox.Controls.Add(this.btnSearch);
            this.mUserInfogroupBox.Controls.Add(this.label6);
            this.mUserInfogroupBox.Controls.Add(this.mCombRank);
            this.mUserInfogroupBox.Controls.Add(this.label5);
            this.mUserInfogroupBox.Controls.Add(this.mCombDept);
            this.mUserInfogroupBox.Controls.Add(this.label4);
            this.mUserInfogroupBox.Controls.Add(this.mCombCopyType);
            this.mUserInfogroupBox.Controls.Add(this.label3);
            this.mUserInfogroupBox.Controls.Add(this.mTxtCardNum);
            this.mUserInfogroupBox.Controls.Add(this.label2);
            this.mUserInfogroupBox.Controls.Add(this.mTextUserId);
            this.mUserInfogroupBox.Controls.Add(this.label1);
            this.mUserInfogroupBox.Controls.Add(this.mTxtUserName);
            this.mUserInfogroupBox.Location = new System.Drawing.Point(11, 29);
            this.mUserInfogroupBox.Name = "mUserInfogroupBox";
            this.mUserInfogroupBox.Size = new System.Drawing.Size(541, 202);
            this.mUserInfogroupBox.TabIndex = 0;
            this.mUserInfogroupBox.TabStop = false;
            this.mUserInfogroupBox.Text = "用户信息";
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(235, 45);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(45, 26);
            this.btnSearch.TabIndex = 12;
            this.btnSearch.Text = "查询";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(313, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "职务:";
            // 
            // mCombRank
            // 
            this.mCombRank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mCombRank.Font = new System.Drawing.Font("SimSun", 9F);
            this.mCombRank.FormattingEnabled = true;
            this.mCombRank.Location = new System.Drawing.Point(352, 135);
            this.mCombRank.Name = "mCombRank";
            this.mCombRank.Size = new System.Drawing.Size(176, 20);
            this.mCombRank.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 142);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "部门:";
            // 
            // mCombDept
            // 
            this.mCombDept.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mCombDept.Font = new System.Drawing.Font("SimSun", 9F);
            this.mCombDept.FormattingEnabled = true;
            this.mCombDept.Location = new System.Drawing.Point(53, 139);
            this.mCombDept.Name = "mCombDept";
            this.mCombDept.Size = new System.Drawing.Size(176, 20);
            this.mCombDept.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(289, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "同步类型:";
            // 
            // mCombCopyType
            // 
            this.mCombCopyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mCombCopyType.Font = new System.Drawing.Font("SimSun", 9F);
            this.mCombCopyType.FormattingEnabled = true;
            this.mCombCopyType.Location = new System.Drawing.Point(352, 87);
            this.mCombCopyType.Name = "mCombCopyType";
            this.mCombCopyType.Size = new System.Drawing.Size(176, 20);
            this.mCombCopyType.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "卡号:";
            // 
            // mTxtCardNum
            // 
            this.mTxtCardNum.Font = new System.Drawing.Font("SimSun", 9F);
            this.mTxtCardNum.Location = new System.Drawing.Point(53, 91);
            this.mTxtCardNum.Name = "mTxtCardNum";
            this.mTxtCardNum.Size = new System.Drawing.Size(176, 21);
            this.mTxtCardNum.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "工号:";
            // 
            // mTextUserId
            // 
            this.mTextUserId.Font = new System.Drawing.Font("SimSun", 9F);
            this.mTextUserId.Location = new System.Drawing.Point(352, 45);
            this.mTextUserId.Name = "mTextUserId";
            this.mTextUserId.ReadOnly = true;
            this.mTextUserId.Size = new System.Drawing.Size(176, 21);
            this.mTextUserId.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "姓名:";
            // 
            // mTxtUserName
            // 
            this.mTxtUserName.Font = new System.Drawing.Font("SimSun", 9F);
            this.mTxtUserName.Location = new System.Drawing.Point(53, 45);
            this.mTxtUserName.Name = "mTxtUserName";
            this.mTxtUserName.Size = new System.Drawing.Size(176, 21);
            this.mTxtUserName.TabIndex = 0;
            // 
            // minfoLable
            // 
            this.minfoLable.Location = new System.Drawing.Point(39, 280);
            this.minfoLable.Name = "minfoLable";
            this.minfoLable.Size = new System.Drawing.Size(158, 23);
            this.minfoLable.TabIndex = 2;
            this.minfoLable.Text = "数据处理中，请稍等...";
            this.minfoLable.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 345);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "模板录入";
            this.panel1.ResumeLayout(false);
            this.mUserInfogroupBox.ResumeLayout(false);
            this.mUserInfogroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox mUserInfogroupBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mTextUserId;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox mTxtUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mTxtCardNum;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox mCombRank;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox mCombDept;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox mCombCopyType;
        private System.Windows.Forms.Button mBtnConfirm;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label minfoLable;
    }
}