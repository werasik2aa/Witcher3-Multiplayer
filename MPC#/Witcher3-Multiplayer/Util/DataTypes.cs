using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Witcher3_Multiplayer.Game
{
    [Serializable]
    public struct FightInfo
    {
        public int FighState;
        public string FightName;
        public int TargetGuid;
    }
    [Serializable]
    public struct EntityInfo
    {
        public string Template;
        public int HP;
        public int Guid;
        public Vector3 Position;
        public bool IsDead;
        public bool IsBoss;
        public bool IsMonster;
        public bool IsOnHorse;
        public bool Equals(EntityInfo cbdata)
        {
            return cbdata.IsDead == IsDead & 
                cbdata.Guid == Guid & 
                cbdata.HP == HP & 
                cbdata.IsMonster == IsMonster & 
                cbdata.Position.Comapare(Position);
        }
    }
    [Serializable]
    public struct PlayerData
    {
        public double Version;
        public string NickName;
        public string CharacterTemplate;
        public int ID;
        public bool IsOnHorse;
        public bool IsDead;
        public bool ISMainCharacter;
        public int HP;
        public int AreaID;
        public int Plevel;
        public int AnimState;
        public Vector3 PlayerPosition;
        public Vector3 HorsePosition;
        public float PlayerAngle;
        public float HorseAngle;
        public FightInfo FightData;
    }
    [Serializable]
    public struct ServerInfo
    {
        public string Name;
        public int CurPlayers;
        public int MaxPlayers;
        public double Version;
        public int DifficultyLevel;
        public int TimeAsMS;
        public bool RequireCheckVersion;
    }
    [Serializable]
    public struct Vector3
    {
        public float x;
        public float y;
        public float z;
        public Vector3(float X=0, float Y=0, float Z=0)
        {
            x = X;
            y = Y;
            z = Z;
        }
        public Vector3(string raw)
        {
            string[] splitted = raw.Split(' ');
            x = float.Parse(splitted[0]);
            y = float.Parse(splitted[1]);
            z = float.Parse(splitted[2]);
        }
        override public string ToString()
        {
            return Math.Round(x) + ", " + Math.Round(y) + ", " + Math.Round(z);
        }
        public bool Equals(Vector3 v)
        {
            return this.Comapare(v);
        }
    }
    [Serializable]
    public struct Quaternion
    {
        public float x;
        public float y;
        public float z;
        public Quaternion(float X, float Y, float Z)
        {
            x = X;
            y = Y;
            z = Z;
        }
        override public string ToString()
        {
            return Math.Round(x) + ", " + Math.Round(y) + ", " + Math.Round(z);
        }
    }
    [Serializable]
    public struct Vector2
    {
        public float x;
        public float y;
        public Vector2(float X, float Y)
        {
            x = X;
            y = Y;
        }
        override public string ToString()
        {
            return Math.Round(x) + ", " + Math.Round(y);
        }
    }
    public static class GameMath
    {
        public static double GetDistanceTO(this Vector3 a, Vector3 b)
        {
            return Math.Sqrt(Math.Pow((b.x - a.x), 2) + Math.Pow((b.y - a.y), 2));
        }
        public static double GetDistanceVTOV(Vector3 a, Vector3 b)
        {
            return Math.Sqrt(Math.Pow((b.x - a.x), 2) + Math.Pow((b.y - a.y), 2));
        }
        public static bool Comapare(this Vector3 a, Vector3 b)
        {
            return a.x == b.x & a.y == b.y & a.y == b.y;
        }
        public static bool Comapare(this Quaternion a, Quaternion b)
        {
            return a.x == b.x & a.y == b.y & a.y == b.y;
        }
        public static float VectorAngleF(this Vector3 a, Vector3 b)
        {
            return (float)Math.Atan2(b.y - a.y, b.x - a.x);
        }
        public static bool IsSame(this EntityInfo cbdata, EntityInfo data)
        {
            return cbdata.Guid == data.Guid;
        }
        public static EntityInfo ReadEntityInfo(this JObject CombatData)
        {
            return new EntityInfo()
            {
                Guid = (int)CombatData["COMBATTARGGUID"],
                Template = (string)CombatData["COMBATTARGNAME"],
                HP = (int)CombatData["COMBATTHP"],
                Position = new Vector3((string)CombatData["COMBATTARGPOS"]),
                IsDead = (bool)CombatData["ISDEAD"]
            };
        }
        public static PlayerData ReadPlayerData(this JObject PlayerData, int id, string name, string template)
        {
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
                PlayerAngle = (float)PlayerData["ANGLEP"],
                HorseAngle = (float)PlayerData["ANGLEH"],
                CharacterTemplate = template,
                FightData = new FightInfo()
                {
                    FighState = (int)PlayerData["FIGHTACTION"],
                    TargetGuid = (int)PlayerData["TARGETGUID"],
                    FightName = (string)PlayerData["ANSTATENAME"]
                }
            };
        }
    }
}
