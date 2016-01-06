// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings tab.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.View
{
    #region Using Directives

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The settings tab.
    /// </summary>
    public partial class Settings
    {
        #region Constants and Fields


        /// <summary>
        ///   The add destination button.
        /// </summary>
        private Button addDestinationButton;

        /// <summary>
        ///   The delete empty check.
        /// </summary>
        private CheckBox deleteEmptyCheck;

        /// <summary>
        ///   The destination list.
        /// </summary>
        private ListBox destinationList;

        /// <summary>
        ///   The file extensions button.
        /// </summary>
        private Button fileExtensionsButton;

        /// <summary>
        ///   The folder dialog.
        /// </summary>
        private FolderBrowserDialog folderDialog;

        /// <summary>
        ///   The format builder button.
        /// </summary>
        private Button formatBuilderButton;

        /// <summary>
        ///   The format text.
        /// </summary>
        private TextBox formatText;

        /// <summary>
        ///   The group directories.
        /// </summary>
        private GroupBox groupDirectories;

        /// <summary>
        ///   The recurse subdirectories check.
        /// </summary>
        private CheckBox recurseSubdirectoriesCheck;

        /// <summary>
        ///   The reg ex button.
        /// </summary>
        private Button regExButton;

        /// <summary>
        ///   The remove destination button.
        /// </summary>
        private Button removeDestinationButton;

        /// <summary>
        ///   The rename if exists check.
        /// </summary>
        private CheckBox renameIfExistsCheck;

        /// <summary>
        ///   The revert button.
        /// </summary>
        private Button revertButton;

        /// <summary>
        ///   The save button.
        /// </summary>
        private Button saveButton;

        /// <summary>
        ///   The source browse.
        /// </summary>
        private Button sourceBrowse;

        /// <summary>
        ///   The source text.
        /// </summary>
        private TextBox sourceText;

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// true if managed resources should be disposed; otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify 
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TableLayoutPanel mainTable;
            System.Windows.Forms.TableLayoutPanel tableDirectories;
            System.Windows.Forms.Label sourceLabel;
            System.Windows.Forms.Label destinationListLabel;
            System.Windows.Forms.FlowLayoutPanel destinationButtonsFlow;
            System.Windows.Forms.Label defaultDestinationLabel;
            System.Windows.Forms.GroupBox sortOptionsGroup;
            System.Windows.Forms.FlowLayoutPanel sortOptionsFlow;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Settings));
            System.Windows.Forms.GroupBox searchOptionsGroup;
            System.Windows.Forms.FlowLayoutPanel searchOptionsFlow;
            System.Windows.Forms.Button editOverwriteKeywordsButton;
            System.Windows.Forms.GroupBox formatOptionsGroup;
            System.Windows.Forms.TableLayoutPanel formatTable;
            System.Windows.Forms.Label formatLabel;
            System.Windows.Forms.FlowLayoutPanel flowBottomButtons;
            this.groupDirectories = new System.Windows.Forms.GroupBox();
            this.sourceText = new System.Windows.Forms.TextBox();
            this.sourceBrowse = new System.Windows.Forms.Button();
            this.destinationList = new System.Windows.Forms.ListBox();
            this.addDestinationButton = new System.Windows.Forms.Button();
            this.removeDestinationButton = new System.Windows.Forms.Button();
            this.defaultDestinationDirectory = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.addIgnore_btn = new System.Windows.Forms.Button();
            this.removeIgnore_btn = new System.Windows.Forms.Button();
            this.ignoreList = new System.Windows.Forms.ListBox();
            this.recurseSubdirectoriesCheck = new System.Windows.Forms.CheckBox();
            this.deleteEmptyCheck = new System.Windows.Forms.CheckBox();
            this.renameIfExistsCheck = new System.Windows.Forms.CheckBox();
            this.addUnmatchedShowsCheck = new System.Windows.Forms.CheckBox();
            this.unlockAndUpdateCheck = new System.Windows.Forms.CheckBox();
            this.lockShowWithNoNewEpisodesCheck = new System.Windows.Forms.CheckBox();
            this.regExButton = new System.Windows.Forms.Button();
            this.fileExtensionsButton = new System.Windows.Forms.Button();
            this.formatText = new System.Windows.Forms.TextBox();
            this.formatBuilderButton = new System.Windows.Forms.Button();
            this.revertButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            mainTable = new System.Windows.Forms.TableLayoutPanel();
            tableDirectories = new System.Windows.Forms.TableLayoutPanel();
            sourceLabel = new System.Windows.Forms.Label();
            destinationListLabel = new System.Windows.Forms.Label();
            destinationButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            defaultDestinationLabel = new System.Windows.Forms.Label();
            sortOptionsGroup = new System.Windows.Forms.GroupBox();
            sortOptionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            searchOptionsGroup = new System.Windows.Forms.GroupBox();
            searchOptionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            editOverwriteKeywordsButton = new System.Windows.Forms.Button();
            formatOptionsGroup = new System.Windows.Forms.GroupBox();
            formatTable = new System.Windows.Forms.TableLayoutPanel();
            formatLabel = new System.Windows.Forms.Label();
            flowBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            mainTable.SuspendLayout();
            this.groupDirectories.SuspendLayout();
            tableDirectories.SuspendLayout();
            destinationButtonsFlow.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            sortOptionsGroup.SuspendLayout();
            sortOptionsFlow.SuspendLayout();
            searchOptionsGroup.SuspendLayout();
            searchOptionsFlow.SuspendLayout();
            formatOptionsGroup.SuspendLayout();
            formatTable.SuspendLayout();
            flowBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            mainTable.ColumnCount = 1;
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTable.Controls.Add(this.groupDirectories, 0, 0);
            mainTable.Controls.Add(sortOptionsGroup, 0, 2);
            mainTable.Controls.Add(searchOptionsGroup, 0, 3);
            mainTable.Controls.Add(formatOptionsGroup, 0, 1);
            mainTable.Controls.Add(flowBottomButtons, 0, 4);
            mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTable.Location = new System.Drawing.Point(0, 0);
            mainTable.Name = "mainTable";
            mainTable.RowCount = 5;
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            mainTable.Size = new System.Drawing.Size(734, 652);
            mainTable.TabIndex = 0;
            // 
            // groupDirectories
            // 
            this.groupDirectories.AutoSize = true;
            this.groupDirectories.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupDirectories.Controls.Add(tableDirectories);
            this.groupDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDirectories.Location = new System.Drawing.Point(3, 3);
            this.groupDirectories.Name = "groupDirectories";
            this.groupDirectories.Size = new System.Drawing.Size(728, 401);
            this.groupDirectories.TabIndex = 0;
            this.groupDirectories.TabStop = false;
            this.groupDirectories.Text = "Directory Settings";
            // 
            // tableDirectories
            // 
            tableDirectories.AutoSize = true;
            tableDirectories.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            tableDirectories.ColumnCount = 3;
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            tableDirectories.Controls.Add(sourceLabel, 0, 0);
            tableDirectories.Controls.Add(this.sourceText, 1, 0);
            tableDirectories.Controls.Add(this.sourceBrowse, 2, 0);
            tableDirectories.Controls.Add(destinationListLabel, 0, 1);
            tableDirectories.Controls.Add(this.destinationList, 1, 1);
            tableDirectories.Controls.Add(destinationButtonsFlow, 2, 1);
            tableDirectories.Controls.Add(this.defaultDestinationDirectory, 1, 3);
            tableDirectories.Controls.Add(this.label1, 0, 2);
            tableDirectories.Controls.Add(this.flowLayoutPanel1, 2, 2);
            tableDirectories.Controls.Add(this.ignoreList, 1, 2);
            tableDirectories.Controls.Add(defaultDestinationLabel, 0, 3);
            tableDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            tableDirectories.Location = new System.Drawing.Point(3, 16);
            tableDirectories.Name = "tableDirectories";
            tableDirectories.RowCount = 4;
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle());
            tableDirectories.Size = new System.Drawing.Size(722, 382);
            tableDirectories.TabIndex = 0;
            // 
            // sourceLabel
            // 
            sourceLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new System.Drawing.Point(34, 6);
            sourceLabel.Name = "sourceLabel";
            sourceLabel.Size = new System.Drawing.Size(89, 13);
            sourceLabel.TabIndex = 0;
            sourceLabel.Text = "Source Directory:";
            // 
            // sourceText
            // 
            this.sourceText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceText.Location = new System.Drawing.Point(129, 3);
            this.sourceText.Name = "sourceText";
            this.sourceText.ReadOnly = true;
            this.sourceText.Size = new System.Drawing.Size(485, 20);
            this.sourceText.TabIndex = 1;
            this.toolTip.SetToolTip(this.sourceText, "The source directory to search for TV Show files.");
            // 
            // sourceBrowse
            // 
            this.sourceBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceBrowse.Location = new System.Drawing.Point(620, 3);
            this.sourceBrowse.Name = "sourceBrowse";
            this.sourceBrowse.Size = new System.Drawing.Size(99, 20);
            this.sourceBrowse.TabIndex = 2;
            this.sourceBrowse.Text = "Browse";
            this.sourceBrowse.UseVisualStyleBackColor = true;
            this.sourceBrowse.Click += new System.EventHandler(this.SourceBrowseClick);
            // 
            // destinationListLabel
            // 
            destinationListLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            destinationListLabel.AutoSize = true;
            destinationListLabel.Location = new System.Drawing.Point(7, 101);
            destinationListLabel.Name = "destinationListLabel";
            destinationListLabel.Size = new System.Drawing.Size(116, 13);
            destinationListLabel.TabIndex = 5;
            destinationListLabel.Text = "Destination Directories:";
            // 
            // destinationList
            // 
            this.destinationList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationList.FormattingEnabled = true;
            this.destinationList.Location = new System.Drawing.Point(129, 29);
            this.destinationList.Name = "destinationList";
            this.destinationList.Size = new System.Drawing.Size(485, 158);
            this.destinationList.TabIndex = 6;
            this.toolTip.SetToolTip(this.destinationList, "All the directories where TV Shows are stored. Only the selected one will have TV" +
        " moved to it but all will be used in Missing and Duplicate episode searches.");
            // 
            // destinationButtonsFlow
            // 
            destinationButtonsFlow.AutoSize = true;
            destinationButtonsFlow.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            destinationButtonsFlow.Controls.Add(this.addDestinationButton);
            destinationButtonsFlow.Controls.Add(this.removeDestinationButton);
            destinationButtonsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            destinationButtonsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            destinationButtonsFlow.Location = new System.Drawing.Point(617, 26);
            destinationButtonsFlow.Margin = new System.Windows.Forms.Padding(0);
            destinationButtonsFlow.Name = "destinationButtonsFlow";
            destinationButtonsFlow.Size = new System.Drawing.Size(105, 164);
            destinationButtonsFlow.TabIndex = 7;
            // 
            // addDestinationButton
            // 
            this.addDestinationButton.Location = new System.Drawing.Point(3, 3);
            this.addDestinationButton.Name = "addDestinationButton";
            this.addDestinationButton.Size = new System.Drawing.Size(99, 23);
            this.addDestinationButton.TabIndex = 0;
            this.addDestinationButton.Text = "Add";
            this.addDestinationButton.UseVisualStyleBackColor = true;
            this.addDestinationButton.Click += new System.EventHandler(this.AddDestinationButtonClick);
            // 
            // removeDestinationButton
            // 
            this.removeDestinationButton.Location = new System.Drawing.Point(3, 32);
            this.removeDestinationButton.Name = "removeDestinationButton";
            this.removeDestinationButton.Size = new System.Drawing.Size(99, 23);
            this.removeDestinationButton.TabIndex = 1;
            this.removeDestinationButton.Text = "Remove";
            this.removeDestinationButton.UseVisualStyleBackColor = true;
            this.removeDestinationButton.Click += new System.EventHandler(this.RemoveDestinationButtonClick);
            // 
            // defaultDestinationDirectory
            // 
            this.defaultDestinationDirectory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.defaultDestinationDirectory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.defaultDestinationDirectory.FormattingEnabled = true;
            this.defaultDestinationDirectory.Location = new System.Drawing.Point(129, 357);
            this.defaultDestinationDirectory.Name = "defaultDestinationDirectory";
            this.defaultDestinationDirectory.Size = new System.Drawing.Size(485, 21);
            this.defaultDestinationDirectory.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 265);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Ignored folders:";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.addIgnore_btn);
            this.flowLayoutPanel1.Controls.Add(this.removeIgnore_btn);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(617, 190);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(105, 164);
            this.flowLayoutPanel1.TabIndex = 12;
            // 
            // addIgnore_btn
            // 
            this.addIgnore_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addIgnore_btn.Location = new System.Drawing.Point(3, 3);
            this.addIgnore_btn.Name = "addIgnore_btn";
            this.addIgnore_btn.Size = new System.Drawing.Size(96, 23);
            this.addIgnore_btn.TabIndex = 0;
            this.addIgnore_btn.Text = "Add";
            this.addIgnore_btn.UseVisualStyleBackColor = true;
            this.addIgnore_btn.Click += new System.EventHandler(this.AddIgnoreButtonClick);
            // 
            // removeIgnore_btn
            // 
            this.removeIgnore_btn.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.removeIgnore_btn.Location = new System.Drawing.Point(3, 32);
            this.removeIgnore_btn.Name = "removeIgnore_btn";
            this.removeIgnore_btn.Size = new System.Drawing.Size(96, 23);
            this.removeIgnore_btn.TabIndex = 1;
            this.removeIgnore_btn.Text = "Remove";
            this.removeIgnore_btn.UseVisualStyleBackColor = true;
            this.removeIgnore_btn.Click += new System.EventHandler(this.RemoveIgnoreButtonClick);
            // 
            // ignoreList
            // 
            this.ignoreList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ignoreList.FormattingEnabled = true;
            this.ignoreList.Location = new System.Drawing.Point(129, 193);
            this.ignoreList.Name = "ignoreList";
            this.ignoreList.Size = new System.Drawing.Size(485, 158);
            this.ignoreList.TabIndex = 11;
            // 
            // defaultDestinationLabel
            // 
            defaultDestinationLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            defaultDestinationLabel.AutoSize = true;
            defaultDestinationLabel.Location = new System.Drawing.Point(23, 361);
            defaultDestinationLabel.Name = "defaultDestinationLabel";
            defaultDestinationLabel.Size = new System.Drawing.Size(100, 13);
            defaultDestinationLabel.TabIndex = 8;
            defaultDestinationLabel.Text = "Default Destination:";
            // 
            // sortOptionsGroup
            // 
            sortOptionsGroup.Controls.Add(sortOptionsFlow);
            sortOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            sortOptionsGroup.Location = new System.Drawing.Point(3, 464);
            sortOptionsGroup.Name = "sortOptionsGroup";
            sortOptionsGroup.Size = new System.Drawing.Size(728, 89);
            sortOptionsGroup.TabIndex = 1;
            sortOptionsGroup.TabStop = false;
            sortOptionsGroup.Text = "Sort Options";
            // 
            // sortOptionsFlow
            // 
            sortOptionsFlow.Controls.Add(this.recurseSubdirectoriesCheck);
            sortOptionsFlow.Controls.Add(this.deleteEmptyCheck);
            sortOptionsFlow.Controls.Add(this.renameIfExistsCheck);
            sortOptionsFlow.Controls.Add(this.addUnmatchedShowsCheck);
            sortOptionsFlow.Controls.Add(this.unlockAndUpdateCheck);
            sortOptionsFlow.Controls.Add(this.lockShowWithNoNewEpisodesCheck);
            sortOptionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            sortOptionsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            sortOptionsFlow.Location = new System.Drawing.Point(3, 16);
            sortOptionsFlow.Name = "sortOptionsFlow";
            sortOptionsFlow.Size = new System.Drawing.Size(722, 70);
            sortOptionsFlow.TabIndex = 0;
            // 
            // recurseSubdirectoriesCheck
            // 
            this.recurseSubdirectoriesCheck.AutoSize = true;
            this.recurseSubdirectoriesCheck.Location = new System.Drawing.Point(3, 3);
            this.recurseSubdirectoriesCheck.Name = "recurseSubdirectoriesCheck";
            this.recurseSubdirectoriesCheck.Size = new System.Drawing.Size(136, 17);
            this.recurseSubdirectoriesCheck.TabIndex = 0;
            this.recurseSubdirectoriesCheck.Text = "Recurse Subdirectories";
            this.toolTip.SetToolTip(this.recurseSubdirectoriesCheck, "When selected, this option will search the subdirectories of the source directory" +
        " as well.");
            this.recurseSubdirectoriesCheck.UseVisualStyleBackColor = true;
            // 
            // deleteEmptyCheck
            // 
            this.deleteEmptyCheck.AutoSize = true;
            this.deleteEmptyCheck.Location = new System.Drawing.Point(3, 26);
            this.deleteEmptyCheck.Name = "deleteEmptyCheck";
            this.deleteEmptyCheck.Size = new System.Drawing.Size(159, 17);
            this.deleteEmptyCheck.TabIndex = 1;
            this.deleteEmptyCheck.Text = "Delete Empty Subdirectories";
            this.toolTip.SetToolTip(this.deleteEmptyCheck, "When selected, this option will delete subdirectories of Source Directory after f" +
        "iles have been moved out of them if this leaves the directory empty.");
            this.deleteEmptyCheck.UseVisualStyleBackColor = true;
            // 
            // renameIfExistsCheck
            // 
            this.renameIfExistsCheck.AutoSize = true;
            this.renameIfExistsCheck.Location = new System.Drawing.Point(3, 49);
            this.renameIfExistsCheck.Name = "renameIfExistsCheck";
            this.renameIfExistsCheck.Size = new System.Drawing.Size(213, 17);
            this.renameIfExistsCheck.TabIndex = 2;
            this.renameIfExistsCheck.Text = "Rename if Episode Exists at Destination";
            this.toolTip.SetToolTip(this.renameIfExistsCheck, "When selected, this option will search the destination directoy for the episode b" +
        "eing processed and renamed the copy there if it exists with a different name.");
            this.renameIfExistsCheck.UseVisualStyleBackColor = true;
            // 
            // addUnmatchedShowsCheck
            // 
            this.addUnmatchedShowsCheck.AutoSize = true;
            this.addUnmatchedShowsCheck.Location = new System.Drawing.Point(222, 3);
            this.addUnmatchedShowsCheck.Name = "addUnmatchedShowsCheck";
            this.addUnmatchedShowsCheck.Size = new System.Drawing.Size(203, 17);
            this.addUnmatchedShowsCheck.TabIndex = 3;
            this.addUnmatchedShowsCheck.Text = "Add Unmatched Shows Automatically";
            this.toolTip.SetToolTip(this.addUnmatchedShowsCheck, resources.GetString("addUnmatchedShowsCheck.ToolTip"));
            this.addUnmatchedShowsCheck.UseVisualStyleBackColor = true;
            // 
            // unlockAndUpdateCheck
            // 
            this.unlockAndUpdateCheck.AutoSize = true;
            this.unlockAndUpdateCheck.Location = new System.Drawing.Point(222, 26);
            this.unlockAndUpdateCheck.Name = "unlockAndUpdateCheck";
            this.unlockAndUpdateCheck.Size = new System.Drawing.Size(202, 17);
            this.unlockAndUpdateCheck.TabIndex = 4;
            this.unlockAndUpdateCheck.Text = "Unlock and Update Locked Matches";
            this.toolTip.SetToolTip(this.unlockAndUpdateCheck, "When selected, this option will unlock any shows that are locked and update them " +
        "if a match is found.");
            this.unlockAndUpdateCheck.UseVisualStyleBackColor = true;
            // 
            // lockShowWithNoNewEpisodesCheck
            // 
            this.lockShowWithNoNewEpisodesCheck.AutoSize = true;
            this.lockShowWithNoNewEpisodesCheck.Location = new System.Drawing.Point(222, 49);
            this.lockShowWithNoNewEpisodesCheck.Name = "lockShowWithNoNewEpisodesCheck";
            this.lockShowWithNoNewEpisodesCheck.Size = new System.Drawing.Size(264, 17);
            this.lockShowWithNoNewEpisodesCheck.TabIndex = 5;
            this.lockShowWithNoNewEpisodesCheck.Text = "Lock Show After 3 Weeks With No New Episodes\r\n";
            this.toolTip.SetToolTip(this.lockShowWithNoNewEpisodesCheck, "When selected, during an update, if the show hasn\'t \r\nhad any new episodes for 3 " +
        "weeks, the show will be\r\nlocked and skipped in future updates.");
            this.lockShowWithNoNewEpisodesCheck.UseVisualStyleBackColor = true;
            // 
            // searchOptionsGroup
            // 
            searchOptionsGroup.Controls.Add(searchOptionsFlow);
            searchOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            searchOptionsGroup.Location = new System.Drawing.Point(3, 559);
            searchOptionsGroup.Name = "searchOptionsGroup";
            searchOptionsGroup.Size = new System.Drawing.Size(728, 50);
            searchOptionsGroup.TabIndex = 2;
            searchOptionsGroup.TabStop = false;
            searchOptionsGroup.Text = "Search Options";
            // 
            // searchOptionsFlow
            // 
            searchOptionsFlow.Controls.Add(this.regExButton);
            searchOptionsFlow.Controls.Add(this.fileExtensionsButton);
            searchOptionsFlow.Controls.Add(editOverwriteKeywordsButton);
            searchOptionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            searchOptionsFlow.Location = new System.Drawing.Point(3, 16);
            searchOptionsFlow.Name = "searchOptionsFlow";
            searchOptionsFlow.Size = new System.Drawing.Size(722, 31);
            searchOptionsFlow.TabIndex = 0;
            // 
            // regExButton
            // 
            this.regExButton.Location = new System.Drawing.Point(3, 3);
            this.regExButton.Name = "regExButton";
            this.regExButton.Size = new System.Drawing.Size(136, 23);
            this.regExButton.TabIndex = 0;
            this.regExButton.Text = "Edit Regular Expressions";
            this.toolTip.SetToolTip(this.regExButton, "Edit the regular expressions used for searching.\r\nSee http://code.google.com/p/tv" +
        "sorter for more\r\ninformation.");
            this.regExButton.UseVisualStyleBackColor = true;
            this.regExButton.Click += new System.EventHandler(this.RegExButtonClick);
            // 
            // fileExtensionsButton
            // 
            this.fileExtensionsButton.Location = new System.Drawing.Point(145, 3);
            this.fileExtensionsButton.Name = "fileExtensionsButton";
            this.fileExtensionsButton.Size = new System.Drawing.Size(136, 23);
            this.fileExtensionsButton.TabIndex = 1;
            this.fileExtensionsButton.Text = "Edit File Extensions";
            this.toolTip.SetToolTip(this.fileExtensionsButton, "Edit the file extensions that are searched.");
            this.fileExtensionsButton.UseVisualStyleBackColor = true;
            this.fileExtensionsButton.Click += new System.EventHandler(this.FileExtensionsButtonClick);
            // 
            // editOverwriteKeywordsButton
            // 
            editOverwriteKeywordsButton.Location = new System.Drawing.Point(287, 3);
            editOverwriteKeywordsButton.Name = "editOverwriteKeywordsButton";
            editOverwriteKeywordsButton.Size = new System.Drawing.Size(146, 23);
            editOverwriteKeywordsButton.TabIndex = 2;
            editOverwriteKeywordsButton.Text = "Edit Overwrite Keywords";
            this.toolTip.SetToolTip(editOverwriteKeywordsButton, resources.GetString("editOverwriteKeywordsButton.ToolTip"));
            editOverwriteKeywordsButton.UseVisualStyleBackColor = true;
            editOverwriteKeywordsButton.Click += new System.EventHandler(this.EditOverwriteKeywordsButtonClick);
            // 
            // formatOptionsGroup
            // 
            formatOptionsGroup.Controls.Add(formatTable);
            formatOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            formatOptionsGroup.Location = new System.Drawing.Point(3, 410);
            formatOptionsGroup.Name = "formatOptionsGroup";
            formatOptionsGroup.Size = new System.Drawing.Size(728, 48);
            formatOptionsGroup.TabIndex = 3;
            formatOptionsGroup.TabStop = false;
            formatOptionsGroup.Text = "Format Options";
            // 
            // formatTable
            // 
            formatTable.ColumnCount = 3;
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            formatTable.Controls.Add(formatLabel, 0, 0);
            formatTable.Controls.Add(this.formatText, 1, 0);
            formatTable.Controls.Add(this.formatBuilderButton, 2, 0);
            formatTable.Dock = System.Windows.Forms.DockStyle.Fill;
            formatTable.Location = new System.Drawing.Point(3, 16);
            formatTable.Name = "formatTable";
            formatTable.RowCount = 1;
            formatTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formatTable.Size = new System.Drawing.Size(722, 29);
            formatTable.TabIndex = 0;
            // 
            // formatLabel
            // 
            formatLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            formatLabel.AutoSize = true;
            formatLabel.Location = new System.Drawing.Point(46, 8);
            formatLabel.Name = "formatLabel";
            formatLabel.Size = new System.Drawing.Size(77, 13);
            formatLabel.TabIndex = 0;
            formatLabel.Text = "Output Format:";
            // 
            // formatText
            // 
            this.formatText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formatText.Location = new System.Drawing.Point(129, 3);
            this.formatText.Name = "formatText";
            this.formatText.Size = new System.Drawing.Size(485, 20);
            this.formatText.TabIndex = 1;
            this.toolTip.SetToolTip(this.formatText, "The formatting string used to set the output path for an episode. This setting ca" +
        "n be overriden on a per show basis.");
            // 
            // formatBuilderButton
            // 
            this.formatBuilderButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.formatBuilderButton.Location = new System.Drawing.Point(620, 3);
            this.formatBuilderButton.Name = "formatBuilderButton";
            this.formatBuilderButton.Size = new System.Drawing.Size(99, 23);
            this.formatBuilderButton.TabIndex = 2;
            this.formatBuilderButton.Text = "Format Builder";
            this.formatBuilderButton.UseVisualStyleBackColor = true;
            this.formatBuilderButton.Click += new System.EventHandler(this.FormatBuilderButtonClick);
            // 
            // flowBottomButtons
            // 
            flowBottomButtons.Controls.Add(this.revertButton);
            flowBottomButtons.Controls.Add(this.saveButton);
            flowBottomButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowBottomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowBottomButtons.Location = new System.Drawing.Point(3, 615);
            flowBottomButtons.Name = "flowBottomButtons";
            flowBottomButtons.Size = new System.Drawing.Size(728, 34);
            flowBottomButtons.TabIndex = 4;
            // 
            // revertButton
            // 
            this.revertButton.Location = new System.Drawing.Point(650, 3);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(75, 23);
            this.revertButton.TabIndex = 0;
            this.revertButton.Text = "Revert";
            this.revertButton.UseVisualStyleBackColor = true;
            this.revertButton.Click += new System.EventHandler(this.RevertButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(569, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(mainTable);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(734, 652);
            this.Load += new System.EventHandler(this.SettingsLoad);
            mainTable.ResumeLayout(false);
            mainTable.PerformLayout();
            this.groupDirectories.ResumeLayout(false);
            this.groupDirectories.PerformLayout();
            tableDirectories.ResumeLayout(false);
            tableDirectories.PerformLayout();
            destinationButtonsFlow.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            sortOptionsGroup.ResumeLayout(false);
            sortOptionsFlow.ResumeLayout(false);
            sortOptionsFlow.PerformLayout();
            searchOptionsGroup.ResumeLayout(false);
            searchOptionsFlow.ResumeLayout(false);
            formatOptionsGroup.ResumeLayout(false);
            formatTable.ResumeLayout(false);
            formatTable.PerformLayout();
            flowBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ToolTip toolTip;
        private CheckBox addUnmatchedShowsCheck;
        private CheckBox unlockAndUpdateCheck;
        private IContainer components = null;
        private CheckBox lockShowWithNoNewEpisodesCheck;
        private ComboBox defaultDestinationDirectory;
        private Label label1;
        private ListBox ignoreList;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button addIgnore_btn;
        private Button removeIgnore_btn;
    }
}