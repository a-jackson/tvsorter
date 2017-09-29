// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShowRepository.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Manages the collection of TV Shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using TVSorter.Data;
using TVSorter.Model;
using TVSorter.Storage;

namespace TVSorter.Repostitory
{
    /// <summary>
    ///     Manages the collection of TV shows.
    /// </summary>
    public class TvShowRepository : ITvShowRepository
    {
        /// <summary>
        ///     The data provider.
        /// </summary>
        private readonly IDataProvider dataProvider;

        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     Initialises a new instance of the <see cref="TvShowRepository" /> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        /// <param name="dataProvider">The data provider.</param>
        public TvShowRepository(IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;

            // Add event handlers to the storage provider.
            this.storageProvider.TvShowAdded += OnTvShowAdded;
            this.storageProvider.TvShowChanged += OnTvShowChanged;
            this.storageProvider.TvShowRemoved += OnTvShowRemoved;
        }

        /// <summary>
        ///     Occurs when a TV Show is added.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        ///     Occurs when a TV Show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        ///     Occurs when a TV Show is removed.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowRemoved;

        /// <summary>
        ///     Returns a new TVShow from a search result.
        /// </summary>
        /// <param name="searchResult">
        ///     The search result.
        /// </param>
        /// <returns>
        ///     The new TVShow.
        /// </returns>
        public TvShow FromSearchResult(TvShow searchResult)
        {
            var show = new TvShow
            {
                Name = searchResult.Name,
                FolderName = searchResult.FolderName,
                TvdbId = searchResult.TvdbId
            };

            show.InitialiseDefaultData();
            return show;
        }

        /// <summary>
        ///     Deletes the specified show.
        /// </summary>
        /// <param name="show">
        ///     The show to delete.
        /// </param>
        public void Delete(TvShow show)
        {
            storageProvider.RemoveShow(show);
        }

        /// <summary>
        ///     Saves the specified show.
        /// </summary>
        /// <param name="show">
        ///     The show to save.
        /// </param>
        public void Save(TvShow show)
        {
            storageProvider.SaveShow(show);
        }

        /// <summary>
        ///     Updates the specified show.
        /// </summary>
        /// <param name="show">
        ///     The show to update.
        /// </param>
        public void Update(TvShow show)
        {
            dataProvider.UpdateShow(show);
            LockIfNoEpisodes(show);
            Save(show);
        }

        /// <summary>
        ///     Gets the full list of all TVShows from the specified provider.
        /// </summary>
        /// <returns>
        ///     The full list of TV Shows.
        /// </returns>
        public IEnumerable<TvShow> GetTvShows()
        {
            return storageProvider.LoadTvShows();
        }

        /// <summary>
        ///     Searches for new shows.
        /// </summary>
        /// <param name="name">
        ///     The name of the show to search.
        /// </param>
        /// <returns>
        ///     The results of the search.
        /// </returns>
        public List<TvShow> SearchShow(string name)
        {
            return dataProvider.SearchShow(name);
        }

        /// <summary>
        ///     Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        ///     The shows to update.
        /// </param>
        public void UpdateShows(IList<TvShow> shows)
        {
            foreach (var show in dataProvider.UpdateShows(shows))
            {
                LockIfNoEpisodes(show);
                Save(show);
            }
        }

        /// <summary>
        ///     Locks the show if it has no episodes.
        /// </summary>
        /// <param name="show">
        ///     The show to lock.
        /// </param>
        private void LockIfNoEpisodes(TvShow show)
        {
            if (storageProvider.Settings.LockShowsWithNoEpisodes && show.LastUpdated != DateTime.MinValue)
            {
                var mostRecentAirDate =
                (from episode in show.Episodes
                    where episode.FirstAir < DateTime.Today
                    orderby episode.FirstAir descending
                    select episode.FirstAir).FirstOrDefault();

                var threeWeeksAgo = DateTime.Today.Subtract(TimeSpan.FromDays(21));

                if (threeWeeksAgo > mostRecentAirDate)
                {
                    show.Locked = true;
                    Logger.OnLogMessage(
                        this,
                        "Locking {0}. No new episodes since {1:dd-MMM-yyyy}",
                        LogType.Info,
                        show.Name,
                        mostRecentAirDate);
                }
            }
        }

        /// <summary>
        ///     Handles the IStorageProvider's OnTvShowAdded event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowAdded(object sender, TvShowEventArgs e)
        {
            if (TvShowAdded != null)
            {
                TvShowAdded(sender, e);
            }
        }

        /// <summary>
        ///     Handles the IStorageProvider's OnTvShowChanged event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowChanged(object sender, TvShowEventArgs e)
        {
            if (TvShowChanged != null)
            {
                TvShowChanged(sender, e);
            }
        }

        /// <summary>
        ///     Handles the IStorageProvider's OnTvShowRemoved event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowRemoved(object sender, TvShowEventArgs e)
        {
            if (TvShowRemoved != null)
            {
                TvShowRemoved(sender, e);
            }
        }
    }
}
