namespace WinFormsUI
{
    partial class MainForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.LiveViewPicBox = new System.Windows.Forms.PictureBox();
            this.LiveViewButton = new System.Windows.Forms.Button();
            this.CameraListBox = new System.Windows.Forms.ListBox();
            this.SessionButton = new System.Windows.Forms.Button();
            this.SessionLabel = new System.Windows.Forms.Label();
            this.InitGroupBox = new System.Windows.Forms.GroupBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.LiveViewGroupBox = new System.Windows.Forms.GroupBox();
            this.taskListBox = new System.Windows.Forms.ComboBox();
            this.TaskRefreshButton = new System.Windows.Forms.Button();
            this.SettingsGroupBox = new System.Windows.Forms.GroupBox();
            this.MainProgressBar = new System.Windows.Forms.ProgressBar();
            this.WBCoBox = new System.Windows.Forms.ComboBox();
            this.SavePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.RecordVideoButton = new System.Windows.Forms.Button();
            this.TakePhotoButton = new System.Windows.Forms.Button();
            this.BulbUpDo = new System.Windows.Forms.NumericUpDown();
            this.ISOCoBox = new System.Windows.Forms.ComboBox();
            this.TvCoBox = new System.Windows.Forms.ComboBox();
            this.AvCoBox = new System.Windows.Forms.ComboBox();
            this.SaveToGroupBox = new System.Windows.Forms.GroupBox();
            this.STBothButton = new System.Windows.Forms.RadioButton();
            this.STComputerButton = new System.Windows.Forms.RadioButton();
            this.STCameraButton = new System.Windows.Forms.RadioButton();
            this.SaveFolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.listBoxPathologyNum = new System.Windows.Forms.ListBox();
            this.labelPathologyNum = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.listBoxName = new System.Windows.Forms.ListBox();
            this.listBoxGender = new System.Windows.Forms.ListBox();
            this.labelGender = new System.Windows.Forms.Label();
            this.listBoxAge = new System.Windows.Forms.ListBox();
            this.labelAge = new System.Windows.Forms.Label();
            this.listBoxRegisterNum = new System.Windows.Forms.ListBox();
            this.labelRegisterNum = new System.Windows.Forms.Label();
            this.labelOperator = new System.Windows.Forms.Label();
            this.listBoxOperator = new System.Windows.Forms.ListBox();
            this.labelFileName = new System.Windows.Forms.Label();
            this.labelPicDescription = new System.Windows.Forms.Label();
            this.listBoxFilename = new System.Windows.Forms.ListBox();
            this.listBoxPicDesciption = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).BeginInit();
            this.InitGroupBox.SuspendLayout();
            this.LiveViewGroupBox.SuspendLayout();
            this.SettingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).BeginInit();
            this.SaveToGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // LiveViewPicBox
            // 
            this.LiveViewPicBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LiveViewPicBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LiveViewPicBox.Location = new System.Drawing.Point(8, 27);
            this.LiveViewPicBox.Name = "LiveViewPicBox";
            this.LiveViewPicBox.Size = new System.Drawing.Size(572, 357);
            this.LiveViewPicBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LiveViewPicBox.TabIndex = 1;
            this.LiveViewPicBox.TabStop = false;
            this.LiveViewPicBox.SizeChanged += new System.EventHandler(this.LiveViewPicBox_SizeChanged);
            this.LiveViewPicBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.LiveViewPicBox_MouseDown);
            // 
            // LiveViewButton
            // 
            this.LiveViewButton.Location = new System.Drawing.Point(664, 176);
            this.LiveViewButton.Name = "LiveViewButton";
            this.LiveViewButton.Size = new System.Drawing.Size(70, 22);
            this.LiveViewButton.TabIndex = 2;
            this.LiveViewButton.Text = "开始实时取景";
            this.LiveViewButton.UseVisualStyleBackColor = true;
            this.LiveViewButton.Click += new System.EventHandler(this.LiveViewButton_Click);
            // 
            // CameraListBox
            // 
            this.CameraListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CameraListBox.FormattingEnabled = true;
            this.CameraListBox.Location = new System.Drawing.Point(8, 35);
            this.CameraListBox.Name = "CameraListBox";
            this.CameraListBox.Size = new System.Drawing.Size(121, 82);
            this.CameraListBox.TabIndex = 6;
            // 
            // SessionButton
            // 
            this.SessionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SessionButton.Location = new System.Drawing.Point(6, 127);
            this.SessionButton.Name = "SessionButton";
            this.SessionButton.Size = new System.Drawing.Size(84, 23);
            this.SessionButton.TabIndex = 7;
            this.SessionButton.Text = "连接相机";
            this.SessionButton.UseVisualStyleBackColor = true;
            this.SessionButton.Click += new System.EventHandler(this.SessionButton_Click);
            // 
            // SessionLabel
            // 
            this.SessionLabel.AutoSize = true;
            this.SessionLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionLabel.Location = new System.Drawing.Point(6, 16);
            this.SessionLabel.Name = "SessionLabel";
            this.SessionLabel.Size = new System.Drawing.Size(98, 16);
            this.SessionLabel.TabIndex = 8;
            this.SessionLabel.Text = "尚未连接相机";
            // 
            // InitGroupBox
            // 
            this.InitGroupBox.Controls.Add(this.RefreshButton);
            this.InitGroupBox.Controls.Add(this.CameraListBox);
            this.InitGroupBox.Controls.Add(this.SessionLabel);
            this.InitGroupBox.Controls.Add(this.SessionButton);
            this.InitGroupBox.Location = new System.Drawing.Point(664, 12);
            this.InitGroupBox.Name = "InitGroupBox";
            this.InitGroupBox.Size = new System.Drawing.Size(135, 158);
            this.InitGroupBox.TabIndex = 9;
            this.InitGroupBox.TabStop = false;
            this.InitGroupBox.Text = "初始化";
            // 
            // RefreshButton
            // 
            this.RefreshButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.RefreshButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RefreshButton.Location = new System.Drawing.Point(96, 127);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(31, 23);
            this.RefreshButton.TabIndex = 9;
            this.RefreshButton.Text = "↻";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // LiveViewGroupBox
            // 
            this.LiveViewGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LiveViewGroupBox.Controls.Add(this.LiveViewPicBox);
            this.LiveViewGroupBox.Location = new System.Drawing.Point(12, 139);
            this.LiveViewGroupBox.Name = "LiveViewGroupBox";
            this.LiveViewGroupBox.Size = new System.Drawing.Size(646, 423);
            this.LiveViewGroupBox.TabIndex = 10;
            this.LiveViewGroupBox.TabStop = false;
            this.LiveViewGroupBox.Text = "实时图像";
            // 
            // taskListBox
            // 
            this.taskListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.taskListBox.FormattingEnabled = true;
            this.taskListBox.Location = new System.Drawing.Point(740, 527);
            this.taskListBox.Name = "taskListBox";
            this.taskListBox.Size = new System.Drawing.Size(191, 21);
            this.taskListBox.TabIndex = 9;
            // 
            // TaskRefreshButton
            // 
            this.TaskRefreshButton.Location = new System.Drawing.Point(664, 523);
            this.TaskRefreshButton.Name = "TaskRefreshButton";
            this.TaskRefreshButton.Size = new System.Drawing.Size(70, 27);
            this.TaskRefreshButton.TabIndex = 11;
            this.TaskRefreshButton.Text = "更新";
            this.TaskRefreshButton.UseVisualStyleBackColor = true;
            this.TaskRefreshButton.Click += new System.EventHandler(this.TaskRefreshButton_Click);
            // 
            // SettingsGroupBox
            // 
            this.SettingsGroupBox.Controls.Add(this.WBCoBox);
            this.SettingsGroupBox.Controls.Add(this.label4);
            this.SettingsGroupBox.Controls.Add(this.label3);
            this.SettingsGroupBox.Controls.Add(this.label2);
            this.SettingsGroupBox.Controls.Add(this.label5);
            this.SettingsGroupBox.Controls.Add(this.label1);
            this.SettingsGroupBox.Controls.Add(this.BulbUpDo);
            this.SettingsGroupBox.Controls.Add(this.ISOCoBox);
            this.SettingsGroupBox.Controls.Add(this.TvCoBox);
            this.SettingsGroupBox.Controls.Add(this.AvCoBox);
            this.SettingsGroupBox.Location = new System.Drawing.Point(664, 209);
            this.SettingsGroupBox.MinimumSize = new System.Drawing.Size(300, 100);
            this.SettingsGroupBox.Name = "SettingsGroupBox";
            this.SettingsGroupBox.Size = new System.Drawing.Size(313, 101);
            this.SettingsGroupBox.TabIndex = 11;
            this.SettingsGroupBox.TabStop = false;
            this.SettingsGroupBox.Text = "设置";
            this.SettingsGroupBox.Enter += new System.EventHandler(this.SettingsGroupBox_Enter);
            // 
            // MainProgressBar
            // 
            this.MainProgressBar.Location = new System.Drawing.Point(822, 496);
            this.MainProgressBar.Name = "MainProgressBar";
            this.MainProgressBar.Size = new System.Drawing.Size(130, 20);
            this.MainProgressBar.TabIndex = 8;
            // 
            // WBCoBox
            // 
            this.WBCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.WBCoBox.FormattingEnabled = true;
            this.WBCoBox.Items.AddRange(new object[] {
            "Auto",
            "Daylight",
            "Cloudy",
            "Tungsten",
            "Fluorescent",
            "Strobe",
            "White Paper",
            "Shade"});
            this.WBCoBox.Location = new System.Drawing.Point(142, 19);
            this.WBCoBox.Name = "WBCoBox";
            this.WBCoBox.Size = new System.Drawing.Size(110, 21);
            this.WBCoBox.TabIndex = 7;
            this.WBCoBox.SelectedIndexChanged += new System.EventHandler(this.WBCoBox_SelectedIndexChanged);
            // 
            // SavePathTextBox
            // 
            this.SavePathTextBox.Location = new System.Drawing.Point(670, 406);
            this.SavePathTextBox.Name = "SavePathTextBox";
            this.SavePathTextBox.Size = new System.Drawing.Size(230, 20);
            this.SavePathTextBox.TabIndex = 6;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(909, 407);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(68, 19);
            this.BrowseButton.TabIndex = 5;
            this.BrowseButton.Text = "浏览";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(242, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Bulb (s)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(106, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 16);
            this.label3.TabIndex = 3;
            this.label3.Text = "ISO";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(106, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tv";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(258, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "WB";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(106, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Av";
            // 
            // RecordVideoButton
            // 
            this.RecordVideoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RecordVideoButton.Location = new System.Drawing.Point(740, 494);
            this.RecordVideoButton.Name = "RecordVideoButton";
            this.RecordVideoButton.Size = new System.Drawing.Size(71, 23);
            this.RecordVideoButton.TabIndex = 2;
            this.RecordVideoButton.Text = "录像";
            this.RecordVideoButton.UseVisualStyleBackColor = true;
            this.RecordVideoButton.Visible = false;
            this.RecordVideoButton.Click += new System.EventHandler(this.RecordVideoButton_Click);
            // 
            // TakePhotoButton
            // 
            this.TakePhotoButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TakePhotoButton.Location = new System.Drawing.Point(663, 493);
            this.TakePhotoButton.Name = "TakePhotoButton";
            this.TakePhotoButton.Size = new System.Drawing.Size(71, 24);
            this.TakePhotoButton.TabIndex = 2;
            this.TakePhotoButton.Text = "拍照";
            this.TakePhotoButton.UseVisualStyleBackColor = true;
            this.TakePhotoButton.Click += new System.EventHandler(this.TakePhotoButton_Click);
            // 
            // BulbUpDo
            // 
            this.BulbUpDo.Location = new System.Drawing.Point(142, 46);
            this.BulbUpDo.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.BulbUpDo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.BulbUpDo.Name = "BulbUpDo";
            this.BulbUpDo.Size = new System.Drawing.Size(94, 20);
            this.BulbUpDo.TabIndex = 1;
            this.BulbUpDo.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // ISOCoBox
            // 
            this.ISOCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ISOCoBox.FormattingEnabled = true;
            this.ISOCoBox.Location = new System.Drawing.Point(6, 73);
            this.ISOCoBox.Name = "ISOCoBox";
            this.ISOCoBox.Size = new System.Drawing.Size(94, 21);
            this.ISOCoBox.TabIndex = 0;
            this.ISOCoBox.SelectedIndexChanged += new System.EventHandler(this.ISOCoBox_SelectedIndexChanged);
            // 
            // TvCoBox
            // 
            this.TvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.TvCoBox.FormattingEnabled = true;
            this.TvCoBox.Location = new System.Drawing.Point(6, 46);
            this.TvCoBox.Name = "TvCoBox";
            this.TvCoBox.Size = new System.Drawing.Size(94, 21);
            this.TvCoBox.TabIndex = 0;
            this.TvCoBox.SelectedIndexChanged += new System.EventHandler(this.TvCoBox_SelectedIndexChanged);
            // 
            // AvCoBox
            // 
            this.AvCoBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.AvCoBox.FormattingEnabled = true;
            this.AvCoBox.Location = new System.Drawing.Point(6, 19);
            this.AvCoBox.Name = "AvCoBox";
            this.AvCoBox.Size = new System.Drawing.Size(94, 21);
            this.AvCoBox.TabIndex = 0;
            this.AvCoBox.SelectedIndexChanged += new System.EventHandler(this.AvCoBox_SelectedIndexChanged);
            // 
            // SaveToGroupBox
            // 
            this.SaveToGroupBox.Controls.Add(this.STBothButton);
            this.SaveToGroupBox.Controls.Add(this.STComputerButton);
            this.SaveToGroupBox.Controls.Add(this.STCameraButton);
            this.SaveToGroupBox.Location = new System.Drawing.Point(814, 20);
            this.SaveToGroupBox.Name = "SaveToGroupBox";
            this.SaveToGroupBox.Size = new System.Drawing.Size(101, 142);
            this.SaveToGroupBox.TabIndex = 4;
            this.SaveToGroupBox.TabStop = false;
            this.SaveToGroupBox.Text = "保存到";
            // 
            // STBothButton
            // 
            this.STBothButton.AutoSize = true;
            this.STBothButton.Checked = true;
            this.STBothButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STBothButton.Location = new System.Drawing.Point(6, 71);
            this.STBothButton.Name = "STBothButton";
            this.STBothButton.Size = new System.Drawing.Size(53, 20);
            this.STBothButton.TabIndex = 0;
            this.STBothButton.TabStop = true;
            this.STBothButton.Text = "Both";
            this.STBothButton.UseVisualStyleBackColor = true;
            this.STBothButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STComputerButton
            // 
            this.STComputerButton.AutoSize = true;
            this.STComputerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STComputerButton.Location = new System.Drawing.Point(6, 45);
            this.STComputerButton.Name = "STComputerButton";
            this.STComputerButton.Size = new System.Drawing.Size(56, 20);
            this.STComputerButton.TabIndex = 0;
            this.STComputerButton.Text = "电脑";
            this.STComputerButton.UseVisualStyleBackColor = true;
            this.STComputerButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // STCameraButton
            // 
            this.STCameraButton.AutoSize = true;
            this.STCameraButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.STCameraButton.Location = new System.Drawing.Point(6, 19);
            this.STCameraButton.Name = "STCameraButton";
            this.STCameraButton.Size = new System.Drawing.Size(56, 20);
            this.STCameraButton.TabIndex = 0;
            this.STCameraButton.TabStop = true;
            this.STCameraButton.Text = "相机";
            this.STCameraButton.UseVisualStyleBackColor = true;
            this.STCameraButton.CheckedChanged += new System.EventHandler(this.SaveToButton_CheckedChanged);
            // 
            // SaveFolderBrowser
            // 
            this.SaveFolderBrowser.Description = "存储图像到...";
            // 
            // listBoxPathologyNum
            // 
            this.listBoxPathologyNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPathologyNum.FormattingEnabled = true;
            this.listBoxPathologyNum.Location = new System.Drawing.Point(70, 19);
            this.listBoxPathologyNum.Name = "listBoxPathologyNum";
            this.listBoxPathologyNum.Size = new System.Drawing.Size(72, 17);
            this.listBoxPathologyNum.TabIndex = 12;
            // 
            // labelPathologyNum
            // 
            this.labelPathologyNum.AutoSize = true;
            this.labelPathologyNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPathologyNum.Location = new System.Drawing.Point(17, 20);
            this.labelPathologyNum.Name = "labelPathologyNum";
            this.labelPathologyNum.Size = new System.Drawing.Size(53, 16);
            this.labelPathologyNum.TabIndex = 10;
            this.labelPathologyNum.Text = "病理号";
            this.labelPathologyNum.Click += new System.EventHandler(this.label6_Click);
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(148, 19);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(38, 16);
            this.labelName.TabIndex = 13;
            this.labelName.Text = "姓名";
            // 
            // listBoxName
            // 
            this.listBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxName.FormattingEnabled = true;
            this.listBoxName.Location = new System.Drawing.Point(182, 18);
            this.listBoxName.Name = "listBoxName";
            this.listBoxName.Size = new System.Drawing.Size(72, 17);
            this.listBoxName.TabIndex = 14;
            // 
            // listBoxGender
            // 
            this.listBoxGender.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxGender.FormattingEnabled = true;
            this.listBoxGender.Location = new System.Drawing.Point(303, 18);
            this.listBoxGender.Name = "listBoxGender";
            this.listBoxGender.Size = new System.Drawing.Size(72, 17);
            this.listBoxGender.TabIndex = 15;
            // 
            // labelGender
            // 
            this.labelGender.AutoSize = true;
            this.labelGender.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelGender.Location = new System.Drawing.Point(261, 18);
            this.labelGender.Name = "labelGender";
            this.labelGender.Size = new System.Drawing.Size(38, 16);
            this.labelGender.TabIndex = 16;
            this.labelGender.Text = "性别";
            // 
            // listBoxAge
            // 
            this.listBoxAge.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxAge.FormattingEnabled = true;
            this.listBoxAge.Location = new System.Drawing.Point(420, 18);
            this.listBoxAge.Name = "listBoxAge";
            this.listBoxAge.Size = new System.Drawing.Size(52, 17);
            this.listBoxAge.TabIndex = 17;
            // 
            // labelAge
            // 
            this.labelAge.AutoSize = true;
            this.labelAge.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAge.Location = new System.Drawing.Point(381, 18);
            this.labelAge.Name = "labelAge";
            this.labelAge.Size = new System.Drawing.Size(38, 16);
            this.labelAge.TabIndex = 18;
            this.labelAge.Text = "年龄";
            // 
            // listBoxRegisterNum
            // 
            this.listBoxRegisterNum.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxRegisterNum.FormattingEnabled = true;
            this.listBoxRegisterNum.Location = new System.Drawing.Point(591, 19);
            this.listBoxRegisterNum.Name = "listBoxRegisterNum";
            this.listBoxRegisterNum.Size = new System.Drawing.Size(52, 17);
            this.listBoxRegisterNum.TabIndex = 19;
            // 
            // labelRegisterNum
            // 
            this.labelRegisterNum.AutoSize = true;
            this.labelRegisterNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRegisterNum.Location = new System.Drawing.Point(480, 19);
            this.labelRegisterNum.Name = "labelRegisterNum";
            this.labelRegisterNum.Size = new System.Drawing.Size(102, 16);
            this.labelRegisterNum.TabIndex = 20;
            this.labelRegisterNum.Text = "门诊号/住院号";
            // 
            // labelOperator
            // 
            this.labelOperator.AutoSize = true;
            this.labelOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOperator.Location = new System.Drawing.Point(669, 436);
            this.labelOperator.Name = "labelOperator";
            this.labelOperator.Size = new System.Drawing.Size(68, 16);
            this.labelOperator.TabIndex = 21;
            this.labelOperator.Text = "操作人员";
            // 
            // listBoxOperator
            // 
            this.listBoxOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxOperator.FormattingEnabled = true;
            this.listBoxOperator.Location = new System.Drawing.Point(743, 436);
            this.listBoxOperator.Name = "listBoxOperator";
            this.listBoxOperator.Size = new System.Drawing.Size(209, 17);
            this.listBoxOperator.TabIndex = 22;
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFileName.Location = new System.Drawing.Point(670, 332);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(53, 16);
            this.labelFileName.TabIndex = 23;
            this.labelFileName.Text = "文件名";
            // 
            // labelPicDescription
            // 
            this.labelPicDescription.AutoSize = true;
            this.labelPicDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPicDescription.Location = new System.Drawing.Point(670, 363);
            this.labelPicDescription.Name = "labelPicDescription";
            this.labelPicDescription.Size = new System.Drawing.Size(68, 16);
            this.labelPicDescription.TabIndex = 24;
            this.labelPicDescription.Text = "图片描述";
            // 
            // listBoxFilename
            // 
            this.listBoxFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxFilename.FormattingEnabled = true;
            this.listBoxFilename.Location = new System.Drawing.Point(760, 331);
            this.listBoxFilename.Name = "listBoxFilename";
            this.listBoxFilename.Size = new System.Drawing.Size(209, 17);
            this.listBoxFilename.TabIndex = 25;
            // 
            // listBoxPicDesciption
            // 
            this.listBoxPicDesciption.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listBoxPicDesciption.FormattingEnabled = true;
            this.listBoxPicDesciption.Location = new System.Drawing.Point(761, 362);
            this.listBoxPicDesciption.Name = "listBoxPicDesciption";
            this.listBoxPicDesciption.Size = new System.Drawing.Size(209, 17);
            this.listBoxPicDesciption.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(17, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(68, 16);
            this.label6.TabIndex = 27;
            this.label6.Text = "任务信息";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 600);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.listBoxPicDesciption);
            this.Controls.Add(this.listBoxFilename);
            this.Controls.Add(this.labelPicDescription);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.MainProgressBar);
            this.Controls.Add(this.listBoxOperator);
            this.Controls.Add(this.labelOperator);
            this.Controls.Add(this.SavePathTextBox);
            this.Controls.Add(this.labelRegisterNum);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.listBoxRegisterNum);
            this.Controls.Add(this.labelAge);
            this.Controls.Add(this.listBoxAge);
            this.Controls.Add(this.labelGender);
            this.Controls.Add(this.listBoxGender);
            this.Controls.Add(this.listBoxName);
            this.Controls.Add(this.RecordVideoButton);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.TakePhotoButton);
            this.Controls.Add(this.labelPathologyNum);
            this.Controls.Add(this.listBoxPathologyNum);
            this.Controls.Add(this.TaskRefreshButton);
            this.Controls.Add(this.taskListBox);
            this.Controls.Add(this.LiveViewButton);
            this.Controls.Add(this.SettingsGroupBox);
            this.Controls.Add(this.LiveViewGroupBox);
            this.Controls.Add(this.InitGroupBox);
            this.Controls.Add(this.SaveToGroupBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(880, 638);
            this.Name = "MainForm";
            this.Text = "Canon 相机控制软件 － 路漫科技 ";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.LiveViewPicBox)).EndInit();
            this.InitGroupBox.ResumeLayout(false);
            this.InitGroupBox.PerformLayout();
            this.LiveViewGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.ResumeLayout(false);
            this.SettingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.BulbUpDo)).EndInit();
            this.SaveToGroupBox.ResumeLayout(false);
            this.SaveToGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox LiveViewPicBox;
        private System.Windows.Forms.Button LiveViewButton;
        private System.Windows.Forms.ListBox CameraListBox;
        private System.Windows.Forms.Button SessionButton;
        private System.Windows.Forms.Label SessionLabel;
        private System.Windows.Forms.GroupBox InitGroupBox;
        private System.Windows.Forms.GroupBox LiveViewGroupBox;
        private System.Windows.Forms.GroupBox SettingsGroupBox;
        private System.Windows.Forms.Button TakePhotoButton;
        private System.Windows.Forms.NumericUpDown BulbUpDo;
        private System.Windows.Forms.ComboBox ISOCoBox;
        private System.Windows.Forms.ComboBox TvCoBox;
        private System.Windows.Forms.ComboBox AvCoBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox SaveToGroupBox;
        private System.Windows.Forms.RadioButton STBothButton;
        private System.Windows.Forms.RadioButton STComputerButton;
        private System.Windows.Forms.RadioButton STCameraButton;
        private System.Windows.Forms.TextBox SavePathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.FolderBrowserDialog SaveFolderBrowser;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.ComboBox WBCoBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button RecordVideoButton;
        private System.Windows.Forms.ProgressBar MainProgressBar;
        private System.Windows.Forms.Button TaskRefreshButton;
        private System.Windows.Forms.ComboBox taskListBox;
        private System.Windows.Forms.ListBox listBoxPathologyNum;
        private System.Windows.Forms.Label labelPathologyNum;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.ListBox listBoxName;
        private System.Windows.Forms.ListBox listBoxGender;
        private System.Windows.Forms.Label labelGender;
        private System.Windows.Forms.ListBox listBoxAge;
        private System.Windows.Forms.Label labelAge;
        private System.Windows.Forms.ListBox listBoxRegisterNum;
        private System.Windows.Forms.Label labelRegisterNum;
        private System.Windows.Forms.Label labelOperator;
        private System.Windows.Forms.ListBox listBoxOperator;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.Label labelPicDescription;
        private System.Windows.Forms.ListBox listBoxFilename;
        private System.Windows.Forms.ListBox listBoxPicDesciption;
        private System.Windows.Forms.Label label6;
    }
}

