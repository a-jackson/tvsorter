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

        /// <summary>
        /// The file path of the XSD file. Contains the version number of the XML file.
        /// </summary>
        private const string XsdFile = "TVSorter-{0}.xsd";

        /// <summary>
        /// The current verison of the XML file.
        /// </summary>
        private const int XmlVersion = 1;

        /// <summary>
        /// The namespace of the XML.
        /// </summary>
        private static readonly XNamespace XmlNamespace = "http://code.google.com/p/tvsorter";

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
        /// Gets the XName for a name.
        /// </summary>
        /// <param name="name">The name to get.</param>
        /// <returns>The XName.</returns>
        public static XName GetName(string name)
        {
            return XmlNamespace + name;
        }

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

            XDocument doc = this.GetDocument();

            List<XElement> showElements = (from element in doc.Descendants(GetName("Show"))
                                           let tvShow = new TvShow(element)
                                           let showNames = tvShow.GetShowNames()
                                           where showNames.Contains(name, StringComparer.InvariantCultureIgnoreCase)
                                           select element).ToList();
            results = showElements.Count;

            if (showElements.Count != 0 && showElements.Count <= 1)
            {
                TvShow.FromXml(showElements[0], show);
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
            XDocument doc = this.GetDocument();
            if (doc.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement settingsNode = doc.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            Settings.FromXml(settingsNode, settings);
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
                XDocument doc = this.GetDocument();

                return doc.Descendants(GetName("Show")).Select(x => new TvShow(x)).OrderBy(x => x.Name);
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
            XDocument doc = this.GetDocument();

            XElement showElement = doc.Descendants(GetName("Show")).Where(
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

            if (episode.Show == null)
            {
                throw new NullReferenceException("The episode's show parameter cannot be null.");
            }

            XDocument doc = this.GetDocument();

            var show = doc.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.Show.TvdbId);

            if (show == null)
            {
                throw new InvalidOperationException("The specified show is not saved.");
            }

            var episodes = show.Element(GetName("Episodes"));

            if (episodes == null)
            {
                throw new XmlException("XML is invalid.");
            }
            
            var episodeElement =
                episodes.Elements(GetName("Episode")).FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.TvdbId);

            if (episodeElement != null)
            {
                episodeElement.ReplaceWith(episode.ToXml());
            }
            else
            {
                episodes.Add(episode.ToXml());
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
            XDocument doc = this.GetDocument();

            if (doc.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = doc.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }
            
            settingsNode.ReplaceWith(settings.ToXml());
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
            XDocument doc = this.GetDocument();

            var showElement = doc.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId);

            if (showElement != null)
            {
                showElement.ReplaceWith(show.ToXml());
            }
            else
            {
                var shows = doc.Descendants(GetName("Shows")).FirstOrDefault();
                if (shows == null)
                {
                    throw new XmlException("XML is invalid.");
                }
                
                shows.Add(show.ToXml());
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
            XDocument doc = this.GetDocument();

            var results = new Dictionary<TvShow, Dictionary<int, int>>();

            IEnumerable<XElement> shows = doc.Descendants(GetName("Show"));

            foreach (XElement show in shows)
            {
                // Group the show's episodes by season number.
                IEnumerable<IGrouping<int, XElement>> seasons =
                    show.Descendants(GetName("Episode")).GroupBy(x => int.Parse(x.GetAttribute("seasonnum", "-1")));

                var tvShow = new TvShow(show);
                results.Add(tvShow, seasons.ToDictionary(x => x.Key, x => x.Count()));
            }

            return results;
        }
        
        #endregion

        #region Methods

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
            XDocument doc = this.GetDocument();

            return from element in doc.Descendants(GetName("Episode"))
                   where fileCountSelector(int.Parse(element.GetAttribute("filecount", "0")))
                   let episodesElement = element.Parent
                   where episodesElement != null
                   select new Episode(element);
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
                    var doc = new XDocument(
                        new XDeclaration(XmlVersion + ".0", "utf-8", "yes"),
                        new XElement(GetName("TVSorter"), new Settings(true).ToXml(), new XElement(GetName("Shows"))));

                    doc.Save(XmlFile);
                }
            }
            catch (Exception e)
            {
                throw new IOException("Unable to load XML file.", e);
            }
        }
       
        /// <summary>
        /// Get the XDocument instance of the XML file.
        /// </summary>
        /// <returns>The XDocument.</returns>
        private XDocument GetDocument()
        {
            var doc = XDocument.Load(XmlFile);
            var version = (int)float.Parse(doc.Declaration.Version);

            // Check that the XML is up to date.
            if (version < XmlVersion)
            {
                // Update the latest version
            }

            string schema = string.Format(XsdFile, doc.Declaration.Version);

            if (File.Exists(schema))
            {
                // Get the schemas to validate against.
                var schemas = new XmlSchemaSet();
                schemas.Add(XmlNamespace.NamespaceName, schema);

                doc.Validate(
                    schemas, (sender, args) => { throw new XmlSchemaValidationException(args.Message, args.Exception); });
            }
            else
            {
                throw new FileNotFoundException("The XSD schema could not be found.", schema);
            }

            return doc;
        }

        #endregion
    }
}