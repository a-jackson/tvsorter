// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Xml.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Class that manages access to the XML file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Storage
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    #endregion

    /// <summary>
    /// Class that manages access to the XML file.
    /// </summary>
    internal class Xml
    {
        #region Constants

        /// <summary>
        ///   The path the XML file.
        /// </summary>
        private const string XmlFile = "TVSorter.xml";

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Xml" /> class. Initialises a new instance of the TvShowXml class.
        /// </summary>
        public Xml()
        {
            this.Initialise();
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Finds a TV Show in the file and loads it.
        /// </summary>
        /// <param name="show">
        /// The TV Show to load data into.
        /// </param>
        /// <param name="name">
        /// The name of the show to search.
        /// </param>
        /// <param name="results">
        /// The number of matches found.
        /// </param>
        public void FindTvShow(TvShow show, string name, out int results)
        {
            // Remove any duplicate spaces
            name =
                name.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(
                    (x, y) => string.Concat(x, " ", y));

            XDocument doc = XDocument.Load(XmlFile);

            List<XElement> showElements = (from element in doc.Descendants("Show")
                                           let showNames = this.GetShowsNames(element)
                                           where showNames.Contains(name, StringComparer.InvariantCultureIgnoreCase)
                                           select element).ToList();
            results = showElements.Count;

            if (showElements.Count != 0 && showElements.Count <= 1)
            {
                LoadTvShow(showElements[0], show);
            }
        }

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
        /// Reads the settings from the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to set from the XML.
        /// </param>
        public void LoadSettings(Settings settings)
        {
            if (!File.Exists(XmlFile))
            {
                throw new IOException("Settings file does not exist.");
            }

            ValidateXml.Validate(XmlFile);

            try
            {
                XDocument doc = XDocument.Load(XmlFile);
                XElement settingsNode = doc.Root.Element("Settings");

                if (settingsNode == null)
                {
                    throw new XmlSchemaException("The XML file is invalid.");
                }

                settings.SourceDirectory = settingsNode.GetAttribute("sourcedirectory", string.Empty);
                settings.DefaultOutputFormat = settingsNode.GetAttribute("defaultformat", string.Empty);
                settings.RecurseSubdirectories = bool.Parse(settingsNode.GetAttribute("recursesubdirectories", "false"));
                settings.DeleteEmptySubdirectories =
                    bool.Parse(settingsNode.GetAttribute("deleteemptysubdirectories", "false"));
                settings.RenameIfExists = bool.Parse(settingsNode.GetAttribute("renameifexists", "false"));
                settings.DestinationDirectories = new List<string>();
                settings.DestinationDirectory = string.Empty;

                XElement destinationDirectories = settingsNode.Element("DestinationDirectories");

                if (destinationDirectories != null)
                {
                    settings.DestinationDirectories = destinationDirectories.GetElementsText("Destination").ToList();
                    foreach (XElement dir in from dir in destinationDirectories.Descendants("Destination")
                                             let selected = dir.GetAttribute("selected", "false")
                                             where bool.Parse(selected)
                                             select dir)
                    {
                        settings.DestinationDirectory = dir.Value;
                    }
                }

                settings.FileExtensions = settingsNode.Element("FileExtensions").GetElementsText("Extension").ToList();
                settings.RegularExpressions =
                    settingsNode.Element("RegularExpression").GetElementsText("RegEx").ToList();
                settings.OverwriteKeywords =
                    settingsNode.Element("OverwriteKeywords").GetElementsText("Keyword").ToList();
            }
            catch (Exception e)
            {
                throw new XmlException("Unable to load the Settings.", e);
            }
        }

        /// <summary>
        /// Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        /// The list of TV shows. 
        /// </returns>
        public IEnumerable<TvShow> LoadTvShows()
        {
            try
            {
                XDocument doc = XDocument.Load(XmlFile);

                return doc.Descendants("Show").Select(
                    x =>
                        {
                            var show = new TvShow();
                            LoadTvShow(x, show);
                            return show;
                        }).OrderBy(x => x.Name);
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
            }

            doc.Save(XmlFile);
        }

        /// <summary>
        /// Saves the specified episode.
        /// </summary>
        /// <param name="episode">
        /// The episode to save.
        /// </param>
        public void SaveEpisode(Episode episode)
        {
            if (episode == null)
            {
                throw new ArgumentNullException("episode", "Episode cannot be null.");
            }

            XDocument doc = XDocument.Load(XmlFile);

            XElement episodes = GetEpisodesElement(episode.Show, doc);
            SaveEpisode(false, episode, episodes);
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

            doc.Root.Element("Settings").ReplaceWith(
                new XElement(
                    "Settings", 
                    new XElement(
                        "DestinationDirectories", 
                        settings.DestinationDirectories.Select(
                            dir =>
                            new XElement(
                                "Destination", new XAttribute("selected", dir == settings.DestinationDirectory), dir))), 
                    new XElement(
                        "FileExtensions", settings.FileExtensions.Select(ext => new XElement("Extension", ext))), 
                    new XElement(
                        "RegularExpressions", settings.RegularExpressions.Select(exp => new XElement("RegEx", exp))), 
                    new XElement(
                        "OverwriteKeywords", settings.OverwriteKeywords.Select(key => new XElement("Keyword", key))), 
                    new XElement("SourceDirectory", settings.SourceDirectory), 
                    new XElement("DefaultFormat", settings.DefaultOutputFormat), 
                    new XElement("RecurseSubdirectories", settings.RecurseSubdirectories), 
                    new XElement("DeleteEmptySubdirectories", settings.DeleteEmptySubdirectories), 
                    new XElement("RenameIfExists", settings.RenameIfExists)));

            doc.Save(XmlFile);
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
                throw new Exception("XML file in invalid");
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

            XElement showsElement = doc.Root;
            if (showsElement == null)
            {
                throw new Exception("XML file is invalid");
            }

            foreach (TvShow show in shows)
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
            XDocument doc = XDocument.Load(XmlFile);

            var results = new Dictionary<TvShow, Dictionary<int, int>>();

            IEnumerable<XElement> shows = doc.Descendants("Show");

            foreach (XElement show in shows)
            {
                // Group the show's episodes by season number.
                IEnumerable<IGrouping<int, XElement>> seasons =
                    show.Descendants("Episode").GroupBy(x => int.Parse(x.GetAttribute("seasonnum", "-1")));

                var tvShow = new TvShow();
                LoadTvShow(show, tvShow);

                results.Add(tvShow, seasons.ToDictionary(x => x.Key, x => x.Count()));
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
                    element => element.GetAttribute("tvdbid", string.Empty).Equals(episode.TvdbId));

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
                    Name = episode.GetAttribute("name", string.Empty), 
                    TvdbId = episode.GetAttribute("tvdbid", string.Empty), 
                    SeasonNumber = int.Parse(episode.GetAttribute("seasonnum", "-1")), 
                    EpisodeNumber = int.Parse(episode.GetAttribute("episodenum", "-1")), 
                    FirstAir = DateTime.Parse(episode.GetAttribute("firstair", "1970-01-01 00:00:00")), 
                    FileCount = int.Parse(episode.GetAttribute("filecount", "0")), 
                };
        }

        /// <summary>
        /// Gets the episodes element from the specified document.
        /// </summary>
        /// <param name="show">
        /// The show to get the episodes for.
        /// </param>
        /// <param name="doc">
        /// The doc to get the episodes from.
        /// </param>
        /// <returns>
        /// The Episodes element.
        /// </returns>
        private static XElement GetEpisodesElement(TvShow show, XDocument doc)
        {
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

            return episodesElement;
        }

        /// <summary>
        /// Creates a TV show object from the specified XElement.
        /// </summary>
        /// <param name="element">
        /// The element to load from.
        /// </param>
        /// <param name="show">
        /// The show to load into.
        /// </param>
        private static void LoadTvShow(XElement element, TvShow show)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element", "Element cannot be null");
            }

            if (show == null)
            {
                throw new ArgumentNullException("show", "Show cannot be null");
            }

            show.Name = element.GetAttribute("name", string.Empty);
            show.FolderName = element.GetAttribute("foldername", string.Empty);
            show.TvdbId = element.GetAttribute("tvdbid", string.Empty);
            show.Banner = element.GetAttribute("banner", string.Empty);
            show.CustomFormat = element.GetAttribute("customformat", string.Empty);
            show.UseCustomFormat = bool.Parse(element.GetAttribute("usecustomformat", "false"));
            show.UseDvdOrder = bool.Parse(element.GetAttribute("usedvdorder", "false"));
            show.Locked = bool.Parse(element.GetAttribute("locked", "false"));
            show.LastUpdated = DateTime.Parse(element.GetAttribute("lastupdated", "1970-01-01 00:00:00"));
            show.AlternateNames = element.Descendants("AlternateName").Select(altName => altName.Value).ToList();
            show.Episodes = element.Descendants("Episode").Select(CreateEpisode);
        }

        /// <summary>
        /// Saves the specified episode into the specified element.
        /// </summary>
        /// <param name="retainFileCount">
        /// A value indicating whether the file count should be retained.
        /// </param>
        /// <param name="episode">
        /// The episode to save.
        /// </param>
        /// <param name="episodesElement">
        /// The parent element to save the episode to.
        /// </param>
        private static void SaveEpisode(bool retainFileCount, Episode episode, XElement episodesElement)
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
            XDocument doc = XDocument.Load(XmlFile);

            return from element in doc.Descendants("Episode")
                   where fileCountSelector(int.Parse(element.GetAttribute("filecount", "0")))
                   let episodesElement = element.Parent
                   where episodesElement != null
                   select CreateEpisode(element);
        }

        /// <summary>
        /// Gets all the possible show names from the specified show element.
        /// </summary>
        /// <param name="show">
        /// The show element to search.
        /// </param>
        /// <returns>
        /// The possible names for the show.
        /// </returns>
        private IEnumerable<string> GetShowsNames(XElement show)
        {
            // Return the show's name and file safe name.
            yield return show.GetAttribute("name");
            yield return TvShow.GetFileSafeName(show.GetAttribute("name"));

            // Return the show's folder name and file safe name.
            yield return show.GetAttribute("foldername");
            yield return TvShow.GetFileSafeName(show.GetAttribute("foldername"));

            // If the show has alternate names, return them and file safe names.
            XElement alternateNames = show.Element("AlternateNames");
            if (alternateNames == null)
            {
                yield break;
            }

            foreach (XElement alternateName in alternateNames.Elements("AlternateName"))
            {
                yield return alternateName.Value;
                yield return TvShow.GetFileSafeName(alternateName.Value);
            }
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
                    // Create the file and populate with default settings.
                    var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));

                    var root = new XElement("Shows");

                    doc.Add(root);
                    doc.Save(XmlFile);
                }

                ValidateXml.Validate(XmlFile);
            }
            catch (Exception e)
            {
                throw new IOException("Unable to load XML file.", e);
            }
        }

        /// <summary>
        /// Saves the specified collection of episodes for the specified show. Updates them if they already exist or adds if they do not.
        /// </summary>
        /// <param name="show">
        /// The show to save the episode of. 
        /// </param>
        /// <param name="showNode">
        /// The node to save into. 
        /// </param>
        /// <param name="retainFileCount">
        /// A value indicating whether the file count in the XML should be retained. 
        /// </param>
        private void SaveEpisodes(TvShow show, XElement showNode, bool retainFileCount)
        {
            if (show == null)
            {
                throw new ArgumentNullException("show");
            }

            XElement episodesElement = showNode.Element("Episodes");

            if (show.Episodes != null)
            {
                // Loop through each episode
                foreach (Episode episode in show.Episodes.Where(x => x != null))
                {
                    SaveEpisode(retainFileCount, episode, episodesElement);
                }
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
            }
            else
            {
                shows.Add(element);
            }

            this.SaveEpisodes(show, element, true);
        }

        #endregion
    }
}