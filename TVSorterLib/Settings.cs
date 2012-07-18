// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// The settings.
    /// </summary>
    public class Settings
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class. 
        /// </summary>
        internal Settings()
        {
            this.SetDefault();
        }

        #endregion

        #region Public Properties

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

        #region Public Methods and Operators

        /// <summary>
        /// Loads the settings from the default provider.
        /// </summary>
        /// <returns>The default provider.</returns>
        public static Settings LoadSettings()
        {
            return LoadSettings(Factory.StorageProvider);
        }

        /// <summary>
        /// Gets the destination directories as DirectoryInfo objects checking that they exist.
        /// </summary>
        /// <returns>The collection destination directories.</returns>
        public IEnumerable<DirectoryInfo> GetDestinationDirectories()
        {
            return from dir in this.DestinationDirectories where Directory.Exists(dir) select new DirectoryInfo(dir);
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        public void Save()
        {
            this.Save(Factory.StorageProvider);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings from the specified provider.
        /// </summary>
        /// <param name="provider">
        /// The provider to load the settings from.
        /// </param>
        /// <returns>
        /// The new settings object.
        /// </returns>
        internal static Settings LoadSettings(IStorageProvider provider)
        {
            var settings = new Settings();
            try
            {
                provider.LoadSettings(settings);
            }
            catch (IOException)
            {
                // Settings file does not exist. Use default settings.
                settings.SetDefault();
            }

            return settings;
        }

        /// <summary>
        /// Saves the settings with the specified provider.
        /// </summary>
        /// <param name="provider">
        /// The provider to save the settings to.
        /// </param>
        internal void Save(IStorageProvider provider)
        {
            provider.SaveSettings(this);
        }

        /// <summary>
        ///   Sets default settings.
        /// </summary>
        private void SetDefault()
        {
            var regularExpressions = new List<string>
                {
                    @"s(?<S>[0-9]+)e(?<E>[0-9]+)",
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
            this.DestinationDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            this.FileExtensions = new List<string> { ".avi", ".mkv", ".wmv", ".mpg", ".mp4" };
            this.RegularExpressions = regularExpressions;
            this.DefaultOutputFormat = "{FName}" + Path.DirectorySeparatorChar + "Season {SNum(1)}"
                                       + Path.DirectorySeparatorChar + "{SName(.)}."
                                       + "S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}";
            this.DeleteEmptySubdirectories = false;
            this.OverwriteKeywords = new List<string> { "repack", "proper" };
            this.RecurseSubdirectories = false;
            this.RenameIfExists = false;
        }

        #endregion
    }
}