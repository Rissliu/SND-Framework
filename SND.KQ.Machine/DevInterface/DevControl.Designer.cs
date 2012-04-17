namespace SND.KQ.Machines.DevInterface
{
    partial class DevControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DevControl));
            this.axFirsFaceSdk1 = new AxFirsFaceSdkLib.AxFirsFaceSdk();
            ((System.ComponentModel.ISupportInitialize)(this.axFirsFaceSdk1)).BeginInit();
            this.SuspendLayout();
            // 
            // axFirsFaceSdk1
            // 
            this.axFirsFaceSdk1.Enabled = true;
            this.axFirsFaceSdk1.Location = new System.Drawing.Point(172, 190);
            this.axFirsFaceSdk1.Name = "axFirsFaceSdk1";
            this.axFirsFaceSdk1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axFirsFaceSdk1.OcxState")));
            this.axFirsFaceSdk1.Size = new System.Drawing.Size(100, 50);
            this.axFirsFaceSdk1.TabIndex = 0;
            this.axFirsFaceSdk1.OnEventEnrollUser += new AxFirsFaceSdkLib._DFirsFaceSdkEvents_OnEventEnrollUserEventHandler(this.axFirsFaceSdk1_OnEventEnrollUser);
            this.axFirsFaceSdk1.OnEventTrap += new AxFirsFaceSdkLib._DFirsFaceSdkEvents_OnEventTrapEventHandler(this.axFirsFaceSdk1_OnEventTrap);
            // 
            // DevControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.axFirsFaceSdk1);
            this.Name = "DevControl";
            this.Text = "DevControl";
            ((System.ComponentModel.ISupportInitialize)(this.axFirsFaceSdk1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxFirsFaceSdkLib.AxFirsFaceSdk axFirsFaceSdk1;
    }
}