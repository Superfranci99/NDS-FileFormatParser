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

        public void AddDipendencies(KeyValuePair<string, object> dependency)
        {
            foreach (string dep in this.DependenciesNeeded)
                if (dep == dependency.Key)
                    this.DependenciesFound.Add(dependency.Key, dependency.Value);
        }

        public abstract void Import();
        public abstract void Export();
    }
}
