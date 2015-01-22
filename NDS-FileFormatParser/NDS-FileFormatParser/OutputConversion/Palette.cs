using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NDS_FileFormatParser.OutputConversion
{
    class Palette : DefaultType
    {
        public override string Type { get { return "Palette"; } }

        public Palette()
        {
            this.DependenciesNeeded = new List<string>(new string[] { "PaletteSize", "Color"});
        }

        public override void Import()
        {
            throw new NotImplementedException();
        }

        public override void Export()
        {
            throw new NotImplementedException();
        }

    }
}
