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
            this.lstInputFolder = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSort = new System.Windows.Forms.ToolStripSplitButton();
            this.btnRenameOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRenameMove = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRenameCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSetShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnDeselectAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.cboFolderFilter = new System.Windows.Forms.ToolStripComboBox();
            this.tpgTVShows = new System.Windows.Forms.TabPage();
            this.tsShowControls = new System.Windows.Forms.ToolStrip();
            this.btnSaveShowSettings = new System.Windows.Forms.ToolStripButton();
            this.btnResetShowSettings = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdateSelected = new System.Windows.Forms.ToolStripButton();
            this.btnForceUpdateSelected = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemoveShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.grpShowUpdateOptions = new System.Windows.Forms.GroupBox();
            this.chkDvdOrder = new System.Windows.Forms.CheckBox();
            this.btnLockShow = new System.Windows.Forms.Button();
            this.grpShowCustomNames = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtAltNames = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtShowFolderName = new System.Windows.Forms.TextBox();
            this.grpShowCustomFormat = new System.Windows.Forms.GroupBox();
            this.txtShowCustomFormat = new System.Windows.Forms.TextBox();
            this.txtShowExampleFileName = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chkUseDefaultFormat = new System.Windows.Forms.CheckBox();
            this.lblLastUpdate = new System.Windows.Forms.Label();
            this.lblTvdbId = new System.Windows.Forms.Label();
            this.picShowPic = new System.Windows.Forms.PictureBox();
            this.lblSelectedShow = new System.Windows.Forms.Label();
            this.lstTVShows = new System.Windows.Forms.ListBox();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnUpdateAll = new System.Windows.Forms.ToolStripButton();
            this.btnForceUpdateAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddShow = new System.Windows.Forms.ToolStripButton();
            this.btnSearchShow = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnLockAll = new System.Windows.Forms.ToolStripButton();
            this.btnUnlockAll = new System.Windows.Forms.ToolStripButton();
            this.tpgMissingEps = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvMissingEps = new System.Windows.Forms.TreeView();
            this.lblSearchResults = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkSkipPart2 = new System.Windows.Forms.CheckBox();
            this.rdoDuplicateEps = new System.Windows.Forms.RadioButton();
            this.chkSkipMissingSeasons = new System.Windows.Forms.CheckBox();
            this.rdoSearchMissingEps = new System.Windows.Forms.RadioButton();
            this.chkSkipSeason0 = new System.Windows.Forms.CheckBox();
            this.btnRefreshFileList = new System.Windows.Forms.Button();
            this.btnSearchMissingEpisodes = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.tpgSettings = new System.Windows.Forms.TabPage();
            this.settingsSubTabs = new System.Windows.Forms.TabControl();
            this.generalSettings = new System.Windows.Forms.TabPage();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnRevert = new System.Windows.Forms.Button();
            this.grpShowNames = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtExampleName = new System.Windows.Forms.TextBox();
            this.txtNameFormat = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grpDirectories = new System.Windows.Forms.GroupBox();
            this.chkDeleteEmpty = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseInputFolder = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.chkRecurse = new System.Windows.Forms.CheckBox();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.btnBrowseOutputFolder = new System.Windows.Forms.Button();
            this.regularExpressionsTabPage = new System.Windows.Forms.TabPage();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.btnApplyRegexp = new System.Windows.Forms.ToolStripButton();
            this.btnSaveRegexp = new System.Windows.Forms.ToolStripButton();
            this.btnRevertRegexp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMoveUpRegexp = new System.Windows.Forms.ToolStripButton();
            this.btnMoveDownRegexp = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRemoveRegexp = new System.Windows.Forms.ToolStripButton();
            this.lstRegexp = new System.Windows.Forms.ListBox();
            this.grpAddRegexp = new System.Windows.Forms.GroupBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnAddRegexp = new System.Windows.Forms.ToolStripButton();
            this.txtRegexp = new System.Windows.Forms.TextBox();
            this.grpRegexpInfo = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tpgLog = new System.Windows.Forms.TabPage();
            this.lstLog = new System.Windows.Forms.ListBox();
            this.dlgFolderSelect = new System.Windows.Forms.FolderBrowserDialog();
            this.chkSkipLocked = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tpgInputFolder.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.tpgTVShows.SuspendLayout();
            this.tsShowControls.SuspendLayout();
            this.grpShowUpdateOptions.SuspendLayout();
            this.grpShowCustomNames.SuspendLayout();
            this.grpShowCustomFormat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowPic)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.tpgMissingEps.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpgSettings.SuspendLayout();
            this.settingsSubTabs.SuspendLayout();
            this.generalSettings.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.grpShowNames.SuspendLayout();
            this.grpDirectories.SuspendLayout();
            this.regularExpressionsTabPage.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            this.grpAddRegexp.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.grpRegexpInfo.SuspendLayout();
            this.tpgLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpgInputFolder);
            this.tabControl1.Controls.Add(this.tpgTVShows);
            this.tabControl1.Controls.Add(this.tpgMissingEps);
            this.tabControl1.Controls.Add(this.tpgSettings);
            this.tabControl1.Controls.Add(this.tpgLog);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(944, 530);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tpgInputFolder
            // 
            this.tpgInputFolder.Controls.Add(this.lstInputFolder);
            this.tpgInputFolder.Controls.Add(this.toolStrip1);
            this.tpgInputFolder.Location = new System.Drawing.Point(4, 22);
            this.tpgInputFolder.Name = "tpgInputFolder";
            this.tpgInputFolder.Padding = new System.Windows.Forms.Padding(3);
            this.tpgInputFolder.Size = new System.Drawing.Size(936, 504);
            this.tpgInputFolder.TabIndex = 0;
            this.tpgInputFolder.Text = "Input folder";
            this.tpgInputFolder.UseVisualStyleBackColor = true;
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
            this.lstInputFolder.Location = new System.Drawing.Point(3, 28);
            this.lstInputFolder.Name = "lstInputFolder";
            this.lstInputFolder.Size = new System.Drawing.Size(930, 473);
            this.lstInputFolder.TabIndex = 10;
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
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSort,
            this.toolStripSeparator1,
            this.btnSetShow,
            this.toolStripSeparator2,
            this.btnSelectAll,
            this.btnDeselectAll,
            this.toolStripSeparator7,
            this.btnRefresh,
            this.cboFolderFilter});
            this.toolStrip1.Location = new System.Drawing.Point(3, 3);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(930, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStripInputFolder";
            // 
            // btnSort
            // 
            this.btnSort.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSort.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRenameOnly,
            this.btnRenameMove,
            this.btnRenameCopy});
            this.btnSort.Image = ((System.Drawing.Image)(resources.GetObject("btnSort.Image")));
            this.btnSort.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(44, 22);
            this.btnSort.Text = "Sort";
            this.btnSort.ButtonClick += new System.EventHandler(this.btnSort_ButtonClick);
            // 
            // btnRenameOnly
            // 
            this.btnRenameOnly.Name = "btnRenameOnly";
            this.btnRenameOnly.Size = new System.Drawing.Size(163, 22);
            this.btnRenameOnly.Text = "&Rename Only";
            this.btnRenameOnly.Click += new System.EventHandler(this.btnRenameOnly_Click);
            // 
            // btnRenameMove
            // 
            this.btnRenameMove.Name = "btnRenameMove";
            this.btnRenameMove.Size = new System.Drawing.Size(163, 22);
            this.btnRenameMove.Text = "Rename && &Move";
            this.btnRenameMove.Click += new System.EventHandler(this.btnRenameMove_Click);
            // 
            // btnRenameCopy
            // 
            this.btnRenameCopy.Name = "btnRenameCopy";
            this.btnRenameCopy.Size = new System.Drawing.Size(163, 22);
            this.btnRenameCopy.Text = "Rename && &Copy";
            this.btnRenameCopy.Click += new System.EventHandler(this.btnRenameCopy_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSetShow
            // 
            this.btnSetShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSetShow.Image = ((System.Drawing.Image)(resources.GetObject("btnSetShow.Image")));
            this.btnSetShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetShow.Name = "btnSetShow";
            this.btnSetShow.Size = new System.Drawing.Size(59, 22);
            this.btnSetShow.Text = "Set Show";
            this.btnSetShow.Click += new System.EventHandler(this.btnSetShow_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectAll.Image")));
            this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(59, 22);
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnDeselectAll
            // 
            this.btnDeselectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDeselectAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDeselectAll.Image")));
            this.btnDeselectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeselectAll.Name = "btnDeselectAll";
            this.btnDeselectAll.Size = new System.Drawing.Size(72, 22);
            this.btnDeselectAll.Text = "Deselect All";
            this.btnDeselectAll.Click += new System.EventHandler(this.btnDeselectAll_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(50, 22);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // cboFolderFilter
            // 
            this.cboFolderFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFolderFilter.Name = "cboFolderFilter";
            this.cboFolderFilter.Size = new System.Drawing.Size(150, 25);
            // 
            // tpgTVShows
            // 
            this.tpgTVShows.Controls.Add(this.tsShowControls);
            this.tpgTVShows.Controls.Add(this.grpShowUpdateOptions);
            this.tpgTVShows.Controls.Add(this.grpShowCustomNames);
            this.tpgTVShows.Controls.Add(this.grpShowCustomFormat);
            this.tpgTVShows.Controls.Add(this.lblLastUpdate);
            this.tpgTVShows.Controls.Add(this.lblTvdbId);
            this.tpgTVShows.Controls.Add(this.picShowPic);
            this.tpgTVShows.Controls.Add(this.lblSelectedShow);
            this.tpgTVShows.Controls.Add(this.lstTVShows);
            this.tpgTVShows.Controls.Add(this.toolStrip2);
            this.tpgTVShows.Location = new System.Drawing.Point(4, 22);
            this.tpgTVShows.Name = "tpgTVShows";
            this.tpgTVShows.Padding = new System.Windows.Forms.Padding(3);
            this.tpgTVShows.Size = new System.Drawing.Size(936, 504);
            this.tpgTVShows.TabIndex = 1;
            this.tpgTVShows.Text = "TV Shows";
            this.tpgTVShows.UseVisualStyleBackColor = true;
            // 
            // tsShowControls
            // 
            this.tsShowControls.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSaveShowSettings,
            this.btnResetShowSettings,
            this.toolStripSeparator3,
            this.btnUpdateSelected,
            this.btnForceUpdateSelected,
            this.toolStripSeparator4,
            this.btnRemoveShow,
            this.toolStripSeparator5});
            this.tsShowControls.Location = new System.Drawing.Point(212, 442);
            this.tsShowControls.Name = "tsShowControls";
            this.tsShowControls.Size = new System.Drawing.Size(721, 25);
            this.tsShowControls.TabIndex = 12;
            // 
            // btnSaveShowSettings
            // 
            this.btnSaveShowSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveShowSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveShowSettings.Image")));
            this.btnSaveShowSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveShowSettings.Name = "btnSaveShowSettings";
            this.btnSaveShowSettings.Size = new System.Drawing.Size(35, 22);
            this.btnSaveShowSettings.Text = "Save";
            this.btnSaveShowSettings.Click += new System.EventHandler(this.btnSaveShowSettings_Click);
            // 
            // btnResetShowSettings
            // 
            this.btnResetShowSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnResetShowSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnResetShowSettings.Image")));
            this.btnResetShowSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnResetShowSettings.Name = "btnResetShowSettings";
            this.btnResetShowSettings.Size = new System.Drawing.Size(39, 22);
            this.btnResetShowSettings.Text = "Reset";
            this.btnResetShowSettings.Click += new System.EventHandler(this.btnResetShowSettings_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdateSelected
            // 
            this.btnUpdateSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUpdateSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateSelected.Image")));
            this.btnUpdateSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdateSelected.Name = "btnUpdateSelected";
            this.btnUpdateSelected.Size = new System.Drawing.Size(49, 22);
            this.btnUpdateSelected.Text = "Update";
            this.btnUpdateSelected.Click += new System.EventHandler(this.btnUpdateSelected_Click);
            // 
            // btnForceUpdateSelected
            // 
            this.btnForceUpdateSelected.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnForceUpdateSelected.Image = ((System.Drawing.Image)(resources.GetObject("btnForceUpdateSelected.Image")));
            this.btnForceUpdateSelected.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForceUpdateSelected.Name = "btnForceUpdateSelected";
            this.btnForceUpdateSelected.Size = new System.Drawing.Size(81, 22);
            this.btnForceUpdateSelected.Text = "Force Update";
            this.btnForceUpdateSelected.Click += new System.EventHandler(this.btnForceUpdateSelected_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemoveShow
            // 
            this.btnRemoveShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRemoveShow.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveShow.Image")));
            this.btnRemoveShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveShow.Name = "btnRemoveShow";
            this.btnRemoveShow.Size = new System.Drawing.Size(86, 22);
            this.btnRemoveShow.Text = "Remove Show";
            this.btnRemoveShow.Click += new System.EventHandler(this.btnRemoveShow_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // grpShowUpdateOptions
            // 
            this.grpShowUpdateOptions.Controls.Add(this.chkDvdOrder);
            this.grpShowUpdateOptions.Controls.Add(this.btnLockShow);
            this.grpShowUpdateOptions.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShowUpdateOptions.Location = new System.Drawing.Point(212, 397);
            this.grpShowUpdateOptions.Name = "grpShowUpdateOptions";
            this.grpShowUpdateOptions.Size = new System.Drawing.Size(721, 45);
            this.grpShowUpdateOptions.TabIndex = 13;
            this.grpShowUpdateOptions.TabStop = false;
            this.grpShowUpdateOptions.Text = "Update Options";
            // 
            // chkDvdOrder
            // 
            this.chkDvdOrder.AutoSize = true;
            this.chkDvdOrder.Dock = System.Windows.Forms.DockStyle.Left;
            this.chkDvdOrder.Location = new System.Drawing.Point(103, 16);
            this.chkDvdOrder.Name = "chkDvdOrder";
            this.chkDvdOrder.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.chkDvdOrder.Size = new System.Drawing.Size(110, 26);
            this.chkDvdOrder.TabIndex = 1;
            this.chkDvdOrder.Text = "Use DVD Order";
            this.chkDvdOrder.UseVisualStyleBackColor = true;
            // 
            // btnLockShow
            // 
            this.btnLockShow.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnLockShow.Location = new System.Drawing.Point(3, 16);
            this.btnLockShow.Name = "btnLockShow";
            this.btnLockShow.Size = new System.Drawing.Size(100, 26);
            this.btnLockShow.TabIndex = 0;
            this.btnLockShow.Text = "Show Locked";
            this.btnLockShow.UseVisualStyleBackColor = true;
            this.btnLockShow.Click += new System.EventHandler(this.btnLockShow_Click);
            // 
            // grpShowCustomNames
            // 
            this.grpShowCustomNames.Controls.Add(this.label11);
            this.grpShowCustomNames.Controls.Add(this.txtAltNames);
            this.grpShowCustomNames.Controls.Add(this.label4);
            this.grpShowCustomNames.Controls.Add(this.label9);
            this.grpShowCustomNames.Controls.Add(this.txtShowFolderName);
            this.grpShowCustomNames.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShowCustomNames.Location = new System.Drawing.Point(212, 293);
            this.grpShowCustomNames.Name = "grpShowCustomNames";
            this.grpShowCustomNames.Size = new System.Drawing.Size(721, 104);
            this.grpShowCustomNames.TabIndex = 10;
            this.grpShowCustomNames.TabStop = false;
            this.grpShowCustomNames.Text = "Custom Names";
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
            this.grpShowCustomFormat.Controls.Add(this.txtShowCustomFormat);
            this.grpShowCustomFormat.Controls.Add(this.txtShowExampleFileName);
            this.grpShowCustomFormat.Controls.Add(this.label10);
            this.grpShowCustomFormat.Controls.Add(this.label8);
            this.grpShowCustomFormat.Controls.Add(this.chkUseDefaultFormat);
            this.grpShowCustomFormat.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpShowCustomFormat.Location = new System.Drawing.Point(212, 200);
            this.grpShowCustomFormat.Name = "grpShowCustomFormat";
            this.grpShowCustomFormat.Size = new System.Drawing.Size(721, 93);
            this.grpShowCustomFormat.TabIndex = 8;
            this.grpShowCustomFormat.TabStop = false;
            this.grpShowCustomFormat.Text = "Custom Format";
            // 
            // txtShowCustomFormat
            // 
            this.txtShowCustomFormat.Location = new System.Drawing.Point(109, 36);
            this.txtShowCustomFormat.Name = "txtShowCustomFormat";
            this.txtShowCustomFormat.Size = new System.Drawing.Size(589, 20);
            this.txtShowCustomFormat.TabIndex = 2;
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
            // lblLastUpdate
            // 
            this.lblLastUpdate.AutoSize = true;
            this.lblLastUpdate.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLastUpdate.Location = new System.Drawing.Point(212, 187);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(68, 13);
            this.lblLastUpdate.TabIndex = 3;
            this.lblLastUpdate.Text = "Last Update:";
            // 
            // lblTvdbId
            // 
            this.lblTvdbId.AutoSize = true;
            this.lblTvdbId.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTvdbId.Location = new System.Drawing.Point(212, 174);
            this.lblTvdbId.Name = "lblTvdbId";
            this.lblTvdbId.Size = new System.Drawing.Size(56, 13);
            this.lblTvdbId.TabIndex = 2;
            this.lblTvdbId.Text = "TVDB ID: ";
            // 
            // picShowPic
            // 
            this.picShowPic.Dock = System.Windows.Forms.DockStyle.Top;
            this.picShowPic.Location = new System.Drawing.Point(212, 48);
            this.picShowPic.Name = "picShowPic";
            this.picShowPic.Size = new System.Drawing.Size(721, 126);
            this.picShowPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picShowPic.TabIndex = 7;
            this.picShowPic.TabStop = false;
            // 
            // lblSelectedShow
            // 
            this.lblSelectedShow.AutoSize = true;
            this.lblSelectedShow.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedShow.Location = new System.Drawing.Point(212, 28);
            this.lblSelectedShow.Name = "lblSelectedShow";
            this.lblSelectedShow.Size = new System.Drawing.Size(150, 20);
            this.lblSelectedShow.TabIndex = 1;
            this.lblSelectedShow.Text = "No show selected";
            // 
            // lstTVShows
            // 
            this.lstTVShows.Dock = System.Windows.Forms.DockStyle.Left;
            this.lstTVShows.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lstTVShows.FormattingEnabled = true;
            this.lstTVShows.ItemHeight = 20;
            this.lstTVShows.Location = new System.Drawing.Point(3, 28);
            this.lstTVShows.Name = "lstTVShows";
            this.lstTVShows.Size = new System.Drawing.Size(209, 473);
            this.lstTVShows.TabIndex = 0;
            this.lstTVShows.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstTVShows_DrawItem);
            this.lstTVShows.SelectedIndexChanged += new System.EventHandler(this.lstTVShows_SelectedIndexChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUpdateAll,
            this.btnForceUpdateAll,
            this.toolStripSeparator6,
            this.btnAddShow,
            this.btnSearchShow,
            this.toolStripSeparator8,
            this.btnLockAll,
            this.btnUnlockAll});
            this.toolStrip2.Location = new System.Drawing.Point(3, 3);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(930, 25);
            this.toolStrip2.TabIndex = 11;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnUpdateAll
            // 
            this.btnUpdateAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUpdateAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdateAll.Image")));
            this.btnUpdateAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdateAll.Name = "btnUpdateAll";
            this.btnUpdateAll.Size = new System.Drawing.Size(66, 22);
            this.btnUpdateAll.Text = "Update All";
            this.btnUpdateAll.Click += new System.EventHandler(this.btnUpdateAll_Click);
            // 
            // btnForceUpdateAll
            // 
            this.btnForceUpdateAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnForceUpdateAll.Image = ((System.Drawing.Image)(resources.GetObject("btnForceUpdateAll.Image")));
            this.btnForceUpdateAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnForceUpdateAll.Name = "btnForceUpdateAll";
            this.btnForceUpdateAll.Size = new System.Drawing.Size(98, 22);
            this.btnForceUpdateAll.Text = "Force Update All";
            this.btnForceUpdateAll.Click += new System.EventHandler(this.btnForceUpdateAll_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAddShow
            // 
            this.btnAddShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddShow.Image = ((System.Drawing.Image)(resources.GetObject("btnAddShow.Image")));
            this.btnAddShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddShow.Name = "btnAddShow";
            this.btnAddShow.Size = new System.Drawing.Size(65, 22);
            this.btnAddShow.Text = "Add Show";
            this.btnAddShow.Click += new System.EventHandler(this.btnAddShow_Click);
            // 
            // btnSearchShow
            // 
            this.btnSearchShow.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSearchShow.Image = ((System.Drawing.Image)(resources.GetObject("btnSearchShow.Image")));
            this.btnSearchShow.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearchShow.Name = "btnSearchShow";
            this.btnSearchShow.Size = new System.Drawing.Size(101, 22);
            this.btnSearchShow.Text = "Search for Shows";
            this.btnSearchShow.Click += new System.EventHandler(this.btnSearchShows_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnLockAll
            // 
            this.btnLockAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnLockAll.Image = ((System.Drawing.Image)(resources.GetObject("btnLockAll.Image")));
            this.btnLockAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnLockAll.Name = "btnLockAll";
            this.btnLockAll.Size = new System.Drawing.Size(53, 22);
            this.btnLockAll.Text = "Lock All";
            this.btnLockAll.Click += new System.EventHandler(this.btnLockAll_Click);
            // 
            // btnUnlockAll
            // 
            this.btnUnlockAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUnlockAll.Image = ((System.Drawing.Image)(resources.GetObject("btnUnlockAll.Image")));
            this.btnUnlockAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnlockAll.Name = "btnUnlockAll";
            this.btnUnlockAll.Size = new System.Drawing.Size(65, 22);
            this.btnUnlockAll.Text = "Unlock All";
            this.btnUnlockAll.Click += new System.EventHandler(this.btnUnlockAll_Click);
            // 
            // tpgMissingEps
            // 
            this.tpgMissingEps.Controls.Add(this.splitContainer1);
            this.tpgMissingEps.Location = new System.Drawing.Point(4, 22);
            this.tpgMissingEps.Name = "tpgMissingEps";
            this.tpgMissingEps.Size = new System.Drawing.Size(936, 504);
            this.tpgMissingEps.TabIndex = 4;
            this.tpgMissingEps.Text = "Missing/Duplicate Episodes";
            this.tpgMissingEps.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvMissingEps);
            this.splitContainer1.Panel1.Controls.Add(this.lblSearchResults);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.btnRefreshFileList);
            this.splitContainer1.Panel2.Controls.Add(this.btnSearchMissingEpisodes);
            this.splitContainer1.Panel2.Controls.Add(this.label13);
            this.splitContainer1.Size = new System.Drawing.Size(936, 504);
            this.splitContainer1.SplitterDistance = 249;
            this.splitContainer1.TabIndex = 0;
            // 
            // tvMissingEps
            // 
            this.tvMissingEps.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvMissingEps.Location = new System.Drawing.Point(0, 13);
            this.tvMissingEps.Name = "tvMissingEps";
            this.tvMissingEps.Size = new System.Drawing.Size(249, 491);
            this.tvMissingEps.TabIndex = 0;
            // 
            // lblSearchResults
            // 
            this.lblSearchResults.AutoSize = true;
            this.lblSearchResults.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSearchResults.Location = new System.Drawing.Point(0, 0);
            this.lblSearchResults.Name = "lblSearchResults";
            this.lblSearchResults.Size = new System.Drawing.Size(91, 13);
            this.lblSearchResults.TabIndex = 8;
            this.lblSearchResults.Text = "Search Results: 0";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkSkipLocked);
            this.groupBox1.Controls.Add(this.chkSkipPart2);
            this.groupBox1.Controls.Add(this.rdoDuplicateEps);
            this.groupBox1.Controls.Add(this.chkSkipMissingSeasons);
            this.groupBox1.Controls.Add(this.rdoSearchMissingEps);
            this.groupBox1.Controls.Add(this.chkSkipSeason0);
            this.groupBox1.Location = new System.Drawing.Point(6, 82);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(320, 136);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Search Options";
            // 
            // chkSkipPart2
            // 
            this.chkSkipPart2.AutoSize = true;
            this.chkSkipPart2.Location = new System.Drawing.Point(6, 88);
            this.chkSkipPart2.Name = "chkSkipPart2";
            this.chkSkipPart2.Size = new System.Drawing.Size(200, 17);
            this.chkSkipPart2.TabIndex = 7;
            this.chkSkipPart2.Text = "Skip Episode with name ending in (2)";
            this.chkSkipPart2.UseVisualStyleBackColor = true;
            // 
            // rdoDuplicateEps
            // 
            this.rdoDuplicateEps.AutoSize = true;
            this.rdoDuplicateEps.Location = new System.Drawing.Point(118, 19);
            this.rdoDuplicateEps.Name = "rdoDuplicateEps";
            this.rdoDuplicateEps.Size = new System.Drawing.Size(116, 17);
            this.rdoDuplicateEps.TabIndex = 1;
            this.rdoDuplicateEps.Text = "Duplicate Episodes";
            this.rdoDuplicateEps.UseVisualStyleBackColor = true;
            // 
            // chkSkipMissingSeasons
            // 
            this.chkSkipMissingSeasons.AutoSize = true;
            this.chkSkipMissingSeasons.Location = new System.Drawing.Point(6, 65);
            this.chkSkipMissingSeasons.Name = "chkSkipMissingSeasons";
            this.chkSkipMissingSeasons.Size = new System.Drawing.Size(159, 17);
            this.chkSkipMissingSeasons.TabIndex = 6;
            this.chkSkipMissingSeasons.Text = "Skip Entire Missing Seasons";
            this.chkSkipMissingSeasons.UseVisualStyleBackColor = true;
            // 
            // rdoSearchMissingEps
            // 
            this.rdoSearchMissingEps.AutoSize = true;
            this.rdoSearchMissingEps.Checked = true;
            this.rdoSearchMissingEps.Location = new System.Drawing.Point(6, 19);
            this.rdoSearchMissingEps.Name = "rdoSearchMissingEps";
            this.rdoSearchMissingEps.Size = new System.Drawing.Size(106, 17);
            this.rdoSearchMissingEps.TabIndex = 0;
            this.rdoSearchMissingEps.TabStop = true;
            this.rdoSearchMissingEps.Text = "Missing Episodes";
            this.rdoSearchMissingEps.UseVisualStyleBackColor = true;
            // 
            // chkSkipSeason0
            // 
            this.chkSkipSeason0.AutoSize = true;
            this.chkSkipSeason0.Location = new System.Drawing.Point(6, 42);
            this.chkSkipSeason0.Name = "chkSkipSeason0";
            this.chkSkipSeason0.Size = new System.Drawing.Size(144, 17);
            this.chkSkipSeason0.TabIndex = 2;
            this.chkSkipSeason0.Text = "Skip Season 0 (Specials)";
            this.chkSkipSeason0.UseVisualStyleBackColor = true;
            // 
            // btnRefreshFileList
            // 
            this.btnRefreshFileList.Location = new System.Drawing.Point(6, 53);
            this.btnRefreshFileList.Name = "btnRefreshFileList";
            this.btnRefreshFileList.Size = new System.Drawing.Size(106, 23);
            this.btnRefreshFileList.TabIndex = 5;
            this.btnRefreshFileList.Text = "Refresh File List";
            this.btnRefreshFileList.UseVisualStyleBackColor = true;
            this.btnRefreshFileList.Click += new System.EventHandler(this.btnRefreshFileList_Click);
            // 
            // btnSearchMissingEpisodes
            // 
            this.btnSearchMissingEpisodes.Location = new System.Drawing.Point(6, 224);
            this.btnSearchMissingEpisodes.Name = "btnSearchMissingEpisodes";
            this.btnSearchMissingEpisodes.Size = new System.Drawing.Size(75, 23);
            this.btnSearchMissingEpisodes.TabIndex = 1;
            this.btnSearchMissingEpisodes.Text = "Search";
            this.btnSearchMissingEpisodes.UseVisualStyleBackColor = true;
            this.btnSearchMissingEpisodes.Click += new System.EventHandler(this.btnSearchMissingEpisodes_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(2, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(388, 39);
            this.label13.TabIndex = 0;
            this.label13.Text = "This searches the output directory for episodes and compares it to the database. " +
                "\r\nAny database entries that aren\'t found in the output directory are shown on th" +
                "e \r\nleft as missing episodes.";
            // 
            // tpgSettings
            // 
            this.tpgSettings.AutoScroll = true;
            this.tpgSettings.Controls.Add(this.settingsSubTabs);
            this.tpgSettings.Location = new System.Drawing.Point(4, 22);
            this.tpgSettings.Name = "tpgSettings";
            this.tpgSettings.Size = new System.Drawing.Size(936, 504);
            this.tpgSettings.TabIndex = 2;
            this.tpgSettings.Text = "Settings";
            this.tpgSettings.UseVisualStyleBackColor = true;
            // 
            // settingsSubTabs
            // 
            this.settingsSubTabs.Controls.Add(this.generalSettings);
            this.settingsSubTabs.Controls.Add(this.regularExpressionsTabPage);
            this.settingsSubTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settingsSubTabs.Location = new System.Drawing.Point(0, 0);
            this.settingsSubTabs.Name = "settingsSubTabs";
            this.settingsSubTabs.SelectedIndex = 0;
            this.settingsSubTabs.Size = new System.Drawing.Size(936, 504);
            this.settingsSubTabs.TabIndex = 11;
            // 
            // generalSettings
            // 
            this.generalSettings.Controls.Add(this.flowLayoutPanel2);
            this.generalSettings.Controls.Add(this.grpShowNames);
            this.generalSettings.Controls.Add(this.grpDirectories);
            this.generalSettings.Location = new System.Drawing.Point(4, 22);
            this.generalSettings.Name = "generalSettings";
            this.generalSettings.Padding = new System.Windows.Forms.Padding(3);
            this.generalSettings.Size = new System.Drawing.Size(928, 478);
            this.generalSettings.TabIndex = 0;
            this.generalSettings.Text = "General Settings";
            this.generalSettings.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.btnApply);
            this.flowLayoutPanel2.Controls.Add(this.btnSave);
            this.flowLayoutPanel2.Controls.Add(this.btnRevert);
            this.flowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowLayoutPanel2.Location = new System.Drawing.Point(3, 265);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(922, 30);
            this.flowLayoutPanel2.TabIndex = 13;
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
            // btnRevert
            // 
            this.btnRevert.Location = new System.Drawing.Point(165, 3);
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
            this.grpShowNames.Location = new System.Drawing.Point(3, 116);
            this.grpShowNames.Name = "grpShowNames";
            this.grpShowNames.Size = new System.Drawing.Size(922, 149);
            this.grpShowNames.TabIndex = 12;
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
            this.grpDirectories.Location = new System.Drawing.Point(3, 3);
            this.grpDirectories.Name = "grpDirectories";
            this.grpDirectories.Size = new System.Drawing.Size(922, 113);
            this.grpDirectories.TabIndex = 11;
            this.grpDirectories.TabStop = false;
            this.grpDirectories.Text = "Directories";
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
            // regularExpressionsTabPage
            // 
            this.regularExpressionsTabPage.Controls.Add(this.toolStrip4);
            this.regularExpressionsTabPage.Controls.Add(this.lstRegexp);
            this.regularExpressionsTabPage.Controls.Add(this.grpAddRegexp);
            this.regularExpressionsTabPage.Controls.Add(this.grpRegexpInfo);
            this.regularExpressionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.regularExpressionsTabPage.Name = "regularExpressionsTabPage";
            this.regularExpressionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.regularExpressionsTabPage.Size = new System.Drawing.Size(928, 478);
            this.regularExpressionsTabPage.TabIndex = 1;
            this.regularExpressionsTabPage.Text = "Regular Expressions";
            this.regularExpressionsTabPage.UseVisualStyleBackColor = true;
            // 
            // toolStrip4
            // 
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnApplyRegexp,
            this.btnSaveRegexp,
            this.btnRevertRegexp,
            this.toolStripSeparator9,
            this.btnMoveUpRegexp,
            this.btnMoveDownRegexp,
            this.toolStripSeparator10,
            this.btnRemoveRegexp});
            this.toolStrip4.Location = new System.Drawing.Point(3, 371);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(922, 25);
            this.toolStrip4.TabIndex = 3;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // btnApplyRegexp
            // 
            this.btnApplyRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnApplyRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApplyRegexp.Name = "btnApplyRegexp";
            this.btnApplyRegexp.Size = new System.Drawing.Size(42, 22);
            this.btnApplyRegexp.Text = "Apply";
            this.btnApplyRegexp.Click += new System.EventHandler(this.btnApplyRegexp_Click);
            // 
            // btnSaveRegexp
            // 
            this.btnSaveRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnSaveRegexp.Image")));
            this.btnSaveRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveRegexp.Name = "btnSaveRegexp";
            this.btnSaveRegexp.Size = new System.Drawing.Size(35, 22);
            this.btnSaveRegexp.Text = "Save";
            this.btnSaveRegexp.Click += new System.EventHandler(this.btnSaveRegexp_Click);
            // 
            // btnRevertRegexp
            // 
            this.btnRevertRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRevertRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnRevertRegexp.Image")));
            this.btnRevertRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRevertRegexp.Name = "btnRevertRegexp";
            this.btnRevertRegexp.Size = new System.Drawing.Size(44, 22);
            this.btnRevertRegexp.Text = "Revert";
            this.btnRevertRegexp.Click += new System.EventHandler(this.btnRevertRegexp_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // btnMoveUpRegexp
            // 
            this.btnMoveUpRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMoveUpRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveUpRegexp.Image")));
            this.btnMoveUpRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveUpRegexp.Name = "btnMoveUpRegexp";
            this.btnMoveUpRegexp.Size = new System.Drawing.Size(59, 22);
            this.btnMoveUpRegexp.Text = "Move Up";
            this.btnMoveUpRegexp.Click += new System.EventHandler(this.btnMoveUpRegexp_Click);
            // 
            // btnMoveDownRegexp
            // 
            this.btnMoveDownRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnMoveDownRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnMoveDownRegexp.Image")));
            this.btnMoveDownRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMoveDownRegexp.Name = "btnMoveDownRegexp";
            this.btnMoveDownRegexp.Size = new System.Drawing.Size(75, 22);
            this.btnMoveDownRegexp.Text = "Move Down";
            this.btnMoveDownRegexp.Click += new System.EventHandler(this.btnMoveDownRegexp_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRemoveRegexp
            // 
            this.btnRemoveRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRemoveRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnRemoveRegexp.Image")));
            this.btnRemoveRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveRegexp.Name = "btnRemoveRegexp";
            this.btnRemoveRegexp.Size = new System.Drawing.Size(54, 22);
            this.btnRemoveRegexp.Text = "Remove";
            this.btnRemoveRegexp.Click += new System.EventHandler(this.btnRemoveRegexp_Click);
            // 
            // lstRegexp
            // 
            this.lstRegexp.Dock = System.Windows.Forms.DockStyle.Top;
            this.lstRegexp.FormattingEnabled = true;
            this.lstRegexp.Location = new System.Drawing.Point(3, 224);
            this.lstRegexp.Name = "lstRegexp";
            this.lstRegexp.Size = new System.Drawing.Size(922, 147);
            this.lstRegexp.TabIndex = 2;
            // 
            // grpAddRegexp
            // 
            this.grpAddRegexp.Controls.Add(this.toolStrip3);
            this.grpAddRegexp.Controls.Add(this.txtRegexp);
            this.grpAddRegexp.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpAddRegexp.Location = new System.Drawing.Point(3, 159);
            this.grpAddRegexp.Name = "grpAddRegexp";
            this.grpAddRegexp.Size = new System.Drawing.Size(922, 65);
            this.grpAddRegexp.TabIndex = 1;
            this.grpAddRegexp.TabStop = false;
            this.grpAddRegexp.Text = "Add Regular Expression";
            // 
            // toolStrip3
            // 
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddRegexp});
            this.toolStrip3.Location = new System.Drawing.Point(3, 36);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(916, 25);
            this.toolStrip3.TabIndex = 1;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // btnAddRegexp
            // 
            this.btnAddRegexp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddRegexp.Image = ((System.Drawing.Image)(resources.GetObject("btnAddRegexp.Image")));
            this.btnAddRegexp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddRegexp.Name = "btnAddRegexp";
            this.btnAddRegexp.Size = new System.Drawing.Size(33, 22);
            this.btnAddRegexp.Text = "Add";
            this.btnAddRegexp.Click += new System.EventHandler(this.btnAddRegexp_Click);
            // 
            // txtRegexp
            // 
            this.txtRegexp.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtRegexp.Location = new System.Drawing.Point(3, 16);
            this.txtRegexp.Name = "txtRegexp";
            this.txtRegexp.Size = new System.Drawing.Size(916, 20);
            this.txtRegexp.TabIndex = 0;
            // 
            // grpRegexpInfo
            // 
            this.grpRegexpInfo.Controls.Add(this.label12);
            this.grpRegexpInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpRegexpInfo.Location = new System.Drawing.Point(3, 3);
            this.grpRegexpInfo.Name = "grpRegexpInfo";
            this.grpRegexpInfo.Size = new System.Drawing.Size(922, 156);
            this.grpRegexpInfo.TabIndex = 0;
            this.grpRegexpInfo.TabStop = false;
            this.grpRegexpInfo.Text = "Regular Expression Information";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Top;
            this.label12.Location = new System.Drawing.Point(3, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(739, 130);
            this.label12.TabIndex = 0;
            this.label12.Text = resources.GetString("label12.Text");
            // 
            // tpgLog
            // 
            this.tpgLog.Controls.Add(this.lstLog);
            this.tpgLog.Location = new System.Drawing.Point(4, 22);
            this.tpgLog.Name = "tpgLog";
            this.tpgLog.Padding = new System.Windows.Forms.Padding(3);
            this.tpgLog.Size = new System.Drawing.Size(936, 504);
            this.tpgLog.TabIndex = 3;
            this.tpgLog.Text = "Log";
            this.tpgLog.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            this.lstLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstLog.FormattingEnabled = true;
            this.lstLog.HorizontalScrollbar = true;
            this.lstLog.Location = new System.Drawing.Point(3, 3);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(930, 498);
            this.lstLog.TabIndex = 0;
            // 
            // chkSkipLocked
            // 
            this.chkSkipLocked.AutoSize = true;
            this.chkSkipLocked.Location = new System.Drawing.Point(6, 111);
            this.chkSkipLocked.Name = "chkSkipLocked";
            this.chkSkipLocked.Size = new System.Drawing.Size(121, 17);
            this.chkSkipLocked.TabIndex = 8;
            this.chkSkipLocked.Text = "Skip Locked Shows";
            this.chkSkipLocked.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(944, 530);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Text = "TV Sorter";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpgInputFolder.ResumeLayout(false);
            this.tpgInputFolder.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tpgTVShows.ResumeLayout(false);
            this.tpgTVShows.PerformLayout();
            this.tsShowControls.ResumeLayout(false);
            this.tsShowControls.PerformLayout();
            this.grpShowUpdateOptions.ResumeLayout(false);
            this.grpShowUpdateOptions.PerformLayout();
            this.grpShowCustomNames.ResumeLayout(false);
            this.grpShowCustomNames.PerformLayout();
            this.grpShowCustomFormat.ResumeLayout(false);
            this.grpShowCustomFormat.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picShowPic)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.tpgMissingEps.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpgSettings.ResumeLayout(false);
            this.settingsSubTabs.ResumeLayout(false);
            this.generalSettings.ResumeLayout(false);
            this.flowLayoutPanel2.ResumeLayout(false);
            this.grpShowNames.ResumeLayout(false);
            this.grpShowNames.PerformLayout();
            this.grpDirectories.ResumeLayout(false);
            this.grpDirectories.PerformLayout();
            this.regularExpressionsTabPage.ResumeLayout(false);
            this.regularExpressionsTabPage.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            this.grpAddRegexp.ResumeLayout(false);
            this.grpAddRegexp.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.grpRegexpInfo.ResumeLayout(false);
            this.grpRegexpInfo.PerformLayout();
            this.tpgLog.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpgInputFolder;
        private System.Windows.Forms.TabPage tpgTVShows;
        private System.Windows.Forms.Label lblSelectedShow;
        private System.Windows.Forms.ListBox lstTVShows;
        private System.Windows.Forms.Label lblLastUpdate;
        private System.Windows.Forms.Label lblTvdbId;
        private System.Windows.Forms.TabPage tpgSettings;
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
        private System.Windows.Forms.GroupBox grpShowCustomNames;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtAltNames;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage tpgLog;
        private System.Windows.Forms.ListBox lstLog;
        private System.Windows.Forms.ListView lstInputFolder;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton btnSort;
        private System.Windows.Forms.ToolStripMenuItem btnRenameOnly;
        private System.Windows.Forms.ToolStripMenuItem btnRenameMove;
        private System.Windows.Forms.ToolStripMenuItem btnRenameCopy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSetShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSelectAll;
        private System.Windows.Forms.ToolStripButton btnDeselectAll;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripComboBox cboFolderFilter;
        private System.Windows.Forms.ToolStrip tsShowControls;
        private System.Windows.Forms.ToolStripButton btnSaveShowSettings;
        private System.Windows.Forms.ToolStripButton btnResetShowSettings;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnUpdateSelected;
        private System.Windows.Forms.ToolStripButton btnForceUpdateSelected;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnRemoveShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnUpdateAll;
        private System.Windows.Forms.ToolStripButton btnForceUpdateAll;
        private System.Windows.Forms.ToolStripButton btnAddShow;
        private System.Windows.Forms.ToolStripButton btnSearchShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.GroupBox grpShowUpdateOptions;
        private System.Windows.Forms.CheckBox chkDvdOrder;
        private System.Windows.Forms.Button btnLockShow;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnLockAll;
        private System.Windows.Forms.ToolStripButton btnUnlockAll;
        private System.Windows.Forms.TabControl settingsSubTabs;
        private System.Windows.Forms.TabPage generalSettings;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnRevert;
        private System.Windows.Forms.GroupBox grpShowNames;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtExampleName;
        private System.Windows.Forms.TextBox txtNameFormat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpDirectories;
        private System.Windows.Forms.CheckBox chkDeleteEmpty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInputFolder;
        private System.Windows.Forms.Button btnBrowseInputFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkRecurse;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button btnBrowseOutputFolder;
        private System.Windows.Forms.TabPage regularExpressionsTabPage;
        private System.Windows.Forms.GroupBox grpRegexpInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton btnApplyRegexp;
        private System.Windows.Forms.ToolStripButton btnSaveRegexp;
        private System.Windows.Forms.ToolStripButton btnRevertRegexp;
        private System.Windows.Forms.ListBox lstRegexp;
        private System.Windows.Forms.GroupBox grpAddRegexp;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton btnAddRegexp;
        private System.Windows.Forms.TextBox txtRegexp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton btnMoveUpRegexp;
        private System.Windows.Forms.ToolStripButton btnMoveDownRegexp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripButton btnRemoveRegexp;
        private System.Windows.Forms.TabPage tpgMissingEps;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvMissingEps;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnSearchMissingEpisodes;
        private System.Windows.Forms.CheckBox chkSkipSeason0;
        private System.Windows.Forms.Button btnRefreshFileList;
        private System.Windows.Forms.CheckBox chkSkipMissingSeasons;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoDuplicateEps;
        private System.Windows.Forms.RadioButton rdoSearchMissingEps;
        private System.Windows.Forms.CheckBox chkSkipPart2;
        private System.Windows.Forms.Label lblSearchResults;
        private System.Windows.Forms.CheckBox chkSkipLocked;
    }
}

