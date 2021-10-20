using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SDRAC
{
    public class Cordinants
    {
        private double x, y, z, rZ, rY, rX, xL, yL, zL, xB, yB, zB;
        private string dataS,dataSF,dataVelocity;
        private int codeData = 0;
        private int[] dataToSend = new int[6];
        private int[] angleLast = new int[6];
        private int[] angleLastFile = new int[6];
        private int[] velocity = new int[8];
        private double[] anglesRaw = new double[6];
        private double[] anglesDeg = new double[6];
        private double[] anglesRad = new double[6];
        public Classes.SimpleLan.Command cm1, cm2;
        Matrix3D rotXYZ = new Matrix3D(0, 0, 1, 0, 0, -1, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0);

        public void SetDataToSend(int code,int[] _dataToSend)
        {
            int i = 0;
            codeData = code;
            dataToSend = new int[_dataToSend.Length];
            foreach (int item in _dataToSend)
            {
                dataToSend[i] = item;
                i++;
            }
        
        }


        #region GETSET
        public double X { get => x; set => x = value; }
        public double Y { get => y; set => y = value; }
        public double Z { get => z; set => z = value; }
        public double RZ { get => rZ; set => rZ = value; }
        public double RY { get => rY; set => rY = value; }
        public double RX { get => rX; set => rX = value; }
        public double XB { get => xB; set => xB = value; }
        public double YB { get => yB; set => yB = value; }
        public double ZB { get => zB; set => zB = value; }
        public double XL { get => xL; set => xL = value; }
        public double YL { get => yL; set => yL = value; }
        public double ZL { get => zL; set => zL = value; }
        public string DataS { get => dataS; set => dataS = value; }
        public int[] AngleLast { get => angleLast; set => angleLast = value; }
        public string DataSF { get => dataSF; set => dataSF = value; }
        public int[] AngleLastFile { get => angleLastFile; set => angleLastFile = value; }
        public int[] Velocity { get => velocity; set => velocity = value; }
        public double[] AnglesRaw { get => anglesRaw; set => anglesRaw = value; }
        public Matrix3D RotXYZ { get => rotXYZ; set => rotXYZ = value; }
        public string DataVelocity { get => dataVelocity; set => dataVelocity = value; }
        public double[] AnglesDeg { get => anglesDeg; set => anglesDeg = value; }
        public double[] AnglesRad { get => anglesRad; set => anglesRad = value; }
        public int[] DataToSend { get => dataToSend; set => dataToSend = value; }
        public int CodeData { get => codeData; set => codeData = value; }
        #endregion
    }
}
