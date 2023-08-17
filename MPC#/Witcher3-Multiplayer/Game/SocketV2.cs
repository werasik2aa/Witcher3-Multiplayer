using System.Linq;
using System.Net.Sockets;
using System.Threading;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.Game.Response;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public class SocketV2
    {
        public static TcpClient TCP_GAME = new TcpClient();
        public static Response.Data Response;
        public static string raw = "NONE";
        public static bool ConnectToGame()
        {
            TCP_GAME.Connect("127.0.0.1", 37001);

            Convertors.Init().ForEach(x => TCP_GAME.Client.Send(x, SocketFlags.None));
            return TCP_GAME.Connected;
        }
        public static bool IsConneted()
        {
            return TCP_GAME.Connected;
        }
        public static void WriteData(byte[] data)
        {
            TCP_GAME.GetStream().Write(data, 0, data.Length);
        }
        public static string ReadString(byte[] data)
        {
            TCP_GAME.GetStream().Flush();
            byte[] datare = new byte[8192 * 32];
            TCP_GAME.GetStream().Write(data, 0, data.Length);
            TCP_GAME.GetStream().Read(datare, 0, datare.Length);
            Response = new Response.Data(datare);
            raw = Response.Params.Last().ToString();
            return raw;
        }
        public static Vector3 ReadVector3(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().Flush();
                byte[] datare = new byte[8192 * 32];
                TCP_GAME.GetStream().Write(data, 0, data.Length);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Replace(".", ",").Trim().Replace("W3MP ", "");
                string[] splitted = raw.Split(' ');
                return new Vector3()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
            }
            catch
            {
               //  if (debug) LOG("CATCH-VECTOR3_READ: " + raw);
                return new Vector3(0, 0, 0);
            }
        }
        public static Quaternion ReadQuaternion(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().Flush();
                byte[] datare = new byte[8192 * 32];
                TCP_GAME.GetStream().Write(data, 0, data.Length);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Replace(".", ",").Trim().Replace("W3MP ", "");
                string[] splitted = raw.Split(' ');
                return new Quaternion()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
            }
            catch
            {
               //  if (debug) LOG("CATCH-VECTOR3_READ: " + raw);
                return new Quaternion(0, 0, 0);
            }
        }
        public static int ReadInt(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().Flush();
                byte[] datare = new byte[8192 * 32];
                TCP_GAME.GetStream().Write(data, 0, data.Length);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "").Replace(".", ",");
                return (int)float.Parse(raw.Replace(".", ","));
            }
            catch
            {
               //  if (debug) LOG("CATCH-INT_READ: " + raw);
                return -1;
            }
        }
        public static float ReadFloat(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().Flush();
                byte[] datare = new byte[8192 * 32];
                TCP_GAME.GetStream().Write(data, 0, data.Length);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "").Replace(".", ",");
                return float.Parse(raw);
            }
            catch
            {
               //  if (debug) LOG("CATCH-INT_READ: " + raw);
                return -1;
            }
        }
        public static bool ReadBool(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().Flush();
                byte[] datare = new byte[8192 * 32];
                TCP_GAME.GetStream().Write(data, 0, data.Length);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "");
                return bool.Parse(raw);
            }
            catch
            {
               //  if (debug) LOG("CATCH-BOOL_READ: " + raw);
                return false;
            }
        }
    }
}
