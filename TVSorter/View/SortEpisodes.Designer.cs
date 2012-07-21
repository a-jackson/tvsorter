// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SortEpisodes.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The sort episodes tab.
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
    /// The sort episodes tab.
    /// </summary>
    public partial class SortEpisodes
    {
        #region Constants and Fields


        /// <summary>
        ///   The copy button.
        /// </summary>
        private Button copyButton;

        /// <summary>
        ///   The deselect all button.
        /// </summary>
        private Button deselectAllButton;

        /// <summary>
        ///   The destination file.
        /// </summary>
        private ColumnHeader destinationFile;

        /// <summary>
        ///   The episode column.
        /// </summary>
        private ColumnHeader episodeColumn;

        /// <summary>
        ///   The episode name column.
        /// </summary>
        private ColumnHeader episodeNameColumn;

        /// <summary>
        ///   The move button.
        /// </summary>
        private Button moveButton;

        /// <summary>
        ///   The results list.
        /// </summary>
        private ListView resultsList;

        /// <summary>
        ///   The scan button.
        /// </summary>
        private Button scanButton;

        /// <summary>
        ///   The season column.
        /// </summary>
        private ColumnHeader seasonColumn;

        /// <summary>
        ///   The select all button.
        /// </summary>
        private Button selectAllButton;

        /// <summary>
        ///   The set episode button.
        /// </summary>
        private Button setEpisodeButton;

        /// <summary>
        ///   The set show button.
        /// </summary>
        private Button setShowButton;

        /// <summary>
        ///   The source file column.
        /// </summary>
        private ColumnHeader sourceFileColumn;

        /// <summary>
        ///   The sub directory filter.
        /// </summary>
        private ComboBox subDirectoryFilter;

        /// <summary>
        ///   The tv show column.
        /// </summary>
        private ColumnHeader tvShowColumn;

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
            System.Windows.Forms.FlowLayoutPanel topButtonsFlow;
            System.Windows.Forms.FlowLayoutPanel selectButtonsFlow;
            this.subDirectoryFilter = new System.Windows.Forms.ComboBox();
            this.scanButton = new System.Windows.Forms.Button();
            this.copyButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.resultsList = new System.Windows.Forms.ListView();
            this.sourceFileColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tvShowColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.seasonColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.episodeColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.episodeNameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.destinationFile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.selectAllButton = new System.Windows.Forms.Button();
            this.deselectAllButton = new System.Windows.Forms.Button();
            this.setShowButton = new System.Windows.Forms.Button();
            this.setEpisodeButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            topButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            selectButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            topButtonsFlow.SuspendLayout();
            selectButtonsFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // topButtonsFlow
            // 
            topButtonsFlow.Controls.Add(this.subDirectoryFilter);
            topButtonsFlow.Controls.Add(this.scanButton);
            topButtonsFlow.Controls.Add(this.copyButton);
            topButtonsFlow.Controls.Add(this.moveButton);
            topButtonsFlow.Dock = System.Windows.Forms.DockStyle.Top;
            topButtonsFlow.Location = new System.Drawing.Point(0, 0);
            topButtonsFlow.Name = "topButtonsFlow";
            topButtonsFlow.Size = new System.Drawing.Size(678, 31);
            topButtonsFlow.TabIndex = 1;
            // 
            // subDirectoryFilter
            // 
            this.subDirectoryFilter.FormattingEnabled = true;
            this.subDirectoryFilter.Location = new System.Drawing.Point(3, 3);
            this.subDirectoryFilter.Name = "subDirectoryFilter";
            this.subDirectoryFilter.Size = new System.Drawing.Size(121, 21);
            this.subDirectoryFilter.TabIndex = 0;
            this.toolTip.SetToolTip(this.subDirectoryFilter, "The subdirectory of Source Directory to search from.");
            // 
            // scanButton
            // 
            this.scanButton.Location = new System.Drawing.Point(130, 3);
            this.scanButton.Name = "scanButton";
            this.scanButton.Size = new System.Drawing.Size(93, 23);
            this.scanButton.TabIndex = 1;
            this.scanButton.Text = "Scan for Files";
            this.toolTip.SetToolTip(this.scanButton, "Scans for files in the selected subdirectory of Source Directory.");
            this.scanButton.UseVisualStyleBackColor = true;
            this.scanButton.Click += new System.EventHandler(this.ScanButtonClick);
            // 
            // copyButton
            // 
            this.copyButton.Location = new System.Drawing.Point(229, 3);
            this.copyButton.Name = "copyButton";
            this.copyButton.Size = new System.Drawing.Size(95, 23);
            this.copyButton.TabIndex = 2;
            this.copyButton.Text = "Copy to Dest.";
            this.toolTip.SetToolTip(this.copyButton, "Copies the selected episodes to the Destination Directory");
            this.copyButton.UseVisualStyleBackColor = true;
            this.copyButton.Click += new System.EventHandler(this.CopyButtonClick);
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(330, 3);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(102, 23);
            this.moveButton.TabIndex = 3;
            this.moveButton.Text = "Move to Dest.";
            this.toolTip.SetToolTip(this.moveButton, "Moves the selected episodes to the destination directory.");
            this.moveButton.UseVisualStyleBackColor = true;
            this.moveButton.Click += new System.EventHandler(this.MoveButtonClick);
            // 
            // resultsList
            // 
            this.resultsList.CheckBoxes = true;
            this.resultsList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.sourceFileColumn,
            this.tvShowColumn,
            this.seasonColumn,
            this.episodeColumn,
            this.episodeNameColumn,
            this.destinationFile});
            this.resultsList.Cursor = System.Windows.Forms.Cursors.Default;
            this.resultsList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.resultsList.Location = new System.Drawing.Point(0, 66);
            this.resultsList.Name = "resultsList";
            this.resultsList.Size = new System.Drawing.Size(678, 348);
            this.resultsList.TabIndex = 2;
            this.resultsList.UseCompatibleStateImageBehavior = false;
            this.resultsList.View = System.Windows.Forms.View.Details;
            this.resultsList.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.ResultsListItemChecked);
            // 
            // sourceFileColumn
            // 
            this.sourceFileColumn.Text = "Source File";
            this.sourceFileColumn.Width = 200;
            // 
            // tvShowColumn
            // 
            this.tvShowColumn.Text = "TV Show";
            this.tvShowColumn.Width = 92;
            // 
            // seasonColumn
            // 
            this.seasonColumn.Text = "Season";
            this.seasonColumn.Width = 56;
            // 
            // episodeColumn
            // 
            this.episodeColumn.Text = "Episode";
            this.episodeColumn.Width = 51;
            // 
            // episodeNameColumn
            // 
            this.episodeNameColumn.Text = "Episode Name";
            this.episodeNameColumn.Width = 99;
            // 
            // destinationFile
            // 
            this.destinationFile.Text = "Destination";
            this.destinationFile.Width = 172;
            // 
            // selectButtonsFlow
            // 
            selectButtonsFlow.Controls.Add(this.selectAllButton);
            selectButtonsFlow.Controls.Add(this.deselectAllButton);
            selectButtonsFlow.Controls.Add(this.setShowButton);
            selectButtonsFlow.Controls.Add(this.setEpisodeButton);
            selectButtonsFlow.Dock = System.Windows.Forms.DockStyle.Top;
            selectButtonsFlow.Location = new System.Drawing.Point(0, 31);
            selectButtonsFlow.Name = "selectButtonsFlow";
            selectButtonsFlow.Size = new System.Drawing.Size(678, 35);
            selectButtonsFlow.TabIndex = 3;
            // 
            // selectAllButton
            // 
            this.selectAllButton.Location = new System.Drawing.Point(3, 3);
            this.selectAllButton.Name = "selectAllButton";
            this.selectAllButton.Size = new System.Drawing.Size(75, 23);
            this.selectAllButton.TabIndex = 0;
            this.selectAllButton.Text = "Select All";
            this.toolTip.SetToolTip(this.selectAllButton, "Selects all the epsodes.");
            this.selectAllButton.UseVisualStyleBackColor = true;
            this.selectAllButton.Click += new System.EventHandler(this.SelectAllButtonClick);
            // 
            // deselectAllButton
            // 
            this.deselectAllButton.Location = new System.Drawing.Point(84, 3);
            this.deselectAllButton.Name = "deselectAllButton";
            this.deselectAllButton.Size = new System.Drawing.Size(75, 23);
            this.deselectAllButton.TabIndex = 1;
            this.deselectAllButton.Text = "Deselect All";
            this.toolTip.SetToolTip(this.deselectAllButton, "Deselects all the episodes.");
            this.deselectAllButton.UseVisualStyleBackColor = true;
            this.deselectAllButton.Click += new System.EventHandler(this.DeselectAllButtonClick);
            // 
            // setShowButton
            // 
            this.setShowButton.Location = new System.Drawing.Point(165, 3);
            this.setShowButton.Name = "setShowButton";
            this.setShowButton.Size = new System.Drawing.Size(79, 23);
            this.setShowButton.TabIndex = 2;
            this.setShowButton.Text = "Set Show";
            this.toolTip.SetToolTip(this.setShowButton, "Overrides the show of the selected episodes.");
            this.setShowButton.UseVisualStyleBackColor = true;
            this.setShowButton.Click += new System.EventHandler(this.SetShowButtonClick);
            // 
            // setEpisodeButton
            // 
            this.setEpisodeButton.Location = new System.Drawing.Point(250, 3);
            this.setEpisodeButton.Name = "setEpisodeButton";
            this.setEpisodeButton.Size = new System.Drawing.Size(75, 23);
            this.setEpisodeButton.TabIndex = 4;
            this.setEpisodeButton.Text = "Set Episode";
            this.toolTip.SetToolTip(this.setEpisodeButton, ".");
            this.setEpisodeButton.UseVisualStyleBackColor = true;
            this.setEpisodeButton.Click += new System.EventHandler(this.SetEpisodeButtonClick);
            // 
            // SortEpisodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.resultsList);
            this.Controls.Add(selectButtonsFlow);
            this.Controls.Add(topButtonsFlow);
            this.Name = "SortEpisodes";
            this.Size = new System.Drawing.Size(678, 414);
            this.Load += new System.EventHandler(this.SortEpisodesLoad);
            topButtonsFlow.ResumeLayout(false);
            selectButtonsFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private IContainer components;
        private ToolTip toolTip;
    }
}