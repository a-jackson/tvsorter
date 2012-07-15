// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Xml.cs">
//   2012 - Andrew Jackson
// </copyright>
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
        /// The current verison of the XML file.
        /// </summary>
        private const int XmlVersion = 1;

        /// <summary>
        /// The file path of the XSD file. Contains the version number of the XML file.
        /// </summary>
        private const string XsdFile = "TVSorter-{0}.xsd";

        #endregion

        #region Static Fields

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
        /// <param name="name">
        /// The name to get.
        /// </param>
        /// <returns>
        /// The XName.
        /// </returns>
        public static XName GetName(string name)
        {
            return XmlNamespace + name;
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

            XElement show =
                doc.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.Show.TvdbId);

            if (show == null)
            {
                throw new InvalidOperationException("The specified show is not saved.");
            }

            XElement episodes = show.Element(GetName("Episodes"));

            if (episodes == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement episodeElement =
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

            XElement settingsNode = doc.Root.Element(GetName("Settings"));
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

            XElement showElement =
                doc.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId);

            if (showElement != null)
            {
                showElement.ReplaceWith(show.ToXml());
            }
            else
            {
                XElement shows = doc.Descendants(GetName("Shows")).FirstOrDefault();
                if (shows == null)
                {
                    throw new XmlException("XML is invalid.");
                }

                shows.Add(show.ToXml());
            }

            doc.Save(XmlFile);
        }

        /// <summary>
        /// Saves a collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to save.
        /// </param>
        public void SaveShows(IEnumerable<TvShow> shows)
        {
            XDocument doc = this.GetDocument();

            XElement showsElement = doc.Descendants(GetName("Shows")).FirstOrDefault();
            if (showsElement == null)
            {
                throw new XmlException("XML is invalid.");
            }

            foreach (var show in shows)
            {
                // Check if the show already exists. Else add it.
                XElement showElement =
                    doc.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId);

                if (showElement != null)
                {
                    showElement.ReplaceWith(show.ToXml());
                }
                else
                {
                    showsElement.Add(show.ToXml());
                }
            }

            doc.Save(XmlFile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get the XDocument instance of the XML file.
        /// </summary>
        /// <returns>The XDocument.</returns>
        private XDocument GetDocument()
        {
            XDocument doc = XDocument.Load(XmlFile);
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
                var settings = new XmlReaderSettings() { ValidationType = ValidationType.Schema };
                settings.Schemas.Add(XmlNamespace.NamespaceName, schema);
                settings.ValidationEventHandler +=
                    (sender, args) => { throw new XmlSchemaValidationException(args.Message, args.Exception); };

                using (var reader = XmlReader.Create(XmlFile, settings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("The XSD schema could not be found.", schema);
            }

            return doc;
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
            IEnumerable<TvShow> tvshows = this.LoadTvShows();
            return tvshows.SelectMany(x => x.Episodes).Where(x => fileCountSelector(x.FileCount));
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

        #endregion
    }
}