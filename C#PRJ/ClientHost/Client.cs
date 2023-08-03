using System;
using System.Net.Sockets;
using System.Net;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
using Witcher3_Multiplayer.Game;
using System.Linq;
using System.Text;
using System.Threading;

namespace Witcher3_Multiplayer.ClientHost
{
    public class Client
    {
        public static Socket socket_client;
        private static State state = new State();
        private static EndPoint epFrom = new IPEndPoint(IPAddress.Any, 33220);
        private static AsyncCallback recv = null;
        private static PlayerData player_data;
        private static int ServerConnectedID;
        public static void ConnectToServer(string address, int port, bool RCON, string Password = "")
        {
            if (!SocketManager.IsConnected())
            {
                LOG("[host] Please start game first!");
                return;
            }
            if (IsHost)
                LOG("[client] You Host The Game! And it will Joining to: ");
            if (RCON && string.IsNullOrEmpty(Password))
            {
                LOG("[client] NEED A PASSWORD!");
                return;
            }
            socket_client = new Socket(SocketType.Stream, ProtocolType.Tcp);
            LOG("Connection to server: " + address + ":" + port);
            try
            {
                socket_client.Connect(IPAddress.Parse(address), port);
            }
            catch
            {
                LOG("Connection refused or wrong");
                return;
            }
            if (socket_client.Connected)
            {
                if (!RCON)
                {
                    if (GetData())
                    {
                        LOG("Connected to HOST-SERVER");
                        IsConnected = true;
                        socket_client.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                        {
                            try
                            {
                                State so = (State)ar.AsyncState;
                                int bytes = socket_client.EndReceiveFrom(ar, ref epFrom);
                                socket_client.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                                OperateWithData(so.buffer, bytes);
                            }
                            catch { }
                        }, state);
                    }
                }
                else
                {
                    if (AccessShell("NET")) {
                        LOG("Connected to HOST-CONSOLE");
                        IsConnected = true;
                        while (SocketManager.GameSocket.Connected)
                        {
                            socket_client.BeginReceiveFrom(state.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv = (ar) =>
                            {
                                try
                                {
                                    State so = (State)ar.AsyncState;
                                    int bytes = socket_client.EndReceiveFrom(ar, ref epFrom);
                                    socket_client.BeginReceiveFrom(so.buffer, 0, bufSize, SocketFlags.None, ref epFrom, recv, so);
                                    OperateWithRet(so.buffer, bytes);
                                }
                                catch { }
                            }, state);
                            Manager.SendData(socket_client, state, (int)RecvSendTypes.RCV_VECTOR3, GameManager.GetPosition());
                            Manager.SendData(socket_client, state, (int)RecvSendTypes.RCV_ROTATION, GameManager.GetRotation());
                            Thread.Sleep(128);
                        }
                    }
                }
            }
        }
        public static void OperateWithRet(byte[] data, int count)
        {
            LOG("");
        }
        public static void OperateWithData(byte[] data, int count)
        {
            byte[] header4 = data.Take(4).ToArray();
            int ahead = BitConverter.ToInt16(header4, 0);
            byte[] recvdata;
            if (data.Length > 4)
                recvdata = data.Skip(4).ToArray();
            else recvdata = header4;
            if (debug)
            {
                LOG("[host] Recv Packet: " + ahead);
                LOG("[host] Recv data: " + data.Skip(4).ToArray().Length);
            }
            RecvSendTypes head = (RecvSendTypes)ahead;
            switch (head)
            {
                case RecvSendTypes.SCV_PLAYERINFO:
                    OnJoined(langproc.BytesToStruct<PlayerData>(recvdata));
                    break;
                case RecvSendTypes.RCV_COMMANDRESPONSE:
                    LOG("RESPONSE: " + Encoding.ASCII.GetString(recvdata));
                    break;
            }
        }
        public static void Disconnect()
        {
            Manager.SendData(socket_client, state, (int)RecvSendTypes.SVC_DISCONNECTED, BitConverter.GetBytes(ServerConnectedID));
            socket_client.Disconnect(false);
            socket_client.Dispose();
        }
        public static bool AccessShell(string Password)
        {
            Manager.SendData(socket_client, state, (int)RecvSendTypes.RET_ACCESS, Password);
            byte[] data = Manager.GetSyncData(socket_client, state);
            if (data == null || data.Length < 3)
                return BitConverter.ToBoolean(data, 0);
            return false;
        }
        public static bool GetData()
        {
            Manager.SendData(socket_client, state, (int)RecvSendTypes.RET_CONNECTED);
            byte[] data = Manager.GetSyncData(socket_client, state);
            if (data == null || data.Length < 3)
            {
                ServerInfo ServerResp = langproc.BytesToStruct<ServerInfo>(data);
                if (!string.IsNullOrEmpty(ServerResp.Name))
                {
                    if (ServerResp.MaxPlayers > ServerResp.CurPlayers+1)
                    {
                        LOG("Sending player Data");
                        player_data = new PlayerData()
                        {
                            ID = ServerResp.CurPlayers + 1,
                            NickName = "Werasik2aa",
                            HP = GameManager.GetHP(),
                            LevelID = GameManager.GetLevelId(),
                            Plevel = GameManager.GetPLevel(),
                            pos = GameManager.GetPosition(),
                            rot = GameManager.GetRotation(),
                            State = GameManager.GetStateAnim()
                        };
                        Manager.SendData(socket_client, state, (int)RecvSendTypes.SCV_PLAYERINFO, langproc.StructToBytes(player_data));
                        return true;
                    } else
                        LOG("Server Already Full");
                }
                LOG("Server Connection wrong: " + ServerResp.CurPlayers +  "|" + ServerResp.MaxPlayers);
            }
            Disconnect();
            return false;
        }
        public static void OnJoined(PlayerData info)
        {
            if (info.ID == player_data.ID) return;
            LOG("[client] ===Joined===");
            LOG("[client] NickName: " + info.NickName);
            LOG("[client] HP: " + info.HP);
            LOG("[client] LevelID: " + info.LevelID);
            LOG("[client] Plevel: " + info.Plevel);
            Manager.PlayerData.Add(info);
        }
        public static void OnDisconnect(int id)
        {
            Manager.PlayerData.RemoveAt(id-1);
        }
    }
}
