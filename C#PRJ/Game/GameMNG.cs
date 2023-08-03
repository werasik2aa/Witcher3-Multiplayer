using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static Witcher3_Multiplayer.ClientHost.DataTypes;
using static Witcher3_Multiplayer.langproc;
namespace Witcher3_Multiplayer.Game
{
    public static class GameManager
    {
        public static string ConsoleCommand(string cmd)
        {
            if (debug)
                LOG("Executing: " + cmd);
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute(cmd));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SendChatNotify(string msg)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("NotifyChatMSG(" + msg + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string Teleport(int id, Vector3 positon)
        {
            string pos = positon.x + " " + positon.y + " " + positon.z;
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("TeleportEntityById(" + pos + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SetPositionWalkTo(int id, Vector3 pos)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("EntityMoveTo(" + id + ", " + pos.x + ", " + pos.y + ", " + pos.z + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SpawnNPCEntity(string data, Vector3 pos, Quaternion rot)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SpawnNpcEntityE(" + data + ", " + pos.x + ", " + pos.y + ", " + pos.z + ", " + rot.x + ", " + rot.y + ", " + rot.z + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SpawnNPCEntity(int data, Vector3 pos, Quaternion rot)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("Joined(" + data + ", " + pos.x + ", " + pos.y + ", " + pos.z + ", " + rot.x + ", " + rot.y + ", " + rot.z + ")"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetPlayerNameFromGame()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("headname"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetPlayerNameV2FromGame()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetPlayerName2"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetPlayerNameFromServer()
        {
            //FUCK U
            return SocketManager.ReciveData_DoWork();
        }
        public static string GetTime()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("telltime"));
            return SocketManager.ReciveData_DoWork();
        }
        public static int GetLevelId()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetLevelId"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static int GetHP()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetMyCurrentHP"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static int GetPLevel()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetMyCurrentPLevel"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static bool IsInMenu()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsInMenu"));
            return bool.Parse(SocketManager.ReciveData_DoWork());
        }
        public static bool IsGameStoped()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("IsGameStoped"));
            return bool.Parse(SocketManager.ReciveData_DoWork());
        }
        public static int GetHP(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentHP(id)"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static int GetPLevel(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetCurrentPLevel(" + id + ")"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static int GetStateAnim(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnim(" + id + ")"));
            return int.Parse(SocketManager.ReciveData_DoWork());
        }
        public static string GetStateAnim()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetStateAnim"));
            return SocketManager.ReciveData_DoWork();
        }
        public static string SetStateAnim(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("SetStateAnim"));
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
        public static Vector3 GetPosition(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetEntityPosition(" + id + ")"));
            string raw = SocketManager.ReciveData_DoWork();
            string[] splitted = raw.Split(' ');
            Vector3 f = new Vector3()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
        public static Vector3 GetPosition()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetMyPosition"));
            string raw = SocketManager.ReciveData_DoWork().Trim().Replace(".", ",");
            string[] splitted = raw.Split(' ');
            Vector3 f = new Vector3()
            {
                x = Single.Parse(splitted[0]),
                y = Single.Parse(splitted[1]),
                z = Single.Parse(splitted[2])
            };
            return f;
        }
        public static Quaternion GetRotation()
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetMyRotation"));
            string raw = SocketManager.ReciveData_DoWork().Trim().Replace(".", ","); ;
            string[] splitted = raw.Split(' ');
            Quaternion f = new Quaternion()
            {
                x = Single.Parse(splitted[0]),
                y = Single.Parse(splitted[1]),
                z = Single.Parse(splitted[2])
            };
            return f;
        }
        public static Quaternion GetRotation(int id)
        {
            SocketManager.Send(SocketManager.GameSocket, Convertors.Execute("GetEntityRotation(" + id + ")"));
            string raw = SocketManager.ReciveData_DoWork();
            string[] splitted = raw.Split(':');
            Quaternion f = new Quaternion()
            {
                x = float.Parse(splitted[0]),
                y = float.Parse(splitted[1]),
                z = float.Parse(splitted[2])
            };
            return f;
        }
    }
}
