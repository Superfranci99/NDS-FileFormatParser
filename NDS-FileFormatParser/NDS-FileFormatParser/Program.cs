using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDS_FileFormatParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("~~ NDS-FileFormatParser ~~");
            Console.WriteLine("by Superfranci99 -- V:{0}",
                System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);

            RunCommand(args);
        }

        static void RunCommand(string[] args)
        {
            /*
             * At the moment I don't really know which commands
             * there will be and which name I will give to them
             * For now just use this method for parse a file
             * where args[0] = path of the file to parse
             *       args[1] = path of the XML file
            */
            args = new string[2];
            args[0] = @"Files\palette.nclr";
            args[1] = @"Files\test.xml";

            Controller controller = new Controller(args[0], args[1]);
            Console.Read();
        }
    }
}
