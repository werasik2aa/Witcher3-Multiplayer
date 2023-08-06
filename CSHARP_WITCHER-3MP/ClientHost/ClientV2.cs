using System;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class ClientV2
    {
        private static UdpClient UDP_CLIENT;
        private static PlayerData player_data;
        private static ServerInfo ConnectedInfo;
        private static IPEndPoint ConnectedEPOINT;
        private static int MyId;
        public async static void Connect(string address, int port, bool RCON, string Password = "")
        {
            LOG("Verifying game satate");
            if (!SocketManager.IsConnected())
            {
                LOG("[host] Please start game first!");
                return;
            }
            if (GameManagerUI.IsInMenu() & GameManagerUI.IsGameStoped() & GameManagerUI.IsGamePaused())
            {
                LOG("YOU_CAN'T HOST GAME IN MAIN MENU! PLEASE load a save");
                //return;
            }
            if (GameManagerUI.IsGameStoped() || GameManagerUI.IsGamePaused())
            {
                GameManagerUI.UnpauseGame();
            }
            if (IsHost)
                LOG("[client] You Hoster of The Game!");
            if (RCON && string.IsNullOrEmpty(Password))
            {
                LOG("[client] NEED A PASSWORD!");
                return;
            }
            LOG("Connecting to " + address + ":" + port);
            ConnectedEPOINT = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            UDP_CLIENT = new UdpClient(address, port);
            RCON = RCON ? AccessShell(Password) : RCON;
            IsConnected = true;
            ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.RET_CONNECTED);
            while (true)
            {
                if (!SocketManager.GameSocket.Connected || !IsConnected)
                {
                    LOG("Game Or connection closed");
                    break;
                }
                var f = await UDP_CLIENT.ReceiveAsync();
                OperateWithData(f.Buffer);
                if (IsConnected)
                {
                    ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERPOSITION, GameManagerMY.GetPlayerPosition());
                    ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERSTATE, GameManagerMY.GetPlayerMovingType());
                }
            }
        }
        public static bool AccessShell(string Password)
        {
            ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.RET_ACCESS, Password);
            byte[] data = ClientSender.GetSyncData(UDP_CLIENT, ConnectedEPOINT);
            if (data == null || data.Length < 3)
                return BitConverter.ToBoolean(data, 0);
            return false;
        }
        public static void OperateWithData(byte[] data)
        {
            if (data != null & data.Length >= 8)
            {
                byte[] header = data.Take(4).ToArray();
                byte[] header4 = data.Skip(4).Take(4).ToArray();
                int IDClient = BitConverter.ToInt16(header, 0);
                int ahead = BitConverter.ToInt16(header4, 0);
                byte[] recvdata = data.Skip(8).ToArray();
                RecvSendTypes head = (RecvSendTypes)ahead;
                if (debug)
                {
                    //LOG("[client] Recv Packet: " + head.ToString());
                    //LOG("[client] Recv data: " + recvdata.Length);
                }
                switch (head)
                {
                    case RecvSendTypes.RCV_PLAYERINFO:
                        PlayerData iiii = recvdata.ToStructure<PlayerData>();
                        if (!PlayerDataClient.ContainsKey(iiii.ID) & (iiii.ID != player_data.ID || TESTMYCLIENT))
                        {
                            LOG("[client] ===Joined===");
                            LOG("[client] NickName: " + iiii.NickName);
                            LOG("[client] Player ID: " + iiii.ID);
                            PlayerDataClient.Add(iiii.ID, iiii);
                            GameManagerMY.Spawn_Player(iiii.NickName, iiii.ID, "characters\\npc_entities\\secondary_npc\\letho.w2ent", iiii.pos, new Quaternion(0, 0, 0));
                        }
                        break;
                    case RecvSendTypes.RCV_COMMANDRESPONSE:
                        LOG("[client-cmd] RESPONSE: " + Encoding.ASCII.GetString(recvdata));
                        break;
                    case RecvSendTypes.RCV_HOSTINFO:
                        INITDATA(recvdata);
                        break;
                    case RecvSendTypes.RCV_PLAYERPOSITION:
                        var vect = recvdata.ToStructure<Vector3>();
                        GameManagerMY.SetFollowToPlayer(IDClient, vect);
                        GameManagerMY.SetPlayerHeading(IDClient, vect);
                        break;
                    case RecvSendTypes.RCV_PLAYERROTATION:
                        
                        break;
                    case RecvSendTypes.RCV_PLAYERSTATE:
                        GameManagerMY.SetPlayerMovingType(IDClient, BitConverter.ToInt16(recvdata, 0));
                        break;
                }
                Thread.Sleep(SendDataDelay);
            }
        }
        public static void INITDATA(byte[] recvdata)
        {
            ServerInfo ServerResp = recvdata.ToStructure<ServerInfo>();
            if(debug) LOG("[pre] Verify the data");
            if (!string.IsNullOrEmpty(ServerResp.Name))
            {
                if (ServerResp.MaxPlayers > ServerResp.CurPlayers + 1)
                {
                    if (debug) LOG("[pre] Preparing Player Data");
                    player_data = new PlayerData()
                    {
                        ID = ServerResp.YourID,
                        NickName = "Werasik2aa",
                        HP = GameManagerMY.GetPlayerHP(),
                        LevelID = GameManagerMY.GetPlayerAreaID(),
                        Plevel = GameManagerMY.GetPlayerLevel(),
                        pos = GameManagerMY.GetPlayerPosition(),
                        rot = GameManagerMY.GetPlayerHeading(),
                        State = GameManagerMY.GetPlayerStateInt()
                    };
                    if (debug) LOG("[pre] Sending Player Data to host");
                    ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.RET_PLAYERDATAS);
                    ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERINFO, player_data.ToByteArray());
                    IsConnected = true;
                    ConnectedInfo = ServerResp;
                    if (debug) LOG("[pre] Starting Data sender Of my CLient!");
                }
                else
                {
                    LOG("[pre] Server Already Full");
                    Disconnect();
                }
            }
            else
            {
                LOG("[pre] Server Connection wrong ServerName EMPTY");
                Disconnect();
            }
        }
        public static void Disconnect()
        {
            ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
            UDP_CLIENT.Close();
            IsConnected = false;
        }
    }
}
