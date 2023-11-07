using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.langproc;
using System.Threading;

namespace Witcher3_Multiplayer.ClientHost.Data
{
    public class ClientCommandHandler
    {
        public static string CommandLST(UdpClient UDP_CLIENT, string cmd, string[] args)
        {
            if (cmd.StartsWith("//"))
            {
                if (cmd.StartsWith("//Connect"))
                {
                    if (args.Length <= 1)
                        return "[GameClient] Please input ServerIP";
                    if (args.Length == 2)
                        return "[GameClient] Please input ServerPort";
                    if (args.Length > 3)
                        return "[GameClient] Not Implemented";
                    if (IsHost && !DataAPP.ServerDedicated)
                    {
                        LOG("Your hosted server Stopped");
                        ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_DISCONNECTED);
                        return "[GameClient] Already hosted and connected!" + NewLineGame + " Please stop server to connect to another server";
                    }
                    if (!IsConnected)
                    {
                        DataAPP.ClientPort = int.Parse(args[2]);
                        DataAPP.ServerIP = args[3];
                        LOG("Connecting to: " + DataAPP.ServerIP + ":" + DataAPP.ClientPort);
                        MForm.SaveData();
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
                    return "Connected: " + IsConnected;
                }
                else if (cmd.StartsWith("//HostGame"))
                {
                    if (!IsHost)
                    {
                        if (args.Length <= 1)
                            return "[GameClient] Please input ServerPort";
                        if (args.Length > 2)
                            return "[GameClient] Not Implemented";
                        DataAPP.ServerPort = int.Parse(args[1]);
                        DataAPP.ClientPort = DataAPP.ServerPort;
                        MForm.SaveData();
                        LOG("HostingGame: " + DataAPP.ClientPort);
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
                                    ClientV2.Connect(DataAPP.NickName, DataAPP.character, "127.0.0.1", DataAPP.ServerPort, DataAPP.ClientRCON);
                                }
                            }
                            else
                            {
                                var b = Task.Run(() =>
                                {
                                    ServerV2.CreateServer(DataAPP.ServerPort, DataAPP.ServerIP, DataAPP.ServerMaxPlayers, DataAPP.ServerRCON, DataAPP.ServerConsolePassword);
                                });
                                Thread.Sleep(200);
                                ClientV2.Connect(DataAPP.NickName, DataAPP.character, "127.0.0.1", DataAPP.ServerPort, DataAPP.ClientRCON);
                            }
                        });
                        return "[GameClient] Game hosted";
                    }
                    else return "[GameClient] Already hosted";
                }
                else if (cmd.StartsWith("//Disconnect"))
                {
                    if (IsConnected)
                    {
                        if (args.Length > 1)
                            return "[GameClient] Not Implemented";
                        ClientV2.Disconnect();
                        return "[GameClient] Disconnected";
                    }
                    else
                        return "[GameClient] You Not connected";
                }
                else
                {
                    if (IsConnected)
                    {
                        if (args.Length > 1) foreach (var o in args) if (cmd != o) cmd += " " + o;
                        ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_COMMANDORCHATMSG, cmd);
                        return "";
                    }
                    else return "[GameClient] Command Not Found";
                }
            }
            else if (cmd.StartsWith("/"))
            {
                if (args.Length > 1)
                {
                    cmd = args[0].Substring(1);
                    cmd += "(";
                    for (int o = 1; o < args.Length; o++) if (cmd != args[o]) cmd += args[o] + (args.Length > o + 1 ? ", " : "");
                    cmd += ")";
                }
                else cmd = cmd.Substring(1);
                LOG("[Console] Executing: " + cmd);
                return "[CONSOLE] " + GameManagerMY.ExecConsoleCommand(cmd).Replace("W3MP ", "");
            }
            else return "[CONSOLE] Failed to send chat MSG! Connect to server first!";
        }
    }
}