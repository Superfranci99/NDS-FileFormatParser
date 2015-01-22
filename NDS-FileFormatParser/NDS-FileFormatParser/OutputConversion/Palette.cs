using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

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
            //read the palette with the BGR555 encoding
            this.PaletteSize = int.Parse(this.DependenciesFound[this.DependenciesNeeded[0].ToString()].ToString()); //get the palette size

            //get the raw color data
            this.ColorData = new ushort[this.PaletteSize / 2];
            int palCounter = 0;
            foreach (KeyValuePair<string, object> dep in this.DependenciesFound)
            {
                int pos = dep.Key.IndexOf('_');
                string s = dep.Key;
                if (pos != -1)
                    s = dep.Key.Remove(pos);
                if (s == this.DependenciesNeeded[1].ToString())
                    this.ColorData[palCounter++] = ushort.Parse(dep.Value.ToString());
            }

            //convert the raw data to color with BGR555
            this.Colors = new Color[this.PaletteSize / 2];
            for (int i = 0; i < this.PaletteSize / 2; i++)
            {
                int r = (this.ColorData[i] & 0x001F) * 0x08;
                int g = ((this.ColorData[i] & 0x03E0) >> 5) * 0x08;
                int b = ((this.ColorData[i] & 0x7C00) >> 10) * 0x08;
                this.Colors[i] = Color.FromArgb(r, g, b);
            }
        }

        public override void Export()
        {
            throw new NotImplementedException();
        }


        #region Properties

        public int      PaletteSize { get; set; }
        public ushort[] ColorData   { get; set; }
        public Color[]  Colors      { get; set; }

        #endregion
    }
}
