// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MissingDuplicateEpisodes.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The missing and duplicated episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using TVSorter.Controller;

namespace TVSorter.View
{
    /// <summary>
    ///     The missing and duplicated episodes tab.
    /// </summary>
    public partial class MissingDuplicateEpisodes : UserControl, IView
    {
        /// <summary>
        ///     The controller.
        /// </summary>
        private MissingDuplicateController controller;

        /// <summary>
        ///     Initialises a new instance of the <see cref="MissingDuplicateEpisodes" /> class.
        /// </summary>
        public MissingDuplicateEpisodes()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Starts the progress indication for the specified Project Task.
        /// </summary>
        /// <param name="task">
        ///     The task.
        /// </param>
        /// <param name="taskName">
        ///     The task name.
        /// </param>
        public void StartTaskProgress(IProgressTask task, string taskName)
        {
            var dialog = new ProgressDialog(task) { Text = taskName };
            dialog.ShowDialog(this);
        }

        /// <summary>
        ///     Handles the duplicates button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void DuplicatesButtonClick(object sender, EventArgs e)
        {
            controller.SearchDuplicateEpisodes();
        }

        /// <summary>
        ///     Handles the missing button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void MissingButtonClick(object sender, EventArgs e)
        {
            controller.Settings.HideLocked = hideLocked.Checked;
            controller.Settings.HidePart2 = hidePart2.Checked;
            controller.Settings.HideSeason0 = hideSeason0.Checked;
            controller.Settings.HideNotYetAired = hideUnaired.Checked;
            controller.Settings.HideMissingSeasons = hideWholeSeasons.Checked;

            controller.SearchMissingEpisodes();
        }

        /// <summary>
        ///     The missing duplicate episodes load.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void MissingDuplicateEpisodesLoad(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                controller = CompositionRoot.Get<MissingDuplicateController>();
                controller.PropertyChanged += OnPropertyChanged;
                controller.Initialise(this);
                hideLocked.Checked = controller.Settings.HideLocked;
                hidePart2.Checked = controller.Settings.HidePart2;
                hideSeason0.Checked = controller.Settings.HideSeason0;
                hideUnaired.Checked = controller.Settings.HideNotYetAired;
                hideWholeSeasons.Checked = controller.Settings.HideMissingSeasons;
            }
        }

        /// <summary>
        ///     The on property changed.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "Episodes":
                    UpdateTree();
                    episodesCountLabel.Text = "Number of Episodes: " + controller.Episodes.Count;
                    break;
            }
        }

        /// <summary>
        ///     Handles the refresh button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void RefreshButtonClick(object sender, EventArgs e)
        {
            controller.RefreshFileCounts();
        }

        /// <summary>
        ///     Updates the tree view.
        /// </summary>
        private void UpdateTree()
        {
            episodesTree.Nodes.Clear();
            foreach (var show in controller.Episodes.GroupBy(x => x.Show))
            {
                var showNode = new TreeNode(show.Key.Name);
                episodesTree.Nodes.Add(showNode);
                var seasons = show.GroupBy(
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
    }
}
