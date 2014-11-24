using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NDS_FileFormatParser.XML
{
    public class XMLreader
    {
        public string    FilePath { get; set; }
        public XDocument Doc      { get; set; }

        public bool HasRootElement      { get; set; }
        public bool HasStructureElement { get; set; }
        public bool HasInfoElement      { get; set; }

        #region Constructors
        public XMLreader(string filePath) : this()
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("The path of the XML file cannot be null or white space");
            this.FilePath = filePath;
            this.Doc = XDocument.Load(filePath);
            Read();
        }

        public XMLreader(string filedata, bool useless) : this()
        {
            if (string.IsNullOrWhiteSpace(filedata))
                throw new ArgumentNullException("The XML file cannot be null or white space");
            this.Doc = XDocument.Parse(filedata);       
            Read();
        }

        private XMLreader()
        {
            this.HasInfoElement = false;
            this.HasRootElement = false;
            this.HasStructureElement = false;
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
            XElement fileStruct = root.Element("structure");
            XElement fileInfo   = root.Element("info");
            if (fileStruct == null)
                throw new NullReferenceException("The <data></data> element needs the <structure></structure> element");
            else
                this.HasStructureElement = true;
            if (fileInfo == null)
                throw new NullReferenceException("The <data></data> element needs the <info></info> element");
            else
                this.HasInfoElement = true;

            //TODO read the file structure section
            //TODO read the file info section
        }
    }
}
