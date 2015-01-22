using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NDS_FileFormatParser.OutputConversion;

namespace NDS_FileFormatParser.Parsing
{
    class Parser
    {

        FileStream   Fs { get; set; }
        BinaryReader Br { get; set; }

        public Parser(string fileToParsePath)
        {
            this.Fs = new FileStream(fileToParsePath, FileMode.Open);
            this.Br = new BinaryReader(Fs);
        }

        public object Read(string type, int offset, int length)
        {
            this.Br.BaseStream.Seek(offset, SeekOrigin.Begin);

            switch (type)
            {
                case "string":
                    return Encoding.GetEncoding("shift_jis").GetString(this.Br.ReadBytes(length));
                case "int":
                    switch (length)
                    {
                        case 1: return this.Br.ReadByte();
                        case 2: return this.Br.ReadUInt16();
                        case 4: return this.Br.ReadUInt32();
                        default: return this.Br.ReadBytes(length);
                    }
                default:
                    throw new NotImplementedException("Type " + type + " not supported yet");
            }
        }
    }
}
