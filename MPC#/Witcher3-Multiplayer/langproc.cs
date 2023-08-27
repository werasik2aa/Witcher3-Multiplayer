﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Witcher3_Multiplayer.ClientHost;
namespace Witcher3_Multiplayer
{
    public static class langproc
    {
        public static Settings DataAPP = new Settings();
        [Serializable]
        public struct Settings
        {
            public string character;
            public string NickName;
            public string ServerIP;
            public string GamePath;
            public int ClientPort;
            public int ServerPort;
            public int ServerMaxPlayers;
            public string ServerPassword;
            public bool ClientRCON;
            public string ServerName;
            public string ServerConsolePassword;
            public bool ServerSyncChestItems;
            public bool Debug;
            public bool ServerDedicated;
            public bool ServerRCON;
            public bool ServerCheckClientVersion;
            public bool JoinTestLocalClient;
        }
        public static TextBox LOGGERB;
        public static bool IsHost = false, IsConnected = false, AllowLocalDisc = false;
        public static double VersionCur = 1.0;
        public static int SendDataDelayC_S_C = 64;
        public static int CheckAndSendDataDelayCMISC_S_CALL = 64;
        public static Main MForm;
        public static string ChatInp = "";
        public static string MonstersPath = "characters\\npc_entities\\monsters\\";//+MONSTERNAME

        public static List<PlayerServer> ServerData = new List<PlayerServer>();//sender
        public static Dictionary<IPEndPoint, PlayerData> PlayerDataServer = new Dictionary<IPEndPoint, PlayerData>();//PDATA ON SERVER
        public static List<ChestData> ChestDatasServer = new List<ChestData>(); //CHEST ITEMS TO RESEND

        public static Dictionary<int, PlayerData> PlayerDataClient = new Dictionary<int, PlayerData>();//PDATA CLIENT
        public static List<CombatTarget> CombatTargetsClient = new List<CombatTarget>(); //COMBAT TARGET
        public static List<ChestData> ChestDatasClient = new List<ChestData>(); // CHEST ITEMS
        public static void LOG(string s)
        {
            Action t = () =>
            {
                LOGGERB.Text += ("[INFO] " + s + Environment.NewLine);
                LOGGERB.SelectionStart = LOGGERB.Text.Length;
                LOGGERB.ScrollToCaret();
            };
            if (LOGGERB.InvokeRequired)
                LOGGERB.Invoke(t);
            else
                t();
        }
        public static void ELOG(string s)
        {
            Action t = () => { LOGGERB.Text += "[ERROR] " + s + Environment.NewLine; };
            if (LOGGERB.InvokeRequired)
                LOGGERB.Invoke(t);
            else
                t();
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
            T obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
        public static void SeriliazeToFile(this object structure, string path)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, structure);
            File.WriteAllBytes(path, ms.ToArray());
            LOG("[host] Data Saved: " + path);
        }
        public static T FileToStructure<T>(string path) where T : struct
        {
            MemoryStream memStream = new MemoryStream(File.ReadAllBytes(path));
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Seek(0, SeekOrigin.Begin);
            T obj = (T)binForm.Deserialize(memStream);
            return obj;
        }
    }
}
