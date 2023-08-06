using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
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
        private static int MaxP = 5340;
        private static string Password = "NET";
        private static bool CommandShell = false;
        private static bool SV_CHEATS = false;
        public async static void CreateServer(int port, string Name, bool RCON, string password = "")
        {
            if (!SocketManager.GameSocket.Connected)
            {
                LOG("[host] Please start game first!");
                return;
            }
            if (IsHost)
            {
                LOG("[host] Already Server Runned!");
                return;
            }
            if (IsConnected)
            {
                LOG("[host] Please Disconnect from server, before create new server. OR MANY BUGS!");
                // return;
            }
            if (RCON && string.IsNullOrEmpty(password))
            {
                LOG("[client] NEED A PASSWORD!");
                return;
            }
            LOG("Creating Server");
            CommandShell = RCON;
            Password = password;
            ServerName = Name;
            IsHost = true;
            UDP_SERVER = new UdpClient(port);
            LOG("Server Created!");
            while (true)
            {
                if (!SocketManager.GameSocket.Connected)
                {
                    LOG("Game closed");
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
                RecvSendTypes head = (RecvSendTypes)ahead;
                if (debug)
                {
                    //LOG("[host] Recv Packet: " + head.ToString());
                    //LOG("[host] Recv data: " + recvdata.Length);
                }
                switch (head)
                {
                    case RecvSendTypes.RET_ACCESS:
                        HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_ACCESSSHELL, BitConverter.GetBytes(CommandShell && Password == Encoding.UTF8.GetString(recvdata)));
                        break;
                    case RecvSendTypes.SND_PLAYERINFO:
                        PlayerData iiii = recvdata.ToStructure<PlayerData>();
                        if (!PlayerDataServerDATAS.ContainsKey(iiii.ID))
                        {
                            PlayerDataServer.Add(fromclie, iiii.ID);
                            PlayerDataServerDATAS.Add(iiii.ID, iiii);
                            LOG("[host] ===Joined===");
                            LOG("[host] CurrentPlayers: " + PlayerDataServer.Count);
                            LOG("[host] Player ID: " + iiii.ID);
                            LOG("[host] NickName: " + iiii.NickName);
                            LOG("[host] HP: " + iiii.HP);
                            LOG("[host] LevelID: " + iiii.LevelID);
                            LOG("[host] Plevel: " + iiii.Plevel);
                        }
                        HostSender.SendDataToAll(UDP_SERVER, (int)RecvSendTypes.RCV_PLAYERINFO, recvdata);
                        break;
                    case RecvSendTypes.SND_COMMAND:
                        if(CommandShell)
                            HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_COMMANDRESPONSE, GameManagerMY.ExecConsoleCommand(Encoding.UTF8.GetString(recvdata)));
                        break;
                    case RecvSendTypes.RET_CONNECTED:
                        if (debug)
                            LOG("[host] Preparing Server INFO For new client");
                        host_data = new ServerInfo()
                        {
                            Name = ServerName,
                            MaxPlayers = MaxP,
                            CurPlayers = GameManagerMY.GetCurrentPlayers(),
                            Version = VersionCur
                        };
                        host_data.YourID = host_data.CurPlayers;
                        if (debug)
                            LOG("[host] Sending Current INFO to");
                        HostSender.SendDataHost(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_HOSTINFO, host_data.ToByteArray());
                        break;
                    case RecvSendTypes.SND_DISCONNECTED:
                        PlayerDataServer.Remove(fromclie);
                        break;
                    case RecvSendTypes.RET_PLAYERDATAS:
                        foreach (var o in PlayerDataServerDATAS)
                        {
                            byte[] dataa = o.Value.ToByteArray();
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERINFO, dataa);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERPOSITION:
                        if (PlayerDataServer.ContainsKey(fromclie)) 
                        { 
                            var a = PlayerDataServerDATAS[PlayerDataServer[fromclie]];
                            a.rot = BitConverter.ToInt16(recvdata, 0);
                            PlayerDataServerDATAS[PlayerDataServer[fromclie]] = a;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERPOSITION, recvdata);
                        }
                        break;
                    case RecvSendTypes.SND_PLAYERROTATION:
                        
                        break;
                    case RecvSendTypes.SND_PLAYERSTATE:
                        if (PlayerDataServer.ContainsKey(fromclie))
                        {
                            var aa = PlayerDataServerDATAS[PlayerDataServer[fromclie]];
                            aa.State = BitConverter.ToInt16(recvdata, 0);
                            PlayerDataServerDATAS[PlayerDataServer[fromclie]] = aa;
                            HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_PLAYERSTATE, recvdata);
                        }
                        break;
                }
            }
        }
        public static void StopServer()
        {
            UDP_SERVER.Close();
            IsHost = false;
        }
    }
}
