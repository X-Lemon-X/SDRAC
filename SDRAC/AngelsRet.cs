using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    public class AnglesRet
    {

        private double[] anglesRaw = new double[6];
        private double[] anglesRad = new double[6];
        private double[] anglesDeg = new double[6];
        private bool deg=false, rad=false;

        public double[] AnglesRad { get => anglesRad; set => anglesRad = value; }
        public double[] AnglesDeg { get => anglesDeg; set => anglesDeg = value; }
        public bool Deg { get => deg; set => deg = value; }
        public bool Rad { get => rad; set => rad = value; }
        public double[] AnglesRaw { get => anglesRaw; set => anglesRaw = value; }
    }
}
