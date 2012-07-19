// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShow.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The tv show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using TVSorter.Data;
    using TVSorter.Files;
    using TVSorter.Storage;

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
            CreateNfoFiles(Settings.LoadSettings());
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
            return ScanManager.SearchNewShows(Factory.StorageProvider, Factory.DataProvider);
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
        /// <param name="settings">
        /// The settings to use.
        /// </param>
        internal static void CreateNfoFiles(Settings settings)
        {
            foreach (TvShow show in GetTvShows())
            {
                show.CreateNfoFile(settings);
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
        /// Create an NFO file for the show.
        /// </summary>
        /// <param name="settings">
        /// The settings to use.
        /// </param>
        internal void CreateNfoFile(Settings settings)
        {
            string url = string.Format("http://thetvdb.com/?tab=series&id={0}&lid=7", this.TvdbId);
            IEnumerable<string> files = from destination in settings.DestinationDirectories
                                        where
                                            Directory.Exists(
                                                string.Concat(destination, Path.DirectorySeparatorChar, this.FolderName))
                                        select
                                            string.Format(
                                                "{0}{2}{1}{2}tvshow.nfo", 
                                                destination, 
                                                this.FolderName, 
                                                Path.DirectorySeparatorChar);
            foreach (string file in files.Where(x => !File.Exists(x)))
            {
                File.WriteAllText(file, url);
                Logger.OnLogMessage(this, "Created nfo file {0}", file);
            }
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
            if (this.Name != null)
            {
                yield return this.Name;
                yield return GetFileSafeName(this.Name);
            }

            if (this.FolderName != null)
            {
                yield return GetFileSafeName(this.FolderName);
                yield return this.FolderName;
            }

            if (this.AlternateNames != null)
            {
                foreach (string alternateName in this.AlternateNames)
                {
                    yield return alternateName;
                    yield return GetFileSafeName(alternateName);
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
            this.Save(storageProvider);
        }

        /// <summary>
        /// Strips any characters that can't be in a file name from the specified string.
        /// </summary>
        /// <param name="str">
        /// The string to edit.
        /// </param>
        /// <returns>
        /// The file name safe string.
        /// </returns>
        private static string GetFileSafeName(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "The string cannot be null.");
            }

            return new string(str.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
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