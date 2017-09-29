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


    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;


    /// <summary>
    /// The missing and duplicated episodes tab.
    /// </summary>
    public partial class MissingDuplicateEpisodes
    {




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
            System.Windows.Forms.Panel panelControls;
            System.Windows.Forms.GroupBox filtersGroup;
            System.Windows.Forms.SplitContainer splitContainer;
            System.Windows.Forms.TableLayoutPanel tableLeft;
            System.Windows.Forms.TableLayoutPanel tableRight;
            this.episodesTree = new System.Windows.Forms.TreeView();
            this.episodesCountLabel = new System.Windows.Forms.Label();
            this.refreshButton = new System.Windows.Forms.Button();
            this.hideWholeSeasons = new System.Windows.Forms.CheckBox();
            this.hideLocked = new System.Windows.Forms.CheckBox();
            this.hidePart2 = new System.Windows.Forms.CheckBox();
            this.hideSeason0 = new System.Windows.Forms.CheckBox();
            this.hideUnaired = new System.Windows.Forms.CheckBox();
            this.duplicatesButton = new System.Windows.Forms.Button();
            this.missingButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            panelControls = new System.Windows.Forms.Panel();
            filtersGroup = new System.Windows.Forms.GroupBox();
            splitContainer = new System.Windows.Forms.SplitContainer();
            tableLeft = new System.Windows.Forms.TableLayoutPanel();
            tableRight = new System.Windows.Forms.TableLayoutPanel();
            panelControls.SuspendLayout();
            filtersGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            tableLeft.SuspendLayout();
            tableRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // episodesTree
            // 
            this.episodesTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.episodesTree.Location = new System.Drawing.Point(3, 36);
            this.episodesTree.Name = "episodesTree";
            this.episodesTree.Size = new System.Drawing.Size(232, 497);
            this.episodesTree.TabIndex = 0;
            this.toolTip.SetToolTip(this.episodesTree, "Shows the results of the search");
            // 
            // episodesCountLabel
            // 
            this.episodesCountLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.episodesCountLabel.AutoSize = true;
            this.episodesCountLabel.Location = new System.Drawing.Point(3, 20);
            this.episodesCountLabel.Name = "episodesCountLabel";
            this.episodesCountLabel.Size = new System.Drawing.Size(114, 13);
            this.episodesCountLabel.TabIndex = 1;
            this.episodesCountLabel.Text = "Number of Episodes: 0";
            // 
            // refreshButton
            // 
            this.refreshButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.refreshButton.Location = new System.Drawing.Point(3, 5);
            this.refreshButton.Name = "refreshButton";
            this.refreshButton.Size = new System.Drawing.Size(144, 23);
            this.refreshButton.TabIndex = 1;
            this.refreshButton.Text = "Refresh Episode Counts";
            this.toolTip.SetToolTip(this.refreshButton, "Searches the Output Directories for episodes and updates their file count numbers" +
        ". \r\nThis is used to determine the results of the missing and duplicate episode\r\n" +
        "searches.");
            this.refreshButton.UseVisualStyleBackColor = true;
            this.refreshButton.Click += new System.EventHandler(this.RefreshButtonClick);
            // 
            // panelControls
            // 
            panelControls.Controls.Add(filtersGroup);
            panelControls.Controls.Add(this.duplicatesButton);
            panelControls.Controls.Add(this.missingButton);
            panelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            panelControls.Location = new System.Drawing.Point(0, 33);
            panelControls.Margin = new System.Windows.Forms.Padding(0);
            panelControls.Name = "panelControls";
            panelControls.Size = new System.Drawing.Size(468, 503);
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
            // hideWholeSeasons
            // 
            this.hideWholeSeasons.AutoSize = true;
            this.hideWholeSeasons.Location = new System.Drawing.Point(6, 111);
            this.hideWholeSeasons.Name = "hideWholeSeasons";
            this.hideWholeSeasons.Size = new System.Drawing.Size(156, 17);
            this.hideWholeSeasons.TabIndex = 4;
            this.hideWholeSeasons.Text = "Hide entire missing seasons";
            this.toolTip.SetToolTip(this.hideWholeSeasons, "Hides episodes where and entire season is missing. This can be useful for long ru" +
        "nning where you don\'t have early seasons.");
            this.hideWholeSeasons.UseVisualStyleBackColor = true;
            // 
            // hideLocked
            // 
            this.hideLocked.AutoSize = true;
            this.hideLocked.Location = new System.Drawing.Point(6, 88);
            this.hideLocked.Name = "hideLocked";
            this.hideLocked.Size = new System.Drawing.Size(116, 17);
            this.hideLocked.TabIndex = 3;
            this.hideLocked.Text = "Hide locked shows";
            this.toolTip.SetToolTip(this.hideLocked, "Hides the episodes from shows that are locked.");
            this.hideLocked.UseVisualStyleBackColor = true;
            // 
            // hidePart2
            // 
            this.hidePart2.AutoSize = true;
            this.hidePart2.Location = new System.Drawing.Point(6, 65);
            this.hidePart2.Name = "hidePart2";
            this.hidePart2.Size = new System.Drawing.Size(165, 17);
            this.hidePart2.TabIndex = 2;
            this.hidePart2.Text = "Hide episodes ending with (2)";
            this.toolTip.SetToolTip(this.hidePart2, "Hides episodes with the name ending in (2). \r\nIt is possible these aren\'t missing" +
        " but the filename\r\ndoesn\'t contain both episode numbers and so\r\nTVSorter isn\'t a" +
        "ble to match it.");
            this.hidePart2.UseVisualStyleBackColor = true;
            // 
            // hideSeason0
            // 
            this.hideSeason0.AutoSize = true;
            this.hideSeason0.Location = new System.Drawing.Point(6, 42);
            this.hideSeason0.Name = "hideSeason0";
            this.hideSeason0.Size = new System.Drawing.Size(143, 17);
            this.hideSeason0.TabIndex = 1;
            this.hideSeason0.Text = "Hide season 0 (Specials)";
            this.toolTip.SetToolTip(this.hideSeason0, "Hides episode in season 0. These are generally specials.");
            this.hideSeason0.UseVisualStyleBackColor = true;
            // 
            // hideUnaired
            // 
            this.hideUnaired.AutoSize = true;
            this.hideUnaired.Location = new System.Drawing.Point(6, 19);
            this.hideUnaired.Name = "hideUnaired";
            this.hideUnaired.Size = new System.Drawing.Size(109, 17);
            this.hideUnaired.TabIndex = 0;
            this.hideUnaired.Text = "Hide not yet aired";
            this.toolTip.SetToolTip(this.hideUnaired, "Hides episodes from the results whose air date is in the future");
            this.hideUnaired.UseVisualStyleBackColor = true;
            // 
            // duplicatesButton
            // 
            this.duplicatesButton.Location = new System.Drawing.Point(153, 3);
            this.duplicatesButton.Name = "duplicatesButton";
            this.duplicatesButton.Size = new System.Drawing.Size(144, 23);
            this.duplicatesButton.TabIndex = 3;
            this.duplicatesButton.Text = "Display Duplicate Episodes";
            this.toolTip.SetToolTip(this.duplicatesButton, "Searches for duplicate episodes");
            this.duplicatesButton.UseVisualStyleBackColor = true;
            this.duplicatesButton.Click += new System.EventHandler(this.DuplicatesButtonClick);
            // 
            // missingButton
            // 
            this.missingButton.Location = new System.Drawing.Point(3, 3);
            this.missingButton.Name = "missingButton";
            this.missingButton.Size = new System.Drawing.Size(144, 23);
            this.missingButton.TabIndex = 2;
            this.missingButton.Text = "Display Missing Episodes";
            this.toolTip.SetToolTip(this.missingButton, "Searches for missing episodes.");
            this.missingButton.UseVisualStyleBackColor = true;
            this.missingButton.Click += new System.EventHandler(this.MissingButtonClick);
            // 
            // splitContainer
            // 
            splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer.Location = new System.Drawing.Point(0, 0);
            splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.Controls.Add(tableLeft);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.Controls.Add(tableRight);
            splitContainer.Size = new System.Drawing.Size(710, 536);
            splitContainer.SplitterDistance = 238;
            splitContainer.TabIndex = 5;
            // 
            // tableLeft
            // 
            tableLeft.ColumnCount = 1;
            tableLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLeft.Controls.Add(this.episodesTree, 0, 1);
            tableLeft.Controls.Add(this.episodesCountLabel, 0, 0);
            tableLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLeft.Location = new System.Drawing.Point(0, 0);
            tableLeft.Name = "tableLeft";
            tableLeft.RowCount = 2;
            tableLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            tableLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLeft.Size = new System.Drawing.Size(238, 536);
            tableLeft.TabIndex = 0;
            // 
            // tableRight
            // 
            tableRight.ColumnCount = 1;
            tableRight.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableRight.Controls.Add(panelControls, 0, 1);
            tableRight.Controls.Add(this.refreshButton, 0, 0);
            tableRight.Dock = System.Windows.Forms.DockStyle.Fill;
            tableRight.Location = new System.Drawing.Point(0, 0);
            tableRight.Name = "tableRight";
            tableRight.RowCount = 2;
            tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            tableRight.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableRight.Size = new System.Drawing.Size(468, 536);
            tableRight.TabIndex = 0;
            // 
            // MissingDuplicateEpisodes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(splitContainer);
            this.Name = "MissingDuplicateEpisodes";
            this.Size = new System.Drawing.Size(710, 536);
            this.Load += new System.EventHandler(this.MissingDuplicateEpisodesLoad);
            panelControls.ResumeLayout(false);
            filtersGroup.ResumeLayout(false);
            filtersGroup.PerformLayout();
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(splitContainer)).EndInit();
            splitContainer.ResumeLayout(false);
            tableLeft.ResumeLayout(false);
            tableLeft.PerformLayout();
            tableRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }


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
        private ToolTip toolTip;
        private IContainer components;
    }
}