using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost.Data;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class ServerV2
    {
        private static UdpClient UDP_SERVER;
        private static ServerInfo host_data;
        private static string ServerName = "EBalo";
        private static string Password = "NET";
        private static bool CommandShell = false;
        private static int MaxPlayers = 1;
        public async static void CreateServer(int port, string Name, int MaxP, bool RCON, string password = "")
        {
            if (!SocketV2.IsConneted() & !DataAPP.ServerDedicated)
            {
                ELOG("[host] Please start game first!");
                return;
            }
            if (IsHost)
            {
                ELOG("[host] Already Server Runned!");
                return;
            }
            if (IsConnected)
            {
                LOG("[host] Please Disconnect from server, before create new server. OR MANY BUGS!");
                return;
            }
            if (!DataAPP.ServerDedicated & GameManagerUI.IsGameNotLaunched())
            {
                ELOG("YOU_CAN'T HOST GAME IN MAIN MENU! PLEASE load a save");
                return;
            }
            if (RCON && string.IsNullOrEmpty(password))
            {
                ELOG("[host] NEED A PASSWORD! Because without password server easy to hack");
                return;
            }
            LOG("[host] Totall DEBUG: " + DataAPP.JoinTestLocalClient);
            LOG("[host] Creating Server...");
            MaxPlayers = MaxP;
            CommandShell = RCON;
            Password = password;
            ServerName = Name;
            IsHost = true;
            UDP_SERVER = new UdpClient(port);
            host_data = new ServerInfo()
            {
                Name = ServerName,
                MaxPlayers = MaxPlayers,
                CurPlayers = 0,
                Version = VersionCur,
                RequireCheckVersion = DataAPP.ServerCheckClientVersion
            };
            if (File.Exists("Data\\ServerData.bin"))
            {
                LOG("[host] Loading saved ServerData...");
                ServerData = (Dictionary<int, PlayerServer>)FileToObject("Data\\ServerData.bin");
                LOG("[host] Loaded " + ServerData.Count + " entries from server data");
                if (!DataAPP.ServerDedicated && DataAPP.NickName != ServerData[0].NickName)
                {
                    LOG("[host] Replacing Default");
                    ServerData[0] = new PlayerServer()
                    {
                        NickName = DataAPP.NickName,
                        ISAdmin = true,
                    };
                    ServerData.SeriliazeToFile("Data\\PlayerData\\ServerData.bin");
                }
            }
            else if (!DataAPP.ServerDedicated)
            {
                LOG("[host] Creating new Server Data with admin right of hoster");
                ServerData.Add(0, new PlayerServer()
                {
                    NickName = DataAPP.NickName,
                    ISAdmin = true,
                });
                ServerData.SeriliazeToFile("Data\\ServerData.bin");
            }
            LOG("[host] Server Created!");
            while (true)
            {
                if (!SocketV2.IsConneted() & !DataAPP.ServerDedicated)
                {
                    ELOG("[host] Game closed or server closed");
                    break;
                }
                UdpReceiveResult result;
                try
                {
                    result = await UDP_SERVER.ReceiveAsync();
                    OperateWithData(result.Buffer, result.RemoteEndPoint);
                }
                catch (ObjectDisposedException msg)
                {
                    if (result.RemoteEndPoint != null && PlayerDataServer.ContainsKey(result.RemoteEndPoint))
                    {
                        if (DataAPP.Debug) LOG("EC: " + msg);
                            LOG("[host] ===Connection Lost===");
                            LOG("[host] PlayerID: " + PlayerDataServer[result.RemoteEndPoint].ID);
                            LOG("[host] PlayerName: " + PlayerDataServer[result.RemoteEndPoint].NickName);
                        PlayerDataServer.Remove(result.RemoteEndPoint);
                    }
                }
            }
        }

        public static void OperateWithData(byte[] data, IPEndPoint fromclie)
        {
            if (data != null & data.Length >= 4)
            {
                byte[] header4 = data.Take(4).ToArray();
                int ahead = BitConverter.ToInt16(header4, 0);
                byte[] recvdata = data.Skip(4).ToArray();
                int IDClient = PlayerDataServer.ContainsKey(fromclie) ? PlayerDataServer[fromclie].ID : -1;
                RecvSendTypes head = (RecvSendTypes)ahead;

                switch (head)
                {
                    case RecvSendTypes.RET_ACCESS:
                        HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_ACCESSSHELL, BitConverter.GetBytes(CommandShell && Password == Encoding.UTF8.GetString(recvdata)));
                        break;
                    case RecvSendTypes.SND_PLAYERINFO:
                        if (DataAPP.Debug) LOG("[host] Decrypting PlayerInfo");
                        PlayerData iiii = recvdata.ToStructure<PlayerData>();
                        if (!PlayerDataServer.ContainsKey(fromclie))
                        {
                            LOG("[host] ===Joined===");
                            LOG("[host] Player ID: " + iiii.ID);
                            LOG("[host] NickName: " + iiii.NickName);
                            LOG("[host] HP: " + iiii.HP);
                            LOG("[host] LevelID: " + iiii.AreaID);
                            LOG("[host] Plevel: " + iiii.Plevel);
                            PlayerDataServer.Add(fromclie, iiii);
                            LOG("[host] CurrentPlayers: " + (host_data.CurPlayers = PlayerDataServer.Count)); //SET CURRENT PLAYERS
                            if (PlayerDataServer.Count > 1)
                                LOG("[host] Resend Connected Player INFO to All");

                            if (!ServerData.ContainsKey(iiii.ID))
                            {
                                LOG("[host] Saving Player state to FILE");
                                ServerData.Add(iiii.ID, new PlayerServer()
                                {
                                    NickName = iiii.NickName,
                                    ISAdmin = false,
                                });
                                ServerData.SeriliazeToFile("Data\\ServerData.bin");
                            }
                        }
                        HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERINFO, iiii.ToByteArray());
                        break;
                    case RecvSendTypes.SND_COMMANDORCHATMSG:
                        var cmd = Encoding.UTF8.GetString(recvdata);
                        var args = cmd.GetArgs();
                        var SCL = ServerData[IDClient];
                        if (DataAPP.Debug) LOG("[host] Receive Message: " + cmd);
                        if (cmd.StartsWith("//"))
                            HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_COMMANDRESPONSE,
                                ServerCommandHandler.CommandLST(UDP_SERVER,
                                fromclie,
                                SCL.ISAdmin,
                                args.Length > 0 ? args[0] : cmd, args));
                        else
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_CHATRESPONSE, cmd);
                        break;
                    case RecvSendTypes.RET_CONNECTED:
                        LOG("[host] Found New Client! Sending Server Info Response!");
                        string ClientName = Encoding.UTF8.GetString(recvdata);
                        var host_datan = host_data; //Set If exist or no
                        if(PlayerDataServer.Any(x => x.Value.NickName == ClientName) && !DataAPP.JoinTestLocalClient)
                            host_datan.MaxPlayers = -1;
                        HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_HOSTINFO, host_datan.ToByteArray());
                        break;
                    case RecvSendTypes.SND_DISCONNECTED:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            LOG("[host] ===Disconnected===");
                            LOG("[host] PlayerID: " + IDClient);
                            LOG("[host] PlayerName: " + PlayerDataServer[fromclie].NickName);
                            PlayerDataServer.Remove(fromclie);
                        }
                        HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_DISCONNECTED);
                        break;
                    case RecvSendTypes.RET_PLAYERDATAS:
                        if (PlayerDataServer.ContainsKey(fromclie) & PlayerDataServer.Count > 1)
                        {
                            if (DataAPP.Debug) LOG("[host] Client ret All player Dats: ");
                            foreach (var o in PlayerDataServer)
                            {
                                if (DataAPP.Debug) LOG("[host] Sending data of Player: " + o.Value.NickName + " : " + o.Value.ID + " To PlayerID: " + IDClient);
                                HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERINFO, o.Value.ToByteArray());
                            }
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERPOSITION:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var a = PlayerDataServer[fromclie];
                            a.PlayerPosition = recvdata.ToStructure<Vector3>();
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERPOSITION, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERHORSEPOSITION:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var a = PlayerDataServer[fromclie];
                            a.HorsePosition = recvdata.ToStructure<Vector3>();
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERHORSEPOSITION, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERONHORSE:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            if (DataAPP.Debug) LOG("[host] Resend Horse State of playerID" + IDClient);
                            var a = PlayerDataServer[fromclie];
                            a.IsOnHorse = BitConverter.ToBoolean(recvdata, 0);
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERONHORSE, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERCOMBATTARGET:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var a = PlayerDataServer[fromclie];
                            a.CurrentTarget = recvdata.ToStructure<CombatTarget>();
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERCOMBATTARGET, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERHP:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var a = PlayerDataServer[fromclie];
                            a.HP = BitConverter.ToUInt16(recvdata, 0);
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERHP, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERLEVEL:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var a = PlayerDataServer[fromclie];
                            a.Plevel = BitConverter.ToUInt16(recvdata, 0);
                            PlayerDataServer[fromclie] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERLEVEL, recvdata);
                        }
                        break;
                }
            }
        }
        public static void StopServer()
        {
            PlayerDataServer.Clear();
            ServerData.Clear();
            ClientV2.UDP_CLIENT.Close();
            ClientV2.UDP_CLIENT.Dispose();
            UDP_SERVER.Close();
            UDP_SERVER.Dispose();
            IsConnected = IsHost = false;
        }
    }
}
