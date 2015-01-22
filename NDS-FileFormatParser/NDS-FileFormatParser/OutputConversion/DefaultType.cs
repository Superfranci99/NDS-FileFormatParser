using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDS_FileFormatParser.OutputConversion
{
    abstract class DefaultType
    {
        public abstract string Type { get; }
        public List<string> DependenciesNeeded { get; set; }
        public Dictionary<string, object> DependenciesFound { get; set; }

        public DefaultType()
        {
            this.DependenciesFound = new Dictionary<string, object>();
        }

        public void AddDipendencies(string name, object value)
        {
            foreach (string dep in this.DependenciesNeeded)
            {
                int pos = name.IndexOf('_');
                string s=name;
                if (pos != -1)
                    s = name.Remove(pos);
                if (s == dep)
                    this.DependenciesFound.Add(name, value);
            }
                
        }

        public abstract void Import();
        public abstract void Export();
    }
}
