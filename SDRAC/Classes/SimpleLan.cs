using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;


namespace SDRAC.Classes
{
    public class SimpleLan
    {
        #region Description

        /* summary errors id
         * 
         * id = 1, shortMsg = "Skipped message"
         * id = 2, shortMsg = "SendHandler() Error"
         * id = 3, shortMsg = "ReciveHandler() Error"
         * id = 4, shortMsg = "Send() Error", 
         * id = 5, shortMsg = "Start() Error/Setings
         * id = 6, shortMsg = "Start() Error/Begining"
         * 
         * id = 10, shortMsg = "Couldn't set local Ip!"
         * id = 11, shortMsg = "No network connection!"
         * 
         */

        #endregion

        #region Objects
        public Thread readLW, sendLW = null;
        public TcpClient tcpClient = null;
        public static Socket socketCon = null;
        public NetworkStream stream = null;
        public List<Command> cListOut = null;
        public List<Command> cListIn = null;
        public List<ErrorClass> cListErrors = null;
        public List<int> idListIn = null, idListOut = null;
        public event EventHandler<Command> NewDataIncomeEvent;
        public event EventHandler<ErrorClass> ErrorIncomingEvent;
        public event EventHandler<bool> ConnectionStatusChangedEvent; 

        private static Mutex mutexAdd = new Mutex(), mutexRead = new Mutex();
        #endregion

        #region Setup veriables
        public int bufferSize = 256;
        public long connectionNoAnswearDelay = 500; //ms
        public static int maxSizeOfOneMessage = 64;
        public int reciveTimeout = 1000; //ms
        public int sendTimeout = 500; //ms
        public int oneMessageTimeout = 30; //ms
        public int messagesTimeout = 400; //ms
        public int receiveBufferSize = 8192;
        public int defaultPort = 55000;
        public string localIp = null;
        public bool readingLanWifi = false;
        public bool connected = false;
        public string hostIp;
        public bool SerwerOrClient = false; // true for server      false for client
        #endregion

        #region Veriables
        int commandCounter = 0;     
        //private delEventHandler _show;
        public int code = 0;
        bool ack, notAck, connectedLastState=false;
        int msgIdAck;
        long communicatsRecived = 0, notAcknowledgeRecived = 0, acknowledgeRecived = 0, passedCommunicats = 0, sthRecived=0;
        private string _hostIp;

        #endregion

        #region Classes
        public class Command
        {
            public byte[] dataf = null, dataOnly = null;
            public int id = -1, size = -1, code = -1, fullSize=0;
            public bool rm = false;

            #region CreateNew
            public void CreateNew(int _code, bool _rm, int _Length, long[] _data)
            {
                BM(_code, _rm, _Length, _data.Length);

                dataOnly = new byte[_data.Length * _Length];

                int p = 10, h=0;
                foreach (long item in _data)
                {
                    int i = 0;
                    byte[] ds = ValueToBytes(item, _Length);
                    foreach (byte it in ds)
                    { dataf[p] = ds[i]; i++; p++; dataOnly[h] = it; h++; }
                }

            }
            public void CreateNew(int _code, bool _rm, int _Length, int[] _data)
            {
                BM(_code, _rm, _Length, _data.Length);

                dataOnly = new byte[_data.Length * _Length];
                int p = 10, h=0;
                foreach (int item in _data)
                {
                    int i = 0;
                    byte[] ds = ValueToBytes(item, _Length);
                    foreach (byte it in ds)
                    { dataf[p] = ds[i]; i++; p++; dataOnly[h] = it; h++; }
                }
            }
            public void CreateNew(int _code, bool _rm, byte[] _data)
            {
                BM(_code, _rm, 1, _data.Length);

                dataOnly = new byte[_data.Length];
                int h = 0, p=10;
                foreach (byte item in _data)
                {
                    dataf[p] = item;  
                    dataOnly[h] = item; 
                    h++; p++; 
                }
            }
            public void CreateNew(int _code, bool _rm)
            {
                BM(_code, _rm, 0, 0);
            }
            public void CreateNew(int _code, bool _rm, string _data)
            {
                BM(_code, _rm, 1, _data.Length);
                int p = 10, h = 0;
                foreach (char item in _data)
                { dataf[p] = (byte)item; p++; dataOnly[h] = (byte)item; h++; }
            }
            #endregion

            private void BM(int _code, bool _rm, int _Length, int _dLength)
            {
                size = (_dLength * _Length) + 6;
                id = 0;
                code = _code;
                rm = _rm;

                dataf = new byte[size + 5];
                byte[] sup = ValueToBytes(size, 3);

                dataf[0] = (byte)'@';
                if (rm)
                    dataf[1] = (byte)'R';
                else
                    dataf[1] = (byte)'M';

                dataf[2] = sup[0];
                dataf[3] = sup[1];
                dataf[4] = sup[2];
                dataf[5] = 0;
                dataf[6] = 0;
                dataf[7] = (byte)'G';
                dataf[8] = (byte)(code >> 8);
                dataf[9] = (byte)code;
                dataf[dataf.Length - 1] = (byte)'#';

            }
            public int DecomposeCommand(byte[] _data)
            {
                try
                {
                    size = BytesToInt(ByteArrayCutter(_data, 2, 3));
                    code = BytesToInt(ByteArrayCutter(_data, 8, 2));
                    id = BytesToInt(ByteArrayCutter(_data, 5, 2));
                    
                    dataf = new byte[size + 5];
                    if(size > 6) dataOnly = ByteArrayCutter(_data,11,size-6);
                   
                    if (ByteArrayCutter(_data, 1, 1)[0] == (byte)'R') rm = true;
                    else rm = false;

                    int p = 0;
                    foreach (byte item in _data) { dataf[p] = item; p++; }

                    if (_data[7] == (byte)'G' && _data[size + 4] == (byte)'#' && size <= maxSizeOfOneMessage) return 0;
                    else return -1;
                }
                catch (Exception) { return -2; }

            }
            public byte[] Compose(int _id)
            {
                id = _id;
                dataf[5] = (byte)(id >> 8);
                dataf[6] = (byte)id;
                fullSize = dataf.Length;

                return dataf;
            }

        }
        public class ErrorClass
        {
            public int id = 0;
            public string shortMsg = null, longMsg = null, time=null;
            public Command command = null;
            public DateTime datatime;

            public string SetCurrentTime()
            {
                time = "";
                string tp = DateTime.Now.Hour.ToString();
                if (tp.Length < 2) time += "0" + tp;
                else time += tp;
                time += ":";

                tp = DateTime.Now.Minute.ToString();
                if (tp.Length < 2) time += "0" + tp;
                else time += tp;
                time += ":";

                tp = DateTime.Now.Second.ToString();
                if (tp.Length < 2) time += "0" + tp;
                else time += tp;
                time += ":";

                tp = DateTime.Now.Millisecond.ToString();
                if (tp.Length < 2) time += "0" + tp;
                else time += tp;

                datatime = DateTime.Now;
                
                return time;
            }

        }

        public class ConnectionData
        {
            string _ip=null;
            int _portPrivate=0;
            bool connected = false;
            public TcpClient tcpClient = null;
            public static Socket socketCon = null;
            public NetworkStream stream = null;
            public List<Command> cListOut = null;
            public List<Command> cListIn = null;
            public List<ErrorClass> cListErrors = null;
            public List<int> idListIn = null, idListOut = null;

        }

        public class SetupClient
        {
            public int _msgMaxSize=64, _oneMessageTimeout=100, _defaultPort=25000;
            public string _hostIp4;
        }

        public class SetupSerwer
        {
            public int _msgMaxSize = 64, _defoultPort = 25000, _oneMessageTimeout = 100, _disconnectedTimeout = 1000, _maxHosts = 8;

        }
        #endregion

        #region SendNewCommand
        public int SendNewCommand(int _code, bool _rm, int _Length, long[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _Length, _data);
            return AddQueue(cm);
        }
        public int SendNewCommand(int _code, bool _rm, int _Length, int[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _Length, _data);
            return AddQueue(cm);
        }
        public int SendNewCommand(int _code, bool _rm, byte[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _data);
            return AddQueue(cm);
        }
        public int SendNewCommand(int _code, bool _rm)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm);
            return AddQueue(cm);
        }
        public int SendNewCommand(int _code, bool _rm, string _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _data);
            return AddQueue(cm);
        }
        public int SendNewCommand(Command command)
        {
            return AddQueue(command);
        }

        #endregion

        #region SetupFunctions

        #region EasySetup
        public int EasySetup(int _msgMaxSize,int _oneMessageTimeout, int _port, string _hostIp4)
        {
            maxSizeOfOneMessage = _msgMaxSize;
            receiveBufferSize = _msgMaxSize * 64;
            bufferSize = 2 * _msgMaxSize;
            defaultPort = _port;
            hostIp = _hostIp4;
            localIp = LocalIp4();

            oneMessageTimeout = _oneMessageTimeout;
            messagesTimeout = _oneMessageTimeout * 10 + 100;
            connectionNoAnswearDelay = messagesTimeout * 2;
            reciveTimeout = messagesTimeout;
            return 1;
        }

        public int EasySetup(SetupClient sc)
        {
            maxSizeOfOneMessage = sc._msgMaxSize;
            receiveBufferSize = sc._msgMaxSize * 64;
            bufferSize = 2 * sc._msgMaxSize;
            defaultPort = sc._defaultPort;
            hostIp = sc._hostIp4;
            localIp = LocalIp4();

            oneMessageTimeout = sc._oneMessageTimeout;
            messagesTimeout = sc._oneMessageTimeout * 10 + 100;
            connectionNoAnswearDelay = messagesTimeout * 2;
            reciveTimeout = messagesTimeout;
            SerwerOrClient = false;
            return 1;
        }

        public int EasySetup(SetupSerwer ss)
        {
            maxSizeOfOneMessage = ss._msgMaxSize;
            receiveBufferSize = ss._msgMaxSize * 64;
            bufferSize = 2 * ss._msgMaxSize;
            defaultPort = ss._defoultPort;
            localIp = LocalIp4();

            oneMessageTimeout = ss._oneMessageTimeout;
            messagesTimeout = ss._oneMessageTimeout * 10 + 100;
            connectionNoAnswearDelay = messagesTimeout * 2;
            reciveTimeout = messagesTimeout;
            SerwerOrClient = true;

            return 0;
        }

        #endregion

        public int Start()
        {
            try
            {
                Stop();
                cListOut = new List<Command>();
                cListIn = new List<Command>();
                cListErrors = new List<ErrorClass>();
                communicatsRecived = 0;
                notAcknowledgeRecived = 0;
                acknowledgeRecived = 0; 
                passedCommunicats = 0;

                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    if (localIp != null)
                    {

                        try
                        {
                            if (socketCon != null) if (socketCon.Connected) socketCon.Close();

                            socketCon = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                            socketCon.ReceiveBufferSize = receiveBufferSize;
                            socketCon.ReceiveTimeout = reciveTimeout;
                            socketCon.SendTimeout = sendTimeout;
                            socketCon.Connect(hostIp, defaultPort);

                            readingLanWifi = false;

                            if (readLW != null) if (readLW.IsAlive) readLW.Join();
                            if (sendLW != null) if (sendLW.IsAlive) sendLW.Join();

                            readingLanWifi = true;
                            readLW = new Thread(new ThreadStart(ReadHandler));
                            sendLW = new Thread(new ThreadStart(SendHendler));
                            readLW.Start();
                            sendLW.Start();
                            
                            return 0;

                        }
                        catch (Exception ex) { cListErrors.Add(new ErrorClass() { id = 5, shortMsg = "Start() Error/Setings", longMsg = ex.ToString() }); }


                    }
                    else MessageBox.Show("No Connection.", "Network Problem", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else AddError(new ErrorClass() { id = 11, shortMsg = "No network connection!" });
            }
            catch (Exception ex) { cListErrors.Add(new ErrorClass() { id = 6, shortMsg = "Start() Error/Begining", longMsg = ex.ToString() }); }

            return -1;
        }
        public int Stop()
        {
            readingLanWifi = false;
            try
            {
                if (readLW != null) if (readLW.IsAlive) readLW.Join();
                if (sendLW != null) if (sendLW.IsAlive) sendLW.Join();
                return 0;
            }
            catch (Exception)
            {

                return-1;
            }
        }
        #endregion

        #region Public
        public string LocalIp4()
        {
            string lP = null;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                    {
                        socket.Connect("8.8.8.8", 65530);
                        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                        lP = endPoint.Address.ToString();
                    }
                }
                else
                    AddError(new ErrorClass() { id = 11, shortMsg = "No network connection!" });
                
            }
            catch (Exception e)
            { AddError(new ErrorClass() { id = 10, shortMsg="Couldn't set local Ip!", longMsg=e.ToString()}); }
            return lP;
        }

        #region ValueToBytes
        public static byte[] ValueToBytes(long value, int length) //przy 0 MSB na dole LSB
        {
            byte[] data = new byte[length];
            if (length <= 8)
                for (int i = 0; i < length; i++)
                {
                    data[length - i - 1] = (byte)(value >> i * 8);
                }
            return data;
        }
        public static byte[] ValueToBytes(int value, int length) //przy 0 MSB na dole LSB
        {
            byte[] data = new byte[length];
            if (length <= 8)
                for (int i = 0; i < length; i++)
                {
                    data[length - i - 1] = (byte)(value >> i * 8);
                }
            return data;
        }
        public static byte[] ArrayValueToBytes(int[] value, int length)
        {
            int lg = value.Length;
            byte[] data = new byte[lg*length];
            int c,cp=0;

            for (int i = 0; i < lg; i++)
            {
                c = 0;
                foreach (byte item in ValueToBytes(value[i],length))
                {
                    data[cp + c] = item; c++; 
                }
                cp += length;
            }

            return data;
        }

        #endregion

        public static int BytesToInt(byte[] bytes) //prszy 0 MSB na dole LSB
        {
            int value = 0;
            int he;
            int size = bytes.Length;

            for (int i = 0; i < size; i++)
            {
                he = bytes[i];
                he = (he << ((size - i - 1) * 8));
                value += he;
            }

            return value;
        }
        public  static int[] BytesToIntArray(byte[] bytes, int intLength) //prszy 0 MSB na dole LSB
        {

            int size = bytes.Length, he;
            int amount = size / intLength;
            int[] value = new int[amount];
            
            for (int p = 0; p < amount; p++)
            {
                for (int i = 0; i < intLength; i++)
                {
                    he = bytes[i + (p * intLength)];
                    he = (he << ((intLength - i - 1) * 8));
                    value[p] += he;
                }
            }

            return value;
        }
        public  static byte[] ByteArrayCutter(byte[] array, int begIndex, int length)
        {
            byte[] arr = new byte[length];
            int p = 0;

            try
            {
                if (length > 0)
                    for (; p < length; p++)
                    {
                        arr[p] = array[begIndex]; begIndex++;
                    }
            }
            catch (Exception) { }
            return arr;
        }
        #endregion

        #region Privates
        private void ReadHandler()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Command cm, _notA = new Command(), _ack = new Command();
            _notA.CreateNew(29,true);
            _ack.CreateNew(25,true);

            int[] empty ={0};

            while (readingLanWifi)
            {
                try
                {
                    byte[] readB = new byte[bufferSize];
                    bool read = true;
                    try { int g = socketCon.Receive(readB); sthRecived++; } catch (Exception ex) { read = false; }

                    int p = 0;
                    if(read)
                    foreach (byte item in readB)
                    {
                        if (item == (byte)'@')
                        {
                            //data (a) - 8 * a bit   [] - a few bits  -->  @ (number of bytes)  G (code)    [data]       #  <--
                            //bytes                   bits = b                             0    1   <b + 3 ;b-2>   b-1    
                            // Data Look (number)bl => (number of bytes)
                            // @  R|M (3nb size) (2nb msgNumber) G (2 nb code) [data] #
                            
                            int size = BytesToInt(ByteArrayCutter(readB, p + 2, 3));
                            if (size <= maxSizeOfOneMessage)
                            {
                                byte[] data = ByteArrayCutter(readB, p, size + 5);
                                cm = new Command();

                                communicatsRecived++;

                                mutexRead.WaitOne();
                                if (cm.DecomposeCommand(data) >= 0)
                                {
                                    connected = true;

                                    passedCommunicats++;
                                    if (cm.code == 29 || cm.code == 25)
                                    {
                                        if (cm.id == msgIdAck)
                                        {
                                            if (cm.code == 29) { ack = true; acknowledgeRecived++; }
                                            else { notAck = true; notAcknowledgeRecived++; }
                                        }
                                        else
                                        {
                                           AddError(new ErrorClass() { id = 9, command = cm, shortMsg = "Id not confirmed!", longMsg = "in=>" + cm.id.ToString() + "   out=>" + msgIdAck.ToString() });
                                        }
                                    }
                                    else
                                    {
                                        Send(_ack, cm.id);
                                        cListIn.Add(cm);
                                        NewDataIncomeEvent?.Invoke(this,cm);    // ?  robi sprawdzenie czy ivent nie jest null
                                        //CommandTrigger.Invoke();
                                    }
                                }
                                else { Send(_notA, 0); }
                                mutexRead.ReleaseMutex();

                                sw.Restart();
                            }
                        }
                        p++;
                    }                 
                }
                catch (Exception ex) { AddError(new ErrorClass() { id = 3, shortMsg = "ReciveHandler() Error", longMsg = ex.ToString() }); }

                if (sw.ElapsedMilliseconds > connectionNoAnswearDelay)
                    connected = false;

                if (connectedLastState != connected) { connectedLastState = connected; ConnectionStatusChangedEvent?.Invoke(this,connected); }

            }
            connected = false;

        }
        private void SendHendler()
        {
            Stopwatch timer = Stopwatch.StartNew(), tAlive = Stopwatch.StartNew();
            Command aliveC = new Command(), currentCm = null;
            aliveC.CreateNew(8, false);

            int counter = 0;
            bool wait = false, elap = false;
            long elapsed;
            int id = msgIdAck;

            timer.Reset();
            tAlive.Restart();

            while (readingLanWifi)
            {
                try
                {  
                    mutexRead.WaitOne();

                    if (tAlive.ElapsedMilliseconds >= sendTimeout)
                    { id++; Send(aliveC, id); msgIdAck = id; tAlive.Restart(); }

                    connected = true;
                    if (connected)
                    {
                        if (currentCm == null)
                        {
                            mutexAdd.WaitOne();
                            if (cListOut.Count > 0)
                            {
                                currentCm = cListOut.First();
                                cListOut.RemoveAt(0);
                            }
                            mutexAdd.ReleaseMutex();
                        }

                        if ((!wait || notAck || elap) && currentCm != null)
                        {
                            id++;
                            Send(currentCm, id);
                            msgIdAck = id;

                            timer.Restart();
                            wait = true;
                            elap = false;
                            notAck = false;
                            ack = false;
                            counter++;
                        }

                        if (ack && wait)
                        {
                            ack = false;
                            notAck = false;
                            wait = false;
                            timer.Stop();
                            tAlive.Restart();
                            counter = 0;
                            currentCm = null;
                        }
                        else
                        {
                            if (counter > 10)
                            {
                                ack = false;
                                notAck = false;
                                wait = false;
                                timer.Stop();
                                tAlive.Restart();
                                counter = 0;
                                AddError(new ErrorClass() { id = 1, command = currentCm, shortMsg = "Skipped message/Timed out" });
                                currentCm = null;
                            }
                            else if ((elapsed = timer.ElapsedMilliseconds) >= oneMessageTimeout) elap = true;
                        }                 
                    }

                    if (id > 65534) id = 0;

                    mutexRead.ReleaseMutex();

                    Thread.Sleep(1);
                }
                catch (Exception ex) { AddError(new ErrorClass() { id = 2, shortMsg = "SendHandler() Error", longMsg = ex.ToString() }); }
            }
        }
        private void Send(Command cm, int id)
        {
            try
            {
                socketCon.Send(cm.Compose(id), cm.fullSize, SocketFlags.None);
            }
            catch (Exception ex) { AddError(new ErrorClass() { id = 4, shortMsg = "Send() Error", longMsg = ex.ToString() }); }
        }
        private int AddQueue(Command dataAdd)
        {
            if(dataAdd != null)
             if (dataAdd.id >= 0)
             {
                mutexAdd.WaitOne();
                cListOut.Add(dataAdd);
                mutexAdd.ReleaseMutex();
                return 0;
             }

            return -1;
        }
        private void AddError(ErrorClass er)
        {
            if (er != null)
            {
                er.SetCurrentTime();
                cListErrors.Add(er);
                ErrorIncomingEvent?.Invoke(this, er);
            }
        }
        #endregion

    }
}
