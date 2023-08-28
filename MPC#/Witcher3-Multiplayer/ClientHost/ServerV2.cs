using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
                ELOG("[client] NEED A PASSWORD! Because without password server easy to hack");
                return;
            }
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
            LOG("[host] Server Created!");
            while (true)
            {
                if (!SocketV2.IsConneted() & !DataAPP.ServerDedicated)
                {
                    ELOG("[host] Game closed or server closed");
                    break;
                }
                var result = await UDP_SERVER.ReceiveAsync();
                OperateWithData(result.Buffer, result.RemoteEndPoint);
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
                        }
                        HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERINFO, iiii.ToByteArray());
                        break;
                    case RecvSendTypes.SND_COMMANDORCHATMSG:
                        var cmd = Encoding.UTF8.GetString(recvdata);
                        var args = cmd.GetArgs();
                        var SCL = ServerData.Find(x => x.ClientID == IDClient);
                        if (DataAPP.Debug) LOG("[host] Receive Message: " + cmd);
                        if (cmd.StartsWith("//"))
                           HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_COMMANDRESPONSE, 
                               ServerCommandHandler.CommandLST(UDP_SERVER, 
                               fromclie, 
                               SCL.ISAdmin || SCL.ISConsole,
                               args.Length > 0 ? args[0] : cmd, args));
                        else
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_CHATRESPONSE, cmd);
                        break;
                    case RecvSendTypes.RET_CONNECTED:
                        LOG("[host] New Client wants to connect");
                        ServerData.Add(new PlayerServer()
                        {
                            ClientID = host_data.CurPlayers,
                            ISAdmin = true,
                            ISConsole = false
                        });
                        LOG("[host] Sending ServerINFO to new client");
                        HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_HOSTINFO, host_data.ToByteArray());
                        
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
                        if (PlayerDataServer.ContainsKey(fromclie))
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
            UDP_SERVER.Close();
            //Server closed packet?
            UDP_SERVER.Dispose();
            IsHost = false;
        }
    }
}
