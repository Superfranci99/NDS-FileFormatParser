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
        public Dictionary<string, object> Variables { get; set; }

        public Debugger(string fileToParsePath)
        {
            this.Parse = new Parser(fileToParsePath);
            this.Variables = new Dictionary<string, object>();
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
            
            //get the values from the Xinstruction
            DataTable dt = new DataTable();
        
            string name    = xinstruction.Args["name"];
            string type    = xinstruction.Args["type"];
            string offStr  = xinstruction.Args["offset"];
            string sizeStr = xinstruction.Args["size"];

            foreach (KeyValuePair<string, object> v in this.Variables)
                offStr = offStr.Replace(v.Key, v.Value.ToString());

            foreach (KeyValuePair<string, object> v in this.Variables)
                sizeStr = sizeStr.Replace(v.Key, v.Value.ToString());

            int offset = int.Parse(dt.Compute(offStr, "").ToString());
            int size   = int.Parse(dt.Compute(sizeStr, "").ToString());
           
            //get the value from the file
            object value = this.Parse.Read(type, offset, size);

            //TODO - check if a variable with the same name already exists
            //add the current variable to the variable list
            this.Variables.Add(name, value);

            //output the value with all infos
            Console.WriteLine("{0}, {1}, {2}, {3} --> {4}", name, type, offset, size, value);
        }
    }
}
