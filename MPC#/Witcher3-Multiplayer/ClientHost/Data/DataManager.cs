using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Witcher3_Multiplayer.Game;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.ClientHost
{
    public class DataManager
    {
        public static JObject PlayerData, CombatData;
        public static Dictionary<int, JObject> QuestData = new Dictionary<int, JObject>();
        public static JObject GetData(string tag, string cmd, string[] args = null)
        {
            if(args != null)
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
    }
}
