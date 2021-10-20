namespace SDRAC
{
    partial class PathsForm
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
            this.textBoxDirectory = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.iconButtonFileNew = new FontAwesome.Sharp.IconButton();
            this.iconButtonOpenFile = new FontAwesome.Sharp.IconButton();
            this.dialog = new System.Windows.Forms.OpenFileDialog();
            this.listBoxPoints = new System.Windows.Forms.ListBox();
            this.iconButtonRemove = new FontAwesome.Sharp.IconButton();
            this.iconButtonLoad = new FontAwesome.Sharp.IconButton();
            this.iconButtonEdit = new FontAwesome.Sharp.IconButton();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.labelPos = new System.Windows.Forms.Label();
            this.iconButtonRun = new FontAwesome.Sharp.IconButton();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCurPoi = new System.Windows.Forms.Label();
            this.radioButtonRS = new System.Windows.Forms.RadioButton();
            this.radioButtonRB = new System.Windows.Forms.RadioButton();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.iconButtonAdd = new FontAwesome.Sharp.IconButton();
            this.radioButtonUnder = new System.Windows.Forms.RadioButton();
            this.radioButtonOver = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.labelFileSize = new System.Windows.Forms.Label();
            this.groupBoxUO = new System.Windows.Forms.GroupBox();
            this.groupBoxR = new System.Windows.Forms.GroupBox();
            this.groupBoxUO.SuspendLayout();
            this.groupBoxR.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxDirectory
            // 
            this.textBoxDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.textBoxDirectory.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.textBoxDirectory.ForeColor = System.Drawing.Color.DarkGray;
            this.textBoxDirectory.Location = new System.Drawing.Point(16, 39);
            this.textBoxDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxDirectory.Multiline = true;
            this.textBoxDirectory.Name = "textBoxDirectory";
            this.textBoxDirectory.ReadOnly = true;
            this.textBoxDirectory.Size = new System.Drawing.Size(937, 29);
            this.textBoxDirectory.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(11, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(232, 28);
            this.label1.TabIndex = 11;
            this.label1.Text = "PathFile";
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
            this.iconButtonFileNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonFileNew.Location = new System.Drawing.Point(794, 136);
            this.iconButtonFileNew.Name = "iconButtonFileNew";
            this.iconButtonFileNew.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonFileNew.Rotation = 0D;
            this.iconButtonFileNew.Size = new System.Drawing.Size(159, 50);
            this.iconButtonFileNew.TabIndex = 13;
            this.iconButtonFileNew.Text = "  New File";
            this.iconButtonFileNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonFileNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonFileNew.UseVisualStyleBackColor = false;
            this.iconButtonFileNew.Click += new System.EventHandler(this.iconButtonFileNew_Click);
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
            this.iconButtonOpenFile.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonOpenFile.Location = new System.Drawing.Point(794, 80);
            this.iconButtonOpenFile.Name = "iconButtonOpenFile";
            this.iconButtonOpenFile.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonOpenFile.Rotation = 0D;
            this.iconButtonOpenFile.Size = new System.Drawing.Size(159, 50);
            this.iconButtonOpenFile.TabIndex = 12;
            this.iconButtonOpenFile.Text = "  Pick file";
            this.iconButtonOpenFile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonOpenFile.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonOpenFile.UseVisualStyleBackColor = false;
            this.iconButtonOpenFile.Click += new System.EventHandler(this.iconButtonOpenFile_Click);
            // 
            // dialog
            // 
            this.dialog.FileName = "openFileDialog1";
            // 
            // listBoxPoints
            // 
            this.listBoxPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxPoints.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.listBoxPoints.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBoxPoints.ForeColor = System.Drawing.Color.White;
            this.listBoxPoints.FormattingEnabled = true;
            this.listBoxPoints.ItemHeight = 20;
            this.listBoxPoints.Location = new System.Drawing.Point(16, 86);
            this.listBoxPoints.Name = "listBoxPoints";
            this.listBoxPoints.Size = new System.Drawing.Size(755, 460);
            this.listBoxPoints.TabIndex = 14;
            this.listBoxPoints.SelectedIndexChanged += new System.EventHandler(this.listBoxPoints_SelectedIndexChanged);
            // 
            // iconButtonRemove
            // 
            this.iconButtonRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonRemove.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonRemove.Enabled = false;
            this.iconButtonRemove.FlatAppearance.BorderSize = 0;
            this.iconButtonRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonRemove.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonRemove.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonRemove.IconChar = FontAwesome.Sharp.IconChar.TimesCircle;
            this.iconButtonRemove.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonRemove.IconSize = 36;
            this.iconButtonRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonRemove.Location = new System.Drawing.Point(794, 248);
            this.iconButtonRemove.Name = "iconButtonRemove";
            this.iconButtonRemove.Rotation = 0D;
            this.iconButtonRemove.Size = new System.Drawing.Size(160, 50);
            this.iconButtonRemove.TabIndex = 45;
            this.iconButtonRemove.Text = "  Remove";
            this.iconButtonRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonRemove.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonRemove.UseVisualStyleBackColor = false;
            this.iconButtonRemove.Click += new System.EventHandler(this.iconButtonRemove_Click);
            // 
            // iconButtonLoad
            // 
            this.iconButtonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonLoad.FlatAppearance.BorderSize = 0;
            this.iconButtonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonLoad.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonLoad.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonLoad.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonLoad.IconChar = FontAwesome.Sharp.IconChar.FileImport;
            this.iconButtonLoad.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonLoad.IconSize = 32;
            this.iconButtonLoad.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonLoad.Location = new System.Drawing.Point(794, 192);
            this.iconButtonLoad.Name = "iconButtonLoad";
            this.iconButtonLoad.Padding = new System.Windows.Forms.Padding(10, 0, 20, 0);
            this.iconButtonLoad.Rotation = 0D;
            this.iconButtonLoad.Size = new System.Drawing.Size(160, 50);
            this.iconButtonLoad.TabIndex = 46;
            this.iconButtonLoad.Text = "  Load";
            this.iconButtonLoad.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonLoad.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonLoad.UseVisualStyleBackColor = false;
            this.iconButtonLoad.Click += new System.EventHandler(this.iconButtonLoad_Click);
            // 
            // iconButtonEdit
            // 
            this.iconButtonEdit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonEdit.Enabled = false;
            this.iconButtonEdit.FlatAppearance.BorderSize = 0;
            this.iconButtonEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonEdit.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonEdit.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonEdit.IconChar = FontAwesome.Sharp.IconChar.Edit;
            this.iconButtonEdit.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonEdit.IconSize = 36;
            this.iconButtonEdit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonEdit.Location = new System.Drawing.Point(794, 304);
            this.iconButtonEdit.Name = "iconButtonEdit";
            this.iconButtonEdit.Rotation = 0D;
            this.iconButtonEdit.Size = new System.Drawing.Size(160, 50);
            this.iconButtonEdit.TabIndex = 48;
            this.iconButtonEdit.Text = "  Edit";
            this.iconButtonEdit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonEdit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonEdit.UseVisualStyleBackColor = false;
            this.iconButtonEdit.Click += new System.EventHandler(this.iconButtonEdit_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.ForeColor = System.Drawing.Color.DarkGray;
            this.label2.Location = new System.Drawing.Point(12, 557);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 20);
            this.label2.TabIndex = 49;
            this.label2.Text = "Amount of positions:";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.ForeColor = System.Drawing.Color.DarkGray;
            this.label3.Location = new System.Drawing.Point(194, 557);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(140, 20);
            this.label3.TabIndex = 50;
            this.label3.Text = "Amount of points:";
            // 
            // labelPoints
            // 
            this.labelPoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPoints.AutoSize = true;
            this.labelPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelPoints.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPoints.ForeColor = System.Drawing.Color.DarkGray;
            this.labelPoints.Location = new System.Drawing.Point(202, 577);
            this.labelPoints.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(18, 20);
            this.labelPoints.TabIndex = 51;
            this.labelPoints.Text = "0";
            // 
            // labelPos
            // 
            this.labelPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPos.AutoSize = true;
            this.labelPos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelPos.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelPos.ForeColor = System.Drawing.Color.DarkGray;
            this.labelPos.Location = new System.Drawing.Point(20, 577);
            this.labelPos.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelPos.Name = "labelPos";
            this.labelPos.Size = new System.Drawing.Size(18, 20);
            this.labelPos.TabIndex = 52;
            this.labelPos.Text = "0";
            // 
            // iconButtonRun
            // 
            this.iconButtonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonRun.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonRun.FlatAppearance.BorderSize = 0;
            this.iconButtonRun.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonRun.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonRun.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonRun.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonRun.IconChar = FontAwesome.Sharp.IconChar.Play;
            this.iconButtonRun.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonRun.IconSize = 36;
            this.iconButtonRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonRun.Location = new System.Drawing.Point(793, 450);
            this.iconButtonRun.Name = "iconButtonRun";
            this.iconButtonRun.Rotation = 0D;
            this.iconButtonRun.Size = new System.Drawing.Size(161, 50);
            this.iconButtonRun.TabIndex = 53;
            this.iconButtonRun.Text = "  Run";
            this.iconButtonRun.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonRun.UseVisualStyleBackColor = false;
            this.iconButtonRun.Click += new System.EventHandler(this.iconButtonRun_Click);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.ForeColor = System.Drawing.Color.DarkGray;
            this.label4.Location = new System.Drawing.Point(353, 557);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(148, 20);
            this.label4.TabIndex = 54;
            this.label4.Text = "Current  pos/point:";
            // 
            // labelCurPoi
            // 
            this.labelCurPoi.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurPoi.AutoSize = true;
            this.labelCurPoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelCurPoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelCurPoi.ForeColor = System.Drawing.Color.DarkGray;
            this.labelCurPoi.Location = new System.Drawing.Point(361, 577);
            this.labelCurPoi.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurPoi.Name = "labelCurPoi";
            this.labelCurPoi.Size = new System.Drawing.Size(32, 20);
            this.labelCurPoi.TabIndex = 55;
            this.labelCurPoi.Text = "0/0";
            // 
            // radioButtonRS
            // 
            this.radioButtonRS.AutoSize = true;
            this.radioButtonRS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButtonRS.ForeColor = System.Drawing.Color.DarkGray;
            this.radioButtonRS.Location = new System.Drawing.Point(1, 39);
            this.radioButtonRS.Name = "radioButtonRS";
            this.radioButtonRS.Size = new System.Drawing.Size(134, 20);
            this.radioButtonRS.TabIndex = 56;
            this.radioButtonRS.Text = "Run from selected";
            this.radioButtonRS.UseVisualStyleBackColor = true;
            // 
            // radioButtonRB
            // 
            this.radioButtonRB.AutoSize = true;
            this.radioButtonRB.Checked = true;
            this.radioButtonRB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButtonRB.ForeColor = System.Drawing.Color.DarkGray;
            this.radioButtonRB.Location = new System.Drawing.Point(3, 13);
            this.radioButtonRB.Name = "radioButtonRB";
            this.radioButtonRB.Size = new System.Drawing.Size(155, 20);
            this.radioButtonRB.TabIndex = 57;
            this.radioButtonRB.TabStop = true;
            this.radioButtonRB.Text = "Run from the begining";
            this.radioButtonRB.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.progressBar.Location = new System.Drawing.Point(612, 557);
            this.progressBar.MarqueeAnimationSpeed = 25;
            this.progressBar.Maximum = 1000;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(159, 23);
            this.progressBar.TabIndex = 58;
            // 
            // iconButtonAdd
            // 
            this.iconButtonAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.iconButtonAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(32)))), ((int)(((byte)(32)))));
            this.iconButtonAdd.Enabled = false;
            this.iconButtonAdd.FlatAppearance.BorderSize = 0;
            this.iconButtonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.iconButtonAdd.Flip = FontAwesome.Sharp.FlipOrientation.Normal;
            this.iconButtonAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.iconButtonAdd.ForeColor = System.Drawing.Color.DarkGray;
            this.iconButtonAdd.IconChar = FontAwesome.Sharp.IconChar.Plus;
            this.iconButtonAdd.IconColor = System.Drawing.Color.DarkGray;
            this.iconButtonAdd.IconSize = 36;
            this.iconButtonAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonAdd.Location = new System.Drawing.Point(793, 362);
            this.iconButtonAdd.Name = "iconButtonAdd";
            this.iconButtonAdd.Rotation = 0D;
            this.iconButtonAdd.Size = new System.Drawing.Size(161, 50);
            this.iconButtonAdd.TabIndex = 59;
            this.iconButtonAdd.Text = "  Add";
            this.iconButtonAdd.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.iconButtonAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.iconButtonAdd.UseVisualStyleBackColor = false;
            this.iconButtonAdd.Click += new System.EventHandler(this.iconButtonAdd_Click);
            // 
            // radioButtonUnder
            // 
            this.radioButtonUnder.AutoSize = true;
            this.radioButtonUnder.Checked = true;
            this.radioButtonUnder.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButtonUnder.ForeColor = System.Drawing.Color.DarkGray;
            this.radioButtonUnder.Location = new System.Drawing.Point(6, 8);
            this.radioButtonUnder.Name = "radioButtonUnder";
            this.radioButtonUnder.Size = new System.Drawing.Size(63, 20);
            this.radioButtonUnder.TabIndex = 61;
            this.radioButtonUnder.TabStop = true;
            this.radioButtonUnder.Text = "Under";
            this.radioButtonUnder.UseVisualStyleBackColor = true;
            // 
            // radioButtonOver
            // 
            this.radioButtonOver.AutoSize = true;
            this.radioButtonOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.radioButtonOver.ForeColor = System.Drawing.Color.DarkGray;
            this.radioButtonOver.Location = new System.Drawing.Point(86, 8);
            this.radioButtonOver.Name = "radioButtonOver";
            this.radioButtonOver.Size = new System.Drawing.Size(55, 20);
            this.radioButtonOver.TabIndex = 60;
            this.radioButtonOver.Text = "Over";
            this.radioButtonOver.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.ForeColor = System.Drawing.Color.DarkGray;
            this.label5.Location = new System.Drawing.Point(523, 557);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 20);
            this.label5.TabIndex = 62;
            this.label5.Text = "File size:";
            // 
            // labelFileSize
            // 
            this.labelFileSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelFileSize.AutoSize = true;
            this.labelFileSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelFileSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.labelFileSize.ForeColor = System.Drawing.Color.DarkGray;
            this.labelFileSize.Location = new System.Drawing.Point(533, 577);
            this.labelFileSize.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFileSize.Name = "labelFileSize";
            this.labelFileSize.Size = new System.Drawing.Size(18, 20);
            this.labelFileSize.TabIndex = 63;
            this.labelFileSize.Text = "0";
            // 
            // groupBoxUO
            // 
            this.groupBoxUO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxUO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.groupBoxUO.Controls.Add(this.radioButtonUnder);
            this.groupBoxUO.Controls.Add(this.radioButtonOver);
            this.groupBoxUO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupBoxUO.ForeColor = System.Drawing.SystemColors.ControlText;
            this.groupBoxUO.Location = new System.Drawing.Point(793, 410);
            this.groupBoxUO.Name = "groupBoxUO";
            this.groupBoxUO.Size = new System.Drawing.Size(161, 34);
            this.groupBoxUO.TabIndex = 64;
            this.groupBoxUO.TabStop = false;
            // 
            // groupBoxR
            // 
            this.groupBoxR.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxR.Controls.Add(this.radioButtonRB);
            this.groupBoxR.Controls.Add(this.radioButtonRS);
            this.groupBoxR.Location = new System.Drawing.Point(794, 506);
            this.groupBoxR.Name = "groupBoxR";
            this.groupBoxR.Size = new System.Drawing.Size(161, 67);
            this.groupBoxR.TabIndex = 65;
            this.groupBoxR.TabStop = false;
            // 
            // PathsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(964, 604);
            this.Controls.Add(this.groupBoxR);
            this.Controls.Add(this.groupBoxUO);
            this.Controls.Add(this.labelFileSize);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.iconButtonAdd);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelCurPoi);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.iconButtonRun);
            this.Controls.Add(this.labelPos);
            this.Controls.Add(this.labelPoints);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iconButtonEdit);
            this.Controls.Add(this.iconButtonLoad);
            this.Controls.Add(this.iconButtonRemove);
            this.Controls.Add(this.listBoxPoints);
            this.Controls.Add(this.iconButtonFileNew);
            this.Controls.Add(this.iconButtonOpenFile);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PathsForm";
            this.Text = "PathsForm";
            this.groupBoxUO.ResumeLayout(false);
            this.groupBoxUO.PerformLayout();
            this.groupBoxR.ResumeLayout(false);
            this.groupBoxR.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxDirectory;
        private System.Windows.Forms.Label label1;
        private FontAwesome.Sharp.IconButton iconButtonFileNew;
        private FontAwesome.Sharp.IconButton iconButtonOpenFile;
        private System.Windows.Forms.OpenFileDialog dialog;
        private System.Windows.Forms.ListBox listBoxPoints;
        private FontAwesome.Sharp.IconButton iconButtonRemove;
        private FontAwesome.Sharp.IconButton iconButtonLoad;
        private FontAwesome.Sharp.IconButton iconButtonEdit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelPoints;
        private System.Windows.Forms.Label labelPos;
        private FontAwesome.Sharp.IconButton iconButtonRun;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCurPoi;
        private System.Windows.Forms.RadioButton radioButtonRS;
        private System.Windows.Forms.RadioButton radioButtonRB;
        private System.Windows.Forms.ProgressBar progressBar;
        private FontAwesome.Sharp.IconButton iconButtonAdd;
        private System.Windows.Forms.RadioButton radioButtonUnder;
        private System.Windows.Forms.RadioButton radioButtonOver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelFileSize;
        private System.Windows.Forms.GroupBox groupBoxUO;
        private System.Windows.Forms.GroupBox groupBoxR;
    }
}