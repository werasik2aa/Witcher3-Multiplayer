using System;
using System.Diagnostics;
using Witcher3_Multiplayer.ClientHost;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public static class GameManagerUI
    {
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
            if (string.IsNullOrEmpty(a))
                return IsInMenu();
            return bool.Parse(a);
        }
        public static bool IsGamePaused()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsGamePaused"));
            string a = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(a)) return IsGamePaused();
            return bool.Parse(a);
        }
        public static bool IsGameStoped()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsGameStoped"));
            string a = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(a))
                return IsGameStoped();

            return bool.Parse(a);
        }
        public static string SetDebugState(int i)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetHostDebugMode("+i+")"));
            string a = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(a)) SetDebugState(i);
            return a;
        }
    }
    public static class GameManagerMY
    {
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
            if (string.IsNullOrEmpty(adata)) return GetPlayerHP();
            return int.Parse(adata.Trim().Split('.')[0]);
        }
        public static int GetPlayerLevel()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPLevel"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerLevel();
            return int.Parse(adata);
        }
        public static int GetPlayerAreaID()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentLevelId"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerLevel();
            return int.Parse(adata);
        }
        public static int GetPlayerStateInt()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnimInt"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerStateInt();
            return int.Parse(adata);
        }
        public static string GetPlayerStateName()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnimName"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerStateName();
            return adata;
        }
        public static Vector3 GetPlayerPosition()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPosition"));
            string raw = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(raw) || raw.Contains("err") || raw.Contains("error")) GetPlayerPosition();
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
            if (string.IsNullOrEmpty(raw) || raw.Contains("err") || raw.Contains("error")) return GetPlayerRotationWorld();
            string[] splitted = raw.Replace('.', ',').Split(' ');
            Quaternion f = new Quaternion()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
        public static int GetPlayerHeading()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentHeading"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerHeading();
            return int.Parse(adata.Trim().Split('.')[0]);
        }
        public static int GetCurrentPlayers()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPlayers"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetCurrentPlayers();
            return int.Parse(adata.Substring(0, 2));
        }
        public static string SetPlayerHeading(int id, Vector3 pos)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetHeading_Player("+id + ", " + postr + ")"));
            string adata = SocketManager.ReciveData_DoWork();
            return adata;
        }
        public static string SetPlayerMovingType(int id, int state)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetTypeMoving_Player(" + id + ", " + state + ")"));
            string adata = SocketManager.ReciveData_DoWork();
            return adata;
        }
        public static int GetPlayerMovingType()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetMovingTypeState"));
            string adata = SocketManager.ReciveData_DoWork();
            if (string.IsNullOrEmpty(adata)) return GetPlayerMovingType();
            return int.Parse(adata.Substring(0, 1));
        }
        public static string SpawnPlayer(int idofcharact, Vector3 pos, Quaternion rot)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Spawn_Player(" + idofcharact + ", " + postr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SpawnPlayer(string Template, Vector3 pos, Quaternion rot)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Spawn_PlayerV2(" + Template + ", " + postr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SpawnNPC(string Template, Vector3 pos, Quaternion rot)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Spawn_NPC(" + Template + ", " + postr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string TeleportPlayer(int id, Vector3 pos)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Teleport_Player(" + id + ", " + postr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SetFollowToPlayer(int id, Vector3 pos)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetMoveTo_Player(" + id + ", " + postr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string TeleportRotatePlayer(int id, Vector3 pos, Quaternion rot)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            string rotstr = Math.Round(rot.x) + ", " + Math.Round(rot.y) + ", " + Math.Round(rot.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("TeleportRotate_Player(" + id + ", " + postr + ", " + rotstr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string RotatePlayer(int id, Quaternion rot)
        {
            string rotstr = Math.Round(rot.x) + ", " + Math.Round(rot.y) + ", " + Math.Round(rot.z);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Rotate_Player(" + id + ", " + rotstr + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string Spawn_Player(string nick, int clientid, string data, Vector3 pos, Quaternion rot)
        {
            string postr = Math.Round(pos.x) + ", " + Math.Round(pos.y) + ", " + Math.Round(pos.z);
            string rotstr = Math.Round(rot.x) + ", " + Math.Round(rot.y) + ", " + Math.Round(rot.z);
            string donestr = nick + ", " + clientid + ", \"" + data + "\", " + postr + ", " + rotstr;
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
    }
}
