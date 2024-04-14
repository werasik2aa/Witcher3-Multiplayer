using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.Game.Response;
using static Witcher3_Multiplayer.Langproc;

namespace Witcher3_Multiplayer.ClientHost
{
    public class Client
    {
        public static UdpClient UDP_CLIENT = new UdpClient();
        private static ServerInfo Server_Data;
        private static PlayerData MyPlayerData;
        private static EntityInfo MyCombatEntity;
        private static IPEndPoint ConnectedEPOINT;
        private static string MyCharacterTemplate;
        public static string MyNickName = "Local";
        private static int MyID;
        public static Dictionary<int, PlayerData> ConnectedClientsData = new Dictionary<int, PlayerData>();
        public static Dictionary<int, EntityInfo> AllTargets = new Dictionary<int, EntityInfo>();
        public static async void Connect(string address, int port, string character, string mynick)
        {
            LOG(0, "[client] Verifying game state");
            if (!SocketV2.IsConneted())
            {
                LOG(0, "[client] Please start game first!");
                return;
            }
            if (GameManagerUI.IsGameNotLaunched())
            {
                LOG(0, "You can't connect while you in Main-menu! Please load save of game");
                return;
            }
            if (GameManagerUI.IsPaused())
                GameManagerUI.UnpauseGame(); //TRY TO UNPAUSE GAME
            if (IsHost)
                LOG(0, "[client] You Hoster of The Game!");
            LOG(0, "[client] Connecting to Socket " + address + ":" + port);
            try
            {
                ConnectedEPOINT = new IPEndPoint(IPAddress.Parse(address), port);
                UDP_CLIENT.Connect(address, port);
                MyNickName = mynick;
                IsConnected = true;
                MyCharacterTemplate = character;
                ClientHandler.SendData(PacketType.REQ_CONNECTED); //BEGIN
                LOG(0, "[client] Connected to Socket");
            }
            catch
            {
                LOG(0, "[client] Failed to connect to Socket: ", address + ":" + port);
                IsConnected = false;
                return;
            }
            while (IsConnected)
            {
                try
                {
                    UdpReceiveResult result = await UDP_CLIENT.ReceiveAsync();
                    ClientReceived(result.Buffer.ToStructure<ServerPacket>());
                }
                catch (Exception e)
                {
                    LOG(2, "[client] Connection with host closed! : " + e.Message);
                }
            }
        }
        public static void ClientThread()
        {
            Task.Run(() =>
            {
                while (IsConnected)
                {
                    Dictionary<int, EntityInfo> TEMP = AllTargets;
                    /*foreach (var atarg in TEMP)
                    {
                        if (!atarg.Value.IsDead) continue;
                        EntityInfo req = GameManager.ReadEntityInfoByID(atarg.Key);
                        if (req.IsDead)
                        {
                            AllTargets[atarg.Key] = req;
                            ClientHandler.SendData(PacketType.SND_ENTITYINFO, req.ToByteArray());
                        }
                    }*/
                    Thread.Sleep(ClientTickSend);
                    // Read
                    PlayerData pldata = GameManager.ReadPlayerData(MyID, MyNickName, MyCharacterTemplate);
                    EntityInfo cbdata = GameManager.ReadCombatTarget();
                    if (!AllTargets.ContainsKey(cbdata.Guid))
                        AllTargets.Add(cbdata.Guid, cbdata);

                    if(!MyCombatEntity.Equals(cbdata))
                        ClientHandler.SendData(PacketType.SND_PLAYERTARGETINFO, cbdata.ToByteArray());

                    if (!pldata.PlayerPosition.Comapare(MyPlayerData.PlayerPosition))
                        ClientHandler.SendData(PacketType.SND_PLAYERPOSITION, pldata.PlayerPosition.ToByteArray());

                    if (!pldata.HorsePosition.Comapare(MyPlayerData.HorsePosition))
                        ClientHandler.SendData(PacketType.SND_PLAYERHORSEPOSITION, pldata.HorsePosition.ToByteArray());

                    if (pldata.IsOnHorse != MyPlayerData.IsOnHorse)
                        ClientHandler.SendData(PacketType.SND_PLAYERONHORSE, BitConverter.GetBytes(pldata.IsOnHorse));

                    if (pldata.HP != MyPlayerData.HP)
                        ClientHandler.SendData(PacketType.SND_PLAYERHP, BitConverter.GetBytes(pldata.HP));

                    if (pldata.Plevel != MyPlayerData.Plevel)
                        ClientHandler.SendData(PacketType.SND_PLAYERPLEVEL, BitConverter.GetBytes(pldata.Plevel));

                    if (pldata.AreaID != MyPlayerData.AreaID)
                        ClientHandler.SendData(PacketType.SND_PLAYERAREAMAP, BitConverter.GetBytes(pldata.Plevel));

                    if (pldata.PlayerAngle != MyPlayerData.PlayerAngle)
                        ClientHandler.SendData(PacketType.SND_ANGLEP, BitConverter.GetBytes(pldata.PlayerAngle));

                    if (pldata.HorseAngle != MyPlayerData.HorseAngle)
                        ClientHandler.SendData(PacketType.SND_ANGLEH, BitConverter.GetBytes(pldata.HorseAngle));

                    if (pldata.FightData.FighState != MyPlayerData.FightData.FighState)
                        ClientHandler.SendData(PacketType.SND_FIGHTSTATE, BitConverter.GetBytes(pldata.FightData.FighState));

                    MyPlayerData = pldata;
                    MyPlayerData.FightData.FighState = 0;
                    MyCombatEntity = cbdata;
                }
            });
        }
        public static void ClientReceived(ServerPacket data)
        {
            switch (data.Header)
            {
                case PacketType.RES_BEGIN:
                    var pkg = data.Buffer.ToStructure<ServerInfo>();
                    LOG(0, "[client] Connecting to Server BEGIN");
                    if (pkg.Name.Length > 0)
                    {
                        if (pkg.CurPlayers < pkg.MaxPlayers)
                        {
                            LOG(-1, "");
                            LOG(0, "[client] ===CONNECTION===");
                            LOG(0, "[client] ServerName: " + pkg.Name);
                            LOG(0, "[client] ServerPlayers: " + pkg.CurPlayers + " <|> " + pkg.MaxPlayers);
                            LOG(0, "[client] Difficulty: " + pkg.DifficultyLevel);
                            MyID = pkg.CurPlayers;
                            MyPlayerData = GameManager.ReadPlayerData(pkg.CurPlayers, MyNickName, MyCharacterTemplate);
                            ClientHandler.SendData(PacketType.SND_PLAYERINFO, MyPlayerData.ToByteArray());
                            LOG(0, "[client] Sending our Client Data to host");
                            Server_Data = pkg;
                            return;
                        }
                        else
                            LOG(2, "[client] Server Full");
                    }
                    else
                        LOG(2, "[client] Server name NULL, Failed to connect, is that right server?");
                    IsConnected = false;
                    break;
                case PacketType.RES_END:
                    LOG(0, "[client] Connecting to Server END");
                    LOG(0, "[client] Connected");
                    GameManagerUI.InitGame(MyCharacterTemplate);
                    ClientHandler.SendData(PacketType.REQ_PLAYERDATA);
                    ClientThread();
                    break;
                case PacketType.RES_PLAYERTARGETINFO:
                    ManageTarget(data.ClientID, data.Buffer.ToStructure<EntityInfo>());
                    break;
                case PacketType.RES_CHATMSG:
                    LOG(0, ConnectedClientsData[data.ClientID].NickName + ":", Encoding.UTF8.GetString(data.Buffer));
                    break;
                case PacketType.RES_PLAYERINFO:
                    PlayerData pldata = data.Buffer.ToStructure<PlayerData>();
                    if (!ConnectedClientsData.ContainsKey(pldata.ID) & (TestLocalClient || pldata.ID != MyID))
                    {
                        ConnectedClientsData.Add(pldata.ID, pldata);
                        LOG(0, "[client] <====JOINED====>");
                        LOG(0, "[client] PlayerTemplate: " + pldata.CharacterTemplate);
                        LOG(0, "[client] PlayerID: " + pldata.ID);
                        LOG(0, "[client] PlayerName: " + pldata.NickName);
                        LOG(0, "[client] AreaID: " + pldata.AreaID);
                        LOG(0, "[client] HP: " + pldata.HP);
                        LOG(0, "[client] PlayerLevel: " + pldata.Plevel);
                        LOG(0, "[client] HorsePos: " + pldata.HorsePosition);
                        LOG(0, "[client] PlayerPos: " + pldata.PlayerPosition);
                        if(MyPlayerData.AreaID == pldata.AreaID)
                            GameManager.Spawn_Player(pldata.CharacterTemplate, pldata.NickName, pldata.PlayerPosition, pldata.HorsePosition);
                        GameManager.OnConnected(pldata.NickName);
                    }
                    ManagePlayerData(pldata);
                    break;
                default:
                    if (debug)
                        LOG(0, "[Client] No packet setup: " + data.Header.ToString());
                    break;
            }
        }
        public static void ManageEntity(EntityInfo EntityInfo)
        {
            if (EntityInfo.IsDead) 
                GameManager.KillEntityByID(EntityInfo.Guid);
        }
        public static void ManageTarget(int playerid, EntityInfo EntityInfo)
        {
            //CMBT
            if (playerid != MyID || TestLocalClient)
            {
                if (!ConnectedClientsData.ContainsKey(playerid)) return;

                if (CombatLog & RecLog)
                {
                    LOG(0, "<CombatInfo>");
                    LOG(0, "[client] Entity ID: " + EntityInfo.Guid, " isDead: " + EntityInfo.IsDead);
                    LOG(0, "[client] HP: " + EntityInfo.HP);
                    LOG(0, "[client] Position" + EntityInfo.Position);
                    LOG(0, "[client] Set as Target OnClient: " + playerid);
                }

                GameManager.DamageEntity(EntityInfo.Guid, EntityInfo.HP);
                GameManager.SetEntityAIPositionByID(EntityInfo.Guid, EntityInfo.Position, false, false);
                GameManager.SetTargetByID(playerid, EntityInfo.Guid);
            }

            AllTargets[EntityInfo.Guid] = EntityInfo;
        }
        public static void ManagePlayerData(PlayerData pldata)
        {
            //NOT MY ID SET
            if (pldata.ID != MyID || TestLocalClient)
            {
                if (!ConnectedClientsData.ContainsKey(pldata.ID)) return;

                PlayerData OldPlayerData = ConnectedClientsData[pldata.ID];
                if (!pldata.PlayerPosition.Comapare(OldPlayerData.PlayerPosition))
                {
                    if (PosLog & ClientLog)
                        LOG(0, "[host] Horse of Player" + pldata.NickName, " HorsePosition: " + pldata.HorsePosition);
                    GameManager.SetEntityAIPositionByID(pldata.ID, pldata.PlayerPosition, true, false);
                }

                if (!pldata.HorsePosition.Comapare(OldPlayerData.HorsePosition))
                {
                    if (PosLog & ClientLog)
                        LOG(0, "[host] Player: " + pldata.NickName, " Position: " + pldata.PlayerPosition);
                    GameManager.SetEntityAIPositionByID(pldata.ID, pldata.HorsePosition, true, true);
                }

                if (pldata.IsOnHorse != OldPlayerData.IsOnHorse)
                {
                    if (StatLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, "SitDownType:", pldata.IsOnHorse ? "Mounted" : "Dismounted");
                    GameManager.MountPlayerToHorse(pldata.ID, pldata.IsOnHorse);
                }

                if (pldata.HP != OldPlayerData.HP)
                {
                    if (StatLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, " NewHP:", pldata.HP);
                    GameManager.SetHealthByID(pldata.ID, pldata.HP, true);
                }

                if (pldata.Plevel != OldPlayerData.Plevel)
                {
                    if (StatLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, " NewLEVEL:", pldata.Plevel);
                    GameManager.SetLevelByID(pldata.ID, pldata.Plevel, true);
                }

                if (pldata.PlayerAngle != OldPlayerData.PlayerAngle)
                {
                    if (PosLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, " PlayerAngle:", pldata.PlayerAngle);
                    GameManager.SetAngleByID(pldata.ID, pldata.PlayerAngle, true, false);
                }

                if (pldata.HorseAngle != OldPlayerData.HorseAngle)
                {
                    if (PosLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, " HorseAngle:", pldata.HorseAngle);
                    GameManager.SetAngleByID(pldata.ID, pldata.HorseAngle, true, true);
                }

                if (pldata.AreaID != OldPlayerData.AreaID)
                {
                    if (StatLog & ClientLog)
                        LOG(0, "[client] Player: " + pldata.NickName, " NewMAPid:", pldata.AreaID);
                    //remove or again spawn npc for cur map
                }

                if (pldata.FightData.FighState != OldPlayerData.FightData.FighState)
                {
                    if (CombatLog & ClientLog & debug)
                        LOG(0, "[client] Player: " + pldata.NickName, " FighState:", pldata.FightData.FighState);
                    GameManager.SetFightStateByID(pldata.ID, pldata.FightData.FighState);
                }

                ConnectedClientsData[pldata.ID] = pldata;
            }
            /*if (pldata.ID == MyID)
            {
                if (pldata.PlayerPosition.GetDistanceTO(MyPlayerData.PlayerPosition) > 20)
                    GameManager.TeleportClient(pldata.PlayerPosition, pldata.IsOnHorse);
            }*/
        }
    }
}
