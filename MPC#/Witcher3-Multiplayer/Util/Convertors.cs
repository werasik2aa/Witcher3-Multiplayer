using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Witcher3_Multiplayer.Game
{
    public static class Convertors
    {
        public static string NsScriptDebugger = "ScriptDebugger";
        public static string NsScriptProfiler = "ScriptProfiler";
        public static string NsScriptCompiler = "ScriptCompiler";
        public static string NsScripts = "scripts";
        public static string NsRemote = "Remote";
        public static string NsUtility = "Utility";
        public static string NsConfig = "Config";

        public static string CmdBind = "BIND";

        public static byte[] PacketHead = { 0xDE, 0xAD }; // DEAD
        public static byte[] PacketTail = { 0xBE, 0xEF }; // BEEF

        public static byte[] TypeByte = { 0x81, 0x08 };
        public static byte[] TypeStringUtf8 = { 0xAC, 0x08 };
        public static byte[] TypeStringUtf16 = { 0x9C, 0x16 };
        public static byte[] TypeInt16 = { 0x81, 0x16 };
        public static byte[] TypeInt32 = { 0x81, 0x32 };
        public static byte[] TypeInt64 = { 0x81, 0x64 };
        public static byte[] TypeUint32 = { 0x71, 0x32 };

        public static List<byte[]> Init()
        {
            return new List<byte[]>()
            {
                Bind(NsScriptCompiler),
                Bind(NsScriptDebugger),
                Bind(NsScriptProfiler),
                Bind(NsScripts),
                Bind(NsUtility),
                Bind(NsRemote),
                Bind(NsConfig)
            };
        }

        public static byte[] Bind(string nspace)
        {
            return new byte[0].AppendUtf8(CmdBind).AppendUtf8(nspace).End();
        }

        public static byte[] Execute(string command)
        {
            return new byte[0].AppendUtf8(NsRemote).AppendInt32(unchecked((Int32)0x12345678)).AppendInt32(unchecked((Int32)0x81160008)).AppendUtf8(command).End();
        }

        public static byte[] SetVar(string section, string name, string value)
        {
            return new byte[0].AppendUtf8("Config")
                        .AppendInt32(BitConverter.ToInt32(new[] { (byte)0xCC, (byte)0x00, (byte)0xCC, (byte)0x00 }, 0))
                        .AppendUtf8("set").AppendUtf8(section).AppendUtf8(name).AppendUtf16(value)
                        .End();
        }

        public static byte[] Append(this byte[] payload, byte[] data)
        {
            return payload.Concat(data).ToArray();
        }

        public static byte[] AppendByte(this byte[] payload, byte Value)
        {
            return payload.Append(TypeByte).Append(new byte[] { Value });
        }

        public static byte[] AppendUtf8(this byte[] payload, string text)
        {
            return payload.Append(TypeStringUtf8).AppendInt16((Int16)(text.Length)).Append(Encoding.UTF8.GetBytes(text));
        }

        public static byte[] AppendUtf16(this byte[] payload, string text)
        {
            return payload.Append(TypeStringUtf16).AppendInt16((Int16)(text.Length * 2)).Append(Encoding.BigEndianUnicode.GetBytes(text));
        }

        public static byte[] AppendInt16(this byte[] payload, Int16 value)
        {
            return payload.Append(TypeInt16).Append(BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public static byte[] AppendInt32(this byte[] payload, Int32 value)
        {
            return payload.Append(TypeInt32).Append(BitConverter.GetBytes(value).Reverse().ToArray());
        }

        public static byte[] End(this byte[] payload)
        {
            return PacketHead.Concat(BitConverter.GetBytes((short)(payload.Length + 6)).Reverse().Concat(payload)).Concat(PacketTail).ToArray();
        }
    }
}
