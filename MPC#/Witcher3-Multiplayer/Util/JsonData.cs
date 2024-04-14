using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Witcher3_Multiplayer.Game;
namespace Witcher3_Multiplayer
{
    public class JsonData
    {
        public static JObject PlayerData, CombatData;
        public static Dictionary<int, JObject> QuestData = new Dictionary<int, JObject>();
        private static Keyboard mykeyboard = new Keyboard();
        public static JObject GetData(string cmd, string tag)
        {
            return SocketV2.ReadDataJson(cmd, tag);
        }
        public static string ReadString(JObject d, string data)
        {
            return (string)d[data];
        }
        public static bool ReadBool(JObject d, string data)
        {
            return (bool)d[data];
        }
        public static int ReadInt(JObject d, string data)
        {
            return (int)d[data];
        }
        public static double ReadDouble(JObject d, string data)
        {
            return (double)d[data];
        }
        public static Vector3 ReadVector3(JObject d, string data)
        {
            return new Vector3(ReadString(d, data));
        }
        public static byte[] GetBytesFromFile(string path)
        {
            return File.ReadAllBytes(path);
        }
        public static string ReadPInfo(string var)
        {
            return (string)PlayerData[var];
        }
        public static string ReadCInfo(string var)
        {
            return (string)CombatData[var];
        }
        public static char GetKey(Keys key)
        {
            bool caplock = mykeyboard.CapsLock;
            bool shift = mykeyboard.ShiftKeyDown;
            bool iscap = (caplock && !shift) || (!caplock && shift);
            switch (key)
            {
                case Keys.Enter: return '\n';
                case Keys.A: return (iscap ? 'A' : 'a');
                case Keys.B: return (iscap ? 'B' : 'b');
                case Keys.C: return (iscap ? 'C' : 'c');
                case Keys.D: return (iscap ? 'D' : 'd');
                case Keys.E: return (iscap ? 'E' : 'e');
                case Keys.F: return (iscap ? 'F' : 'f');
                case Keys.G: return (iscap ? 'G' : 'g');
                case Keys.H: return (iscap ? 'H' : 'h');
                case Keys.I: return (iscap ? 'I' : 'i');
                case Keys.J: return (iscap ? 'J' : 'j');
                case Keys.K: return (iscap ? 'K' : 'k');
                case Keys.L: return (iscap ? 'L' : 'l');
                case Keys.M: return (iscap ? 'M' : 'm');
                case Keys.N: return (iscap ? 'N' : 'n');
                case Keys.O: return (iscap ? 'O' : 'o');
                case Keys.P: return (iscap ? 'P' : 'p');
                case Keys.Q: return (iscap ? 'Q' : 'q');
                case Keys.R: return (iscap ? 'R' : 'r');
                case Keys.S: return (iscap ? 'S' : 's');
                case Keys.T: return (iscap ? 'T' : 't');
                case Keys.U: return (iscap ? 'U' : 'u');
                case Keys.V: return (iscap ? 'V' : 'v');
                case Keys.W: return (iscap ? 'W' : 'w');
                case Keys.X: return (iscap ? 'X' : 'x');
                case Keys.Y: return (iscap ? 'Y' : 'y');
                case Keys.Z: return (iscap ? 'Z' : 'z');
                case Keys.D0: return (shift ? ')' : '0');
                case Keys.D1: return (shift ? '!' : '1');
                case Keys.D2: return (shift ? '@' : '2');
                case Keys.D3: return (shift ? '#' : '3');
                case Keys.D4: return (shift ? '$' : '4');
                case Keys.D5: return (shift ? '%' : '5');
                case Keys.D6: return (shift ? '^' : '6');
                case Keys.D7: return (shift ? '&' : '7');
                case Keys.D8: return (shift ? '*' : '8');
                case Keys.D9: return (shift ? '(' : '9');
                case Keys.Oemplus: return (shift ? '+' : '=');
                case Keys.OemMinus: return (shift ? '_' : '-');
                case Keys.OemQuestion: return (shift ? '?' : '/');
                case Keys.Oemcomma: return (shift ? '<' : ',');
                case Keys.OemPeriod: return (shift ? '>' : '.');
                case Keys.OemOpenBrackets: return (shift ? '{' : '[');
                case Keys.OemQuotes: return (shift ? '"' : '\'');
                case Keys.Oem1: return (shift ? ':' : ';');
                case Keys.Oem3: return (shift ? '~' : '`');
                case Keys.Oem5: return (shift ? '|' : '\\');
                case Keys.Oem6: return (shift ? '}' : ']');
                case Keys.Tab: return '\t';
                case Keys.Space: return ' ';

                // Number Pad
                case Keys.NumPad0: return '0';
                case Keys.NumPad1: return '1';
                case Keys.NumPad2: return '2';
                case Keys.NumPad3: return '3';
                case Keys.NumPad4: return '4';
                case Keys.NumPad5: return '5';
                case Keys.NumPad6: return '6';
                case Keys.NumPad7: return '7';
                case Keys.NumPad8: return '8';
                case Keys.NumPad9: return '9';
                case Keys.Subtract: return '-';
                case Keys.Add: return '+';
                case Keys.Decimal: return '.';
                case Keys.Divide: return '/';
                case Keys.Multiply: return '*';

                default: return '\x00';
            }
        }
    }
}
