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
                Host.CreateServer("127.0.0.1", 33299, false);
            });
            await a;
            OverlForm.Show();
        }

        private async void Connect_Click(object sender, EventArgs e)
        {

            var a = Task.Run(() =>
            {
                if (!SocketManager.IsConnected()) {
                    if (SocketManager.ConnectToGame())
                    {
                        SocketManager.ConnectToGame();
                        Client.ConnectToServer("127.0.0.1", 33299, false);
                    }
                }
            });
            await a;
        }

        private async void HostBTN_Click(object sender, EventArgs e)
        {
            var a = Task.Run(() =>
            {
                SocketManager.ConnectToGame();
                Host.CreateServer("127.0.0.1", 33299, false);
                Thread.Sleep(200);
                Client.ConnectToServer("127.0.0.1", 33299, false);
            });
            await a;
        }
    }
}
