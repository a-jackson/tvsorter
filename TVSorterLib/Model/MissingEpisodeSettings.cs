// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MissingEpisodeSettings.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings for a missing episode search.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
{
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
        internal MissingEpisodeSettings()
        {
            this.SetDefault();
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

        #region Public Methods and Operators

        /// <summary>
        /// Loads a new MissingEpisodeSettings objects.
        /// </summary>
        /// <returns>The new MissingEpisodeSettings object.</returns>
        public static MissingEpisodeSettings LoadSettings()
        {
            return LoadSettings(Factory.StorageProvider);
        }

        /// <summary>
        /// Saves the missing episode settings.
        /// </summary>
        public void Save()
        {
            this.Save(Factory.StorageProvider);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        /// <param name="provider">
        /// The storage provider to load the settings from.
        /// </param>
        /// <returns>
        /// The new MissingEpisodeSettings object.
        /// </returns>
        internal static MissingEpisodeSettings LoadSettings(IStorageProvider provider)
        {
            var settings = new MissingEpisodeSettings();
            provider.LoadMissingEpisodeSettings(settings);
            return settings;
        }

        /// <summary>
        /// Saves the missing episode settings.
        /// </summary>
        /// <param name="provider">
        /// The settings to save.
        /// </param>
        internal void Save(IStorageProvider provider)
        {
            provider.SaveMissingEpisodeSettings(this);
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