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
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using TVSorter.Model;

namespace TVSorter.Storage
{
    /// <inheritdoc />
    /// <summary>
    ///     Class that manages access to the XML file.
    /// </summary>
    public class Xml : IStorageProvider
    {
        private const string XmlFile = "TVSorter.xml";

        public static readonly XNamespace XmlNamespace = "http://code.google.com/p/tvsorter";
        private readonly IXmlMigration xmlMigration;

        private XDocument document;

        public Xml(IXmlMigration xmlMigration)
        {
            this.xmlMigration = xmlMigration;
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
        public Settings Settings { get; private set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets the missing episode settings.
        /// </summary>
        public MissingEpisodeSettings MissingEpisodeSettings { get; private set; }

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

            var settingsNode = document.Root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            Settings.FromXml(settingsNode);
            return Settings;
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
                return document.Descendants("Show".GetElementName()).Select(NewTvShow).OrderBy(x => x.Name);
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
            var showElement = document.Descendants("Show".GetElementName())
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

            var show = document.Descendants("Show".GetElementName())
                .FirstOrDefault(x => x.GetAttribute("tvdbid") == episode.Show.TvdbId.ToString());

            if (show == null)
            {
                throw new InvalidOperationException("The specified show is not saved.");
            }

            var episodes = show.Element("Episodes".GetElementName());

            if (episodes == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var episodeElement = episodes.Elements("Episode".GetElementName())
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

            var settingsNode = document.Root.Element("MissingEpisodeSettings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(MissingEpisodeSettings.ToXml());
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

            var settingsNode = document.Root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("Xml is invalid");
            }

            settingsNode.ReplaceWith(Settings.ToXml());
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
            var showElement = document.Descendants("Show".GetElementName())
                .FirstOrDefault(x => x.GetAttribute("tvdbid") == show.TvdbId.ToString());

            if (showElement != null)
            {
                showElement.ReplaceWith(show.ToXml());
                OnTvShowChanged(show);
            }
            else
            {
                var shows = document.Descendants("Shows".GetElementName()).FirstOrDefault();
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
            var showsElement = document.Descendants("Shows".GetElementName()).FirstOrDefault();
            if (showsElement == null)
            {
                throw new XmlException("XML is invalid.");
            }

            foreach (var show in shows)
            {
                // Check if the show already exists. Else add it.
                var showElement = document.Descendants("Show".GetElementName())
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
        ///     Loads the missing episode settings from the XML file.
        /// </summary>
        private void LoadMissingEpisodeSettings()
        {
            if (document.Root == null)
            {
                throw new XmlException("XML is invalid.");
            }

            var settingsNode = document.Root.Element("MissingEpisodeSettings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlSchemaException("The XML file is invalid.");
            }

            MissingEpisodeSettings.FromXml(settingsNode);
        }

        /// <summary>
        ///     Get the XDocument instance of the XML file.
        /// </summary>
        private void GetDocument()
        {
            document = XDocument.Load(XmlFile);
            xmlMigration.MigrateIfRequired(document, XmlFile);
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
                Settings = new Settings();
                MissingEpisodeSettings = new MissingEpisodeSettings();

                // Check that the file exists
                if (!File.Exists(XmlFile))
                {
                    // Create the file and populate with default settings.
                    var root = new XElement(
                        "TVSorter".GetElementName(),
                        new XAttribute("version", XmlMigration.XmlVersion),
                        Settings.ToXml(),
                        MissingEpisodeSettings.ToXml(),
                        new XElement("Shows".GetElementName()));

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
            SettingsSaved?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        ///     Fires a TVShow added event.
        /// </summary>
        /// <param name="show">
        ///     The show that was added.
        /// </param>
        private void OnTvShowAdded(TvShow show)
        {
            TvShowAdded?.Invoke(this, new TvShowEventArgs(show));
        }

        /// <summary>
        ///     Fires a TvShowChanged event.
        /// </summary>
        /// <param name="show">
        ///     The show that changed.
        /// </param>
        private void OnTvShowChanged(TvShow show)
        {
            TvShowChanged?.Invoke(this, new TvShowEventArgs(show));
        }

        /// <summary>
        ///     Fires a TvShowRemoved event.
        /// </summary>
        /// <param name="show">
        ///     The show that was removed.
        /// </param>
        private void OnTvShowRemoved(TvShow show)
        {
            TvShowRemoved?.Invoke(this, new TvShowEventArgs(show));
        }
    }
}
