// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MainForm.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The main form of the program.
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
    /// The main form of the program.
    /// </summary>
    public partial class MainForm
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        ///   The log.
        /// </summary>
        private Log log;

        /// <summary>
        ///   The log page.
        /// </summary>
        private TabPage logPage;

        /// <summary>
        ///   The main tabs.
        /// </summary>
        private TabControl mainTabs;

        /// <summary>
        ///   The missing duplicate episodes.
        /// </summary>
        private MissingDuplicateEpisodes missingDuplicateEpisodes;

        /// <summary>
        ///   The missing duplicate page.
        /// </summary>
        private TabPage missingDuplicatePage;

        /// <summary>
        ///   The settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///   The settings page.
        /// </summary>
        private TabPage settingsPage;

        /// <summary>
        ///   The sort episodes.
        /// </summary>
        private SortEpisodes sortEpisodes;

        /// <summary>
        ///   The sort episodes page.
        /// </summary>
        private TabPage sortEpisodesPage;

        /// <summary>
        ///   The tv shows.
        /// </summary>
        private TvShows tvShows;

        /// <summary>
        ///   The tv shows page.
        /// </summary>
        private TabPage tvShowsPage;

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
            this.mainTabs = new System.Windows.Forms.TabControl();
            this.sortEpisodesPage = new System.Windows.Forms.TabPage();
            this.sortEpisodes = new TVSorter.View.SortEpisodes();
            this.tvShowsPage = new System.Windows.Forms.TabPage();
            this.tvShows = new TVSorter.View.TvShows();
            this.missingDuplicatePage = new System.Windows.Forms.TabPage();
            this.missingDuplicateEpisodes = new TVSorter.View.MissingDuplicateEpisodes();
            this.settingsPage = new System.Windows.Forms.TabPage();
            this.settings = new TVSorter.View.Settings();
            this.logPage = new System.Windows.Forms.TabPage();
            this.log = new TVSorter.View.Log();
            this.mainTabs.SuspendLayout();
            this.sortEpisodesPage.SuspendLayout();
            this.tvShowsPage.SuspendLayout();
            this.missingDuplicatePage.SuspendLayout();
            this.settingsPage.SuspendLayout();
            this.logPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTabs
            // 
            this.mainTabs.Controls.Add(this.sortEpisodesPage);
            this.mainTabs.Controls.Add(this.tvShowsPage);
            this.mainTabs.Controls.Add(this.missingDuplicatePage);
            this.mainTabs.Controls.Add(this.settingsPage);
            this.mainTabs.Controls.Add(this.logPage);
            this.mainTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabs.Location = new System.Drawing.Point(0, 0);
            this.mainTabs.Name = "mainTabs";
            this.mainTabs.SelectedIndex = 0;
            this.mainTabs.Size = new System.Drawing.Size(941, 591);
            this.mainTabs.TabIndex = 0;
            // 
            // sortEpisodesPage
            // 
            this.sortEpisodesPage.Controls.Add(this.sortEpisodes);
            this.sortEpisodesPage.Location = new System.Drawing.Point(4, 22);
            this.sortEpisodesPage.Name = "sortEpisodesPage";
            this.sortEpisodesPage.Padding = new System.Windows.Forms.Padding(3);
            this.sortEpisodesPage.Size = new System.Drawing.Size(933, 565);
            this.sortEpisodesPage.TabIndex = 1;
            this.sortEpisodesPage.Text = "Sort Episode Files";
            this.sortEpisodesPage.UseVisualStyleBackColor = true;
            // 
            // sortEpisodes
            // 
            this.sortEpisodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sortEpisodes.Location = new System.Drawing.Point(3, 3);
            this.sortEpisodes.Name = "sortEpisodes";
            this.sortEpisodes.Size = new System.Drawing.Size(927, 559);
            this.sortEpisodes.TabIndex = 0;
            // 
            // tvShowsPage
            // 
            this.tvShowsPage.Controls.Add(this.tvShows);
            this.tvShowsPage.Location = new System.Drawing.Point(4, 22);
            this.tvShowsPage.Name = "tvShowsPage";
            this.tvShowsPage.Size = new System.Drawing.Size(933, 565);
            this.tvShowsPage.TabIndex = 2;
            this.tvShowsPage.Text = "TV Shows";
            this.tvShowsPage.UseVisualStyleBackColor = true;
            // 
            // tvShows
            // 
            this.tvShows.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvShows.Location = new System.Drawing.Point(0, 0);
            this.tvShows.Name = "tvShows";
            this.tvShows.Size = new System.Drawing.Size(933, 565);
            this.tvShows.TabIndex = 0;
            // 
            // missingDuplicatePage
            // 
            this.missingDuplicatePage.Controls.Add(this.missingDuplicateEpisodes);
            this.missingDuplicatePage.Location = new System.Drawing.Point(4, 22);
            this.missingDuplicatePage.Name = "missingDuplicatePage";
            this.missingDuplicatePage.Size = new System.Drawing.Size(933, 565);
            this.missingDuplicatePage.TabIndex = 3;
            this.missingDuplicatePage.Text = "Missing / Duplicate Episodes";
            this.missingDuplicatePage.UseVisualStyleBackColor = true;
            // 
            // missingDuplicateEpisodes
            // 
            this.missingDuplicateEpisodes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.missingDuplicateEpisodes.Location = new System.Drawing.Point(0, 0);
            this.missingDuplicateEpisodes.Name = "missingDuplicateEpisodes";
            this.missingDuplicateEpisodes.Size = new System.Drawing.Size(192, 74);
            this.missingDuplicateEpisodes.TabIndex = 0;
            // 
            // settingsPage
            // 
            this.settingsPage.Controls.Add(this.settings);
            this.settingsPage.Location = new System.Drawing.Point(4, 22);
            this.settingsPage.Name = "settingsPage";
            this.settingsPage.Size = new System.Drawing.Size(933, 565);
            this.settingsPage.TabIndex = 4;
            this.settingsPage.Text = "Settings";
            this.settingsPage.UseVisualStyleBackColor = true;
            // 
            // settings
            // 
            this.settings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.settings.Location = new System.Drawing.Point(0, 0);
            this.settings.Name = "settings";
            this.settings.Size = new System.Drawing.Size(192, 74);
            this.settings.TabIndex = 0;
            // 
            // logPage
            // 
            this.logPage.Controls.Add(this.log);
            this.logPage.Location = new System.Drawing.Point(4, 22);
            this.logPage.Name = "logPage";
            this.logPage.Size = new System.Drawing.Size(933, 565);
            this.logPage.TabIndex = 5;
            this.logPage.Text = "Log";
            this.logPage.UseVisualStyleBackColor = true;
            // 
            // log
            // 
            this.log.Dock = System.Windows.Forms.DockStyle.Fill;
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(933, 565);
            this.log.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 591);
            this.Controls.Add(this.mainTabs);
            this.Name = "MainForm";
            this.Text = "TV Sorter";
            this.Load += new System.EventHandler(this.MainFormLoad);
            this.mainTabs.ResumeLayout(false);
            this.sortEpisodesPage.ResumeLayout(false);
            this.tvShowsPage.ResumeLayout(false);
            this.missingDuplicatePage.ResumeLayout(false);
            this.settingsPage.ResumeLayout(false);
            this.logPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}