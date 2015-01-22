using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDS_FileFormatParser.XML;
using NDS_FileFormatParser.Parsing;
using NDS_FileFormatParser.OutputConversion;
using System.Data;

namespace NDS_FileFormatParser.Debugging
{
    class Debugger
    {
        public string FileToParsePath { get; set; }
        public Parser Parse { get; set; }
        public Dictionary<string, object> Variables { get; set; }
        public Dictionary<string, int> Counters { get; set; }
        public DefaultType Output { get; set; }

        int forCounter;

        public Debugger(string fileToParsePath, DefaultType t)
        {
            this.Parse     = new Parser(fileToParsePath);
            this.Variables = new Dictionary<string, object>();
            this.Counters  = new Dictionary<string, int>();
            this.Output = t;
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
                    this.Counters.Remove(xinstruction.Args["counter"]); //remove the last counter
                    break;
                default:
                    throw new NotImplementedException("Instruction not supported");
            }
        }

        public void ExecuteFor(Xinstruction xinstruction)
        {
            //<for counter="i" start="0" to="PaletteSize/2-1" step="2"></for>

            forCounter = 0;
            DataTable dt = new DataTable();

            string counterName = xinstruction.Args["counter"];
            string startStr    = xinstruction.Args["start"];
            string toStr       = xinstruction.Args["to"];
            string stepStr     = xinstruction.Args["step"];

            //check start
            foreach (KeyValuePair<string, object> v in this.Variables)
                startStr = startStr.Replace(v.Key, v.Value.ToString());
            foreach (KeyValuePair<string, int> v in this.Counters)
                startStr = startStr.Replace(v.Key, v.Value.ToString());

            //check to
            foreach (KeyValuePair<string, object> v in this.Variables)
                toStr = toStr.Replace(v.Key, v.Value.ToString());
            foreach (KeyValuePair<string, int> v in this.Counters)
                toStr = toStr.Replace(v.Key, v.Value.ToString());

            //check step
            foreach (KeyValuePair<string, object> v in this.Variables)
                stepStr = stepStr.Replace(v.Key, v.Value.ToString());
            foreach (KeyValuePair<string, int> v in this.Counters)
                stepStr = stepStr.Replace(v.Key, v.Value.ToString());

            int start = int.Parse(dt.Compute(startStr, "").ToString());
            int to    = int.Parse(dt.Compute(toStr, "").ToString());
            int step  = int.Parse(dt.Compute(stepStr, "").ToString());

            this.Counters.Add(counterName, start);
            for (int i = start; i <= to; i += step)
            {
                this.Counters[counterName] = i; //update the counter                
                //excecute each instruction inside the for
                for (int j = 0; j < xinstruction.Xinstructions.Count; j++)
                {
                    Xinstruction curr = xinstruction.Xinstructions[j];
                    //delete the cycle counter from the primitive last cycle name
                    string n = curr.Args["name"];
                    for (int a = 0; a < n.Length; a++)
                        if (n[a] == '_')
                        {
                            n = n.Remove(a);
                            break;
                        }
                    curr.Args["name"] = n + "_" + forCounter.ToString();

                    AddInstruction(curr);
                    forCounter++;
                }
            }
                
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
            foreach (KeyValuePair<string, int> v in this.Counters)
                offStr = offStr.Replace(v.Key, v.Value.ToString());

            foreach (KeyValuePair<string, object> v in this.Variables)
                sizeStr = sizeStr.Replace(v.Key, v.Value.ToString());
            foreach (KeyValuePair<string, int> v in this.Counters)
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
            this.Output.AddDipendencies(name, value);
        }
    }
}
