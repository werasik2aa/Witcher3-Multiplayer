using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.langproc;
using Witcher3_Multiplayer.ClientHost;
using System.Threading;

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
                ServerV2.CreateServer(33299, "COCKFUCK", false);
            });
            await a;
            OverlForm.Show();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {

            var a = Task.Run(() =>
            {
                if (!SocketManager.IsConnected())
                {
                    if (SocketManager.ConnectToGame())
                    {
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(IPPORT.Text, 33299, false);
                        });
                    }
                }
                else
                {
                    var c = Task.Run(() =>
                    {
                        ClientV2.Connect(IPPORT.Text, 33299, false);
                    });
                }
            });
            await a;
        }

        private async void HostBTN_Click(object sender, EventArgs e)
        {
            var a = Task.Run(() =>
            {
                if (!SocketManager.IsConnected())
                {
                    if (SocketManager.ConnectToGame())
                    {
                        var b = Task.Run(() =>
                    {
                        ServerV2.CreateServer(33299, "COCKFUCK", false);
                    });
                        Thread.Sleep(200);
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect("127.0.0.1", 33299, false);
                        });
                    }
                }
                else
                {
                    var b = Task.Run(() =>
                    {
                        ServerV2.CreateServer(33299, "COCKFUCK", false);
                    });
                    Thread.Sleep(200);
                    var c = Task.Run(() =>
                    {
                        ClientV2.Connect("127.0.0.1", 33299, false);
                    });
                }
            });
            await a;
        }

        private async void CmdBTN_Click(object sender, EventArgs e)
        {
            var a = Task.Run(() =>
            {
                if (!SocketManager.IsConnected())
                    if (SocketManager.ConnectToGame())
                        LOG(GameManagerMY.ExecConsoleCommand(CMDb.Text));
                else
                    LOG(GameManagerMY.ExecConsoleCommand(CMDb.Text));
            });
            await a;
        }
    }
}
