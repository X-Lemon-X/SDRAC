using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    class NewDevice
    {
        private byte type = 0;
        private string iP = null;
        private string portCom = null;
        private string name = null;

        public byte Type { get => type; set => type = value; }
        public string IP { get => iP; set => iP = value; }
        public string PortCom { get => portCom; set => portCom = value; }
        public string Name { get => name; set => name = value; }
    }
}
