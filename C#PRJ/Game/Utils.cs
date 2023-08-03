﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Witcher3_Multiplayer.Game;

namespace Witcher3_Multiplayer.Game
{
    public class Response
    {
        public class Data
        {
            public byte[] Head = Convertors.PacketHead;
            public UInt16 Length;
            public List<Param> Params;

            public Data(byte[] recievedBytes)
            {
                using (var br = new BinaryReader(new MemoryStream(recievedBytes)))
                {
                    Head = br.ReadBytes(2);
                    Length = BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0);
                    Params = new List<Param>();
                    for (; ; )
                    {
                        if (!(br.BaseStream.Length - br.BaseStream.Position > 4)) break;
                        var type = (ParamType)(BitConverter.ToUInt16(br.ReadBytes(2).Reverse().ToArray(), 0));
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
                            case ParamType.Byte:
                                {
                                    var param = new Byte_
                                    {
                                        Value = br.ReadSByte(),
                                        Type = ParamType.Byte
                                    };
                                    Params.Add(param);
                                    break;
                                }
                            case ParamType.Uint32:
                                {
                                    var param = new Uint_32
                                    {
                                        Value = BitConverter.ToUInt32(br.ReadBytes(4).Reverse().ToArray(), 0),
                                        Type = ParamType.Uint32
                                    };
                                    Params.Add(param);
                                    break;
                                }
                            case ParamType.Int32:
                                {
                                    var param = new Int_32
                                    {
                                        Value = BitConverter.ToInt32(br.ReadBytes(4).Reverse().ToArray(), 0),
                                        Type = ParamType.Int32
                                    };
                                    Params.Add(param);
                                    break;
                                }
                            case ParamType.Int64:
                                {
                                    var param = new Int_64
                                    {
                                        Value = BitConverter.ToInt64(br.ReadBytes(4).Reverse().ToArray(), 0),
                                        Type = ParamType.Int64
                                    };
                                    Params.Add(param);
                                    break;
                                }
                            default:
                                break;
                        }
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

        public class Byte_ : Param
        {
            public sbyte Value;

            public override string ToString()
            {
                return "0x" + Value.ToString("X");
            }
        }

        public class Uint_32 : Param
        {
            public UInt32 Value;

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class Int_32 : Param
        {
            public Int32 Value;

            public override string ToString()
            {
                return Value.ToString();
            }
        }

        public class Int_64 : Param
        {
            public Int64 Value;

            public override string ToString()
            {
                return Value.ToString();
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
