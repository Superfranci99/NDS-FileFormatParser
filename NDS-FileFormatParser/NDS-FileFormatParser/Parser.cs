﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NDS_FileFormatParser
{
    class Parser
    {
        public string Path_fileToParse { get; private set; }
        public string Path_xmlInfo     { get; private set; }


        public Parser(string path_fileToParse, string path_xmlInfo)
        {
            if (string.IsNullOrWhiteSpace(path_fileToParse) || string.IsNullOrWhiteSpace(path_xmlInfo))
                throw new ArgumentNullException("File paths are null");

            this.Path_fileToParse = path_fileToParse;
            this.Path_xmlInfo     = path_xmlInfo;

            XML.XMLreader xml = new XML.XMLreader(this.Path_xmlInfo);

            //TODO
            //read the values from the file
        }

        
    }
}
