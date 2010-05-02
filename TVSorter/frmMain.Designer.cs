namespace TVSorter
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpgInputFolder = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.cboFolderFilter = new System.Windows.Forms.ComboBox();
            this.btnSetShow = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnDeselectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnRenameMove = new System.Windows.Forms.Button();
            this.btnRenameOnly = new System.Windows.Forms.Button();
            this.lstInputFolder = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.tpgTVShows = new System.Windows.Forms.TabPage();
            this.flShowControls = new System.Windows.Forms.FlowLayoutPanel();
            this.btnApplyShowSetttings = new System.Windows.Forms.Button();
            this.btnSaveShowSettings = new System.Windows.Forms.Button();
            this.btnResetShowSettings = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAltNames = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtShowFolderName = new System.Windows.Forms.TextBox();
            this.grpShowCustomFormat = new System.Windows.Forms.GroupBox();
            this.txtShowExampleFileName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtShowCustomFormat = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkUseDefaultFormat = new System.Windows.Forms.CheckBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnUpdateSelected = new System.Windows.Forms.Button();
            this.btnUpdateAll = new System.Windows.Forms.Button();
            this.btnForceUpdateAll = new System.Windows.Forms.Button();
            this.btnAddShow = new System.Windows.Forms.Button();
            this.btnRemoveShow = new System.Windows.Forms.Button();
            this.btnSearchShows = new System.Windows.Forms.Button();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.lblTvdbId = new System.Windows.Forms.Label();
            this.picShowPic = new System.Windows.Forms.PictureBox();
            this.lblSelectedShow = new System.Windows.Forms.Label();
            this.lstTVShows = new System.Windows.Forms.ListBox();
            this.tpgSettings = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.grpShowNames = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExampleName = new System.Windows.Forms.TextBox();
            this.txtNameFormat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpDirectories = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseInputFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRecurse = new System.Windows.Forms.CheckBox();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.tpgLog = new System.Windows.Forms.TabPage();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.dlgFolderSelect = new System.Windows.Forms.FolderBrowserDialog();
            this.chkDeleteEmpty = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpgInputFolder.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tpgTVShows.SuspendLayout();
            this.flShowControls.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpShowCustomFormat.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowPic)).BeginInit();
            this.tpgSettings.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.grpShowNames.SuspendLayout();
            this.grpDirectories.SuspendLayout();
            this.tpgLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpgInputFolder);
            this.tabControl1.Controls.Add(this.tpgTVShows);
            this.tabControl1.Controls.Add(this.tpgSettings);
            this.tabControl1.Controls.Add(this.tpgLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(927, 530);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpgInputFolder
            // 
            this.tpgInputFolder.Controls.Add(this.splitContainer1);
            this.tpgInputFolder.Location = new System.Drawing.Point(4, 22);
            this.tpgInputFolder.Name = "tpgInputFolder";
            this.tpgInputFolder.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInputFolder.Size = new System.Drawing.Size(919, 504);
            this.tpgInputFolder.TabIndex = 0;
            this.tpgInputFolder.Text = "Input folder";
            this.tpgInputFolder.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cboFolderFilter);
            this.splitContainer1.Panel1.Controls.Add(this.btnSetShow);
            this.splitContainer1.Panel1.Controls.Add(this.btnRefresh);
            this.splitContainer1.Panel1.Controls.Add(this.btnDeselectAll);
            this.splitContainer1.Panel1.Controls.Add(this.btnSelectAll);
            this.splitContainer1.Panel1.Controls.Add(this.btnRenameMove);
            this.splitContainer1.Panel1.Controls.Add(this.btnRenameOnly);
            this.splitContainer1.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lstInputFolder);
            this.splitContainer1.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer1.Size = new System.Drawing.Size(913, 498);
            this.splitContainer1.SplitterDistance = 30;
            this.splitContainer1.TabIndex = 1;
            // 
            // cboFolderFilter
            // 
            this.cboFolderFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFolderFilter.FormattingEnabled = true;
            this.cboFolderFilter.Location = new System.Drawing.Point(546, 5);
            this.cboFolderFilter.Name = "cboFolderFilter";
            this.cboFolderFilter.Size = new System.Drawing.Size(192, 21);
            this.cboFolderFilter.TabIndex = 6;
            // 
            // btnSetShow
            // 
            this.btnSetShow.Location = new System.Drawing.Point(222, 3);
            this.btnSetShow.Name = "btnSetShow";
            this.btnSetShow.Size = new System.Drawing.Size(75, 23);
            this.btnSetShow.TabIndex = 5;
            this.btnSetShow.Text = "Set Show";
            this.btnSetShow.UseVisualStyleBackColor = true;
            this.btnSetShow.Click += new System.EventHandler(this.btnSetShow_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(465, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.Location = new System.Drawing.Point(384, 3);
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(75, 23);
            this.btnDeselectAll.TabIndex = 3;
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.UseVisualStyleBackColor = true;
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(303, 3);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 2;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnRenameMove
            // 
            this.btnRenameMove.Location = new System.Drawing.Point(102, 4);
            this.btnRenameMove.Name = "btnRenameMove";
            this.btnRenameMove.Size = new System.Drawing.Size(114, 23);
            this.btnRenameMove.TabIndex = 1;
            this.btnRenameMove.Text = "Rename && Move";
            this.btnRenameMove.UseVisualStyleBackColor = true;
            this.btnRenameMove.Click += new System.EventHandler(this.btnRenameMove_Click);
            // 
            // btnRenameOnly
            // 
            this.btnRenameOnly.Location = new System.Drawing.Point(5, 3);
            this.btnRenameOnly.Name = "btnRenameOnly";
            this.btnRenameOnly.Size = new System.Drawing.Size(91, 23);
            this.btnRenameOnly.TabIndex = 0;
            this.btnRenameOnly.Text = "Rename Only";
            this.btnRenameOnly.UseVisualStyleBackColor = true;
            this.btnRenameOnly.Click += new System.EventHandler(this.btnRenameOnly_Click);
            // 
            // lstInputFolder
            // 
            this.lstInputFolder.CheckBoxes = true;
            this.lstInputFolder.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6});
            this.lstInputFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstInputFolder.Location = new System.Drawing.Point(0, 0);
            this.lstInputFolder.Name = "lstInputFolder";
            this.lstInputFolder.Size = new System.Drawing.Size(913, 464);
            this.lstInputFolder.TabIndex = 0;
            this.lstInputFolder.UseCompatibleStateImageBehavior = false;
            this.lstInputFolder.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Input Path";
            this.columnHeader1.Width = 212;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "TV Show";
            this.columnHeader2.Width = 128;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Season";
            this.columnHeader3.Width = 59;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Episode";
            this.columnHeader4.Width = 53;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Episode Name";
            this.columnHeader5.Width = 123;
            // 
            // columnHeader6
            // 
            this.columnHeader6.Text = "Output Path";
            this.columnHeader6.Width = 205;
            // 
            // tpgTVShows
            // 
            this.tpgTVShows.Controls.Add(this.flShowControls);
            this.tpgTVShows.Controls.Add(this.groupBox1);
            this.tpgTVShows.Controls.Add(this.grpShowCustomFormat);
            this.tpgTVShows.Controls.Add(this.flowLayoutPanel1);
            this.tpgTVShows.Controls.Add(this.lblLastUpdate);
            this.tpgTVShows.Controls.Add(this.lblTvdbId);
            this.tpgTVShows.Controls.Add(this.picShowPic);
            this.tpgTVShows.Controls.Add(this.lblSelectedShow);
            this.tpgTVShows.Controls.Add(this.lstTVShows);
            this.tpgTVShows.Location = new System.Drawing.Point(4, 22);
            this.tpgTVShows.Name = "tpgTVShows";
            this.tpgTVShows.Padding = new System.Windows.Forms.Padding(3);
            this.tpgTVShows.Size = new System.Drawing.Size(919, 504);
            this.tpgTVShows.TabIndex = 1;
            this.tpgTVShows.Text = "TV Shows";
            this.tpgTVShows.UseVisualStyleBackColor = true;
            // 
            // flShowControls
            // 
            this.flShowControls.Controls.Add(this.btnApplyShowSetttings);
            this.flShowControls.Controls.Add(this.btnSaveShowSettings);
            this.flShowControls.Controls.Add(this.btnResetShowSettings);
            this.flShowControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.flShowControls.Location = new System.Drawing.Point(212, 375);
            this.flShowControls.Name = "flShowControls";
            this.flShowControls.Size = new System.Drawing.Size(704, 32);
            this.flShowControls.TabIndex = 9;
            // 
            // btnApplyShowSetttings
            // 
            this.btnApplyShowSetttings.Location = new System.Drawing.Point(3, 3);
            this.btnApplyShowSetttings.Name = "btnApplyShowSetttings";
            this.btnApplyShowSetttings.Size = new System.Drawing.Size(75, 23);
            this.btnApplyShowSetttings.TabIndex = 0;
            this.btnApplyShowSetttings.Text = "Apply";
            this.btnApplyShowSetttings.UseVisualStyleBackColor = true;
            this.btnApplyShowSetttings.Click += new System.EventHandler(this.btnApplyShowSetttings_Click);
            // 
            // btnSaveShowSettings
            // 
            this.btnSaveShowSettings.Location = new System.Drawing.Point(84, 3);
            this.btnSaveShowSettings.Name = "btnSaveShowSettings";
            this.btnSaveShowSettings.Size = new System.Drawing.Size(75, 23);
            this.btnSaveShowSettings.TabIndex = 1;
            this.btnSaveShowSettings.Text = "Save";
            this.btnSaveShowSettings.UseVisualStyleBackColor = true;
            this.btnSaveShowSettings.Click += new System.EventHandler(this.btnSaveShowSettings_Click);
            // 
            // btnResetShowSettings
            // 
            this.btnResetShowSettings.Location = new System.Drawing.Point(165, 3);
            this.btnResetShowSettings.Name = "btnResetShowSettings";
            this.btnResetShowSettings.Size = new System.Drawing.Size(75, 23);
            this.btnResetShowSettings.TabIndex = 2;
            this.btnResetShowSettings.Text = "Reset";
            this.btnResetShowSettings.UseVisualStyleBackColor = true;
            this.btnResetShowSettings.Click += new System.EventHandler(this.btnResetShowSettings_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtAltNames);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtShowFolderName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(212, 271);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(704, 104);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Custom Names";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 68);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(675, 26);
            this.label11.TabIndex = 7;
            this.label11.Text = resources.GetString("label11.Text");
            // 
            // txtAltNames
            // 
            this.txtAltNames.Location = new System.Drawing.Point(110, 45);
            this.txtAltNames.Name = "txtAltNames";
            this.txtAltNames.Size = new System.Drawing.Size(588, 20);
            this.txtAltNames.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Alternate names";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(67, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Folder Name";
            // 
            // txtShowFolderName
            // 
            this.txtShowFolderName.Location = new System.Drawing.Point(109, 19);
            this.txtShowFolderName.Name = "txtShowFolderName";
            this.txtShowFolderName.Size = new System.Drawing.Size(589, 20);
            this.txtShowFolderName.TabIndex = 4;
            // 
            // grpShowCustomFormat
            // 
            this.grpShowCustomFormat.Controls.Add(this.txtShowExampleFileName);
            this.grpShowCustomFormat.Controls.Add(this.label10);
            this.grpShowCustomFormat.Controls.Add(this.txtShowCustomFormat);
            this.grpShowCustomFormat.Controls.Add(this.label8);
            this.grpShowCustomFormat.Controls.Add(this.chkUseDefaultFormat);
            this.grpShowCustomFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShowCustomFormat.Location = new System.Drawing.Point(212, 178);
            this.grpShowCustomFormat.Name = "grpShowCustomFormat";
            this.grpShowCustomFormat.Size = new System.Drawing.Size(704, 93);
            this.grpShowCustomFormat.TabIndex = 8;
            this.grpShowCustomFormat.TabStop = false;
            this.grpShowCustomFormat.Text = "Custom Format";
            // 
            // txtShowExampleFileName
            // 
            this.txtShowExampleFileName.Location = new System.Drawing.Point(109, 62);
            this.txtShowExampleFileName.Name = "txtShowExampleFileName";
            this.txtShowExampleFileName.ReadOnly = true;
            this.txtShowExampleFileName.Size = new System.Drawing.Size(589, 20);
            this.txtShowExampleFileName.TabIndex = 6;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 65);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(97, 13);
            this.label10.TabIndex = 5;
            this.label10.Text = "Example File Name";
            // 
            // txtShowCustomFormat
            // 
            this.txtShowCustomFormat.Location = new System.Drawing.Point(109, 36);
            this.txtShowCustomFormat.Name = "txtShowCustomFormat";
            this.txtShowCustomFormat.Size = new System.Drawing.Size(589, 20);
            this.txtShowCustomFormat.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 39);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Custom Format ";
            // 
            // chkUseDefaultFormat
            // 
            this.chkUseDefaultFormat.AutoSize = true;
            this.chkUseDefaultFormat.Location = new System.Drawing.Point(6, 19);
            this.chkUseDefaultFormat.Name = "chkUseDefaultFormat";
            this.chkUseDefaultFormat.Size = new System.Drawing.Size(117, 17);
            this.chkUseDefaultFormat.TabIndex = 0;
            this.chkUseDefaultFormat.Text = "Use Default Format";
            this.chkUseDefaultFormat.UseVisualStyleBackColor = true;
            this.chkUseDefaultFormat.CheckedChanged += new System.EventHandler(this.chkUseDefaultFormat_CheckedChanged);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.btnUpdateSelected);
            this.flowLayoutPanel1.Controls.Add(this.btnUpdateAll);
            this.flowLayoutPanel1.Controls.Add(this.btnForceUpdateAll);
            this.flowLayoutPanel1.Controls.Add(this.btnAddShow);
            this.flowLayoutPanel1.Controls.Add(this.btnRemoveShow);
            this.flowLayoutPanel1.Controls.Add(this.btnSearchShows);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(212, 470);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(704, 31);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // btnUpdateSelected
            // 
            this.btnUpdateSelected.Location = new System.Drawing.Point(3, 3);
            this.btnUpdateSelected.Name = "btnUpdateSelected";
            this.btnUpdateSelected.Size = new System.Drawing.Size(153, 23);
            this.btnUpdateSelected.TabIndex = 5;
            this.btnUpdateSelected.Text = "Update Selected Show";
            this.btnUpdateSelected.UseVisualStyleBackColor = true;
            this.btnUpdateSelected.Click += new System.EventHandler(this.btnUpdateSelected_Click);
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.Location = new System.Drawing.Point(162, 3);
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateAll.TabIndex = 6;
            this.btnUpdateAll.Text = "Update All";
            this.btnUpdateAll.UseVisualStyleBackColor = true;
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // btnForceUpdateAll
            // 
            this.btnForceUpdateAll.Location = new System.Drawing.Point(243, 3);
            this.btnForceUpdateAll.Name = "btnForceUpdateAll";
            this.btnForceUpdateAll.Size = new System.Drawing.Size(107, 23);
            this.btnForceUpdateAll.TabIndex = 10;
            this.btnForceUpdateAll.Text = "Force Update All";
            this.btnForceUpdateAll.UseVisualStyleBackColor = true;
            this.btnForceUpdateAll.Click += new System.EventHandler(this.btnForceUpdateAll_Click);
            // 
            // btnAddShow
            // 
            this.btnAddShow.Location = new System.Drawing.Point(356, 3);
            this.btnAddShow.Name = "btnAddShow";
            this.btnAddShow.Size = new System.Drawing.Size(75, 23);
            this.btnAddShow.TabIndex = 7;
            this.btnAddShow.Text = "Add Show";
            this.btnAddShow.UseVisualStyleBackColor = true;
            this.btnAddShow.Click += new System.EventHandler(this.btnAddShow_Click);
            // 
            // btnRemoveShow
            // 
            this.btnRemoveShow.Location = new System.Drawing.Point(437, 3);
            this.btnRemoveShow.Name = "btnRemoveShow";
            this.btnRemoveShow.Size = new System.Drawing.Size(90, 23);
            this.btnRemoveShow.TabIndex = 8;
            this.btnRemoveShow.Text = "Remove Show";
            this.btnRemoveShow.UseVisualStyleBackColor = true;
            this.btnRemoveShow.Click += new System.EventHandler(this.btnRemoveShow_Click);
            // 
            // btnSearchShows
            // 
            this.btnSearchShows.Location = new System.Drawing.Point(533, 3);
            this.btnSearchShows.Name = "btnSearchShows";
            this.btnSearchShows.Size = new System.Drawing.Size(104, 23);
            this.btnSearchShows.TabIndex = 9;
            this.btnSearchShows.Text = "Search for Shows";
            this.btnSearchShows.UseVisualStyleBackColor = true;
            this.btnSearchShows.Click += new System.EventHandler(this.btnSearchShows_Click);
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLastUpdate.Location = new System.Drawing.Point(212, 165);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(68, 13);
            this.lblLastUpdate.TabIndex = 3;
            this.lblLastUpdate.Text = "Last Update:";
            // 
            // lblTvdbId
            // 
            this.lblTvdbId.AutoSize = true;
            this.lblTvdbId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTvdbId.Location = new System.Drawing.Point(212, 152);
            this.lblTvdbId.Name = "lblTvdbId";
            this.lblTvdbId.Size = new System.Drawing.Size(56, 13);
            this.lblTvdbId.TabIndex = 2;
            this.lblTvdbId.Text = "TVDB ID: ";
            // 
            // picShowPic
            // 
            this.picShowPic.Dock = System.Windows.Forms.DockStyle.Top;
            this.picShowPic.Location = new System.Drawing.Point(212, 23);
            this.picShowPic.Name = "picShowPic";
            this.picShowPic.Size = new System.Drawing.Size(704, 129);
            this.picShowPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picShowPic.TabIndex = 7;
            this.picShowPic.TabStop = false;
            // 
            // lblSelectedShow
            // 
            this.lblSelectedShow.AutoSize = true;
            this.lblSelectedShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedShow.Location = new System.Drawing.Point(212, 3);
            this.lblSelectedShow.Name = "lblSelectedShow";
            this.lblSelectedShow.Size = new System.Drawing.Size(150, 20);
            this.lblSelectedShow.TabIndex = 1;
            this.lblSelectedShow.Text = "No show selected";
            // 
            // lstTVShows
            // 
            this.lstTVShows.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstTVShows.FormattingEnabled = true;
            this.lstTVShows.Location = new System.Drawing.Point(3, 3);
            this.lstTVShows.Name = "lstTVShows";
            this.lstTVShows.Size = new System.Drawing.Size(209, 498);
            this.lstTVShows.TabIndex = 0;
            this.lstTVShows.SelectedIndexChanged += new System.EventHandler(this.lstTVShows_SelectedIndexChanged);
            // 
            // tpgSettings
            // 
            this.tpgSettings.AutoScroll = true;
            this.tpgSettings.Controls.Add(this.flowLayoutPanel2);
            this.tpgSettings.Controls.Add(this.grpShowNames);
            this.tpgSettings.Controls.Add(this.grpDirectories);
            this.tpgSettings.Location = new System.Drawing.Point(4, 22);
            this.tpgSettings.Name = "tpgSettings";
            this.tpgSettings.Size = new System.Drawing.Size(919, 504);
            this.tpgSettings.TabIndex = 2;
            this.tpgSettings.Text = "Settings";
            this.tpgSettings.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnApply);
            this.flowLayoutPanel2.Controls.Add(this.btnSave);
            this.flowLayoutPanel2.Controls.Add(this.btnReset);
            this.flowLayoutPanel2.Controls.Add(this.btnRevert);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(0, 262);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(919, 30);
            this.flowLayoutPanel2.TabIndex = 10;
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(3, 3);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(84, 3);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(165, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 2;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(246, 3);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(106, 23);
            this.btnRevert.TabIndex = 3;
            this.btnRevert.Text = "Revert To Saved";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // grpShowNames
            // 
            this.grpShowNames.Controls.Add(this.label7);
            this.grpShowNames.Controls.Add(this.label6);
            this.grpShowNames.Controls.Add(this.label5);
            this.grpShowNames.Controls.Add(this.txtExampleName);
            this.grpShowNames.Controls.Add(this.txtNameFormat);
            this.grpShowNames.Controls.Add(this.label1);
            this.grpShowNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShowNames.Location = new System.Drawing.Point(0, 113);
            this.grpShowNames.Name = "grpShowNames";
            this.grpShowNames.Size = new System.Drawing.Size(919, 149);
            this.grpShowNames.TabIndex = 9;
            this.grpShowNames.TabStop = false;
            this.grpShowNames.Text = "Default File Name Format";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(328, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(342, 52);
            this.label7.TabIndex = 5;
            this.label7.Text = "\r\n{EName(#)} - The episode name (# is character to separate words)\r\n{Date(#)} - T" +
                "he date the epsiode was aired (# is .Net date format string)\r\n{Ext} - The file e" +
                "xtension";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(314, 65);
            this.label6.TabIndex = 4;
            this.label6.Text = resources.GetString("label6.Text");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 120);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Example File Name:";
            // 
            // txtExampleName
            // 
            this.txtExampleName.Location = new System.Drawing.Point(114, 117);
            this.txtExampleName.Name = "txtExampleName";
            this.txtExampleName.ReadOnly = true;
            this.txtExampleName.Size = new System.Drawing.Size(556, 20);
            this.txtExampleName.TabIndex = 3;
            // 
            // txtNameFormat
            // 
            this.txtNameFormat.Location = new System.Drawing.Point(114, 91);
            this.txtNameFormat.Name = "txtNameFormat";
            this.txtNameFormat.Size = new System.Drawing.Size(556, 20);
            this.txtNameFormat.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Output Format:";
            // 
            // grpDirectories
            // 
            this.grpDirectories.Controls.Add(this.chkDeleteEmpty);
            this.grpDirectories.Controls.Add(this.label2);
            this.grpDirectories.Controls.Add(this.txtInputFolder);
            this.grpDirectories.Controls.Add(this.btnBrowseInputFolder);
            this.grpDirectories.Controls.Add(this.label3);
            this.grpDirectories.Controls.Add(this.chkRecurse);
            this.grpDirectories.Controls.Add(this.txtOutputFolder);
            this.grpDirectories.Controls.Add(this.btnBrowseOutputFolder);
            this.grpDirectories.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpDirectories.Location = new System.Drawing.Point(0, 0);
            this.grpDirectories.Name = "grpDirectories";
            this.grpDirectories.Size = new System.Drawing.Size(919, 113);
            this.grpDirectories.TabIndex = 8;
            this.grpDirectories.TabStop = false;
            this.grpDirectories.Text = "Directories";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Input folder:";
            // 
            // txtInputFolder
            // 
            this.txtInputFolder.Location = new System.Drawing.Point(83, 13);
            this.txtInputFolder.Name = "txtInputFolder";
            this.txtInputFolder.ReadOnly = true;
            this.txtInputFolder.Size = new System.Drawing.Size(211, 20);
            this.txtInputFolder.TabIndex = 1;
            // 
            // btnBrowseInputFolder
            // 
            this.btnBrowseInputFolder.Location = new System.Drawing.Point(300, 13);
            this.btnBrowseInputFolder.Name = "btnBrowseInputFolder";
            this.btnBrowseInputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseInputFolder.TabIndex = 2;
            this.btnBrowseInputFolder.Text = "Browse";
            this.btnBrowseInputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseInputFolder.Click += new System.EventHandler(this.btnBrowseInputFolder_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Output folder:";
            // 
            // chkRecurse
            // 
            this.chkRecurse.AutoSize = true;
            this.chkRecurse.Location = new System.Drawing.Point(9, 65);
            this.chkRecurse.Name = "chkRecurse";
            this.chkRecurse.Size = new System.Drawing.Size(136, 17);
            this.chkRecurse.TabIndex = 6;
            this.chkRecurse.Text = "Recurse Subdirectories";
            this.chkRecurse.UseVisualStyleBackColor = true;
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(83, 39);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.ReadOnly = true;
            this.txtOutputFolder.Size = new System.Drawing.Size(211, 20);
            this.txtOutputFolder.TabIndex = 4;
            // 
            // btnBrowseOutputFolder
            // 
            this.btnBrowseOutputFolder.Location = new System.Drawing.Point(300, 37);
            this.btnBrowseOutputFolder.Name = "btnBrowseOutputFolder";
            this.btnBrowseOutputFolder.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseOutputFolder.TabIndex = 5;
            this.btnBrowseOutputFolder.Text = "Browse";
            this.btnBrowseOutputFolder.UseVisualStyleBackColor = true;
            this.btnBrowseOutputFolder.Click += new System.EventHandler(this.btnBrowseOutputFolder_Click);
            // 
            // tpgLog
            // 
            this.tpgLog.Controls.Add(this.lstLog);
            this.tpgLog.Location = new System.Drawing.Point(4, 22);
            this.tpgLog.Name = "tpgLog";
            this.tpgLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpgLog.Size = new System.Drawing.Size(919, 504);
            this.tpgLog.TabIndex = 3;
            this.tpgLog.Text = "Log";
            this.tpgLog.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.Location = new System.Drawing.Point(3, 3);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(913, 498);
            this.lstLog.TabIndex = 0;
            // 
            // chkDeleteEmpty
            // 
            this.chkDeleteEmpty.AutoSize = true;
            this.chkDeleteEmpty.Location = new System.Drawing.Point(9, 88);
            this.chkDeleteEmpty.Name = "chkDeleteEmpty";
            this.chkDeleteEmpty.Size = new System.Drawing.Size(165, 17);
            this.chkDeleteEmpty.TabIndex = 7;
            this.chkDeleteEmpty.Text = "Remove empty subdirectories";
            this.chkDeleteEmpty.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(927, 530);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Text = "TV Sorter";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpgInputFolder.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.tpgTVShows.ResumeLayout(false);
            this.tpgTVShows.PerformLayout();
            this.flShowControls.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpShowCustomFormat.ResumeLayout(false);
            this.grpShowCustomFormat.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picShowPic)).EndInit();
            this.tpgSettings.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.grpShowNames.ResumeLayout(false);
            this.grpShowNames.PerformLayout();
            this.grpDirectories.ResumeLayout(false);
            this.grpDirectories.PerformLayout();
            this.tpgLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpgInputFolder;
        private System.Windows.Forms.TabPage tpgTVShows;
        private System.Windows.Forms.ListView lstInputFolder;
        private System.Windows.Forms.Label lblSelectedShow;
        private System.Windows.Forms.ListBox lstTVShows;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button btnRenameMove;
        private System.Windows.Forms.Button btnRenameOnly;
        private System.Windows.Forms.Button btnDeselectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Label lblTvdbId;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnUpdateSelected;
        private System.Windows.Forms.Button btnUpdateAll;
        private System.Windows.Forms.Button btnAddShow;
        private System.Windows.Forms.Button btnRemoveShow;
        private System.Windows.Forms.Button btnSearchShows;
        private System.Windows.Forms.TabPage tpgSettings;
        private System.Windows.Forms.GroupBox grpShowNames;
        private System.Windows.Forms.GroupBox grpDirectories;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Button btnBrowseInputFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRecurse;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtExampleName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNameFormat;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderSelect;
        private System.Windows.Forms.PictureBox picShowPic;
        private System.Windows.Forms.GroupBox grpShowCustomFormat;
        private System.Windows.Forms.TextBox txtShowCustomFormat;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkUseDefaultFormat;
        private System.Windows.Forms.TextBox txtShowExampleFileName;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtShowFolderName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FlowLayoutPanel flShowControls;
        private System.Windows.Forms.Button btnApplyShowSetttings;
        private System.Windows.Forms.Button btnSaveShowSettings;
        private System.Windows.Forms.Button btnResetShowSettings;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.Button btnSetShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAltNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnForceUpdateAll;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.TabPage tpgLog;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ComboBox cboFolderFilter;
        private System.Windows.Forms.CheckBox chkDeleteEmpty;
    }
}

