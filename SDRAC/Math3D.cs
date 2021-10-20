using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace SDRAC
{
    class Math3D
    {
        public static bool CompareMatrix(Matrix3D a, Matrix3D b, double accuracy)
        {
            bool condition = true;
          
            //row1
            if (Math.Abs(b.M11 - a.M11) > accuracy ) condition = false;
            else if (Math.Abs(b.M12 - a.M12) > accuracy) condition = false;
            else if(Math.Abs(b.M13 - a.M13) > accuracy) condition = false;
            //row2
            else if(Math.Abs(b.M21 - a.M21) > accuracy) condition = false;
            else if(Math.Abs(b.M22 - a.M22) > accuracy) condition = false;
            else if(Math.Abs(b.M23 - a.M23) > accuracy) condition = false;
            //row3
            else if(Math.Abs(b.M31 - a.M31) > accuracy) condition = false;
            else if(Math.Abs(b.M32 - a.M32) > accuracy) condition = false;
            else if(Math.Abs(b.M33 - a.M33) > accuracy) condition = false;

            return condition;
        }
        public static double CountRadians(double sin, double cos, double accuracy)
        {
            double ar = double.NaN;

                if (sin > 0 && cos > 0) ar = Math.Acos(cos);     
                else if (sin > 0 && cos < 0)ar = Math.Acos(cos);
                else if (sin < 0 && cos < 0)ar = Math.Acos(Math.Abs(cos)) + Math.PI;
                else if (sin < 0 && cos > 0)ar = Math.Acos(Math.Abs(cos)) + Math.PI + Math.PI / 2;
                else if (CompareNum(cos, 0, accuracy) && sin > accuracy) ar = Math.PI / 2;
                else if (CompareNum(cos, 0, accuracy) && sin < accuracy)ar = Math.PI + Math.PI / 2;
                else if (CompareNum(cos, 1, accuracy) && CompareNum(sin, 0, accuracy))ar = 0;
                else if (CompareNum(cos, -1, accuracy) && CompareNum(sin, 0, accuracy))ar = Math.PI;         
            return ar;
        }

        public static bool CompareNum(double a, double b, double accuracy)
        {
            bool con = true;
            if (Math.Abs(a - b) > accuracy)con = false; 
            return con;
        }

        public static Matrix3D RotX(double a)
        {
             Matrix3D MatX = new Matrix3D(1, 0, 0, 0, 0 , Math.Cos(a), -Math.Sin(a),0, 0, Math.Sin(a), Math.Cos(a), 0, 0, 0, 0, 0);
            return MatX;
        }

        public static Matrix3D RotY(double b)
        {
            Matrix3D MatY = new Matrix3D(Math.Cos(b), 0, Math.Sin(b), 0, 0, 1, 0, 0, -Math.Sin(b), 0, Math.Cos(b), 0, 0, 0, 0, 0);
            return MatY;
        }

        public static Matrix3D RotZ(double g)
        {
            Matrix3D MatZ = new Matrix3D(Math.Cos(g), -Math.Sin(g), 0, 0, Math.Sin(g), Math.Cos(g), 0, 0, 0, 0, 1, 0, 0, 0, 0, 0);
            return MatZ;
        }

        public Matrix3D ConMatrDis(Matrix3D matrix, Vector3D vector)
        {
            matrix.OffsetX = 0;
            matrix.OffsetY = 0;
            matrix.OffsetZ = 0;
            matrix.M44 = 1;
            matrix.M14 = vector.X;
            matrix.M24 = vector.Y;
            matrix.M34 = vector.Z;

            return matrix;
        }

        public Vector3D ExtractVectorFromMatrix(Matrix3D matrix)
        {
            Vector3D vector= new Vector3D();
            vector.X = matrix.M14;
            vector.Y = matrix.M24;
            vector.Z = matrix.M34;

            return vector;
        }
    }
}
