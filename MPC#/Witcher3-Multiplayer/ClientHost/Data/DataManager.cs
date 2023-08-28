using Microsoft.VisualBasic.Devices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using Witcher3_Multiplayer.ClientHost.Data;
using Witcher3_Multiplayer.Game;
using static System.Runtime.CompilerServices.RuntimeHelpers;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class DataManager
    {
        public static JObject PlayerData, CombatData;
        public static Dictionary<int, JObject> QuestData = new Dictionary<int, JObject>();
        private static Keyboard mykeyboard = new Keyboard();
        public static JObject GetData(string tag, string cmd, string[] args = null)
        {
            if (args != null)
                foreach (var o in args) cmd += ", " + o;
            string adata = SocketV2.ReadString(Convertors.Execute(cmd));
            if (string.IsNullOrEmpty(adata) || !adata.Contains("W3MP:!" + tag + "!")) return GetData(tag, cmd, args);
            return (JObject)JsonConvert.DeserializeObject(adata.Replace("W3MP:!" + tag + "!", ""));
        }
        public static PlayerData ReadPlayerData(int id, string name, string template)
        {
            PlayerData = GetData("PLAYERDATA", "GetPlayerData");
            return new PlayerData()
            {
                Version = (double)PlayerData["VERS"],
                ID = id,
                NickName = name,
                PlayerPosition = new Vector3((string)PlayerData["PLAYERPOSITION"]),
                HorsePosition = new Vector3((string)PlayerData["HORSEPOSITION"]),
                AnimState = (int)PlayerData["ANIMINT"],
                IsDead = (bool)PlayerData["ISDEAD"],
                HP = (int)PlayerData["HP"],
                Plevel = (int)PlayerData["PLEVEL"],
                AreaID = (int)PlayerData["AREAID"],
                IsOnHorse = (bool)PlayerData["ISONHORSE"],
                CharacterTemplate = template,
                CurrentTarget = ReadCombatTarget()
            };
        }
        public static CombatTarget ReadCombatTarget()
        {
            CombatData = GetData("COMBATDATA", "GetTargetData");
            return new CombatTarget()
            {
                Guid = (int)CombatData["COMBATTARGGUID"],
                Template = (string)CombatData["COMBATTARGNAME"],
                HP = (int)CombatData["COMBATTHP"],
                Position = new Vector3((string)CombatData["COMBATTARGPOS"]),
                FightState = (int)CombatData["FIGHTACTION"],
                IsDead = (bool)(CombatData["ISDEAD"])
            };
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
        public static bool IsClientContainsCombat(int guid)
        {
            foreach (var o in CombatTargetsClient)
                if (o.Guid == guid) return true;
            return false;
        }
        public static string ReadPInfo(string var)
        {
            return (string)PlayerData[var];
        }
        public static string ReadCInfo(string var)
        {
            return (string)CombatData[var];
        }
        public static void KeyHandle(Keys keycode)
        {
            if (!ChatInputMode && keycode == Keys.RControlKey)
            {
                ChatInputMode = true;
                MyChatName = "Type Message> ";
                GameManagerUI.BlockOrUnblockAllActions(true);
                return;
            }
            if (ChatInputMode)
            {
                if (Keys.Back == keycode && ChatTextInput.Length > 0)
                    ChatTextInput = ChatTextInput.Substring(0, ChatTextInput.Length - 1);
                else if (keycode == Keys.Enter || keycode == Keys.Escape)
                {
                    ChatTextInput.Trim();
                    if (ChatTextInput.Length > 1 & keycode == Keys.Enter)
                    {
                        string re = ClientCommandHandler.CommandLST(ClientV2.UDP_CLIENT, ChatTextInput, ChatTextInput.GetArgs());
                        if (ChatTextInput.StartsWith("/"))
                            PrevChatText += string.IsNullOrEmpty(re) ? "" : NewLineGame + re;
                        else
                        {
                            PrevChatText += NewLineGame + "[" + DataAPP.NickName + "] " + ChatTextInput;
                            if(IsConnected) ClientSender.SendData(ClientV2.UDP_CLIENT, (int)RecvSendTypes.SND_COMMANDORCHATMSG, ChatTextInput);
                        }
                    }
                    ChatTextInput = "";
                    ChatInputMode = false;
                    MyChatName = "";
                    if (PrevChatText.Replace(NewLineGame, "\n").Split('\n').Length > 5) 
                        PrevChatText = PrevChatText.Replace(NewLineGame, "\n").Split('\n').Last();
                    Thread.Sleep(200);
                }
                else if(GetKey(keycode) != '\x00')
                {
                    bool ctrl = mykeyboard.CtrlKeyDown;
                    GameManagerUI.BlockOrUnblockAllActions(ChatInputMode);
                    ChatTextInput += ctrl && keycode == Keys.V ? (string)Clipboard.GetDataObject().GetData(DataFormats.Text, true) : GetKey(keycode).ToString();
                }
                GameManagerUI.BlockOrUnblockAllActions(ChatInputMode);
                GameManagerUI.ChatUpdate(PrevChatText + NewLineGame + MyChatName + ChatTextInput);
            }
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
