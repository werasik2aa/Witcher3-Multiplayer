
using System.Net.Sockets;
using System.Net;
using System;
using static Witcher3_Multiplayer.langproc;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using System.Linq;
using Witcher3_Multiplayer.Game;
using System.Text;
using System.Threading;

namespace Witcher3_Multiplayer.ClientHost
{
    public class Host
    {
        public static Socket socket_server, socket_client;
        private static State state = new State();
        private static EndPoint epFrom = new IPEndPoint(IPAddress.Any, 33220);
        private static AsyncCallback recv = null;
        private static ServerInfo host_data;
        private static string ServerName = "EBalo";
        private static int MaxP = 30;
        private static string Password = "NET";
        private static bool CommandShell = false;
        private static bool SV_CHEATS = false;
        public static void CreateServer(string address, int port, bool RCON)
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
            if (RCON && string.IsNullOrEmpty(Password))
            {
                LOG("[client] NEED A PASSWORD!");
                return;
            }
            CommandShell = RCON;
            host_data = new ServerInfo()
            {
                Name = ServerName,
                MaxPlayers = MaxP,
                CurPlayers = 0,
                Version = VersionCur
            };
            socket_server = new Socket(SocketType.Stream, ProtocolType.Tcp);
            socket_server.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.ReuseAddress, true);
            socket_server.Bind(new IPEndPoint(IPAddress.Parse(address), port));
            LOG("Creating server");
            if (socket_server.Connected)
            {
                LOG("Server Created");
                IsHost = true;
                while (SocketManager.GameSocket.Connected)
                {
                    socket_server.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                    {
                        try
                        {
                            State so = (State)ar.AsyncState;
                            int bytes = socket_server.EndReceiveFrom(ar, ref epFrom);
                            socket_server.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                            OperateWithData(so.buffer, bytes);
                        }
                        catch { }
                    }, state);
                    Manager.SendData(socket_server, state, (int) RecvSendTypes.RCV_VECTOR3, GameManager.GetPosition());
                    Manager.SendData(socket_server, state, (int)RecvSendTypes.RCV_ROTATION, GameManager.GetRotation());
                    Thread.Sleep(128);
                }
            }
        }
        public static void OperateWithData(byte[] data, int count)
        {
            if (count > 0) {
                byte[] header4 = data.Take(4).ToArray();
                byte[] recvdata;
                if (data.Length > 4)
                    recvdata = data.Skip(4).ToArray();
                else recvdata = header4;
                int ahead = BitConverter.ToInt16(header4, 0);
                if (debug)
                {
                    LOG("[host] Recv Packet: " + ahead);
                    LOG("[host] Recv data: " + data.Skip(4).ToArray().Length);
                }
                RecvSendTypes head = (RecvSendTypes)ahead;
                switch (head)
                {
                    case RecvSendTypes.RET_ACCESS:
                        Manager.SendData(socket_server, state, BitConverter.GetBytes(CommandShell && Password == Encoding.UTF8.GetString(recvdata)));
                        break;
                    case RecvSendTypes.SCV_PLAYERINFO:
                        OnJoined(langproc.BytesToStruct<PlayerData>(recvdata));
                        break;
                    case RecvSendTypes.SVC_COMMAND:
                        Manager.SendData(socket_server, state, (int)RecvSendTypes.RCV_COMMANDRESPONSE, GameManager.ConsoleCommand(Encoding.UTF8.GetString(recvdata)));
                        break;
                    case RecvSendTypes.RET_CONNECTED:
                        SendHostDataFirst();
                        break;
                    case RecvSendTypes.SVC_DISCONNECTED:
                        OnDisconnect(BitConverter.ToInt16(recvdata, 0));
                        break;
                }
                //SENDTOALL
                Manager.SendData(socket_server, state, ahead, recvdata);
            }
        }
        public static void SendHostDataFirst()
        {
            host_data = new ServerInfo()
            {
                Name = ServerName,
                MaxPlayers = MaxP,
                CurPlayers = Manager.PlayerData.Count,
                Version = VersionCur
            };
            Manager.SendData(socket_server, state, (int)RecvSendTypes.SCV_HOSTINFO, langproc.StructToBytes(host_data));
        }
        public static void SendPlayersData()
        {
            byte[] data = new byte[5];
            foreach (var o in Manager.PlayerData)
            {
                byte[] obaHa = langproc.StructToBytes(o);
                data.Append(obaHa);
            }
            Manager.SendData(socket_server, state, (int)RecvSendTypes.SCV_HOSTINFO, data);
        }
        public static void StopServer()
        {
            socket_server.Disconnect(false);
            socket_server.Dispose();
        }
        public static void OnJoined(PlayerData info)
        {
            LOG("[host] ===Joined===");
            LOG("[host] NickName: " + info.NickName);
            LOG("[host] HP: " + info.HP);
            LOG("[host] LevelID: " + info.LevelID);
            LOG("[host] Plevel: " + info.Plevel);
            Manager.PlayerData.Add(info);
            GameManager.SpawnNPCEntity(info.ID, info.pos, info.rot);
        }
        
        public static void OnDisconnect(int id)
        {
            Manager.PlayerData.RemoveAt(id-1);
        }
    }
}
