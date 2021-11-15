using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRAC
{
    public partial class ParametersForm : Form
    {
        int countLog = 0;
        public ParametersForm()
        {
            InitializeComponent();
           
        }

        private void ParametersForm_Load(object sender, EventArgs e)
        {
            
            UpdateListBox();
            UpdateJointLimitsView();
            UpdateOffsetsValues();
        }

        private void UpdateOffsetsValues()
        {
            int[] offsets = MainMenuForm.dataClass.JointsOffsetValues;
            int o = 0;
            foreach (int item in offsets)
            {
                if (item > 10000) offsets[o] = -(item - 10000);
                o++;
            }

            numericUpDownJ1Offset.Value = Convert.ToDecimal(offsets[0]);
            numericUpDownJ2Offset.Value = Convert.ToDecimal(offsets[1]);
            numericUpDownJ3Offset.Value = Convert.ToDecimal(offsets[2]);
            numericUpDownJ4Offset.Value = Convert.ToDecimal(offsets[3]);
            numericUpDownJ5Offset.Value = Convert.ToDecimal(offsets[4]);
            numericUpDownJ6Offset.Value = Convert.ToDecimal(offsets[5]);
        }

        #region Functions
        private void UpdateJointLimitsView()
        {
            double[] lim = MainMenuForm.dataClass.JoLimits;


            j1x.Value = Convert.ToDecimal(lim[0]);
            j2x.Value = Convert.ToDecimal(lim[2]);
            j3x.Value = Convert.ToDecimal(lim[4]);
            j4x.Value = Convert.ToDecimal(lim[6]);
            j5x.Value = Convert.ToDecimal(lim[8]);
            j6x.Value = Convert.ToDecimal(lim[10]);

            j1n.Value = Convert.ToDecimal(lim[1]);
            j2n.Value = Convert.ToDecimal(lim[3]);
            j3n.Value = Convert.ToDecimal(lim[5]);
            j4n.Value = Convert.ToDecimal(lim[7]);
            j5n.Value = Convert.ToDecimal(lim[9]);
            j6n.Value = Convert.ToDecimal(lim[11]);
        }

        private void UpdateListBox()
        {
            listBoxTools.Items.Clear();
            foreach (var item in MainMenuForm.Tools.Tool)
            {
                listBoxTools.Items.Add(item.Name);
            }
        }
        #endregion

        #region Control
        private void iconButtonAddTool_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
           
            if (MainMenuForm.Tools.Tool.Exists(j => j.Name == name))
            {
                MessageBox.Show("Tool with this name already exists!","Tool Error",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            else
            {
                double x = Convert.ToDouble(numericUpDownX.Value);
                double y = Convert.ToDouble(numericUpDownY.Value);
                double z = Convert.ToDouble(numericUpDownZ.Value);

                MainMenuForm.Tools.Tool.Add(new ToolC {Name = name,X = x, Y=y , Z=z, Id = 0});
                UpdateListBox();
            }
            
        }
        
        private void iconButtonRemove_Click(object sender, EventArgs e)
        {
            int index = listBoxTools.SelectedIndex;
            if (index != -1)
            {
                var na = listBoxTools.SelectedItem;
                string name = na.ToString();
                if (name != "Default Tool")
                {
                    if (MainMenuForm.Tools.Tool.Exists(j => j.Name == name))
                    {
                        MainMenuForm.Tools.Tool.RemoveAt(index);
                        UpdateListBox();
                    }
                    else
                    {
                        MessageBox.Show("Tool deesn't exists!", "Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("You can't deleate Default Tool", "Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }


            }
            else
            {
                MessageBox.Show("Select what do you want to remove!", "Tool Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void iconButtonSaveJ_Click(object sender, EventArgs e)
        {
            double[] Max = new double[12];

            Max[0] = Convert.ToDouble(j1x.Value);
            Max[2] = Convert.ToDouble(j2x.Value);
            Max[4] = Convert.ToDouble(j3x.Value);
            Max[6] = Convert.ToDouble(j4x.Value);
            Max[8] = Convert.ToDouble(j5x.Value);
            Max[10] = Convert.ToDouble(j6x.Value);

            Max[1] = Convert.ToDouble(j1n.Value);
            Max[3] = Convert.ToDouble(j2n.Value);
            Max[5] = Convert.ToDouble(j3n.Value);
            Max[7] = Convert.ToDouble(j4n.Value);
            Max[9] = Convert.ToDouble(j5n.Value);
            Max[11] = Convert.ToDouble(j6n.Value);

            if(MainMenuForm.dataClass.Threads)
            while (MainMenuForm.dataClass.UpdateSetup)
            { int i = 0; }

            MainMenuForm.dataClass.JoLimits = Max;
            MainMenuForm.dataClass.UpdateSetup = true;

        }

        private void listBoxTools_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index= listBoxTools.SelectedIndex;
            if (index != -1)
            {
                ToolC tc = MainMenuForm.Tools.Tool.ElementAt(index);
                textBoxName.Text = tc.Name;
                numericUpDownX.Value = Convert.ToDecimal(tc.X);
                numericUpDownY.Value = Convert.ToDecimal(tc.Y);
                numericUpDownZ.Value = Convert.ToDecimal(tc.Z);
            }
            

           


        }
        #endregion

        private void timerUpdateLog_Tick(object sender, EventArgs e)
        {
            int i = 0;
            if(MainMenuForm.errorListNumerous!= null)
            if (countLog < MainMenuForm.errorListNumerous.Count)
            {
                listBoxErrorList.Items.Clear();
                while (i < MainMenuForm.errorListNumerous.Count)
                {
                    var ad = MainMenuForm.ErrorExplenationRobot(MainMenuForm.errorListNumerous[i], true);
                    i++;
                    listBoxErrorList.Items.Add(ad);
                }
                countLog = i;
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            int[] data = new int[6];

            data[0] = Convert.ToInt32(numericUpDownJ1Offset.Value);
            data[1] = Convert.ToInt32(numericUpDownJ2Offset.Value);
            data[2] = Convert.ToInt32(numericUpDownJ3Offset.Value);
            data[3] = Convert.ToInt32(numericUpDownJ4Offset.Value);
            data[4] = Convert.ToInt32(numericUpDownJ5Offset.Value);
            data[5] = Convert.ToInt32(numericUpDownJ6Offset.Value);

            int p = 0;
            foreach (int item in data)
            {
                if (item < 0) data[p] = Math.Abs(item) + 10000;
                p++;
            }

            MainMenuForm.dataClass.JointsOffsetValues = data;
            MainMenuForm.dataClass.UpdateOffset = true;
        }
    }
}
