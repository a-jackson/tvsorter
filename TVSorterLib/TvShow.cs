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

    using TVSorter.Data.Tvdb;
    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// The tv show.
    /// </summary>
    public class TvShow : IEquatable<TvShow>
    {
        #region Fields

        /// <summary>
        /// A value indicating whether the data can be from cache or not.
        /// </summary>
        private readonly bool allowCached;

        /// <summary>
        /// The name of the show.
        /// </summary>
        private readonly string name;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the show.
        /// </param>
        /// <param name="allowCached">
        /// A value indcating whether cached data is ok.
        /// </param>
        public TvShow(string name, bool allowCached)
        {
            this.name = name;
            this.allowCached = allowCached;
            this.Initialise();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class from a search result.
        /// </summary>
        /// <param name="searchResult">
        /// The search result.
        /// </param>
        public TvShow(TvShow searchResult)
        {
            this.Name = searchResult.Name;
            this.FolderName = GetFileSafeName(searchResult.Name);
            this.TvdbId = searchResult.TvdbId;
            this.InitialiseDefaultData();
        }

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
        public IEnumerable<Episode> Episodes { get; internal set; }

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
            var settings = new Settings();
            foreach (TvShow show in GetTvShows())
            {
                string url = string.Format("http://thetvdb.com/?tab=series&id={0}&lid=7", show.TvdbId);
                TvShow show1 = show;
                IEnumerable<string> files = from destination in settings.DestinationDirectories
                                            where
                                                Directory.Exists(
                                                    string.Concat(
                                                        destination, Path.DirectorySeparatorChar, show1.FolderName))
                                            select
                                                string.Format(
                                                    "{0}{2}{1}{2}tvshow.nfo", 
                                                    destination, 
                                                    show1.FolderName, 
                                                    Path.DirectorySeparatorChar);
                foreach (string file in files.Where(x => !File.Exists(x)))
                {
                    File.WriteAllText(file, url);
                }
            }
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
        public static string GetFileSafeName(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "The string cannot be null.");
            }

            return new string(str.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
        }

        /// <summary>
        /// Gets the full list of all TVShows from the XML.
        /// </summary>
        /// <returns>The XML from the TV Shows.</returns>
        public static IEnumerable<TvShow> GetTvShows()
        {
            var xml = new Xml();
            return xml.LoadTvShows();
        }

        /// <summary>
        /// Searches for new TVShows.
        /// </summary>
        /// <returns>The ambiguous results of the search for user selection.</returns>
        public static Dictionary<string, List<TvShow>> SearchNewShows()
        {
            var settings = new Settings();
            var showDirs = new List<string>();
            List<string> existingShows = GetTvShows().Select(x => x.FolderName).ToList();
            foreach (DirectoryInfo dirInfo in settings.GetDestinationDirectories())
            {
                // Add all of dirInfo's subdirectories where they don't already exist and 
                // there isn't already a tv show with it as a folder name.
                showDirs.AddRange(
                    from dir in dirInfo.GetDirectories()
                    where !showDirs.Contains(dir.Name) && !existingShows.Contains(dir.Name)
                    select dir.Name);
            }

            var searchResults = new Dictionary<string, List<TvShow>>();
            foreach (string showName in showDirs)
            {
                // Search for each of the shows using the directory name as the show name.
                List<TvShow> results = SearchShow(showName);

                // Any with only one result should be saved.
                if (results.Count == 1)
                {
                    var show = new TvShow(results[0]);
                    show.Save();
                }
                else
                {
                    // Any 0 or more than 1 result should be added to the dictionary for user selection.
                    searchResults.Add(showName, results);
                }
            }

            return searchResults;
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
            var tvdb = new Tvdb();
            return tvdb.SearchShow(name);
        }

        /// <summary>
        /// Deletes the show.
        /// </summary>
        public void Delete()
        {
            var xml = new Xml();
            xml.RemoveShow(this);
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
            var xml = new Xml();
            xml.SaveShow(this);
        }

        /// <summary>
        /// Updates the show's data.
        /// </summary>
        public void Update()
        {
            var tvdb = new Tvdb();
            tvdb.UpdateShow(this);
            this.Save();
        }

        #endregion

        #region Methods

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
        /// Initialises the object.
        /// </summary>
        private void Initialise()
        {
            // Search the XMl for the show.
            var showXml = new Xml();

            int count;
            showXml.FindTvShow(this, this.name, out count);
            if (count > 1)
            {
                throw new ArgumentException("There was more than one TV Show found for " + this.name);
            }

            var tvdb = new Tvdb();

            if (count == 0)
            {
                // Show could not be found. Attempt to add.
                List<TvShow> results = tvdb.SearchShow(this.name);
                if (results.Count == 0)
                {
                    throw new Exception("TV Show could not be found.");
                }

                if (results.Count == 1 || results[0].GetShowNames().Contains(this.name))
                {
                    this.Name = results[0].Name;
                    this.FolderName = GetFileSafeName(results[0].Name);
                    this.TvdbId = results[0].TvdbId;
                    this.InitialiseDefaultData();
                }
            }

            // If caching is not allowed or the data is more than 30 days old. Download new data.
            if (!this.allowCached || DateTime.Now - this.LastUpdated > TimeSpan.FromDays(30))
            {
                tvdb.UpdateShow(this);
                showXml.SaveShow(this);
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