using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Witcher3_Multiplayer
{
    public static class Langproc
    {
        public static bool IsHost = false, IsConnected = false, TestLocalClient = true;
        public static bool debug = false, ClientLog = true, ServerLog = true, PosLog = false, StatLog = false, CombatLog = true, ReqLog = false, RecLog = false;
        public static TextBox LOGGER;
        public static string Newline = Environment.NewLine;
        public static int ClientTickSend = 64, ServerTickSend = 10;
        public static string MonstersPath = "characters\\npc_entities\\monsters\\";//+MONSTERNAME
        public static string NewLineGame = "&#10;"; //NEW LINE FOR GAME
        public static double VersionCur = 1.0;
        public static void LOG(int a = 0, object i = null, object j = null, object e = null)
        {
            string state = "FUCK YOU";
            switch(a)
            {
                default:
                    state = "";
                    break;
                case 0:
                    state = "[INFO]";
                    break;
                case 1:
                    state = "[WARN]";
                    break;
                case 2:
                    state = "[ERROR]";
                    break;
            }
            Action ex = () => { LOGGER.AppendText(state + " " + i + " " + j + " " + e + Newline); };
            if (LOGGER.InvokeRequired)
                LOGGER.Invoke(ex);
            else
                ex();
        }
        public static byte[] ToByteArray(this object structure)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, structure);
            return ms.ToArray();
        }
        public static T ToStructure<T>(this byte[] arrBytes) where T : struct
        {
            MemoryStream memStream = new MemoryStream(arrBytes);
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Seek(0, SeekOrigin.Begin);
            return (T)binForm.Deserialize(memStream);
        }
        public static void SeriliazeToFile(this object structure, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, structure);
            File.WriteAllBytes(path, ms.ToArray());
        }
        public static T FileToStructure<T>(string path) where T : struct
        {
            MemoryStream memStream = new MemoryStream(File.ReadAllBytes(path));
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Seek(0, SeekOrigin.Begin);
            return (T)binForm.Deserialize(memStream);
        }
        public static object FileToObject(string path)
        {
            MemoryStream memStream = new MemoryStream(File.ReadAllBytes(path));
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Seek(0, SeekOrigin.Begin);
            return binForm.Deserialize(memStream);
        }
    }
}
