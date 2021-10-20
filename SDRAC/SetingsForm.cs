using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRAC
{
    public partial class SetingsForm : Form
    {
        #region Veriables
        bool SerialPortSel = true, autoManula=false;
        public bool SerialPortFound = false, wiFILanFoundNewDevice=false;
        byte messageNum = 0;
        public bool threadAuto = false, serchForRobots = true;
        Thread readAuto = null, lookingForRobot = null;
        List<NewDevice> devices = new List<NewDevice> { };
        #endregion

        public SetingsForm()
        {

            InitializeComponent();
            SetVaues();

        }

        private void SetVaues()
        {
            try
            {
                if (MainMenuForm.dataClass.Ip != null)
                {
                    string[] ip = MainMenuForm.dataClass.Ip.Split('.');
                    ip1.Text = ip[0];
                    ip2.Text = ip[1];
                    ip3.Text = ip[2];
                    ip4.Text = ip[3];
                }

                if (MainMenuForm.dataClass.Mask != null)
                {
                    string[] mask = MainMenuForm.dataClass.Mask.Split('.');
                    m1.Text = mask[0];
                    m2.Text = mask[1];
                    m3.Text = mask[2];
                    m4.Text = mask[3];
                }

                if (MainMenuForm.dataClass.Gate != null)
                {
                    string[] gate = MainMenuForm.dataClass.Gate.Split('.');
                    g1.Text = gate[0];
                    g2.Text = gate[1];
                    g3.Text = gate[2];
                    g4.Text = gate[3];
                }

                tbport.Text = MainMenuForm.dataClass.Port;
                tbpass.Text = MainMenuForm.dataClass.Pass;
                tbssid.Text = MainMenuForm.dataClass.Ssid;

            }
            catch (Exception ex) { MessageBox.Show(ex.ToString(), "Network setup problem"); }

            rbClick();

            if (autoManula)
                switch (MainMenuForm.dataClass.ConectionType)
                {
                    case 1: radioButtonUSB.Checked = true; StartAutoPort(); break;
                    case 2: radioButtonLan.Checked = true; StartLookingForRobotOnTheLanWIfi(Convert.ToInt32(MainMenuForm.dataClass.Port)); break;
                    case 3: radioButtonEthernet.Checked = true; break;

                    default: break;
                }
            else
                switch (MainMenuForm.dataClass.ConectionType)
                {
                    case 1: radioButtonUSB.Checked = true; StartmanualPort(); break;
                    case 2: radioButtonLan.Checked = true; StartManualLanWiFi(); break;
                    case 3: radioButtonEthernet.Checked = true; StartManulaEthernet(); break;

                    default: break;
                }


        }

        //ended
        #region Auto Functions

        #region PortCOM
        private void StartAutoPort()
        {
            if (!MainMenuForm.dataClass.Connect)
            {
                threadAuto = false;
                if (readAuto != null) if (readAuto.IsAlive) readAuto.Join();
                                       
                SerialPortFound = false;
                SerialPortSel = true;
                LightBtn(iconButtonAutoPort, iconButtonManual, Color.FromArgb(255, 0, 0));

                labelPortname.Text = "";
                listViewPorts.Visible = false;
                labelTextPort.ForeColor = Color.Maroon;
                labelTextPort.Text = "Searching on COMs...";
                string line = "", text = "";

                threadAuto = true;
                readAuto = new Thread(AutoPortSel);
                readAuto.Start();

            }

        }
        private void StopAutoPort()
        {
            if (readAuto != null)
            {
                threadAuto = false;

                if (readAuto.IsAlive) readAuto.Join();
            }
        }
        private void AutoPortSel()
        {

            string line = "", text = "";
            Stopwatch sw = new Stopwatch();
            Stopwatch stopwatch = new Stopwatch();
            int delay = 500;
            int begDelay = 0;
            sw.Start();
            string send = "@" + (char)2 + "G" + (char)38;

            while (threadAuto)
            {
                if (sw.ElapsedMilliseconds >= begDelay && !MainMenuForm.dataClass.Connect)
                {
                    foreach (var item in SerialPort.GetPortNames())
                    {
                        if (MainMenuForm.serialPort.IsOpen) MainMenuForm.serialPort.Close();

                        MainMenuForm.serialPort.PortName = item;
                        MainMenuForm.serialPort.ReadTimeout = 50;
                        MainMenuForm.serialPort.BaudRate = 115200;

                        bool portInUse = false;
                        try { MainMenuForm.serialPort.Open(); }
                        catch (Exception) { portInUse = true; }

                        if (!portInUse)
                        {
                            try
                            {

                                MainMenuForm.serialPort.Write(send);

                                long elapsed = 0;
                                stopwatch.Start();

                                while (stopwatch.ElapsedMilliseconds < 1000 && !SerialPortFound)
                                {
                                    int toread = MainMenuForm.serialPort.BytesToRead;

                                    if (elapsed + 100 == stopwatch.ElapsedMilliseconds && toread > 3)
                                    {
                                        byte[] byt = new byte[toread];
                                        MainMenuForm.serialPort.Read(byt, 0, toread);
                                        MainMenuForm.serialPort.Write(send);
                                        // line = MainMenuForm.serialPort.ReadExisting();
                                        int cou = 0;
                                        foreach (char sign in byt)
                                        {
                                            if (sign == '@')
                                            {
                                                try
                                                {
                                                    text = "";
                                                    text += (char)byt[cou + 1];
                                                    text += (char)byt[cou + 2];
                                                    text += (char)byt[cou + 3];
                                                }
                                                catch { }

                                                if (text == "\u0002G\u001c")
                                                {
                                                    MainMenuForm.serialPort.Close();
                                                    MainMenuForm.dataClass.Portname = item;
                                                    MainMenuForm.serialPort.PortName = item;
                                                    SerialPortFound = true;
                                                    threadAuto = false;
                                                    MainMenuForm.dataClass.UploadAble = true;
                                                    UpdateInfo.Start();
                                                    break;
                                                }
                                            }
                                            cou++;
                                        }
                                        elapsed = stopwatch.ElapsedMilliseconds;
                                    }
                                }
                                stopwatch.Stop();
                                MainMenuForm.serialPort.Close();
                            }
                            catch (Exception) { }
                        }
                    }
                    sw.Restart();
                }
                begDelay = delay;
                Thread.Sleep(100);
            }
        }
        #endregion

        #region WIFI / LAN
        public void StartLookingForRobotOnTheLanWIfi(int port)
        {
            if (!MainMenuForm.dataClass.Connect)
            {
                listViewPorts.Items.Clear();
                listViewPorts.Visible = true;
                LightBtn(iconButtonAutoPort, iconButtonManual, Color.FromArgb(255, 0, 0));
                labelTextPort.ForeColor = Color.OrangeRed;
                labelPortname.Text = "";
                labelTextPort.Text = "Searching on Lan / WiFi...";

                serchForRobots = false;
                if (lookingForRobot != null) if (lookingForRobot.IsAlive) lookingForRobot.Join();
                   
                lookingForRobot = new Thread(SerchingForRobotLW);
                string host = null;
                MainMenuForm.socketCon = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                MainMenuForm.socketCon.ReceiveBufferSize = 8192;
                MainMenuForm.socketCon.ReceiveTimeout = 2000;
                MainMenuForm.socketCon.EnableBroadcast = true;

                IPEndPoint ipep = new IPEndPoint(IPAddress.Broadcast, port);
                MainMenuForm.socketCon.Connect(ipep);

                serchForRobots = true;
                lookingForRobot.Start();
                
            }
        }
        private void StopLookingFROTLW()
        {       
            if (lookingForRobot!=null)
            {
                serchForRobots = false;
                if (lookingForRobot.IsAlive) 
                    lookingForRobot.Join();                   
            }
        }
        private void SerchingForRobotLW()
        {
            string ipdev = MainMenuForm.DeviceIP();      
            byte[] buffer = new byte[4096];
            serchForRobots = true;

            while (serchForRobots)
            {
                try
                {
                    string data = '@' + (char)(3 + ipdev.Length) + 'G' + (char)50 + ipdev + '#';
                    byte[] dataAsk = ConvertMessageInToBytes(data);
                    MainMenuForm.socketCon.Send(dataAsk);
                    int rec = MainMenuForm.socketCon.Receive(buffer);

                    int counter = 0;
                    if (rec>0)
                    {
                        for (int i = 0; i < rec; i++)
                        {
                            try
                            {
                                if (buffer[i] == (byte)'@')
                                {
                                    int length = buffer[i + 1];
                                    if (buffer[i + 1] == messageNum && buffer[i + 2] == (byte)'G' && buffer[i + 3] == 50 && buffer[i + length] == (byte)'#')
                                    {
                                        string ip = null;
                                        for (int g = 0; g < length - 3; g++)
                                        {
                                            ip += (char)buffer[i + g + 2];
                                        }

                                        if (!devices.Exists(k => k.IP == ip))
                                        {
                                            devices.Add(new NewDevice() { IP = ip, PortCom = null, Type = 1, Name = null });
                                            wiFILanFoundNewDevice = true;
                                        }
                                    }
                                }
                            }
                            catch (Exception) { }

                        }
                    }

                }
                catch (Exception ex) { }
            }
        }
        public byte[] ConvertMessageInToBytes(string data)
        {
            byte[] dataAsk = new byte[data.Length];
            int counter = 0;
            foreach (char item in data)
            {
                dataAsk[counter] = (byte)item;
                counter++;
            }
            return dataAsk;
        }
        #endregion

        #region Ethernet
        private void StartConectingByEthernet()
        {

            if (!MainMenuForm.dataClass.Connect)
            {
                LightBtn(iconButtonAutoPort, iconButtonManual, Color.FromArgb(255, 0, 0));

                labelPortname.Text = "";
                listViewPorts.Visible = false;
                labelTextPort.ForeColor = Color.DarkViolet;
                labelTextPort.Text = "Searching on Ethernet...";
                string line = "", text = "";
            }
        }
        private void StopAutoConnectionEthernet()
        {
            bool i = true;
        }
        #endregion

        private void StopAllAuto()
        {
            StopAutoPort();
            StopLookingFROTLW();
            StopAutoConnectionEthernet();
        }

        #endregion

        #region Manula Functions

        #region PortCOM
        private void StartmanualPort()
        {
            listViewPorts.Visible = true;
            labelTextPort.ForeColor = Color.Maroon;
            labelTextPort.Text = "List of avilable ports:";
            listViewPorts.Items.Clear(); timerSerialPortChrck.Start();
            SerialPortSel = false;
        }
        private void ManualPortSel()
        {
           
            listViewPorts.Visible = true;
            labelTextPort.Text = "List of avilable ports:";
            listViewPorts.Items.Clear(); 
            SerialPortSel = false;
            
            string[] portList = SerialPort.GetPortNames();
            bool exist;
            foreach (string item in portList)
            {
                listViewPorts.BeginUpdate();
                listViewPorts.Items.Add(item);
                listViewPorts.EndUpdate();
            }
        }
        private void timerSerialPortCOMCheck_Tick(object sender, EventArgs e)
        {
            if (!MainMenuForm.dataClass.Connect && MainMenuForm.dataClass.ConectionType==1 && !autoManula)
            {       
                    ManualPortSel();        
            }
        }
        private void listViewPorts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!MainMenuForm.dataClass.Threads)
            {
                if (listViewPorts.SelectedItem != null)
                {
                    int index = listViewPorts.SelectedIndex;

                    switch (MainMenuForm.dataClass.ConectionType)
                    {
                        case 1:                     
                            string serialComName = listViewPorts.SelectedItem.ToString();
                            MainMenuForm.dataClass.Portname = serialComName;
                            labelPortname.Text = "Selected: " + serialComName;
                            MainMenuForm.dataClass.UploadAble = true;
                            MainMenuForm.dataClass.Portname = "0";
                            break;
                        case 2: 
                            NewDevice nd = devices[index];
                            MainMenuForm.dataClass.Ip = nd.IP; 
                            labelPortname.Text = "Selected: " + nd.IP;
                            MainMenuForm.dataClass.UploadAble = true;
                            MainMenuForm.dataClass.Portname = "0";
                            break;
                        case 3: 
                            break;

                        default: break;
                    }

                    

                }
            }
        }
        private void StopManualPort()
        {
            timerSerialPortChrck.Stop();
            listViewPorts.Visible = false;
            labelTextPort.Text = "";

        }

        #endregion

        #region LAN / WIFI
        private void StartManualLanWiFi()
        {
            PanelWiFILanEth.Visible = true;
            listViewPorts.Visible = false;
            labelTextPort.ForeColor = Color.OrangeRed;
            labelTextPort.Text = "Lan/ WIFi manual ";
        }

        private void StopManualWiFILan()
        {
            PanelWiFILanEth.Visible = false;
        }
        #endregion

        #region Ethernet
        private void StartManulaEthernet()
        {
            PanelWiFILanEth.Visible = true;
            listViewPorts.Visible = false;
            labelTextPort.ForeColor = Color.DarkViolet;
            labelTextPort.Text = "Ethernet manual ";
        }
        private void StopManualEthernet()
        {
            PanelWiFILanEth.Visible = false;

        }
        #endregion

        private void StopAllManual()
        {
            StopManualWiFILan();
            StopManualEthernet();
            StopManualPort();
        }
        #endregion

        private void UpdateInfo_Tick(object sender, EventArgs e)
        {
            if (MainMenuForm.dataClass.ConectionType==1 && SerialPortFound)
            {
                StopAutoPort();
                SerialPortFound = false;           
                labelTextPort.Text = "Robot found on port: " + MainMenuForm.dataClass.Portname;
                labelPortname.Text = "Selected: " + MainMenuForm.dataClass.Portname;
                timerSerialPortChrck.Stop();
            }
            else if (MainMenuForm.dataClass.ConectionType == 2 && wiFILanFoundNewDevice)
            {
                wiFILanFoundNewDevice = false;
                listViewPorts.Items.Clear();
                foreach (var item in devices)
                {
                    listViewPorts.Items.Add("IP: " + item.IP + "  Name: " + item.Name);
                }
            }
            else
            {

            }
        }

        //ended
        #region BTN Animation
        private void LightBtn(object sender , object receiver, Color color)
        {
            IconButton btnToTurnOn = (IconButton)sender;
            IconButton btnToTurnOff = (IconButton)receiver;
            btnToTurnOn.IconColor = color;
            btnToTurnOn.IconSize = 40;
            btnToTurnOn.ForeColor = color;
           
            if (btnToTurnOff!=null)
            {
                btnToTurnOff.IconSize = 32;
                btnToTurnOff.IconColor = Color.DarkGray;
                btnToTurnOff.ForeColor = Color.DarkGray;
            }
        }

        #endregion
        
        //ended
        #region Buttons
        private void iconButtonAuto_Click(object sender, EventArgs e)
        {
            autoManula = true;
            StopAllAuto();
            StopAllManual();
            switch (MainMenuForm.dataClass.ConectionType)
            {
                case 1: StartAutoPort(); break;
                case 2: StartLookingForRobotOnTheLanWIfi(Convert.ToInt32(MainMenuForm.dataClass.Port)); break;
                case 3: StartConectingByEthernet(); break;

                default: break;
            }
        }
      
        private void iconButtonManual_Click(object sender, EventArgs e)
        {
            autoManula = false;     
            
            LightBtn(sender, iconButtonAutoPort, Color.FromArgb(150, 150, 0));      
            StopAllManual();
            StopAllAuto();

            switch (MainMenuForm.dataClass.ConectionType)
            {
                case 1: StartmanualPort();     break;
                case 2: StartManualLanWiFi();  break;
                case 3: StartManulaEthernet(); break;

                default: break;
            }
        
        }

      
        private void iconButtonOpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog.InitialDirectory = @"C:\%USERPROFILE%\Desktop";
            openFileDialog.Filter = "xml files (*.xml)|*.xml"; ;
            DialogResult dd = openFileDialog.ShowDialog();

            if (dd == DialogResult.OK)
            {
                try
                {
                    string appPath = Application.ExecutablePath;
                    appPath = appPath.Substring(0, appPath.Length - Application.ProductName.Length);
                    appPath += @"Csetup.xml";
                    string path = openFileDialog.FileName;

                    bool con = true;
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
                        System.IO.File.Copy(path, appPath);
                        textBoxDirectory.Text = appPath;
                        MainMenuForm.Tools = MainMenuForm.ReadXMLFile();
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Couldn't Copy Setup file!", "FileProblem");
                }
            }

            
        }

        private void iconButtonFileNew_Click(object sender, EventArgs e)
        {
            MainMenuForm.CreateXmlFile();
          //  MainMenuForm.Tools = MainMenuForm.ReadXMLFile();
        }

        private void radioButtonUSB_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonUSB.Checked)
            {
                MainMenuForm.dataClass.ConectionType = 1;
                rbClick();
            }       
        }

        private void radioButtonLan_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLan.Checked)
            {
                MainMenuForm.dataClass.ConectionType = 2;
                rbClick();
            }          
        }

        private void radioButtonEthernet_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEthernet.Checked)
            {
                MainMenuForm.dataClass.ConectionType = 3;
                rbClick();
            }
        }

        private void rbClick()
        {
            if (autoManula)
                iconButtonAuto_Click(iconButtonAutoPort, new EventArgs());
            else
                iconButtonManual_Click(iconButtonManual, new EventArgs());
        }
        private void iconButtonSave_Click(object sender, EventArgs e)
        {
            MainMenuForm.SaveXmlFile();
        }

        private void iconButtonSet_Click(object sender, EventArgs e)
        {
            string[] data = { ip1.Text, ip2.Text, ip3.Text, ip4.Text };
            MainMenuForm.dataClass.Ip = CreateAdd(data, 1, '0', '.');

            data = new string[] { g1.Text, g2.Text, g3.Text, g4.Text };
            MainMenuForm.dataClass.Gate = CreateAdd(data, 1, '0', '.');

            data = new string[] { m1.Text, m2.Text, m3.Text, m4.Text };
            MainMenuForm.dataClass.Mask = CreateAdd(data, 1, '0', '.');

            MainMenuForm.dataClass.Port = tbport.Text;
            MainMenuForm.dataClass.Pass = tbpass.Text;
            MainMenuForm.dataClass.Ssid = tbssid.Text;
            MainMenuForm.dataClass.UploadAble = true;
            MainMenuForm.dataClass.Portname = "0";
            MessageBox.Show("Data set!", "Lan / WiFi connection", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        #endregion

        //ended
        #region REST
        private void SetingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopAllAuto();
        }

        private void SetingsForm_Load_1(object sender, EventArgs e)
        {
            timerSerialPortChrck.Enabled = true;
        }   

        private string CreateAdd(string[] data,int length, char add, char seperator)
        {
            string ret = null;

            int counter = 0;
            foreach (string item in data)
            {
                int leng = item.Length;
                string aditives = null;
                if (length != leng) for (int i = 0; i < length - leng; i++) { aditives += add; }

                if (data.Length -1 != counter)ret += aditives + data[counter] + seperator;      
                else ret += aditives + data[counter];

                counter++;
            }

            return ret;
        }

        #endregion

        #region textboxs values handling
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&(e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ip2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void ip3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void ip4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void m1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void m2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void m3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void m4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void g1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void ip1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void g2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        

        private void g3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void g4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void tbport_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        #endregion

       
    }
}
