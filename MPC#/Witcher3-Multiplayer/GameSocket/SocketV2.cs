using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Witcher3_Multiplayer.Game.Response;
using static Witcher3_Multiplayer.Langproc;
namespace Witcher3_Multiplayer.Game
{
    public class SocketV2
    {
        public static TcpClient TCP_GAME = new TcpClient();
        public static Data Response;
        public static bool ConnectToGame()
        {
            try
            {
                TCP_GAME.Connect("127.0.0.1", 37001);
                Convertors.Init().ForEach(x => TCP_GAME.Client.Send(x, SocketFlags.None));
            }
            catch
            {
                //nocon
            }
            return TCP_GAME.Connected;
        }
        public static void Disconnect()
        {
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
        public static void NoRead(string data)
        {
            if (debug)
                LOG(0, "[GameSocket] Executing: ", data);
            WriteData(Convertors.Execute(data));
            TCP_GAME.GetStream().Flush();
        }
        public static string ReadRaw(byte[] data)
        {
            if (debug)
                LOG(0, "[GameSocket] Executing: ", Encoding.UTF8.GetString(data));
            try
            {
                byte[] datare = new byte[8192 * 64];
                WriteData(data);
                TCP_GAME.GetStream().Read(datare, 0, datare.Length);
                Response = new Data(datare);
                return Response.Params.First().ToString().Replace("\\n", Newline).Replace('@', '"').Replace("\\", "\\\\");
            }
            catch (Exception e)
            {
                return ReadRaw(data);
            }
        }
        public static string ReadRawPattern(byte[] data, string tag)
        {
            try
            {
                string raw = ReadRaw(data);
                if (string.IsNullOrEmpty(raw) || !raw.Contains("W3MP:!" + tag + "!"))
                {
                    if (raw.ToLower().Contains("fail"))
                    {
                        LOG(2, "[GameSocket] Failed to exec command: ", Encoding.ASCII.GetString(data));
                        LOG(2, "Data: ", raw);
                    }
                    return ReadRawPattern(data, tag); //FUCKING SHIT tag not need
                }
                raw = raw.Replace("W3MP:!" + tag + "!", "");
                return raw;
            }
            catch (Exception e)
            {
                return ReadRawPattern(data, tag);
            }
        }
        public static JObject ReadDataJson(string cmd, string tag)
        {
            try
            {
                return (JObject)JsonConvert.DeserializeObject(ReadRawPattern(Convertors.Execute(cmd), tag));
            }
            catch
            {
                LOG(2, "Wrong Return from cmd: " + cmd);
                return null;
            }
        }
        public static Vector3 ReadVector3(string data, string tag)
        {
            var resp = ReadDataJson(data, tag);
            return new Vector3()
            {
                x = (int)resp.GetValue("X"),
                y = (int)resp.GetValue("Y"),
                z = (int)resp.GetValue("Z")
            };
        }
        public static Quaternion ReadQuaternion(string data, string tag)
        {
            var resp = ReadDataJson(data, tag);
            return new Quaternion()
            {
                x = (int)resp.GetValue("X"),
                y = (int)resp.GetValue("Y"),
                z = (int)resp.GetValue("Z")
            };
        }
        public static bool ReadBool(string data, string tag)
        {
            var resp = ReadDataJson(data, tag);
            return (bool)resp.GetValue("Value");
        }
        public static int ReadInteger(string data, string tag)
        {
            var resp = ReadDataJson(data, tag);
            return (int)resp.GetValue("Value");
        }
        public static float ReadFloat(string data, string tag)
        {
            var resp = ReadDataJson(data, tag);
            return (float)resp.GetValue("Value");
        }
    }
}