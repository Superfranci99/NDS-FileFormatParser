using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NDS_FileFormatParser.XML
{
    class Xinstruction
    {
        /// <summary>
        /// Object with some info about an instruction, the debugger will interpret it
        /// </summary>
        /// <param name="commandName">Instruction identifier</param>
        /// <param name="args">Dictionary for args args[identifier][value]</param>
        /// <param name="xinstructions">Optional - Instructions inside the scope</param>
        public Xinstruction(string commandName, Dictionary<string,string> args, List<Xinstruction> xinstructions)
        {
            //command name
            this.CommandName = commandName;
            if (IsValidCommand(this.CommandName))
                this.IsValid = true;
            else
            {
                this.IsValid = false;
                throw new NotImplementedException("This command doesn't exist");
            }

            //container
            this.Xinstructions = xinstructions;
            if (this.Xinstructions == null)
                this.IsContainer = false;
            else
                this.IsContainer = true;

            //args
            this.Args = args;
            if (this.Args == null)
                this.HasNoParams = true;
            else
                this.HasNoParams = false;

            //TODO - check if the params of an instruction are correct (maybe later)
        }

        #region Properties

        public string CommandName { get; set; }
        public Dictionary<string, string> Args { get; set; }
        public List<Xinstruction> Xinstructions { get; set; }

        public bool IsValid { get; set; }
        public bool IsContainer { get; set; }

        public bool HasCorrectParams { get; set; }
        public bool HasNoParams { get; set; }

        #endregion


        #region StaticMethods

        /// <summary>
        /// Check if the instruction command is supported
        /// </summary>
        /// <param name="xInstr">Instruction to check</param>
        /// <returns></returns>
        public static bool IsValidCommand(Xinstruction xInstr)
        {
            return IsValidCommand(xInstr.CommandName);
        }

        /// <summary>
        /// Check if the instruction command is supported
        /// </summary>
        /// <param name="command">Identifier of the instruction</param>
        /// <returns></returns>
        public static bool IsValidCommand(string command)
        {
            bool isValid;
            switch (command)
            {
                case "primitive":
                    isValid = true;
                    break;
                case "for":
                case "if":
                    throw new NotImplementedException("Xinstruction command not supported yet");
                default:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        /// <summary>
        /// Check if an instruction contains other instructions
        /// </summary>
        /// <param name="command">Identifier of the instruction</param>
        /// <returns></returns>
        public static bool IsContainerCommand(string command)
        {
            bool isContainerCommand;
            switch (command)
            {
                case "primitive":
                    isContainerCommand = false;
                    break;
                case "for":
                case "if":
                    isContainerCommand = true;
                    break;
                default:
                    throw new NotImplementedException(command + " - not supported yet");
            }
            return isContainerCommand;
        }

        /// <summary>
        /// Check if an instruction contains other instructions
        /// </summary>
        /// <param name="xinstruction">Instruction to check</param>
        /// <returns></returns>
        public static bool IsContainerCommand(Xinstruction xinstruction)
        {
            return IsContainerCommand(xinstruction.CommandName);
        }

        /// <summary>
        /// Convert a XElement to a Xinstruction
        /// </summary>
        /// <param name="el">XElement to convert</param>
        /// <returns></returns>
        public static Xinstruction FromXElementToXinstruction(XElement el)
        {
            List<Xinstruction> contained = new List<Xinstruction>();
            if (Xinstruction.IsContainerCommand(el.Name.ToString()))
                foreach (XElement conEl in el.Elements())
                    contained.Add(FromXElementToXinstruction(conEl));
            else
                contained = null;

            Dictionary<string, string> args = new Dictionary<string, string>();
            foreach (XAttribute attr in el.Attributes())
                args.Add(attr.Name.ToString(), attr.Value);

            return new Xinstruction(el.Name.ToString(), args, contained);
        }

        #endregion

    }
}
