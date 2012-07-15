// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.Schema;

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
        public Settings()
        {
            this.LoadSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class. With the option to use the default settings.
        /// </summary>
        /// <param name="useDefault">
        /// A value indicating whether the default settings should be used.
        /// </param>
        internal Settings(bool useDefault)
        {
            if (useDefault)
            {
                this.SetDefault();
            }
            else
            {
                this.LoadSettings();
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Settings"/> class form XML.
        /// </summary>
        /// <param name="element">
        /// The settings element.
        /// </param>
        internal Settings(XElement element)
        {
            FromXml(element, this);
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
            var xml = new Xml();
            xml.SaveSettings(this);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Populates the specified settings object from the XElement.
        /// </summary>
        /// <param name="settingsNode">
        /// The settings element.
        /// </param>
        /// <param name="settings">
        /// The settings object.
        /// </param>
        internal static void FromXml(XElement settingsNode, Settings settings)
        {
            settings.SourceDirectory = settingsNode.GetAttribute("sourcedirectory", string.Empty);
            settings.DefaultOutputFormat = settingsNode.GetAttribute("defaultformat", string.Empty);
            settings.RecurseSubdirectories = bool.Parse(settingsNode.GetAttribute("recursesubdirectories", "false"));
            settings.DeleteEmptySubdirectories =
                bool.Parse(settingsNode.GetAttribute("deleteemptysubdirectories", "false"));
            settings.RenameIfExists = bool.Parse(settingsNode.GetAttribute("renameifexists", "false"));
            settings.DestinationDirectories = new List<string>();
            settings.DestinationDirectory = string.Empty;

            XElement destinationDirectories = settingsNode.Element(Xml.GetName("DestinationDirectories"));

            if (destinationDirectories != null)
            {
                settings.DestinationDirectories =
                    destinationDirectories.GetElementsText(Xml.GetName("Destination")).ToList();
                foreach (XElement dir in from dir in destinationDirectories.Descendants(Xml.GetName("Destination"))
                                         let selected = dir.GetAttribute("selected", "false")
                                         where bool.Parse(selected)
                                         select dir)
                {
                    settings.DestinationDirectory = dir.Value;
                }
            }

            settings.FileExtensions =
                settingsNode.Element(Xml.GetName("FileExtensions")).GetElementsText(Xml.GetName("Extension")).ToList();
            settings.RegularExpressions =
                settingsNode.Element(Xml.GetName("RegularExpression")).GetElementsText(Xml.GetName("RegEx")).ToList();
            settings.OverwriteKeywords =
                settingsNode.Element(Xml.GetName("OverwriteKeywords")).GetElementsText(Xml.GetName("Keyword")).ToList();
        }

        /// <summary>
        /// Gets the XML for these settings.
        /// </summary>
        /// <returns>The XML element that represents these settings.</returns>
        internal XElement ToXml()
        {
            var destinationDirectories = new XElement(
                Xml.GetName("DestinationDirectories"), 
                this.DestinationDirectories.Select(
                    dir =>
                    new XElement(
                        Xml.GetName("Destination"), new XAttribute("selected", dir == this.DestinationDirectory), dir)));

            var fileExtensions = new XElement(
                Xml.GetName("FileExtensions"), 
                this.FileExtensions.Select(ext => new XElement(Xml.GetName("Extension"), ext)));

            var regularExpressions = new XElement(
                Xml.GetName("RegularExpression"), 
                this.RegularExpressions.Select(exp => new XElement(Xml.GetName("RegEx"), exp)));

            var overwriteKeywords = new XElement(
                Xml.GetName("OverwriteKeywords"), 
                this.OverwriteKeywords.Select(key => new XElement(Xml.GetName("Keyword"), key)));

            return new XElement(
                Xml.GetName("Settings"), 
                new XAttribute("sourcedirectory", this.SourceDirectory), 
                new XAttribute("defaultformat", this.DefaultOutputFormat), 
                new XAttribute("recursesubdirectories", this.RecurseSubdirectories), 
                new XAttribute("deleteemptysubdirectories", this.DeleteEmptySubdirectories), 
                new XAttribute("renameifexists", this.RenameIfExists), 
                destinationDirectories, 
                fileExtensions, 
                regularExpressions, 
                overwriteKeywords);
        }

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                var xml = new Xml();
                xml.LoadSettings(this);
            }
            catch (IOException)
            {
                // Settings file does not exist. Use default settings.
                this.SetDefault();
            }
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
            this.DefaultOutputFormat = "{SName( )}\\Season {SNum(1)}\\{SName(.)}."
                                       + "S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}";
            this.DeleteEmptySubdirectories = false;
            this.OverwriteKeywords = new List<string> { "repack", "proper" };
            this.RecurseSubdirectories = false;
            this.RenameIfExists = false;
        }

        #endregion
    }
}