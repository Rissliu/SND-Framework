using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using SND.KQ.BL;
using SND.KQ.Log;

namespace SND.KQ.ServiceForm
{
    public partial class MainForm : Form
    {
        // 线程管理类
        ThreadManager manager = null;
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //加载配置文件
            DataCollection.Config = SysConfig.LoadConfig();

            //初始化日志管理类
            LogManager.InitialLog(DataCollection.Config.SysLogPath, DataCollection.Config.InfoLogPath);

            // 将数据库数据加载到内存
            DataCollection.LoadDBData();

            //初始线程管理类
            manager = new ThreadManager();
            manager.Initialize(DataCollection.Config);

            // 启动线程管理类
            manager.OnStart();
        }

        private void MinimizedToNormal()
        {
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            KQService.Visible = false;

        }

        private void NormalToMinimized()
        {
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.KQService.Visible = true;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                NormalToMinimized();
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            NormalToMinimized();
        }

        private void KQService_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.MinimizedToNormal();
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show("确认要退出程序？", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.Close();
                    if (manager != null)
                    {
                        manager.OnStop();
                    }
                }
            }

        }
    }
}
