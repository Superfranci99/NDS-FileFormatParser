using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NDS_FileFormatParser.Debugging;

namespace NDS_FileFormatParser.XML
{
    class XMLreader
    {
        public string    XmlPath  { get; set; }
        public XDocument Doc      { get; set; }

        public string FileToParsePath { get; set; }

        public bool HasRootElement      { get; set; }
        public bool HasStructureElement { get; set; }
        public bool HasInfoElement      { get; set; }

        public XElement StuctElement { get; set; }
        public XElement InfoElement  { get; set; }

        public Debugger Debug { get; set; }

        #region Constructors
        public XMLreader(string xmlPath, string fileToParsePath)
            : this(fileToParsePath)
        {
            if (string.IsNullOrWhiteSpace(xmlPath))
                throw new ArgumentNullException("The path of the XML file cannot be null or white space");
            this.XmlPath = xmlPath;
            this.Doc = XDocument.Load(xmlPath);
            Read();
        }

        public XMLreader(string xmlData, string fileToParsePath, bool useless)
            : this(fileToParsePath)
        {
            if (string.IsNullOrWhiteSpace(xmlData))
                throw new ArgumentNullException("The XML file cannot be null or white space");
            this.Doc = XDocument.Parse(xmlData);
            Read();
        }

        private XMLreader(string fileToParsePath)
        {
            this.HasInfoElement      = false;
            this.HasRootElement      = false;
            this.HasStructureElement = false;

            this.FileToParsePath = fileToParsePath;
            this.Debug = new Debugger(this.FileToParsePath);
        }
        #endregion

        private void Read()
        {
            // <root>
            //   <info></info>
            //   <primitive></primitive>
            // </root>

            //get the root element and check it
            XElement root = this.Doc.Element("root");
            if (root == null)
                throw new NullReferenceException("The XML file needs a parent <root></root> element");
            else
                this.HasRootElement = true;

            //get the structure and info elements and check them
            this.StuctElement = root.Element("structure");
            this.InfoElement  = root.Element("info");
            if (this.StuctElement  == null)
                throw new NullReferenceException("The <data></data> element needs the <structure></structure> element");
            else
                this.HasStructureElement = true;
            if (this.InfoElement == null)
                throw new NullReferenceException("The <data></data> element needs the <info></info> element");
            else
                this.HasInfoElement = true;     

            //read file structure
            foreach (XElement el in this.StuctElement.Elements())
                this.Debug.AddInstruction(Xinstruction.FromXElementToXinstruction(el));

            //TODO - read the file info section
        }
    }
}
