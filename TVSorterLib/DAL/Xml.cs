// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Xml.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Class that manages access to the XML file.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// Class that manages access to the XML file.
    /// </summary>
    internal class Xml : DalBase, IStorageProvider
    {
        #region Constants and Fields

        /// <summary>
        ///   The path the XML file.
        /// </summary>
        private const string XmlFile = "TVSorter.xml";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Xml" /> class. Initialises a new instance of the Xml class.
        /// </summary>
        public Xml()
        {
            this.Initialise();
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the settings are chanaged.
        /// </summary>
        public event EventHandler SettingsChanged;

        /// <summary>
        ///   Occurs when a new show is added.
        /// </summary>
        public event EventHandler<ShowEventArgs> ShowAdded;

        /// <summary>
        ///   Occurs when a show is removed.
        /// </summary>
        public event EventHandler<ShowEventArgs> ShowRemoved;

        /// <summary>
        ///   Occurs when a show is updated.
        /// </summary>
        public event EventHandler<ShowEventArgs> ShowUpdated;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Gets the episodes that have more than 1 file grouped by show.
        /// </summary>
        /// <returns>
        /// The collection of episodes that are duplicated. 
        /// </returns>
        public IEnumerable<Episode> GetDuplicateEpisodes()
        {
            return this.GetEpisodes(x => x > 1);
        }

        /// <summary>
        /// Gets the episodes that are missing grouped by show.
        /// </summary>
        /// <returns>
        /// The collection of episodes that are missing. 
        /// </returns>
        public IEnumerable<Episode> GetMissingEpisodes()
        {
            return this.GetEpisodes(x => x == 0);
        }

        /// <summary>
        /// Loads the episodes for the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to get episodes for. 
        /// </param>
        /// <returns>
        /// The list of episodes for the show. 
        /// </returns>
        public List<Episode> LoadEpisodes(TvShow show)
        {
            XDocument doc = XDocument.Load(XmlFile);
            if (doc.Root == null)
            {
                throw new Exception("XML file is invalid.");
            }

            XElement showsElement = doc.Root.Element("Shows");
            if (showsElement == null)
            {
                throw new Exception("XML file is invalid");
            }

            XElement showElement =
                showsElement.Elements("Show").FirstOrDefault(x => GetAttribute(x, "tvdbid", "-1").Equals(show.TvdbId));
            if (showElement == null)
            {
                throw new Exception("Show cannot be found");
            }

            return showElement.Descendants("Episode").Select(CreateEpisode).ToList();
        }

        /// <summary>
        /// Reads the settings from the XML file.
        /// </summary>
        /// <returns>
        /// The settings object that represents the settings in the file. 
        /// </returns>
        public Settings LoadSettings()
        {
            try
            {
                XDocument doc = XDocument.Load(XmlFile);
                XElement settingsNode = doc.Descendants("Settings").FirstOrDefault();

                if (settingsNode == null)
                {
                    throw new Exception("XML is not valid");
                }

                Settings settings = CreateSettings(settingsNode);

                return settings;
            }
            catch
            {
                return Settings.Default;
            }
        }

        /// <summary>
        /// Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        /// The list of TV shows. 
        /// </returns>
        public List<TvShow> LoadTvShows()
        {
            try
            {
                XDocument doc = XDocument.Load(XmlFile);

                return doc.Descendants("Show").Select(CreateTvShow).OrderBy(x => x.Name).ToList();
            }
            catch
            {
                return new List<TvShow>();
            }
        }

        /// <summary>
        /// Removes the specified show from the storage.
        /// </summary>
        /// <param name="show">
        /// The show to remove. 
        /// </param>
        public void RemoveShow(TvShow show)
        {
            XDocument doc = XDocument.Load(XmlFile);

            XElement showElement = doc.Descendants("Show").Where(
                x =>
                    {
                        XAttribute tvdbId = x.Attribute("tvdbid");
                        return tvdbId != null && tvdbId.Value.Equals(show.TvdbId);
                    }).FirstOrDefault();

            if (showElement != null)
            {
                showElement.Remove();
                this.OnShowRemoved(show);
            }

            doc.Save(XmlFile);
        }

        /// <summary>
        /// Saves the specified collection of episodes for the specified show. Updates them if they already exist or adds if they do not.
        /// </summary>
        /// <param name="show">
        /// The show the episodes are for. 
        /// </param>
        /// <param name="episodes">
        /// The collection of episodes to save. 
        /// </param>
        /// <param name="retainFileCount">
        /// A value indicating whether the file count in the XML should be retained. 
        /// </param>
        public void SaveEpisodes(TvShow show, IEnumerable<Episode> episodes, bool retainFileCount)
        {
            if (show == null)
            {
                throw new ArgumentNullException("show");
            }

            if (episodes == null)
            {
                throw new ArgumentNullException("episodes");
            }

            XDocument doc = XDocument.Load(XmlFile);

            if (doc.Root == null)
            {
                throw new Exception("XML file is invalid");
            }

            // Ensure that Shows exists in the file
            XElement showsElement = doc.Root.Element("Shows");
            if (showsElement == null)
            {
                throw new Exception("XML file is invalid");
            }

            // Find the specified show
            XElement showElement = showsElement.Descendants("Show").Where(
                x =>
                    {
                        XAttribute tvdbid = x.Attribute("tvdbid");
                        return tvdbid != null && tvdbid.Value.Equals(show.TvdbId);
                    }).FirstOrDefault();
            if (showElement == null)
            {
                throw new Exception("Show cannot be found.");
            }

            // Ensure that the Episodes element exists and add it if it doesn't
            XElement episodesElement = showElement.Element("Episodes");
            if (episodesElement == null)
            {
                episodesElement = new XElement("Episodes");
                showElement.Add(episodesElement);
            }

            // Loop through each episode
            foreach (var episode in episodes.Where(x => x != null))
            {
                // Create the XML element
                XElement episodeElement = CreateElement(episode);

                // Check to see if this episode already exists.
                XElement existingElement = episodesElement.Descendants("Episode").Where(
                    x =>
                        {
                            XAttribute tvdbid = x.Attribute("tvdbid");
                            return tvdbid != null && tvdbid.Value.Equals(episode.TvdbId);
                        }).FirstOrDefault();

                // Replace the elment if it exists and add it if it does.
                if (existingElement != null)
                {
                    if (retainFileCount)
                    {
                        XAttribute newCount = episodeElement.Attribute("filecount");
                        XAttribute existingCount = existingElement.Attribute("filecount");
                        if (newCount != null && existingCount != null)
                        {
                            newCount.Value = existingCount.Value;
                        }
                    }

                    existingElement.ReplaceWith(episodeElement);
                }
                else
                {
                    episodesElement.Add(episodeElement);
                }
            }

            doc.Save(XmlFile);
        }

        /// <summary>
        /// Saves the specified settings into the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to save. 
        /// </param>
        public void SaveSettings(Settings settings)
        {
            XDocument doc = XDocument.Load(XmlFile);

            XElement newSettingsElement = CreateElement(settings);

            if (doc.Root != null)
            {
                XElement settingsElement = doc.Root.Element("Settings");
                if (settingsElement != null)
                {
                    settingsElement.ReplaceWith(newSettingsElement);
                }
                else
                {
                    doc.Root.AddFirst(newSettingsElement);
                }
            }

            doc.Save(XmlFile);
            this.OnSettingsChanged();
        }

        /// <summary>
        /// Saves the specified show. Updates if it already exists and adds if it doesn't
        /// </summary>
        /// <param name="show">
        /// The show to save. 
        /// </param>
        public void SaveShow(TvShow show)
        {
            XDocument doc = XDocument.Load(XmlFile);
            if (doc.Root == null)
            {
                throw new Exception("XML file is invalid.");
            }

            XElement shows = doc.Root.Element("Shows");
            if (shows == null)
            {
                throw new Exception("XML file is invalid");
            }

            this.UpdateShow(show, shows);
            doc.Save(XmlFile);
        }

        /// <summary>
        /// Saves the specified collection of shows. Updates them if they already exist or adds if they do not.
        /// </summary>
        /// <param name="shows">
        /// The collection of shows to save. 
        /// </param>
        public void SaveShow(IEnumerable<TvShow> shows)
        {
            XDocument doc = XDocument.Load(XmlFile);
            if (doc.Root == null)
            {
                throw new Exception("XML file is invalid.");
            }

            XElement showsElement = doc.Root.Element("Shows");
            if (showsElement == null)
            {
                throw new Exception("XML file is invalid");
            }

            foreach (var show in shows)
            {
                this.UpdateShow(show, showsElement);
            }

            doc.Save(XmlFile);
        }

        /// <summary>
        /// Gets the number of episodes in every season.
        /// </summary>
        /// <returns>
        /// The number of episodes in each season.
        /// </returns>
        public Dictionary<TvShow, Dictionary<int, int>> SeasonEpisodeCount()
        {
            var doc = XDocument.Load(XmlFile);

            var results = new Dictionary<TvShow, Dictionary<int, int>>();

            var shows = doc.Descendants("Show");

            foreach (var show in shows)
            {
                var seasons = show.Descendants("Episode").GroupBy(x => int.Parse(GetAttribute(x, "seasonnum", "-1")));

                results.Add(CreateTvShow(show), seasons.ToDictionary(x => x.Key, x => x.Count()));
            }

            return results;
        }

        /// <summary>
        /// Updates the episode.
        /// </summary>
        /// <param name="episode">
        /// The episode to update. 
        /// </param>
        public void UpdateEpisode(Episode episode)
        {
            XDocument doc = XDocument.Load(XmlFile);

            XElement episodeElement =
                doc.Descendants("Episode").FirstOrDefault(
                    element => GetAttribute(element, "tvdbid", string.Empty).Equals(episode.TvdbId));

            if (episodeElement == null)
            {
                throw new Exception("Episode doesn't exists");
            }

            XElement newElement = CreateElement(episode);

            episodeElement.ReplaceWith(newElement);

            doc.Save(XmlFile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates an XElement from the specified settings.
        /// </summary>
        /// <param name="settings">
        /// The settings to convert. 
        /// </param>
        /// <returns>
        /// The XElement of the settings. 
        /// </returns>
        private static XElement CreateElement(Settings settings)
        {
            var settingsNode = new XElement("Settings");
            var sourceDirectory = new XAttribute("sourcedirectory", settings.SourceDirectory);
            var defaultFormat = new XAttribute("defaultformat", settings.DefaultOutputFormat);
            var recurseSubdirectories = new XAttribute("recursesubdirectories", settings.RecurseSubdirectories);
            var deleteEmptySubdirectories = new XAttribute(
                "deleteemptysubdirectories", settings.DeleteEmptySubdirectories);
            var renameIfExists = new XAttribute("renameifexists", settings.RenameIfExists);

            var directories =
                settings.DestinationDirectories.Select(
                    dir =>
                    new XElement("Destination", new XAttribute("selected", dir == settings.DestinationDirectory), dir));

            var destinationDirectory = new XElement("DestinationDirectories", directories);
            var fileExtensions = new XElement(
                "FileExtensions", from ext in settings.FileExtensions select new XElement("Extension", ext));
            var regularExpress = new XElement(
                "RegularExpression", 
                from regularExpression in settings.RegularExpressions select new XElement("RegEx", regularExpression));
            var overwriteKeywords = new XElement(
                "OverwriteKeywords", from keyword in settings.OverwriteKeywords select new XElement("Keyword", keyword));

            settingsNode.Add(
                sourceDirectory, 
                defaultFormat, 
                recurseSubdirectories, 
                deleteEmptySubdirectories, 
                renameIfExists, 
                destinationDirectory, 
                fileExtensions, 
                regularExpress, 
                overwriteKeywords);
            return settingsNode;
        }

        /// <summary>
        /// Converts the specified episode into an XElement.
        /// </summary>
        /// <param name="episode">
        /// The episode to convert. 
        /// </param>
        /// <returns>
        /// The converted episode XElement. 
        /// </returns>
        private static XElement CreateElement(Episode episode)
        {
            var episodeElement = new XElement(
                "Episode", 
                new XAttribute("name", episode.Name), 
                new XAttribute("tvdbid", episode.TvdbId), 
                new XAttribute("seasonnum", episode.SeasonNumber), 
                new XAttribute("episodenum", episode.EpisodeNumber), 
                new XAttribute("firstair", episode.FirstAir), 
                new XAttribute("filecount", episode.FileCount));
            return episodeElement;
        }

        /// <summary>
        /// Converts the specified show into an XElement.
        /// </summary>
        /// <param name="show">
        /// The show to convert. 
        /// </param>
        /// <returns>
        /// The converted show XElement. 
        /// </returns>
        private static XElement CreateElement(TvShow show)
        {
            XElement alternateNames;
            if (show.AlternateNames != null)
            {
                alternateNames = new XElement(
                    "AlternateNames", show.AlternateNames.Select(name => new XElement("AlternateName", name)));
            }
            else
            {
                alternateNames = new XElement("AlternateNames");
            }

            var element = new XElement(
                "Show", 
                new XAttribute("name", show.Name ?? string.Empty), 
                new XAttribute("foldername", show.FolderName ?? string.Empty), 
                new XAttribute("tvdbid", show.TvdbId ?? string.Empty), 
                new XAttribute("banner", show.Banner ?? string.Empty), 
                new XAttribute("customformat", show.CustomFormat ?? string.Empty), 
                new XAttribute("usecustomformat", show.UseCustomFormat), 
                new XAttribute("usedvdorder", show.UseDvdOrder), 
                new XAttribute("locked", show.Locked), 
                new XAttribute("lastupdated", show.LastUpdated), 
                alternateNames);
            return element;
        }

        /// <summary>
        /// Creates an episode object from the specified element.
        /// </summary>
        /// <param name="episode">
        /// The episode element. 
        /// </param>
        /// <returns>
        /// The episode object. 
        /// </returns>
        private static Episode CreateEpisode(XElement episode)
        {
            if (episode == null)
            {
                return null;
            }

            return new Episode
                {
                    Name = GetAttribute(episode, "name", string.Empty), 
                    TvdbId = GetAttribute(episode, "tvdbid", string.Empty), 
                    SeasonNumber = int.Parse(GetAttribute(episode, "seasonnum", "-1")), 
                    EpisodeNumber = int.Parse(GetAttribute(episode, "episodenum", "-1")), 
                    FirstAir = DateTime.Parse(GetAttribute(episode, "firstair", "1970-01-01 00:00:00")), 
                    FileCount = int.Parse(GetAttribute(episode, "filecount", "0")), 
                    Show = episode.Parent == null ? null : CreateTvShow(episode.Parent.Parent), 
                };
        }

        /// <summary>
        /// Converts the XElement into a Settings object.
        /// </summary>
        /// <param name="settingsNode">
        /// The settings node. 
        /// </param>
        /// <returns>
        /// The settings object. 
        /// </returns>
        private static Settings CreateSettings(XElement settingsNode)
        {
            if (settingsNode == null)
            {
                return null;
            }

            var settings = new Settings
                {
                    SourceDirectory = GetAttribute(settingsNode, "sourcedirectory", string.Empty), 
                    DefaultOutputFormat = GetAttribute(settingsNode, "defaultformat", string.Empty), 
                    RecurseSubdirectories = bool.Parse(GetAttribute(settingsNode, "recursesubdirectories", "false")), 
                    DeleteEmptySubdirectories =
                        bool.Parse(GetAttribute(settingsNode, "deleteemptysubdirectories", "false")), 
                    RenameIfExists = bool.Parse(GetAttribute(settingsNode, "renameifexists", "false")), 
                    DestinationDirectories = new List<string>(), 
                    DestinationDirectory = string.Empty
                };

            XElement destinationDirectories = settingsNode.Element("DestinationDirectories");

            if (destinationDirectories != null)
            {
                foreach (var dir in destinationDirectories.Descendants("Destination"))
                {
                    settings.DestinationDirectories.Add(dir.Value);
                    var attribute = dir.Attribute("selected");
                    if (attribute != null && bool.Parse(attribute.Value))
                    {
                        settings.DestinationDirectory = dir.Value;
                    }
                }
            }

            settings.FileExtensions =
                (from extension in settingsNode.Descendants("Extension") select extension.Value).ToList();

            settings.RegularExpressions = (from regex in settingsNode.Descendants("RegEx") select regex.Value).ToList();

            settings.OverwriteKeywords =
                (from keyword in settingsNode.Descendants("Keyword") select keyword.Value).ToList();
            return settings;
        }

        /// <summary>
        /// Creates a TV show object from the specified XElement.
        /// </summary>
        /// <param name="show">
        /// The show element. 
        /// </param>
        /// <returns>
        /// The TvShow object. 
        /// </returns>
        private static TvShow CreateTvShow(XElement show)
        {
            if (show == null)
            {
                return null;
            }

            return new TvShow
                {
                    Name = GetAttribute(show, "name", string.Empty), 
                    FolderName = GetAttribute(show, "foldername", string.Empty), 
                    TvdbId = GetAttribute(show, "tvdbid", string.Empty), 
                    Banner = GetAttribute(show, "banner", string.Empty), 
                    CustomFormat = GetAttribute(show, "customformat", string.Empty), 
                    UseCustomFormat = bool.Parse(GetAttribute(show, "usecustomformat", "false")), 
                    UseDvdOrder = bool.Parse(GetAttribute(show, "usedvdorder", "false")), 
                    Locked = bool.Parse(GetAttribute(show, "locked", "false")), 
                    LastUpdated = DateTime.Parse(GetAttribute(show, "lastupdated", "1970-01-01 00:00:00")), 
                    AlternateNames = show.Descendants("AlternateName").Select(altName => altName.Value).ToList()
                };
        }

        /// <summary>
        /// Gets the value of the specified attribute from the specified element.
        /// </summary>
        /// <param name="element">
        /// The element to read the attribute from. 
        /// </param>
        /// <param name="name">
        /// The name of the attribute. 
        /// </param>
        /// <param name="defaultValue">
        /// The value to return if the attribute doesn't exist. 
        /// </param>
        /// <returns>
        /// The value of the specified attribute or default value if it does not exist. 
        /// </returns>
        private static string GetAttribute(XElement element, string name, string defaultValue)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            XAttribute attribute = element.Attribute(name);
            return attribute != null ? attribute.Value : defaultValue;
        }

        /// <summary>
        /// Gets the episodes based on the specified function of file count.
        /// </summary>
        /// <param name="fileCountSelector">
        /// The function to determine if the episode should be selected based on file count. 
        /// </param>
        /// <returns>
        /// The collection of episodes that match the predicate. 
        /// </returns>
        private IEnumerable<Episode> GetEpisodes(Func<int, bool> fileCountSelector)
        {
            var doc = XDocument.Load(XmlFile);

            return from element in doc.Descendants("Episode")
                   where fileCountSelector(int.Parse(GetAttribute(element, "filecount", "0")))
                   let episodesElement = element.Parent
                   where episodesElement != null
                   let show = CreateTvShow(episodesElement.Parent)
                   select CreateEpisode(element);
        }

        /// <summary>
        /// Initialises the class for reading the XML file.
        /// </summary>
        private void Initialise()
        {
            try
            {
                // Check that the file exists
                if (!File.Exists(XmlFile))
                {
                    Settings settings = Settings.Default;

                    // Create the file and populate with default settings.
                    var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

                    var root = new XElement("TVSorter");
                    XElement settingsNode = CreateElement(settings);

                    var showsNode = new XElement("Shows");

                    root.Add(settingsNode, showsNode);

                    doc.Add(root);
                    doc.Save(XmlFile);
                }
            }
            catch (Exception e)
            {
                this.OnLogMessage("Unable to open XML file. {0}", e.Message);
            }
        }

        /// <summary>
        /// Raises a settings changed event.
        /// </summary>
        private void OnSettingsChanged()
        {
            if (this.SettingsChanged != null)
            {
                this.SettingsChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises an on show added event.
        /// </summary>
        /// <param name="show">
        /// The show that has been added. 
        /// </param>
        private void OnShowAdded(TvShow show)
        {
            if (this.ShowAdded != null)
            {
                this.ShowAdded(this, new ShowEventArgs(show));
            }
        }

        /// <summary>
        /// Raises an on show removed event.
        /// </summary>
        /// <param name="show">
        /// The show that has been removed. 
        /// </param>
        private void OnShowRemoved(TvShow show)
        {
            if (this.ShowRemoved != null)
            {
                this.ShowRemoved(this, new ShowEventArgs(show));
            }
        }

        /// <summary>
        /// Raises an on show updated event.
        /// </summary>
        /// <param name="show">
        /// The show that has been updated. 
        /// </param>
        private void OnShowUpdated(TvShow show)
        {
            if (this.ShowUpdated != null)
            {
                this.ShowUpdated(this, new ShowEventArgs(show));
            }
        }

        /// <summary>
        /// Updates the specified show into the specified shows element.
        /// </summary>
        /// <param name="show">
        /// The show to update/insert. 
        /// </param>
        /// <param name="shows">
        /// The shows element. 
        /// </param>
        private void UpdateShow(TvShow show, XElement shows)
        {
            XElement element = CreateElement(show);

            XElement showNode = shows.Descendants("Show").Where(
                x =>
                    {
                        XAttribute xAttribute = x.Attribute("tvdbid");
                        return xAttribute != null && xAttribute.Value == show.TvdbId;
                    }).FirstOrDefault();
            if (showNode != null)
            {
                XElement episodes = showNode.Element("Episodes");
                if (episodes != null)
                {
                    element.Add(episodes);
                }

                showNode.ReplaceWith(element);
                this.OnShowUpdated(show);
            }
            else
            {
                shows.Add(element);
                this.OnShowAdded(show);
            }
        }

        #endregion
    }
}