using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml;
using FontAwesome.Sharp;

namespace SDRAC
{
    public partial class MainMenuForm : Form
    {
        #region Veriables and Classes
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChileForm;
        int anim = 510;
        public bool serchForRobots = true, readingLanWiFi = false;
        int errorListindex = 0;

        public Thread readSP = null, prepare = null, queueSend = null,readLanWifiThread=null;
        public TcpClient tcpClient = null;
        public static Socket socketCon = null;
        public NetworkStream stream = null;
        public static DataClass dataClass = new DataClass();
        public static ToolsClass Tools = new ToolsClass();
        public static SerialPort serialPort = new SerialPort();
        public static MovementWayPath movementWayPath = new MovementWayPath();
        Classes.SimpleLan simpleLan = null;
        public static List<Classes.SimpleLan.ErrorClass> errorListNumerous = null;


        public static List<ErrorBytes> _errorList = new List<ErrorBytes>();
        #endregion

        #region Start of the form
        public MainMenuForm()
        {
            InitializeComponent();


            // Auto Start
            // RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            // rkApp.SetValue("BackUpManager", Assembly.GetExecutingAssembly().Location.ToString());

            StartOFTheProgram();
        }

        private void StartOFTheProgram()
        {
            leftBorderBtn = panel1;
            leftBorderBtn.Size = new Size(7, 62);
            Tools = ReadXMLFile();
            SetupSerialPort();

            if (dataClass.Ip != null && dataClass.Port != null)
            { dataClass.UploadAble = true;
                iconButtonConnect.BackColor = Color.FromArgb(35, 35, 35);
                iconButtonConnect.ForeColor = RGBcolors.color7;
                iconButtonConnect.IconColor = RGBcolors.color7;
            }
        }   

        #endregion

        // 95% is working ?
        #region Math

        private Cordinants MathAllinOne(Cordinants cord, bool lastPointCorrection)
        {
            #region Seting Veriables
        
            string dataS;
            int movementType = MainMenuForm.dataClass.MovementType;
            int frameType = MainMenuForm.dataClass.FrameType;

            #endregion

            #region Redefine positions to Movement type and add rotations

            cord = ApplyRotationMatrix(cord);

            switch (frameType)
            {
                case 0: cord = ApplyBaseRot(cord , lastPointCorrection);
                    break;
                case 1: cord = ApplyToolRot(cord, lastPointCorrection);
                    break;
                case 2: cord = ApplyToolRot(cord, lastPointCorrection);
                    break;
                case 3:  //plane 
                    break;
                default: //off
                    break;
            }

            switch (movementType)
            {
                case 0: //vector
                    double JumpVector = MainMenuForm.dataClass.JumpVector;
                    int velocity = MainMenuForm.dataClass.Speed;
                    cord = VectorMovementUpgraded(cord, JumpVector);
                    cord = AStandardBasic(cord);              
                    cord = VectorVelocity(cord, velocity, true);
                    break;
                case 1: //v.r ?
                    cord = AStandardBasic(cord);
                    break;
                case 2: //joint
                    cord = JointMovement(cord);
                    cord = AStandardBasic(cord);
                    cord = SetVeocityJoint(cord);
                    break;
                case 3: //off
                    cord = AStandardBasic(cord);
                    break;

                default: break;
            }

            #endregion     

            #region Prepare data to Send

            int[] dat = new int[6];
            for (int i = 0; i < 6; i++)
            {
                double anglesData = cord.AnglesRad[i];
                if (!double.IsNaN(anglesData))
                {
                    anglesData = anglesData / (2 * Math.PI) * MainMenuForm.dataClass.GearReduction[i];

                    int angle = Convert.ToInt32(anglesData);
                    cord.AngleLast[i] = angle;
                    dat[i] = angle;
                }
                else
                {
                    dat[i] = 0;
                }  
            }

            Classes.SimpleLan.Command cm = new Classes.SimpleLan.Command();
            cm.CreateNew(34,false,2,dat);
            cord.cm1 = cm;

            //dataS = ConvertToMessage(34, dat);
            //cord.DataS = dataS;

            #endregion

            return cord;
        }

        private Cordinants ApplyRotationMatrix(Cordinants cord)
        {
   
            double xrot = MainMenuForm.dataClass.RxL / 360.0 * 2.0 * Math.PI;
            double yrot = MainMenuForm.dataClass.RyL / 360.0 * 2.0 * Math.PI;
            double zrot = MainMenuForm.dataClass.RzL / 360.0 * 2.0 * Math.PI;

            Matrix3D R0_6 = Matrix3D.Multiply(Math3D.RotX(xrot),Math3D.RotZ(zrot));

            R0_6 = Matrix3D.Multiply(Math3D.RotY(yrot), R0_6 );

            cord.RotXYZ = Matrix3D.Multiply(R0_6, cord.RotXYZ);

            return cord;
        }

        private Cordinants ApplyToolRot(Cordinants cord, bool last)
        {
            Matrix3D vec = new Matrix3D(1, 0, 0, MainMenuForm.Tools.LenX, 0, 1, 0, MainMenuForm.Tools.LenY, 0, 0, 1, MainMenuForm.Tools.LenZ, 0, 0, 0, 1);

            vec = Matrix3D.Multiply(cord.RotXYZ, vec);

            cord.X -= vec.M14;
            cord.Y -= vec.M24;
            cord.Z -= vec.M34;

            if (last)
            { 
                cord.XL -= vec.M14;
                cord.YL -= vec.M24;
                cord.ZL -= vec.M34;
            }

            vec = new Matrix3D(1, 0, 0, MainMenuForm.dataClass.XL, 0, 1, 0, MainMenuForm.dataClass.Yl, 0, 0, 1, MainMenuForm.dataClass.ZL, 0, 0, 0, 1);
            vec = Matrix3D.Multiply(cord.RotXYZ, vec);

            cord.X += vec.M14;
            cord.Y += vec.M24;
            cord.Z += vec.M34;

            if (last)
            {
                cord.XL += vec.M14;
                cord.YL += vec.M24;
                cord.ZL += vec.M34;
            }
            return cord;
        }

        private Cordinants ApplyBaseRot(Cordinants cord, bool last)
        {
            Matrix3D vec = new Matrix3D(0, 0, 0, MainMenuForm.Tools.LenX, 0, 0, 0, MainMenuForm.Tools.LenY, 0, 0, 0, MainMenuForm.Tools.LenZ, 0, 0, 0, 1);

            vec = Matrix3D.Multiply(cord.RotXYZ, vec);

            cord.X -= vec.M14;
            cord.Y -= vec.M24;
            cord.Z -= vec.M34;

            cord.X += MainMenuForm.dataClass.XL;
            cord.Y += MainMenuForm.dataClass.Yl;
            cord.Z += MainMenuForm.dataClass.ZL;

            if (last)
            {
                cord.XL -= vec.M14;
                cord.YL -= vec.M24;
                cord.ZL -= vec.M34;
                cord.XL += MainMenuForm.dataClass.XL;
                cord.YL += MainMenuForm.dataClass.Yl;
                cord.ZL += MainMenuForm.dataClass.ZL;
            }


            return cord;
        }

        public Cordinants AStandardBasic(Cordinants cor)
        {
            #region Veriables        
            double X = cor.XL, Y = cor.YL, Z = cor.ZL, RX = cor.RX, RY = cor.RY, RZ = cor.RZ;

            double a1 = MainMenuForm.dataClass.A1;
            double a0 = MainMenuForm.dataClass.A0;
            double accuracy = MainMenuForm.dataClass.Accuracy;

            double alfa1 = Math.PI, alfa1Row = 0;

            double[] alfa3Row = new double[4];
            double[] alfa2Row = new double[4];
            double[] alfa3 = new double[4];
            double[] alfa2 = new double[4];

            bool Za0 = false;
            double l, L, A, B1, C, del = 0, Y1 = double.NaN, Y2 = double.NaN, k;
            double[] xX = new double[4];
            double[] yY = new double[2];
            #endregion

            bool repeat = true;
            for (int i = 0; i < 2; i++)
            {

                #region Math first three angles
                if (Y != 0)
                {
                    alfa1 = Math.Acos(Y / (Math.Sqrt((Y * Y) + (X * X))));
                    if (alfa1 > 90)
                    {
                        alfa1Row = -1 * (alfa1 - Math.PI);
                    }
                    else
                    {
                        alfa1Row = Math.PI - alfa1;
                    }

                }

                if (repeat)
                {
                    repeat = false;
                    Matrix3D resu = Math3D.RotZ(alfa1);
                    Matrix3D vec = new Matrix3D(0, 0, 0, 0, 0, 0, 0, 30, 0, 0, 0, 0, 0, 0, 0, 0);
                    resu = Matrix3D.Multiply(resu, vec);
                    double rX = resu.M14;
                    double rY = resu.M24;
                    X = X - rX;
                    Y = Y -rY;
                }

                l = Math.Sqrt((X * X) + (Y * Y));
            }

            l = (X * X) + (Y * Y);

            L = ( l + (Z * Z) - (a0 * a0)) / 2;
            l = Math.Sqrt(l);


            if (l == 0)
            {
                Y1 = (Z + a0) / 2;
                Y2 = Y1;
            }
            else
            {
                if (Z == a0)
                {
                    double x = l / 2;
                    double b = (-2 * a0);
                    double c = ((a0 * a0) + ((l * l) / 4) - (250 * 250));
                    double delta = (b * b) - (4 * c);
                    delta = Math.Sqrt(delta);
                    Y1 = ((-1 * b) - delta) / 2;
                    Y2 = ((-1 * b) + delta) / 2;
                    Za0 = true;
                }
                else
                {
                    double y;
                    y = L / (Z - a0);

                    A = ((Z - a0) * (Z - a0)) + (l * l);
                    B1 = (-2 * a0 * l * l) - (2 * L * (Z - a0));
                    C = ((-1) * l * l * 250 * 250) + (L * L) + (a0 * a0 * l * l);
                    del = Math.Sqrt((B1 * B1) - (4 * A * C));

                    if (!double.IsNaN(del))
                    {
                        Y1 = (-B1 - del) / (2 * A);
                        Y2 = (-B1 + del) / (2 * A);
                    }
                }

            }


            if (!double.IsNaN(del))
            {

                yY[0] = Y1;
                yY[1] = Y2;

                xX[0] = Math.Sqrt((250 * 250) - ((Y1 - a0) * (Y1 - a0)));
                xX[1] = (-1) * Math.Sqrt((250 * 250) - ((Y1 - a0) * (Y1 - a0)));
                xX[2] = Math.Sqrt((250 * 250) - ((Y2 - a0) * (Y2 - a0)));
                xX[3] = (-1) * Math.Sqrt((250 * 250) - ((Y2 - a0) * (Y2 - a0)));

                int j = 0;
                for (int i = 0; i < 4;)
                {
                    double res = ((xX[i] - l) * (xX[i] - l)) + ((yY[j] - Z) * (yY[j] - Z));
                    if (res > (250 + accuracy) * (250 + accuracy) || res < (250 - accuracy) * (250 - accuracy))
                    {
                        xX[i] = double.NaN;
                        alfa3[i] = double.NaN;
                        alfa2[i] = double.NaN;
                        alfa2Row[i] = double.NaN;
                        alfa3Row[i] = double.NaN;
                    }
                    else
                    {
                        // J3
                        alfa3[i] = Math.Acos((((l - xX[i]) * (xX[i])) + ((Z - yY[j]) * (yY[j] - a0))) / (250 * 250));
                        alfa3Row[i] = alfa3[i];

                        bool rightleft = false;

                        k = ((yY[j] - a0) / xX[i]);

                        if (xX[i] > 0) //  I quarter & IV quarter
                        {
                            if (Z < ((k * l) + a0))    // jeśli po prawej
                            {
                                alfa3[i] = Math.PI + alfa3[i];
                                rightleft = true;
                            }
                            else  //jeśli po lewej stronie
                            {
                                alfa3[i] = Math.PI - alfa3[i];
                            }
                        }
                        else if (xX[i] < 0) //   II quarter  &  III quarter
                        {
                            if (Z > ((k * l) + a0))    // jeśli po prawej
                            {
                                alfa3[i] = Math.PI + alfa3[i];
                                rightleft = true;
                            }
                            else  //jeśli po lewej stronie
                            {
                                alfa3[i] = Math.PI - alfa3[i];
                            }
                        }
                        else if (xX[i] == 0)
                        {
                            alfa3[i] = Math.PI + alfa3[i];
                            rightleft = true;
                        }


                        if (rightleft) // raw angle for futher equations 
                        {
                            if (alfa3Row[i] < (Math.PI / 2))
                            { alfa3Row[i] = (Math.PI / 2) - alfa3Row[i]; }
                            else if (alfa3Row[i] > (Math.PI / 2))
                            { alfa3Row[i] = -1 * (alfa3Row[i] - (Math.PI / 2)); }
                            else
                            { alfa3Row[i] = 0; }
                        }
                        else
                        {
                            alfa3Row[i] = (Math.PI / 2) + alfa3Row[i];
                        }



                        //J2
                        alfa2[i] = Math.Acos(((yY[j] - a0)) / (a1));
                        alfa2Row[i] = alfa2[i];

                        if (xX[i] > 0)   //Jeśli jest po Prawej
                        {
                            alfa2[i] = Math.PI + alfa2[i];
                            alfa2Row[i] = -1 * alfa2Row[i];
                        }
                        else if (xX[i] == 0)
                        {
                            alfa2[i] = Math.PI;
                            alfa2Row[i] = 0;
                        }
                        else   //Jeśli jest po Lewej
                        {
                            alfa2[i] = Math.PI - alfa2[i];
                            // alfa2Row[i] = alfa2[i];
                        }
                    }

                    if (i == 1) j++;
                    i++;
                }

                #endregion

                #region Picking Nice Looking Curve 

                int curveShape = MainMenuForm.dataClass.CurveShape;
                int picked = 0;
                int compare = 0;
                bool ok = false;
                bool y1check = true, y2check = true;


                if (double.IsNaN(Y1) || double.IsInfinity(Y1)) y1check = false;

                if (double.IsNaN(Y2) || double.IsInfinity(Y2)) y2check = false;


                if (y1check && y2check) compare = Y1.CompareTo(Y2) * curveShape;
                else if (y1check) compare = 1;
                else if (y2check) compare = -1;
                else compare = 2;


                if (compare == 1) { picked = 0; }
                else if (compare == -1) { picked = 2; }
                else if (compare == 0)
                { }


                if (compare != 2)
                {
                    if (!double.IsNaN(xX[picked])) { ok = true; }
                    else if (!double.IsNaN(xX[picked + 1])) { picked++; ok = true; }

                    cor.AnglesRaw[0] = alfa1;
                    cor.AnglesRaw[1] = alfa2Row[picked];
                    cor.AnglesRaw[2] = alfa3Row[picked];

                    cor.AnglesRad[0] = alfa1;
                    cor.AnglesRad[1] = alfa2[picked];
                    cor.AnglesRad[2] = alfa3[picked];
                }

                #endregion

                #region Wrist angles
                if (ok)
                {
                   
                    #region  R0_3


                    double C1 = Math.Cos(alfa1Row);
                    double S1 = Math.Sin(alfa1Row);

                    double C2 = Math.Cos(alfa2Row[picked]);
                    double S2 = Math.Sin(alfa2Row[picked]);

                    double C3 = Math.Cos(alfa3Row[picked]);
                    double S3 = Math.Sin(alfa3Row[picked]);


                    Matrix3D R0_3 = new Matrix3D
                    (
                      (-C1 * C2 * S3) - (C1 * S2 * C3), S1, (C1 * S2 * S3) + (C1 * C2 * C3), 0,
                      (-S1 * C2 * S3) - (S1 * S2 * C3), -C1, (-S1 * S2 * S3) + (S1 * C2 * C3), 0,
                      (-S2 * S3) + (C2 * C3), 0, (C2 * S3) + (S2 * C3), 0,
                      0, 0, 0, 1
                    );


                    #endregion            

                    #region R3_6
                    R0_3.Invert();

                    Matrix3D R3_6 = Matrix3D.Multiply(cor.RotXYZ, R0_3);


                    #endregion

                    #region Sin Cos Angles
                    double[] sinPNj = new double[6];
                    double[] cosPNj = new double[6];

                    double[] anglesRadPN = new double[6];
                    double[] anglesDegPN = new double[6];


                    double cos5 = R3_6.M33;
                    double sinN5;
                    double sin5 = Math.Sqrt(Math.Abs(1 - (cos5 * cos5)));
                    if (sin5 > 1) sin5 = 1;

                    sinN5 = (-1) * sin5;


                    if (sin5 != 0)
                    {
                        // sin(a4)
                        sinPNj[0] = R3_6.M23 / sin5;

                        sinPNj[3] = R3_6.M23 / sinN5;

                        // cos(a4)
                        cosPNj[0] = R3_6.M13 / sin5;

                        cosPNj[3] = R3_6.M13 / sinN5;

                        //sin(a5)
                        sinPNj[1] = sin5;

                        sinPNj[4] = sinN5;

                        //cos(a5)
                        cosPNj[1] = cos5;

                        cosPNj[4] = cos5;

                        //sin(a6)
                        sinPNj[2] = R3_6.M32 / sin5;

                        sinPNj[5] = R3_6.M32 / sinN5;

                        //cos(a6)
                        cosPNj[2] = (-1) * R3_6.M31 / sin5;

                        cosPNj[5] = (-1) * R3_6.M31 / sinN5;

                        //sinPNj[6] cosPNj[6]

                        for (int i = 0; i < 6;)
                        {
                            anglesRadPN[i] = Math3D.CountRadians(sinPNj[i], cosPNj[i], accuracy);
                            anglesDegPN[i] = anglesRadPN[i] / (2 * Math.PI) * 360;                          
                            i++;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 6;)
                        {
                            anglesRadPN[i] = 0;
                            anglesDegPN[i] = 0;                    
                            i++;
                        }

                        anglesRadPN[2] = RZ / 360 * 2 * Math.PI;
                        anglesRadPN[5] = anglesRadPN[2];
                        anglesDegPN[2] = RZ;
                        anglesDegPN[5] = RZ;

                    }

                    #endregion

                    #region Count Angles
                    Matrix3D CHECK1 = Matrix3D.Identity;
                    Matrix3D CHECK2 = Matrix3D.Identity;

                    for (int i = 0; i < 4;)
                    {
                        double C4 = Math.Cos(anglesRadPN[i]);
                        double C5 = Math.Cos(anglesRadPN[i + 1]);
                        double C6 = Math.Cos(anglesRadPN[i + 2]);

                        double S4 = Math.Sin(anglesRadPN[i]);
                        double S5 = Math.Sin(anglesRadPN[i + 1]);
                        double S6 = Math.Sin(anglesRadPN[i + 2]);

                        if (i == 0)
                        {
                            CHECK1 = new Matrix3D
                            (
                             (C5 * C4 * C6) - (S4 * S6), (-S4 * C6) - (C5 * C4 * S6), C4 * S5, 0,
                             (C5 * S4 * C6) + (C4 * S6), (C4 * C6) - (C5 * S4 * S6), S4 * S5, 0,
                             -S5 * C6, S5 * S6, C5, 0,
                             0, 0, 0, 0
                            );
                        }
                        else
                        {
                            CHECK2 = new Matrix3D
                            (
                             (C5 * C4 * C6) - (S4 * S6), (-S4 * C6) - (C5 * C4 * S6), C4 * S5, 0,
                             (C5 * S4 * C6) + (C4 * S6), (C4 * C6) - (C5 * S4 * S6), S4 * S5, 0,
                             -S5 * C6, S5 * S6, C5, 0,
                             0, 0, 0, 0
                            );
                        }
                        i += 3;
                    }

                    byte matrixCompRes = 4;

                    if (Math3D.CompareMatrix(CHECK1, R3_6, accuracy)) matrixCompRes = 0;
                    else if (Math3D.CompareMatrix(CHECK2, R3_6, accuracy)) matrixCompRes = 3;

                    if (matrixCompRes != 4)
                    {
                        for (int i = 0; i < 3;)
                        {
                            cor.AnglesRaw[i + 3] = anglesRadPN[i + matrixCompRes];

                            cor.AnglesRad[i + 3] = anglesRadPN[i + matrixCompRes];
                            i++;
                        }

                        #region Convert rad to deg
                        for (int i = 0; i < 6; i++)
                        {
                            cor.AnglesDeg[i] = (cor.AnglesRad[i] * 360) / (2 * Math.PI);
                        }
                        #endregion
                    }
                    #endregion

                }

                #endregion
            }

            return cor;
        }

        private Cordinants JointMovement(Cordinants cord)
        {
            cord.XL = cord.X;
            cord.YL = cord.Y;
            cord.ZL = cord.Z;
            return cord;
        }

        //most is working 
        private Cordinants VectorMovement(Cordinants cord, double g)
        {
            double X = cord.X, Y = cord.Y, Z = cord.Z, XL = cord.XL, YL = cord.YL, ZL = cord.ZL;
            double Xa = cord.XB, Ya = cord.YB, Za = cord.ZB;
            double Xo, Zo, Yo, a, b, k, m, s;            
      
            double l = ((Y - YL) * (Y - YL)) + ((X - XL) * (X - XL));

            if ((g * g)< ((l + ((Z - ZL) * (Z - ZL)))))
            {             
                double cosa = (Z - ZL) / (Math.Sqrt(l + ((Z - ZL) * (Z - ZL))));
                double cosb = (X - XL) / Math.Sqrt(l);

                a = Math.Acos(cosa);
                b = Math.Acos(cosb);

                if (a > (Math.PI / 2)) a = a - (Math.PI / 2);

                if (b > (Math.PI / 2)) b = b - (Math.PI / 2);
                else b = (Math.PI / 2) - b;

                m = 1;
                k = 1;
                s = 1;
                if (X < XL) k = -1;
                if (Z < ZL) s = -1;
                if (Y < YL) m = -1;        

                double h = Math.Sin(a) * g;

                Zo = (s * Math.Cos(a) * g) + ZL;

                Xo = (k * Math.Sin(b) * h) + XL;

                Yo = (m * Math.Cos(b) * h) + YL;


                if (Xo < 0.0000001 && Xo > -0.0000001) Xo = 0;

                if (Zo < 0.0000001 && Zo > -0.0000001) Zo = 0;

                if (Yo < 0.0000001 && Yo > -0.0000001) Yo = 0;


                cord.YL = Yo;
                cord.XL = Xo;
                cord.ZL = Zo;

            }
            else
            {
                cord.YL = cord.Y;
                cord.XL = cord.X;
                cord.ZL = cord.Z;
            }



            MainMenuForm.dataClass.CorLast[0] = cord.XL;
            MainMenuForm.dataClass.CorLast[1] = cord.YL;
            MainMenuForm.dataClass.CorLast[2] = cord.ZL;

            return cord;
        }

        private Cordinants VectorMovementUpgraded(Cordinants cord, double g)
        {
            double X = cord.X, Y = cord.Y, Z = cord.Z, XL = cord.XL, YL = cord.YL, ZL = cord.ZL;                                

            double l = Math.Sqrt(((Y - YL) * (Y - YL)) + ((X - XL) * (X - XL)) + ((Z - ZL) * (Z - ZL)));

            if (g < l)
            {
                double skala = g / l;

                cord.XL += skala * (X - XL);
                cord.YL += skala * (Y - YL);
                cord.ZL += skala * (Z - ZL);
         
            }
            else
            {
                cord.YL = cord.Y;
                cord.XL = cord.X;
                cord.ZL = cord.Z;
            }


            MainMenuForm.dataClass.CorLast[0] = cord.XL;
            MainMenuForm.dataClass.CorLast[1] = cord.YL;
            MainMenuForm.dataClass.CorLast[2] = cord.ZL;

            return cord;
        }
        private Cordinants VectorVelocity(Cordinants cord, double velo, bool byt)
        {
            #region Vector Velo
            double X = cord.X, Y = cord.Y, Z = cord.Z, XL = cord.XL, YL = cord.YL, ZL = cord.ZL;

            double l = Math.Sqrt(((Y - YL) * (Y - YL)) + ((X - XL) * (X - XL)) + ((Z - ZL) * (Z - ZL)));

            double skala = velo / l;

            double Xo = skala * (X - XL);
            double Yo = skala * (Y - YL);
            double Zo = skala * (Z - ZL);

            #endregion
            // Xo, Yo, Zo output

            try
            {

                #region Count arm velo
                double a = Math.Cos(cord.AnglesRaw[0]);
                double b = Math.Sin(cord.AnglesRaw[0]);
                double c = Math.Cos(cord.AnglesRaw[1]);
                double d = Math.Sin(cord.AnglesRaw[1]);
                double e = Math.Cos(cord.AnglesRaw[2]);
                double f = Math.Sin(cord.AnglesRaw[2]);
                double A, B, C, D, E, F, G;
                double x1, x2, x3, y1, y2, y3, v1, v2, v3, w1, w2, w3;
                double a1 = MainMenuForm.dataClass.A1, a3 = MainMenuForm.dataClass.A3;

                A = 2 * a;
                B = -(a * c * f) - (a * d * e) + (b * e / f);
                C = b - (a * a / b);
                D = -(b * c * f) - (b * d * e) - (a * e / f);
                E = d - (c * c / d);
                F = (c * e) - (d * f);

                x3 = (Yo - (Xo * C / A)) / (D - (B * C / A));
                x1 = (Xo - (x3 * B)) / A;
                x2 = (Zo - (x3 * F)) / E;

                y1 = x1 * a / b;
                y2 = x2 * c / d;
                y3 = x3 * e / f;

                v1 = Math.Sqrt((x1 * x1) + (y1 * y1));
                v2 = Math.Sqrt((x2 * x2) + (y2 * y2));
                v3 = Math.Sqrt((x3 * x3) + (y3 * y3));

                w1 = v1 / Math.Sqrt((Yo * Yo) + (Xo * Xo));
                w2 = v2 / a1;
                w3 = v3 / a1;

                int[] gr = dataClass.GearReduction;
                double velScale = MainMenuForm.dataClass.VelScale;

                cord.Velocity[0] = Convert.ToInt32(w1 / 2 / Math.PI * gr[0]);
                cord.Velocity[1] = Convert.ToInt32(w2 / 2 / Math.PI * gr[1]);
                cord.Velocity[2] = Convert.ToInt32(w3 / 2 / Math.PI * gr[2]);
                cord.Velocity[3] = Convert.ToInt32(velo / a3 / 2 / Math.PI);
                cord.Velocity[4] = cord.Velocity[3] * Convert.ToInt32(gr[4]);
                cord.Velocity[5] = cord.Velocity[3] * Convert.ToInt32(gr[5]);
                cord.Velocity[3] = cord.Velocity[3] * Convert.ToInt32(gr[3]);
                cord.Velocity[6] = MainMenuForm.dataClass.Acceleration;
                cord.Velocity[7] = MainMenuForm.dataClass.Deacceleration;
                #endregion
            }
            catch (Exception) { byt = false; }

            #region Prepare to send
            string data = null;
            if (byt)
            {
                int[] vel = new int[8];
                for (int i = 0; i < 8; i++)
                {
                    vel[i] = Convert.ToInt32(Convert.ToDouble(cord.Velocity[i]) * MainMenuForm.dataClass.VelScale);
                }

                Classes.SimpleLan.Command cm = new Classes.SimpleLan.Command();
                cm.CreateNew(35, false, 2, vel);
                cord.cm2 = cm;
                //data = ConvertToMessage(35,vel);
            }
            #endregion
            // data (string to send) output
            //cord.DataVelocity = data;

            return cord;
        }

        #endregion

        //maIN
        /*
        private void Main()
        {

            #region  Veriables for SP
            int QueLength = MainMenuForm.dataClass.QueLength;
            string[] data = new string[QueLength];

            Stopwatch stopwatch = new Stopwatch();
            stopwatch = Stopwatch.StartNew();
            Stopwatch timer = new Stopwatch();
            timer = Stopwatch.StartNew();

            int counter = 0;
            bool wait = false, elap = false;
            long elapsed = 0;


            stopwatch.Reset();
            timer.Restart();
            #endregion

            #region Comunication Codes
            string stopTrue = "@\u0004G" + (char)32 + (char)61 + '#';

            string stopFalse = "@\u0004G" + (char)32 + (char)69 + '#';

            string onlineCode = "@" + (char)3+ 'G' + (char)8 + '#';

            string ShutDownRobot = "@" + (char)3 + 'G' + (char)60 + '#';

            #endregion

            #region Veriables
            //AngelsRet angles = new AngelsRet();
            Cordinants cord = new Cordinants();
            bool counted = false, startCount=false;
            bool forceCount;
            bool fileOpened = false;
            int indexCur = 0;
            string readed;
            int compare = 5;
            string veloAutoCounted = null;
            string angleCounted = null;
            MainMenuForm.dataClass.UpdateSetup = true;
            MainMenuForm.dataClass.TurnOffRobot = false;
            #endregion

            #region FromFile
            StreamReader reader = null;

            if (MainMenuForm.movementWayPath.RunFrom)
            {
                if (System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
                {
                    reader = new StreamReader(MainMenuForm.movementWayPath.Path);
                    fileOpened = true;
                    counted = false;

                    //If not from the beginning
                    if (MainMenuForm.movementWayPath.SkipLines && fileOpened)
                    {
                        int index = MainMenuForm.movementWayPath.IndexLb;
                        string readedin;

                        while (reader.Peek() != -1 && indexCur != index)
                        {
                            readedin = reader.ReadLine();
                            indexCur++;
                        }

                    }
                }
                else
                {
                    MainMenuForm.dataClass.ThreadFileProblem = true;
                    MainMenuForm.movementWayPath.Run = false;
                    //StopAllinOne();
                }
            }

            #endregion

            byte conType = dataClass.ConectionType;

            switch (conType)
            {
                case 1:
                    ConnectSerialPort();
                    break;
                case 2:
                    ConnectLanWiFi();
                    break;
                case 3:

                    break;
            }

            if (dataClass.ConnectionAcomplished)
            {
                try
                {
                    while (MainMenuForm.dataClass.Threads)
                    {
                        //ReadSerialPortAllinOne();

                        #region Math
                        if (MainMenuForm.dataClass.UploadMath)
                        {
                            #region Math
                            string dataAngles = null;
                            string setSpeed = null;
                            if (MainMenuForm.dataClass.ManualOrAuto)
                            {
                                dataAngles = SetAnglesManual();
                                setSpeed = SetSpeedManual();
                                MainMenuForm.dataClass.UploadMath = false;
                            }
                            else
                            {
                                forceCount = MainMenuForm.dataClass.ForceCount;
                                if (forceCount)
                                {
                                    cord.X = MainMenuForm.dataClass.Cor[0];
                                    cord.Y = MainMenuForm.dataClass.Cor[1];
                                    cord.Z = MainMenuForm.dataClass.Cor[2];
                                    cord.RX = MainMenuForm.dataClass.Jo[3];
                                    cord.RY = MainMenuForm.dataClass.Jo[4];
                                    cord.RZ = MainMenuForm.dataClass.Jo[5];
                                }
                                

                                #region Check whether next run is required
                                if (cord.XL == cord.X && cord.YL == cord.Y && cord.ZL == cord.Z)
                                {
                                    cord.XB = cord.X; cord.YB = cord.Y; cord.ZB = cord.Z;
                                    dataClass.UploadMath = false;
                                }
                                #endregion

                                if (CheckIfachived(MainMenuForm.dataClass.JoIN, cord.AngleLast, compare) && counted && !forceCount || startCount)
                                {
                                    startCount = false;
                                    setSpeed = veloAutoCounted;
                                    dataAngles = angleCounted;
                                    counted = false;
                                }
                                else if (!counted || forceCount)
                                {       
                                    cord = MathAllinOne(cord,false);
                                    veloAutoCounted = cord.DataVelocity;
                                    angleCounted = cord.DataS;
                                    counted = true;

                                    if (forceCount)
                                        startCount = true;
                                    MainMenuForm.dataClass.ForceCount = false;
                                }

                            }
                            #endregion

                            // Whitch first should be send?
                            //1

                            //2
                            data = AddQueueOut(data, setSpeed, QueLength);
                            //3
                            data = AddQueueOut(data, dataAngles, QueLength);
                            //4

                        }
                        else if (MainMenuForm.movementWayPath.Run)
                        {
                            string dataAngles = null;
                            //open Streamreader to read file                 
                            try
                            {
                                if (CheckIfachived(MainMenuForm.dataClass.JoIN, cord.AngleLast, compare) && counted)
                                {
                                    dataAngles = cord.DataSF;
                                    counted = false;
                                }
                                else if (!counted)
                                {
                                    readed = reader.ReadLine();

                                    if (readed.Length >= 2)
                                    {
                                        if (readed != "G4")
                                        {
                                            cord = EncodeFromFileToSend(readed, cord);

                                            counted = true;
                                        }
                                    }

                                }
                                data = AddQueueOut(data, dataAngles, QueLength);
                            }
                            catch (Exception) { }
                        }
                        #endregion

                        #region Seting

                        if (MainMenuForm.dataClass.UpdateSetup)
                        {
                            MainMenuForm.dataClass.UpdateSetup = false;

                            int[] dataLimits = new int[12];
                            for (int h = 0; h < 12; h++)
                            {
                                double limitDa= MainMenuForm.dataClass.JoLimits[h]; 
                                if (h <= 5)
                                    limitDa = limitDa * 11.375;    // 0-360
                                else
                                    limitDa = (limitDa * 11.375) + 2047.5;         // (-180)-180
                                dataLimits[h] = Convert.ToInt32(limitDa);
                            }

                            data = AddQueueOut(data,ConvertToMessage(40,dataLimits),QueLength);
                        }

                        if (!MainMenuForm.dataClass.StopSend)
                        {
                            string stopData;
                            if (MainMenuForm.dataClass.Stop) stopData = stopTrue;
                            else stopData = stopFalse;
                            MainMenuForm.dataClass.StopSend = true;
                            data = AddQueueOut(data, stopData, QueLength);
                        }

                        if (MainMenuForm.dataClass.TurnOffRobot)
                        {
                            MainMenuForm.dataClass.TurnOffRobot = false;
                            data = AddQueueOut(data, ShutDownRobot, QueLength);
                        }

                        #endregion

                        #region Send             

                        data = AddQueueOut(data, "", QueLength);
                        try
                        {

                            if (timer.ElapsedMilliseconds >= 500)
                            { SendToRobot(conType, onlineCode); timer.Restart(); }

                            string da = data[0];
                            if (!wait || MainMenuForm.dataClass.Notack || elap)
                            {
                                if (da != null)
                                {
                                    SendToRobot(conType, da);
                                    stopwatch.Restart();
                                    wait = true;
                                    elap = false;
                                    MainMenuForm.dataClass.Notack = false;
                                    MainMenuForm.dataClass.Ack = false;
                                    counter++;
                                }
                            }

                            if (MainMenuForm.dataClass.Ack && wait)
                            {
                                MainMenuForm.dataClass.Ack = false;                                                      
                                MainMenuForm.dataClass.Notack = false;
                                wait = false;
                                stopwatch.Stop();
                                timer.Restart();
                                counter = 0;
                                data[0] = null;                                                           
                            }
                            else
                            {
                                elapsed = stopwatch.ElapsedMilliseconds;
                                if (elapsed >= 20) elap = true;
                            }

                            if (counter > 10)
                            {
                                MainMenuForm.dataClass.Ack = false;
                                MainMenuForm.dataClass.Notack = false;
                                wait = false;
                                stopwatch.Stop();
                                timer.Restart();
                                counter = 0;
                                data[0] = null;

                            }

                        }
                        catch (Exception) { }

                        #endregion

                    }

                    if (reader != null) reader.Close();

                }
                catch (Exception)
                {
                    MainMenuForm.dataClass.ErrorStop = true;
                }
            }
            switch (conType)
            {
                case 1:
                    if (serialPort.IsOpen) serialPort.Close();
                    break;
                case 2:
                    if (readLanWifiThread.IsAlive)
                    {
                        readingLanWiFi = false;
                        readLanWifiThread.Join();
                    }
                    break;
                case 3:
                    break;
            }
        }
        */
        
        //ended
        #region Start/Stop/Restart
        private void Start()
        {
            try
            {
                if (dataClass.UploadAble)
                {
                    dataClass.Threads = true;
                    readSP = new Thread(MainUpgraded);
                    if (!readSP.IsAlive) readSP.Start();

                    iconButtonConnect.Text = "Disconect";
                    iconButtonConnect.BackColor = Color.FromArgb(50, 50, 50);
                    iconButtonConnect.ForeColor = RGBcolors.color6;
                    iconButtonConnect.IconColor = RGBcolors.color6;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error, Starting Conection", "Conection Error");
                Stop();
                dataClass.Connect = false;
            }
        }

        private void Stop()
        {
            if (readSP != null)
            {
                if (readSP.IsAlive)
                {
                    MainMenuForm.dataClass.Threads = false;
                    readSP.Join();
                }

                iconButtonConnect.Text = "Connect";
                label4.Text = "Conection Status: Disconected";
                iconButtonConnect.BackColor = Color.FromArgb(35, 35, 35);
                iconButtonConnect.ForeColor = RGBcolors.color7;
                iconButtonConnect.IconColor = RGBcolors.color7;
            }

            MainMenuForm.dataClass.Missed += MainMenuForm.dataClass.MissedCur;
            MainMenuForm.dataClass.MissedCur = 0;
        }

        private void Restart()
        {
            Stop();
            Start();
        }

        #endregion

        //ended
        #region Set speed Manual CheckIfAchAngle  SetManualAngles

        private Classes.SimpleLan.Command SetSpeedManual()
        {
            string data;
           
            int[] vel = new int[8];
            for (int i = 0; i < 6;)
            {
                vel[i] = Convert.ToInt32(MainMenuForm.dataClass.SpeedManula[i] / 360.0 * MainMenuForm.dataClass.VelScale * dataClass.GearReduction[i]);
            }
            vel[7] = MainMenuForm.dataClass.Acceleration;
            vel[8] = MainMenuForm.dataClass.Deacceleration;

            Classes.SimpleLan.Command cm = new Classes.SimpleLan.Command();
            cm.CreateNew(35, false, 2, vel);
            //data = ConvertToMessage(35,vel);       
            return cm;
        }

        private Cordinants SetVeocityJoint(Cordinants cord)
        {
            double ve = Convert.ToDouble(MainMenuForm.dataClass.Speed) / 360.0;

            cord.Velocity[6] = MainMenuForm.dataClass.Acceleration;
            cord.Velocity[7] = MainMenuForm.dataClass.Deacceleration;
            int[] velo = new int[8];
            velo[6] = cord.Velocity[6];
            velo[7] = cord.Velocity[7];

            for (int i = 0; i < 6; i++)
            {
                cord.Velocity[i] = Convert.ToInt32(ve * Convert.ToDouble(dataClass.GearReduction[i]));
                velo[i] = Convert.ToInt32(Convert.ToDouble(cord.Velocity[i]) * MainMenuForm.dataClass.VelScale);
            }
            Classes.SimpleLan.Command cm = new Classes.SimpleLan.Command();
            cm.CreateNew(35, false, 2, velo);
            cord.cm2 =cm;
            //cord.SetDataToSend(35, velo);
            //cord.DataVelocity = ConvertToMessage(35, velo);

            return cord;
        }

        private bool CheckIfachived(int[] joIN, int[] Jo, int approximateNumber)
        {
            bool condition = true;

            for (int i = 0; i < 6;)
            {
                if (Math.Abs(joIN[i] - Jo[i]) < approximateNumber)
                {
                    condition = false;
                    break;
                }
                i++;
            }

            return condition;
        }

        private Classes.SimpleLan.Command SetAnglesManualCom()
        {
            int[] dat = new int[6];
            int i = 0, resulution = MainMenuForm.dataClass.Resolution; ;
            
            foreach (double item in  MainMenuForm.dataClass.Jo)
            {
                double g = item / 360 * resulution;
                dat[i] = Convert.ToInt32(g);
                i++;
            }

            Classes.SimpleLan.Command cm = new Classes.SimpleLan.Command();
            cm.CreateNew(34,false,2,dat);
            //string data = ConvertToMessage(34,dat);
           
            return cm;
        }
        private string SetAnglesManual()
        {
            int[] dat = new int[6];
            int i = 0, resulution = MainMenuForm.dataClass.Resolution; ;

            foreach (double item in MainMenuForm.dataClass.Jo)
            {
                double g = item / 360 * resulution;
                dat[i] = Convert.ToInt32(g);
                i++;
            }
            string data = ConvertToMessage(34,dat);

            return data;
        }

        #endregion    

        //In progress 2/3
        #region Connections

        #region SerialPort Read and Execute
        private void ConnectSerialPort()
        {
            try
            {
                if (MainMenuForm.serialPort.IsOpen) MainMenuForm.serialPort.Close();
                SetupSerialPort();
                serialPort.PortName = dataClass.Portname;
                serialPort.DataReceived += ReadSerialPort;
                MainMenuForm.serialPort.Open();
                dataClass.ConnectionAcomplished = true;
            }
            catch (Exception) { }
        }
        private void SetupSerialPort()
        {
            serialPort.ReadBufferSize = 8192;
            serialPort.BaudRate = 115200;
            serialPort.ReadTimeout = 50;
        }
        private void ReadSerialPort(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort) sender;
            try
            {
                //
                if (MainMenuForm.serialPort.BytesToRead > 4)
                {
                    char beg = Convert.ToChar(serialPort.ReadChar());

                    if (beg == '@')
                    {
                        MainMenuForm.dataClass.StopE++;
                        int cou = (int)MainMenuForm.serialPort.ReadByte();
                        byte[] byts = new byte[cou];
                        MainMenuForm.serialPort.Read(byts, 0, cou);

                        if (byts != null)
                        {
                            MainMenuForm.dataClass.StopD++;
                            QueueIn(byts);
                        }
                        else
                        {
                            MainMenuForm.dataClass.MissedCur++;
                        }

                    }
                    else {  MainMenuForm.dataClass.StopB++; }
                }
            }
            catch (Exception ex) { }
        }
        #endregion

        #region LanWiFIConection

        private void ConnectLanWiFi()
        {
            string host = dataClass.Ip;
            int port = Convert.ToInt32(dataClass.Port);
            string localIP = DeviceIP();

            try
            {     
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    bool connectedToTheNetwork = true;            
                                
                    if (connectedToTheNetwork && localIP != null)
                    {                                    
                        try
                        {
                            if (socketCon != null) if (socketCon.Connected) socketCon.Close();

                            socketCon = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                            socketCon.ReceiveBufferSize = 8192;
                            socketCon.ReceiveTimeout = 1000;
                            
                            socketCon.Connect(host,port);
                            dataClass.ConnectionAcomplished = true;

                            readingLanWiFi = false;

                            if (readLanWifiThread != null) if (readLanWifiThread.IsAlive) readLanWifiThread.Join();
                            
                            readingLanWiFi = true;
                            readLanWifiThread = new Thread(ReadLanWifi);
                            readLanWifiThread.Start();

                        }
                        catch (Exception e){ MessageBox.Show(e.ToString()) ; }
                                                                                                                                      
                    }
                    else MessageBox.Show("No Connection.", "Network Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else MessageBox.Show("No available connections!", "Network Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){ MessageBox.Show("Couldn't start connection." + ex.ToString(),"Network Problem",MessageBoxButtons.OK,MessageBoxIcon.Information);}      
        }

        public static string DeviceIP()
        {
            string localIP = null;
            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }
            }
            catch (Exception) { }

            return localIP;
        }

        private void ReadLanWifi()
        {
            //List<byte> buffer = new List<byte> { }; 
            Stopwatch sw = new Stopwatch();
            sw.Start();
            long delay = 500;
            bool firstData = false;
            while (readingLanWiFi)
            {
                try
                {
                    byte[] readB = new byte[64];
                    int g = socketCon.Receive(readB);
                    int p = 0;
                    foreach (byte item in readB)
                    {
                        if (item == (byte)'@')
                        {
                            //data () - 8 bit   [] - a few bits  -->  @ (number of bytes)  G (code)    [data]       #  <--
                            //bytes                   bits = b                             0    1   <b + 3 ;b-2>   b-1    

                            int size = readB[p + 1];
                            if (readB[p + 2] == (byte)'G' && readB[size + p + 1] == (byte)'#' && size < 40)
                            {
                                sw.Restart();
                                byte[] buffer = new byte[size];
                                int h = 0;
                                for (int i = p + 2; i <= (p + size); i++)
                                {
                                    buffer[h++] = readB[i];
                                }
                                MainMenuForm.dataClass.StopD++;
                                QueueIn(buffer);
                                MainMenuForm.dataClass.GetConInfo = true;

                            }
                            else
                                MainMenuForm.dataClass.MissedCur++;

                        }
                        else
                            // MainMenuForm.dataClass.StopB++;
                            p++;
                    }

                    if (sw.ElapsedMilliseconds > delay)
                    { MainMenuForm.dataClass.GetConInfo = false; firstData = false; }

                    if (firstData != MainMenuForm.dataClass.GetConInfo) 
                    {
                        firstData = MainMenuForm.dataClass.GetConInfo;
                        ErrorBytes iop = new ErrorBytes();
                        iop.Code = 10;

                        if (MainMenuForm.dataClass.GetConInfo)
                            iop.Value = 1;
                        else
                            iop.Value = 2;

                        iop.SetTime();

                        _errorList.Add(iop);
                        MainMenuForm.dataClass.EngError = true;
                    }
                }
                catch (Exception e) { MainMenuForm.dataClass.GetConInfo = false; }
            }
            MainMenuForm.dataClass.GetConInfo = false;

            ErrorBytes ios = new ErrorBytes();
            ios.Code = 10;
            ios.Value = 2;
            ios.SetTime();
            _errorList.Add(ios);
            MainMenuForm.dataClass.EngError = true;

        }

        #endregion

        #region Rest        
        private void SendToRobot(byte conType, string data)
        {

            switch (conType)
            {
                case 1:
                    MainMenuForm.serialPort.Write(data);
                    break;
                case 2:
                    byte[] dat = new byte[data.Length];
                    int i = 0;
                    foreach (char item in data)
                    {
                        dat[i]= (byte)item; i++;
                    }
                    int ret = MainMenuForm.socketCon.Send(dat,dat.Length,SocketFlags.None);
                   // MainMenuForm.socketCon.Send(dat);
                    break;
                case 3:
                    break;
            }
        }

        public void QueueIn(byte[] data)
        {
            try
            {
                if (data[0] == (byte)'G')
                {
                    
                    switch (data[1])
                    {

                        case 21:   // Stop COM----------------------

                            MainMenuForm.dataClass.GetConInfo = true;
                            if (data[2] == 61) { MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                            else if (data[2] == 31) { MainMenuForm.dataClass.Stop = false; MainMenuForm.dataClass.StopDon = true; }
                            break;

                        case 22:    //No Power COM----------------------

                            MainMenuForm.dataClass.GetConInfo = true;
                            if (data[2] == 62) { MainMenuForm.dataClass.NoPower = true; MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                            else if (data[2] == 32) { MainMenuForm.dataClass.NoPower = false; }
                            break;


                        case 24: // ANgels Read--------------

                            MainMenuForm.dataClass.GetConInfo = true;
                            ReadAngels(data);
                            break;

                        case 25: // NOACK
                            MainMenuForm.dataClass.GetConInfo = true;                           
                            MainMenuForm.dataClass.Notack = true;
                            break;

                        case 29: // ACK
                            MainMenuForm.dataClass.GetConInfo = true;                         
                            MainMenuForm.dataClass.Ack = true;
                            break;

                        case 30: 
                            MainMenuForm.dataClass.GetConInfo = true;

                            ErrorBytes errorBytes = new ErrorBytes();
                            errorBytes.Code = data[2];
                            if(data.Length>=3)
                               errorBytes.Value = data[3];
                            errorBytes.SetTime();

                            _errorList.Add(errorBytes);
                            MainMenuForm.dataClass.EngError = true;
                            break;
                        case 31:

                            break;

                    }

                }
                else
                {
                    //MessageBox.Show("Error, Thread Prepare, (read:@G)");
                }


            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Prepare"); }

        }

        private string[] AddQueueOut(string[] data, string EndLine, int QueLength)
        {
            if (EndLine != null)
            {
                if (EndLine == "") EndLine = null;

                if (data[QueLength - 1] == null)
                    data[QueLength - 1] = EndLine;
                else
                {
                    MainMenuForm.dataClass.Threads = false;
                    MessageBox.Show("Queue Is Full", "Sending problem");
                }

                int counter2 = 0;
                int numCh = 0;
                foreach (string item in data)
                {
                    if (data[counter2] != null)
                    {
                        counter2++;
                    }
                    else if (item != null)
                    {
                        data[numCh] = null;
                        data[counter2] = item;
                        counter2++;
                        if (counter2 >= QueLength) counter2 = 0;
                    }
                    numCh++;
                    if (numCh >= QueLength) numCh = 0;
                }
            }

            return data;
        }

        private void ReadAngels(byte[] byt)
        {
            int[] ang = new int[6];
            int per;
            int counter = 0;

            for (int i = 2; i <= 12;)
            {
                per = (int)byt[i];
                per = (per << 8);
                per += (int)byt[i + 1];
                ang[counter] = per;
                counter++;
                i += 2;
            }

            MainMenuForm.dataClass.JoIN = ang;
        }

        private string ConvertToMessage(byte code, int[] data)
        {
            char end = '#', codeBeg = 'G';
            string dataReturn = "@";
            int length = data.Length;

            dataReturn += (char)(2 * length + 3);
            dataReturn += codeBeg;
            dataReturn += (char)code;
            //dataReturn += beg + (char)(2*length + 2) + codeBeg + (char)code;

            for (int i = 0; i < length; i++)
            {
                int value = data[i];
                byte byt2 = (byte)(value & 0xff);
                value = (value >> 8);
                byte byt1 = (byte)(value & 0xff);
                dataReturn += (char)byt1;
                dataReturn += (char)byt2;
            }

            dataReturn += end;
            return dataReturn;
        }

        private void QueueInUpgraded(Classes.SimpleLan.Command cm)
        {
            try
            {           
                switch (cm.code)
                {

                    case 21:   // Stop COM----------------------

                        MainMenuForm.dataClass.GetConInfo = true;
                        if (cm.dataOnly[0] == 61) { MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                        else if (cm.dataOnly[0] == 31) { MainMenuForm.dataClass.Stop = false; MainMenuForm.dataClass.StopDon = true; }
                        break;

                    case 22:    //No Power COM----------------------

                        MainMenuForm.dataClass.GetConInfo = true;
                        if (cm.dataOnly[0] == 62) { MainMenuForm.dataClass.NoPower = true; MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                        else if (cm.dataOnly[0] == 32) { MainMenuForm.dataClass.NoPower = false; }
                        break;


                    case 24: // ANgels Read--------------

                        MainMenuForm.dataClass.GetConInfo = true;
                        ReadAngelsUpgraded(cm.dataOnly);
                        break;

                    case 30:
                        MainMenuForm.dataClass.GetConInfo = true;

                        Classes.SimpleLan.ErrorClass ec = new Classes.SimpleLan.ErrorClass() { command = cm, shortMsg = "DeviceError"};
                        ec.SetCurrentTime();
                        errorListNumerous.Add(ec);
                        try
                        {
                            var a = ErrorExplenationRobot(ec,false);
                            if (listBoxErrorList.Items.Count > 30) listBoxErrorList.Items.Clear();
                            listBoxErrorList.Items.Add(a);
                            if (listBoxErrorList.Items.Count > 1) listBoxErrorList.SelectedIndex = listBoxErrorList.Items.Count - 1;
                        }
                        catch (Exception) { }

                        break;
                    case 31:

                        break;

                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Prepare"); }

        }

        private void ReadAngelsUpgraded(byte[] byt)
        {
            MainMenuForm.dataClass.JoIN = Classes.SimpleLan.BytesToIntArray(byt, 2);
        }

        #endregion

        #endregion

        //(work suspended)
        #region OLD

        private void StartThread()
        {
            try
            {
                if (MainMenuForm.dataClass.UploadAble)
                {
                    dataClass.Threads = true;

                    readSP = new Thread(ReadSerialPortOptimalised);
                    if (!readSP.IsAlive) readSP.Start();

                    // readSP = new Thread(ReadSerialPort);               
                    //  if (!readSP.IsAlive) readSP.Start();  

                    //  prepare = new Thread(QueueIN);                 
                    //  if (!prepare.IsAlive) prepare.Start(); 

                    queueSend = new Thread(SendSerialport);
                    if (!queueSend.IsAlive) queueSend.Start();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Error, Starting threads");
            }
        }
        public void StopThread()
        {

            if (readSP != null)
            {
                if (readSP.IsAlive)
                {
                    MainMenuForm.dataClass.Threads = false;
                    readSP.Join();
                }
            }
            if (prepare != null)
            {
                if (prepare.IsAlive)
                {
                    MainMenuForm.dataClass.Threads = false;
                    prepare.Join();
                }
            }
            if (queueSend != null)
            {
                if (queueSend.IsAlive)
                {

                    MainMenuForm.dataClass.Threads = false;
                    queueSend.Join();
                }
            }


            MainMenuForm.dataClass.Missed += MainMenuForm.dataClass.MissedCur;
            MainMenuForm.dataClass.MissedCur = 0;

        }

        //ended (work suspended)
        #region OLD Read Serial with QueueIn 50% of the processor
        private void RSP()
        {

            char beg;
            byte[] lon = new byte[1];
            int length;
            string i;
            int g = 0;
            int c = 0;
            int cou = 0;


            while (MainMenuForm.dataClass.Threads)
            {
                try
                {

                    if (MainMenuForm.serialPort.BytesToRead > 32)
                    {
                        beg = Convert.ToChar(serialPort.ReadChar());

                        if (beg == '@')
                        {
                            MainMenuForm.dataClass.StopE++;
                            cou = (int)MainMenuForm.serialPort.ReadByte();
                            byte[] byts = new byte[cou];
                            MainMenuForm.serialPort.Read(byts, 0, cou);
                            if (byts != null)
                            {
                                c = 0;
                                MainMenuForm.dataClass.StopD++;
                                MainMenuForm.dataClass.Modifi = g;
                                foreach (byte item in byts)
                                {
                                    MainMenuForm.dataClass.QueueIn[g] += (char)byts[c];
                                    c++;
                                    MainMenuForm.dataClass.StopC++;
                                }
                                g++;


                            }
                            else
                            {
                                MainMenuForm.dataClass.MissedCur++;
                            }
                            if (g == 64) g = 0;
                        }
                        else
                        {
                            int po = (int)beg;
                            MainMenuForm.dataClass.Trash = po.ToString();
                            MainMenuForm.dataClass.StopB++;
                        }
                    }


                }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Read: ReadSerialPort "); }
            }

        }

        private void QueueIN()
        {
            bool iga = false;
            int g = 0;
            int length = 0;
            string data = "";
            while (MainMenuForm.dataClass.Threads)
            {
                try
                {
                    data = null;
                    if (MainMenuForm.dataClass.Modifi != g) data = MainMenuForm.dataClass.QueueIn[g];

                    if (data != null)
                    {

                        length = data.Length;
                        char[] ch = new char[length]; byte[] byt = new byte[length];

                        int count = 0;
                        foreach (char h in data)
                        {
                            ch[count] = h;
                            byt[count] = (byte)h;
                            count++;
                        }

                        if (ch[0] == 'G')
                        {
                            switch (byt[1])
                            {
                                case 21:   // Stop COM
                                    if (!iga)
                                    {
                                        MainMenuForm.dataClass.StopD = 0;
                                        MainMenuForm.dataClass.StopE = 0;
                                        MainMenuForm.dataClass.StopB = 0;
                                        iga = true;
                                    }
                                    MainMenuForm.dataClass.StopC++;
                                    if (byt[2] == 61) { MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                                    else if (byt[2] == 31) { MainMenuForm.dataClass.Stop = false; MainMenuForm.dataClass.StopDon = true; }
                                    break;

                                case 22:    //No Power COM
                                    if (byt[2] == 62) { MainMenuForm.dataClass.NoPower = true; MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                                    else if (byt[2] == 32) { MainMenuForm.dataClass.NoPower = false; }
                                    break;


                                case 24:
                                    ReadAngels(byt);
                                    break;
                                case 25:

                                    break;
                                case 28:
                                    break;
                                case 29:
                                    break;
                                case 30:
                                    break;
                            }
                            MainMenuForm.dataClass.QueueIn[g] = null;
                        }
                        else
                        {
                            //MessageBox.Show("Error, Thread Prepare, (read:@G)");
                        }
                    }
                    g++; if (g >= 64) g = 0;
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Prepare"); }
            }
        }
        #endregion

        //ended  (work suspended)
        #region Optimalized to 25% of the processor READ Srial port AND QueIN FUNCTION
        private void QueueInOptimalised(byte[] data)
        {
            try
            {
                if (data != null)
                {
                    if (data[0] == (byte)'G')
                    {
                        switch (data[1])
                        {

                            case 21:   // Stop COM----------------------


                                if (data[2] == 61) { MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                                else if (data[2] == 31) { MainMenuForm.dataClass.Stop = false; MainMenuForm.dataClass.StopDon = true; }
                                break;

                            case 22:    //No Power COM----------------------

                                if (data[2] == 62) { MainMenuForm.dataClass.NoPower = true; MainMenuForm.dataClass.Stop = true; MainMenuForm.dataClass.StopDon = true; }
                                else if (data[2] == 32) { MainMenuForm.dataClass.NoPower = false; }
                                break;


                            case 24: // ANgels Read--------------
                                MainMenuForm.dataClass.StopC++;
                                ReadAngels(data);
                                break;

                            case 25:
                                MainMenuForm.dataClass.Notack = true;
                                break;
                            case 28:
                                break;
                            case 29:
                                MainMenuForm.dataClass.Ack = true;
                                break;
                            case 30:
                                break;
                        }
                    }
                    else
                    {
                        //MessageBox.Show("Error, Thread Prepare, (read:@G)");
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Prepare"); }
        }
        private void ReadSerialPortOptimalised()
        {
            char beg;
            byte[] lon = new byte[1];
            int length;
            string i;
            int g = 0;
            int c = 0;
            int cou = 0;

            while (MainMenuForm.dataClass.Threads)
            {
                try
                {
                    if (MainMenuForm.serialPort.BytesToRead > 16)
                    {
                        beg = Convert.ToChar(serialPort.ReadChar());

                        if (beg == '@')
                        {
                            MainMenuForm.dataClass.StopE++;
                            cou = (int)MainMenuForm.serialPort.ReadByte();
                            byte[] byts = new byte[cou];
                            MainMenuForm.serialPort.Read(byts, 0, cou);
                            if (byts != null)
                            {
                                MainMenuForm.dataClass.StopD++;
                                QueueInOptimalised(byts);
                            }
                            else
                            {
                                MainMenuForm.dataClass.MissedCur++;
                            }
                        }
                        else
                        {
                            int po = (int)beg;
                            MainMenuForm.dataClass.Trash = po.ToString();
                            MainMenuForm.dataClass.StopB++;
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error, Thread Read: ReadSerialPort "); }
            }
        }

        #endregion

        #region SendSerialPort and chech what to send
        private void SendSerialport()
        {
            byte[] byt = { (byte)'@', 2, (byte)'G', 8 };
            string s = "", EndLine = "";
            s += (char)byt[0]; s += (char)byt[1]; s += (char)byt[2]; s += (char)byt[3];
            int QueLength = MainMenuForm.dataClass.QueLength;
            string[] data = new string[QueLength];


            Stopwatch stopwatch = new Stopwatch();
            stopwatch = Stopwatch.StartNew();
            Stopwatch timer = new Stopwatch();
            timer = Stopwatch.StartNew();

            int counter = 0, counter2 = 0, numCh = 0;

            bool wait = false, elap = false; ;
            long elapsed = 0;


            stopwatch.Reset();
            timer.Restart();

            while (MainMenuForm.dataClass.Threads)
            {
                try
                {
                    try
                    {
                        if (MainMenuForm.dataClass.Busy && MainMenuForm.dataClass.Take)
                        {
                            if (data[QueLength - 1] == null)
                            {
                                EndLine = MainMenuForm.dataClass.QueueAdd;
                                MainMenuForm.dataClass.QueueAdd = null;
                                MainMenuForm.dataClass.Take = false;
                                MainMenuForm.dataClass.Busy = false;
                                data[QueLength - 1] = EndLine;
                                EndLine = null;
                            }
                            else
                            {
                                MessageBox.Show("Queue Is Full", "Queue Send problem");
                            }

                        }
                        counter2 = 0;
                        numCh = 0;
                        foreach (string item in data)
                        {

                            if (data[counter2] != null)
                            {
                                counter2++;
                            }
                            else if (item != null)
                            {
                                data[numCh] = null;
                                data[counter2] = item;
                                counter2++;
                                if (counter2 >= QueLength) counter2 = 0;
                            }
                            numCh++;
                            if (numCh >= QueLength) numCh = 0;
                        }
                    }
                    catch (Exception) { }


                    if (timer.ElapsedMilliseconds >= 500) { MainMenuForm.serialPort.Write(s); timer.Restart(); }


                    if (!wait || MainMenuForm.dataClass.Notack || elap)
                    {
                        if (MainMenuForm.dataClass.QueueOut[0] != null)
                        {
                            serialPort.Write(MainMenuForm.dataClass.QueueOut[0]);
                            stopwatch.Restart();
                            wait = true;
                            elap = false;
                            MainMenuForm.dataClass.Notack = false;
                            MainMenuForm.dataClass.Ack = false;
                            counter++;
                        }
                    }

                    if (MainMenuForm.dataClass.Ack && wait == true)
                    {
                        MainMenuForm.dataClass.Ack = false;
                        MainMenuForm.dataClass.Notack = false;
                        wait = false;
                        stopwatch.Stop();
                        timer.Restart();
                        counter = 0;
                        data[0] = null;
                    }
                    else
                    {
                        elapsed = stopwatch.ElapsedMilliseconds;
                        if (elapsed >= 20) elap = true;
                    }

                    if (counter >= 10)
                    {
                        label4.Text = "Error, Sending: " + MainMenuForm.dataClass.QueueOut[0];
                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.ToString(), "Error,Thread Send:  Queue Send"); }
            }
        }

        private void SendingToQue(string data)
        {
            //------Sending data to queue
            bool done = true;
            do
            {
                if (!MainMenuForm.dataClass.Busy)
                {
                    MainMenuForm.dataClass.Busy = true;
                    MainMenuForm.dataClass.QueueAdd = data;
                    MainMenuForm.dataClass.Take = true;
                    done = false;
                }

            } while (done);

        }
        #endregion

        #endregion

        //ended
        #region Update
        private void timerUpdate_Tick(object sender, EventArgs e)
        {
            #region SerialPort
            if (movementWayPath.StopSP)
            {

                Stop();
                movementWayPath.StopSP = false;
                movementWayPath.Run = true;
                Start();
            }

            if (dataClass.ErrorStop)
            {
                dataClass.ErrorStop = false;
                Stop();
            }

            #endregion

            #region Update Conections info
            if (dataClass.Connect)
            {
                if (dataClass.Stop && dataClass.StopDon)
                {
                    StopBtn();

                }
                else if (!dataClass.Stop && dataClass.StopDon)
                {
                    StopBtn();

                }

                if (dataClass.ThreadFileProblem)
                {
                    MessageBox.Show("File doesn't exist or sth else!", "File problem", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (dataClass.GetConInfo)
                {
                    label4.Text = "Conection Status: Conected";
                    if (dataClass.NoPower)
                    {
                        label5.Text = "Power: No AC Voltage aplied";
                    }
                    else
                    {
                        label5.Text = "Power: AC Voltage aplied";
                    }
                }
                else
                    label4.Text = "Conection Status: Disconected";          


                label3.Text = dataClass.StopC.ToString();
                label6.Text = dataClass.StopD.ToString();
                label7.Text = dataClass.StopB.ToString();
                label8.Text = dataClass.StopE.ToString();
            }
            else
                label4.Text = "Conection Status: Disconected";

            #endregion
            if(errorListNumerous!=null)
            if (errorListNumerous.Count > errorListindex && errorListNumerous.Count != 0)
            {
                var a = ErrorExplenationRobot(errorListNumerous[(errorListNumerous.Count-1)],false);
                listBoxErrorList.Items.Add(a);
                errorListindex++;
            }


            #region Edit Point or Add in specific place
            if (MainMenuForm.movementWayPath.EditPoint)
            {
                MainMenuForm.movementWayPath.EditPoint = false;
                MainMenuForm.movementWayPath.EditingPoint = true;

                ActivateBtn(iconButtonControl, RGBcolors.color1);
                OpenChildForm(new ControlForm());

            }
            #endregion      

            #region Add Next Move
            if (MainMenuForm.movementWayPath.Add)
            {
                MainMenuForm.movementWayPath.Add = false;

                if (MainMenuForm.movementWayPath.EditingPoint)
                {
                    MainMenuForm.movementWayPath.EditingPoint = false;

                    AddNextMove(MainMenuForm.movementWayPath.IndexLb, MainMenuForm.movementWayPath.Mode);

                    ActivateBtn(iconButtonPaths, RGBcolors.color4);
                    OpenChildForm(new PathsForm());

                }
                else
                {
                    AddNextMove(0, MainMenuForm.movementWayPath.Mode);
                }

            }

            #endregion

            #region Remuve Last Move

            if (MainMenuForm.movementWayPath.Remove)
            {
                MainMenuForm.movementWayPath.Remove = false;

                if (!MainMenuForm.movementWayPath.EditingPoint)
                {
                    RemoveLastPoint();
                }
                else
                {
                    MainMenuForm.movementWayPath.EditingPoint = false;
                    ActivateBtn(iconButtonPaths, RGBcolors.color4);
                    OpenChildForm(new PathsForm());
                }
            }


            #endregion

        }

        public static string AddErrorToList(ErrorBytes eb, bool ext)
        {
            string add="";
            if (ext)
                add += "Data [h:m:s:ms]: " + eb.Time + "  =>  " ;

            if (eb.Code >= 0 && eb.Code <= 5)
            {
                if (ext)
                    add += "  I2C error/";

                add += "code: " + eb.Code.ToString() + "/";
                if (eb.Value == 1)
                    add += "Multiplexer";
                else if (eb.Value == 2)
                    add += "Seting Pointer";
                else if (eb.Value == 3)
                    add += "Error reding angle";
            }
            else if (eb.Code == 7)
                add += "Data/Unsigned comand: " + eb.Value.ToString();
            else if (eb.Code == 10)
            {
                if (eb.Value == 1)
                    add += "Connected with: " + MainMenuForm.dataClass.Ip;
                else if (eb.Value == 2)
                    add += "Disconnected from: " + MainMenuForm.dataClass.Ip;
            }

            return add;
        }

        #endregion

        //ended
        #region File 
        private void AddNextMove(int index, int mode)
        {
            // mode 1 dla dodania na końcu
            // mode 2 dla podmiany
            // mode 3 dla dodania przed podany indeksem
            // mode 4 dla dodania po podanym indeksie
            // mode (negatywny) restart pozycji dla wybranego modu

            #region Veriables

            Cordinants cordMain = new Cordinants();

            int acceRet = 0, deaccelRet = 0;
            int[] speed = { 1200, 1200, 1200, 1200, 1200, 1200 };
            int[] speedRet = new int[6];
            double[] joRet = new double[6];
            double[] jo = { 0, 180, 270, 0, 0, 0 };
            bool manualRet = false;

            string begin = "G0ǿX";
            string lastRXY = null;

            string end = "G4";

            FileFun ff = new FileFun();

            bool condition = true;
            int modeAbs = mode;
            modeAbs = Math.Abs(modeAbs);
            string readed = null, change = null;

            // path of the origin file
            string pathMane = MainMenuForm.movementWayPath.Path;

            #endregion

            #region Mode setup
            if (mode >= 0)
            {
                cordMain.X = MainMenuForm.dataClass.Cor[0];
                cordMain.Y = MainMenuForm.dataClass.Cor[1];
                cordMain.Z = MainMenuForm.dataClass.Cor[2];
                cordMain.RX = MainMenuForm.dataClass.Jo[3];
                cordMain.RY = MainMenuForm.dataClass.Jo[4];
                cordMain.RZ = MainMenuForm.dataClass.Jo[5];

                begin += cordMain.X.ToString() + "ǿY" + cordMain.Y.ToString() + "ǿZ" + cordMain.Z.ToString();
                begin += "ǿRX" + cordMain.RX.ToString() + "ǿRY" + cordMain.RY.ToString() + "ǿRZ" + cordMain.RZ.ToString();

                string type = null,additives=null;

                if (MainMenuForm.dataClass.ManualOrAuto)
                {
                    type = "0";
                }
                else
                {
                    type = "1";
                    additives = MainMenuForm.dataClass.MovementType.ToString();
                }

                begin += "ǿG7" + type + "ǿG8" + additives;
            }
            else
            {


                acceRet = dataClass.Acceleration;
                deaccelRet = dataClass.Deacceleration;
                joRet = dataClass.Jo;
                speedRet = dataClass.SpeedManula;
                manualRet = dataClass.ManualOrAuto;


                dataClass.Jo = jo;
                dataClass.SpeedManula = speed;
                dataClass.ManualOrAuto = true;
                dataClass.Deacceleration = 100;
                dataClass.Acceleration = 100;


                begin += 250.ToString() + "ǿY" + 0.ToString() + "ǿZ" + 260.ToString();
                begin += "ǿRX" + 0.ToString() + "ǿRY" + 0.ToString() + "ǿRZ" + 0.ToString();
                begin += "ǿG5";
            }
            #endregion

            try
            {
                //check if file exist
                if (System.IO.File.Exists(pathMane))
                {

                    //Create temp file for our modifications
                    string pathTemp = ff.CreateTempFile("TempAdd", Application.ExecutablePath, Application.ProductName);

                    int line = 0;

                    using (StreamWriter writer = new StreamWriter(pathTemp))
                    {
                        using (StreamReader reader = new StreamReader(pathMane))
                        {
                            #region Reading and writing points before modifiaction
                            if (modeAbs == 2 || modeAbs == 3)
                            {
                                while (reader.Peek() != -1 && line != index)
                                {
                                    readed = reader.ReadLine();
                                    writer.WriteLine(readed);

                                    if (readed.Substring(0, 2) == "G0") lastRXY = readed;

                                    line++;
                                }
                            }
                            else if (modeAbs == 4)
                            {
                                while (reader.Peek() != -1 && line != index)
                                {
                                    readed = reader.ReadLine();
                                    writer.WriteLine(readed);

                                    if (readed.Substring(0, 2) == "G0") lastRXY = readed;

                                    line++;
                                }


                                while (reader.Peek() != -1)
                                {

                                    readed = reader.ReadLine();

                                    if (readed.Substring(0, 2) == "G0") lastRXY = readed;


                                    if (readed.Substring(0, 2) == "G4")
                                    {
                                        writer.WriteLine(readed);
                                        line++;
                                        break;
                                    }


                                    line++;
                                    writer.WriteLine(readed);
                                }

                            }
                            else if (modeAbs == 1)
                            {
                                while (reader.Peek() != -1)
                                {
                                    readed = reader.ReadLine();
                                    writer.WriteLine(readed);
                                    if (readed.Substring(0, 2) == "G0") lastRXY = readed;

                                    line++;
                                }
                            }

                            #endregion

                            #region Adding new points


                            //bigining of the pos
                            writer.WriteLine(begin);

                            //veriables
                            string setVelocity = null;
                            string compareVel = "";
                            string dataMove;
                            bool veloAdd = true;
                            bool lastPointCorrection = false;

                            #region setup for the new pos acording to the latest

                            if (lastRXY != null)
                            {
                                if (lastRXY.Substring(0, 2) == "G0")
                                {
                                    string[] add = lastRXY.Split('ǿ');
                                    if (add.Length >= 7)
                                    {
                                        cordMain.XL = Convert.ToDouble(add[1].Substring(1));
                                        cordMain.YL = Convert.ToDouble(add[2].Substring(1));
                                        cordMain.ZL = Convert.ToDouble(add[3].Substring(1));
                                        cordMain.XB = cordMain.XL;
                                        cordMain.YB = cordMain.YL;
                                        cordMain.ZB = cordMain.ZL;
                                        lastPointCorrection = true;
                                    }
                                }
                            }

                            #endregion

                            #region New points
                            while (condition)
                            {
                                if (dataClass.ManualOrAuto)
                                {
                                    dataMove = SetAnglesManual();
                                    setVelocity = SetSpeed("G1ǿ", "@");
                                    veloAdd = true;
                                    condition = false;
                                }
                                else
                                {
                                    cordMain.X = MainMenuForm.dataClass.Cor[0];
                                    cordMain.Y = MainMenuForm.dataClass.Cor[1];
                                    cordMain.Z = MainMenuForm.dataClass.Cor[2];
                                    cordMain.RX = MainMenuForm.dataClass.Jo[3];
                                    cordMain.RY = MainMenuForm.dataClass.Jo[4];
                                    cordMain.RZ = MainMenuForm.dataClass.Jo[5];

                                    cordMain = MathAllinOne(cordMain, lastPointCorrection);
                                    lastPointCorrection = false;

                                    string dm = setVelocity;
                                    dataMove = TransformToCodeFile(cordMain.AngleLast, "G2ǿ", "@");                              
                                    setVelocity = TransformToCodeFile(cordMain.Velocity, "G1ǿ", "@");

                                    veloAdd = true;

                                    if (dm == setVelocity) veloAdd = false;

                                    if (cordMain.XL == cordMain.X && cordMain.YL == cordMain.Y && cordMain.ZL == cordMain.Z)
                                    {
                                        cordMain.XB = cordMain.X; cordMain.YB = cordMain.Y; cordMain.ZB = cordMain.Z;
                                        condition = false;
                                    }                             
                                }

                                if(veloAdd) writer.WriteLine(setVelocity);

                                writer.WriteLine(dataMove);

                            }
                            #endregion

                            //end of the pos
                            writer.WriteLine(end);

                            #endregion

                            #region Writing rest of the points
                            if (modeAbs == 2)
                            {
                                readed = "";
                                bool read = false;

                                while (reader.Peek() != -1)
                                {
                                    readed = reader.ReadLine();

                                    if (read) { writer.WriteLine(readed); }
                                    else if (readed.Substring(0, 2) == "G4")
                                    {
                                        read = true;
                                    }

                                }
                            }
                            else if (modeAbs == 3 || modeAbs == 4)
                            {
                                while (reader.Peek() != -1)
                                {
                                    writer.WriteLine(reader.ReadLine());
                                }
                            }

                            #endregion

                            #region Fixing dataclass values          
                            if (mode < 0)
                            {
                                dataClass.Deacceleration = deaccelRet;
                                dataClass.Acceleration = acceRet;
                                dataClass.Jo = joRet;

                                dataClass.SpeedManula = speedRet;
                                dataClass.ManualOrAuto = manualRet;


                            }
                            #endregion

                            //seting mode to standard value
                            MainMenuForm.movementWayPath.Mode = 1;
                        }
                    }
                    // coping data from temp file to original
                    ff.CopyFromOneFileToAnother(pathTemp, pathMane);

                    //deleting temp file
                    //FileSystem               
                    FileInfo fileInfo = new FileInfo(pathTemp);
                    fileInfo.Delete();
                }
                else
                { MessageBox.Show("File isn't set or doesn't exist.", "Problew with a file", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            }
            catch (Exception)
            { MessageBox.Show("File writing problem.", "Problew with a file", MessageBoxButtons.OK, MessageBoxIcon.Information); }

        }

        private string TransformToCodeFile(int[] data, string beg, string end)
        {
            string dataret = null;
            dataret += beg;

            for (int i = 0; i < data.Length;)
            {
                dataret += data[i].ToString() + 'ǿ';
                i++;
            }

            dataret += end;
            return dataret;
        }

        private string SetSpeed(string beg, string end)
        {
            string data = beg;

            if (MainMenuForm.dataClass.ManualOrAuto)
            {
                for (int i = 0; i < 6;)
                {
                    data += MainMenuForm.dataClass.SpeedManula[i] / 360 * dataClass.GearReduction[i]; i++;
                }

                data += MainMenuForm.dataClass.Acceleration + 'ǿ';
                data += MainMenuForm.dataClass.Deacceleration + 'ǿ';
                data += end;
            }

            return data;
        }
        //ended
        private void RemoveLastPoint()
        {
            FileFun fileFun = new FileFun();

            try
            {
                if (System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
                {
                    string readed;
                    long beg = 0, end = 0, lines = 0;

                    var unsignedListBegins = new List<long> { };

                    string pathProgram = fileFun.CreateTempFile("TempRemove", Application.ExecutablePath, Application.ProductName);

                    if (pathProgram != null)
                    {

                        using (StreamReader reader = new StreamReader(MainMenuForm.movementWayPath.Path))
                        {
                            while (reader.Peek() != -1)
                            {
                                readed = reader.ReadLine();

                                if (readed.Length >= 2)
                                {
                                    if (readed.Substring(0, 2) == "G0")
                                    {
                                        unsignedListBegins.Add(lines);
                                        beg++;
                                    }

                                    if (readed.Substring(0, 2) == "G4") end++;
                                }

                                lines++;
                            }
                        }

                        if (unsignedListBegins.Count != 0 && end == beg)
                        {

                            using (StreamReader reader = new StreamReader(MainMenuForm.movementWayPath.Path))
                            {
                                long counter = 0;

                                using (StreamWriter writer = new StreamWriter(pathProgram))
                                {

                                    while (reader.Peek() != -1)
                                    {
                                        readed = reader.ReadLine();

                                        if (unsignedListBegins.Last() == counter)
                                        {
                                            writer.Close();
                                            reader.Close();
                                            break;
                                        }
                                        writer.WriteLine(readed);

                                        if (counter > lines) break;
                                        counter++;

                                    }
                                }
                            }
                            fileFun.CopyFromOneFileToAnother(pathProgram, MainMenuForm.movementWayPath.Path);
                        }
                        else
                        {
                            MessageBox.Show("Nothing left to delate \n or porblem with the beg/end of the file", "File Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        FileInfo fileInfo = new FileInfo(pathProgram);
                        fileInfo.Delete();
                    }
                }
                else
                {
                    MessageBox.Show("File isn't set or doesn't exist.", "Problew with a file", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("File Reading problem.", "Problew with a file", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        //ended  
        private Cordinants EncodeFromFileToSend(string readed, Cordinants cord)
        {
            string readedSub = readed.Substring(0, 2);
            cord.DataSF = null;
            int[] dataToCon;

            switch (readedSub)
            {
                case "G0":
                    string[] add = readed.Split('ǿ');
                    if (add.Length == 7 || add.Length == 8)
                    {
                        cord.X = Convert.ToDouble(add[1].Substring(1));
                        cord.Y = Convert.ToDouble(add[2].Substring(1));
                        cord.Z = Convert.ToDouble(add[3].Substring(1));
                        cord.RX = Convert.ToDouble(add[4].Substring(1));
                        cord.RY = Convert.ToDouble(add[5].Substring(1));
                        cord.RZ = Convert.ToDouble(add[6].Substring(1));
                    }
                    break;

                case "G1":
                    add = readed.Split('ǿ');                   
                    if (add.Length == 9)
                    {
                        dataToCon = new int[8];
                        for (int i = 0; i < 8; i++)
                        {
                            dataToCon[i] = Convert.ToInt32(Convert.ToInt32(add[i + 1].Substring(1)) * MainMenuForm.dataClass.VelScale);
                        }
                        cord.SetDataToSend(35,dataToCon);                    
                       // cord.DataSF = ConvertToMessage((byte)35, dataToCon);
                    }
                    break;

                case "G2":
                    add = readed.Split('ǿ');
                    if (add.Length == 7)
                    {
                        dataToCon = new int[6];
                        for (int i = 0; i < 6; i++)
                        {
                            dataToCon[i] = Convert.ToInt32(add[i + 1].Substring(1));
                        }
                        cord.SetDataToSend(34, dataToCon);
                        //cord.DataSF = ConvertToMessage((byte)34, dataToCon);
                    }
                    break;

                default:
                    break;
            }
        
            return cord;
        }

        public static ToolsClass ReadXMLFile()
        {
            string appPath = Application.ExecutablePath;
            appPath = appPath.Substring(0, appPath.Length - Application.ProductName.Length);
            appPath += "Csetup.xml";

            ToolsClass toolsClass = new ToolsClass();
            ToolC tc = new ToolC();

            double[] limits = new double[12];
            try
            {
                if (System.IO.File.Exists(appPath))
                {
                    using (FileStream fs = new FileStream(appPath, FileMode.Open))
                    {
                        using (XmlTextReader r = new XmlTextReader(fs))
                        {

                            int type = 0, underType = 0, counter = 0;
                            string name = null, value = null;
                            bool[] con = new bool[12];                      

                            while (r.Read())
                            {

                                try
                                {
                                    if (r.NodeType == XmlNodeType.Element) name = r.Name;
                                    //r.Read();
                                    if (r.NodeType == XmlNodeType.Text) value = r.Value;
                                }
                                catch (Exception) { }


                                switch (name)
                                {
                                    case "tools": type = 1; con = new bool[12]; break;
                                    case "tool": underType = 1; con = new bool[12]; break;
                                    case "limits": type = 2; con = new bool[12]; break;
                                    case "Conection": type = 3; con = new bool[12]; break;
                                    default:
                                        break;
                                }

                                switch (type)
                                {
                                    case 1:
                                        if (underType == 1)
                                        {
                                            bool beg = false;
                                            if (value != null)
                                            {
                                                if (name == "name") { tc.Name = value; con[0] = true; value = null; name = null; beg = true; }
                                                else if (name == "x") { tc.X = Convert.ToDouble(value); con[1] = true; value = null; name = null; beg = true; }
                                                else if (name == "y") { tc.Y = Convert.ToDouble(value); con[2] = true; value = null; name = null; beg = true; }
                                                else if (name == "z") { tc.Z = Convert.ToDouble(value); con[3] = true; value = null; name = null; beg = true; }

                                                if (beg)
                                                {
                                                    bool p = true;
                                                    for (int i = 0; i < 4;) { if (!con[i]) { p = false; break; } i++; }
                                                    if (p) { toolsClass.Tool.Add(tc); tc = new ToolC(); con = new bool[12]; name = null; value = null; underType = 0; }
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        for (int i = 0; i < 12;)
                                        {
                                            if (name != "limits" && value != null)
                                            {
                                                try
                                                {
                                                    if (name == "j" + i.ToString())
                                                    {
                                                        limits[i] = Convert.ToDouble(value);
                                                        con[i] = true;
                                                        bool upp = true;
                                                        for (int p = 0; p < 12;)
                                                        {
                                                            if (!con[p]) { upp = false; break; }
                                                            p++;
                                                        }
                                                        if (upp)
                                                        { dataClass.JoLimits = limits; type = 0; }
                                                        value = null;
                                                        name = null;

                                                    }
                                                }
                                                catch (Exception ex) { throw ex; }

                                            }
                                            else { break; }
                                            i++;
                                        }
                                        break;
                                    case 3:                      
                                        if (value != null)
                                        {               
                                                 if (name == "IP")   { MainMenuForm.dataClass.Ip = value;    value = null; name = null; }
                                            else if (name == "mask") { MainMenuForm.dataClass.Mask = value;  value = null; name = null; }
                                            else if (name == "gate") { MainMenuForm.dataClass.Gate = value;  value = null; name = null; }
                                            else if (name == "port") { MainMenuForm.dataClass.Port = value;  value = null; name = null; }
                                            else if (name == "ssid") { MainMenuForm.dataClass.Ssid = value;  value = null; name = null; }
                                            else if (name == "pass") { MainMenuForm.dataClass.Pass = value;  value = null; name = null; }
                                        }
                                        break;

                                    default: break;
                                }

                            }
                        }
                    }
                    bool end = true;
                }
                else
                {
                    DialogResult dr = MessageBox.Show("Cannot find setup file. \n Program requires the setup file with the name: 'SDRACsetup.xml', in the program directory. \n Do you want to create new setup file?  ", "File problem", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (dr == DialogResult.OK)
                    {
                        CreateXmlFile();
                        ReadXMLFile();
                    }

                }
            }
            catch (Exception) { }

            return toolsClass;
        }

        public static void CreateXmlFile()
        {
            string appPath = Application.ExecutablePath;
            appPath = appPath.Substring(0, appPath.Length - Application.ProductName.Length);
            appPath += "Csetup.xml";
            bool con=true;
            if (System.IO.File.Exists(appPath))
            {
                DialogResult dr = MessageBox.Show("Setup File already exist.\n Do you want to owerwrite it?", "File Problem", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (dr == DialogResult.Yes)
                {
                    FileInfo fileInfo = new FileInfo(appPath);
                    fileInfo.Delete();
                }
                else { con = false; }
            }
            if (con)
            {
                FileStream fs = new FileStream(appPath, FileMode.Create);

                using (XmlWriter xw = XmlWriter.Create(fs))
                {
                    xw.WriteStartDocument(false);
                    // xw.WriteDocType("bookstore", null, "books.dtd", null);
                    // xw.WriteComment("This file represents another fragment of a book store inventory database");
                    xw.WriteStartElement("setup");
                    xw.WriteStartElement("tools", null);
                    xw.WriteStartElement("tool", null);
                    xw.WriteElementString("name", "Default");
                    xw.WriteElementString("x", "20");
                    xw.WriteElementString("y", "0");
                    xw.WriteElementString("z", "0");
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    xw.WriteStartElement("limits", null);
                    xw.WriteElementString("j0", "360");
                    xw.WriteElementString("j1", "0");
                    xw.WriteElementString("j2", "360");
                    xw.WriteElementString("j3", "0");
                    xw.WriteElementString("j4", "360");
                    xw.WriteElementString("j5", "0");
                    xw.WriteElementString("j6", "180");
                    xw.WriteElementString("j7", "-180");
                    xw.WriteElementString("j8", "180");
                    xw.WriteElementString("j9", "-180");
                    xw.WriteElementString("j10", "180");
                    xw.WriteElementString("j11", "-180");
                    xw.WriteEndElement();
                    xw.WriteStartElement("Conection", null);
                    xw.WriteElementString("IP", "192.168.0.1");
                    xw.WriteElementString("mask", "255.255.255.1");
                    xw.WriteElementString("gate", "192.168.1.1");
                    xw.WriteElementString("port", "5005");
                    xw.WriteElementString("ssid", "");
                    xw.WriteElementString("pass", "");
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                }
                fs.Close();
            }
        }

        public static void SaveXmlFile()
        {
            string appPath = Application.ExecutablePath;
            appPath = appPath.Substring(0, appPath.Length - Application.ProductName.Length);
            appPath += "Csetup.xml";
            try
            {
                FileStream fs = new FileStream(appPath, FileMode.Create);

                using (XmlWriter xw = XmlWriter.Create(fs))
                {

                    xw.WriteStartDocument(false);
                    // xw.WriteDocType("bookstore", null, "books.dtd", null);
                    // xw.WriteComment("This file represents another fragment of a book store inventory database");
                    xw.WriteStartElement("setup");

                    xw.WriteStartElement("tools", null);

                    if (!Tools.Tool.Exists(j => j.Name == "Default"))
                    { Tools.Tool.Add(new ToolC { Name = "Default", X = 20, Y = 0, Z = 0, Id = 0 }); }

                    foreach (var item in Tools.Tool)
                    {
                        xw.WriteStartElement("tool", null);
                        xw.WriteElementString("name", item.Name);
                        xw.WriteElementString("x", item.X.ToString());
                        xw.WriteElementString("y", item.Y.ToString());
                        xw.WriteElementString("z", item.Z.ToString());
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("limits", null);
                    int i = 0;
                    foreach (double item in MainMenuForm.dataClass.JoLimits)
                    {
                        xw.WriteElementString("j" + i.ToString(), item.ToString());
                        i++;
                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("Conection", null);
                    xw.WriteElementString("IP", MainMenuForm.dataClass.Ip);
                    xw.WriteElementString("mask", MainMenuForm.dataClass.Mask);
                    xw.WriteElementString("gate", MainMenuForm.dataClass.Gate);
                    xw.WriteElementString("port", MainMenuForm.dataClass.Port);
                    xw.WriteElementString("ssid", MainMenuForm.dataClass.Ssid);
                    xw.WriteElementString("pass", MainMenuForm.dataClass.Pass);
                    xw.WriteEndElement();
                    xw.WriteEndElement();
                    MessageBox.Show("Saved Setup","File",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }
                fs.Close();
            }
            catch (Exception ex) { MessageBox.Show("Couldn't save setup!", "File problem", MessageBoxButtons.OK, MessageBoxIcon.Error); }
         
           


        }


        #endregion

        //ended
        #region Closing App
        private void MainMenuForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (readSP != null)
            {
                if (readSP.IsAlive)
                {
                    dataClass.Threads = false;
                    readSP.Join();
                }
            }

            if (prepare != null)
            {
                if (prepare.IsAlive)
                {
                    dataClass.Threads = false;
                    prepare.Join();
                }
            }

            if (serialPort.IsOpen) serialPort.Close();

            Application.Exit();
        }

        #endregion

        //ended
        #region Buttons Animations

        //Button Colors
        public struct RGBcolors
        {
            public static Color color1 = Color.FromArgb(150, 1, 2);
            public static Color color2 = Color.FromArgb(0, 135, 29);
            public static Color color3 = Color.FromArgb(139, 58, 210);
            public static Color color4 = Color.FromArgb(254, 88, 0);
            public static Color color5 = Color.FromArgb(0, 114, 250);
            public static Color color6 = Color.FromArgb(255, 0, 0);
            public static Color color7 = Color.FromArgb(0, 153, 0);
        }

        private void ActivateBtn(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {


                DeActivateBtn();
                //button color
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(50, 50, 50);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                currentBtn.IconSize = 40;

                //Stripe
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Height = currentBtn.Height;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();

                //Icon
                iconPictureBoxLogoIcon.IconChar = currentBtn.IconChar;
                iconPictureBoxLogoIcon.IconColor = color;

                //Title text
                labelTitle.Text = currentBtn.Text;
                labelTitle.ForeColor = color;
            }
        }

        private void DeActivateBtn()
        {
            if (currentBtn != null)
            {
                currentBtn.BackColor = Color.FromArgb(35, 35, 35);
                currentBtn.ForeColor = Color.DarkGray;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.DarkGray;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconSize = 32;


            }
        }


        #endregion

        //ended
        #region Child Form 

        private void OpenChildForm(Form childForm)
        {
            if (currentChileForm != null)
            {

                currentChileForm.Close();
            }
            currentChileForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();

        }

        #endregion

        //ended
        #region Buttons
        private void iconButtonControl_Click(object sender, EventArgs e)
        {
                     
            MainMenuForm.movementWayPath.EditingPoint = false;
            ActivateBtn(sender,RGBcolors.color1);
            OpenChildForm(new ControlForm());
            

        }

        private void iconButtonParameters_Click(object sender, EventArgs e)
        {
            MainMenuForm.movementWayPath.EditingPoint = false;
            ActivateBtn(sender, RGBcolors.color2);
            OpenChildForm(new ParametersForm());
        }

        private void iconButtonSetings_Click(object sender, EventArgs e)
        {

            if (MainMenuForm.dataClass.Threads)
            {
                DialogResult result = MessageBox.Show("First you have to disconect \n Do you want to disconnect?", "Conection Problem", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    StopThread();
                    dataClass.Connect = false;
                    iconButtonConnect.Text = "Connect";
                    iconButtonConnect.BackColor = Color.FromArgb(35, 35, 35);
                    iconButtonConnect.ForeColor = RGBcolors.color7;
                    iconButtonConnect.IconColor = RGBcolors.color7;
                    MainMenuForm.movementWayPath.EditingPoint = false;
                    ActivateBtn(sender, RGBcolors.color3);
                    OpenChildForm(new SetingsForm());

                }
            }
            else
            {
                MainMenuForm.movementWayPath.EditingPoint = false;
                ActivateBtn(sender, RGBcolors.color3);
                OpenChildForm(new SetingsForm());
            }

            

            
            

        }

        private void iconButtonPaths_Click(object sender, EventArgs e)
        {
            ActivateBtn(sender, RGBcolors.color4);
            OpenChildForm(new PathsForm());
        }

        private void iconButtonHelp_Click(object sender, EventArgs e)
        {
            MainMenuForm.movementWayPath.EditingPoint = false;
            ActivateBtn(sender, RGBcolors.color5);
            OpenChildForm(new HelpForm());
        } 

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MainMenuForm.movementWayPath.EditingPoint = false;
            Reset();
                
        }

        private void Reset()
        {
            DeActivateBtn();
            leftBorderBtn.Visible = false;
            iconPictureBoxLogoIcon.IconChar = FontAwesome.Sharp.IconChar.Home;
            iconPictureBoxLogoIcon.IconColor = Color.DarkGray;
            labelTitle.Text = "  Home";
            labelTitle.ForeColor = Color.DarkGray;
            if (currentChileForm!=null) currentChileForm.Close();
                      
        }

        private void iconButtonConnect_Click(object sender, EventArgs e)
        {

           // if (dataClass.Portname != null)
           // {
                dataClass.Connect = !dataClass.Connect;
                if (dataClass.Connect)
                {                 
                    Start();
                }
                else
                {
                    Stop();
                }
            //}
        }

        private void timerSendOnline_Tick(object sender, EventArgs e)
        {
            dataClass.Timer = true;

        
            if (dataClass.Portname != null && !dataClass.Connect)
            {

                iconButtonConnect.Enabled = true;
                iconButtonConnect.ForeColor = RGBcolors.color7;
                iconButtonConnect.IconColor = RGBcolors.color7;
            }
            else if (dataClass.Portname == null)
            {
                iconButtonConnect.IconColor = Color.DarkGray;
                iconButtonConnect.ForeColor = Color.DarkGray;
                iconButtonConnect.Enabled = false;

            }

        }

        #endregion

        //ended
        #region BTN

        private void StopBtn()
        {
            
            if (MainMenuForm.dataClass.Stop)
            {
                iconButtonStop.BackColor = Color.FromArgb(50, 50, 50);
                iconButtonStop.ForeColor = RGBcolors.color6;
                iconButtonStop.TextAlign = ContentAlignment.MiddleCenter;
                iconButtonStop.IconColor = RGBcolors.color6;
                iconButtonStop.TextImageRelation = TextImageRelation.TextBeforeImage;
                iconButtonStop.ImageAlign = ContentAlignment.MiddleRight;
                iconButtonStop.IconSize = 40;

                //Stripe
                panelStop.Size = new Size(7, 62);
                panelStop.Height = iconButtonStop.Height;
                panelStop.BackColor = RGBcolors.color6;
                panelStop.Location = new Point(0, iconButtonStop.Location.Y);
                panelStop.Visible = true;
                panelStop.BringToFront();          
                //Timer
                timerStopAni.Enabled = true;
                anim = 0;             

            }
            else
            {
                iconButtonStop.BackColor = Color.FromArgb(35, 35, 35);
                iconButtonStop.ForeColor = Color.DarkGray;
                iconButtonStop.TextAlign = ContentAlignment.MiddleLeft;
                iconButtonStop.IconColor = Color.DarkGray;
                iconButtonStop.TextImageRelation = TextImageRelation.ImageBeforeText;
                iconButtonStop.ImageAlign = ContentAlignment.MiddleLeft;
                iconButtonStop.IconSize = 32;
                panelStop.Visible = false;
                timerStopAni.Enabled = false;               

            }
            dataClass.StopDon = false;
        }    
      
        private void iconButtonStop_Click(object sender, EventArgs e)
        {
            if (dataClass.GetConInfo)
            {
                MainMenuForm.dataClass.StopDon = true;

                bool stop = MainMenuForm.dataClass.Stop;
                MainMenuForm.dataClass.Stop = !stop;

                MainMenuForm.dataClass.StopSend = false;
            }
        }

        private void buttonClearEL_Click(object sender, EventArgs e)
        {
            listBoxErrorList.Items.Clear();
        }

        private void timerStopAni_Tick(object sender, EventArgs e)
        {       

            int j=0;
            if ( anim >=255)
            {
                j = anim-255;
                anim-=1;
            }
            else if (anim<255 && anim>1 )
            {
                
                j =255 - anim;
                anim-=1;
            }
            else
            {
                anim = 510;
            }

    
                iconButtonStop.ForeColor = Color.FromArgb(j, 0, 0);
                panelStop.BackColor = Color.FromArgb(j, 0, 0);
                iconButtonStop.IconColor = Color.FromArgb(j, 0, 0);


        }
        #endregion

        private int StartSimpleLan()
        {
            string adip = IPAddress.Any.ToString();
            errorListNumerous = new List<Classes.SimpleLan.ErrorClass>();
            simpleLan = new Classes.SimpleLan();
            //simpleLan.EasySetup(128,150,25000,MainMenuForm.dataClass.Ip);

            Classes.SimpleLan.SetupClient sl = new Classes.SimpleLan.SetupClient();
            sl._defaultPort = 25000;
            sl._hostIp4 = MainMenuForm.dataClass.Ip;
            sl._msgMaxSize = 64;
            sl._oneMessageTimeout = 50;

            simpleLan.EasySetup(sl);

            simpleLan.EventClassReturnHandler += SimpleLan_EventClassReturnHandler;

            Classes.SimpleLan.SetupClient sc = new Classes.SimpleLan.SetupClient();

            simpleLan.NewDataIncomeEvent += SimpleLan_NewDataIncomeEvent;
            simpleLan.ErrorIncomingEvent += SimpleLan_ErrorIncomingEvent;
            simpleLan.ConnectionStatusChangedEvent += SimpleLan_ConnectionStatusChangedEvent;
            return simpleLan.Start();

        }

        private void SimpleLan_EventClassReturnHandler(object sender, Classes.SimpleLan.EventClassReturn e)
        {
            if (e._command != null) QueueInUpgraded(e._command);
            else if (e._errorClass != null) errorListNumerous.Add(e._errorClass);
            else if (e._connection) MainMenuForm.dataClass.GetConInfo = e._connected;
        }

        private void SimpleLan_ConnectionStatusChangedEvent(object sender, bool e)
        {
            MainMenuForm.dataClass.GetConInfo = e;
        }

        private void SimpleLan_ErrorIncomingEvent(object sender, Classes.SimpleLan.ErrorClass e)
        {
            errorListNumerous.Add(e);
        }

        private void SimpleLan_NewDataIncomeEvent(object sender, Classes.SimpleLan.Command cm)
        {
            QueueInUpgraded(cm);
        }

        private void MainUpgraded()
        {
            #region Veriables
            //AngelsRet angles = new AngelsRet();
            Cordinants cord = new Cordinants();
            bool counted = false, startCount = false;
            bool forceCount;
            bool fileOpened = false;
            int indexCur = 0;
            string readed;
            int compare = 5;
            string veloAutoCounted = null;
            string angleCounted = null;
            MainMenuForm.dataClass.UpdateSetup = true;
            MainMenuForm.dataClass.TurnOffRobot = false;
            #endregion

            #region FromFile
            StreamReader reader = null;
            if (MainMenuForm.movementWayPath.RunFrom)
            {
                if (System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
                {
                    FileFun dd = new FileFun();
                    string pathProgram = dd.CreateTempFile("TempPerform", Application.ExecutablePath, Application.ProductName);
                    dd.CopyFromOneFileToAnother(MainMenuForm.movementWayPath.Path,pathProgram);

                    reader = new StreamReader(pathProgram);
                    fileOpened = true;
                    counted = false;

                    //If not from the beginning
                    if (MainMenuForm.movementWayPath.SkipLines && fileOpened)
                    {
                        int index = MainMenuForm.movementWayPath.IndexLb;
                        string readedin;

                        while (reader.Peek() != -1 && indexCur != index)
                        {
                            readedin = reader.ReadLine();
                            indexCur++;
                        }

                    }
                }
                else
                {
                    MainMenuForm.dataClass.ThreadFileProblem = true;
                    MainMenuForm.movementWayPath.Run = false;
                    //StopAllinOne();
                }
            }

            #endregion

            if (StartSimpleLan() >= 0)
            {
                try
                {
                    while (MainMenuForm.dataClass.Threads)
                    {
                        #region Math
                        if (MainMenuForm.dataClass.UploadMath)
                        {
                            #region Math
                          
                            if (MainMenuForm.dataClass.ManualOrAuto)
                            {
                                simpleLan.SendNewCommand(0,SetAnglesManualCom(),false);
                                simpleLan.SendNewCommand(0, SetSpeedManual(), false);
                                MainMenuForm.dataClass.UploadMath = false;
                            }
                            else
                            {
                                forceCount = MainMenuForm.dataClass.ForceCount;
                                if (forceCount)
                                {
                                    cord.X = MainMenuForm.dataClass.Cor[0];
                                    cord.Y = MainMenuForm.dataClass.Cor[1];
                                    cord.Z = MainMenuForm.dataClass.Cor[2];
                                    cord.RX = MainMenuForm.dataClass.Jo[3];
                                    cord.RY = MainMenuForm.dataClass.Jo[4];
                                    cord.RZ = MainMenuForm.dataClass.Jo[5];
                                }


                                #region Check whether next run is required
                                if (cord.XL == cord.X && cord.YL == cord.Y && cord.ZL == cord.Z)
                                {
                                    cord.XB = cord.X; cord.YB = cord.Y; cord.ZB = cord.Z;
                                    dataClass.UploadMath = false;
                                }
                                #endregion

                                if (CheckIfachived(MainMenuForm.dataClass.JoIN, cord.AngleLast, compare) && counted && !forceCount || startCount)
                                {
                                    startCount = false;
                                    simpleLan.SendNewCommand(0, cord.cm2, false);
                                    simpleLan.SendNewCommand(0, cord.cm1, false);                           
                                    counted = false;
                                }
                                else if (!counted || forceCount)
                                {
                                    cord = MathAllinOne(cord, false);
                                    counted = true;

                                    if (forceCount) startCount = true;
                                    MainMenuForm.dataClass.ForceCount = false;
                                }

                            }
                            #endregion
                        }
                        else if (MainMenuForm.movementWayPath.Run)
                        {                       
                            //open Streamreader to read file                 
                            try
                            {
                                if (CheckIfachived(MainMenuForm.dataClass.JoIN, cord.AngleLast, compare) && counted)
                                {
                                    simpleLan.SendNewCommand(0, cord.CodeData,false,2,cord.DataToSend, false);
                                    counted = false;
                                }
                                else if (!counted)
                                {
                                    readed = reader.ReadLine();

                                    if (readed.Length >= 2)
                                    {
                                        if (readed != "G4")
                                        {
                                            cord = EncodeFromFileToSend(readed, cord);

                                            counted = true;
                                        }
                                    }
                                }                               
                            }
                            catch (Exception) { }
                        }
                        #endregion

                        #region Seting
                        if (MainMenuForm.dataClass.UpdateSetup)
                        {
                            MainMenuForm.dataClass.UpdateSetup = false;

                            int[] dataLimits = new int[12];
                            for (int h = 0; h < 12; h++)
                            {
                                double limitDa = MainMenuForm.dataClass.JoLimits[h];
                                if (h <= 5)
                                    limitDa = limitDa * 11.375;    // 0-360
                                else
                                    limitDa = (limitDa * 11.375) + 2047.5;         // (-180)-180
                                dataLimits[h] = Convert.ToInt32(limitDa);
                            }

                            simpleLan.SendNewCommand(0, 40,false,2,dataLimits, false);
                         
                        }

                        if (!MainMenuForm.dataClass.StopSend)
                        {
                            MainMenuForm.dataClass.StopSend = true;
                            int[] datS = new int[1];
                            if (MainMenuForm.dataClass.Stop) datS[0]=61;
                            else datS[0] = 69;
                            simpleLan.SendNewCommand(0, 32, false, 1, datS, true);
                        }

                        if (MainMenuForm.dataClass.TurnOffRobot)
                        {
                            MainMenuForm.dataClass.TurnOffRobot = false;                        
                            simpleLan.SendNewCommand(0, 60,false, false);
                        }
                        #endregion       
                    }
                    if (reader != null) reader.Close();
                }
                catch (Exception) {MainMenuForm.dataClass.ErrorStop = true;}
            }
            else MessageBox.Show("ErrorSatrtingConnection");
            simpleLan.Stop();
        }

        public static string ErrorExplenationRobot(Classes.SimpleLan.ErrorClass eb, bool extendedInfo)
        {
            string add = "";
            if (extendedInfo)
                add += "[h:m:s:ms]: " + eb.time + "  =>  ";

            if (eb.command.code == 30)
            {
                int value = eb.command.dataOnly[1], code = eb.command.dataOnly[0];

                if (code >= 0 && code <= 5)
                {
                    if (extendedInfo)
                        add += "  I2C error/";

                    add += "code: " + code.ToString() + "/";
                    if (value == 1) add += "Multiplexer";
                    else if (value == 2) add += "Seting Pointer";
                    else if (value == 3) add += "Error reding angle";

                }
                else if (code == 7) add += "Data/Unsigned comand: " + value.ToString();
                else if (code == 10)
                {
                    if (value == 1) add += "Connected with: " + MainMenuForm.dataClass.Ip;
                    else if (value == 2) add += "Disconnected from: " + MainMenuForm.dataClass.Ip;
                }
            }
            else
            {
                
                add += "Er:" + eb.id;
                add += "/";
                if (eb.shortMsg!= null) add += eb.shortMsg;
                if (extendedInfo)
                {
                    if (eb.longMsg != null) add += '/' + eb.longMsg;
                    if (eb.command != null)
                    {
                        add += "/Msg_ID:" + eb.command.id.ToString();
                        add += "/Code:" + eb.command.code.ToString();
                        add += "/size:" + eb.command.size.ToString();
                        add += "/full_size:" + eb.command.fullSize.ToString();
                        if (!eb.command.rm) add += "/type:Message";
                        else add += "/type:Response";

                    }
                }
            }

            return add;
        
        }
    }
    public class ErrorBytes
    {
        private byte code, value;
        private string time = "";
          
        public byte Code { get => code; set => code = value; }
        public byte Value { get => value; set => this.value = value; }
        public string Time { get => time; set => time = value; }

        public void SetTime()
        {
            Time = DateTime.Now.Hour.ToString() + ':' + DateTime.Now.Minute.ToString() + ':' + DateTime.Now.Second.ToString() + ":" + DateTime.Now.Millisecond.ToString();
        }
    }
}
