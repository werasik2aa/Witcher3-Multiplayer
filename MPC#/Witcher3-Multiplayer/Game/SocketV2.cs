using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.Game.Response;

namespace Witcher3_Multiplayer.Game
{
    public class SocketV2
    {
        public static TcpClient TCP_GAME = new TcpClient();
        public static NetworkStream stream;
        public static Response.Data Response;
        public static bool ConnectToGame()
        {
            TCP_GAME.Connect("127.0.0.1", 37001);
            stream = TCP_GAME.GetStream();
            Convertors.Init().ForEach(x => Write(x));
            return TCP_GAME.Connected;
        }
        public static bool IsConneted()
        {
            return TCP_GAME.Connected;
        }
        public static void Write(byte[] data)
        {
            stream.Write(data, 0, data.Length);
        }
        public static string Read()
        {
            byte[] data = new byte[8192 * 32];
            stream.Read(data, 0, data.Length);
            Response = new Response.Data(data);
            string returnns = Response.Params.Last().ToString();
            Thread.Sleep(1);
            return returnns;
        }
    }
}
