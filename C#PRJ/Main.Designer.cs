namespace Witcher3_Multiplayer
{
    partial class Main
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.BURG = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.LogBoxman = new System.Windows.Forms.TextBox();
            this.CMDb = new System.Windows.Forms.TextBox();
            this.Connect = new HuaweiUnlocker.UI.NButton();
            this.RunGame = new HuaweiUnlocker.UI.NButton();
            this.HostBTN = new HuaweiUnlocker.UI.NButton();
            this.BURG.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // BURG
            // 
            this.BURG.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.BURG.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.BURG.Controls.Add(this.pictureBox1);
            this.BURG.Location = new System.Drawing.Point(-1, -1);
            this.BURG.Name = "BURG";
            this.BURG.Size = new System.Drawing.Size(74, 553);
            this.BURG.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-2, -2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(74, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // LogBoxman
            // 
            this.LogBoxman.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(40)))));
            this.LogBoxman.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.LogBoxman.ForeColor = System.Drawing.SystemColors.Window;
            this.LogBoxman.Location = new System.Drawing.Point(78, 12);
            this.LogBoxman.Multiline = true;
            this.LogBoxman.Name = "LogBoxman";
            this.LogBoxman.ReadOnly = true;
            this.LogBoxman.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.LogBoxman.Size = new System.Drawing.Size(1035, 453);
            this.LogBoxman.TabIndex = 2;
            this.LogBoxman.Text = "dfhdfh";
            // 
            // CMDb
            // 
            this.CMDb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(26)))), ((int)(((byte)(40)))));
            this.CMDb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.CMDb.ForeColor = System.Drawing.SystemColors.MenuBar;
            this.CMDb.Location = new System.Drawing.Point(79, 471);
            this.CMDb.Multiline = true;
            this.CMDb.Name = "CMDb";
            this.CMDb.Size = new System.Drawing.Size(1035, 22);
            this.CMDb.TabIndex = 4;
            // 
            // Connect
            // 
            this.Connect.BackColor = System.Drawing.Color.Tomato;
            this.Connect.BackColorAdditional = System.Drawing.Color.Gray;
            this.Connect.BackColorGradientEnabled = false;
            this.Connect.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.Connect.BorderColor = System.Drawing.Color.Tomato;
            this.Connect.BorderColorEnabled = false;
            this.Connect.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.Connect.BorderColorOnHoverEnabled = false;
            this.Connect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Connect.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.Connect.ForeColor = System.Drawing.Color.White;
            this.Connect.Location = new System.Drawing.Point(276, 499);
            this.Connect.Name = "Connect";
            this.Connect.RippleColor = System.Drawing.Color.Black;
            this.Connect.RoundingEnable = false;
            this.Connect.Size = new System.Drawing.Size(191, 38);
            this.Connect.TabIndex = 5;
            this.Connect.Text = "ConnectToSrv";
            this.Connect.TextHover = null;
            this.Connect.UseDownPressEffectOnClick = false;
            this.Connect.UseRippleEffect = true;
            this.Connect.UseVisualStyleBackColor = false;
            this.Connect.UseZoomEffectOnHover = false;
            this.Connect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // RunGame
            // 
            this.RunGame.BackColor = System.Drawing.Color.Tomato;
            this.RunGame.BackColorAdditional = System.Drawing.Color.Gray;
            this.RunGame.BackColorGradientEnabled = false;
            this.RunGame.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.RunGame.BorderColor = System.Drawing.Color.Tomato;
            this.RunGame.BorderColorEnabled = false;
            this.RunGame.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.RunGame.BorderColorOnHoverEnabled = false;
            this.RunGame.Cursor = System.Windows.Forms.Cursors.Hand;
            this.RunGame.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.RunGame.ForeColor = System.Drawing.Color.White;
            this.RunGame.Location = new System.Drawing.Point(674, 499);
            this.RunGame.Name = "RunGame";
            this.RunGame.RippleColor = System.Drawing.Color.Black;
            this.RunGame.RoundingEnable = false;
            this.RunGame.Size = new System.Drawing.Size(439, 38);
            this.RunGame.TabIndex = 3;
            this.RunGame.Text = "nButton1";
            this.RunGame.TextHover = null;
            this.RunGame.UseDownPressEffectOnClick = false;
            this.RunGame.UseRippleEffect = true;
            this.RunGame.UseVisualStyleBackColor = false;
            this.RunGame.UseZoomEffectOnHover = false;
            this.RunGame.Click += new System.EventHandler(this.RunGame_Click);
            // 
            // HostBTN
            // 
            this.HostBTN.BackColor = System.Drawing.Color.Tomato;
            this.HostBTN.BackColorAdditional = System.Drawing.Color.Gray;
            this.HostBTN.BackColorGradientEnabled = false;
            this.HostBTN.BackColorGradientMode = System.Drawing.Drawing2D.LinearGradientMode.Horizontal;
            this.HostBTN.BorderColor = System.Drawing.Color.Tomato;
            this.HostBTN.BorderColorEnabled = false;
            this.HostBTN.BorderColorOnHover = System.Drawing.Color.Tomato;
            this.HostBTN.BorderColorOnHoverEnabled = false;
            this.HostBTN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.HostBTN.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.HostBTN.ForeColor = System.Drawing.Color.White;
            this.HostBTN.Location = new System.Drawing.Point(79, 499);
            this.HostBTN.Name = "HostBTN";
            this.HostBTN.RippleColor = System.Drawing.Color.Black;
            this.HostBTN.RoundingEnable = false;
            this.HostBTN.Size = new System.Drawing.Size(191, 38);
            this.HostBTN.TabIndex = 6;
            this.HostBTN.Text = "HostSrv";
            this.HostBTN.TextHover = null;
            this.HostBTN.UseDownPressEffectOnClick = false;
            this.HostBTN.UseRippleEffect = true;
            this.HostBTN.UseVisualStyleBackColor = false;
            this.HostBTN.UseZoomEffectOnHover = false;
            this.HostBTN.Click += new System.EventHandler(this.HostBTN_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(36)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(1125, 549);
            this.Controls.Add(this.HostBTN);
            this.Controls.Add(this.Connect);
            this.Controls.Add(this.CMDb);
            this.Controls.Add(this.RunGame);
            this.Controls.Add(this.LogBoxman);
            this.Controls.Add(this.BURG);
            this.Name = "Main";
            this.Text = "Form1";
            this.BURG.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel BURG;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox LogBoxman;
        private HuaweiUnlocker.UI.NButton RunGame;
        private System.Windows.Forms.TextBox CMDb;
        private HuaweiUnlocker.UI.NButton Connect;
        private HuaweiUnlocker.UI.NButton HostBTN;
    }
}

