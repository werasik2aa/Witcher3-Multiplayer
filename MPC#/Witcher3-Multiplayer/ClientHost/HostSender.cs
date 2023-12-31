﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class HostSender
    {
        public static int GetClientID(IPEndPoint clien)
        {
            return PlayerDataServer.ContainsKey(clien) ? PlayerDataServer[clien].ID : -1;
        }
        public static void SendDataHost(UdpClient socks, IPEndPoint clien, byte[] data)
        {
            data = BitConverter.GetBytes(GetClientID(clien)).Append(data);
            socks.SendAsync(data, data.Length, clien);
        }
        public static void SendDataToAllExceptOne(UdpClient socks, IPEndPoint EXCEPT, int sta, byte[] info)
        {
            byte[] data = BitConverter.GetBytes(sta).Append(info);
            foreach (var o in PlayerDataServer)
                if (EXCEPT.ToString() != o.Key.ToString() || DataAPP.JoinTestLocalClient)
                    SendDataHost(socks, o.Key, data);
        }
        public static void SendDataToAllExceptOne(UdpClient socks, IPEndPoint EXCEPT, int sta, string info)
        {
            byte[] data = BitConverter.GetBytes(sta).Append(Encoding.UTF8.GetBytes(info));
            foreach (var o in PlayerDataServer)
                if (EXCEPT.ToString() != o.Key.ToString() || DataAPP.JoinTestLocalClient)
                    SendDataHost(socks, o.Key, data);
        }
        public static void SendDataToAllExceptOne(UdpClient socks, IPEndPoint EXCEPT, int sta)
        {
            byte[] data = BitConverter.GetBytes(sta);
            foreach (var o in PlayerDataServer)
                if (EXCEPT.ToString() != o.Key.ToString() || DataAPP.JoinTestLocalClient)
                    SendDataHost(socks, o.Key, data);
        }
        public static void SendDataToAllExceptOne(UdpClient socks, IPEndPoint EXCEPT, byte[] data)
        {
            foreach (var o in PlayerDataServer)
                if (EXCEPT.ToString() != o.Key.ToString() || DataAPP.JoinTestLocalClient)
                    SendDataHost(socks, o.Key, data);
        }
        public static void SendDataToAll(UdpClient socks, int sta, byte[] info)
        {
            byte[] data = BitConverter.GetBytes(sta).Append(info);
            foreach (var o in PlayerDataServer)
                SendDataHost(socks, o.Key, data);
        }
        public static void SendDataHost(UdpClient socks, IPEndPoint fromclie, int sta)
        {
            byte[] dataenc = BitConverter.GetBytes(sta);
            SendDataHost(socks, fromclie, dataenc);
        }
        public static void SendDataHost(UdpClient socks, IPEndPoint fromclie, int sta, byte[] data)
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(data);
            SendDataHost(socks, fromclie, dataenc);
        }
        public static void SendDataHost(UdpClient socks, IPEndPoint fromclie, int sta, string data)
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(Encoding.UTF8.GetBytes(data));
            SendDataHost(socks, fromclie, dataenc);
        }
        public static void SendDataHost<T>(UdpClient socks, IPEndPoint fromclie, int sta, T data) where T : struct
        {
            byte[] dataenc = BitConverter.GetBytes(sta).Append(data.ToByteArray());
            SendDataHost(socks, fromclie, dataenc);
        }
    }
}
