namespace Witcher3_Multiplayer
{
    partial class SimpleOverlay
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
            this.ConnectBTN = new HuaweiUnlocker.UI.NButton();
            this.HostBtn = new HuaweiUnlocker.UI.NButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ConnectBTN
            // 
            this.ConnectBTN.BackColor = System.Drawing.Color.Tomato;
            this.ConnectBTN.BackColorAdditional = System.Drawing.Color.Gray;
            this.ConnectBTN.BackColorGradientEnabled = false;
            this.ConnectBTN.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.ConnectBTN.BorderColor = System.Drawing.Color.Tomato;
            this.ConnectBTN.BorderColorEnabled = false;
            this.ConnectBTN.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.ConnectBTN.BorderColorOnHoverEnabled = false;
            this.ConnectBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConnectBTN.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ConnectBTN.ForeColor = System.Drawing.Color.White;
            this.ConnectBTN.Location = new System.Drawing.Point(6, 114);
            this.ConnectBTN.Name = "ConnectBTN";
            this.ConnectBTN.RippleColor = System.Drawing.Color.Black;
            this.ConnectBTN.RoundingEnable = false;
            this.ConnectBTN.Size = new System.Drawing.Size(176, 30);
            this.ConnectBTN.TabIndex = 0;
            this.ConnectBTN.Text = "ConnetToServer";
            this.ConnectBTN.TextHover = null;
            this.ConnectBTN.UseDownPressEffectOnClick = false;
            this.ConnectBTN.UseRippleEffect = true;
            this.ConnectBTN.UseVisualStyleBackColor = false;
            this.ConnectBTN.UseZoomEffectOnHover = false;
            this.ConnectBTN.Click += new System.EventHandler(this.ConnectBTN_Click);
            // 
            // HostBtn
            // 
            this.HostBtn.BackColor = System.Drawing.Color.Tomato;
            this.HostBtn.BackColorAdditional = System.Drawing.Color.Gray;
            this.HostBtn.BackColorGradientEnabled = false;
            this.HostBtn.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.HostBtn.BorderColor = System.Drawing.Color.Tomato;
            this.HostBtn.BorderColorEnabled = false;
            this.HostBtn.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.HostBtn.BorderColorOnHoverEnabled = false;
            this.HostBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HostBtn.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.HostBtn.ForeColor = System.Drawing.Color.White;
            this.HostBtn.Location = new System.Drawing.Point(298, 114);
            this.HostBtn.Name = "HostBtn";
            this.HostBtn.RippleColor = System.Drawing.Color.Black;
            this.HostBtn.RoundingEnable = false;
            this.HostBtn.Size = new System.Drawing.Size(179, 30);
            this.HostBtn.TabIndex = 1;
            this.HostBtn.Text = "HostServer";
            this.HostBtn.TextHover = null;
            this.HostBtn.UseDownPressEffectOnClick = false;
            this.HostBtn.UseRippleEffect = true;
            this.HostBtn.UseVisualStyleBackColor = false;
            this.HostBtn.UseZoomEffectOnHover = false;
            this.HostBtn.Click += new System.EventHandler(this.HostBtn_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(-1, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(504, 176);
            this.tabControl1.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.ConnectBTN);
            this.tabPage1.Controls.Add(this.HostBtn);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(496, 150);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Actions";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(496, 150);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "ServerController";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // SimpleOverlay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 180);
            this.Controls.Add(this.tabControl1);
            this.Name = "SimpleOverlay";
            this.Text = "SimpleOverlay";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private HuaweiUnlocker.UI.NButton ConnectBTN;
        private HuaweiUnlocker.UI.NButton HostBtn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
    }
}