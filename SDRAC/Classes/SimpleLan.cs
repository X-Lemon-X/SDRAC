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
        #region Error ID Description

        /* summary errors id 
         * 
         * id = 1, shortMsg = "Skipped message"
         * id = 2, shortMsg = "SendHandler() Error"
         * id = 3, shortMsg = "ReciveHandler() Error"
         * id = 4, shortMsg = "Send() Error", 
         * id = 5, shortMsg = "Start() Error/Setings
         * id = 6, shortMsg = "Start() Error/Begining"
         * id = 7, shortMsg = "localIp = null"
         * id = 10, shortMsg = "Couldn't set local Ip!"
         * id = 11, shortMsg = "No network connection!"
         * 
         * Don't touch
         */

        #endregion

        #region Objects
        public ConnectionData cdlSerwer = null;
        public List<ConnectionData> cdl = null;
        public List<ErrorClass> cListErrorsDef = new List<ErrorClass>();
        public int[] ConnectionIdL = null;
        public Codes codes = new Codes();

        public event EventHandler<Command> NewDataIncomeEvent;
        public event EventHandler<ErrorClass> ErrorIncomingEvent;
        public event EventHandler<bool> ConnectionStatusChangedEvent;
        public event EventHandler<EventClassReturn> EventClassReturnHandler;

        // private static Mutex mutexAdd = new Mutex(), mutexRead = new Mutex();
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
        public int maxHosts;
        #endregion

        #region Classes

        public class Codes
        { 
            private int _ackNowledge = 35000;
            private int _notAckNowledge = 36000;
            private int _portChange = 37000;
            private int _alive = 38000;

            public int AckNowledge { get => _ackNowledge; }
            public int NotAckNowledge { get => _notAckNowledge; }
            public int PortChange { get => _portChange; }
            public int Alive { get => _alive;}
        }

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
            public int id = 0,idConnection;
            public string shortMsg = null, longMsg = null, time=null;
            public Command command = null;
            public DateTime datatime;
            public ConnectionData connectionData=null;

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
            public string _ip=null;
            public int _portPrivate =0, id=-1,code = 0, msgIdAck=0;
            public bool connected = false, working=false,ack, notAck, connectedLastState=false;
            public Socket socketCon = null;
            public List<Command> cListOut = null;
            public List<Command> cListIn = null;
            public List<ErrorClass> cListErrors = null;
            public List<int> idListIn = null, idListOut = null;
            public Mutex mutexAdd = null, mutexRead = null;
            public Thread readLW= null, sendLW = null;
            public long communicatsRecived = 0, notAcknowledgeRecived = 0, acknowledgeRecived = 0, passedCommunicats = 0, sthRecived=0;

            public void DefaultStart()
            {
               cListOut = new List<Command>();
               cListIn = new List<Command>();
               cListErrors = new List<ErrorClass>();
               idListIn = new List<int>(); 
               idListOut = new List<int>();
               mutexAdd = new Mutex();
               mutexRead = new Mutex();
               id = 0;
            }
        }

        public class EventClassReturn
        {
            public int _idConn = -1;
            public ErrorClass _errorClass = null;
            public Command _command = null;
            public ConnectionData _connectionData = null;
            public bool _connection = false, _connected = false;
        }

        public class SetupClient
        {
            public int _msgMaxSize=64, _oneMessageTimeout=100, _defaultPort=25000;
            public string _hostIp4;
        }

        public class SetupSerwer
        {
            public int _msgMaxSize = 64, _defoultPort = 25000, _oneMessageTimeout = 100, _disconnectedTimeout = 1000, _maxHosts = 16;

        }

        #endregion

        #region SendNewCommand
        public int SendNewCommand(int idConnection,int _code, bool _rm, int _Length, long[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _Length, _data);
            return AddQueue(cm, idConnection);
        }
        public int SendNewCommand(int idConnection, int _code, bool _rm, int _Length, int[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _Length, _data);
            return AddQueue(cm, idConnection);
        }
        public int SendNewCommand(int idConnection, int _code, bool _rm, byte[] _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _data);
            return AddQueue(cm, idConnection);
        }
        public int SendNewCommand(int idConnection, int _code, bool _rm)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm);
            return AddQueue(cm, idConnection);
        }
        public int SendNewCommand(int idConnection, int _code, bool _rm, string _data)
        {
            Command cm = new Command();
            cm.CreateNew(_code, _rm, _data);
            return AddQueue(cm, idConnection);
        }
        public int SendNewCommand(int idConnection, Command command)
        {
            return AddQueue(command, idConnection);
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
            SerwerOrClient = false;
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
            cdl = new List<ConnectionData>();
            maxSizeOfOneMessage = ss._msgMaxSize;
            receiveBufferSize = ss._msgMaxSize * 64;
            bufferSize = 2 * ss._msgMaxSize;
            defaultPort = ss._defoultPort;
            localIp = LocalIp4();
            maxHosts = ss._maxHosts;
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
            if (NetworkInterface.GetIsNetworkAvailable())
            {
                if (localIp != null)
                {
                    try
                    {
                        Stop();
                        if (SerwerOrClient) return SetSerwer();
                        else return SetClient();
                    }
                    catch (Exception ex) { AddError(0, new ErrorClass() { id = 6, shortMsg = "Start() Error/Begining", longMsg = ex.ToString() }); }
                }
                else AddError(0, new ErrorClass() { id = 7, shortMsg = "LoclaIp=null" });
            }
            else AddError(0, new ErrorClass() { id = 11, shortMsg = "No network connection!" });

            return -2;
        }

        public int Stop()
        {
                if (SerwerOrClient) return StopSerwer();
                else return StopClient();
        }

        public int StopAll()
        {
            if (1 == StopClient() && 1 == StopSerwer()) return 1;
            else return -1;
        }

        private int StopCertainConnection(int connectionId)
        {
            try
            {
                if (cdl != null)
                {
                    var item = cdl.Find(e => e.id == connectionId);
                    if (item != null)
                    {
                        item.working = false;
                        if (item.readLW != null) if (item.readLW.IsAlive) item.readLW.Join();
                        if (item.sendLW != null) if (item.sendLW.IsAlive) item.sendLW.Join();
                        item.socketCon.Close();
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                AddError(connectionId, new ErrorClass() { idConnection = connectionId, shortMsg = "StopCertainConnection error" });
                return -1;
            }
        }

        private int SetSerwer()
        {
            try
            {
                cdlSerwer.socketCon = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                cdlSerwer.socketCon.ReceiveBufferSize = receiveBufferSize;
                cdlSerwer.socketCon.ReceiveTimeout = reciveTimeout;
                cdlSerwer.socketCon.SendTimeout = sendTimeout;
                cdl = new List<ConnectionData>();
                cdlSerwer.readLW = new Thread(() => ListenSerwerHandler(cdlSerwer));
                cdlSerwer.sendLW = new Thread(() => SendHendler(cdlSerwer));
                cdlSerwer.readLW.Start();
                cdlSerwer.sendLW.Start();
                return 1;
            }
            catch (Exception ex) { AddError(0,new ErrorClass() { id = 5, shortMsg = "Start() Error/Setings", longMsg = ex.ToString() }); }                  
            return -1;
        }

        private int SetClient()
        {
            try
            {
                cdl = new List<ConnectionData>();
                cdl.Add(new ConnectionData());
                ConnectionData cd = cdl[0];
                
                cd.socketCon = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                cd.socketCon.ReceiveBufferSize = receiveBufferSize;
                cd.socketCon.ReceiveTimeout = reciveTimeout;
                cd.socketCon.SendTimeout = sendTimeout;
                cd.socketCon.Connect(hostIp, defaultPort);
                cd._ip = hostIp;
                cd._portPrivate = defaultPort;
                cd.id = 0;
                cd.DefaultStart();
                cd.working = true;             
                cd.readLW = new Thread(() => ReadHandler(cd));
                cd.sendLW = new Thread(() => SendHendler(cd));
                cd.readLW.Start();
                cd.sendLW.Start();

                return 1;
            }
            catch (Exception ex) { AddError(0,new ErrorClass() { id = 5, shortMsg = "Start() Error/Setings", longMsg = ex.ToString() }); }
               

            return -1;
        }

        private int StopSerwer()
        {
            try
            {
                ConnectionData cds = cdlSerwer;
                if (cds != null)
                {
                    cds.working = false;
                    if (cds.readLW != null) if (cds.readLW.IsAlive) cds.readLW.Join();
                    if (cds.sendLW != null) if (cds.sendLW.IsAlive) cds.sendLW.Join();
                    cds.socketCon.Close();
                }

                if (cdl != null)
                {
                    foreach (var item in cdl)
                    {
                        item.working = false;
                        if (item.readLW != null) if (item.readLW.IsAlive) item.readLW.Join();
                        if (item.sendLW != null) if (item.sendLW.IsAlive) item.sendLW.Join();
                        item.socketCon.Close();
                    }
                    cdl.Clear();
                }
                return 1;
            }
            catch (Exception)
            {
                AddError(0,new ErrorClass() {shortMsg="StopSerwer() error" });
                return -1;
            }
        }

        private int StopClient()
        { 
            try 
            {	  
                if(cdl!=null)
                if(cdl.Count>0)
                {
                    cdl[0].working = false;
                    if (cdl[0].readLW != null) if (cdl[0].readLW.IsAlive) cdl[0].readLW.Join();
                    if (cdl[0].sendLW != null) if (cdl[0].sendLW.IsAlive) cdl[0].sendLW.Join();
                }
                return 1;
            }
            catch (Exception)
            {

	           return -1;
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
                    AddError(0,new ErrorClass() { id = 11, shortMsg = "No network connection!" });
                
            }
            catch (Exception e)
            { AddError(0,new ErrorClass() { id = 10, shortMsg="Couldn't set local Ip!", longMsg=e.ToString()}); }
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

        #region Private

        private void ListenSerwerHandler(ConnectionData cd)
        {
            Codes cod = new Codes();
            Command cm, _notA = new Command(), _ack = new Command(), _portC = new Command();
            IPAddress hp = (Dns.Resolve(IPAddress.Any.ToString())).AddressList[0];
            IPEndPoint ep = new IPEndPoint(hp, defaultPort);
            Socket sm = cd.socketCon;

            sm.Bind(ep);
            _notA.CreateNew(cod.NotAckNowledge, true);
            _ack.CreateNew(cod.AckNowledge, true);

            bool read = true;
            while (cd.working)
            {
                try
                {
                    byte[] buffer = new byte[receiveBufferSize];
                    try { int g = sm.Receive(buffer); read = true; } catch (Exception) { read = false; }
                    int p = 0;
                    if (read)
                    foreach (byte item in buffer)
                    {
                        if (item == (byte)'@')
                        {
                            // @  R|M (3nb size) (2nb msgNumber) G (2 nb code) [data] #

                            int size = BytesToInt(ByteArrayCutter(buffer, p + 2, 3));
                            if (size <= maxSizeOfOneMessage)
                            {
                                byte[] data = ByteArrayCutter(buffer, p, size + 5);
                                cm = new Command();

                                if (cm.DecomposeCommand(data) >= 0)
                                {
                                    if (cm.code == cod.PortChange)
                                    {
                                        int portCheck = 25001;
                                        if (cdl.Count != 0 || cdl.Count < maxHosts)
                                        while (true)
                                        {
                                            var list = cdl.Find(e => e._portPrivate == portCheck);
                                            if (list != null) break;
                                            portCheck++;
                                            if (portCheck > 65535) portCheck = 25001;         
                                        }
                                        int[] pch = { portCheck };
                                        _portC.CreateNew(cod.PortChange, true, 2, pch);
                                        Send(sm, _portC, 0);
                                    }
                                }
                                else { Send(sm, _notA, 0); }
                            }
                        }
                        p++;
                    }
                }
                catch (Exception ex)
                {AddError(0,new ErrorClass() { id = 3, shortMsg = "ListenHandler() Error", longMsg = ex.ToString() });}
            }

            sm.Close();
        }

        private void ReadHandler(ConnectionData cd)
        {
            Codes cod = new Codes();
            int idConnection = cd.id;
            Stopwatch sw = new Stopwatch(); sw.Start();
            Command cm, _notA = new Command(), _ack = new Command();
            _notA.CreateNew(cod.NotAckNowledge,true);
            _ack.CreateNew(cod.AckNowledge,true);
            int[] empty ={0};

            while (cd.working)
            {
                try
                {
                    byte[] readB = new byte[bufferSize];
                    bool read = true;
                    try { int g = cd.socketCon.Receive(readB); cd.sthRecived++; } catch (Exception ex) { read = false; }

                    int p = 0;
                    if(read)
                    foreach (byte item in readB)
                    {
                        if (item == (byte)'@')
                        {
                            // @  R|M (3nb size) (2nb msgNumber) G (2 nb code) [data] #
                            
                            int size = BytesToInt(ByteArrayCutter(readB, p + 2, 3));
                            if (size <= maxSizeOfOneMessage)
                            {
                                byte[] data = ByteArrayCutter(readB, p, size + 5);
                                cm = new Command();

                                cd.communicatsRecived++;

                                cd.mutexRead.WaitOne();
                                if (cm.DecomposeCommand(data) >= 0)
                                {
                                    cd.connected = true;
                                    cd.passedCommunicats++;

                                    if (cm.code == cod.AckNowledge && cm.id == cd.msgIdAck)
                                    {
                                        cd.ack = true; cd.acknowledgeRecived++;
                                    }
                                    else if (cm.code == cod.NotAckNowledge && cm.id==0)
                                    {
                                        cd.notAck = true; cd.notAcknowledgeRecived++;
                                    }
                                    else if(cm.code == cod.AckNowledge && cm.id != cd.msgIdAck)
                                    {
                                        AddError(idConnection,new ErrorClass() { id = 9, command = cm, shortMsg = "Id not confirmed!", longMsg = "in=>" + cm.id.ToString() + "   out=>" + cd.msgIdAck.ToString() });
                                    }        
                                    else
                                    {
                                        Send(cd.socketCon,_ack, cm.id);
                                        cd.cListIn.Add(cm);
                                        EventClassReturnHandler?.Invoke(this,new EventClassReturn() { _command =cm});
                                        //NewDataIncomeEvent?.Invoke(this,cm);    // ?  robi sprawdzenie czy ivent nie jest null                                      
                                    }
                                }
                                else { Send(cd.socketCon,_notA, 0); }
                                cd.mutexRead.ReleaseMutex();

                                sw.Restart();
                            }
                        }
                        p++;
                    }                 
                }
                catch (Exception ex) { AddError(idConnection ,new ErrorClass() { id = 3, shortMsg = "ReciveHandler() Error", longMsg = ex.ToString() }); }

                if (sw.ElapsedMilliseconds > connectionNoAnswearDelay)
                    cd.connected = false;

                if (cd.connectedLastState != cd.connected)
                { 
                    cd.connectedLastState = cd.connected; 
                    EventClassReturnHandler?.Invoke(this, new EventClassReturn() { _connection = true, _connected=cd.connected }); 
                   // ConnectionStatusChangedEvent?.Invoke(this,cd.connected); 
                }

            }
            cd.connected = false;

        }

        private void SendHendler(ConnectionData cd)
        {
            int idConnection = cd.id;
            Codes cod = new Codes();
            //ConnectionData cd = null;

            //if (idConnection >=0) cd = cdl.Find(e => e.id == idConnection);
            //else cd = cdlSerwer;

            Stopwatch timer = Stopwatch.StartNew(), tAlive = Stopwatch.StartNew();
            Command aliveC = new Command(), currentCm = null;
            aliveC.CreateNew(cod.Alive, false);

            int counter = 0;
            bool wait = false, elap = false;
            long elapsed;
            int id = cd.msgIdAck;

            timer.Reset();
            tAlive.Restart();

            while (cd.working)
            {
                try
                {  
                    cd.mutexRead.WaitOne();

                    if (tAlive.ElapsedMilliseconds >= sendTimeout)
                    { id++; Send(cd.socketCon, aliveC, id); cd.msgIdAck = id; tAlive.Restart(); }

                    cd.connected = true;
                    if (cd.connected)
                    {
                        if (currentCm == null)
                        {
                            cd.mutexAdd.WaitOne();
                            if (cd.cListOut.Count > 0)
                            {
                                currentCm = cd.cListOut.First();
                                cd.cListOut.RemoveAt(0);
                            }
                            cd.mutexAdd.ReleaseMutex();
                        }

                        if ((!wait || cd.notAck || elap) && currentCm != null)
                        {
                            id++;
                            Send(cd.socketCon,currentCm, id);
                            cd.msgIdAck = id;

                            timer.Restart();
                            wait = true;
                            elap = false;
                            cd.notAck = false;
                            cd.ack = false;
                            counter++;
                        }

                        if (cd.ack && wait)
                        {
                            cd.ack = false;
                            cd.notAck = false;
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
                                cd.ack = false;
                                cd.notAck = false;
                                wait = false;
                                timer.Stop();
                                tAlive.Restart();
                                counter = 0;
                                AddError(idConnection,new ErrorClass() { id = 1, command = currentCm, shortMsg = "Skipped message/Timed out" });
                                currentCm = null;
                            }
                            else if ((elapsed = timer.ElapsedMilliseconds) >= oneMessageTimeout) elap = true;
                        }                 
                    }

                    if (id > 65534) id = 0;

                    cd.mutexRead.ReleaseMutex();

                    Thread.Sleep(1);
                }
                catch (Exception ex) { AddError(idConnection,new ErrorClass() { id = 2, shortMsg = "SendHandler() Error", longMsg = ex.ToString() }); }
            }
        }

        private int SendPriority(ConnectionData cd)
        {
            int idConnection = cd.id;
            Codes cod = new Codes();

            Stopwatch timer = Stopwatch.StartNew(), tAlive = Stopwatch.StartNew();
            Command aliveC = new Command(), currentCm = null;
            aliveC.CreateNew(cod.Alive, false);

            int counter = 0;
            bool wait = false, elap = false;
            long elapsed;
            int id = cd.msgIdAck;

            timer.Reset();
            tAlive.Restart();

            while (cd.working)
            {
                try
                {
                    
                    if (tAlive.ElapsedMilliseconds >= sendTimeout)
                    { id++; Send(cd.socketCon, aliveC, id); cd.msgIdAck = id; tAlive.Restart(); }

                    cd.connected = true;
                    if (cd.connected)
                    {
                        if (currentCm == null)
                        {
                            cd.mutexAdd.WaitOne();
                            if (cd.cListOut.Count > 0)
                            {
                                currentCm = cd.cListOut.First();
                                cd.cListOut.RemoveAt(0);
                            }
                            cd.mutexAdd.ReleaseMutex();
                        }

                        if ((!wait || cd.notAck || elap) && currentCm != null)
                        {
                            id++;
                            Send(cd.socketCon, currentCm, id);
                            cd.msgIdAck = id;

                            timer.Restart();
                            wait = true;
                            elap = false;
                            cd.notAck = false;
                            cd.ack = false;
                            counter++;
                        }

                        if (cd.ack && wait)
                        {
                            cd.ack = false;
                            cd.notAck = false;
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
                                cd.ack = false;
                                cd.notAck = false;
                                wait = false;
                                timer.Stop();
                                tAlive.Restart();
                                counter = 0;
                                AddError(idConnection, new ErrorClass() { id = 1, command = currentCm, shortMsg = "Skipped message/Timed out" });
                                currentCm = null;
                            }
                            else if ((elapsed = timer.ElapsedMilliseconds) >= oneMessageTimeout) elap = true;
                        }
                    }

                    if (id > 65534) id = 0;

                    cd.mutexRead.ReleaseMutex();

                    Thread.Sleep(1);
                }
                catch (Exception ex) { AddError(idConnection, new ErrorClass() { id = 2, shortMsg = "SendHandler() Error", longMsg = ex.ToString() }); }
            }

            return -1;
        }

        private void Send(Socket sc,Command cm, int id)
        {
            try
            {
                sc.Send(cm.Compose(id), cm.fullSize, SocketFlags.None);
            }
            catch (Exception ex) { AddError(new ErrorClass() { id = 4, shortMsg = "Send() Error", longMsg = ex.ToString() }); }
        }

        private int AddQueue(Command dataAdd,int idConnection)
        {
            if(dataAdd != null && dataAdd.id >= 0 && idConnection >= 0)
            {
                ConnectionData cd = cdl.Find(e => e.id == idConnection);
                cd.mutexAdd.WaitOne();
                cd.cListOut.Add(dataAdd);
                cd.mutexAdd.ReleaseMutex();
                return 1;
            }
            return -1;
        }

        private void AddError(int idConnection, ErrorClass er)
        {
            if (er != null)
            {
                ConnectionData cd= cdl.Find(e => e.id == idConnection);
                er.SetCurrentTime();
                cd.cListErrors.Add(er);
                EventClassReturnHandler?.Invoke(this,new EventClassReturn() { _errorClass = er , _idConn = idConnection});
                //ErrorIncomingEvent?.Invoke(this, er);
            }
        }

        private void AddError(ErrorClass er)
        {
            if (er != null)
            {
                er.SetCurrentTime();
                cListErrorsDef.Add(er);
                EventClassReturnHandler?.Invoke(this, new EventClassReturn() { _errorClass = er });
            }
        }
        #endregion
    }
}
