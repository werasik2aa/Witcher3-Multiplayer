using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Witcher3_Multiplayer.Game;
namespace Witcher3_Multiplayer.ClientHost
{
    public class ClientSender
    {
        public static void SendData(UdpClient socks, int sta)
        {
            byte[] data = BitConverter.GetBytes(sta);
            SendData(socks, data);
        }
        public static void SendData<T>(UdpClient socks, int sta, T data) where T : struct
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(data.ToByteArray());
            SendData(socks, dataenc);
        }
        public static void SendData(UdpClient socks, int sta, string disk, string path)
        {
            if (Directory.Exists(disk + path))
            {
                byte[] data = BitConverter.GetBytes(sta).Append(File.ReadAllBytes(disk + path));
                SendData(socks, data);
            }
        }
        public static void SendData(UdpClient socks, int sta, string text)
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(Encoding.ASCII.GetBytes(text));
            SendData(socks, dataenc);
        }
        public static void SendData(UdpClient socks, int sta, byte[] data)
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(data);
            SendData(socks, dataenc);
        }
        public static void SendData(UdpClient socks, byte[] data)
        {
            socks.SendAsync(data, data.Length, null);
        }
        public static byte[] GetSyncData(UdpClient socks, IPEndPoint fromclie)
        {
            byte[] data = socks.Receive(ref fromclie);
            return data;
        }
    }
}
