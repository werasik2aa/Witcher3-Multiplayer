using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public static class GameManagerUI
    {
        public static bool CheckData(string a)
        {
            a = a.ToLower().Trim();
            if (a.Contains("with error"))
            {
                if (IsHost) ServerV2.StopServer();
                if (IsConnected) ClientV2.Disconnect();
                IsHost = IsConnected = false;
                LOG("Version of this API whic has this mod outdated! And command return null! Please Update Client TO Match ModVersion");
            }
            if (string.IsNullOrEmpty(a) || !a.Contains("w3mp ")) return true;
            return false;
        }
        public static string SendChatNotify(string msg)
        {
            SocketV2.Write(Convertors.Execute("NotifyChatMSG(" + msg + ")"));
            return SocketV2.Read();
        }
        public static string UnpauseGame()
        {
            SocketV2.Write(Convertors.Execute("UnpauseGame"));
            return SocketV2.Read();
        }
        public static bool IsInMenu()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsInMenu"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || (!adata.ToLower().Contains("true") & !adata.ToLower().Contains("false"))) return IsInMenu();
                return bool.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-IsInMenu");
                return IsInMenu();
            }
        }
        public static bool IsGamePaused()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsGamePaused"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || (!adata.ToLower().Contains("true") & !adata.ToLower().Contains("false"))) return IsGamePaused();
                return bool.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-IsGamePaused");
                return IsGamePaused();
            }
        }
        public static bool IsGameStopped()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsGameStoped"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || (!adata.ToLower().Contains("true") & !adata.ToLower().Contains("false")))
                    return IsGameStopped();

                return bool.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-IsGameStopped");
                return IsGameStopped();
            }
        }
        public static string SetDebugState(int i)
        {
            SocketV2.Write(Convertors.Execute("SetHostDebugMode("+i+")"));
            string a = SocketV2.Read();
            return a;
        }
    }
    public static class GameManagerMY
    {
        public static bool CheckData(string a)
        {
            a = a.ToLower().Trim();
            if (a.Contains("with error"))
            {
                if (IsHost) ServerV2.StopServer();
                if (IsConnected) ClientV2.Disconnect();
                IsHost = IsConnected = false;
                LOG("Version of this API whic has this mod outdated! And command return null! Please Update Client TO Match ModVersion");
            }
            if (string.IsNullOrEmpty(a) || !a.Contains("w3mp ")) return true;
            return false;
        }
        public static string ExecConsoleCommand(string cmd)
        {
            if (debug) LOG("Executing: " + cmd);
            SocketV2.Write(Convertors.Execute(cmd));
            return SocketV2.Read();
        }
        public static string SetCurrentPlayers(int icount)
        {
            SocketV2.Write(Convertors.Execute("SetCurPlayers(" + icount + ")"));
            return SocketV2.Read();
        }
        public static string KillEntityByGuid(int Guid, string Template, int idclien)
        {
            SocketV2.Write(Convertors.Execute("KillActorByGuidHash(" + Guid + ", " + Template + ", " + idclien + ")"));
            string adata = SocketV2.Read();
            return adata;
        }
        public static string CheckCombatTargetOrSpawnIt(int Guid, string Template, int idclien)
        {
            SocketV2.Write(Convertors.Execute("CheckCombatTarget(" + Guid + ", " + Template + ", " + idclien + ")"));
            string adata = SocketV2.Read();
            return adata;
        }
        public static int GetCombatTargetGuid()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCombatTargetGuid"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetCombatTargetGuid();
                return int.Parse(adata.Replace("W3MP ", ""));
            } catch
            {
                LOG("CATCH-GetCombatTargetGuid");
                return GetCombatTargetGuid();
            }
        }
        public static bool IsInFight()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsInCombat"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || !adata.Contains("W3MP OHC")) return IsInFight();
                return bool.Parse(adata.Replace("W3MP OHC", ""));
            }
            catch
            {
                LOG("CATCH-IsInFight");
                return IsInFight();
            }
        }
        public static string GetCombatTargetName()
        {
            SocketV2.Write(Convertors.Execute("GetCombatTargetName"));
            string adata = SocketV2.Read();
            return adata;
        }
        public static bool GetCombatTargetIsDead(int guid)
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsCombatTargetDead(" + guid + ")"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || (!adata.ToLower().Contains("true") & !adata.ToLower().Contains("false"))) return GetCombatTargetIsDead(guid);
                return bool.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-GetCombatTargetIsDead");
                return GetCombatTargetIsDead(guid);
            }
        }
        public static int GetCombatStateINT()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetFightSwStateName"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || !adata.Contains("W3MP F")) return GetCombatStateINT();
                return int.Parse(adata.Replace("W3MP F", "").Trim());
            }
            catch
            {
                LOG("CATCH-GetCombatStateINT");
                return GetCombatStateINT();
            }
        }
        public static string Attack(int idcl, int state, int guid)
        {
            SocketV2.Write(Convertors.Execute("Attack(" + idcl + "," + state + ", " + guid + ")"));
            string adata = SocketV2.Read();
            return adata;
        }
        public static int GetPlayerHP()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentHP"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetPlayerHP();
                return int.Parse(adata.Replace("W3MP ", "").Trim().Split('.')[0]);
            }
            catch
            {
                LOG("CATCH-GetPlayerHP");
                return GetPlayerHP();
            }
        }
        public static int GetPlayerLevel()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentPLevel"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetPlayerLevel();
                return int.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-GetPlayerLevel");
                return GetPlayerLevel();
            }
        }
        public static int GetPlayerAreaID()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentLevelId"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetPlayerAreaID();
                return int.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-GetPlayerLevel");
                return GetPlayerAreaID();
            }
        }
        public static string GetPlayerStateName()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetStateAnimName"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetPlayerStateName();
                return adata;
            }
            catch
            {
                LOG("CATCH-GetPlayerStateName");
                return GetPlayerStateName();
            }
        }
        public static Vector3 GetPlayerPosition()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentPosition"));
                string raw = SocketV2.Read();
                if (CheckData(raw) || !raw.Contains(".") || !raw.Contains("playerAP")) GetPlayerPosition();
                string[] splitted = raw.Replace("W3MP ", "").Replace("playerAP", "").Trim().Replace('.', ',').Split(' ');
                Vector3 f = new Vector3()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
                return f;
            }
            catch
            {
                LOG("CATCH-GetPlayerPosition");
                return GetPlayerPosition();
            }
        }
        public static Quaternion GetCurrentRotation()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentRotation"));
                string raw = SocketV2.Read();
                if (CheckData(raw) || !raw.Contains(".")) GetCurrentRotation();
                string[] splitted = raw.Replace("W3MP ", "").Trim().Replace('.', ',').Split(' ');
                Quaternion f = new Quaternion()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
                return f;
            }
            catch
            {
                LOG("CATCH-GetCurrentRotation");
                return GetCurrentRotation();
            }
        }
        public static Vector3 GetPlayerHorsePosition()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentHorsePosition"));
                string raw = SocketV2.Read();
                if (CheckData(raw) || !raw.Contains(".") || !raw.Contains("horseAP")) GetPlayerHorsePosition();
                string[] splitted = raw.Replace("W3MP ", "").Replace("horseAP", "").Trim().Replace('.', ',').Split(' ');
                Vector3 f = new Vector3()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
                return f;
            }
            catch
            {
                LOG("CATCH-GetPlayerHorsePosition");
                return GetPlayerHorsePosition();
            }
        }
        public static Quaternion GetPlayerRotationWorld()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentRotation"));
                string raw = SocketV2.Read();
                if (CheckData(raw) || !raw.Contains(".")) return GetPlayerRotationWorld();
                string[] splitted = raw.Replace("W3MP ", "").Replace('.', ',').Split(' ');
                Quaternion f = new Quaternion()
                {
                    x = float.Parse(splitted[0]),
                    y = float.Parse(splitted[1]),
                    z = float.Parse(splitted[2])
                };
                return f;
            }
            catch
            {
                LOG("CATCH-GetPlayerRotationWorld");
                return GetPlayerRotationWorld();
            }
        }
        public static bool GetPlayerIsOnHorse()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("IsOnHorse"));
                string adata = SocketV2.Read();
                if (CheckData(adata) || !adata.Contains("W3MP ONH")) return GetPlayerIsOnHorse();
                return bool.Parse(adata.Replace("W3MP ONH", "").Trim());
            }
            catch
            {
                LOG("CATCH-GetPlayerIsOnHorse");
                return GetPlayerIsOnHorse();
            }
        }
        public static string SetPlayerIsOnHorse(int client, int state)
        {
            SocketV2.Write(Convertors.Execute("MountHorse(" + client + ", " + state +")"));
            string adata = SocketV2.Read();
            return adata;
        }
        public static int GetCurrentPlayers()
        {
            try
            {
                SocketV2.Write(Convertors.Execute("GetCurrentPlayers"));
                string adata = SocketV2.Read();
                if (CheckData(adata)) return GetCurrentPlayers();
                LOG("WTF: " + adata);
                return int.Parse(adata.Replace("W3MP ", ""));
            }
            catch
            {
                LOG("CATCH-GetCurrentPlayers");
                return GetCurrentPlayers();
            }
        }
        public static string TeleportPlayer(int id, Vector3 pos)
        {
            SocketV2.Write(Convertors.Execute("Teleport_Player(" + id + ", " + pos.ToString() + ")"));
            return SocketV2.Read();
        }
        public static string SetPlayerMoveTo(int id, Vector3 pos)
        {
            SocketV2.Write(Convertors.Execute("SetMoveTo_Player(" + id + ", " + pos.ToString() + ")"));
            return SocketV2.Read();
        }
        public static string Remove_Player(int clientid)
        {
            SocketV2.Write(Convertors.Execute("OnDisconnect(" + clientid + ")"));
            return SocketV2.Read();
        }
        public static string Spawn_Player(string nick, int clientid, string data, Vector3 posp, Vector3 pos2h)
        {
            string donestr = nick + ", " + clientid + ", \"" + data + "\", " + posp.ToString() + ", " + pos2h.ToString();
            SocketV2.Write(Convertors.Execute("Spawn_Player(" + donestr + ")"));
            return SocketV2.Read();
        }
        public static string Spawn_CORPSE(string data, Vector3 pos)
        {
            string donestr = data + "\", " + pos.ToString();
            SocketV2.Write(Convertors.Execute("Spawn_CORPSE(" + donestr + ")"));
            return SocketV2.Read();
        }
        public static string GetPlayerNameFromGame()
        {
            SocketV2.Write(Convertors.Execute("headname"));
            return SocketV2.Read();
        }
        public static string GetTime()
        {
            SocketV2.Write(Convertors.Execute("telltime"));
            return SocketV2.Read();
        }
        public static string SetWeather(int state)
        {
            WeatherState id = (WeatherState)state;
            switch (id)
            {
                case WeatherState.STORM:
                    SocketV2.Write(Convertors.Execute("makeitrain"));
                    break;
                case WeatherState.RAINI:
                    SocketV2.Write(Convertors.Execute("stoprain"));
                    break;
            }
            return SocketV2.Read();
        }
        public static void RunGame(string path)
        {
            path += "\\bin\\x64\\witcher3.exe";
            Process p = new Process();
            p.StartInfo.WorkingDirectory = path;
            p.StartInfo.FileName = path;
            p.StartInfo.Arguments = "-net -debugscripts";
            p.Start();
        }
    }
}
