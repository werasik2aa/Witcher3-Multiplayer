using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using Witcher3_Multiplayer.Game;
using Witcher3_Multiplayer.GameData;
using static Witcher3_Multiplayer.Langproc;

namespace Witcher3_Multiplayer
{
    public partial class Window : Form
    {
        public Window()
        {
            InitializeComponent();
            LOGGER = LOG;
            DataTemplates.NpcsPlayer.ForEach(x => CharacterVisBOX.Items.Add(x));
        }

        private void CreateServerBTN_Click(object sender, EventArgs e)
        {
            TestLocalClient = TmClDebug.Checked;
            debug = DbgChck.Checked;
            string template = CharacterVisBOX.Text;
            string nick = NickNameBOX.Text;
            if (!SocketV2.IsConneted()) SocketV2.ConnectToGame();
            if (SocketV2.IsConneted())
            {
                Task.Run(() =>
                {
                    Server.CreateServer(25565);
                });
                Thread.Sleep(1000);
                Task.Run(() =>
                {
                    Client.Connect("127.0.0.1", 25565, template, nick);
                });
            }
        }

        private void ConnectServerBTN_Click(object sender, EventArgs e)
        {
            debug = DbgChck.Checked;
            TestLocalClient = TmClDebug.Checked;
            string template = CharacterVisBOX.Text;
            var ipport = ServerIPText.Text.Split(':');
            string ip = ipport[0];
            string port = ipport[1];
            string nick = NickNameBOX.Text;
            if (IsHost)
            {
                LOG(2, "[GameSi] You can't create local player client on server from here");
                return;
            }
            if (!SocketV2.IsConneted()) SocketV2.ConnectToGame();
            if (SocketV2.IsConneted())
            {
                Task.Run(() =>
                {
                    Client.Connect(ip, int.Parse(port), template, nick);
                });
            }
        }

        private void SendCmdBTN_Click(object sender, EventArgs e)
        {
            if (!SocketV2.IsConneted()) SocketV2.ConnectToGame();
            if (CMD.Text.StartsWith("/"))
            {
                if (SocketV2.IsConneted())
                    LOG(0, "[CMD] RESP: " + SocketV2.ReadRaw(Convertors.Execute(CMD.Text.Substring(1))));
            }
            else
            {
                LOG(0, Client.MyNickName + ": " + CMD.Text);
                if (IsConnected)
                    ClientHandler.SendData(PacketType.SND_CHATMSG, Encoding.ASCII.GetBytes(CMD.Text));
                else
                    LOG(2, "Message Not delivered to host. You're not connected");
            }
        }
    }
}
