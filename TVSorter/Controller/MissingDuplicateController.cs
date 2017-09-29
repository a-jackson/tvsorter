// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MissingDuplicateController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for the missing / duplicate episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Storage;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Controller for the missing / duplicate episodes tab.
    /// </summary>
    public class MissingDuplicateController : ControllerBase
    {
        /// <summary>
        ///     The file searcher.
        /// </summary>
        private readonly IFileSearch fileSearch;

        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     The episode displayed on the tree.
        /// </summary>
        private List<Episode> episodes;

        /// <summary>
        ///     Initialises a new instance of the <see cref="MissingDuplicateController" /> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        /// <param name="fileSearch">The file search.</param>
        public MissingDuplicateController(IStorageProvider storageProvider, IFileSearch fileSearch)
        {
            this.storageProvider = storageProvider;
            this.fileSearch = fileSearch;
        }

        /// <summary>
        ///     Gets or sets the View.
        /// </summary>
        private IView View { get; set; }

        /// <summary>
        ///     Gets the episodes that are results for the operation.
        /// </summary>
        public List<Episode> Episodes
        {
            get => episodes;

            private set
            {
                episodes = value;
                OnPropertyChanged("Episodes");
            }
        }

        /// <summary>
        ///     Gets or sets the settings.
        /// </summary>
        public MissingEpisodeSettings Settings { get; set; }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            View = view;
            Settings = storageProvider.MissingEpisodeSettings;
        }

        /// <summary>
        ///     Refreshes the file counts of the episodes.
        /// </summary>
        public void RefreshFileCounts()
        {
            var task = new BackgroundTask(fileSearch.RefreshFileCounts);
            task.Start();

            View.StartTaskProgress(task, "Refreshing file counts");
        }

        /// <summary>
        ///     Searches for duplicate episodes.
        /// </summary>
        public void SearchDuplicateEpisodes()
        {
            Episodes = storageProvider.GetDuplicateEpisodes().ToList();
        }

        /// <summary>
        ///     Searches for missing episodes.
        /// </summary>
        public void SearchMissingEpisodes()
        {
            storageProvider.SaveMissingEpisodeSettings();

            var missingEpisodes = storageProvider.GetMissingEpisodes().ToList();

            if (Settings.HideMissingSeasons)
            {
                var episodesWithEntireSeasonMissing = GetEpisodesWithEntireSeasonMissing(missingEpisodes).ToList();
                missingEpisodes = missingEpisodes.Except(episodesWithEntireSeasonMissing).ToList();
            }

            if (Settings.HideNotYetAired)
            {
                missingEpisodes = missingEpisodes.Where(
                        ep => ep.FirstAir < DateTime.Today && !ep.FirstAir.Equals(new DateTime(1970, 1, 1)))
                    .ToList();
            }

            if (Settings.HideSeason0)
            {
                missingEpisodes = missingEpisodes.Where(ep => ep.SeasonNumber != 0).ToList();
            }

            if (Settings.HidePart2)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Name.EndsWith("(2)")).ToList();
            }

            if (Settings.HideLocked)
            {
                missingEpisodes = missingEpisodes.Where(ep => !ep.Show.Locked).ToList();
            }

            Episodes = missingEpisodes.OrderBy(x => x.Show.Name).ToList();
        }

        /// <summary>
        ///     Returns the episodes from missingEpisodes where the entire season is missing.
        /// </summary>
        /// <param name="missingEpisodes">
        ///     The missing episodes.
        /// </param>
        /// <returns>
        ///     The episodes from seasons where the entire season is missing.
        /// </returns>
        private IEnumerable<Episode> GetEpisodesWithEntireSeasonMissing(IEnumerable<Episode> missingEpisodes)
        {
            return (from show in missingEpisodes.GroupBy(x => x.Show)
                select GetEpisodesWithEntireSeasonMissing(show.Key, show)).SelectMany(episodeList => episodeList);
        }

        /// <summary>
        ///     Returns the episodes from missingEpisodes where the entire season is missing.
        /// </summary>
        /// <param name="show">
        ///     The show the episodes are for.
        /// </param>
        /// <param name="missingEpisodes">
        ///     The missing episodes.
        /// </param>
        /// <returns>
        ///     The collection of episodes with their entire season missing.
        /// </returns>
        private IEnumerable<Episode> GetEpisodesWithEntireSeasonMissing(
            TvShow show,
            IEnumerable<Episode> missingEpisodes)
        {
            return from season in missingEpisodes.GroupBy(x => x.SeasonNumber)
                let seasonSize = show.Episodes.Count(x => x.SeasonNumber == season.Key)
                where season.Count() == seasonSize
                from episode in season
                select episode;
        }
    }
}
