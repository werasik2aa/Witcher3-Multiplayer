using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class ClientV2
    {
        public static UdpClient UDP_CLIENT;
        private static PlayerData player_data;
        private static ServerInfo ConnectedInfo;
        private static CombatTarget MyTarget;
        private static IPEndPoint ConnectedEPOINT;
        private static int MyId = 0;
        private static string MyName;
        private static string MyCharacterTemplate;
        public async static void Connect(string nick, string chara, string address, int port, bool RCON, string Password = "")
        {
            LOG("[client] Verifying game satate");
            if (!SocketV2.IsConneted())
            {
                ELOG("[client] Please start game first!");
                return;
            }
            if (GameManagerUI.IsGameNotLaunched())
            {
                ELOG("You can't connect while you in Main-menu! Please load save of game");
                return;
            }
            if (GameManagerUI.IsPausedTWO()) GameManagerUI.UnpauseGame(); //TRY TO UNPAUSE GAME
            if (IsHost)
                LOG("[client] You Hoster of The Game!");
            if (RCON && string.IsNullOrEmpty(Password))
            {
                ELOG("[client] NEED A PASSWORD! FOR REMOTE CONTROLL");
                return;
            }
            LOG("Connecting to " + address + ":" + port);
            ConnectedEPOINT = new IPEndPoint(IPAddress.Parse(address), port);
            UDP_CLIENT = new UdpClient(address, port);
            RCON = RCON ? AccessShell(Password) : RCON;
            IsConnected = true;
            MyName = nick;
            GameManagerUI.InitGame(MyCharacterTemplate = chara);
            ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.RET_CONNECTED);
            while (IsConnected)
            {
                try
                {
                    if (!SocketV2.IsConneted() || !IsConnected) Disconnect();
                    var f = await UDP_CLIENT.ReceiveAsync();
                    OperateWithData(f.Buffer);
                } catch
                {
                    UDP_CLIENT.Close();
                    IsConnected = false;
                    LOG("Game Or Server Closed");
                }
            }
        }
        public static void UpdateMyData()
        {
            var a = Task.Run(() =>
            {
                while (IsConnected)
                {
                    var nwdata = DataManager.ReadPlayerData(MyId, MyName, MyCharacterTemplate); //GET NEW

                    if (!DataManager.IsClientContainsCombat(nwdata.CurrentTarget.Guid) && !nwdata.CurrentTarget.IsDead)
                        CombatTargetsClient.Add(nwdata.CurrentTarget);

                    if (nwdata.IsOnHorse != player_data.IsOnHorse)
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERONHORSE, BitConverter.GetBytes(nwdata.IsOnHorse));

                    if (!nwdata.PlayerPosition.Comapare(player_data.PlayerPosition))
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERPOSITION, nwdata.PlayerPosition.ToByteArray());

                    if (!nwdata.HorsePosition.Comapare(player_data.HorsePosition))
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERHORSEPOSITION, nwdata.HorsePosition.ToByteArray());

                    if (nwdata.CurrentTarget.FightState != player_data.CurrentTarget.FightState || nwdata.CurrentTarget.HP != player_data.CurrentTarget.HP)
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERCOMBATTARGET, nwdata.CurrentTarget.ToByteArray());

                    if (nwdata.HP != player_data.HP)
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERHP, BitConverter.GetBytes(nwdata.HP));

                    if (nwdata.Plevel != player_data.Plevel)
                        ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERLEVEL, BitConverter.GetBytes(nwdata.Plevel));

                    List<CombatTarget> nava = new List<CombatTarget>();//SEARCH DEAD ENTITY
                    foreach (var ia in CombatTargetsClient)
                    {
                        var io = ia;
                        if (!io.IsDead)
                            if (io.IsDead = GameManagerMY.GetCombatTargetIsDead(ia.Guid))
                            {
                                io.HP = 0;
                                ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERCOMBATTARGET, io.ToByteArray());
                            }
                        nava.Add(io);
                    }
                    CombatTargetsClient = nava;
                    player_data = nwdata;
                    Thread.Sleep(SendDataDelayC_S_C);
                }
            });
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
                int IDClient = PlayerDataClient.Count - BitConverter.ToInt16(header, 0) - 1;
                int ahead = BitConverter.ToInt16(header4, 0);
                byte[] recvdata = data.Skip(8).ToArray();
                RecvSendTypes head = (RecvSendTypes)ahead;
                switch (head)
                {
                    case RecvSendTypes.RCV_PLAYERINFO:
                        PlayerData iiii = recvdata.ToStructure<PlayerData>();
                        if (!PlayerDataClient.ContainsKey(iiii.ID) & (iiii.ID != player_data.ID || DataAPP.JoinTestLocalClient))
                        {
                            LOG("[client] ===INFO GET===");
                            LOG("[client] NickName: " + iiii.NickName);
                            LOG("[client] Player ID: " + iiii.ID);
                            PlayerDataClient.Add(iiii.ID, iiii);
                            GameManagerMY.Spawn_Player(iiii.NickName, iiii.ID, iiii.CharacterTemplate, iiii.PlayerPosition, iiii.HorsePosition);
                        }
                        break;
                    case RecvSendTypes.RCV_COMMANDRESPONSE:
                        LOG("[Server] " + Encoding.ASCII.GetString(recvdata));
                        break;
                    case RecvSendTypes.RCV_CHATRESPONSE:
                        LOG(PlayerDataClient[IDClient].NickName + "> " + Encoding.UTF8.GetString(recvdata));
                        PrevChatText += NewLineGame + "[" + PlayerDataClient[IDClient].NickName + "] " + Encoding.UTF8.GetString(recvdata);
                        GameManagerUI.ChatUpdate(PrevChatText);
                        break;
                    case RecvSendTypes.RCV_DISCONNECTED:
                        LOG("[host] ===Disconnected===");
                        LOG("[host] PlayerID: " + IDClient);
                        LOG("[host] PlayerName: " + PlayerDataClient[IDClient].NickName);
                        PlayerDataClient.Remove(IDClient);
                        GameManagerMY.Remove_Player(IDClient);
                        break;
                    case RecvSendTypes.RCV_HOSTINFO:
                        ServerInfo ServerResp = recvdata.ToStructure<ServerInfo>();
                        if (DataAPP.Debug) LOG("[pre] Verify the data");
                        if (!string.IsNullOrEmpty(ServerResp.Name))
                        {
                            if(ServerResp.RequireCheckVersion && ServerResp.Version == DataManager.ReadDouble(DataManager.GetData("MODVER", "GetMODVersion"), "VERS"))
                            {
                                ELOG("[Pre] Version of server doesn't match with version of the mod! Please Update MOD");
                                return;
                            }
                            if (ServerResp.MaxPlayers == -1)
                            {
                                LOG("[pre] Player with that name already exist on server!");
                                Disconnect();
                                return;
                            }
                            if (ServerResp.MaxPlayers > ServerResp.CurPlayers + 1)
                            {
                                LOG("Joined to server: " + ServerResp.Name);
                                if (DataAPP.Debug) LOG("[pre] Preparing Player Data");
                                MyId = ServerResp.CurPlayers;
                                player_data = DataManager.ReadPlayerData(MyId, MyName, MyCharacterTemplate);
                                if (!DataAPP.JoinTestLocalClient) PlayerDataClient.Add(player_data.ID, player_data);
                                if (!DataManager.IsClientContainsCombat(player_data.CurrentTarget.Guid)) CombatTargetsClient.Add(player_data.CurrentTarget); //CHECK TARGET
                                if (DataAPP.Debug) LOG("[pre] Sending Player Data to host LENGTH: " + player_data.ToByteArray().Length);
                                ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_PLAYERINFO, player_data.ToByteArray());
                                ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.RET_PLAYERDATAS);
                                IsConnected = true;
                                ConnectedInfo = ServerResp;
                                UpdateMyData(); //START READ DATA COROUTINE
                            }
                            else
                            {
                                LOG("[pre] Server Already Full");
                                Disconnect();
                            }
                        }
                        else
                        {
                            LOG("[pre] Wrong Server Or ServerName EMPTY");
                            Disconnect();
                        }
                        break;
                    case RecvSendTypes.RCV_PLAYERONHORSE:
                        LOG("[client] PlayerID: " + IDClient + " State Sit OnHorse: " + BitConverter.ToBoolean(recvdata, 0));
                        GameManagerMY.SetPlayerIsOnHorse(IDClient, BitConverter.ToBoolean(recvdata, 0) ? 1 : 0);
                        break;
                    case RecvSendTypes.RCV_PLAYERPOSITION:
                        GameManagerMY.SetPlayerMoveTo(IDClient, recvdata.ToStructure<Vector3>());
                        break;
                    case RecvSendTypes.RCV_PLAYERHORSEPOSITION:
                        GameManagerMY.SetPlayerHorseMoveTo(IDClient, recvdata.ToStructure<Vector3>());
                        break;
                    case RecvSendTypes.RCV_ENTITYDATA:
                        var cbdat = recvdata.ToStructure<CombatTarget>();
                        GameManagerMY.Spawn_NPC(cbdat.Template, cbdat.Position, cbdat.Guid);
                        break;
                    case RecvSendTypes.RCV_PLAYERCOMBATTARGET:
                        var cbdate = recvdata.ToStructure<CombatTarget>();
                        if (DataAPP.Debug)
                            LOG("[Client] EntityGUID: " + cbdate.Guid + " HP: " + cbdate.HP);
                        if (cbdate.IsDead)
                        {
                            LOG("[Client] Killed by: " + IDClient);
                            GameManagerMY.KillEntity(IDClient);
                        }
                        else
                            GameManagerMY.Attack(IDClient, cbdate.FightState, cbdate.HP);
                        break;
                    case RecvSendTypes.RCV_PLAYERHP:
                        GameManagerMY.SetPlayerHP(IDClient, BitConverter.ToUInt16(recvdata, 0));
                        break;
                    case RecvSendTypes.RCV_PLAYERLEVEL:
                        GameManagerMY.SetPlayerHP(IDClient, BitConverter.ToUInt16(recvdata, 0));
                        break;
                    default:
                        if (DataAPP.Debug) ELOG("No Packet Handle function for: " + head);
                        break;
                }
            }
        }
        public static void Disconnect()
        {
            ClientSender.SendData(UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
            Thread.Sleep(500);
            UDP_CLIENT.Close();
            UDP_CLIENT.Dispose();
            IsConnected = false;
            if (!DataAPP.ServerDedicated & IsHost)
                ServerV2.StopServer();
        }
    }
}
