// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MissingDuplicateController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for the missing / duplicate episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Controller
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TVSorter.Scanning;
    using TVSorter.Storage;
    using TVSorter.Types;
    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for the missing / duplicate episodes tab.
    /// </summary>
    public class MissingDuplicateController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   The episode displayed on the tree.
        /// </summary>
        private List<Episode> episodes;

        /// <summary>
        ///   The scan manager.
        /// </summary>
        private IScanManager scanner;

        /// <summary>
        ///   The storage provider.
        /// </summary>
        private IStorageProvider storage;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the episodes that are results for the operation.
        /// </summary>
        public List<Episode> Episodes
        {
            get
            {
                return this.episodes;
            }

            private set
            {
                this.episodes = value;
                this.OnPropertyChanged("Episodes");
            }
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets the View.
        /// </summary>
        private IView View { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.View = view;
            this.scanner = Factory.Scanner;
            this.scanner.RefreshFileCountsComplete += this.OnRefreshFileCountsComplete;
            this.scanner.ProgressChanged += this.OnProgressChanged;
            this.storage = Factory.StorageProvider;
        }

        /// <summary>
        /// Refreshes the file counts of the episodes.
        /// </summary>
        public void RefreshFileCounts()
        {
            this.MaxValue = 1;
            this.Value = 0;
            this.scanner.RefreshFileCountsAsync(this);
            this.View.StartTaskProgress(this, "Refreshing file counts");
        }

        /// <summary>
        /// Searches for duplicate episodes.
        /// </summary>
        public void SearchDuplicateEpisodes()
        {
            this.Episodes = this.storage.GetDuplicateEpisodes().ToList();
        }

        /// <summary>
        /// Searches for missing episodes.
        /// </summary>
        /// <param name="hideUnaired">
        /// A value indicating whether the hide unaired episodes.
        /// </param>
        /// <param name="hideSeason0">
        /// A value indicating whether to hide season 0.
        /// </param>
        /// <param name="hidePart2">
        /// A value indicating whether to hide part 2 episodes.
        /// </param>
        /// <param name="hideLocked">
        /// A value indicating whether to hide locked shows.
        /// </param>
        /// <param name="hideWholeSeasons">
        /// A value indicating whether to hide entire missing seasons. 
        /// </param>
        public void SearchMissingEpisodes(
            bool hideUnaired, bool hideSeason0, bool hidePart2, bool hideLocked, bool hideWholeSeasons)
        {
            List<Episode> missingEpisodes = this.storage.GetMissingEpisodes().ToList();

            if (hideUnaired)
            {
                missingEpisodes =
                    missingEpisodes.Where(
                        ep => ep.FirstAir < DateTime.Today && !ep.FirstAir.Equals(new DateTime(1970, 1, 1))).ToList();
            }

            if (hideSeason0)
            {
                missingEpisodes = missingEpisodes.Where(ep => ep.SeasonNumber != 0).ToList();
            }

            if (hidePart2)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Name.EndsWith("(2)")).ToList();
            }

            if (hideLocked)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Show.Locked).ToList();
            }

            if (hideWholeSeasons)
            {
                Dictionary<TvShow, Dictionary<int, int>> seasonCounts = this.storage.SeasonEpisodeCount();
                var removeEpsiodes = new List<Episode>();
                IEnumerable<IGrouping<TvShow, Episode>> showGroups = missingEpisodes.GroupBy(x => x.Show);
                foreach (var seasonGroup in from showGroup in showGroups
                                            let seasonGroups = showGroup.GroupBy(x => x.SeasonNumber)
                                            from seasonGroup in seasonGroups
                                            where seasonGroup.Count() == seasonCounts[showGroup.Key][seasonGroup.Key]
                                            select seasonGroup)
                {
                    removeEpsiodes.AddRange(seasonGroup);
                }

                missingEpisodes = missingEpisodes.Where(x => !removeEpsiodes.Contains(x)).ToList();
            }

            this.Episodes = missingEpisodes.OrderBy(x => x.Show.Name).ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the progress changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.MaxValue = e.MaxValue;
            this.Value = e.Value;
            this.OnProgressChanged();
        }

        /// <summary>
        /// Handles the refresh file count complete event completing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnRefreshFileCountsComplete(object sender, RefreshFileCountsCompleteEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.OnTaskComplete();
        }

        #endregion
    }
}