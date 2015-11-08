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
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Schema;

    using TVSorter.Model;

    #endregion

    /// <summary>
    /// Class that manages access to the XML file.
    /// </summary>
    internal class Xml : IStorageProvider
    {
        #region Constants

        /// <summary>
        ///   The path the XML file.
        /// </summary>
        private const string XmlFile = "TVSorter.xml";

        /// <summary>
        /// The current verison of the XML file.
        /// </summary>
        private static readonly int XmlVersion = 5;

        /// <summary>
        /// The file path of the XSD file. Contains the version number of the XML file.
        /// </summary>
        private const string XsdFile = "TVSorter-{0}.0.xsd";

        #endregion

        #region Static Fields

        /// <summary>
        /// The namespace of the XML.
        /// </summary>
        private static readonly XNamespace XmlNamespace = "http://code.google.com/p/tvsorter";

        #endregion

        #region Fields

        /// <summary>
        /// The XML Document.
        /// </summary>
        private XDocument document;

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

        #region Public Events

        /// <summary>
        /// Occurs when settings are saved.
        /// </summary>
        public event EventHandler SettingsSaved;

        /// <summary>
        /// Occurs when a TV Show is added.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        /// Occurs when a TV Show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        /// Occurs when a TV Show is removed.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowRemoved;

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
        /// Loads the missing episode settings from the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to load into.
        /// </param>
        public void LoadMissingEpisodeSettings(MissingEpisodeSettings settings)
        {
            if (this.document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement settingsNode = this.document.Root.Element(GetName("MissingEpisodeSettings"));
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            settings.FromXml(settingsNode);
        }

        /// <summary>
        /// Reads the settings from the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to set from the XML.
        /// </param>
        public void LoadSettings(Settings settings)
        {
            if (this.document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement settingsNode = this.document.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            settings.FromXml(settingsNode);
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
                return this.document.Descendants(GetName("Show")).Select(NewTvShow).OrderBy(x => x.Name);
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
            XElement showElement = this.document.Descendants(GetName("Show")).Where(
                x =>
                    {
                        XAttribute tvdbId = x.Attribute("tvdbid");
                        return tvdbId != null && tvdbId.Value.Equals(show.TvdbId);
                    }).FirstOrDefault();

            if (showElement != null)
            {
                showElement.Remove();
                this.OnTvShowRemoved(show);
            }

            this.document.Save(XmlFile);
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

            XElement show =
                this.document.Descendants(GetName("Show")).FirstOrDefault(
                    x => x.GetAttribute("tvdbid") == episode.Show.TvdbId);

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

            this.document.Save(XmlFile);
        }

        /// <summary>
        /// Saves the missing episode settings into the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to save.
        /// </param>
        public void SaveMissingEpisodeSettings(MissingEpisodeSettings settings)
        {
            if (this.document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement settingsNode = this.document.Root.Element(GetName("MissingEpisodeSettings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(settings.ToXml());
            this.document.Save(XmlFile);
        }

        /// <summary>
        /// Saves the specified settings into the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to save. 
        /// </param>
        public void SaveSettings(Settings settings)
        {
            if (this.document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            XElement settingsNode = this.document.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(settings.ToXml());
            this.document.Save(XmlFile);
            this.OnSettingsSaved();
        }

        /// <summary>
        /// Saves the specified show. Updates if it already exists and adds if it doesn't
        /// </summary>
        /// <param name="show">
        /// The show to save. 
        /// </param>
        public void SaveShow(TvShow show)
        {
            XElement showElement =
                this.document.Descendants(GetName("Show")).FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId);

            if (showElement != null)
            {
                showElement.ReplaceWith(show.ToXml());
                this.OnTvShowChanged(show);
            }
            else
            {
                XElement shows = this.document.Descendants(GetName("Shows")).FirstOrDefault();
                if (shows == null)
                {
                    throw new XmlException("XML is invalid.");
                }

                shows.Add(show.ToXml());
                this.OnTvShowAdded(show);
            }

            this.document.Save(XmlFile);
        }

        /// <summary>
        /// Saves a collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to save.
        /// </param>
        public void SaveShows(IEnumerable<TvShow> shows)
        {
            XElement showsElement = this.document.Descendants(GetName("Shows")).FirstOrDefault();
            if (showsElement == null)
            {
                throw new XmlException("XML is invalid.");
            }

            foreach (TvShow show in shows)
            {
                // Check if the show already exists. Else add it.
                XElement showElement =
                    this.document.Descendants(GetName("Show")).FirstOrDefault(
                        x => x.GetAttribute("tvdbid") == show.TvdbId);

                if (showElement != null)
                {
                    showElement.ReplaceWith(show.ToXml());
                    this.OnTvShowChanged(show);
                }
                else
                {
                    showsElement.Add(show.ToXml());
                    this.OnTvShowAdded(show);
                }
            }

            this.document.Save(XmlFile);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a new TVShow from the specified element.
        /// </summary>
        /// <param name="element">
        /// The element to load.
        /// </param>
        /// <returns>
        /// The new TV Show.
        /// </returns>
        private static TvShow NewTvShow(XElement element)
        {
            var show = new TvShow();
            show.FromXml(element);
            return show;
        }

        /// <summary>
        /// Updates the specified element to XML format version 2.
        /// </summary>
        /// <param name="root">
        /// The root element of the document.
        /// </param>
        private static void UpdateToVersion2(XElement root)
        {
            XElement settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid.");
            }

            // Update the first regualar expression to use the one with dual episode support
            XElement regularExpression = settingsNode.Element(GetName("RegularExpression"));
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            XElement regex = regularExpression.Element(GetName("RegEx"));

            // If it is the old text then replace with the new text
            if (regex != null && regex.Value.Equals("s(?<S>[0-9]+)e(?<E>[0-9]+)"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+";
            }

            // Add the new missing episode settings node.
            settingsNode.AddAfterSelf(new MissingEpisodeSettings().ToXml());
        }

        /// <summary>
        /// Updates the XML to verison 3.
        /// </summary>
        /// <param name="root">
        /// The root of the document.
        /// </param>
        private static void UpdateToVersion3(XElement root)
        {
            XElement settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid");
            }

            settingsNode.Add(new XAttribute("addunmatchedshows", false));
            settingsNode.Add(new XAttribute("unlockmatchedshows", false));
            settingsNode.Add(new XAttribute("lockshowsnonewepisodes", false));
        }

        /// <summary>
        /// Updates the XML to version 4
        /// </summary>
        /// <param name="root">The root of the document.</param>
        private static void UpdateToVersion4(XElement root)
        {
            XElement settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            string selectedDestination = string.Empty;
            var destinations = settingsNode.Descendants(GetName("Destination"));
            foreach (XElement destination in destinations)
            {
                if (bool.Parse(destination.GetAttribute("selected", "false")))
                {
                    selectedDestination = destination.Value;
                }

                destination.RemoveAttributes();
            }

            settingsNode.Add(new XAttribute("defaultdestinationdir", selectedDestination));
         
            // Update the first regualar expression
            XElement regularExpression = settingsNode.Element(GetName("RegularExpression"));
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            XElement regex = regularExpression.Element(GetName("RegEx"));

            // If it is the old text then replace with the new text
            if (regex != null && regex.Value.Equals("s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)[e-]{0,1})+";
            }

            foreach (XElement show in root.Descendants(GetName("Show")))
            {
                show.Add(new XAttribute("usecustomdestination", false));
                show.Add(new XAttribute("customdestinationdir", string.Empty));
            }
        }

        private void UpdateToVersion5(XElement root)
        {
            XElement settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            var destinationDirectories = settingsNode.FirstNode;
            destinationDirectories.AddAfterSelf(new XElement("IgnoredDirectories", new string[] { }));         
        }


        /// <summary>
        /// Get the XDocument instance of the XML file.
        /// </summary>
        private void GetDocument()
        {
            this.document = XDocument.Load(XmlFile);
            if (this.document.Root == null)
            {
                throw new XmlSchemaValidationException("The XML file is invalid.");
            }

            XElement root = this.document.Root;

            int version = int.Parse(this.document.Root.GetAttribute("version", "0"));
            if (version == 0)
            {
                // Verison 0 is the XML before it was verisoned from 1.0b.
                // Add the version and namespace declaration so it will match verison 1.
                this.document.Root.Add(new XAttribute("version", 1));

                foreach (XElement element in this.document.Descendants())
                {
                    element.Name = GetName(element.Name.LocalName);
                }

                version = 1;
                this.document.Save(XmlFile);
            }

            // Check that the XML is up to date.
            if (version < XmlVersion)
            {
                // Ensure that the file is valid for it's version
                this.ValidateXml(string.Format(XsdFile, version));

                if (version < 2)
                {
                    UpdateToVersion2(root);
                }

                if (version < 3)
                {
                    UpdateToVersion3(root);
                }

                if (version < 4)
                {
                    UpdateToVersion4(root);
                }

                if(version < 5)
                {
                    UpdateToVersion5(root);
                }

                XAttribute versionAttribute = root.Attribute("version");
                if (versionAttribute != null)
                {
                    versionAttribute.Value = XmlVersion.ToString(CultureInfo.InvariantCulture);
                }

                SanitizeXml(this.document.Root);
                this.document.Save(XmlFile);
            }

            this.ValidateXml(string.Format(XsdFile, XmlVersion));
        }

        private void SanitizeXml(XElement root)
        {
            foreach (var node in root.Descendants())
            {
                // If we have an empty namespace...
                if (node.Name.NamespaceName == "")
                {
                    // Remove the xmlns='' attribute. Note the use of
                    // Attributes rather than Attribute, in case the
                    // attribute doesn't exist (which it might not if we'd
                    // created the document "manually" instead of loading
                    // it from a file.)
                    node.Attributes("xmlns").Remove();
                    // Inherit the parent namespace instead
                    node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                }
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
                        new XDeclaration("1.0", "utf-8", "yes"), 
                        new XElement(
                            GetName("TVSorter"), 
                            new XAttribute("version", XmlVersion), 
                            new Settings().ToXml(), 
                            new MissingEpisodeSettings().ToXml(), 
                            new XElement(GetName("Shows"))));

                    doc.Save(XmlFile);
                }

                this.GetDocument();
            }
            catch (Exception e)
            {
                throw new IOException("Unable to load XML file.", e);
            }
        }

        /// <summary>
        /// Fires the SettingsSaved event.
        /// </summary>
        private void OnSettingsSaved()
        {
            if (this.SettingsSaved != null)
            {
                this.SettingsSaved(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Fires a TVShow added event.
        /// </summary>
        /// <param name="show">
        /// The show that was added.
        /// </param>
        private void OnTvShowAdded(TvShow show)
        {
            if (this.TvShowAdded != null)
            {
                this.TvShowAdded(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        /// Fires a TvShowChanged event.
        /// </summary>
        /// <param name="show">
        /// The show that changed.
        /// </param>
        private void OnTvShowChanged(TvShow show)
        {
            if (this.TvShowChanged != null)
            {
                this.TvShowChanged(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        /// Fires a TvShowRemoved event.
        /// </summary>
        /// <param name="show">
        /// The show that was removed.
        /// </param>
        private void OnTvShowRemoved(TvShow show)
        {
            if (this.TvShowRemoved != null)
            {
                this.TvShowRemoved(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        /// Validates the XML file against the specified schema.
        /// </summary>
        /// <param name="schema">
        /// The schama to validate against.
        /// </param>
        private void ValidateXml(string schema)
        {
            if (File.Exists(schema))
            {
                // Get the schemas to validate against.
                var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                settings.Schemas.Add(XmlNamespace.NamespaceName, schema);
                settings.ValidationEventHandler +=
                    (sender, args) => { throw new XmlSchemaValidationException(args.Message, args.Exception); };

                using (XmlReader reader = XmlReader.Create(XmlFile, settings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
        }

        #endregion
    }
}