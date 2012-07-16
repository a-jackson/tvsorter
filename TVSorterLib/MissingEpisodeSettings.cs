// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEpisodeSettings.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System.IO;
    using System.Xml.Linq;

    using TVSorter.Storage;

    /// <summary>
    /// The settings for a missing episode search.
    /// </summary>
    public class MissingEpisodeSettings
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingEpisodeSettings"/> class.
        /// </summary>
        public MissingEpisodeSettings()
        {
            this.LoadSettings();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MissingEpisodeSettings"/> class.
        /// </summary>
        /// <param name="useDefault">
        /// Indicates whether to use the default settings.
        /// </param>
        internal MissingEpisodeSettings(bool useDefault)
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
        /// Initializes a new instance of the <see cref="MissingEpisodeSettings"/> class from XML.
        /// </summary>
        /// <param name="element">
        /// The element to load from.
        /// </param>
        internal MissingEpisodeSettings(XElement element)
        {
            FromXml(element, this);
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether to hide locked shows from missing episode searches.
        /// </summary>
        public bool HideLocked { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide missing seasons from missing episode searches.
        /// </summary>
        public bool HideMissingSeasons { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide episodes that haven't aired from missing episode searches.
        /// </summary>
        public bool HideNotYetAired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide part 2 of episodes from missing episode searches.
        /// </summary>
        public bool HidePart2 { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide season 0 from missing episode searches.
        /// </summary>
        public bool HideSeason0 { get; set; }

        #endregion

        /// <summary>
        /// Saves the missing episode settings.
        /// </summary>
        public void Save()
        {
            var xml = new Xml();
            xml.SaveMissingEpisodeSettings(this);
        }

        #region Methods

        /// <summary>
        /// Loads the specified missing episode settings from XML.
        /// </summary>
        /// <param name="missingEpsiodeElement">
        /// The XML element to load from.
        /// </param>
        /// <param name="settings">
        /// The settings to load into.
        /// </param>
        internal static void FromXml(XElement missingEpsiodeElement, MissingEpisodeSettings settings)
        {
            settings.HideNotYetAired = bool.Parse(missingEpsiodeElement.GetAttribute("hidenotaired", "false"));
            settings.HideLocked = bool.Parse(missingEpsiodeElement.GetAttribute("hidelocked", "false"));
            settings.HidePart2 = bool.Parse(missingEpsiodeElement.GetAttribute("hidepart2", "false"));
            settings.HideSeason0 = bool.Parse(missingEpsiodeElement.GetAttribute("hideseason0", "false"));
            settings.HideMissingSeasons = bool.Parse(missingEpsiodeElement.GetAttribute("hidemissingseasons", "false"));
        }

        /// <summary>
        /// Converts the settings to XML.
        /// </summary>
        /// <returns>The XML for the settings.</returns>
        internal XElement ToXml()
        {
            return new XElement(
                Xml.GetName("MissingEpisodeSettings"), 
                new XAttribute("hidenotaired", this.HideNotYetAired), 
                new XAttribute("hidelocked", this.HideLocked), 
                new XAttribute("hidepart2", this.HidePart2), 
                new XAttribute("hideseason0", this.HideSeason0), 
                new XAttribute("hidemissingseasons", this.HideMissingSeasons));
        }

        /// <summary>
        /// Loads the settings from the XML file.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                var xml = new Xml();
                xml.LoadMissingEpisodeSettings(this);
            }
            catch (IOException)
            {
                // Settings file does not exist. Use default settings.
                this.SetDefault();
            }
        }

        /// <summary>
        /// Initiaises the settings to default values.
        /// </summary>
        private void SetDefault()
        {
            this.HideLocked = false;
            this.HideMissingSeasons = false;
            this.HideNotYetAired = false;
            this.HidePart2 = false;
            this.HideSeason0 = false;
        }

        #endregion
    }
}