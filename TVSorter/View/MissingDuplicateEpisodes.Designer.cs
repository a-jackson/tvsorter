// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MissingDuplicateEpisodes.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The missing and duplicated episodes tab.
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
    /// The missing and duplicated episodes tab.
    /// </summary>
    public partial class MissingDuplicateEpisodes
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

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
            System.Windows.Forms.TableLayoutPanel tableMain;
            System.Windows.Forms.Panel panelControls;
            System.Windows.Forms.GroupBox filtersGroup;
            this.episodesTree = new System.Windows.Forms.TreeView();
            this.refreshButton = new System.Windows.Forms.Button();
            this.missingButton = new System.Windows.Forms.Button();
            this.duplicatesButton = new System.Windows.Forms.Button();
            this.episodesCountLabel = new System.Windows.Forms.Label();
            this.hideUnaired = new System.Windows.Forms.CheckBox();
            this.hideSeason0 = new System.Windows.Forms.CheckBox();
            this.hidePart2 = new System.Windows.Forms.CheckBox();
            this.hideLocked = new System.Windows.Forms.CheckBox();
            this.hideWholeSeasons = new System.Windows.Forms.CheckBox();
            tableMain = new System.Windows.Forms.TableLayoutPanel();
            panelControls = new System.Windows.Forms.Panel();
            filtersGroup = new System.Windows.Forms.GroupBox();
            tableMain.SuspendLayout();
            panelControls.SuspendLayout();
            filtersGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // episodesTree
            // 
            this.episodesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.episodesTree.Location = new System.Drawing.Point(3, 32);
            this.episodesTree.Name = "episodesTree";
            this.episodesTree.Size = new System.Drawing.Size(212, 501);
            this.episodesTree.TabIndex = 0;
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.refreshButton.Location = new System.Drawing.Point(221, 3);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(144, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh Episode Counts";
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButtonClick);
            // 
            // missingButton
            // 
            this.missingButton.Location = new System.Drawing.Point(3, 3);
            this.missingButton.Name = "missingButton";
            this.missingButton.Size = new System.Drawing.Size(144, 23);
            this.missingButton.TabIndex = 2;
            this.missingButton.Text = "Display Missing Episodes";
            this.missingButton.UseVisualStyleBackColor = true;
            this.missingButton.Click += new System.EventHandler(this.MissingButtonClick);
            // 
            // duplicatesButton
            // 
            this.duplicatesButton.Location = new System.Drawing.Point(153, 3);
            this.duplicatesButton.Name = "duplicatesButton";
            this.duplicatesButton.Size = new System.Drawing.Size(144, 23);
            this.duplicatesButton.TabIndex = 3;
            this.duplicatesButton.Text = "Display Duplicate Episodes";
            this.duplicatesButton.UseVisualStyleBackColor = true;
            this.duplicatesButton.Click += new System.EventHandler(this.DuplicatesButtonClick);
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 218F));
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.Controls.Add(this.episodesTree, 0, 1);
            tableMain.Controls.Add(this.episodesCountLabel, 0, 0);
            tableMain.Controls.Add(this.refreshButton, 1, 0);
            tableMain.Controls.Add(panelControls, 1, 1);
            tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableMain.Location = new System.Drawing.Point(0, 0);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 2;
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 29F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.Size = new System.Drawing.Size(710, 536);
            tableMain.TabIndex = 4;
            // 
            // episodesCountLabel
            // 
            this.episodesCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.episodesCountLabel.AutoSize = true;
            this.episodesCountLabel.Location = new System.Drawing.Point(3, 16);
            this.episodesCountLabel.Name = "episodesCountLabel";
            this.episodesCountLabel.Size = new System.Drawing.Size(114, 13);
            this.episodesCountLabel.TabIndex = 1;
            this.episodesCountLabel.Text = "Number of Episodes: 0";
            // 
            // panelControls
            // 
            panelControls.Controls.Add(filtersGroup);
            panelControls.Controls.Add(this.duplicatesButton);
            panelControls.Controls.Add(this.missingButton);
            panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControls.Location = new System.Drawing.Point(218, 29);
            panelControls.Margin = new System.Windows.Forms.Padding(0);
            panelControls.Name = "panelControls";
            panelControls.Size = new System.Drawing.Size(492, 507);
            panelControls.TabIndex = 2;
            // 
            // filtersGroup
            // 
            filtersGroup.Controls.Add(this.hideWholeSeasons);
            filtersGroup.Controls.Add(this.hideLocked);
            filtersGroup.Controls.Add(this.hidePart2);
            filtersGroup.Controls.Add(this.hideSeason0);
            filtersGroup.Controls.Add(this.hideUnaired);
            filtersGroup.Location = new System.Drawing.Point(3, 32);
            filtersGroup.Name = "filtersGroup";
            filtersGroup.Size = new System.Drawing.Size(294, 136);
            filtersGroup.TabIndex = 4;
            filtersGroup.TabStop = false;
            filtersGroup.Text = "Missing Episode Filters";
            // 
            // hideUnaired
            // 
            this.hideUnaired.AutoSize = true;
            this.hideUnaired.Location = new System.Drawing.Point(6, 19);
            this.hideUnaired.Name = "hideUnaired";
            this.hideUnaired.Size = new System.Drawing.Size(109, 17);
            this.hideUnaired.TabIndex = 0;
            this.hideUnaired.Text = "Hide not yet aired";
            this.hideUnaired.UseVisualStyleBackColor = true;
            // 
            // hideSeason0
            // 
            this.hideSeason0.AutoSize = true;
            this.hideSeason0.Location = new System.Drawing.Point(6, 42);
            this.hideSeason0.Name = "hideSeason0";
            this.hideSeason0.Size = new System.Drawing.Size(143, 17);
            this.hideSeason0.TabIndex = 1;
            this.hideSeason0.Text = "Hide season 0 (Specials)";
            this.hideSeason0.UseVisualStyleBackColor = true;
            // 
            // hidePart2
            // 
            this.hidePart2.AutoSize = true;
            this.hidePart2.Location = new System.Drawing.Point(6, 65);
            this.hidePart2.Name = "hidePart2";
            this.hidePart2.Size = new System.Drawing.Size(165, 17);
            this.hidePart2.TabIndex = 2;
            this.hidePart2.Text = "Hide episodes ending with (2)";
            this.hidePart2.UseVisualStyleBackColor = true;
            // 
            // hideLocked
            // 
            this.hideLocked.AutoSize = true;
            this.hideLocked.Location = new System.Drawing.Point(6, 88);
            this.hideLocked.Name = "hideLocked";
            this.hideLocked.Size = new System.Drawing.Size(116, 17);
            this.hideLocked.TabIndex = 3;
            this.hideLocked.Text = "Hide locked shows";
            this.hideLocked.UseVisualStyleBackColor = true;
            // 
            // hideWholeSeasons
            // 
            this.hideWholeSeasons.AutoSize = true;
            this.hideWholeSeasons.Location = new System.Drawing.Point(6, 111);
            this.hideWholeSeasons.Name = "hideWholeSeasons";
            this.hideWholeSeasons.Size = new System.Drawing.Size(156, 17);
            this.hideWholeSeasons.TabIndex = 4;
            this.hideWholeSeasons.Text = "Hide entire missing seasons";
            this.hideWholeSeasons.UseVisualStyleBackColor = true;
            // 
            // MissingDuplicateEpisodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(tableMain);
            this.Name = "MissingDuplicateEpisodes";
            this.Size = new System.Drawing.Size(710, 536);
            this.Load += new System.EventHandler(this.MissingDuplicateEpisodesLoad);
            tableMain.ResumeLayout(false);
            tableMain.PerformLayout();
            panelControls.ResumeLayout(false);
            filtersGroup.ResumeLayout(false);
            filtersGroup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TreeView episodesTree;
        private Button refreshButton;
        private Button missingButton;
        private Button duplicatesButton;
        private Label episodesCountLabel;
        private CheckBox hideLocked;
        private CheckBox hidePart2;
        private CheckBox hideSeason0;
        private CheckBox hideUnaired;
        private CheckBox hideWholeSeasons;
    }
}