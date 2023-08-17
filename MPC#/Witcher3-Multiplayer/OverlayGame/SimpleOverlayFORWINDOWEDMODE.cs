using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer
{
    public partial class SimpleOverlayFORWINDOWEDMODE : Form
    {
        public bool TimerPass = false;
        public SimpleOverlayFORWINDOWEDMODE()
        {
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "";
            InitializeComponent();
            this.KeyPreview = true;
            this.Opacity = 0f;
            this.TransparencyKey = Color.Magenta;
            this.BackColor = Color.Magenta;
            this.StartPosition = FormStartPosition.Manual;
            this.Left = 0;
            this.ControlBox = false;
            this.ShowInTaskbar = false;
            this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height;
            TimerPass = DisappearChat.Enabled = true;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public void SHOW()
        {
            this.Opacity = 0.9f;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x00);
            TimerPass = false;
        }
        public void HIDE()
        {
            this.Opacity = 0.2f;
            int initialStyle = GetWindowLong(this.Handle, -20);
            SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
            TimerPass = true;
        }
        public void SHOWDELAYED()
        {
            Action f = () =>
            {
                this.Opacity = 0.9f;
                int initialStyle = GetWindowLong(this.Handle, -20);
                SetWindowLong(this.Handle, -20, initialStyle | 0x80000 | 0x20);
                TimerPass = true;
            };
            if (TCONTROL.InvokeRequired)
                TCONTROL.Invoke(f);
            else f();
        }
        private async void ConnectBTN_Click(object sender, EventArgs e)
        {
            if (!IsConnected)
            {
                TESTMYCLIENT = DebugTestClient.Checked;
                int port = int.Parse(Regex.Replace(ServerPORTTEXT.Text, "[^0-9]", ""));
                string charact = MForm.CharacterSelectorTEXT.Text;
                var a = Task.Run(() =>
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                        {
                            var c = Task.Run(() =>
                            {
                                ClientV2.Connect(MForm.NickNameTEXT.Text, charact, ServerIPCONTEXT.Text, port, false);
                            });
                        }
                    }
                    else
                    {
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(MForm.NickNameTEXT.Text, charact, ServerIPCONTEXT.Text, port, false);
                        });
                    }
                });
                await a;
            }
        }
        private async void HostBtn_Click(object sender, EventArgs e)
        {
            debug = DebugCheck.Checked;
            Dedicated = DedicatedFlag.Checked;
            TESTMYCLIENT = DebugTestClient.Checked;
            int serverport = int.Parse(Regex.Replace(ServerPORTTEXT.Text, "[^0-9]", ""));
            int serverMAXP = int.Parse(Regex.Replace(MaxPlTEXT.Text, "[^0-9]", ""));
            if (!IsHost)
            {
                var player = MForm.CharacterSelectorTEXT.Text;
                var a = Task.Run(() =>
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                        {
                            var b = Task.Run(() =>
                            {
                                ServerV2.CreateServer(serverport, ServerNAMETEXT.Text, serverMAXP, ConsoleRCON.Checked, PasswordServ.Text);
                            });
                            Thread.Sleep(200);
                            var c = Task.Run(() =>
                            {
                                ClientV2.Connect(MForm.NickNameTEXT.Text, player, "127.0.0.1", serverport, ConsoleRCON.Checked);
                            });
                        }
                    }
                    else
                    {
                        var b = Task.Run(() =>
                        {
                            ServerV2.CreateServer(serverport, ServerNAMETEXT.Text, serverMAXP, ConsoleRCON.Checked, PasswordServ.Text);
                        });
                        Thread.Sleep(200);
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(MForm.NickNameTEXT.Text, player, "127.0.0.1", serverport, ConsoleRCON.Checked);
                        });
                    }
                });
                await a;
            }
        }
        private void SimpleOverlay_KeyDown(object sender, KeyEventArgs e)
        {
            var press = e.KeyCode;
            if (press == Keys.Enter & SocketV2.IsConneted() & IsConnected)
            {
                if (!string.IsNullOrEmpty(ChatCMDBOX.Text) & TCONTROL.SelectedIndex == 0)
                {
                    if (ChatCMDBOX.Text.StartsWith("/"))
                    {
                        LOG(GameManagerMY.ExecConsoleCommand(ChatCMDBOX.Text.Trim()));
                    }
                    else
                    {
                        LOG(MForm.NickNameTEXT.Text + ": " + ChatCMDBOX.Text.Substring(1).Trim());
                        ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_CHATMSG, Encoding.UTF8.GetBytes(ChatCMDBOX.Text.Trim()));
                    }
                    ChatCMDBOX.Text = "";
                }
            }
        }

        private void DisappearChat_Tick(object sender, EventArgs e)
        {
            if (TimerPass)
                this.Opacity -= 0.01f;
        }
    }
}