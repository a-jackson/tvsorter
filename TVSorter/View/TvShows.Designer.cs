// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShows.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The TV Shows tab.
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
    /// The TV Shows tab.
    /// </summary>
    public partial class TvShows
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components;

        /// <summary>
        ///   The add show button.
        /// </summary>
        private Button addShowButton;

        /// <summary>
        ///   The alternate names button.
        /// </summary>
        private Button alternateNamesButton;

        /// <summary>
        ///   The create nfo files button.
        /// </summary>
        private Button createNfoFilesButton;

        /// <summary>
        ///   The format builder.
        /// </summary>
        private Button formatBuilder;

        /// <summary>
        ///   The remove show button.
        /// </summary>
        private Button removeShowButton;

        /// <summary>
        ///   The revert button.
        /// </summary>
        private Button revertButton;

        /// <summary>
        ///   The save button.
        /// </summary>
        private Button saveButton;

        /// <summary>
        ///   The search shows button.
        /// </summary>
        private Button searchShowsButton;

        /// <summary>
        ///   The selected show banner.
        /// </summary>
        private PictureBox selectedShowBanner;

        /// <summary>
        ///   The selected show custom format text.
        /// </summary>
        private TextBox selectedShowCustomFormatText;

        /// <summary>
        ///   The selected show folder name text.
        /// </summary>
        private TextBox selectedShowFolderNameText;

        /// <summary>
        ///   The selected show last updated.
        /// </summary>
        private Label selectedShowLastUpdated;

        /// <summary>
        ///   The selected show lock button.
        /// </summary>
        private Button selectedShowLockButton;

        /// <summary>
        ///   The selected show name.
        /// </summary>
        private Label selectedShowName;

        /// <summary>
        ///   The selected show tvdb.
        /// </summary>
        private Label selectedShowTvdb;

        /// <summary>
        ///   The selected show use custom format.
        /// </summary>
        private CheckBox selectedShowUseCustomFormat;

        /// <summary>
        ///   The selected show use dvd order.
        /// </summary>
        private CheckBox selectedShowUseDvdOrder;

        /// <summary>
        ///   The tv shows list.
        /// </summary>
        private ListBox tvShowsList;

        /// <summary>
        ///   The update all button.
        /// </summary>
        private Button updateAllButton;

        /// <summary>
        ///   The update show button.
        /// </summary>
        private Button updateShowButton;

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
            System.Windows.Forms.TableLayoutPanel selectedShowTable;
            System.Windows.Forms.GroupBox customFormatGroup;
            System.Windows.Forms.FlowLayoutPanel selectedShowButtons;
            System.Windows.Forms.Label folderNameLabel;
            System.Windows.Forms.GroupBox episodesGroup;
            this.selectedShowName = new System.Windows.Forms.Label();
            this.selectedShowBanner = new System.Windows.Forms.PictureBox();
            this.selectedShowLastUpdated = new System.Windows.Forms.Label();
            this.selectedShowTvdb = new System.Windows.Forms.Label();
            this.customFormatTable = new System.Windows.Forms.TableLayoutPanel();
            this.selectedShowUseCustomFormat = new System.Windows.Forms.CheckBox();
            this.selectedShowCustomFormatText = new System.Windows.Forms.TextBox();
            this.formatLabel = new System.Windows.Forms.Label();
            this.formatBuilder = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.revertButton = new System.Windows.Forms.Button();
            this.updateShowButton = new System.Windows.Forms.Button();
            this.removeShowButton = new System.Windows.Forms.Button();
            this.namesGroup = new System.Windows.Forms.GroupBox();
            this.nameTable = new System.Windows.Forms.TableLayoutPanel();
            this.selectedShowFolderNameText = new System.Windows.Forms.TextBox();
            this.alternateNamesButton = new System.Windows.Forms.Button();
            this.episodesFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.selectedShowUseDvdOrder = new System.Windows.Forms.CheckBox();
            this.selectedShowLockButton = new System.Windows.Forms.Button();
            this.updateAllButton = new System.Windows.Forms.Button();
            this.addShowButton = new System.Windows.Forms.Button();
            this.searchShowsButton = new System.Windows.Forms.Button();
            this.createNfoFilesButton = new System.Windows.Forms.Button();
            this.tvShowsList = new System.Windows.Forms.ListBox();
            this.topButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            this.resetLastUpdatedButton = new System.Windows.Forms.Button();
            selectedShowTable = new System.Windows.Forms.TableLayoutPanel();
            customFormatGroup = new System.Windows.Forms.GroupBox();
            selectedShowButtons = new System.Windows.Forms.FlowLayoutPanel();
            folderNameLabel = new System.Windows.Forms.Label();
            episodesGroup = new System.Windows.Forms.GroupBox();
            selectedShowTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedShowBanner)).BeginInit();
            customFormatGroup.SuspendLayout();
            this.customFormatTable.SuspendLayout();
            selectedShowButtons.SuspendLayout();
            this.namesGroup.SuspendLayout();
            this.nameTable.SuspendLayout();
            episodesGroup.SuspendLayout();
            this.episodesFlow.SuspendLayout();
            this.topButtonsFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectedShowTable
            // 
            selectedShowTable.ColumnCount = 1;
            selectedShowTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            selectedShowTable.Controls.Add(this.selectedShowName, 0, 0);
            selectedShowTable.Controls.Add(this.selectedShowBanner, 0, 1);
            selectedShowTable.Controls.Add(this.selectedShowLastUpdated, 0, 3);
            selectedShowTable.Controls.Add(this.selectedShowTvdb, 0, 2);
            selectedShowTable.Controls.Add(customFormatGroup, 0, 4);
            selectedShowTable.Controls.Add(selectedShowButtons, 0, 7);
            selectedShowTable.Controls.Add(this.namesGroup, 0, 5);
            selectedShowTable.Controls.Add(episodesGroup, 0, 6);
            selectedShowTable.Dock = System.Windows.Forms.DockStyle.Fill;
            selectedShowTable.Location = new System.Drawing.Point(178, 35);
            selectedShowTable.Name = "selectedShowTable";
            selectedShowTable.RowCount = 8;
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 85F));
            selectedShowTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            selectedShowTable.Size = new System.Drawing.Size(571, 521);
            selectedShowTable.TabIndex = 2;
            // 
            // selectedShowName
            // 
            this.selectedShowName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.selectedShowName.AutoSize = true;
            this.selectedShowName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectedShowName.Location = new System.Drawing.Point(3, 5);
            this.selectedShowName.Name = "selectedShowName";
            this.selectedShowName.Size = new System.Drawing.Size(129, 20);
            this.selectedShowName.TabIndex = 0;
            this.selectedShowName.Text = "Selected Show";
            // 
            // selectedShowBanner
            // 
            this.selectedShowBanner.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedShowBanner.Location = new System.Drawing.Point(3, 33);
            this.selectedShowBanner.Name = "selectedShowBanner";
            this.selectedShowBanner.Size = new System.Drawing.Size(565, 144);
            this.selectedShowBanner.TabIndex = 1;
            this.selectedShowBanner.TabStop = false;
            // 
            // selectedShowLastUpdated
            // 
            this.selectedShowLastUpdated.AutoSize = true;
            this.selectedShowLastUpdated.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedShowLastUpdated.Location = new System.Drawing.Point(3, 205);
            this.selectedShowLastUpdated.Name = "selectedShowLastUpdated";
            this.selectedShowLastUpdated.Size = new System.Drawing.Size(565, 25);
            this.selectedShowLastUpdated.TabIndex = 2;
            this.selectedShowLastUpdated.Text = "Last Updated:";
            // 
            // selectedShowTvdb
            // 
            this.selectedShowTvdb.AutoSize = true;
            this.selectedShowTvdb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedShowTvdb.Location = new System.Drawing.Point(3, 180);
            this.selectedShowTvdb.Name = "selectedShowTvdb";
            this.selectedShowTvdb.Size = new System.Drawing.Size(565, 25);
            this.selectedShowTvdb.TabIndex = 3;
            this.selectedShowTvdb.Text = "TVDB: ";
            // 
            // customFormatGroup
            // 
            customFormatGroup.Controls.Add(this.customFormatTable);
            customFormatGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            customFormatGroup.Location = new System.Drawing.Point(3, 233);
            customFormatGroup.Name = "customFormatGroup";
            customFormatGroup.Size = new System.Drawing.Size(565, 79);
            customFormatGroup.TabIndex = 4;
            customFormatGroup.TabStop = false;
            customFormatGroup.Text = "Custom Format";
            // 
            // customFormatTable
            // 
            this.customFormatTable.ColumnCount = 3;
            this.customFormatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.customFormatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.customFormatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.customFormatTable.Controls.Add(this.selectedShowUseCustomFormat, 0, 0);
            this.customFormatTable.Controls.Add(this.selectedShowCustomFormatText, 1, 1);
            this.customFormatTable.Controls.Add(this.formatLabel, 0, 1);
            this.customFormatTable.Controls.Add(this.formatBuilder, 2, 1);
            this.customFormatTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customFormatTable.Location = new System.Drawing.Point(3, 16);
            this.customFormatTable.Margin = new System.Windows.Forms.Padding(0);
            this.customFormatTable.Name = "customFormatTable";
            this.customFormatTable.RowCount = 2;
            this.customFormatTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.customFormatTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.customFormatTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.customFormatTable.Size = new System.Drawing.Size(559, 60);
            this.customFormatTable.TabIndex = 3;
            // 
            // selectedShowUseCustomFormat
            // 
            this.selectedShowUseCustomFormat.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.selectedShowUseCustomFormat.AutoSize = true;
            this.customFormatTable.SetColumnSpan(this.selectedShowUseCustomFormat, 3);
            this.selectedShowUseCustomFormat.Location = new System.Drawing.Point(3, 6);
            this.selectedShowUseCustomFormat.Name = "selectedShowUseCustomFormat";
            this.selectedShowUseCustomFormat.Size = new System.Drawing.Size(118, 17);
            this.selectedShowUseCustomFormat.TabIndex = 0;
            this.selectedShowUseCustomFormat.Text = "Use Custom Format";
            this.selectedShowUseCustomFormat.UseVisualStyleBackColor = true;
            this.selectedShowUseCustomFormat.CheckedChanged += new System.EventHandler(this.SelectedUseCustomFormatCheckedChanged);
            // 
            // selectedShowCustomFormatText
            // 
            this.selectedShowCustomFormatText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedShowCustomFormatText.Enabled = false;
            this.selectedShowCustomFormatText.Location = new System.Drawing.Point(82, 33);
            this.selectedShowCustomFormatText.Name = "selectedShowCustomFormatText";
            this.selectedShowCustomFormatText.Size = new System.Drawing.Size(372, 20);
            this.selectedShowCustomFormatText.TabIndex = 2;
            // 
            // formatLabel
            // 
            this.formatLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.formatLabel.AutoSize = true;
            this.formatLabel.Location = new System.Drawing.Point(34, 38);
            this.formatLabel.Name = "formatLabel";
            this.formatLabel.Size = new System.Drawing.Size(42, 13);
            this.formatLabel.TabIndex = 1;
            this.formatLabel.Text = "Format:";
            // 
            // formatBuilder
            // 
            this.formatBuilder.Enabled = false;
            this.formatBuilder.Location = new System.Drawing.Point(460, 33);
            this.formatBuilder.Name = "formatBuilder";
            this.formatBuilder.Size = new System.Drawing.Size(95, 23);
            this.formatBuilder.TabIndex = 3;
            this.formatBuilder.Text = "Format Builder";
            this.formatBuilder.UseVisualStyleBackColor = true;
            this.formatBuilder.Click += new System.EventHandler(this.FormatBuilderClick);
            // 
            // selectedShowButtons
            // 
            selectedShowButtons.Controls.Add(this.saveButton);
            selectedShowButtons.Controls.Add(this.revertButton);
            selectedShowButtons.Controls.Add(this.updateShowButton);
            selectedShowButtons.Controls.Add(this.removeShowButton);
            selectedShowButtons.Controls.Add(this.resetLastUpdatedButton);
            selectedShowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            selectedShowButtons.Location = new System.Drawing.Point(0, 485);
            selectedShowButtons.Margin = new System.Windows.Forms.Padding(0);
            selectedShowButtons.Name = "selectedShowButtons";
            selectedShowButtons.Size = new System.Drawing.Size(571, 121);
            selectedShowButtons.TabIndex = 5;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(3, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // revertButton
            // 
            this.revertButton.Location = new System.Drawing.Point(84, 3);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(75, 23);
            this.revertButton.TabIndex = 1;
            this.revertButton.Text = "Revert";
            this.revertButton.UseVisualStyleBackColor = true;
            this.revertButton.Click += new System.EventHandler(this.RevertButtonClick);
            // 
            // updateShowButton
            // 
            this.updateShowButton.Location = new System.Drawing.Point(165, 3);
            this.updateShowButton.Name = "updateShowButton";
            this.updateShowButton.Size = new System.Drawing.Size(93, 23);
            this.updateShowButton.TabIndex = 2;
            this.updateShowButton.Text = "Update Show";
            this.updateShowButton.UseVisualStyleBackColor = true;
            this.updateShowButton.Click += new System.EventHandler(this.UpdateShowButtonClick);
            // 
            // removeShowButton
            // 
            this.removeShowButton.Location = new System.Drawing.Point(264, 3);
            this.removeShowButton.Name = "removeShowButton";
            this.removeShowButton.Size = new System.Drawing.Size(75, 23);
            this.removeShowButton.TabIndex = 3;
            this.removeShowButton.Text = "Remove";
            this.removeShowButton.UseVisualStyleBackColor = true;
            this.removeShowButton.Click += new System.EventHandler(this.RemoveShowButtonClick);
            // 
            // namesGroup
            // 
            this.namesGroup.Controls.Add(this.nameTable);
            this.namesGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.namesGroup.Location = new System.Drawing.Point(3, 318);
            this.namesGroup.Name = "namesGroup";
            this.namesGroup.Size = new System.Drawing.Size(565, 79);
            this.namesGroup.TabIndex = 6;
            this.namesGroup.TabStop = false;
            this.namesGroup.Text = "Names";
            // 
            // nameTable
            // 
            this.nameTable.ColumnCount = 3;
            this.nameTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.nameTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.nameTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.nameTable.Controls.Add(folderNameLabel, 0, 0);
            this.nameTable.Controls.Add(this.selectedShowFolderNameText, 1, 0);
            this.nameTable.Controls.Add(this.alternateNamesButton, 1, 1);
            this.nameTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameTable.Location = new System.Drawing.Point(3, 16);
            this.nameTable.Margin = new System.Windows.Forms.Padding(0);
            this.nameTable.Name = "nameTable";
            this.nameTable.RowCount = 2;
            this.nameTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.nameTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.nameTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.nameTable.Size = new System.Drawing.Size(559, 60);
            this.nameTable.TabIndex = 4;
            // 
            // folderNameLabel
            // 
            folderNameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            folderNameLabel.AutoSize = true;
            folderNameLabel.Location = new System.Drawing.Point(7, 8);
            folderNameLabel.Name = "folderNameLabel";
            folderNameLabel.Size = new System.Drawing.Size(70, 13);
            folderNameLabel.TabIndex = 1;
            folderNameLabel.Text = "Folder Name:";
            // 
            // selectedShowFolderNameText
            // 
            this.selectedShowFolderNameText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectedShowFolderNameText.Location = new System.Drawing.Point(83, 3);
            this.selectedShowFolderNameText.Name = "selectedShowFolderNameText";
            this.selectedShowFolderNameText.Size = new System.Drawing.Size(371, 20);
            this.selectedShowFolderNameText.TabIndex = 2;
            // 
            // alternateNamesButton
            // 
            this.alternateNamesButton.Location = new System.Drawing.Point(83, 33);
            this.alternateNamesButton.Name = "alternateNamesButton";
            this.alternateNamesButton.Size = new System.Drawing.Size(100, 23);
            this.alternateNamesButton.TabIndex = 3;
            this.alternateNamesButton.Text = "Alternate Names";
            this.alternateNamesButton.UseVisualStyleBackColor = true;
            this.alternateNamesButton.Click += new System.EventHandler(this.AlternateNamesButtonClick);
            // 
            // episodesGroup
            // 
            episodesGroup.Controls.Add(this.episodesFlow);
            episodesGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            episodesGroup.Location = new System.Drawing.Point(3, 403);
            episodesGroup.Name = "episodesGroup";
            episodesGroup.Size = new System.Drawing.Size(565, 79);
            episodesGroup.TabIndex = 7;
            episodesGroup.TabStop = false;
            episodesGroup.Text = "Episodes";
            // 
            // episodesFlow
            // 
            this.episodesFlow.Controls.Add(this.selectedShowUseDvdOrder);
            this.episodesFlow.Controls.Add(this.selectedShowLockButton);
            this.episodesFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.episodesFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.episodesFlow.Location = new System.Drawing.Point(3, 16);
            this.episodesFlow.Name = "episodesFlow";
            this.episodesFlow.Size = new System.Drawing.Size(559, 60);
            this.episodesFlow.TabIndex = 1;
            // 
            // selectedShowUseDvdOrder
            // 
            this.selectedShowUseDvdOrder.AutoSize = true;
            this.selectedShowUseDvdOrder.Location = new System.Drawing.Point(3, 3);
            this.selectedShowUseDvdOrder.Name = "selectedShowUseDvdOrder";
            this.selectedShowUseDvdOrder.Size = new System.Drawing.Size(100, 17);
            this.selectedShowUseDvdOrder.TabIndex = 0;
            this.selectedShowUseDvdOrder.Text = "Use DVD Order";
            this.selectedShowUseDvdOrder.UseVisualStyleBackColor = true;
            // 
            // selectedShowLockButton
            // 
            this.selectedShowLockButton.Location = new System.Drawing.Point(3, 26);
            this.selectedShowLockButton.Name = "selectedShowLockButton";
            this.selectedShowLockButton.Size = new System.Drawing.Size(90, 23);
            this.selectedShowLockButton.TabIndex = 1;
            this.selectedShowLockButton.Text = "Unlock Show";
            this.selectedShowLockButton.UseVisualStyleBackColor = true;
            this.selectedShowLockButton.Click += new System.EventHandler(this.SelectedShowLockButtonClick);
            // 
            // updateAllButton
            // 
            this.updateAllButton.Location = new System.Drawing.Point(3, 3);
            this.updateAllButton.Name = "updateAllButton";
            this.updateAllButton.Size = new System.Drawing.Size(75, 23);
            this.updateAllButton.TabIndex = 0;
            this.updateAllButton.Text = "Update All";
            this.updateAllButton.UseVisualStyleBackColor = true;
            this.updateAllButton.Click += new System.EventHandler(this.UpdateAllButtonClick);
            // 
            // addShowButton
            // 
            this.addShowButton.Location = new System.Drawing.Point(84, 3);
            this.addShowButton.Name = "addShowButton";
            this.addShowButton.Size = new System.Drawing.Size(75, 23);
            this.addShowButton.TabIndex = 1;
            this.addShowButton.Text = "Add Show";
            this.addShowButton.UseVisualStyleBackColor = true;
            this.addShowButton.Click += new System.EventHandler(this.AddShowButtonClick);
            // 
            // searchShowsButton
            // 
            this.searchShowsButton.Location = new System.Drawing.Point(165, 3);
            this.searchShowsButton.Name = "searchShowsButton";
            this.searchShowsButton.Size = new System.Drawing.Size(112, 23);
            this.searchShowsButton.TabIndex = 2;
            this.searchShowsButton.Text = "Search for Shows";
            this.searchShowsButton.UseVisualStyleBackColor = true;
            this.searchShowsButton.Click += new System.EventHandler(this.SearchShowsButtonClick);
            // 
            // createNfoFilesButton
            // 
            this.createNfoFilesButton.Location = new System.Drawing.Point(283, 3);
            this.createNfoFilesButton.Name = "createNfoFilesButton";
            this.createNfoFilesButton.Size = new System.Drawing.Size(105, 23);
            this.createNfoFilesButton.TabIndex = 3;
            this.createNfoFilesButton.Text = "Create Nfo Files";
            this.createNfoFilesButton.UseVisualStyleBackColor = true;
            this.createNfoFilesButton.Click += new System.EventHandler(this.CreateNfoFilesButtonClick);
            // 
            // tvShowsList
            // 
            this.tvShowsList.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvShowsList.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.tvShowsList.FormattingEnabled = true;
            this.tvShowsList.ItemHeight = 20;
            this.tvShowsList.Location = new System.Drawing.Point(0, 35);
            this.tvShowsList.Name = "tvShowsList";
            this.tvShowsList.Size = new System.Drawing.Size(178, 521);
            this.tvShowsList.TabIndex = 1;
            this.tvShowsList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.TvShowsListDrawItem);
            this.tvShowsList.SelectedIndexChanged += new System.EventHandler(this.TvShowsListSelectedIndexChanged);
            // 
            // topButtonsFlow
            // 
            this.topButtonsFlow.Controls.Add(this.updateAllButton);
            this.topButtonsFlow.Controls.Add(this.addShowButton);
            this.topButtonsFlow.Controls.Add(this.searchShowsButton);
            this.topButtonsFlow.Controls.Add(this.createNfoFilesButton);
            this.topButtonsFlow.Dock = System.Windows.Forms.DockStyle.Top;
            this.topButtonsFlow.Location = new System.Drawing.Point(0, 0);
            this.topButtonsFlow.Name = "topButtonsFlow";
            this.topButtonsFlow.Size = new System.Drawing.Size(749, 35);
            this.topButtonsFlow.TabIndex = 0;
            // 
            // resetLastUpdatedButton
            // 
            this.resetLastUpdatedButton.Location = new System.Drawing.Point(345, 3);
            this.resetLastUpdatedButton.Name = "resetLastUpdatedButton";
            this.resetLastUpdatedButton.Size = new System.Drawing.Size(115, 23);
            this.resetLastUpdatedButton.TabIndex = 4;
            this.resetLastUpdatedButton.Text = "Reset Last Updated";
            this.resetLastUpdatedButton.UseVisualStyleBackColor = true;
            this.resetLastUpdatedButton.Click += new System.EventHandler(this.ResetLastUpdatedButtonClick);
            // 
            // TvShows
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(selectedShowTable);
            this.Controls.Add(this.tvShowsList);
            this.Controls.Add(this.topButtonsFlow);
            this.Name = "TvShows";
            this.Size = new System.Drawing.Size(749, 556);
            this.Load += new System.EventHandler(this.TvShowsLoad);
            selectedShowTable.ResumeLayout(false);
            selectedShowTable.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.selectedShowBanner)).EndInit();
            customFormatGroup.ResumeLayout(false);
            this.customFormatTable.ResumeLayout(false);
            this.customFormatTable.PerformLayout();
            selectedShowButtons.ResumeLayout(false);
            this.namesGroup.ResumeLayout(false);
            this.nameTable.ResumeLayout(false);
            this.nameTable.PerformLayout();
            episodesGroup.ResumeLayout(false);
            this.episodesFlow.ResumeLayout(false);
            this.episodesFlow.PerformLayout();
            this.topButtonsFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel customFormatTable;
        private Label formatLabel;
        private FlowLayoutPanel topButtonsFlow;
        private GroupBox namesGroup;
        private TableLayoutPanel nameTable;
        private FlowLayoutPanel episodesFlow;
        private Button resetLastUpdatedButton;
    }
}