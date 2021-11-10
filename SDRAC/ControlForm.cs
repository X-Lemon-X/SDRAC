using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using FontAwesome.Sharp;

namespace SDRAC
{
    public partial class ControlForm : Form
    {
        #region Veriables
        bool AM =false;
        int btnAn=255;
        byte axisMode = 0;  // 1 = +Z 2 = -Z 3= +X 4= -X  5 = +Y 6 = -Y  7,8,RZ 9,10,RX 11,12RY 
        bool changed = false;
        int CurveShape = 1, MovemnetType = 0;

        #endregion

        #region Start of the form
        public ControlForm()
        {
            InitializeComponent();
            GetingValuesFromDataClass();
            UpdateListBoxTools();
        }

        #endregion

        // (work suspended)
        #region KeyPress Checking if Values are corect

        private KeyPressEventArgs KEPA(object sen,KeyPressEventArgs args)
        {
           
            if (!char.IsControl(args.KeyChar) && !char.IsDigit(args.KeyChar) && (args.KeyChar != '.')) args.Handled = true;
            if ((args.KeyChar == '.') && ((sen as TextBox).Text.IndexOf('.') > -1)) args.Handled = true;
            
            return args;
        }

        #endregion

        //ended
        #region Btn And animations To Btn

        private void LightBtn(object sender, object receiver, Color color)
        {
            if (sender!=null)
            {
                IconButton btnToTurnOn = (IconButton)sender;

                btnToTurnOn.IconColor = color;
                btnToTurnOn.IconSize = 40;
                btnToTurnOn.ForeColor = color;
            }
            

            if (receiver != null)
            {
                IconButton btnToTurnOff = (IconButton)receiver;
                btnToTurnOff.IconSize = 32;
                btnToTurnOff.IconColor = Color.DarkGray;
                btnToTurnOff.ForeColor = Color.DarkGray;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            iconButtonUpload.IconColor = Color.FromArgb(0, btnAn, 0);
            iconButtonUpload.ForeColor = Color.FromArgb(0, btnAn, 0);
            btnAn -= 10;
            if (btnAn <= 30)
            {
                btnAn = 255;
                iconButtonUpload.IconColor = Color.DarkGray;
                iconButtonUpload.ForeColor = Color.DarkGray;
                timer.Enabled = false;
            }

        }

        private void iconButtonAm_Click(object sender, EventArgs e)
        {
            AM = !AM;
            changed = true;
        }

        private void iconButtonUpload_Click(object sender, EventArgs e)
        {        
            timer.Enabled = true;
            btnAn = 255;
            SendingVeriablesToDataClass(true);   
        }
 
        private void iconButtonAdd_Click(object sender, EventArgs e)
        {
            SendingVeriablesToDataClass(false);
            MainMenuForm.movementWayPath.Add = true;
        }

        private void iconButtonRemove_Click(object sender, EventArgs e)
        {
            MainMenuForm.movementWayPath.Remove = true;
        }

        private void iconButtonRresetPos_Click(object sender, EventArgs e)
        {
            MainMenuForm.movementWayPath.Mode = (-1) * MainMenuForm.movementWayPath.Mode;
            MainMenuForm.movementWayPath.Add = true;
        }
        #endregion

        //ended
        #region VALUES

        private void UpdateListBoxTools()
        {
            comboBoxTool.Items.Clear();
            int index = 0;
            int i=0;
            foreach (var item in MainMenuForm.Tools.Tool)
            {
                if (item.Name == MainMenuForm.Tools.SelectedTool) index = i;                     
                comboBoxTool.Items.Add(item.Name);
                i++;
            }
            if(comboBoxTool.Items.Count >0)
            comboBoxTool.SelectedIndex = index;
        }
       
        private void SendingVeriablesToDataClass(bool condition)
        {
            timerUPDATE.Enabled = false;
            try
            {
                double[] Cor = new double[3];

                Cor[0] = Convert.ToDouble(nudX.Value);
                Cor[1] = Convert.ToDouble(nudY.Value);
                Cor[2] = Convert.ToDouble(nudZ.Value);

                double a = MainMenuForm.dataClass.A0;
                double L = (Cor[0] * Cor[0]) + (Cor[1] * Cor[1]);
                double res = (((Cor[2] - a) * (Cor[2] - a)) + L);
                if (res > MainMenuForm.dataClass.MaxRange * MainMenuForm.dataClass.MaxRange) { MessageBox.Show("Max range is " + MainMenuForm.dataClass.MaxRange.ToString() + ": seted is: " + Math.Sqrt(res).ToString(), "Max Range exceeded"); }
                else { MainMenuForm.dataClass.Cor = Cor; }

                double[] Jo = new double[6];
                
                Jo[0] = Convert.ToDouble(nudJ1.Value);
                Jo[1] = Convert.ToDouble(nudJ2.Value);
                Jo[2] = Convert.ToDouble(nudJ3.Value);
                Jo[3] = Convert.ToDouble(nudJ4.Value);
                Jo[4] = Convert.ToDouble(nudJ5.Value);
                Jo[5] = Convert.ToDouble(nudJ6.Value);

                int p = 0;
                for (int i = 0; i < 12; i+=2)
                {
                    double g = MainMenuForm.dataClass.JoLimits[i];
                    double b = MainMenuForm.dataClass.JoLimits[i+1];
                    if (Jo[p] >= g) Jo[p] = g;
                    else if (Jo[p] <= b) Jo[p] = b;
                    p++;
                }

                MainMenuForm.dataClass.Jo = Jo;

                MainMenuForm.dataClass.Accuracy = Convert.ToDouble(numericUpDownAcuracy.Value);

                MainMenuForm.dataClass.Speed = Convert.ToInt32(numericUpDownVelo.Value);
                MainMenuForm.dataClass.SpeedManula[0] = Convert.ToInt32(nudSJ1.Value);
                MainMenuForm.dataClass.SpeedManula[1] = Convert.ToInt32(nudSJ2.Value);
                MainMenuForm.dataClass.SpeedManula[2] = Convert.ToInt32(nudSJ3.Value);
                MainMenuForm.dataClass.SpeedManula[3] = Convert.ToInt32(nudSJ4.Value);
                MainMenuForm.dataClass.SpeedManula[4] = Convert.ToInt32(nudSJ5.Value);
                MainMenuForm.dataClass.SpeedManula[5] = Convert.ToInt32(nudSJ6.Value);

                MainMenuForm.dataClass.Acceleration = Convert.ToInt32(nudAccel.Value);
                MainMenuForm.dataClass.Deacceleration = Convert.ToInt32(nudDdeacel.Value);


                MainMenuForm.dataClass.JumpVector = Convert.ToDouble(numericUpDownJumpValue.Value);
                MainMenuForm.dataClass.MovementType = MovemnetType;
                MainMenuForm.dataClass.CurveShape = CurveShape;

                MainMenuForm.dataClass.ManualOrAuto = AM;
                MainMenuForm.dataClass.ManualSend = false;
                MainMenuForm.dataClass.ForceCount = true;

                if (condition) MainMenuForm.dataClass.UploadMath = true;
                

                
                
            }
            catch (Exception) { MessageBox.Show("", "Inapropriate number", MessageBoxButtons.OK, MessageBoxIcon.Information); }

            timerUPDATE.Enabled = true;
        }

        private void GetingValuesFromDataClass()
        {

            nudAccel.Value = Convert.ToDecimal(MainMenuForm.dataClass.Acceleration);
            nudDdeacel.Value = Convert.ToDecimal(MainMenuForm.dataClass.Deacceleration);


            int mt = MainMenuForm.dataClass.MovementType;
            switch (mt)
            {
                case 0: rbVector.Checked = true;
                    labelAutoVelo.Text = "Auto: mm/s";
                    break;
                case 1:
                    labelAutoVelo.Text = "Auto: VR";
                    rbVS.Checked = true;
                    break;
                case 2:
                    
                    break;
                case 3:
                    labelAutoVelo.Text = "Auto: deg/s";
                    rbOff.Checked = true;
                    break;
            }

           
            try
            {
                nudJ1.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[0]);
                nudJ2.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[1]);
                nudJ3.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[2]);
                nudJ4.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[3]);
                nudJ5.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[4]);
                nudJ6.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[5]);

                nudSJ1.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[0]);
                nudSJ2.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[1]);
                nudSJ3.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[2]);
                nudSJ4.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[3]);
                nudSJ5.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[4]);
                nudSJ6.Value = Convert.ToDecimal(MainMenuForm.dataClass.SpeedManula[5]);

                nudX.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[0]);
                nudY.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[1]);
                nudZ.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[2]);

                numericUpDownAcuracy.Value = Convert.ToDecimal(MainMenuForm.dataClass.Accuracy);
            }
            catch (Exception){}

            AM = MainMenuForm.dataClass.ManualOrAuto;

            if (MainMenuForm.movementWayPath.EditingPoint)
            {
                iconButtonAdd.ForeColor = Color.Red;
                iconButtonAdd.IconColor = Color.Red;
                iconButtonRemove.IconColor = Color.Red;
                iconButtonRemove.ForeColor = Color.Red;
                int mode = MainMenuForm.movementWayPath.Mode;
                if ( mode == 4 || mode == 3)
                {
                    iconButtonAdd.Text = " Add point";
                }
                else
                {
                    iconButtonAdd.Text = " Edit point";
                }
                labelPathdes.Text = "(Edit/Add mode) File Name:";


                iconButtonRemove.Text = " Cancel";
                
            }
            else if(MainMenuForm.movementWayPath.AddingPoint)
            {
                iconButtonAdd.ForeColor = Color.Red;
                iconButtonAdd.IconColor = Color.Red;
                iconButtonRemove.IconColor = Color.Red;
                iconButtonRemove.ForeColor = Color.Red;
                iconButtonAdd.Text = " Add point";
                iconButtonRemove.Text = " Cancel";

            }
            else
            {
                iconButtonRemove.Enabled = true;
            }
            comboBoxType.SelectedIndex = MainMenuForm.dataClass.FrameType;
            changed = true;

        }
        #endregion

        //ended
        #region Update Screen
        private void timerUPDATE_Tick(object sender, EventArgs e)
        {
            if (!AM && changed)
            {
                iconButtonAM.Text = "Auto";
                LightBtn(iconButtonAM, null, Color.FromArgb(145, 0, 212));
                textBox15.ForeColor = Color.Black;
                textBox16.ForeColor = Color.Black;
                textBox17.ForeColor = Color.Black;
                panelCordinants.Enabled = true;
                panelManualJ.Enabled = false;
                panelManualS.Enabled = false;
                panelAutoS.Enabled = true;
                panelToM.Enabled = true;
                panelBtnsXYZ.Enabled = true;
                changed = false;
            }
            else if (AM && changed)
            {
                iconButtonAM.Text = "Manual";
                LightBtn(iconButtonAM,null , Color.FromArgb(255, 80, 0));
                panelManualJ.Enabled = true;
                textBox15.ForeColor = Color.DarkGray;
                textBox16.ForeColor = Color.DarkGray;
                textBox17.ForeColor = Color.DarkGray;
                panelManualS.Enabled = true;
                panelAutoS.Enabled = false;
                panelCordinants.Enabled = false;
                panelToM.Enabled = false;
                panelBtnsXYZ.Enabled = false;
                changed = false;
            }

            if (MainMenuForm.dataClass.GetConInfo)      
                iconButtonShutDown.Enabled = true;
            else      
                iconButtonShutDown.Enabled = false;
            

              labelX.Text = (MainMenuForm.dataClass.Cor[0]).ToString();
              labelY.Text = Math.Round(MainMenuForm.dataClass.Cor[1]).ToString();
              labelZ.Text = Math.Round(MainMenuForm.dataClass.Cor[2]).ToString();

              labelJ1.Text = MainMenuForm.dataClass.JoIN[0].ToString();
              labelJ2.Text = MainMenuForm.dataClass.JoIN[1].ToString();
              labelJ3.Text = MainMenuForm.dataClass.JoIN[2].ToString();
              labelJ4.Text = MainMenuForm.dataClass.JoIN[3].ToString();
              labelJ5.Text = MainMenuForm.dataClass.JoIN[4].ToString();
              labelJ6.Text = MainMenuForm.dataClass.JoIN[5].ToString();

            labelPath.Text = MainMenuForm.movementWayPath.FileName;
            
        }

        #endregion

        //ended
        #region Rest to user interface

        private void rbVector_CheckedChanged(object sender, EventArgs e)
        {
            labelAutoVelo.Text = "Auto mm/s";
            MovemnetType = 0;
        }

        private void rbVS_CheckedChanged(object sender, EventArgs e)
        {
            labelAutoVelo.Text = "Auto VR";
            MovemnetType = 1;
        }

        private void rbOff_CheckedChanged(object sender, EventArgs e)
        {
            labelAutoVelo.Text = "Auto deg/s";
            MovemnetType = 2;
        }
        private void trackBarVel_Scroll(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(trackBarVel.Value);
            label14.Text = "Velocity: (" + i.ToString() + "%)";
            MainMenuForm.dataClass.VelScale = Convert.ToDouble(i)/100.0;
        }
        private void checkBoxCurveShape_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxCurveShape.Checked)
            {
                CurveShape = 1;
                checkBoxCurveShape.Text = "Curve Shape: UP";
            }
            else
            {
                checkBoxCurveShape.Text = "Curve Shape: DOWN";
                CurveShape = -1;
            }

        }
        #endregion

        //ended
        #region BtnXYZ
        private void inZp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }
  
        private void inZp_MouseDown(object sender, MouseEventArgs e)
        {       
            axisMode = 1;
            TimerAdd.Enabled = true;
        }
        private void ibZm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 2;
            TimerAdd.Enabled = true;
        }

        private void ibZm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }
        private void ibXp_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 3;
            TimerAdd.Enabled = true;
        }

        private void ibXp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibXm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 4;
            TimerAdd.Enabled = true;
        }

        private void ibXm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }
        private void ivYp_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 5;
            TimerAdd.Enabled = true;
        }

        private void ivYp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibYm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 6;
            TimerAdd.Enabled = true;
        }

        private void ibYm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }
        private void ibRZp_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 7;
            TimerAdd.Enabled = true;
        }

        private void ibRZp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibRZm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 8;
            TimerAdd.Enabled = true;
        }

        private void ibRZm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibRXp_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 9;
            TimerAdd.Enabled = true;
        }

        private void ibRXp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibRXm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 10;
            TimerAdd.Enabled = true;
        }

        private void ibRXm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibRYp_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 11;
            TimerAdd.Enabled = true;
        }

        private void ibRYp_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }

        private void ibRYm_MouseDown(object sender, MouseEventArgs e)
        {
            axisMode = 12;
            TimerAdd.Enabled = true;
        }

        private void ibRYm_MouseUp(object sender, MouseEventArgs e)
        {
            TimerAdd.Enabled = false;
        }
        #endregion

        //ended
        #region Controls
        private void comboBoxType_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainMenuForm.dataClass.FrameType = comboBoxType.SelectedIndex;
        }

        private void comboBoxTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = comboBoxTool.SelectedItem.ToString();
            int index = comboBoxTool.SelectedIndex;

            ToolC tc = MainMenuForm.Tools.Tool.Find(j => j.Name == name);

            MainMenuForm.Tools.SelectedTool = tc.Name;
            
            MainMenuForm.Tools.LenX = tc.X;
            MainMenuForm.Tools.LenY = tc.Y;
            MainMenuForm.Tools.LenZ = tc.Z + MainMenuForm.dataClass.A3;
        }

        private void iconButtonShutDown_Click(object sender, EventArgs e)
        {
            if (MainMenuForm.dataClass.GetConInfo)
                    MainMenuForm.dataClass.TurnOffRobot = true;
        }
        #endregion

        //ended
        #region AdingValues
        private void TimerAdd_Tick(object sender, EventArgs e)
        {
            double a = Convert.ToDouble(numericUpDownJumpValue.Value);
            double[] Jo = MainMenuForm.dataClass.Jo;
            double[] car = new double[3];
            int p = 0;
            foreach (double item in MainMenuForm.dataClass.Cor)
            {
                car[p] = item;
                p++;
            }
            double xl=0, yl=0, zl=0,rxl=0,ryl=0,rzl=0;


            a *= MainMenuForm.dataClass.VelScale;

            switch (axisMode)
            {
                case 1: car[2] += a; zl = a;
                    break;
                case 2:car[2] -= a; zl = a;
                    break;
                case 3:
                    car[0] +=  a; xl = a;
                    break;
                case 4:
                    car[0] -=  a; xl = a;
                    break;
                case 5:
                    car[1] += a; yl = a;
                    break;
                case 6:
                    car[1] -= a; yl = a;
                    break;
                case 7:
                    Jo[5] += a ; rzl = a;
                    break;
                case 8:
                    Jo[5] -= a ; rzl = a;
                    break;
                case 9:
                    Jo[3] += a ; rxl = a;
                    break;
                case 10:
                    Jo[3] -= a ; rxl = a;
                    break;
                case 11:
                    Jo[4] += a ; rxl = a;
                    break;
                case 12:
                    Jo[4] -= a ; ryl = a;
                    break;
                default:
                    break;
            }

            double[] rl = { rxl,ryl,rzl};

            double b = MainMenuForm.dataClass.A0;

            double x = MainMenuForm.Tools.LenX;
            double y = MainMenuForm.Tools.LenY;
            double z = MainMenuForm.Tools.LenZ;


            double toolVec = (x * x) + (y * y) + (z * z);

            double L = ((car[0]+x) * (car[0]+x)) + ((car[1] + y) * (car[1] + y));
            double res = (((car[2] - b + z) * (car[2] - b + z)) + L);
            double maxRange = (((MainMenuForm.dataClass.A1 * 2) * (MainMenuForm.dataClass.A1 * 2)) + toolVec); 
            if (res >= maxRange)
            {  
                TimerAdd.Enabled = false;   
                MessageBox.Show("Max range is "+ Math.Sqrt(maxRange).ToString() + ": your is: " + Math.Sqrt(res).ToString(), "Max Range exceeded");
            }
            else 
            {               
                MainMenuForm.dataClass.XL = xl;
                MainMenuForm.dataClass.Yl = yl;
                MainMenuForm.dataClass.ZL = zl;
                int g = 3;
                bool jl = true;
                for (int i = 7; i < 12; i+=2)
                {
                    if (Jo[g] < MainMenuForm.dataClass.JoLimits[i])
                    { TimerAdd.Enabled = false;
                        rl[g-3] = MainMenuForm.dataClass.JoLimits[i];
                        MessageBox.Show("Joint: " + (g + 1).ToString() + " limit " + MainMenuForm.dataClass.JoLimits[i].ToString(), "Max Range exceeded"); jl = false; }
                    else if (Jo[g] > MainMenuForm.dataClass.JoLimits[i-1])
                    { TimerAdd.Enabled = false; 
                         rl[g-3] = MainMenuForm.dataClass.JoLimits[i-1]; 
                        MessageBox.Show("Joint: " + (g + 1).ToString() + " limit " + MainMenuForm.dataClass.JoLimits[i].ToString(), "Min Range exceeded"); jl = false;
                    }
                    g++;
                }

                if (jl)
                {
                    MainMenuForm.dataClass.Cor = car;
                    MainMenuForm.dataClass.Jo = Jo;

                    MainMenuForm.dataClass.RxL = rl[0];
                    MainMenuForm.dataClass.RyL = rl[1];
                    MainMenuForm.dataClass.RzL = rl[2];

                    MainMenuForm.dataClass.Accuracy = Convert.ToDouble(numericUpDownAcuracy.Value);
                    MainMenuForm.dataClass.Speed = Convert.ToInt32(numericUpDownVelo.Value);
                    MainMenuForm.dataClass.Acceleration = Convert.ToInt32(nudAccel.Value);
                    MainMenuForm.dataClass.Deacceleration = Convert.ToInt32(nudDdeacel.Value);

                    MainMenuForm.dataClass.JumpVector = a;
                    MainMenuForm.dataClass.MovementType = MovemnetType;
                    MainMenuForm.dataClass.CurveShape = CurveShape;

                    nudX.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[0]);
                    nudY.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[1]);
                    nudZ.Value = Convert.ToDecimal(MainMenuForm.dataClass.Cor[2]);


                    nudJ4.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[3]);
                    nudJ5.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[4]);
                    nudJ6.Value = Convert.ToDecimal(MainMenuForm.dataClass.Jo[5]);


                    MainMenuForm.dataClass.ForceCount = true;
                    MainMenuForm.dataClass.UploadMath = true;
                }
            }
        }
        #endregion
    }
}
