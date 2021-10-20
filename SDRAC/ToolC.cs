using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    class ToolC
    {
        private double x, y, z;
        private string name;
        private byte id;      

        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public string Name { get => name; set => name = value; }
        public byte Id { get => id; set => id = value; }
    }
}
