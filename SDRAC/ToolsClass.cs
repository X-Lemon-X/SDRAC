using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    public class ToolsClass
    {
        private double lenX, lenY, lenZ;
        private byte id;
        private List<ToolC> tool = new List<ToolC>();
        private string selectedTool;   

        public double LenX { get => lenX; set => lenX = value; }
        public double LenY { get => lenY; set => lenY = value; }
        public double LenZ { get => lenZ; set => lenZ = value; }
        public byte Id { get => id; set => id = value; }
        public string SelectedTool { get => selectedTool; set => selectedTool = value; }
        internal List<ToolC> Tool { get => tool; set => tool = value; }
    }
}
