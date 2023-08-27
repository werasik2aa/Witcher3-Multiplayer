using HuaweiUnlocker.UI;
using System;
using System.IO;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using Witcher3_Multiplayer.Game;
using Witcher3_Multiplayer.Overlay;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;

namespace Witcher3_Multiplayer
{
    public partial class Main : Form
    {
        private void OnApplicationExit(object sender, EventArgs e)
        {
            if (IsConnected)
                ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
            SaveData();
        }
        public void SaveData()
        {
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");
            DataAPP = new Settings()
            {
                character = CharacterSelectorTEXT.Text,
                NickName = NickNameTEXT.Text,
                ServerIP = ServerIPCONTEXT.Text,
                ServerPort = int.Parse(Regex.Replace(ServerCONPORTtext.Text, "[^0-9]", "")),
                ServerMaxPlayers = int.Parse(Regex.Replace(MaxPlTEXT.Text, "[^0-9]", "")),
                ServerPassword = ServerPASSText.Text,
                ServerName = ServerNAMETEXT.Text,
                ServerConsolePassword = ConsolePASSWDText.Text,
                ServerSyncChestItems = SyncChestCONTFlag.Checked,
                Debug = DebugCheck.Checked,
                ClientRCON = false,
                ServerDedicated = DedicatedFlag.Checked,
                ServerRCON = ConsoleRCON.Checked,
                ServerCheckClientVersion = CheckClientSTATEFlag.Checked,
                GamePath = GamePathTXT.Text,
                JoinTestLocalClient = DebugTestClient.Checked
            };
            DataAPP.SeriliazeToFile("Data\\Settings.bin");
        }
        public Main()
        {
            InitializeComponent();
            LOGGERB = LogBoxman;
            MForm = this;
            foreach (var a in NpcsPlayer)
                CharacterSelectorTEXT.Items.Add(a);
            CharacterSelectorTEXT.SelectedIndex = 1;
            if (!Directory.Exists("Data") || !File.Exists("Data\\Settings.bin")) SaveData();
            DataAPP = FileToStructure<Settings>("Data\\Settings.bin");
            NickNameTEXT.Text = DataAPP.NickName;
            CharacterSelectorTEXT.Text = DataAPP.character;
            MaxPlTEXT.Text = DataAPP.ServerMaxPlayers.ToString();
            ServerCONPORTtext.Text = DataAPP.ServerPort.ToString();
            DebugCheck.Checked = DataAPP.Debug;
            DedicatedFlag.Checked = DataAPP.ServerDedicated;
            ConsoleRCON.Checked = DataAPP.ServerRCON;
            ConsolePASSWDText.Text = DataAPP.ServerConsolePassword;
            SyncChestCONTFlag.Checked = DataAPP.ServerSyncChestItems;
            CheckClientSTATEFlag.Checked = DataAPP.ServerCheckClientVersion;
            DebugTestClient.Checked = DataAPP.JoinTestLocalClient;
            ServerNAMETEXT.Text = DataAPP.ServerName;
            GamePathTXT.Text = DataAPP.GamePath;
        }
        private void RunGame_Click(object sender, EventArgs e)
        {
            GameManagerMY.RunGame("G:\\The Witcher 3 Wild Hunt");
            LOG("Waiting For game Net Server");
            int cur = 0;
            while (!SocketV2.ConnectToGame())
            {
                if (30 < cur++) {
                    LOG("Game Failed to launch?");
                    return;
                }
                Thread.Sleep(1000);
            }
            if(SocketV2.IsConneted())
                LOG("Connected");
        }

        private void Connect_Click(object sender, EventArgs e)
        {
            if (IsHost)
            {
                LOG("Your hosted server Stopped");
                ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
                return;
            }  
            if (!IsConnected)
            {
                SaveData();
                var a = Task.Run(() =>
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                        {
                            var c = Task.Run(() =>
                            {
                                ClientV2.Connect(DataAPP.NickName, DataAPP.character, DataAPP.ServerIP, DataAPP.ServerPort, DataAPP.ClientRCON);
                            });
                        }
                    }
                    else
                    {
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(DataAPP.NickName, DataAPP.character, DataAPP.ServerIP, DataAPP.ServerPort, DataAPP.ClientRCON);
                        });
                    }
                });
                NButton aaf = (NButton)sender;
                aaf.Text = "Disconnect";
            }
            else
                ClientV2.Disconnect();
        }

        private void HostBTN_Click(object sender, EventArgs e)
        {

            if (!IsHost)
            {
                SaveData();
                var a = Task.Run(() =>
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                        {
                            var b = Task.Run(() =>
                            {
                                ServerV2.CreateServer(DataAPP.ServerPort, DataAPP.ServerIP, DataAPP.ServerMaxPlayers, DataAPP.ServerRCON, DataAPP.ServerConsolePassword);
                            });
                            Thread.Sleep(200);
                            ClientV2.Connect(DataAPP.NickName, DataAPP.character, "127.0.0.1", DataAPP.ServerPort, DataAPP.ClientRCON);
                        }
                    }
                    else
                    {
                        var b = Task.Run(() =>
                        {
                            ServerV2.CreateServer(DataAPP.ServerPort, DataAPP.ServerIP, DataAPP.ServerMaxPlayers, DataAPP.ServerRCON, DataAPP.ServerConsolePassword);
                        });
                        Thread.Sleep(200);
                        ClientV2.Connect(DataAPP.NickName, DataAPP.character, "127.0.0.1", DataAPP.ServerPort, DataAPP.ClientRCON);
                    }
                });
            }
        }

        private void CmdBTN_Click(object sender, EventArgs e)
        {
            var a = Task.Run(() =>
            {
                if (IsConnected)
                    ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_COMMAND, CMDb.Text);
                else
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                            LOG("Exec: " + GameManagerMY.ExecConsoleCommand(CMDb.Text));
                    } else
                        LOG("Exec: " + GameManagerMY.ExecConsoleCommand(CMDb.Text));
                }
            });
        }

        private void SettingsBTN_Click(object sender, EventArgs e)
        {
            SettingsLBOX.Visible = !SettingsLBOX.Visible;
        }

        private void SaveServerData_Tick(object sender, EventArgs e)
        {
            ServerData.SeriliazeToFile("Data\\PlayerData");
        }
    }
}
