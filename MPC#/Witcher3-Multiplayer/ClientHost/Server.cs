using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.Langproc;

namespace Witcher3_Multiplayer.ClientHost
{
    internal class Server
    {
        public static UdpClient UDP_SERVER;
        private static ServerInfo Server_Info;
        private static string ServerName = "EBalo";
        private static int MaxPlayers = 5;
        public static Dictionary<IPEndPoint, int> ConnectedClientsEp = new Dictionary<IPEndPoint, int>();
        public static Dictionary<IPEndPoint, PlayerData> ConnectedClientsData = new Dictionary<IPEndPoint, PlayerData>();
        public static async void CreateServer(int port)
        {
            LOG(0, "[host] Creating server");
            if (!SocketV2.IsConneted())
            {
                LOG(0, "[host] Please start game first!");
                return;
            }
            if (IsHost)
            {
                LOG(2, "[host] You already host!");
                return;
            }
            if (IsConnected)
            {
                LOG(2, "[host] Please Disconnect from server, before create new server. OR MANY BUGS!");
                return;
            }
            if (GameManagerUI.IsGameNotLaunched())
            {
                LOG(2, "YOU_CAN'T HOST GAME IN MAIN MENU! PLEASE load a save");
                return;
            }
            UDP_SERVER = new UdpClient(port);
            IsHost = true;
            LOG(0, "[host] Server Hosted");
            while (true)
            {
                if (!SocketV2.IsConneted())
                {
                    LOG(0, "[host] Game closed or server closed");
                    break;
                }
                UdpReceiveResult result;
                try
                {
                    result = await UDP_SERVER.ReceiveAsync();
                    HostReceived(result.Buffer.ToStructure<ClientPacket>(), result.RemoteEndPoint);
                    foreach (var plda in ConnectedClientsData)
                        ServerHandler.SendDataToClient(plda.Key, PacketType.RES_PLAYERINFO, plda.Value.ToByteArray());
                    //Thread.Sleep(ServerTickSend); //why??^
                }
                catch (ObjectDisposedException msg)
                {
                    LOG(2, "[host] Client Lost Connection " + msg);
                }
            }
        }
        public static void HostReceived(ClientPacket data, IPEndPoint ClientPoint)
        {
            switch (data.Header)
            {
                case PacketType.REQ_CONNECTED:
                    ServerHandler.SendDataToClient(ClientPoint, PacketType.RES_BEGIN, new ServerInfo()
                    {
                        Name = ServerName,
                        MaxPlayers = MaxPlayers,
                        CurPlayers = ConnectedClientsEp.Count(),
                        Version = 1.0,
                        RequireCheckVersion = false
                    }.ToByteArray());
                    LOG(0, "[host] Accepting new Connection from: " + ClientPoint.ToString());
                    break;
                case PacketType.SND_PLAYERINFO:
                    if (!ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        ServerHandler.SendDataToClient(ClientPoint, PacketType.RES_END);
                        PlayerData plinf = data.Buffer.ToStructure<PlayerData>();
                        LOG(-1, "");
                        LOG(0, "[host] <====JOINED====>");
                        LOG(0, "[host] PlayerTemplate: " + plinf.CharacterTemplate);
                        LOG(0, "[host] PlayerID: " + plinf.ID);
                        LOG(0, "[host] PlayerName: " + plinf.NickName);
                        LOG(0, "[host] AreaID: " + plinf.AreaID);
                        LOG(0, "[host] HP: " + plinf.HP);
                        LOG(0, "[host] PlayerLevel: " + plinf.Plevel);
                        LOG(0, "[host] HorsePos: " + plinf.HorsePosition);
                        LOG(0, "[host] PlayerPos: " + plinf.PlayerPosition);
                        ConnectedClientsData.Add(ClientPoint, plinf);
                        ConnectedClientsEp.Add(ClientPoint, ConnectedClientsEp.Count());
                        ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_PLAYERINFO, plinf.ToByteArray());
                        if (ServerLog)
                            LOG(0, "[host] Resend PlayerInfo to ALL: ");
                    }
                    break;
                case PacketType.REQ_PLAYERDATA:
                    foreach (var plda in ConnectedClientsData)
                        ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_PLAYERINFO, plda.Value.ToByteArray());
                    if (ReqLog)
                        LOG(0, "[host] Request DataPacket from ID: " + ClientPoint);
                    break;
                case PacketType.SND_CHATMSG:
                    if (RecLog)
                        LOG(0, "[host] Received chat MESSAGE: " + Encoding.ASCII.GetString(data.Buffer));
                    ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_CHATMSG, data.Buffer);
                    break;
                case PacketType.SND_PLAYERPLEVEL:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.Plevel = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (StatLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName, " ReachLevel: " + PlDat.Plevel);
                    }
                    break;
                case PacketType.SND_PLAYERPOSITION:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.PlayerPosition = data.Buffer.ToStructure<Vector3>();
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (PosLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName, " Position: " + PlDat.PlayerPosition);
                    }
                    break;
                case PacketType.SND_PLAYERHORSEPOSITION:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.HorsePosition = data.Buffer.ToStructure<Vector3>();
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (PosLog & ServerLog)
                            LOG(0, "[host] Horse of Player: " + PlDat.NickName, " HorsePosition: " + PlDat.HorsePosition);
                    }
                    break;
                case PacketType.SND_PLAYERHP:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.HP = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (StatLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName + "HaveHP: " + PlDat.HP);
                    }
                    break;
                case PacketType.SND_PLAYERAREAMAP:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.AreaID = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (StatLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName, " Entered Map: " + PlDat.AreaID);
                    }
                    break;
                case PacketType.SND_PLAYERONHORSE:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.IsOnHorse = BitConverter.ToBoolean(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (StatLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName + " IsOnHorse: " + PlDat.IsOnHorse);
                    }
                    break;
                case PacketType.SND_ANGLEP:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.PlayerAngle = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (PosLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName + " PlayerAngle: " + PlDat.PlayerAngle);
                    }
                    break;
                case PacketType.SND_ANGLEH:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.HorseAngle = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (PosLog & ServerLog)
                            LOG(0, "[host] Player: " + PlDat.NickName + " HorseAngle: " + PlDat.HorseAngle);
                    }
                    break;
                case PacketType.SND_PLAYERTARGETINFO:
                    var Enta = data.Buffer.ToStructure<EntityInfo>();
                    if (CombatLog & ServerLog)
                    {
                        LOG(0, "<EntityInfo>");
                        LOG(0, "[host] Entity ID: " + Enta.Guid, " isDead: " + Enta.IsDead);
                        LOG(0, "[host] HP: " + Enta.HP);
                        LOG(0, "[host] Position: " + Enta.Position);
                        LOG(0, "[host] Looked by: " + ConnectedClientsEp[ClientPoint]);
                    }
                    if (data.Header != PacketType.RES_PLAYERTARGETINFO)
                        ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_PLAYERTARGETINFO, data.Buffer);
                    else
                        ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_ENTITYINFO, data.Buffer);
                    break;
                case PacketType.SND_ENTITYINFO:
                    var enta = data.Buffer.ToStructure<EntityInfo>();
                    if (CombatLog & ServerLog)
                    {
                        LOG(0, "<EntityInfo>");
                        LOG(0, "[host] Entity ID: " + enta.Guid, " isDead: " + enta.IsDead);
                        LOG(0, "[host] HP: " + enta.HP);
                        LOG(0, "[host] Position" + enta.Position);
                    }
                    ServerHandler.SendDataToAll(ConnectedClientsEp, ClientPoint, PacketType.RES_ENTITYINFO, data.Buffer);
                    break;
                case PacketType.SND_FIGHTSTATE:
                    if (ConnectedClientsData.ContainsKey(ClientPoint))
                    {
                        PlayerData PlDat = ConnectedClientsData[ClientPoint];
                        PlDat.FightData.FighState = BitConverter.ToInt16(data.Buffer, 0);
                        ConnectedClientsData[ClientPoint] = PlDat;
                        if (CombatLog & ServerLog & debug)
                            LOG(0, "[host] Player: " + PlDat.NickName + " FightState: " + PlDat.FightData.FighState);
                    }
                    break;
                default:
                    if (debug)
                        LOG(0, "[Host] No packet setup: " + data.Header.ToString());
                    break;
            }
        }
    }
}