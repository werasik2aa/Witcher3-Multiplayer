using System.IO;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public static class GameManagerUI
    {
        public static string SendChatNotify(string msg)
        {
            return SocketV2.ReadString(Convertors.Execute("NotifyChatMSG(" + msg + ")"));
        }
        public static string UnpauseGame()
        {
            return SocketV2.ReadString(Convertors.Execute("UnpauseGame"));
        }
        public static bool IsInMenu()
        {
            return SocketV2.ReadBool(Convertors.Execute("IsInMenu"));
        }
        public static bool IsGamePaused()
        {
            return SocketV2.ReadBool(Convertors.Execute("IsGamePaused"));
        }
        public static bool IsGameStopped()
        {
            return SocketV2.ReadBool(Convertors.Execute("IsGameStoped"));
        }
        public static string SetDebugState(int i)
        {
            return SocketV2.ReadString(Convertors.Execute("SetHostDebugMode(" + i + ")"));
        }
        public static string InitGame(string template)
        {
            return SocketV2.ReadString(Convertors.Execute("LaunchCustomFramework(\"" + template + "\")"));
        }
        public static string ChatUpdate(string chatinput)
        {
            return SocketV2.ReadString(Convertors.Execute("UpdateChat(\"" + chatinput + "\")"));
        }
        public static string BlockOrUnblockAllActions(bool state)
        {
            return SocketV2.ReadString(Convertors.Execute("BlockAllActions(" + state.ToString().ToLower() + ")"));
        }
        public static bool IsGameNotLaunched()
        {
            return IsInMenu() & IsGameStopped() & IsGamePaused();
        }
        public static bool IsPausedTWO()
        {
            return IsGameStopped() || IsGamePaused();
        }
    }
    public static class GameManagerMY
    {
        public static string ExecConsoleCommand(string cmd)
        {
            if (DataAPP.Debug) LOG("Executing: " + cmd);
            return SocketV2.ReadString(Convertors.Execute(cmd));
        }
        public static string SetCurrentPlayers(int icount)
        {
            return SocketV2.ReadString(Convertors.Execute("SetCurPlayers(" + icount + ")"));
        }
        public static string KillEntity(int idclien)
        {
            return SocketV2.ReadString(Convertors.Execute("KillEntity(" + idclien + ")"));
        }
        public static int GetCombatTargetGuid()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetCombatTargetGuid"));
        }
        public static bool IsInFight()
        {
            return SocketV2.ReadBool(Convertors.Execute("IsInCombat"));
        }
        public static string GetCombatTargetName()
        {
            return SocketV2.ReadString(Convertors.Execute("GetCombatTargetName"));
        }
        public static bool GetCombatTargetIsDead(int guid)
        {
            return SocketV2.ReadBool(Convertors.Execute("IsCombatTargetDead(" + guid + ")"));
        }
        public static int GetCombatStateINT()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetFightSwStateName"));
        }
        public static string Attack(int idcl, int state, int hp)
        {
            return SocketV2.ReadString(Convertors.Execute("Attack(" + idcl + "," + state + ", " + hp + ")"));
        }
        public static int GetPlayerHP()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetCurrentHP"));
        }
        public static int SetPlayerHP(int idcl, int health)
        {
            return SocketV2.ReadInt(Convertors.Execute("SetPlayerCurrentHP(" + idcl + ", " + health + ")"));
        }
        public static int GetPlayerLevel()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetCurrentPLevel"));
        }
        public static int GetPlayerAreaID()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetCurrentLevelId"));
        }
        public static string GetPlayerStateName()
        {
            return SocketV2.ReadString(Convertors.Execute("GetStateAnimName"));
        }
        public static Vector3 GetPlayerPosition()
        {
            return SocketV2.ReadVector3(Convertors.Execute("GetCurrentPosition"));
        }
        public static Quaternion GetCurrentRotation()
        {
            return SocketV2.ReadQuaternion(Convertors.Execute("GetCurrentRotation"));
        }
        public static Vector3 GetPlayerHorsePosition()
        {
            return SocketV2.ReadVector3(Convertors.Execute("GetCurrentHorsePosition"));
        }
        public static bool GetPlayerIsOnHorse()
        {
            return SocketV2.ReadBool(Convertors.Execute("IsOnHorse"));
        }
        public static string SetPlayerIsOnHorse(int client, int state)
        {
            return SocketV2.ReadString(Convertors.Execute("MountHorse(" + client + ", " + state + ")"));
        }
        public static int GetCurrentPlayers()
        {
            return SocketV2.ReadInt(Convertors.Execute("GetCurrentPlayers"));
        }
        public static string TeleportPlayer(int id, Vector3 pos)
        {
            return SocketV2.ReadString(Convertors.Execute("Teleport_Player(" + id + ", " + pos.ToString() + ")"));
        }
        public static string SetPlayerMoveTo(int id, Vector3 pos)
        {
            return SocketV2.ReadString(Convertors.Execute("SetMoveTo_Player(" + id + ", " + pos.ToString() + ")"));
        }
        public static string SetPlayerHorseMoveTo(int id, Vector3 pos)
        {
            return SocketV2.ReadString(Convertors.Execute("SetMoveTo_PlayerHorse(" + id + ", " + pos.ToString() + ")"));
        }
        public static string Remove_Player(int clientid)
        {
            return SocketV2.ReadString(Convertors.Execute("OnDisconnect(" + clientid + ")"));
        }
        public static string Spawn_Player(string nick, int clientid, string data, Vector3 posp, Vector3 pos2h)
        {
            string donestr = "\"" + nick + "\", " + clientid + ", \"" + data + "\", " + posp.ToString() + ", " + pos2h.ToString();
            return SocketV2.ReadString(Convertors.Execute("Spawn_Player(" + donestr + ")"));
        }
        public static string Spawn_NPC(string template, Vector3 pos, int guid)
        {
            string donestr = "\"" + template + "\", " + pos.ToString() + ", " + guid;
            return SocketV2.ReadString(Convertors.Execute("Spawn_NPC(" + donestr + ")"));
        }
        public static string Spawn_CORPSE(string data, Vector3 pos)
        {
            string donestr = data + "\", " + pos.ToString();
            return SocketV2.ReadString(Convertors.Execute("Spawn_CORPSE(" + donestr + ")"));
        }
        public static string GetPlayerNameFromGame()
        {
            return SocketV2.ReadString(Convertors.Execute("headname"));
        }
        public static string GetTime()
        {
            return SocketV2.ReadString(Convertors.Execute("telltime"));
        }
        public static string SetWeather(int state)
        {
            WeatherState id = (WeatherState)state;
            switch (id)
            {
                case WeatherState.STORM:
                    return SocketV2.ReadString(Convertors.Execute("makeitrain"));
                case WeatherState.RAINI:
                    return SocketV2.ReadString(Convertors.Execute("stoprain"));
                default:
                    return SocketV2.ReadString(Convertors.Execute("stoprain"));
            }
        }
        public static bool RunGame(string path)
        {
            path += "\\bin\\x64";
            if (File.Exists(path + "\\witcher3.exe"))
            {
                LOG("STARTING: " + path + "\\witcher3.exe");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = path;
                p.StartInfo.FileName = "witcher3.exe";
                p.StartInfo.Arguments = "-net";
                p.Start();
                return true;
            }
            return false;
        }
    }
}
