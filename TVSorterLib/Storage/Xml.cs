// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Xml.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Class that manages access to the XML file.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TVSorter.Model;

namespace TVSorter.Storage
{
    /// <summary>
    ///     Class that manages access to the XML file.
    /// </summary>
    public class Xml : IStorageProvider
    {
        /// <summary>
        ///     The path the XML file.
        /// </summary>
        private const string XmlFile = "TVSorter.xml";

        /// <summary>
        ///     The file path of the XSD file. Contains the version number of the XML file.
        /// </summary>
        private const string XsdFile = "TVSorter-{0}.0.xsd";

        /// <summary>
        ///     The namespace of the XML.
        /// </summary>
        private static readonly XNamespace XmlNamespace = "http://code.google.com/p/tvsorter";

        /// <summary>
        ///     The current version of the XML file.
        /// </summary>
        private static readonly int XmlVersion = 5;

        /// <summary>
        ///     The XML Document.
        /// </summary>
        private XDocument document;

        /// <summary>
        ///     The missing episode settings.
        /// </summary>
        private MissingEpisodeSettings missingEpisodeSettings;

        /// <summary>
        ///     The settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///     Initialises a new instance of the <see cref="Xml" /> class. Initialises a new instance of the TvShowXml class.
        /// </summary>
        public Xml()
        {
            Initialise();
        }

        /// <summary>
        ///     Occurs when settings are saved.
        /// </summary>
        public event EventHandler SettingsSaved;

        /// <summary>
        ///     Occurs when a TV Show is added.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        ///     Occurs when a TV Show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        ///     Occurs when a TV Show is removed.
        /// </summary>
        public event EventHandler<TvShowEventArgs> TvShowRemoved;

        /// <summary>
        ///     Gets the settings.
        /// </summary>
        public Settings Settings => settings;

        /// <inheritdoc />
        /// <summary>
        ///     Gets the missing episode settings.
        /// </summary>
        public MissingEpisodeSettings MissingEpisodeSettings => missingEpisodeSettings;

        /// <summary>
        ///     Gets the XName for a name.
        /// </summary>
        /// <param name="name">
        ///     The name to get.
        /// </param>
        /// <returns>
        ///     The XName.
        /// </returns>
        public static XName GetName(string name)
        {
            return XmlNamespace + name;
        }

        /// <summary>
        ///     Gets the episodes that have more than 1 file grouped by show.
        /// </summary>
        /// <returns>
        ///     The collection of episodes that are duplicated.
        /// </returns>
        public IEnumerable<Episode> GetDuplicateEpisodes()
        {
            return GetEpisodes(x => x > 1);
        }

        /// <summary>
        ///     Gets the episodes that are missing grouped by show.
        /// </summary>
        /// <returns>
        ///     The collection of episodes that are missing.
        /// </returns>
        public IEnumerable<Episode> GetMissingEpisodes()
        {
            return GetEpisodes(x => x == 0);
        }

        /// <summary>
        ///     Loads the missing episode settings from the XML file.
        /// </summary>
        /// <returns>
        ///     The settings that have been loaded.
        /// </returns>
        public MissingEpisodeSettings LoadMissingEpisodeSettings()
        {
            if (document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = document.Root.Element(GetName("MissingEpisodeSettings"));
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            missingEpisodeSettings.FromXml(settingsNode);
            return missingEpisodeSettings;
        }

        /// <summary>
        ///     Reads the settings from the XML file.
        /// </summary>
        /// <returns>
        ///     The settings that have been loaded.
        /// </returns>
        public Settings LoadSettings()
        {
            if (document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = document.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            settings.FromXml(settingsNode);
            return settings;
        }

        /// <summary>
        ///     Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        ///     The list of TV shows.
        /// </returns>
        public IEnumerable<TvShow> LoadTvShows()
        {
            try
            {
                return document.Descendants(GetName("Show")).Select(NewTvShow).OrderBy(x => x.Name);
            }
            catch
            {
                return new List<TvShow>();
            }
        }

        /// <summary>
        ///     Removes the specified show from the storage.
        /// </summary>
        /// <param name="show">
        ///     The show to remove.
        /// </param>
        public void RemoveShow(TvShow show)
        {
            var showElement = document.Descendants(GetName("Show"))
                .Where(
                    x =>
                    {
                        var tvdbId = x.Attribute("tvdbid");
                        return (tvdbId != null) && tvdbId.Value.Equals(show.TvdbId.ToString());
                    })
                .FirstOrDefault();

            if (showElement != null)
            {
                showElement.Remove();
                OnTvShowRemoved(show);
            }

            document.Save(XmlFile);
        }

        /// <summary>
        ///     Saves the specified episode.
        /// </summary>
        /// <param name="episode">
        ///     The episode to save.
        /// </param>
        public void SaveEpisode(Episode episode)
        {
            if (episode == null)
            {
                throw new ArgumentNullException(nameof(episode), "Episode cannot be null.");
            }

            if (episode.Show == null)
            {
                throw new NullReferenceException("The episode's show parameter cannot be null.");
            }

            var show = document.Descendants(GetName("Show"))
                .FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.Show.TvdbId.ToString());

            if (show == null)
            {
                throw new InvalidOperationException("The specified show is not saved.");
            }

            var episodes = show.Element(GetName("Episodes"));

            if (episodes == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var episodeElement = episodes.Elements(GetName("Episode"))
                .FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.TvdbId);

            if (episodeElement != null)
            {
                episodeElement.ReplaceWith(episode.ToXml());
            }
            else
            {
                episodes.Add(episode.ToXml());
            }

            document.Save(XmlFile);
        }

        /// <summary>
        ///     Saves the missing episode settings into the XML file.
        /// </summary>
        public void SaveMissingEpisodeSettings()
        {
            if (document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = document.Root.Element(GetName("MissingEpisodeSettings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(missingEpisodeSettings.ToXml());
            document.Save(XmlFile);
        }

        /// <summary>
        ///     Saves the specified settings into the XML file.
        /// </summary>
        public void SaveSettings()
        {
            if (document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = document.Root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(settings.ToXml());
            document.Save(XmlFile);
            OnSettingsSaved();
        }

        /// <summary>
        ///     Saves the specified show. Updates if it already exists and adds if it doesn't
        /// </summary>
        /// <param name="show">
        ///     The show to save.
        /// </param>
        public void SaveShow(TvShow show)
        {
            var showElement = document.Descendants(GetName("Show"))
                .FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId.ToString());

            if (showElement != null)
            {
                showElement.ReplaceWith(show.ToXml());
                OnTvShowChanged(show);
            }
            else
            {
                var shows = document.Descendants(GetName("Shows")).FirstOrDefault();
                if (shows == null)
                {
                    throw new XmlException("XML is invalid.");
                }

                shows.Add(show.ToXml());
                OnTvShowAdded(show);
            }

            document.Save(XmlFile);
        }

        /// <summary>
        ///     Saves a collection of shows.
        /// </summary>
        /// <param name="shows">
        ///     The shows to save.
        /// </param>
        public void SaveShows(IEnumerable<TvShow> shows)
        {
            var showsElement = document.Descendants(GetName("Shows")).FirstOrDefault();
            if (showsElement == null)
            {
                throw new XmlException("XML is invalid.");
            }

            foreach (var show in shows)
            {
                // Check if the show already exists. Else add it.
                var showElement = document.Descendants(GetName("Show"))
                    .FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId.ToString());

                if (showElement != null)
                {
                    showElement.ReplaceWith(show.ToXml());
                    OnTvShowChanged(show);
                }
                else
                {
                    showsElement.Add(show.ToXml());
                    OnTvShowAdded(show);
                }
            }

            document.Save(XmlFile);
        }

        /// <summary>
        ///     Gets a new TVShow from the specified element.
        /// </summary>
        /// <param name="element">
        ///     The element to load.
        /// </param>
        /// <returns>
        ///     The new TV Show.
        /// </returns>
        private static TvShow NewTvShow(XElement element)
        {
            var show = new TvShow();
            show.FromXml(element);
            return show;
        }

        /// <summary>
        ///     Updates the specified element to XML format version 2.
        /// </summary>
        /// <param name="root">
        ///     The root element of the document.
        /// </param>
        private static void UpdateToVersion2(XElement root)
        {
            var settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid.");
            }

            // Update the first regualar expression to use the one with dual episode support
            var regularExpression = settingsNode.Element(GetName("RegularExpression"));
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            var regex = regularExpression.Element(GetName("RegEx"));

            // If it is the old text then replace with the new text
            if ((regex != null) && regex.Value.Equals("s(?<S>[0-9]+)e(?<E>[0-9]+)"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+";
            }

            // Add the new missing episode settings node.
            settingsNode.AddAfterSelf(new MissingEpisodeSettings().ToXml());
        }

        /// <summary>
        ///     Updates the XML to version 3.
        /// </summary>
        /// <param name="root">
        ///     The root of the document.
        /// </param>
        private static void UpdateToVersion3(XElement root)
        {
            var settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid");
            }

            settingsNode.Add(new XAttribute("addunmatchedshows", false));
            settingsNode.Add(new XAttribute("unlockmatchedshows", false));
            settingsNode.Add(new XAttribute("lockshowsnonewepisodes", false));
        }

        /// <summary>
        ///     Updates the XML to version 4
        /// </summary>
        /// <param name="root">The root of the document.</param>
        private static void UpdateToVersion4(XElement root)
        {
            var settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            var selectedDestination = string.Empty;
            var destinations = settingsNode.Descendants(GetName("Destination"));
            foreach (var destination in destinations)
            {
                if (bool.Parse(destination.GetAttribute("selected", "false")))
                {
                    selectedDestination = destination.Value;
                }

                destination.RemoveAttributes();
            }

            settingsNode.Add(new XAttribute("defaultdestinationdir", selectedDestination));

            // Update the first regualar expression
            var regularExpression = settingsNode.Element(GetName("RegularExpression"));
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            var regex = regularExpression.Element(GetName("RegEx"));

            // If it is the old text then replace with the new text
            if ((regex != null) && regex.Value.Equals("s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)[e-]{0,1})+";
            }

            foreach (var show in root.Descendants(GetName("Show")))
            {
                show.Add(new XAttribute("usecustomdestination", false));
                show.Add(new XAttribute("customdestinationdir", string.Empty));
            }
        }

        /// <summary>
        ///     Updates the xml to version 5
        /// </summary>
        /// <param name="root">
        ///     The root element of the document.
        /// </param>
        private static void UpdateToVersion5(XElement root)
        {
            var settingsNode = root.Element(GetName("Settings"));
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            settingsNode.FirstNode.AddAfterSelf(new XElement("IgnoredDirectories", new object[] { }));
        }

        /// <summary>
        ///     Get the XDocument instance of the XML file.
        /// </summary>
        private void GetDocument()
        {
            document = XDocument.Load(XmlFile);
            if (document.Root == null)
            {
                throw new XmlSchemaValidationException("The XML file is invalid.");
            }

            var root = document.Root;

            var version = int.Parse(document.Root.GetAttribute("version", "0"));
            if (version == 0)
            {
                // Verison 0 is the XML before it was verisoned from 1.0b.
                // Add the version and namespace declaration so it will match verison 1.
                document.Root.Add(new XAttribute("version", 1));

                foreach (var element in document.Descendants())
                {
                    element.Name = GetName(element.Name.LocalName);
                }

                version = 1;
                document.Save(XmlFile);
            }

            // Check that the XML is up to date.
            if (version < XmlVersion)
            {
                // Ensure that the file is valid for it's version
                ValidateXml(string.Format(XsdFile, version));

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

                if (version < 5)
                {
                    UpdateToVersion5(root);
                }

                var versionAttribute = root.Attribute("version");
                if (versionAttribute != null)
                {
                    versionAttribute.Value = XmlVersion.ToString(CultureInfo.InvariantCulture);
                }

                SanitizeXml(document.Root);
                document.Save(XmlFile);
            }

            ValidateXml(string.Format(XsdFile, XmlVersion));
        }

        /// <summary>
        ///     Sanitizes the XML file of empty namespaces
        /// </summary>
        /// <param name="root">
        ///     The root element to sanitize.
        /// </param>
        private void SanitizeXml(XElement root)
        {
            // If we have an empty namespace...
            foreach (var node in root.Descendants())
            {
                if (node.Name.NamespaceName == string.Empty)
                {
                    // Remove the xmlns='' attribute. Note the use of
                    // Attributes rather than Attribute, in case the
                    // attribute doesn't exist (which it might not if we'd
                    // created the document "manually" instead of loading
                    // it from a file.)
                    node.Attributes("xmlns").Remove();

                    // Inherit the parent namespace instead
                    if (node.Parent != null)
                    {
                        node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                    }
                }
            }
        }

        /// <summary>
        ///     Gets the episodes based on the specified function of file count.
        /// </summary>
        /// <param name="fileCountSelector">
        ///     The function to determine if the episode should be selected based on file count.
        /// </param>
        /// <returns>
        ///     The collection of episodes that match the predicate.
        /// </returns>
        private IEnumerable<Episode> GetEpisodes(Func<int, bool> fileCountSelector)
        {
            var tvshows = LoadTvShows();
            return tvshows.SelectMany(x => x.Episodes).Where(x => fileCountSelector(x.FileCount));
        }

        /// <summary>
        ///     Initialises the class for reading the XML file.
        /// </summary>
        private void Initialise()
        {
            try
            {
                settings = new Settings();
                missingEpisodeSettings = new MissingEpisodeSettings();

                // Check that the file exists
                if (!File.Exists(XmlFile))
                {
                    // Create the file and populate with default settings.
                    var root = new XElement(
                        GetName("TVSorter"),
                        new XAttribute("version", XmlVersion),
                        settings.ToXml(),
                        missingEpisodeSettings.ToXml(),
                        new XElement(GetName("Shows")));

                    var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"), root);

                    doc.Save(XmlFile);
                }

                GetDocument();
                LoadSettings();
                LoadMissingEpisodeSettings();
            }
            catch (Exception e)
            {
                throw new IOException("Unable to load XML file.", e);
            }
        }

        /// <summary>
        ///     Fires the SettingsSaved event.
        /// </summary>
        private void OnSettingsSaved()
        {
            if (SettingsSaved != null)
            {
                SettingsSaved(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Fires a TVShow added event.
        /// </summary>
        /// <param name="show">
        ///     The show that was added.
        /// </param>
        private void OnTvShowAdded(TvShow show)
        {
            if (TvShowAdded != null)
            {
                TvShowAdded(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        ///     Fires a TvShowChanged event.
        /// </summary>
        /// <param name="show">
        ///     The show that changed.
        /// </param>
        private void OnTvShowChanged(TvShow show)
        {
            if (TvShowChanged != null)
            {
                TvShowChanged(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        ///     Fires a TvShowRemoved event.
        /// </summary>
        /// <param name="show">
        ///     The show that was removed.
        /// </param>
        private void OnTvShowRemoved(TvShow show)
        {
            if (TvShowRemoved != null)
            {
                TvShowRemoved(this, new TvShowEventArgs(show));
            }
        }

        /// <summary>
        ///     Validates the XML file against the specified schema.
        /// </summary>
        /// <param name="schema">
        ///     The schema to validate against.
        /// </param>
        private void ValidateXml(string schema)
        {
            if (File.Exists(schema))
            {
                // Get the schemas to validate against.
                var xmlSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
                xmlSettings.Schemas.Add(XmlNamespace.NamespaceName, schema);
                xmlSettings.ValidationEventHandler += (sender, args) =>
                {
                    throw new XmlSchemaValidationException(args.Message, args.Exception);
                };

                using (var reader = XmlReader.Create(XmlFile, xmlSettings))
                {
                    while (reader.Read())
                    {
                    }
                }
            }
        }
    }
}
