using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDS_FileFormatParser.XML;
using NDS_FileFormatParser.Parsing;
using System.Data;

namespace NDS_FileFormatParser.Debugging
{
    class Debugger
    {
        public string FileToParsePath { get; set; }
        public Parser Parse { get; set; }

        public Debugger(string fileToParsePath)
        {
            this.Parse = new Parser(fileToParsePath);
        }

        public void AddInstruction(Xinstruction xinstruction)
        {
            switch (xinstruction.CommandName)
            {
                case "primitive":
                    ExecutePrimitive(xinstruction);
                    break;
                case "for":
                    ExecuteFor(xinstruction);
                    break;
                default:
                    throw new NotImplementedException("Instruction not supported");
            }
        }

        private void ExecuteFor(Xinstruction xinstruction)
        {
            throw new NotImplementedException();
        }

        private void ExecutePrimitive(Xinstruction xinstruction)
        {
            // <primitive name="" type="" offset="" size="" />
            DataTable dt = new DataTable();

            string name   = xinstruction.Args["name"];
            string type   = xinstruction.Args["type"];
            int    offset = int.Parse(dt.Compute(xinstruction.Args["offset"], "").ToString());
            int    size   = int.Parse(dt.Compute(xinstruction.Args["size"], "").ToString());
           
            object value = this.Parse.Read(type, offset, size);
            Console.WriteLine("{0}, {1}, {2}, {3} --> {4}", name, type, offset, size, value);
        }
    }
}
