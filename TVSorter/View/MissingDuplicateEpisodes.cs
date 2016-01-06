// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MissingDuplicateEpisodes.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The missing and duplicated episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows.Forms;

    using Controller;
    using Files;
    using Storage;

    #endregion

    /// <summary>
    /// The missing and duplicated episodes tab.
    /// </summary>
    public partial class MissingDuplicateEpisodes : UserControl, IView
    {
        #region Fields

        /// <summary>
        ///   The controller.
        /// </summary>
        private MissingDuplicateController controller;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initialises a new instance of the <see cref="MissingDuplicateEpisodes" /> class.
        /// </summary>
        public MissingDuplicateEpisodes()
        {
            this.InitializeComponent();
        }

        #endregion

        /// <summary>
        /// Gets or sets the Storage Provider.
        /// </summary>
        public IStorageProvider StorageProvider { get; set; }

        /// <summary>
        /// Gets or sets the File Searcher.
        /// </summary>
        public IFileSearch FileSearch { get; set; }

        #region Public Methods and Operators

        /// <summary>
        /// Starts the progress indication for the specified Project Task.
        /// </summary>
        /// <param name="task">
        /// The task. 
        /// </param>
        /// <param name="taskName">
        /// The task name. 
        /// </param>
        public void StartTaskProgress(IProgressTask task, string taskName)
        {
            var dialog = new ProgressDialog(task) { Text = taskName };
            dialog.ShowDialog(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the duplicates button click.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void DuplicatesButtonClick(object sender, EventArgs e)
        {
            this.controller.SearchDuplicateEpisodes();
        }

        /// <summary>
        /// Handles the missing button click.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void MissingButtonClick(object sender, EventArgs e)
        {
            this.controller.Settings.HideLocked = this.hideLocked.Checked;
            this.controller.Settings.HidePart2 = this.hidePart2.Checked;
            this.controller.Settings.HideSeason0 = this.hideSeason0.Checked;
            this.controller.Settings.HideNotYetAired = this.hideUnaired.Checked;
            this.controller.Settings.HideMissingSeasons = this.hideWholeSeasons.Checked;

            this.controller.SearchMissingEpisodes();
        }

        /// <summary>
        /// The missing duplicate episodes load.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void MissingDuplicateEpisodesLoad(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                this.controller = new MissingDuplicateController(StorageProvider, FileSearch);
                this.controller.PropertyChanged += this.OnPropertyChanged;
                this.controller.Initialise(this);
                this.hideLocked.Checked = this.controller.Settings.HideLocked;
                this.hidePart2.Checked = this.controller.Settings.HidePart2;
                this.hideSeason0.Checked = this.controller.Settings.HideSeason0;
                this.hideUnaired.Checked = this.controller.Settings.HideNotYetAired;
                this.hideWholeSeasons.Checked = this.controller.Settings.HideMissingSeasons;
            }
        }

        /// <summary>
        /// The on property changed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The e. 
        /// </param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Episodes":
                    this.UpdateTree();
                    this.episodesCountLabel.Text = "Number of Episodes: " + this.controller.Episodes.Count;
                    break;
            }
        }

        /// <summary>
        /// Handles the refresh button click.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void RefreshButtonClick(object sender, EventArgs e)
        {
            this.controller.RefreshFileCounts();
        }

        /// <summary>
        /// Updates the tree view.
        /// </summary>
        private void UpdateTree()
        {
            this.episodesTree.Nodes.Clear();
            foreach (var show in this.controller.Episodes.GroupBy(x => x.Show))
            {
                var showNode = new TreeNode(show.Key.Name);
                this.episodesTree.Nodes.Add(showNode);
                IEnumerable<IGrouping<string, string>> seasons =
                    show.GroupBy(
                        x => string.Format("Season {0}", x.SeasonNumber), 
                        x => string.Format("{0} - {1}", x.EpisodeNumber, x.Name));
                foreach (var season in seasons)
                {
                    var seasonNode = new TreeNode(season.Key);
                    showNode.Nodes.Add(seasonNode);
                    seasonNode.Nodes.AddRange(season.Select(x => new TreeNode(x)).ToArray());
                }
            }
        }

        #endregion
    }
}