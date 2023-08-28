using HuaweiUnlocker.UI;
using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using Witcher3_Multiplayer.ClientHost.Data;
using Witcher3_Multiplayer.Game;
using Witcher3_Multiplayer.Overlay;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;

namespace Witcher3_Multiplayer
{
    public partial class Main : Form
    {
        private const UInt32 StdOutputHandle = 0xFFFFFFF5;
        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(UInt32 nStdHandle);
        [DllImport("kernel32.dll")]
        private static extern void SetStdHandle(UInt32 nStdHandle, IntPtr handle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
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
        public Main()
        {
            InitializeComponent();
            LOGGERB = LogBoxman;
            MForm = this;
            foreach (var a in NpcsPlayer)
                CharacterSelectorTEXT.Items.Add(a);
            CharacterSelectorTEXT.SelectedIndex = 1;
            if (!Directory.Exists("Data") || !File.Exists("Data\\Settings.bin")) SaveData();
        }
        private void RunGame_Click(object sender, EventArgs e)
        {
            SaveData();
            LOG("Waiting For game Net Server");
            var process = System.Diagnostics.Process.GetProcessesByName("witcher3").FirstOrDefault();
            if (process == null)
            {
                GameManagerMY.RunGame(DataAPP.GamePath);
                int cur = 30;
                while (!SocketV2.ConnectToGame())
                {
                    if (cur-- > 0)
                        Thread.Sleep(1000);
                    else
                        ELOG("Connect game time out");
                }
            }
            else SocketV2.ConnectToGame();
            if (SocketV2.IsConneted())
                LOG("Connected To Game");
        }
        public static void CreateConsole()
        {
            AllocConsole();
            IntPtr defaultStdout = new IntPtr(7);
            IntPtr currentStdout = GetStdHandle(StdOutputHandle);
            if (currentStdout != defaultStdout)
                SetStdHandle(StdOutputHandle, defaultStdout);
            TextWriter writer = new StreamWriter(Console.OpenStandardOutput())
            { AutoFlush = true };
            Console.SetOut(writer);
        }
        public void Connect_ActionTest()
        {
            if (IsHost)
            {
                LOG("Your hosted server Stopped");
                ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
                return;
            }  
            if (!IsConnected)
            {
                DataAPP.ClientPort = DataAPP.ServerPort;
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
                        else LOG("[client] Failed to connect to game is it launched?");
                    }
                    else
                    {
                        var c = Task.Run(() =>
                        {
                            ClientV2.Connect(DataAPP.NickName, DataAPP.character, DataAPP.ServerIP, DataAPP.ServerPort, DataAPP.ClientRCON);
                        });
                    }
                });
            }
            else
                ClientV2.Disconnect();
        }

        public void Host_ActionTest()
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
                            if(!DataAPP.ServerDedicated)
                                ClientV2.Connect(DataAPP.NickName, DataAPP.character, "127.0.0.1", DataAPP.ServerPort, DataAPP.ClientRCON);
                        } else
                            LOG("[client] Failed to Connect to game is it launched?");
                    }
                    else
                    {
                        var b = Task.Run(() =>
                        {
                            ServerV2.CreateServer(DataAPP.ServerPort, DataAPP.ServerIP, DataAPP.ServerMaxPlayers, DataAPP.ServerRCON, DataAPP.ServerConsolePassword);
                        });
                        Thread.Sleep(200);
                        if (!DataAPP.ServerDedicated)
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
                    ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_COMMANDORCHATMSG, CMDb.Text);
                else
                {
                    if (!SocketV2.IsConneted())
                    {
                        if (SocketV2.ConnectToGame())
                            LOG(ClientCommandHandler.CommandLST(ClientV2.UDP_CLIENT, CMDb.Text, CMDb.Text.GetArgs()));
                    } else
                        LOG(ClientCommandHandler.CommandLST(ClientV2.UDP_CLIENT, CMDb.Text, CMDb.Text.GetArgs()));
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

        private void GamePathBTN_Click(object sender, EventArgs e)
        {
            var fbd = new OpenFileDialog();
            if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.FileName)) {
                var tsplit = fbd.FileName.Split('\\');
                GamePathTXT.Text = tsplit[0];
                for (int a = 1; a < tsplit.Length-3; a++) GamePathTXT.Text += "\\" + tsplit[a];
            }
        }

        private void ConBTN_Click(object sender, EventArgs e)
        {
            Connect_ActionTest();
        }

        private void HostBTN_Click(object sender, EventArgs e)
        {
            Host_ActionTest();
        }
    }
}
