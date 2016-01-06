// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Settings"/> class. 
        /// </summary>
        internal Settings()
        {
            this.SetDefault();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether unmatched shows should be added.
        /// </summary>
        public bool AddUnmatchedShows { get; set; }

        /// <summary>
        /// Gets or sets DefaultOutputFormat.
        /// </summary>
        public string DefaultOutputFormat { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether DeleteEmptySubdirectories.
        /// </summary>
        public bool DeleteEmptySubdirectories { get; set; }

        /// <summary>
        /// Gets or sets the list of destination directories.
        /// </summary>
        public List<string> DestinationDirectories { get; set; }

        /// <summary>
        /// Gets or sets the list of ignored directories.
        /// </summary>
        public List<string> IgnoredDirectories { get; set; }

        /// <summary>
        ///   Gets or sets the selected destination directory.
        /// </summary>
        public string DefaultDestinationDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the list of file extensions to search.
        /// </summary>
        public List<string> FileExtensions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to lock shows that have had no episodes in the past 3 weeks.
        /// </summary>
        public bool LockShowsWithNoEpisodes { get; set; }

        /// <summary>
        ///   Gets or sets OverwriteKeywords.
        /// </summary>
        public List<string> OverwriteKeywords { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to recursively search subdirectories.
        /// </summary>
        public bool RecurseSubdirectories { get; set; }

        /// <summary>
        ///   Gets or sets the list of regular expressions used to match shows.
        /// </summary>
        public List<string> RegularExpressions { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether RenameIfExists.
        /// </summary>
        public bool RenameIfExists { get; set; }

        /// <summary>
        ///   Gets or sets the source directory to scan.
        /// </summary>
        public string SourceDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether matched shows should be unlocked.
        /// </summary>
        public bool UnlockMatchedShows { get; set; }

        #endregion

        #region Public Methods and Operators
        
        /// <summary>
        ///   Sets default settings.
        /// </summary>
        private void SetDefault()
        {
            var regularExpressions = new List<string>
                {
                    @"s(?<S>[0-9]+)e((?<E>[0-9]+)[e-]{0,1})+", 
                    @"(?<Y>19\d\d|20\d\d)[.](?<M>0[1-9]|1[012])[.](?<D>0[1-9]|[12][0-9]|3[01])", 
                    @"(?<M>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\.(?<D>\d\d)\.(?<Y>20\d\d)", 
                    @"(?<S>[0-9]+)\s-\s(?<E>[0-9]+)", 
                    @"(?<S>[0-9]+)x(?<E>[0-9]+)", 
                    @"(?<S>[0-9][0-9])(?<E>[0-9][0-9])", 
                    @"(?<S>[0-9])(?<E>[0-9][0-9])", 
                    @"s(?<S>[0-9]+)[.]e(?<E>[0-9]+)"
                };

            this.SourceDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            this.DestinationDirectories = new List<string>();
            this.IgnoredDirectories = new List<string>();
            this.DefaultDestinationDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            this.FileExtensions = new List<string> { ".avi", ".mkv", ".wmv", ".mpg", ".mp4" };
            this.RegularExpressions = regularExpressions;
            this.DefaultOutputFormat = "{FName}" + Path.DirectorySeparatorChar + "Season {SNum(1)}"
                                       + Path.DirectorySeparatorChar + "{SName(.)}."
                                       + "S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}";
            this.DeleteEmptySubdirectories = false;
            this.OverwriteKeywords = new List<string> { "repack", "proper" };
            this.RecurseSubdirectories = false;
            this.RenameIfExists = false;
            this.UnlockMatchedShows = false;
            this.AddUnmatchedShows = false;
            this.LockShowsWithNoEpisodes = false;
        }

        #endregion
    }
}