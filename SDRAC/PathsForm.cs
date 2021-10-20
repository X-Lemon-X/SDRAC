using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SDRAC
{
    public partial class PathsForm : Form
    {
        #region veriables
        List<string> listS = new List<string> { };
        List<long> listL = new List<long> { };
        #endregion

        #region Start of the form
        public PathsForm()
        {
            InitializeComponent();
            textBoxDirectory.Text = MainMenuForm.movementWayPath.Path;
            if (MainMenuForm.movementWayPath.Path != null)
            {
                UpdateListBoxView(listBoxPoints);
            }
        }
        #endregion

        //ended
        #region File

        private bool UpdateListBoxView(ListBox listBox)
        {

            FileFun ff = new FileFun();
            bool ret = false;
            try
            {
                if (System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
                {

                    listBox.Items.Clear();

                    string readed;
                    string pathMain = MainMenuForm.movementWayPath.Path;
                    long beg = 0, end = 0, lines = 0, points=0;

                    var unsignedListBegins = new List<long> { };
                    var unsignedListBinfo = new List<string> { };


                    using (StreamReader reader = new StreamReader(pathMain))
                    {
                        string inside;
                        while (reader.Peek() != -1)
                        {
                            readed = reader.ReadLine();

                            if (readed.Length >= 2)
                            {
                                inside = readed;
                                if (inside.Substring(0, 2) == "G0")
                                {


                                    string[] add = inside.Split('ǿ');
                                    if (add.Length >= 7)
                                    {
                                        string itemToAdd = null;

                                        itemToAdd += "[" + beg.ToString() + "]-->";
                                        itemToAdd += add[1] + " ";
                                        itemToAdd += add[2] + " ";
                                        itemToAdd += add[3] + " ";

                                        itemToAdd += add[4] + " ";
                                        itemToAdd += add[5] + " ";
                                        itemToAdd += add[6];

                                        foreach (string item in add)
                                        {

                                            switch (item)
                                            {
                                                case "G5": itemToAdd += "   #[RESET]";
                                                    break;
                                                case "G80":
                                                    itemToAdd += "   Movement: Vector";
                                                    break;
                                                case "G81":
                                                    itemToAdd += "   Movement: V.R";
                                                    break;
                                                case "G82":
                                                    itemToAdd += "   Movement: Joint";
                                                    break;
                                                case "G70":
                                                    itemToAdd += "   Mode: Manual";
                                                    break;
                                                case "G71":
                                                    itemToAdd += "   Mode: Auto";
                                                    break;                                        
                                            }
                           
                                        }

                                        listBox.Items.Add(itemToAdd);

                                        unsignedListBegins.Add(lines);
                                        unsignedListBinfo.Add(readed);

                                        ret = true;

                                        beg++;
                                    }
                                    else
                                    {
                                        ret = false;
                                    }


                                }

                                inside = readed;

                                if (inside.Substring(0, 2) == "G4") end++;
                               

                                inside = readed;
                                if (inside.Substring(0,2) == "G2") points++;
                              
                            }

                            lines++;
                        }

                        

                        

                        labelPoints.Text = (points).ToString();
                        labelPos.Text = (beg).ToString();
                        

                        listL = unsignedListBegins;
                        listS = unsignedListBinfo;

                    }

                    if (lines == 0) {listBox.Items.Add("File is empty"); ret = true; }
                    else
                    {                  
                        FileInfo fileInfo = new FileInfo(pathMain);
                        labelFileSize.Text = (fileInfo.Length / 1000).ToString() + "kb";
                    }

                    if (!ret)
                    {
                        MessageBox.Show("Encoding problem", "Problew with a file");
                    }

                }
                else
                {
                    ret = false;
                    MessageBox.Show("File isn't set or exist", "Problew with a file");
                }


            }
            catch (Exception)
            {
                ret = false;
                MessageBox.Show("File Reading problem.", "Problew with a file");
            }

            return ret;
        }

        private void RemoveSelectedItem(ListBox listbox)
        {
            FileFun ff = new FileFun();

            try
            {
                int selind = listbox.SelectedIndex;
                if (selind != -1 && listbox.SelectedItem != "File is empty")
                {

                    if (System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
                    {

                        string readed;
                        string pathMain = MainMenuForm.movementWayPath.Path;
                        long beg = 0, lines = 0;


                        var unsignedListBegins = listL;
                        var unsignedListBinfo = listS;

                        string pathTemp = ff.CreateTempFile("TempLoad", Application.ExecutablePath, Application.ProductName);
                        
                        using (StreamReader reader = new StreamReader(pathMain))
                        {
                            using (StreamWriter writer = new StreamWriter(pathTemp))
                            {
                                string inside; bool write = true;

                                while (reader.Peek() != -1)
                                {
                                    readed = reader.ReadLine();

                                    if (readed.Length >= 2)
                                    {
                                        inside = readed;

                                        if (inside.Substring(0, 2) == "G0")
                                        {
                                            if (beg == selind) write = false;                                                                                                                             
                                            beg++;                                   
                                        }

                                        inside = readed;

                                        if (write) writer.WriteLine(readed);
                                                   
                                        if (inside.Substring(0, 2) == "G4" && !write) write = true;

                                    }

                                    lines++;
                                }
                            }
                           
                        }


                        ff.CopyFromOneFileToAnother(pathTemp, pathMain);

                        File.Delete(pathTemp);
                        
                    }
                    else
                    {
                        MessageBox.Show("File isn't set or exist", "Problew with a file");
                    }
                }
                else
                {
                    MessageBox.Show("Select what you want to remove.","",MessageBoxButtons.OK,MessageBoxIcon.Information);
                }

            }
            catch (Exception)
            {

                MessageBox.Show("File Reading problem.", "Problew with a file");
            }
        }

        #endregion

        //ended
        #region BTN and listBox
        private void iconButtonOpenFile_Click(object sender, EventArgs e)
        {
            // openFileDialog.InitialDirectory = CheckPath();
            
            dialog.ValidateNames = true;
            dialog.CheckFileExists = true;
            dialog.FileName = "";
            dialog.Filter = "Text files (*.txt)|*.txt";
            dialog.ShowDialog();


            string path = dialog.FileName;

            if (path != "" & path.Length >4)
            {
                if (path.Substring(path.Length - 4) == ".txt")
                {
                    var onlyFileName = System.IO.Path.GetFileName(dialog.FileName);
                    MainMenuForm.movementWayPath.Path = path;              
                    MainMenuForm.movementWayPath.FileName = onlyFileName;
                    textBoxDirectory.Text = path;
                }   
            }
            

             
        }

        private void iconButtonFileNew_Click(object sender, EventArgs e)
        {
            dialog.ValidateNames = false; 
            dialog.CheckFileExists = false;
            dialog.FileName = "SetFileName";
            dialog.Filter = null;
            dialog.ShowDialog();

            FileFun ff = new FileFun();

            var onlyFileName = System.IO.Path.GetFileName(dialog.FileName);
            string path = dialog.FileName;
            path = path.Substring(0, path.Length - onlyFileName.Length);
            string name=onlyFileName;

            if (name == "SetFileName" || name == "" || name == null) name = "RobotPath";
       

            if (path != null)
            {
                path = ff.CreateFile(name, path);
                MainMenuForm.movementWayPath.Path = path;
                MainMenuForm.movementWayPath.FileName = name;
                textBoxDirectory.Text = path;
                UpdateListBoxView(listBoxPoints);
            }
            
        }

        private void iconButtonRemove_Click(object sender, EventArgs e)
        {           
                RemoveSelectedItem(listBoxPoints);
                UpdateListBoxView(listBoxPoints);                     
        }    

        private void iconButtonLoad_Click(object sender, EventArgs e)
        {
            bool con = UpdateListBoxView(listBoxPoints);
        }

        private void iconButtonEdit_Click(object sender, EventArgs e)
        {
            bool yes;
            int item =0;
            int selind  = listBoxPoints.SelectedIndex;

            if (selind != -1 && listBoxPoints.SelectedItem != "File is empty")
            {
                try
                {
                    var itemek = listL[selind];
                    yes = true;
                    item = Convert.ToInt32(itemek);
                }
                catch (Exception)
                {

                    yes = false;
                }

                if (yes)
                {
                    MainMenuForm.movementWayPath.Mode = 2;
                    MainMenuForm.movementWayPath.IndexLb = item;
                    MainMenuForm.movementWayPath.EditPoint = true;
                }
            }
            else
            {
                MessageBox.Show("Select what you want to remove.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void listBoxPoints_SelectedIndexChanged(object sender, EventArgs e)
        {
            iconButtonAdd.Enabled = true; 
            iconButtonRemove.Enabled = true;
            iconButtonEdit.Enabled = true;
        }

        private void iconButtonAdd_Click(object sender, EventArgs e)
        {
            bool yes;
            int item = 0;
            int selind = listBoxPoints.SelectedIndex;

            if (selind != -1 && listBoxPoints.SelectedItem != "File is empty")
            {
                try
                {
                    var itemek = listL[selind];
                    yes = true;
                    item = Convert.ToInt32(itemek);
                }
                catch (Exception)
                {

                    yes = false;
                }

                if (yes)
                {
                    byte mode = 0;
                    if (radioButtonOver.Checked)
                    {
                        mode = 3;
                    }
                    else if (radioButtonUnder.Checked)
                    {
                        mode = 4;
                    }

                    MainMenuForm.movementWayPath.Mode = mode;
                    MainMenuForm.movementWayPath.IndexLb = item;
                    MainMenuForm.movementWayPath.EditPoint = true;

                }
            }
            else
            {
                MessageBox.Show("Select what you want to remove.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void iconButtonRun_Click(object sender, EventArgs e)
        {
            bool con = false;
            if (radioButtonRB.Checked) con = true;

            if (radioButtonRS.Checked && listBoxPoints.SelectedIndex != -1) con = true;

            bool con2 = UpdateListBoxView(listBoxPoints);
            if (con2 && con && MainMenuForm.movementWayPath.Path != null && System.IO.File.Exists(MainMenuForm.movementWayPath.Path))
            {      
                    if (MainMenuForm.movementWayPath.Run) MainMenuForm.movementWayPath.Run = false;
                                 
                    if (radioButtonRB.Checked)
                    {
                        MainMenuForm.movementWayPath.SkipLines = false;     
                    }
                    else if (radioButtonRS.Checked)
                    {
                        MainMenuForm.movementWayPath.SkipLines = true;                 
                    }
                   
                 MainMenuForm.movementWayPath.RunFrom = true;
              
                MainMenuForm.movementWayPath.StopSP = true;
            }            
        }
        #endregion
    }
}
