using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Witcher3_Multiplayer.Game
{
    public class Response
    {
        public class Data
        {
            public byte[] Head = Convertors.PacketHead;
            public UInt16 Length;
            public List<Param> Params = new List<Param>();

            public Data(byte[] recievedBytes)
            {
                BinaryReader br = new BinaryReader(new MemoryStream(recievedBytes));

                Head = br.ReadBytes(2);
                Length = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
                for (; ; )
                {
                    if (!(br.BaseStream.Length - br.BaseStream.Position > 4)) break;
                    ParamType type = (ParamType)BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
                    if (type == ParamType.PacketEnd) break;
                    switch (type)
                    {
                        case ParamType.StringAnsi:
                            {
                                var param = new StringAnsi
                                {
                                    Unknown = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0),
                                    length = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0),
                                    Type = ParamType.StringAnsi
                                };
                                param.Value = Encoding.ASCII.GetString(br.ReadBytes(param.length));
                                Params.Add(param);
                                break;
                            }
                        case ParamType.StringUtf16:
                            {
                                var param = new StringUtf16
                                {
                                    Unknown = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0),
                                    length = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0),
                                    Type = ParamType.StringUtf16
                                };
                                param.Value = Encoding.BigEndianUnicode.GetString(br.ReadBytes(param.length * 2));
                                Params.Add(param);
                                break;
                            }
                        default:
                            break;

                    }
                }
            }
        }

        public abstract class Param
        {
            public ParamType Type;
        }

        public class StringAnsi : Param
        {
            public UInt16 Unknown;
            public UInt16 length;
            public string Value;

            public override string ToString()
            {
                return Value;
            }
        }

        public class StringUtf16 : Param
        {
            public UInt16 Unknown;
            public UInt16 length;
            public string Value;

            public override string ToString()
            {
                return Value;
            }
        }
        public enum ParamType
        {
            StringAnsi = 0xAC08,
            StringUtf16 = 0x9C16,
            Byte = 0x8108,
            PacketEnd = 0xBEEF,
            Uint32 = 0x7132,
            Int32 = 0x8132,
            Int64 = 0x8164
        }
    }
}
