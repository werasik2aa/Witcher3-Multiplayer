using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using Witcher3_Multiplayer.Game;

namespace Witcher3_Multiplayer.ClientHost
{
    public class ClientHandler
    {
        public static void SendData(PacketType head, byte[] data = null)
        {
            SendData(new ClientPacket
            {
                Header = head,
                Buffer = data,
            }.ToByteArray());
        }
        public static void SendData(byte[] data)
        {
            Client.UDP_CLIENT.SendAsync(data, data.Length);
        }
    }
}
