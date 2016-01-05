using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TVSorter.Data;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Storage;
using TVSorter.Wrappers;

namespace TVSorter.Repostitory
{
    public class TvShowRepository : ITvShowRepository
    {
        private IStorageProvider storageProvider;
        private IDataProvider dataProvider;

        public TvShowRepository(IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;
            
            // Add event handlers to the storage provider.
            storageProvider.TvShowAdded += OnTvShowAdded;
            storageProvider.TvShowChanged += OnTvShowChanged;
            storageProvider.TvShowRemoved += OnTvShowRemoved;
        }

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
        
        public void Delete(TvShow show)
        {

        }

        public void Save(TvShow show)
        {
            storageProvider.SaveShow(show);
        }

        public void Update(TvShow show)
        {
            dataProvider.UpdateShow(show);
            LockIfNoEpisodes(show);
            Save(show);
        }
        
        /// <summary>
        /// Locks the show if it has no episodes.
        /// </summary>
        private void LockIfNoEpisodes(TvShow show)
        {
            if (storageProvider.Settings.LockShowsWithNoEpisodes && show.LastUpdated != DateTime.MinValue)
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
        /// Gets the full list of all TVShows from the specified provider.
        /// </summary>
        /// <returns>
        /// The full list of TV Shows.
        /// </returns>
        public IEnumerable<TvShow> GetTvShows()
        {
            return storageProvider.LoadTvShows();
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
            return dataProvider.SearchShow(name);
        }

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
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
            if (TvShowAdded != null)
            {
                TvShowAdded(sender, e);
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
            if (TvShowChanged != null)
            {
                TvShowChanged(sender, e);
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
            if (TvShowRemoved != null)
            {
                TvShowRemoved(sender, e);
            }
        }
    }
}
