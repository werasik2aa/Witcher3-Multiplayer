using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.ClientHost.DataTypes;

namespace Witcher3_Multiplayer.ClientHost
{
    public class Manager
    {
        public static List<PlayerData> PlayerData = new List<PlayerData>();
        public static void SendData(Socket socks, State state, int sta, string text)
        {
            byte[] dataenc = BitConverter.GetBytes(sta);
            dataenc.Append(Encoding.ASCII.GetBytes(text));
            SendData(socks, state, dataenc);
        }
        public static void SendData(Socket socks, State state, int sta, byte[] data)
        {
            byte[] dataenc = BitConverter.GetBytes(sta);
            dataenc.Append(data);
            SendData(socks, state, dataenc);
        }
        public static void SendData(Socket socks, State state, byte[] data)
        {
            socks.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socks.EndSend(ar);
            }, state);
        }
        public static void SendData(Socket socks, State state, int sta, string disk, string path)
        {
            if (Directory.Exists(disk + path))
            {
                byte[] data = BitConverter.GetBytes(sta);
                data.Append(File.ReadAllBytes(disk + path));
                socks.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
                {
                    State so = (State)ar.AsyncState;
                    int bytes = socks.EndSend(ar);
                }, state);
            }
        }
        public static void SendData(Socket socks, State state, int sta)
        {
            byte[] data = BitConverter.GetBytes(sta);
            socks.BeginSend(data, 0, data.Length, SocketFlags.None, (ar) =>
            {
                State so = (State)ar.AsyncState;
                int bytes = socks.EndSend(ar);
            }, state);
        }
        public static void SendData(Socket socks, State state, int sta, Vector3 data)
        {
            byte[] header = BitConverter.GetBytes(sta);
            byte[] dataenc = new byte[12]; // 4 bytes per float

            Buffer.BlockCopy(BitConverter.GetBytes(data.x), 0, dataenc, 0, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(data.y), 0, dataenc, 4, 4);
            Buffer.BlockCopy(BitConverter.GetBytes(data.z), 0, dataenc, 8, 4);

            header.Append(dataenc);
            SendData(socks, state, dataenc);
        }
        public static void SendData<T>(Socket socks, State state, int sta, T data) where T : struct
        {
            byte[] dataenc = BitConverter.GetBytes(sta);
            dataenc.Append(langproc.StructToBytes<T>(data));
            SendData(socks, state, dataenc);
        }
        public static byte[] GetSyncData(Socket socks, State state)
        {
            byte[] data = new byte[bufSize];
            socks.Receive(data);
            return data;
        }
    }
}
