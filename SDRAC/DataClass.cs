using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SDRAC
{
    public class DataClass
    {
        private string Test, path, portname, fileName, queueAdd, dataIn;
        private bool absolute = true, hold = false, alternative = false, relative = false, upload = false, closeOpenThreads = false, startedThread = false,
        stand = false, uploadAble = false, threads = false, threadMath = false, timer, stop = false, stopSend = false,
        ack = false, notack = false, connect = false, stopDon = true, busy = false, take = false, noPower = false, stopSended = false,
        aM = false, manualSend = true, forceCount = false, getConInfo = false, engError = false, errorStop = false, threadFileProblem = false, connectionAcomplished = false,
        updateSetup = true, turnOffRobot=false;

        private byte conectionType = 2;
        private string ip, mask, gate, port="25000", ssid, pass;
        private int acceleration=8000, deacceleration = 8000, speed=100, maxAc, maxDe, a0=10,a1=250,a2,a3=20,rX,rY,rZ, frameType = 0, movementType,queLength=512, resolution=4096;
        private double a, maxRange=500, b, g,accuracy=0.0000000010000,jumpVector=10, veloAuto, velScale = 1, xL,yl,zL,rxL,ryL,rzL;
        private int missed=0, missedCur,modifi=0,stopC, stopB, stopD,stopE,curveShape=1;
        private byte[] errorNanoNum = new byte[20];
        private double[] jo = new double[6];
        private double[] cor = new double[3];
        private double[] corLast = new double[3];
        private int[] speedManual = {5000, 5000, 5000, 5000, 5000, 5000 };
        private int[] gearReduction = {50000, 50000,50000, 40000, 40000, 40000 };
        private int[] speedAuto = new int[8];
        private int[] speedSend = new int[6];
        private double[] joLimits = { 360,0, 360,0, 360,0, 180, -180, 180, -180, 180, -180 };
        private int[] joIN = new int[6];      
        private int[] joLast = new int[3];
        private string[] queueIN = new string[64];
        private string[] queueOut = new string[64];
        private string trash;

        #region GETSET

        public string Path { get => path; set => path = value; }
        public string Portname { get => portname; set => portname = value; }
        public int Acceleration { get => acceleration; set => acceleration = value; }
        public int Deacceleration { get => deacceleration; set => deacceleration = value; }
        public int Speed { get => speed; set => speed = value; }
        public double[] Jo { get => jo; set => jo = value; }
        public double[] Cor { get => cor; set => cor = value; }
        public bool UploadAble { get => uploadAble; set => uploadAble = value; }
        public string[] QueueIn { get => queueIN; set => queueIN = value; }
        public string[] QueueOut { get => queueOut; set => queueOut = value; }
        public bool Threads { get => threads; set => threads = value; }
        public bool Timer { get => timer; set => timer = value; }
        public int Missed { get => missed; set => missed = value; }
        public int MissedCur { get => missedCur; set => missedCur = value; }
        public bool Stop { get => stop; set => stop = value; }
        public bool Ack { get => ack; set => ack = value; }
        public bool Notack { get => notack; set => notack = value; }
        public bool Connect { get => connect; set => connect = value; }
        public bool StopDon { get => stopDon; set => stopDon = value; }
        public int Modifi { get => modifi; set => modifi = value; }
        public int StopC { get => stopC; set => stopC = value; }
        public int StopB { get => stopB; set => stopB = value; }
        public int StopD { get => stopD; set => stopD = value; }
        public int StopE { get => stopE; set => stopE = value; }
        public bool Busy { get => busy; set => busy = value; }
        public string QueueAdd { get => queueAdd; set => queueAdd = value; }
        public bool Take { get => take; set => take = value; }
        public bool NoPower { get => noPower; set => noPower = value; }
        public bool StopSend { get => stopSend; set => stopSend = value; }
        public int[] SpeedManula { get => speedManual; set => speedManual = value; }
        public bool ManualOrAuto { get => aM; set => aM = value; }
        public int A0 { get => a0; set => a0 = value; }
        public int A1 { get => a1; set => a1 = value; }
        public int A2 { get => a2; set => a2 = value; }
        public int A3 { get => a3; set => a3 = value; }
        public int RX { get => rX; set => rX = value; }
        public int RY { get => rY; set => rY = value; }
        public int RZ { get => rZ; set => rZ = value; }
        public double Accuracy { get => accuracy; set => accuracy = value; }
        public int CurveShape { get => curveShape; set => curveShape = value; }
        public int MovementType { get => movementType; set => movementType = value; }
        public double[] CorLast { get => corLast; set => corLast = value; }
        public double JumpVector { get => jumpVector; set => jumpVector = value; }
        public bool UploadMath { get => upload; set => upload = value; }
        public int QueLength { get => queLength; set => queLength = value; }
        public int[] JoIN { get => joIN; set => joIN = value; }
        public string Trash { get => trash; set => trash = value; }
        public int[] SpeedAuto { get => speedAuto; set => speedAuto = value; }
        public bool ManualSend { get => manualSend; set => manualSend = value; }
        public bool ForceCount { get => forceCount; set => forceCount = value; }
        public bool GetConInfo { get => getConInfo; set => getConInfo = value; }
        public bool EngError { get => engError; set => engError = value; }
        public byte[] ErrorNanoNum { get => errorNanoNum; set => errorNanoNum = value; }
        public int Resolution { get => resolution; set => resolution = value; }
        public double VelScale { get => velScale; set => velScale = value; }
        public double MaxRange { get => maxRange; set => maxRange = value; }
        public int FrameType { get => frameType; set => frameType = value; }
        public int[] GearReduction { get => gearReduction; set => gearReduction = value; }
        public bool ErrorStop { get => errorStop; set => errorStop = value; }
        public bool ThreadFileProblem { get => threadFileProblem; set => threadFileProblem = value; }
        public double XL { get => xL; set => xL = value; }
        public double Yl { get => yl; set => yl = value; }
        public double ZL { get => zL; set => zL = value; }
        public double RxL { get => rxL; set => rxL = value; }
        public double RyL { get => ryL; set => ryL = value; }
        public double RzL { get => rzL; set => rzL = value; }
        public double[] JoLimits { get => joLimits; set => joLimits = value; }
        public string Ip { get => ip; set => ip = value; }
        public string Mask { get => mask; set => mask = value; }
        public string Gate { get => gate; set => gate = value; }
        public string Port { get => port; set => port = value; }
        public string Ssid { get => ssid; set => ssid = value; }
        public string Pass { get => pass; set => pass = value; }
        public byte ConectionType { get => conectionType; set => conectionType = value; }
        public bool ConnectionAcomplished { get => connectionAcomplished; set => connectionAcomplished = value; }
        
        public bool UpdateSetup { get => updateSetup; set => updateSetup = value; }
        public bool TurnOffRobot { get => turnOffRobot; set => turnOffRobot = value; }



        #endregion
    }
}
