using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Witcher3_Multiplayer.ClientHost
{
    [Serializable]
    public struct ServerPacket
    {
        public PacketType Header;
        public int ClientID;
        public byte[] Buffer;
    }
    [Serializable]
    public struct ClientPacket
    {
        public PacketType Header;
        public byte[] Buffer;
    }

    public enum PacketType
    {
        //SND
        SND_PLAYERINFO,
        SND_PLAYERTARGETINFO,
        SND_ENTITYINFO,
        SND_PLAYERPOSITION,
        SND_PLAYERONHORSE,
        SND_PLAYERHORSEPOSITION,
        SND_PLAYERHP,
        SND_PLAYERAREAMAP,
        SND_PLAYERPLEVEL,
        SND_NPCPOSITION,
        SND_DISCONNECT,
        SND_CHATMSG,
        SND_ANGLEP,
        SND_ANGLEH,
        SND_FIGHTSTATE,

        //REQ
        REQ_CONNECTED,
        REQ_PLAYERDATA,

        //RESPONSE
        RES_BEGIN,
        RES_END,
        RES_CON_NOTALLOWED1,
        RES_CON_NOTALLOWED2,
        RES_PLAYERINFO,
        RES_DISCONNECTED,
        RES_CHATMSG,
        RES_ENTITYINFO,
        RES_PLAYERTARGETINFO,
    }
}
