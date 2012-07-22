// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShow.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The tv show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TVSorter.Data;
    using TVSorter.Files;
    using TVSorter.Storage;
    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// The tv show.
    /// </summary>
    public class TvShow : IEquatable<TvShow>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        internal TvShow()
        {
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Occurs when a TV Show is added.
        /// </summary>
        public static event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        /// Occurs when a TV Show changes.
        /// </summary>
        public static event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        /// Occurs when a TV Show is removed.
        /// </summary>
        public static event EventHandler<TvShowEventArgs> TvShowRemoved;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets AlternateNames.
        /// </summary>
        public List<string> AlternateNames { get; set; }

        /// <summary>
        ///   Gets or sets Banner.
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        ///   Gets or sets CustomFormat.
        /// </summary>
        public string CustomFormat { get; set; }

        /// <summary>
        /// Gets the episodes of the show.
        /// </summary>
        public List<Episode> Episodes { get; internal set; }

        /// <summary>
        ///   Gets or sets FolderName.
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        ///   Gets or sets LastUpdated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets TvdbId.
        /// </summary>
        public string TvdbId { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether UseCustomFormat.
        /// </summary>
        public bool UseCustomFormat { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether UseDvdOrder.
        /// </summary>
        public bool UseDvdOrder { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates NFO files for each of the shows.
        /// </summary>
        public static void CreateNfoFiles()
        {
            Settings settings = Settings.LoadSettings();
            IEnumerable<DirectoryInfoWrap> directories =
                settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x));
            CreateNfoFiles(directories.Cast<IDirectoryInfo>().ToList(), Factory.StorageProvider);
        }

        /// <summary>
        /// Returns a new TVShow from a search result.
        /// </summary>
        /// <param name="searchResult">
        /// The search result.
        /// </param>
        /// <returns>
        /// The new TVShow.
        /// </returns>
        public static TvShow FromSearchResult(TvShow searchResult)
        {
            var show = new TvShow
                {
                   Name = searchResult.Name, FolderName = searchResult.FolderName, TvdbId = searchResult.TvdbId, 
                };
            show.InitialiseDefaultData();
            return show;
        }

        /// <summary>
        /// Gets the full list of all TVShows.
        /// </summary>
        /// <returns>The full list of the TV Shows.</returns>
        public static IEnumerable<TvShow> GetTvShows()
        {
            return GetTvShows(Factory.StorageProvider);
        }

        /// <summary>
        /// Searches for new TVShows.
        /// </summary>
        /// <returns>
        /// The ambiguous results of the search for user selection.
        /// </returns>
        public static Dictionary<string, List<TvShow>> SearchNewShows()
        {
            IStorageProvider storageProvider = Factory.StorageProvider;
            Settings settings = Settings.LoadSettings(storageProvider);
            IEnumerable<DirectoryInfoWrap> directories =
                settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x));

            return ScanManager.SearchNewShows(storageProvider, Factory.DataProvider, directories);
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
        public static List<TvShow> SearchShow(string name)
        {
            return SearchShow(name, Factory.DataProvider);
        }

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
        /// </param>
        public static void UpdateShows(IList<TvShow> shows)
        {
            UpdateShows(shows, Factory.DataProvider, Factory.StorageProvider);
        }

        /// <summary>
        /// Deletes the show.
        /// </summary>
        public void Delete()
        {
            this.Delete(Factory.StorageProvider);
        }

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> .
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> ; otherwise, false. 
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/> . 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is TvShow))
            {
                return false;
            }

            return this.Equals((TvShow)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false. 
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object. 
        /// </param>
        public bool Equals(TvShow other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.TvdbId, this.TvdbId);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/> . 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.TvdbId != null ? this.TvdbId.GetHashCode() : 0;
        }

        /// <summary>
        /// Saves the show.
        /// </summary>
        public void Save()
        {
            this.Save(Factory.StorageProvider);
        }

        /// <summary>
        /// Updates the show's data.
        /// </summary>
        public void Update()
        {
            this.Update(Factory.DataProvider, Factory.StorageProvider);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates NFO files for each of the shows.
        /// </summary>
        /// <param name="directories">
        /// The destination directories.
        /// </param>
        /// <param name="storageProvider">
        /// The storage provider to use. 
        /// </param>
        internal static void CreateNfoFiles(IList<IDirectoryInfo> directories, IStorageProvider storageProvider)
        {
            foreach (TvShow show in GetTvShows(storageProvider))
            {
                show.CreateNfoFile(directories);
            }
        }

        /// <summary>
        /// Gets the full list of all TVShows from the specified provider.
        /// </summary>
        /// <param name="provider">
        /// The provider to use.
        /// </param>
        /// <returns>
        /// The full list of TV Shows.
        /// </returns>
        internal static IEnumerable<TvShow> GetTvShows(IStorageProvider provider)
        {
            // Remove the event handlers incase they have been added 
            // before to this storage provider.
            provider.TvShowAdded -= OnTvShowAdded;
            provider.TvShowChanged -= OnTvShowChanged;
            provider.TvShowRemoved -= OnTvShowRemoved;

            // Add event handlers to the storage provider.
            provider.TvShowAdded += OnTvShowAdded;
            provider.TvShowChanged += OnTvShowChanged;
            provider.TvShowRemoved += OnTvShowRemoved;

            return provider.LoadTvShows();
        }

        /// <summary>
        /// Searches for new shows.
        /// </summary>
        /// <param name="name">
        /// The name of the show to search.
        /// </param>
        /// <param name="dataProvider">
        /// The data provider to use for the search.
        /// </param>
        /// <returns>
        /// The results of the search.
        /// </returns>
        internal static List<TvShow> SearchShow(string name, IDataProvider dataProvider)
        {
            return dataProvider.SearchShow(name);
        }

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
        /// </param>
        /// <param name="dataProvider">
        /// The data provider to use.
        /// </param>
        /// <param name="storageProvider">
        /// The storage provider to use.
        /// </param>
        internal static void UpdateShows(
            IList<TvShow> shows, IDataProvider dataProvider, IStorageProvider storageProvider)
        {
            shows.Update(storageProvider, dataProvider);
        }

        /// <summary>
        /// Deletes the show from the specified provider.
        /// </summary>
        /// <param name="provider">
        /// The provider object to remove the show from.
        /// </param>
        internal void Delete(IStorageProvider provider)
        {
            provider.RemoveShow(this);
        }

        /// <summary>
        /// Gets all the possible names of this show.
        /// </summary>
        /// <returns>The possible names of the show.</returns>
        internal IEnumerable<string> GetShowNames()
        {
            return GetNames(new[] { this.Name, this.FolderName }.Concat(this.AlternateNames)).Distinct();
        }

        /// <summary>
        /// Locks the show if it has no episodes.
        /// </summary>
        /// <param name="storageProvider">
        /// The storage provider to use.
        /// </param>
        internal void LockIfNoEpisodes(IStorageProvider storageProvider)
        {
            if (Settings.LoadSettings(storageProvider).LockShowsWithNoEpisodes)
            {
                DateTime mostRecentAirDate = (from episode in this.Episodes
                                              where episode.FirstAir < DateTime.Today
                                              orderby episode.FirstAir descending
                                              select episode.FirstAir).FirstOrDefault();

                DateTime threeWeeksAgo = DateTime.Today.Subtract(TimeSpan.FromDays(21));

                if (threeWeeksAgo > mostRecentAirDate)
                {
                    this.Locked = true;
                    Logger.OnLogMessage(
                        this, "Locking {0}. No new episodes since {1:dd-MMM-yyyy}", this.Name, mostRecentAirDate);
                }
            }
        }

        /// <summary>
        /// Saves the show.
        /// </summary>
        /// <param name="provider">
        /// The provider to save the show to.
        /// </param>
        internal void Save(IStorageProvider provider)
        {
            provider.SaveShow(this);
        }

        /// <summary>
        /// Updates the show.
        /// </summary>
        /// <param name="dataProvider">
        /// The data provider to use.
        /// </param>
        /// <param name="storageProvider">
        /// The storage provider to use.
        /// </param>
        internal void Update(IDataProvider dataProvider, IStorageProvider storageProvider)
        {
            dataProvider.UpdateShow(this);

            this.LockIfNoEpisodes(storageProvider);

            this.Save(storageProvider);
        }

        /// <summary>
        /// Gets the names from the specified collection of names.
        /// </summary>
        /// <param name="names">
        /// The names to convert.
        /// </param>
        /// <returns>
        /// The complete list of names including string processing.
        /// </returns>
        private static IEnumerable<string> GetNames(IEnumerable<string> names)
        {
            foreach (string name in names.Where(name => name != null))
            {
                yield return name;
                yield return name.GetFileSafeName();
                yield return name.RemoveSpacerChars();
                yield return name.RemoveSpecialChars();
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
        private static void OnTvShowAdded(object sender, TvShowEventArgs e)
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
        private static void OnTvShowChanged(object sender, TvShowEventArgs e)
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
        private static void OnTvShowRemoved(object sender, TvShowEventArgs e)
        {
            if (TvShowRemoved != null)
            {
                TvShowRemoved(sender, e);
            }
        }

        /// <summary>
        /// Create an NFO file for the show.
        /// </summary>
        /// <param name="directories">
        /// The destination directories.
        /// </param>
        private void CreateNfoFile(IEnumerable<IDirectoryInfo> directories)
        {
            string url = string.Format("http://thetvdb.com/?tab=series&id={0}&lid=7", this.TvdbId);

            IEnumerable<IFileInfo> files = from destination in directories
                                           from folder in destination.GetDirectories()
                                           where folder.Name.Equals(this.FolderName)
                                           select folder.CreateFile("tvshow.nfo");

            foreach (IFileInfo file in files.Where(x => x.Exists))
            {
                file.WriteAllText(url);
                Logger.OnLogMessage(this, "Created nfo file {0}", file);
            }
        }

        /// <summary>
        /// Initialises the show with default values.
        /// </summary>
        private void InitialiseDefaultData()
        {
            this.AlternateNames = new List<string>();
            this.Banner = string.Empty;
            this.CustomFormat = string.Empty;
            this.LastUpdated = DateTime.MinValue;
            this.Locked = false;
            this.UseCustomFormat = false;
            this.UseDvdOrder = false;
        }

        #endregion
    }
}