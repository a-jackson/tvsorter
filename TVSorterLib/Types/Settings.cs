// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Types
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings
    {
        #region Public Properties

        /// <summary>
        ///   Gets Default.
        /// </summary>
        public static Settings Default
        {
            get
            {
                var regularExpressions = new List<string>();
                regularExpressions.Add(@"s(?<S>[0-9]+)e(?<E>[0-9]+)");
                regularExpressions.Add(@"(?<Y>19\d\d|20\d\d)[.](?<M>0[1-9]|1[012])[.](?<D>0[1-9]|[12][0-9]|3[01])");
                regularExpressions.Add(
                    @"(?<M>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\.(?<D>\d\d)\.(?<Y>20\d\d)");
                regularExpressions.Add(@"(?<S>[0-9]+)\s-\s(?<E>[0-9]+)");
                regularExpressions.Add(@"(?<S>[0-9]+)x(?<E>[0-9]+)");
                regularExpressions.Add(@"(?<S>[0-9][0-9])(?<E>[0-9][0-9])");
                regularExpressions.Add(@"(?<S>[0-9])(?<E>[0-9][0-9])");
                regularExpressions.Add(@"s(?<S>[0-9]+)[.]e(?<E>[0-9]+)");
                return new Settings
                    {
                        SourceDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), 
                        DestinationDirectories = new List<string>(), 
                        DestinationDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture), 
                        FileExtensions = new List<string> { ".avi", ".mkv", ".wmv", ".mpg", ".mp4" }, 
                        RegularExpressions = regularExpressions, 
                        DefaultOutputFormat =
                            "{SName( )}\\Season {SNum(1)}\\{SName(.)}." + "S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}", 
                        DeleteEmptySubdirectories = false, 
                        OverwriteKeywords = new List<string> { "repack", "proper" }, 
                        RecurseSubdirectories = false, 
                        RenameIfExists = false
                    };
            }
        }

        /// <summary>
        ///   Gets or sets DefaultOutputFormat.
        /// </summary>
        public string DefaultOutputFormat { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether DeleteEmptySubdirectories.
        /// </summary>
        public bool DeleteEmptySubdirectories { get; set; }

        /// <summary>
        ///   Gets or sets the list of destination directories.
        /// </summary>
        public List<string> DestinationDirectories { get; set; }

        /// <summary>
        ///   Gets or sets the selected destination directory.
        /// </summary>
        public string DestinationDirectory { get; set; }

        /// <summary>
        ///   Gets or sets the list of file extensions to search.
        /// </summary>
        public List<string> FileExtensions { get; set; }

        /// <summary>
        ///   Gets or sets OverwriteKeywords.
        /// </summary>
        public List<string> OverwriteKeywords { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether RecurseSubdirectories.
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

        #endregion
    }
}