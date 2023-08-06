using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Ink;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public static class GameManagerUI
    {
        public static bool CheckData(string a)
        {
            a = a.ToLower();
            if (a.Contains("with error"))
                throw new Exception("IS CLIENT MOD OUTDATED?");
            if (string.IsNullOrEmpty(a) || a.Contains("flag is")) return true;
            if (a.ToLower().Equals("true"))
                return false;
            if (a.ToLower().Equals("false"))
                return false;
            if (a.ToLower().Contains(".") || a.ToLower().Contains(","))
                return false;
            if (Regex.IsMatch(a.Replace(" ", ""), @"\p{L}"))
                return false;
            return false;
        }
        public static string SendChatNotify(string msg)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("NotifyChatMSG(" + msg + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string UnpauseGame()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("UnpauseGame"));
            return SocketManager.ReciveData_DoWork();
        }
        public static bool IsInMenu()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsInMenu"));
            string a = SocketManager.ReciveData_DoWork();
            if (CheckData(a))
                return IsInMenu();
            return bool.Parse(a);
        }
        public static bool IsGamePaused()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsGamePaused"));
            string a = SocketManager.ReciveData_DoWork();
            if (CheckData(a)) return IsGamePaused();
            return bool.Parse(a);
        }
        public static bool IsGameStoped()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsGameStoped"));
            string a = SocketManager.ReciveData_DoWork();
            if (CheckData(a))
                return IsGameStoped();

            return bool.Parse(a);
        }
        public static string SetDebugState(int i)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetHostDebugMode("+i+")"));
            string a = SocketManager.ReciveData_DoWork();
            if (CheckData(a)) SetDebugState(i);
            return a;
        }
    }
    public static class GameManagerMY
    {
        public static bool CheckData(string a)
        {
            a = a.ToLower();
            if (a.Contains("with error"))
                throw new Exception("IS CLIENT MOD OUTDATED?");
            if (string.IsNullOrEmpty(a) || a.Contains("flag is")) return true;
            if (a.ToLower().Equals("true"))
                return false;
            if (a.ToLower().Equals("false"))
                return false;
            if (a.ToLower().Contains(".") || a.ToLower().Contains(","))
                return false;
            if (Regex.IsMatch(a.Replace(" ", ""), @"\p{L}")) 
                return false;
            return false;
        }
        public static string ExecConsoleCommand(string cmd)
        {
            if (debug) LOG("Executing: " + cmd);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute(cmd));
            return SocketManager.ReciveData_DoWork();
        }
        public static int GetPlayerHP()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentHP"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata) || !adata.Contains(".")) return GetPlayerHP();
            return int.Parse(adata.Trim().Split('.')[0]);
        }
        public static int GetPlayerLevel()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPLevel"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetPlayerLevel();
            return int.Parse(adata);
        }
        public static int GetPlayerAreaID()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentLevelId"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetPlayerLevel();
            return int.Parse(adata);
        }
        public static int GetPlayerStateInt()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnimInt"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetPlayerStateInt();
            return int.Parse(adata);
        }
        public static string GetPlayerStateName()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnimName"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetPlayerStateName();
            return adata;
        }
        public static Vector3 GetPlayerPosition()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPosition"));
            string raw = SocketManager.ReciveData_DoWork();
            if (CheckData(raw) || !raw.Contains(".")) GetPlayerPosition();
            string[] splitted = raw.Trim().Replace('.', ',').Split(' ');
            Vector3 f = new Vector3()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
        public static Vector3 GetPlayerHorsePosition()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentHorsePosition"));
            string raw = SocketManager.ReciveData_DoWork();
            if (CheckData(raw) || !raw.Contains(".")) GetPlayerPosition();
            string[] splitted = raw.Trim().Replace('.', ',').Split(' ');
            Vector3 f = new Vector3()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
        public static Quaternion GetPlayerRotationWorld()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentRotation"));
            string raw = SocketManager.ReciveData_DoWork();
            if (CheckData(raw) || !raw.Contains(".")) return GetPlayerRotationWorld();
            string[] splitted = raw.Replace('.', ',').Split(' ');
            Quaternion f = new Quaternion()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
        public static bool GetPlayerIsOnHorse()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsOnHorse"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetPlayerIsOnHorse();
            return bool.Parse(adata.Trim());
        }
        public static string SetPlayerIsOnHorse(int client, int state)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("MountHorseV1(" + client + ", " + state +")"));
            string adata = SocketManager.ReciveData_DoWork();
            return adata;
        }
        public static int GetCurrentPlayers()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPlayers"));
            string adata = SocketManager.ReciveData_DoWork();
            if (CheckData(adata)) return GetCurrentPlayers();
            return int.Parse(adata);
        }
        public static string TeleportPlayer(int id, Vector3 pos)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Teleport_Player(" + id + ", " + pos.ToString() + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SetPlayerMoveTo(int id, Vector3 pos)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetMoveTo_Player(" + id + ", " + pos.ToString() + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string Spawn_Player(string nick, int clientid, string data, Vector3 pos)
        {
            string donestr = nick + ", " + clientid + ", \"" + data + "\", " + pos.ToString();
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Spawn_Player(" + donestr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetPlayerNameFromGame()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("headname"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetTime()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("telltime"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SetWeather(int state)
        {
            WeatherState id = (WeatherState)state;
            switch (id)
            {
                case WeatherState.STORM:
                    SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("makeitrain"));
                    break;
                case WeatherState.RAINI:
                    SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("stoprain"));
                    break;
            }
            return SocketManager.ReciveData_DoWork();
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
        public static void SpawnHorseLOCAL()
        {
            //SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("instantMount(\"horse\")"));
        }
    }
}
