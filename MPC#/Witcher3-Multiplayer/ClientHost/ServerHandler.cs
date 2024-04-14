using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Logging;

namespace Witcher3_Multiplayer.ClientHost
{
    public class ServerHandler
    {
        public static int GetClientID(IPEndPoint client)
        {
            if (Server.ConnectedClientsEp.ContainsKey(client))
                return Server.ConnectedClientsEp[client];
            return Server.ConnectedClientsEp.Count();
        }
        public static void SendDataToClient(IPEndPoint Client, PacketType type, byte[] data = null)
        {
             SendData(Client, new ServerPacket
             {
                 Header = type,
                 ClientID = GetClientID(Client),
                 Buffer = data,
             }.ToByteArray());
        }
        public static void SendDataToAll(Dictionary<IPEndPoint, int> Clients, IPEndPoint ExceptClient, PacketType type, byte[] data = null)
        {
            foreach (var a in Clients)
                if(a.Key != ExceptClient || Langproc.TestLocalClient)
                    SendData(a.Key, new ServerPacket
                    {
                        Header = type,
                        ClientID = GetClientID(a.Key),
                        Buffer = data,
                    }.ToByteArray()); ;
        }
        public static void SendData(IPEndPoint Client, byte[] data)
        {
            Server.UDP_SERVER.SendAsync(data, data.Length, Client);
        }
    }
}
