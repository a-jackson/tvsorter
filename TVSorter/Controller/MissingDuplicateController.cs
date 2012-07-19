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

        /// <summary>
        /// Gets or sets the settings.
        /// </summary>
        public MissingEpisodeSettings Settings { get; set; }

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
            this.Settings = MissingEpisodeSettings.LoadSettings();
        }

        /// <summary>
        /// Refreshes the file counts of the episodes.
        /// </summary>
        public void RefreshFileCounts()
        {
            var task = new BackgroundTask(FileSearch.RefreshFileCounts);
            task.Start();

            this.View.StartTaskProgress(task, "Refreshing file counts");
        }

        /// <summary>
        /// Searches for duplicate episodes.
        /// </summary>
        public void SearchDuplicateEpisodes()
        {
            this.Episodes = Episode.GetDuplicateEpisodes().ToList();
        }

        /// <summary>
        /// Searches for missing episodes.
        /// </summary>
        public void SearchMissingEpisodes()
        {
            this.Settings.Save();

            List<Episode> missingEpisodes = Episode.GetMissingEpisodes().ToList();

            if (this.Settings.HideNotYetAired)
            {
                missingEpisodes =
                    missingEpisodes.Where(
                        ep => ep.FirstAir < DateTime.Today && !ep.FirstAir.Equals(new DateTime(1970, 1, 1))).ToList();
            }

            if (this.Settings.HideSeason0)
            {
                missingEpisodes = missingEpisodes.Where(ep => ep.SeasonNumber != 0).ToList();
            }

            if (this.Settings.HidePart2)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Name.EndsWith("(2)")).ToList();
            }

            if (this.Settings.HideLocked)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Show.Locked).ToList();
            }

            if (this.Settings.HideMissingSeasons)
            {
                List<Episode> episodesWithEntireSeasonMissing =
                    this.GetEpisodesWithEntireSeasonMissing(missingEpisodes).ToList();
                missingEpisodes = missingEpisodes.Except(episodesWithEntireSeasonMissing).ToList();
            }

            this.Episodes = missingEpisodes.OrderBy(x => x.Show.Name).ToList();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the episodes from missingEpisodes where the entire season is missing.
        /// </summary>
        /// <param name="missingEpisodes">
        /// The missing episodes.
        /// </param>
        /// <returns>
        /// The episodes from seasons where the entire season is missing.
        /// </returns>
        private IEnumerable<Episode> GetEpisodesWithEntireSeasonMissing(IEnumerable<Episode> missingEpisodes)
        {
            return
                (from show in missingEpisodes.GroupBy(x => x.Show)
                 select this.GetEpisodesWithEntireSeasonMissing(show.Key, show)).SelectMany(episodeList => episodeList);
        }

        /// <summary>
        /// Returns the episodes from missingEpisodes where the entire season is missing.
        /// </summary>
        /// <param name="show">
        /// The show the episodes are for.
        /// </param>
        /// <param name="missingEpisodes">
        /// The missing episodes.
        /// </param>
        /// <returns>
        /// The collection of episodes with their entire season missing.
        /// </returns>
        private IEnumerable<Episode> GetEpisodesWithEntireSeasonMissing(
            TvShow show, IEnumerable<Episode> missingEpisodes)
        {
            return from season in missingEpisodes.GroupBy(x => x.SeasonNumber)
                   let seasonSize = show.Episodes.Count(x => x.SeasonNumber == season.Key)
                   where season.Count() == seasonSize
                   from episode in season
                   select episode;
        }

        #endregion
    }
}