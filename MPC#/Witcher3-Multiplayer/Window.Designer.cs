namespace Witcher3_Multiplayer
{
    partial class Window
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
            this.LOG = new System.Windows.Forms.TextBox();
            this.CMD = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.DbgChck = new System.Windows.Forms.CheckBox();
            this.TmClDebug = new System.Windows.Forms.CheckBox();
            this.NickNameBOX = new System.Windows.Forms.ComboBox();
            this.CharacterVisBOX = new System.Windows.Forms.ComboBox();
            this.ServerIPText = new System.Windows.Forms.TextBox();
            this.ConnectServerBTN = new HuaweiUnlocker.UI.NButton();
            this.CreateServerBTN = new HuaweiUnlocker.UI.NButton();
            this.SendCmdBTN = new HuaweiUnlocker.UI.NButton();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // LOG
            // 
            this.LOG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.LOG.Dock = System.Windows.Forms.DockStyle.Top;
            this.LOG.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LOG.ForeColor = System.Drawing.Color.Silver;
            this.LOG.Location = new System.Drawing.Point(0, 0);
            this.LOG.Multiline = true;
            this.LOG.Name = "LOG";
            this.LOG.Size = new System.Drawing.Size(832, 311);
            this.LOG.TabIndex = 1;
            this.LOG.Text = "WITCHER 3MP LOG BOX V2\r\n";
            // 
            // CMD
            // 
            this.CMD.Location = new System.Drawing.Point(12, 317);
            this.CMD.Name = "CMD";
            this.CMD.Size = new System.Drawing.Size(808, 20);
            this.CMD.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.groupBox1.Controls.Add(this.DbgChck);
            this.groupBox1.Controls.Add(this.TmClDebug);
            this.groupBox1.Controls.Add(this.NickNameBOX);
            this.groupBox1.Controls.Add(this.CharacterVisBOX);
            this.groupBox1.Controls.Add(this.ServerIPText);
            this.groupBox1.ForeColor = System.Drawing.Color.Cornsilk;
            this.groupBox1.Location = new System.Drawing.Point(12, 343);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(808, 170);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // DbgChck
            // 
            this.DbgChck.AutoSize = true;
            this.DbgChck.Location = new System.Drawing.Point(115, 99);
            this.DbgChck.Name = "DbgChck";
            this.DbgChck.Size = new System.Drawing.Size(165, 17);
            this.DbgChck.TabIndex = 12;
            this.DbgChck.Text = "Shit, garbage and debug logs";
            this.DbgChck.UseVisualStyleBackColor = true;
            // 
            // TmClDebug
            // 
            this.TmClDebug.AutoSize = true;
            this.TmClDebug.Checked = true;
            this.TmClDebug.CheckState = System.Windows.Forms.CheckState.Checked;
            this.TmClDebug.Location = new System.Drawing.Point(6, 99);
            this.TmClDebug.Name = "TmClDebug";
            this.TmClDebug.Size = new System.Drawing.Size(103, 17);
            this.TmClDebug.TabIndex = 11;
            this.TmClDebug.Text = "Local Client Join";
            this.TmClDebug.UseVisualStyleBackColor = true;
            // 
            // NickNameBOX
            // 
            this.NickNameBOX.FormattingEnabled = true;
            this.NickNameBOX.Location = new System.Drawing.Point(6, 72);
            this.NickNameBOX.Name = "NickNameBOX";
            this.NickNameBOX.Size = new System.Drawing.Size(796, 21);
            this.NickNameBOX.TabIndex = 10;
            this.NickNameBOX.Text = "NickName";
            // 
            // CharacterVisBOX
            // 
            this.CharacterVisBOX.FormattingEnabled = true;
            this.CharacterVisBOX.Location = new System.Drawing.Point(6, 45);
            this.CharacterVisBOX.Name = "CharacterVisBOX";
            this.CharacterVisBOX.Size = new System.Drawing.Size(796, 21);
            this.CharacterVisBOX.TabIndex = 9;
            this.CharacterVisBOX.Text = "characters\\npc_entities\\main_npc\\lambert.w2ent";
            // 
            // ServerIPText
            // 
            this.ServerIPText.Location = new System.Drawing.Point(6, 19);
            this.ServerIPText.Name = "ServerIPText";
            this.ServerIPText.Size = new System.Drawing.Size(796, 20);
            this.ServerIPText.TabIndex = 8;
            this.ServerIPText.Text = "127.0.0.1:25565";
            // 
            // ConnectServerBTN
            // 
            this.ConnectServerBTN.BackColor = System.Drawing.Color.Tomato;
            this.ConnectServerBTN.BackColorAdditional = System.Drawing.Color.Gray;
            this.ConnectServerBTN.BackColorGradientEnabled = false;
            this.ConnectServerBTN.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.ConnectServerBTN.BorderColor = System.Drawing.Color.Tomato;
            this.ConnectServerBTN.BorderColorEnabled = false;
            this.ConnectServerBTN.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.ConnectServerBTN.BorderColorOnHoverEnabled = false;
            this.ConnectServerBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ConnectServerBTN.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.ConnectServerBTN.ForeColor = System.Drawing.Color.White;
            this.ConnectServerBTN.Location = new System.Drawing.Point(396, 519);
            this.ConnectServerBTN.Name = "ConnectServerBTN";
            this.ConnectServerBTN.RippleColor = System.Drawing.Color.Black;
            this.ConnectServerBTN.RoundingEnable = false;
            this.ConnectServerBTN.Size = new System.Drawing.Size(208, 30);
            this.ConnectServerBTN.TabIndex = 6;
            this.ConnectServerBTN.Text = "ConnectServer";
            this.ConnectServerBTN.TextHover = null;
            this.ConnectServerBTN.UseDownPressEffectOnClick = false;
            this.ConnectServerBTN.UseRippleEffect = true;
            this.ConnectServerBTN.UseVisualStyleBackColor = false;
            this.ConnectServerBTN.UseZoomEffectOnHover = false;
            this.ConnectServerBTN.Click += new System.EventHandler(this.ConnectServerBTN_Click);
            // 
            // CreateServerBTN
            // 
            this.CreateServerBTN.BackColor = System.Drawing.Color.Tomato;
            this.CreateServerBTN.BackColorAdditional = System.Drawing.Color.Gray;
            this.CreateServerBTN.BackColorGradientEnabled = false;
            this.CreateServerBTN.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.CreateServerBTN.BorderColor = System.Drawing.Color.Tomato;
            this.CreateServerBTN.BorderColorEnabled = false;
            this.CreateServerBTN.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.CreateServerBTN.BorderColorOnHoverEnabled = false;
            this.CreateServerBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CreateServerBTN.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.CreateServerBTN.ForeColor = System.Drawing.Color.White;
            this.CreateServerBTN.Location = new System.Drawing.Point(610, 519);
            this.CreateServerBTN.Name = "CreateServerBTN";
            this.CreateServerBTN.RippleColor = System.Drawing.Color.Black;
            this.CreateServerBTN.RoundingEnable = false;
            this.CreateServerBTN.Size = new System.Drawing.Size(210, 30);
            this.CreateServerBTN.TabIndex = 5;
            this.CreateServerBTN.Text = "CreateServer";
            this.CreateServerBTN.TextHover = null;
            this.CreateServerBTN.UseDownPressEffectOnClick = false;
            this.CreateServerBTN.UseRippleEffect = true;
            this.CreateServerBTN.UseVisualStyleBackColor = false;
            this.CreateServerBTN.UseZoomEffectOnHover = false;
            this.CreateServerBTN.Click += new System.EventHandler(this.CreateServerBTN_Click);
            // 
            // SendCmdBTN
            // 
            this.SendCmdBTN.BackColor = System.Drawing.Color.Tomato;
            this.SendCmdBTN.BackColorAdditional = System.Drawing.Color.Gray;
            this.SendCmdBTN.BackColorGradientEnabled = false;
            this.SendCmdBTN.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.SendCmdBTN.BorderColor = System.Drawing.Color.Tomato;
            this.SendCmdBTN.BorderColorEnabled = false;
            this.SendCmdBTN.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.SendCmdBTN.BorderColorOnHoverEnabled = false;
            this.SendCmdBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SendCmdBTN.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.SendCmdBTN.ForeColor = System.Drawing.Color.White;
            this.SendCmdBTN.Location = new System.Drawing.Point(12, 519);
            this.SendCmdBTN.Name = "SendCmdBTN";
            this.SendCmdBTN.RippleColor = System.Drawing.Color.Black;
            this.SendCmdBTN.RoundingEnable = false;
            this.SendCmdBTN.Size = new System.Drawing.Size(378, 30);
            this.SendCmdBTN.TabIndex = 4;
            this.SendCmdBTN.Text = "Chat message";
            this.SendCmdBTN.TextHover = null;
            this.SendCmdBTN.UseDownPressEffectOnClick = false;
            this.SendCmdBTN.UseRippleEffect = true;
            this.SendCmdBTN.UseVisualStyleBackColor = false;
            this.SendCmdBTN.UseZoomEffectOnHover = false;
            this.SendCmdBTN.Click += new System.EventHandler(this.SendCmdBTN_Click);
            // 
            // Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(832, 561);
            this.Controls.Add(this.CMD);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.ConnectServerBTN);
            this.Controls.Add(this.CreateServerBTN);
            this.Controls.Add(this.SendCmdBTN);
            this.Controls.Add(this.LOG);
            this.Name = "Window";
            this.Text = "Window";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox LOG;
        private System.Windows.Forms.TextBox CMD;
        private HuaweiUnlocker.UI.NButton SendCmdBTN;
        private HuaweiUnlocker.UI.NButton CreateServerBTN;
        private HuaweiUnlocker.UI.NButton ConnectServerBTN;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox ServerIPText;
        private System.Windows.Forms.ComboBox CharacterVisBOX;
        private System.Windows.Forms.ComboBox NickNameBOX;
        private System.Windows.Forms.CheckBox TmClDebug;
        private System.Windows.Forms.CheckBox DbgChck;
    }
}