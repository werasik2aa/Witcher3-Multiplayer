using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher3_Multiplayer.Game
{
    public static class GameManagerUI
    {
        public static void UnpauseGame()
        {
            SocketV2.NoRead("UnpauseGame");
        }
        public static bool IsInMenu()
        {
            return SocketV2.ReadBool("IsInMenu", "INMENUDATA");
        }
        public static bool IsGamePaused()
        {
            return SocketV2.ReadBool("IsGamePaused", "INPAUSED");
        }
        public static bool IsGameStopped()
        {
            return SocketV2.ReadBool("IsGameStoped", "INSTUPPED");
        }
        public static void InitGame(string template)
        {
            SocketV2.NoRead("StartMP(\"" + template + "\")");
        }
        public static void BlockOrUnblockAllActions(bool state)
        {
            SocketV2.NoRead("BlockAllActions(" + state.ToString().ToLower() + ")");
        }
        public static bool IsGameNotLaunched()
        {
            return IsInMenu() & IsGameStopped() & IsGamePaused();
        }
        public static bool IsPaused()
        {
            return IsGameStopped() || IsGamePaused();
        }
    }
    public static class GameManager
    {
        public static void CommandExec(string data)
        {
            SocketV2.NoRead(data);
        }
        public static void Spawn_Player(string template, string name, Vector3 Pos_P, Vector3 Pos_H)
        {
            SocketV2.NoRead("Spawn_Player(\"" + template + "\", \"" + name + "\", " + Pos_P.ToString() + ", " + Pos_H.ToString());
        }
        public static void Spawn_NPC(string template, string name, Vector3 Pos_P, Vector3 Pos_H)
        {
            SocketV2.NoRead("Spawn_NPC(\"" + template + "\", \"" + name + "\", " + Pos_P.ToString() + ", " + Pos_H.ToString() + ")");
        }
        public static void Spawn_Corpse(string template, Vector3 Pos)
        {
            SocketV2.NoRead("Spawn_Corpse(" + template + ", " + Pos.ToString() + ")");
        }
        public static void TeleportClient(Vector3 Pos, bool IsOnHorse)
        {
            SocketV2.NoRead("TeleportClient(" + Pos.ToString() + ", " + IsOnHorse.ToString().ToLower() + ")");
        }
        public static void TeleportByID(int id, Vector3 Pos, bool isPlayer)
        {
            SocketV2.NoRead("TeleportEntityByID(" + id + ", " + Pos.ToString() + ", " + isPlayer.ToString().ToLower() + ")");
        }
        public static void SetEntityAIPositionByID(int id, Vector3 Pos, bool isPlayer, bool IsHorse)
        {
            SocketV2.NoRead("SetPositionByID(" + id + ", " + Pos.ToString() + ", " + isPlayer.ToString().ToLower() + ", " + IsHorse.ToString().ToLower() + ")");
        }
        public static void Zaloopa()
        {
            SocketV2.NoRead("ZaLoopa");
        }
        public static void KillEntityByID(int id)
        {
            SocketV2.NoRead("KillEntityByID(" + id + ")");
        }
        public static void DamageEntity(int id, int clienthealth)
        {
            SocketV2.NoRead("DamageEntity(" + id + ", " + clienthealth + ")");
        }
        public static void MountPlayerToHorse(int id, bool state)
        {
            SocketV2.NoRead("MountHorse(" + id + ", " + state + ")");
        }
        public static void SetTargetByID(int id, int npcID)
        {
            SocketV2.NoRead("SetTargetByID(" + id + ", " + npcID + ")");
        }
        public static void SetFightStateByID(int id, int AttackType)
        {
            SocketV2.NoRead("SetFightStateByID(" + id + ", " + AttackType + ")");
        }
        public static void SetHealthByID(int id, int health, bool IsPlayer)
        {
            SocketV2.NoRead("SetHealthByID(" + id + ", " + health + ", " + IsPlayer.ToString().ToLower() + ")");
        }
        public static void SetLevelByID(int id, int lvl, bool IsPlayer)
        {
            SocketV2.NoRead("SetLevelByID(" + id + ", " + lvl + ", " + IsPlayer.ToString().ToLower() + ")");
        }
        public static void SetAngleByID(int id, float health, bool IsPlayer, bool IsHorse)
        {
            SocketV2.NoRead("SetAngleByID(" + id + ", " + health + ", " + IsPlayer.ToString().ToLower() + ", " + IsHorse.ToString().ToLower() + ")");
        }
        public static PlayerData ReadPlayerData(int id, string name, string template)
        {
            return SocketV2.ReadDataJson("GetPlayerData", "PLAYERDATA").ReadPlayerData(id, name, template);
        }
        public static EntityInfo ReadCombatTarget()
        {
            return SocketV2.ReadDataJson("GetTargetData", "COMBATDATA").ReadEntityInfo();
        }
        public static EntityInfo ReadEntityInfoByID(int id)
        {
            return SocketV2.ReadDataJson("GetEntityDataByID(" + id + ")", "ENTITYDATA").ReadEntityInfo();
        }
        public static void OnConnected(string nick)
        {
            SocketV2.NoRead("OnConnectedMSG(" + nick + ")");
        }
        public static void OnDisconnected(string nick)
        {
            SocketV2.NoRead("OnDisConnectedMSG(" + nick + ")");
        }
    }
}
