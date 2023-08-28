using System.Net;
using System.Net.Sockets;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost.Data
{
    internal class ServerCommandHandler
    {
        public static string CommandLST(UdpClient UDP_SERVER, IPEndPoint fromclie, bool AllowAdminCMDS, string cmd, string[] args)
        {
            var IDClient = PlayerDataServer[fromclie].ID;
            if (AllowAdminCMDS)
            {
                if (cmd.StartsWith("//"))
                {
                    LOG("Player: " + PlayerDataServer[fromclie].NickName + " Used Server Command: " + cmd);
                    if (cmd.StartsWith("//SetAdmin"))
                    {
                        if (args.Length == 1)
                            return "Enter Value: (True || False)";
                        if (args.Length > 2)
                            return "Too Much args";
                        if (!args[1].ToLower().Contains("true") & !args[1].ToLower().Contains("false"))
                            return "Allowed Value (True || False)";
                        var SCL = ServerData.Find(x => x.ClientID == IDClient);
                        ServerData.Remove(SCL);
                        SCL.ISAdmin = bool.Parse(args[1]);
                        ServerData.Add(SCL);
                        return "Successfull changed player admin state";
                    }
                    else if (cmd.StartsWith("//Kick"))
                    {
                        LOG("[host] ===KICKED===");
                        LOG("[host] PlayerID: " + IDClient);
                        LOG("[host] PlayerName: " + PlayerDataServer[fromclie].NickName);
                        PlayerDataServer.Remove(fromclie);
                        if (!DataAPP.ServerDedicated)
                            PlayerDataClient.Remove(IDClient);
                        HostSender.SendDataToAllExceptOne(UDP_SERVER, fromclie, (int)RecvSendTypes.RCV_DISCONNECTED);
                        return "[SERVER] Successfully kicked from server";
                    }
                    else
                        return "[SERVER] Command not found";
                } else
                    return "[SERVER] NotAServerCommand";
            } else
                return "[SERVER] You don't have permission for that";
        }
    }
}