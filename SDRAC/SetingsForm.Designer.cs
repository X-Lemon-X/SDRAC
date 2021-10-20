namespace SDRAC
{
    partial class SetingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerSerialPortChrck = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label18 = new System.Windows.Forms.Label();
            this.radioButtonUSB = new System.Windows.Forms.RadioButton();
            this.radioButtonEthernet = new System.Windows.Forms.RadioButton();
            this.radioButtonLan = new System.Windows.Forms.RadioButton();
            this.iconButtonAutoPort = new FontAwesome.Sharp.IconButton();
            this.iconButtonManual = new FontAwesome.Sharp.IconButton();
            this.labelPortname = new System.Windows.Forms.Label();
            this.iconButtonOpenFile = new FontAwesome.Sharp.IconButton();
            this.listViewPorts = new System.Windows.Forms.ListBox();
            this.labelTextPort = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iconButtonFileNew = new FontAwesome.Sharp.IconButton();
            this.iconButtonSave = new FontAwesome.Sharp.IconButton();
            this.ip1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ip2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ip3 = new System.Windows.Forms.TextBox();
            this.ip4 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PanelWiFILanEth = new System.Windows.Forms.Panel();
            this.iconButtonSet = new FontAwesome.Sharp.IconButton();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.tbport = new System.Windows.Forms.TextBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.tbpass = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.g1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.g4 = new System.Windows.Forms.TextBox();
            this.g2 = new System.Windows.Forms.TextBox();
            this.g3 = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.tbssid = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.m1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.m4 = new System.Windows.Forms.TextBox();
            this.m2 = new System.Windows.Forms.TextBox();
            this.m3 = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.UpdateInfo = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.PanelWiFILanEth.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerSerialPortChrck
            // 
            this.timerSerialPortChrck.Interval = 1000;
            this.timerSerialPortChrck.Tick += new System.EventHandler(this.timerSerialPortCOMCheck_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.radioButtonUSB);
            this.panel1.Controls.Add(this.radioButtonEthernet);
            this.panel1.Controls.Add(this.radioButtonLan);
            this.panel1.Controls.Add(this.iconButtonAutoPort);
            this.panel1.Controls.Add(this.iconButtonManual);
            this.panel1.Controls.Add(this.labelPortname);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 551);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(964, 53);
            this.panel1.TabIndex = 1;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label18.ForeColor = System.Drawing.Color.DarkGray;
            this.label18.Location = new System.Drawing.Point(558, 11);
            this.label18.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(169, 26);
            this.label18.TabIndex = 24;
            this.label18.Text = "Conection Type:";
            // 
            // radioButtonUSB
            // 
            this.radioButtonUSB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonUSB.AutoSize = true;
            this.radioButtonUSB.Enabled = false;
            this.radioButtonUSB.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.radioButtonUSB.ForeColor = System.Drawing.Color.Maroon;
            this.radioButtonUSB.Location = new System.Drawing.Point(730, 15);
            this.radioButtonUSB.Name = "radioButtonUSB";
            this.radioButtonUSB.Size = new System.Drawing.Size(57, 22);
            this.radioButtonUSB.TabIndex = 16;
            this.radioButtonUSB.Text = "USB";
            this.radioButtonUSB.UseVisualStyleBackColor = true;
            this.radioButtonUSB.CheckedChanged += new System.EventHandler(this.radioButtonUSB_CheckedChanged);
            // 
            // radioButtonEthernet
            // 
            this.radioButtonEthernet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonEthernet.AutoSize = true;
            this.radioButtonEthernet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.radioButtonEthernet.ForeColor = System.Drawing.Color.DarkViolet;
            this.radioButtonEthernet.Location = new System.Drawing.Point(880, 15);
            this.radioButtonEthernet.Name = "radioButtonEthernet";
            this.radioButtonEthernet.Size = new System.Drawing.Size(81, 22);
            this.radioButtonEthernet.TabIndex = 15;
            this.radioButtonEthernet.Text = "Ethernet";
            this.radioButtonEthernet.UseVisualStyleBackColor = true;
            this.radioButtonEthernet.CheckedChanged += new System.EventHandler(this.radioButtonEthernet_CheckedChanged);
            // 
            // radioButtonLan
            // 
            this.radioButtonLan.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.radioButtonLan.AutoSize = true;
            this.radioButtonLan.Checked = true;
            this.radioButtonLan.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.radioButtonLan.ForeColor = System.Drawing.Color.OrangeRed;
            this.radioButtonLan.Location = new System.Drawing.Point(793, 15);
            this.radioButtonLan.Name = "radioButtonLan";
            this.radioButtonLan.Size = new System.Drawing.Size(84, 22);
            this.radioButtonLan.TabIndex = 14;
            this.radioButtonLan.TabStop = true;
            this.radioButtonLan.Text = "Lan/WiFi";
            this.radioButtonLan.UseVisualStyleBackColor = true;
            this.radioButtonLan.CheckedChanged += new System.EventHandler(this.radioButtonLan_CheckedChanged);
            // 
            // iconButtonAutoPort
            // 
            this.iconButtonAutoPort.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonAutoPort.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconButtonAutoPort.FlatAppearance.BorderSize = 0;
            this.iconButtonAutoPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonAutoPort.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonAutoPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonAutoPort.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonAutoPort.IconChar = FontAwesome.Sharp.IconChar.Adn;
            this.iconButtonAutoPort.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonAutoPort.IconSize = 32;
            this.iconButtonAutoPort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonAutoPort.Location = new System.Drawing.Point(145, 0);
            this.iconButtonAutoPort.Name = "iconButtonAutoPort";
            this.iconButtonAutoPort.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonAutoPort.Rotation = 0D;
            this.iconButtonAutoPort.Size = new System.Drawing.Size(128, 53);
            this.iconButtonAutoPort.TabIndex = 7;
            this.iconButtonAutoPort.Text = "  Auto";
            this.iconButtonAutoPort.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonAutoPort.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonAutoPort.UseVisualStyleBackColor = false;
            this.iconButtonAutoPort.Click += new System.EventHandler(this.iconButtonAuto_Click);
            // 
            // iconButtonManual
            // 
            this.iconButtonManual.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonManual.Dock = System.Windows.Forms.DockStyle.Left;
            this.iconButtonManual.FlatAppearance.BorderSize = 0;
            this.iconButtonManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonManual.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonManual.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonManual.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonManual.IconChar = FontAwesome.Sharp.IconChar.HandPaper;
            this.iconButtonManual.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonManual.IconSize = 32;
            this.iconButtonManual.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonManual.Location = new System.Drawing.Point(0, 0);
            this.iconButtonManual.Name = "iconButtonManual";
            this.iconButtonManual.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonManual.Rotation = 0D;
            this.iconButtonManual.Size = new System.Drawing.Size(145, 53);
            this.iconButtonManual.TabIndex = 8;
            this.iconButtonManual.Text = "  Manual";
            this.iconButtonManual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonManual.UseVisualStyleBackColor = false;
            this.iconButtonManual.Click += new System.EventHandler(this.iconButtonManual_Click);
            // 
            // labelPortname
            // 
            this.labelPortname.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPortname.AutoSize = true;
            this.labelPortname.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelPortname.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPortname.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.labelPortname.Location = new System.Drawing.Point(278, 11);
            this.labelPortname.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPortname.Name = "labelPortname";
            this.labelPortname.Size = new System.Drawing.Size(0, 24);
            this.labelPortname.TabIndex = 13;
            // 
            // iconButtonOpenFile
            // 
            this.iconButtonOpenFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonOpenFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonOpenFile.FlatAppearance.BorderSize = 0;
            this.iconButtonOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonOpenFile.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonOpenFile.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonOpenFile.IconChar = FontAwesome.Sharp.IconChar.File;
            this.iconButtonOpenFile.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonOpenFile.IconSize = 32;
            this.iconButtonOpenFile.Location = new System.Drawing.Point(673, 68);
            this.iconButtonOpenFile.Name = "iconButtonOpenFile";
            this.iconButtonOpenFile.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonOpenFile.Rotation = 0D;
            this.iconButtonOpenFile.Size = new System.Drawing.Size(280, 53);
            this.iconButtonOpenFile.TabIndex = 9;
            this.iconButtonOpenFile.Text = "  OpenSetupFile";
            this.iconButtonOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButtonOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonOpenFile.UseVisualStyleBackColor = false;
            this.iconButtonOpenFile.Click += new System.EventHandler(this.iconButtonOpenFile_Click);
            // 
            // listViewPorts
            // 
            this.listViewPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewPorts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.listViewPorts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listViewPorts.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listViewPorts.ForeColor = System.Drawing.Color.DarkGray;
            this.listViewPorts.FormattingEnabled = true;
            this.listViewPorts.ItemHeight = 22;
            this.listViewPorts.Items.AddRange(new object[] {
            " "});
            this.listViewPorts.Location = new System.Drawing.Point(11, 172);
            this.listViewPorts.Margin = new System.Windows.Forms.Padding(2);
            this.listViewPorts.Name = "listViewPorts";
            this.listViewPorts.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listViewPorts.Size = new System.Drawing.Size(262, 374);
            this.listViewPorts.TabIndex = 2;
            this.listViewPorts.SelectedIndexChanged += new System.EventHandler(this.listViewPorts_SelectedIndexChanged);
            // 
            // labelTextPort
            // 
            this.labelTextPort.AutoSize = true;
            this.labelTextPort.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelTextPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelTextPort.ForeColor = System.Drawing.Color.DarkGray;
            this.labelTextPort.Location = new System.Drawing.Point(11, 141);
            this.labelTextPort.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelTextPort.Name = "labelTextPort";
            this.labelTextPort.Size = new System.Drawing.Size(210, 26);
            this.labelTextPort.TabIndex = 3;
            this.labelTextPort.Text = "List of avilable ports:";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "SetupSDRAC.txt";
            this.openFileDialog.Filter = "(*.txt)|";
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBoxDirectory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxDirectory.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxDirectory.Location = new System.Drawing.Point(14, 37);
            this.textBoxDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDirectory.Multiline = true;
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.Size = new System.Drawing.Size(939, 26);
            this.textBoxDirectory.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(9, 7);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 28);
            this.label1.TabIndex = 10;
            this.label1.Text = "SetupFile";
            // 
            // iconButtonFileNew
            // 
            this.iconButtonFileNew.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonFileNew.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonFileNew.FlatAppearance.BorderSize = 0;
            this.iconButtonFileNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonFileNew.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonFileNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonFileNew.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonFileNew.IconChar = FontAwesome.Sharp.IconChar.FileMedical;
            this.iconButtonFileNew.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonFileNew.IconSize = 32;
            this.iconButtonFileNew.Location = new System.Drawing.Point(300, 68);
            this.iconButtonFileNew.Name = "iconButtonFileNew";
            this.iconButtonFileNew.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonFileNew.Rotation = 0D;
            this.iconButtonFileNew.Size = new System.Drawing.Size(366, 53);
            this.iconButtonFileNew.TabIndex = 11;
            this.iconButtonFileNew.Text = "  New File";
            this.iconButtonFileNew.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButtonFileNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonFileNew.UseVisualStyleBackColor = false;
            this.iconButtonFileNew.Click += new System.EventHandler(this.iconButtonFileNew_Click);
            // 
            // iconButtonSave
            // 
            this.iconButtonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonSave.FlatAppearance.BorderSize = 0;
            this.iconButtonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonSave.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonSave.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonSave.IconChar = FontAwesome.Sharp.IconChar.Save;
            this.iconButtonSave.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonSave.IconSize = 32;
            this.iconButtonSave.Location = new System.Drawing.Point(14, 68);
            this.iconButtonSave.Name = "iconButtonSave";
            this.iconButtonSave.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonSave.Rotation = 0D;
            this.iconButtonSave.Size = new System.Drawing.Size(280, 53);
            this.iconButtonSave.TabIndex = 15;
            this.iconButtonSave.Text = "  Save Setup";
            this.iconButtonSave.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.iconButtonSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonSave.UseVisualStyleBackColor = false;
            this.iconButtonSave.Click += new System.EventHandler(this.iconButtonSave_Click);
            // 
            // ip1
            // 
            this.ip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ip1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ip1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.ip1.ForeColor = System.Drawing.Color.White;
            this.ip1.Location = new System.Drawing.Point(96, 10);
            this.ip1.MaxLength = 3;
            this.ip1.Name = "ip1";
            this.ip1.Size = new System.Drawing.Size(40, 25);
            this.ip1.TabIndex = 16;
            this.ip1.Text = "0";
            this.ip1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ip1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(142, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(19, 25);
            this.label2.TabIndex = 17;
            this.label2.Text = ".";
            // 
            // ip2
            // 
            this.ip2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ip2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ip2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.ip2.ForeColor = System.Drawing.Color.White;
            this.ip2.Location = new System.Drawing.Point(167, 10);
            this.ip2.MaxLength = 3;
            this.ip2.Name = "ip2";
            this.ip2.Size = new System.Drawing.Size(40, 25);
            this.ip2.TabIndex = 18;
            this.ip2.Text = "0";
            this.ip2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ip2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip2_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(213, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(19, 25);
            this.label3.TabIndex = 19;
            this.label3.Text = ".";
            // 
            // ip3
            // 
            this.ip3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ip3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ip3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.ip3.ForeColor = System.Drawing.Color.White;
            this.ip3.Location = new System.Drawing.Point(238, 10);
            this.ip3.MaxLength = 3;
            this.ip3.Name = "ip3";
            this.ip3.Size = new System.Drawing.Size(40, 25);
            this.ip3.TabIndex = 20;
            this.ip3.Text = "0";
            this.ip3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ip3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip3_KeyPress);
            // 
            // ip4
            // 
            this.ip4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ip4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ip4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.ip4.ForeColor = System.Drawing.Color.White;
            this.ip4.Location = new System.Drawing.Point(309, 10);
            this.ip4.MaxLength = 3;
            this.ip4.Name = "ip4";
            this.ip4.Size = new System.Drawing.Size(40, 25);
            this.ip4.TabIndex = 21;
            this.ip4.Text = "0";
            this.ip4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ip4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ip4_KeyPress);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(284, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 25);
            this.label4.TabIndex = 22;
            this.label4.Text = ".";
            // 
            // PanelWiFILanEth
            // 
            this.PanelWiFILanEth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PanelWiFILanEth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.PanelWiFILanEth.Controls.Add(this.iconButtonSet);
            this.PanelWiFILanEth.Controls.Add(this.panel8);
            this.PanelWiFILanEth.Controls.Add(this.panel7);
            this.PanelWiFILanEth.Controls.Add(this.panel5);
            this.PanelWiFILanEth.Controls.Add(this.panel6);
            this.PanelWiFILanEth.Controls.Add(this.panel4);
            this.PanelWiFILanEth.Controls.Add(this.panel3);
            this.PanelWiFILanEth.Controls.Add(this.label6);
            this.PanelWiFILanEth.Location = new System.Drawing.Point(563, 141);
            this.PanelWiFILanEth.Name = "PanelWiFILanEth";
            this.PanelWiFILanEth.Size = new System.Drawing.Size(401, 405);
            this.PanelWiFILanEth.TabIndex = 23;
            this.PanelWiFILanEth.Visible = false;
            // 
            // iconButtonSet
            // 
            this.iconButtonSet.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.iconButtonSet.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.iconButtonSet.FlatAppearance.BorderSize = 0;
            this.iconButtonSet.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonSet.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonSet.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonSet.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonSet.IconChar = FontAwesome.Sharp.IconChar.Tasks;
            this.iconButtonSet.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonSet.IconSize = 32;
            this.iconButtonSet.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonSet.Location = new System.Drawing.Point(145, 346);
            this.iconButtonSet.Name = "iconButtonSet";
            this.iconButtonSet.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonSet.Rotation = 0D;
            this.iconButtonSet.Size = new System.Drawing.Size(124, 53);
            this.iconButtonSet.TabIndex = 24;
            this.iconButtonSet.Text = "  Set";
            this.iconButtonSet.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonSet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonSet.UseVisualStyleBackColor = false;
            this.iconButtonSet.Click += new System.EventHandler(this.iconButtonSet_Click);
            // 
            // panel8
            // 
            this.panel8.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel8.Controls.Add(this.label17);
            this.panel8.Controls.Add(this.tbport);
            this.panel8.Location = new System.Drawing.Point(20, 193);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(358, 45);
            this.panel8.TabIndex = 31;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.Color.White;
            this.label17.Location = new System.Drawing.Point(13, 10);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(76, 25);
            this.label17.TabIndex = 18;
            this.label17.Text = "PORT:";
            // 
            // tbport
            // 
            this.tbport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tbport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbport.ForeColor = System.Drawing.Color.White;
            this.tbport.Location = new System.Drawing.Point(94, 10);
            this.tbport.MaxLength = 25;
            this.tbport.Name = "tbport";
            this.tbport.Size = new System.Drawing.Size(253, 22);
            this.tbport.TabIndex = 16;
            this.tbport.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbport.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbport_KeyPress);
            // 
            // panel7
            // 
            this.panel7.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel7.Controls.Add(this.label16);
            this.panel7.Controls.Add(this.tbpass);
            this.panel7.Location = new System.Drawing.Point(20, 295);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(358, 45);
            this.panel7.TabIndex = 30;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(11, 11);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(74, 25);
            this.label16.TabIndex = 18;
            this.label16.Text = "PASS:";
            // 
            // tbpass
            // 
            this.tbpass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tbpass.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbpass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbpass.ForeColor = System.Drawing.Color.White;
            this.tbpass.Location = new System.Drawing.Point(94, 10);
            this.tbpass.MaxLength = 25;
            this.tbpass.Name = "tbpass";
            this.tbpass.Size = new System.Drawing.Size(253, 22);
            this.tbpass.TabIndex = 16;
            this.tbpass.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel5
            // 
            this.panel5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel5.Controls.Add(this.label11);
            this.panel5.Controls.Add(this.g1);
            this.panel5.Controls.Add(this.label12);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Controls.Add(this.g4);
            this.panel5.Controls.Add(this.g2);
            this.panel5.Controls.Add(this.g3);
            this.panel5.Controls.Add(this.label14);
            this.panel5.Location = new System.Drawing.Point(20, 139);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(358, 45);
            this.panel5.TabIndex = 29;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.White;
            this.label11.Location = new System.Drawing.Point(10, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 25);
            this.label11.TabIndex = 18;
            this.label11.Text = "GATE:";
            // 
            // g1
            // 
            this.g1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.g1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.g1.Enabled = false;
            this.g1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.g1.ForeColor = System.Drawing.Color.White;
            this.g1.Location = new System.Drawing.Point(94, 10);
            this.g1.MaxLength = 3;
            this.g1.Name = "g1";
            this.g1.Size = new System.Drawing.Size(40, 25);
            this.g1.TabIndex = 16;
            this.g1.Text = "0";
            this.g1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.g1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.g1_KeyPress);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.White;
            this.label12.Location = new System.Drawing.Point(282, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(19, 25);
            this.label12.TabIndex = 22;
            this.label12.Text = ".";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.White;
            this.label13.Location = new System.Drawing.Point(140, 10);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(19, 25);
            this.label13.TabIndex = 17;
            this.label13.Text = ".";
            // 
            // g4
            // 
            this.g4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.g4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.g4.Enabled = false;
            this.g4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.g4.ForeColor = System.Drawing.Color.White;
            this.g4.Location = new System.Drawing.Point(307, 10);
            this.g4.MaxLength = 3;
            this.g4.Name = "g4";
            this.g4.Size = new System.Drawing.Size(40, 25);
            this.g4.TabIndex = 21;
            this.g4.Text = "0";
            this.g4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.g4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.g4_KeyPress);
            // 
            // g2
            // 
            this.g2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.g2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.g2.Enabled = false;
            this.g2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.g2.ForeColor = System.Drawing.Color.White;
            this.g2.Location = new System.Drawing.Point(165, 10);
            this.g2.MaxLength = 3;
            this.g2.Name = "g2";
            this.g2.Size = new System.Drawing.Size(40, 25);
            this.g2.TabIndex = 18;
            this.g2.Text = "0";
            this.g2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.g2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.g2_KeyPress);
            // 
            // g3
            // 
            this.g3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.g3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.g3.Enabled = false;
            this.g3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.g3.ForeColor = System.Drawing.Color.White;
            this.g3.Location = new System.Drawing.Point(236, 10);
            this.g3.MaxLength = 3;
            this.g3.Name = "g3";
            this.g3.Size = new System.Drawing.Size(40, 25);
            this.g3.TabIndex = 20;
            this.g3.Text = "0";
            this.g3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.g3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.g3_KeyPress);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(211, 10);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(19, 25);
            this.label14.TabIndex = 19;
            this.label14.Text = ".";
            // 
            // panel6
            // 
            this.panel6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel6.Controls.Add(this.label15);
            this.panel6.Controls.Add(this.tbssid);
            this.panel6.Location = new System.Drawing.Point(20, 244);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(358, 45);
            this.panel6.TabIndex = 28;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.White;
            this.label15.Location = new System.Drawing.Point(13, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(66, 25);
            this.label15.TabIndex = 18;
            this.label15.Text = "SSID:";
            // 
            // tbssid
            // 
            this.tbssid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.tbssid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbssid.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbssid.ForeColor = System.Drawing.Color.White;
            this.tbssid.Location = new System.Drawing.Point(94, 10);
            this.tbssid.MaxLength = 25;
            this.tbssid.Name = "tbssid";
            this.tbssid.Size = new System.Drawing.Size(253, 22);
            this.tbssid.TabIndex = 16;
            this.tbssid.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // panel4
            // 
            this.panel4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.m1);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.m4);
            this.panel4.Controls.Add(this.m2);
            this.panel4.Controls.Add(this.m3);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(20, 88);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(358, 45);
            this.panel4.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(10, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(78, 25);
            this.label7.TabIndex = 18;
            this.label7.Text = "MASK:";
            // 
            // m1
            // 
            this.m1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.m1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m1.Enabled = false;
            this.m1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.m1.ForeColor = System.Drawing.Color.White;
            this.m1.Location = new System.Drawing.Point(94, 10);
            this.m1.MaxLength = 3;
            this.m1.Name = "m1";
            this.m1.Size = new System.Drawing.Size(40, 25);
            this.m1.TabIndex = 16;
            this.m1.Text = "0";
            this.m1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m1_KeyPress);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.White;
            this.label8.Location = new System.Drawing.Point(282, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(19, 25);
            this.label8.TabIndex = 22;
            this.label8.Text = ".";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(140, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(19, 25);
            this.label9.TabIndex = 17;
            this.label9.Text = ".";
            // 
            // m4
            // 
            this.m4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.m4.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m4.Enabled = false;
            this.m4.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.m4.ForeColor = System.Drawing.Color.White;
            this.m4.Location = new System.Drawing.Point(307, 10);
            this.m4.MaxLength = 3;
            this.m4.Name = "m4";
            this.m4.Size = new System.Drawing.Size(40, 25);
            this.m4.TabIndex = 21;
            this.m4.Text = "0";
            this.m4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m4.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m4_KeyPress);
            // 
            // m2
            // 
            this.m2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.m2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m2.Enabled = false;
            this.m2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.m2.ForeColor = System.Drawing.Color.White;
            this.m2.Location = new System.Drawing.Point(165, 10);
            this.m2.MaxLength = 3;
            this.m2.Name = "m2";
            this.m2.Size = new System.Drawing.Size(40, 25);
            this.m2.TabIndex = 18;
            this.m2.Text = "0";
            this.m2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m2_KeyPress);
            // 
            // m3
            // 
            this.m3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.m3.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.m3.Enabled = false;
            this.m3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F);
            this.m3.ForeColor = System.Drawing.Color.White;
            this.m3.Location = new System.Drawing.Point(236, 10);
            this.m3.MaxLength = 3;
            this.m3.Name = "m3";
            this.m3.Size = new System.Drawing.Size(40, 25);
            this.m3.TabIndex = 20;
            this.m3.Text = "0";
            this.m3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m3.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.m3_KeyPress);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(211, 10);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 25);
            this.label10.TabIndex = 19;
            this.label10.Text = ".";
            // 
            // panel3
            // 
            this.panel3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.ip1);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.ip4);
            this.panel3.Controls.Add(this.ip2);
            this.panel3.Controls.Add(this.ip3);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Location = new System.Drawing.Point(20, 37);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(360, 45);
            this.panel3.TabIndex = 24;
            this.panel3.Paint += new System.Windows.Forms.PaintEventHandler(this.panel3_Paint);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(13, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(37, 25);
            this.label5.TabIndex = 18;
            this.label5.Text = "IP:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(13, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(193, 25);
            this.label6.TabIndex = 25;
            this.label6.Text = "Network Concetion";
            // 
            // UpdateInfo
            // 
            this.UpdateInfo.Enabled = true;
            this.UpdateInfo.Interval = 50;
            this.UpdateInfo.Tick += new System.EventHandler(this.UpdateInfo_Tick);
            // 
            // SetingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(964, 604);
            this.Controls.Add(this.PanelWiFILanEth);
            this.Controls.Add(this.iconButtonSave);
            this.Controls.Add(this.labelTextPort);
            this.Controls.Add(this.iconButtonFileNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.iconButtonOpenFile);
            this.Controls.Add(this.textBoxDirectory);
            this.Controls.Add(this.listViewPorts);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "SetingsForm";
            this.Text = "Setings";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SetingsForm_FormClosing);
            this.Load += new System.EventHandler(this.SetingsForm_Load_1);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.PanelWiFILanEth.ResumeLayout(false);
            this.PanelWiFILanEth.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private FontAwesome.Sharp.IconButton iconButtonAutoPort;
        private FontAwesome.Sharp.IconButton iconButtonManual;
        private System.Windows.Forms.ListBox listViewPorts;
        private System.Windows.Forms.Label labelTextPort;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private FontAwesome.Sharp.IconButton iconButtonOpenFile;
        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconButton iconButtonFileNew;
        private System.Windows.Forms.Label labelPortname;
        public System.Windows.Forms.Timer timerSerialPortChrck;
        private FontAwesome.Sharp.IconButton iconButtonSave;
        private System.Windows.Forms.TextBox ip1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ip2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ip3;
        private System.Windows.Forms.TextBox ip4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel PanelWiFILanEth;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox tbport;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox tbpass;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox g1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox g4;
        private System.Windows.Forms.TextBox g2;
        private System.Windows.Forms.TextBox g3;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox tbssid;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox m1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox m4;
        private System.Windows.Forms.TextBox m2;
        private System.Windows.Forms.TextBox m3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private FontAwesome.Sharp.IconButton iconButtonSet;
        private System.Windows.Forms.RadioButton radioButtonUSB;
        private System.Windows.Forms.RadioButton radioButtonEthernet;
        private System.Windows.Forms.RadioButton radioButtonLan;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Timer UpdateInfo;
    }
}