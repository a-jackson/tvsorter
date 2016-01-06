// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShowRepository.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Manages the collection of TV Shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Repostitory
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Model;
    using Storage;

    #endregion

    /// <summary>
    /// Manages the collection of TV shows.
    /// </summary>
    public class TvShowRepository : ITvShowRepository
    {
        #region Fields

        /// <summary>
        /// The storage provider.
        /// </summary>
        private IStorageProvider storageProvider;

        /// <summary>
        /// The data provider.
        /// </summary>
        private IDataProvider dataProvider;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TvShowRepository"/> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        /// <param name="dataProvider">The data provider.</param>
        public TvShowRepository(IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;

            // Add event handlers to the storage provider.
            this.storageProvider.TvShowAdded += this.OnTvShowAdded;
            this.storageProvider.TvShowChanged += this.OnTvShowChanged;
            this.storageProvider.TvShowRemoved += this.OnTvShowRemoved;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a TV Show is added.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        /// Occurs when a TV Show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        /// Occurs when a TV Show is removed.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowRemoved;

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a new TVShow from a search result.
        /// </summary>
        /// <param name="searchResult">
        /// The search result.
        /// </param>
        /// <returns>
        /// The new TVShow.
        /// </returns>
        public TvShow FromSearchResult(TvShow searchResult)
        {
            var show = new TvShow
            {
                Name = searchResult.Name,
                FolderName = searchResult.FolderName,
                TvdbId = searchResult.TvdbId,
            };

            show.InitialiseDefaultData();
            return show;
        }
        
        /// <summary>
        /// Deletes the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to delete.
        /// </param>
        public void Delete(TvShow show)
        {
            this.storageProvider.RemoveShow(show);
        }

        /// <summary>
        /// Saves the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to save.
        /// </param>
        public void Save(TvShow show)
        {
            this.storageProvider.SaveShow(show);
        }

        /// <summary>
        /// Updates the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to update.
        /// </param>
        public void Update(TvShow show)
        {
            this.dataProvider.UpdateShow(show);
            this.LockIfNoEpisodes(show);
            this.Save(show);
        }
        
        /// <summary>
        /// Gets the full list of all TVShows from the specified provider.
        /// </summary>
        /// <returns>
        /// The full list of TV Shows.
        /// </returns>
        public IEnumerable<TvShow> GetTvShows()
        {
            return this.storageProvider.LoadTvShows();
        }

        /// <summary>
        /// Searches for new shows.
        /// </summary>
        /// <param name="name">
        /// The name of the show to search.
        /// </param>
        /// <returns>
        /// The results of the search.
        /// </returns>
        public List<TvShow> SearchShow(string name)
        {
            return this.dataProvider.SearchShow(name);
        }

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
        /// </param>
        public void UpdateShows(IList<TvShow> shows)
        {
            foreach (var show in this.dataProvider.UpdateShows(shows))
            {
                this.LockIfNoEpisodes(show);
                this.Save(show);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Locks the show if it has no episodes.
        /// </summary>
        /// <param name="show">
        /// The show to lock.
        /// </param>
        private void LockIfNoEpisodes(TvShow show)
        {
            if (this.storageProvider.Settings.LockShowsWithNoEpisodes && show.LastUpdated != DateTime.MinValue)
            {
                DateTime mostRecentAirDate = (from episode in show.Episodes
                                              where episode.FirstAir < DateTime.Today
                                              orderby episode.FirstAir descending
                                              select episode.FirstAir).FirstOrDefault();

                DateTime threeWeeksAgo = DateTime.Today.Subtract(TimeSpan.FromDays(21));

                if (threeWeeksAgo > mostRecentAirDate)
                {
                    show.Locked = true;
                    Logger.OnLogMessage(
                        this, "Locking {0}. No new episodes since {1:dd-MMM-yyyy}", LogType.Info, show.Name, mostRecentAirDate);
                }
            }
        }

        /// <summary>
        /// Handles the IStorageProvider's OnTvShowAdded event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowAdded(object sender, TvShowEventArgs e)
        {
            if (this.TvShowAdded != null)
            {
                this.TvShowAdded(sender, e);
            }
        }

        /// <summary>
        /// Handles the IStorageProvider's OnTvShowChanged event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowChanged(object sender, TvShowEventArgs e)
        {
            if (this.TvShowChanged != null)
            {
                this.TvShowChanged(sender, e);
            }
        }

        /// <summary>
        /// Handles the IStorageProvider's OnTvShowRemoved event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowRemoved(object sender, TvShowEventArgs e)
        {
            if (this.TvShowRemoved != null)
            {
                this.TvShowRemoved(sender, e);
            }
        }

        #endregion
    }
}
