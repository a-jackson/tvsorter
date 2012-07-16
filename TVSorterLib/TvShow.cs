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
    using System.Xml.Linq;

    using TVSorter.Data.Tvdb;
    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// The tv show.
    /// </summary>
    public class TvShow : IEquatable<TvShow>
    {
        #region Constructors and Destructors
        
        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class from a search result.
        /// </summary>
        /// <param name="searchResult">
        /// The search result.
        /// </param>
        public TvShow(TvShow searchResult)
        {
            this.Name = searchResult.Name;
            this.FolderName = searchResult.FolderName;
            this.TvdbId = searchResult.TvdbId;
            this.InitialiseDefaultData();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        internal TvShow()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShow"/> class from XML.
        /// </summary>
        /// <param name="element">
        /// The Show element.
        /// </param>
        internal TvShow(XElement element)
        {
            FromXml(element, this);
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
                    Logger.OnLogMessage(show1, "Created nfo file {0}", file);
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

            // Sort the directories so the show's are added alphabetically.
            showDirs.Sort();

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
                    Logger.OnLogMessage(show, "Found show {0}", show.Name);
                }
                else
                {
                    // Any 0 or more than 1 result should be added to the dictionary for user selection.
                    searchResults.Add(showName, results);
                    Logger.OnLogMessage(results, "Found {0} results for {1}", results.Count, showName);
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
        /// Loads the specified XElement into the show.
        /// </summary>
        /// <param name="showNode">
        /// The element to read from.
        /// </param>
        /// <param name="show">
        /// The show to read.
        /// </param>
        internal static void FromXml(XElement showNode, TvShow show)
        {
            show.Name = showNode.GetAttribute("name", string.Empty);
            show.FolderName = showNode.GetAttribute("foldername", string.Empty);
            show.TvdbId = showNode.GetAttribute("tvdbid", string.Empty);
            show.Banner = showNode.GetAttribute("banner", string.Empty);
            show.CustomFormat = showNode.GetAttribute("customformat", string.Empty);
            show.UseCustomFormat = bool.Parse(showNode.GetAttribute("usecustomformat", "false"));
            show.UseDvdOrder = bool.Parse(showNode.GetAttribute("usedvdorder", "false"));
            show.Locked = bool.Parse(showNode.GetAttribute("locked", "false"));
            show.LastUpdated = DateTime.Parse(showNode.GetAttribute("lastupdated", "1970-01-01 00:00:00"));
            show.AlternateNames = showNode.Descendants(Xml.GetName("AlternateName")).Select(altName => altName.Value).ToList();
            show.Episodes = showNode.Descendants(Xml.GetName("Episode")).Select(x => new Episode(x) { Show = show }).ToList();
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
        /// Converts the TVShow to XML.
        /// </summary>
        /// <returns>The XML element for the show.</returns>
        internal XElement ToXml()
        {
            var alternateNames = new XElement(Xml.GetName("AlternateNames"));
            if (this.AlternateNames != null)
            {
                alternateNames.Add(
                    this.AlternateNames.Select(
                        alternateName => new XElement(Xml.GetName("AlternateName"), alternateName)));
            }

            var episodes = new XElement(Xml.GetName("Episodes"));

            if (this.Episodes != null)
            {
                episodes.Add(this.Episodes.Select(x => x.ToXml()));
            }

            var element = new XElement(
                Xml.GetName("Show"), 
                new XAttribute("name", this.Name ?? string.Empty), 
                new XAttribute("foldername", this.FolderName ?? string.Empty), 
                new XAttribute("tvdbid", this.TvdbId ?? string.Empty), 
                new XAttribute("banner", this.Banner ?? string.Empty), 
                new XAttribute("customformat", this.CustomFormat ?? string.Empty), 
                new XAttribute("usecustomformat", this.UseCustomFormat), 
                new XAttribute("usedvdorder", this.UseDvdOrder), 
                new XAttribute("locked", this.Locked), 
                new XAttribute("lastupdated", this.LastUpdated), 
                alternateNames, 
                episodes);

            return element;
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