using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.langproc;
using Witcher3_Multiplayer.ClientHost;
using System.Threading;
using System.Text.RegularExpressions;

namespace Witcher3_Multiplayer
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            LOGGERB = LogBoxman;
            MForm = this;
            OverlForm = new SimpleOverlay();
        }

        private async void RunGame_Click(object sender, EventArgs e)
        {
            LogBoxman.Text = "";
            MForm.Hide();
            var a = Task.Run(() =>
            {
                //ServerV2.CreateServer(33299, "COCKFUCK", false);
            });
            await a;
            OverlForm.Show();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {
            if (!IsConnected)
            {
                int port = int.Parse(Regex.Replace(ServerCONPORTtext.Text, "[^0-9]", ""));
                var a = Task.Run(() =>
                {
                    if (!SocketManager.IsConnected())
                    {
                        if (SocketManager.ConnectToGame())
                        {
                            var c = Task.Run(() =>
                            {
                                ClientV2.Connect(NickNameTEXT.Text, CharacterSelectorTEXT.Text, ServerIPCONTEXT.Text, port, false);
                            });
                        }
                    }
                    else
                    {
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(NickNameTEXT.Text, CharacterSelectorTEXT.Text, CharacterSelectorTEXT.Text, port, false);
                        });
                    }
                });
                await a;
            }
        }

        private async void HostBTN_Click(object sender, EventArgs e)
        {
            debug = DebugCheck.Checked;
            TESTMYCLIENT = DebugTestClient.Checked;
            int serverport = int.Parse(Regex.Replace(ServerPORTTEXT.Text, "[^0-9]", ""));
            int serverMAXP = int.Parse(Regex.Replace(MaxPlTEXT.Text, "[^0-9]", ""));
            if (!IsHost)
            {
                var a = Task.Run(() =>
                {
                    if (!SocketManager.IsConnected())
                    {
                        if (SocketManager.ConnectToGame())
                        {
                            var b = Task.Run(() =>
                            {
                                ServerV2.CreateServer(serverport, ServerNAMETEXT.Text, serverMAXP, ConsoleRCON.Checked, ConsolePASSWDText.Text);
                            });
                            Thread.Sleep(200);
                            var c = Task.Run(() =>
                            {
                                ClientV2.Connect(NickNameTEXT.Text, CharacterSelectorTEXT.Text, "127.0.0.1", serverport, ConsoleRCON.Checked);
                            });
                        }
                    }
                    else
                    {
                        var b = Task.Run(() =>
                        {
                            ServerV2.CreateServer(serverport, ServerNAMETEXT.Text, serverMAXP, ConsoleRCON.Checked, ConsolePASSWDText.Text);
                        });
                        Thread.Sleep(200);
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(NickNameTEXT.Text, CharacterSelectorTEXT.Text, "127.0.0.1", serverport, ConsoleRCON.Checked);
                        });
                    }
                });
                await a;
            }
        }

        private async void CmdBTN_Click(object sender, EventArgs e)
        {
            var a = Task.Run(() =>
            {
                if (!SocketManager.IsConnected())
                {
                    if (SocketManager.ConnectToGame())
                        LOG(GameManagerMY.ExecConsoleCommand(CMDb.Text));
                }
                else
                    LOG(GameManagerMY.ExecConsoleCommand(CMDb.Text));
            });
            await a;
        }

        private void SettingsBTN_Click(object sender, EventArgs e)
        {
            SettingsLBOX.Visible = !SettingsLBOX.Visible;
        }
    }
}
