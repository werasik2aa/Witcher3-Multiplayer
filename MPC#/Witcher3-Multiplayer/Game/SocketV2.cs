using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
            try
            {
                TCP_GAME.Connect("127.0.0.1", 37001);
                Convertors.Init().ForEach(x => TCP_GAME.Client.Send(x, SocketFlags.None));
            }
            catch
            {
                if (DataAPP.Debug) ELOG("Failed to connect... Retrying!");
            }
            return TCP_GAME.Connected;
        }
        public static void Disconnect()
        {
            IsConnected = false;
            ClientV2.UDP_CLIENT.Close();
            ClientV2.UDP_CLIENT.Dispose();
            TCP_GAME.Close();
            TCP_GAME.Dispose();
        }
        public static bool IsConneted()
        {
            return TCP_GAME.Connected;
        }
        public static void WriteData(byte[] datad)
        {
            TCP_GAME.GetStream().Flush();
            TCP_GAME.GetStream().Write(datad, 0, datad.Length);
        }
        public static string ReadString(byte[] data)
        {
            try
            {
                byte[] datare = new byte[8192 * 64];
                WriteData(data);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Replace("\\n", Environment.NewLine).Replace('@', '"').Replace("\\", "\\\\");
                return raw;
            }
            catch (Exception e)
            {
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                    return "FAILDISCONNECT";
                }
                else ReadString(data);
                return e.Message;
            }
        }
        public static JObject ReadJson(byte[] data, string tag)
        {
            try
            {
                raw = ReadString(data).Replace("W3MP:!" + tag + "!", "");
                return (JObject)JsonConvert.DeserializeObject(raw);
            }
            catch
            {
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-JSONDATA_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    MForm.Visible = true;
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                }
                else ReadJson(data, tag);
                return null;
            }
        }
        public static Vector3 ReadVector3(byte[] data)
        {
            try
            {
                byte[] datare = new byte[8192 * 32];
                WriteData(data);
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
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-VECTOR3_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                }
                else ReadVector3(data);
                return new Vector3(0, 0, 0);
            }
        }
        public static Quaternion ReadQuaternion(byte[] data)
        {
            try
            {
                byte[] datare = new byte[8192 * 32];
                WriteData(data);
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
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-QUATERNION_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                } else ReadQuaternion(data);
                return new Quaternion(0, 0, 0);
            }
        }
        public static int ReadInt(byte[] data)
        {
            try
            {
                TCP_GAME.GetStream().FlushAsync();
                byte[] datare = new byte[8192 * 32];
                WriteData(data);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "").Replace(".", ",");
                return (int)float.Parse(raw.Replace(".", ","));
            }
            catch
            {
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-INT_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                }
                else ReadInt(data);
                return -1;
            }
        }
        public static float ReadFloat(byte[] data)
        {
            try
            {
                byte[] datare = new byte[8192 * 32];
                WriteData(data);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "").Replace(".", ",");
                return float.Parse(raw);
            }
            catch
            {
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-FLOAT_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                }
                else ReadFloat(data);
                return -1;
            }
        }
        public static bool ReadBool(byte[] data)
        {
            try
            {
                byte[] datare = new byte[8192 * 32];
                WriteData(data);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Response.Data(datare);
                raw = Response.Params.Last().ToString().Trim().Replace("W3MP ", "");
                return bool.Parse(raw);
            }
            catch
            {
                if (DataAPP.Debug) LOG("[Game-Socket] CATCH-BOOL_READ: " + raw + " RET_LEN: " + data.Length);
                if (!TCP_GAME.Connected && IsConnected)
                {
                    Disconnect();
                    ELOG("Game Crash? Or game DataAPP.Debug broken!");
                }
                else ReadBool(data);
                return false;
            }
        }
    }
}
