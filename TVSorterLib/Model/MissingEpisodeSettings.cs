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
    /// <summary>
    ///     The settings for a missing episode search.
    /// </summary>
    public class MissingEpisodeSettings
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="MissingEpisodeSettings" /> class.
        /// </summary>
        internal MissingEpisodeSettings()
        {
            SetDefault();
        }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide locked shows from missing episode searches.
        /// </summary>
        public bool HideLocked { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide missing seasons from missing episode searches.
        /// </summary>
        public bool HideMissingSeasons { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide episodes that haven't aired from missing episode searches.
        /// </summary>
        public bool HideNotYetAired { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide part 2 of episodes from missing episode searches.
        /// </summary>
        public bool HidePart2 { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to hide season 0 from missing episode searches.
        /// </summary>
        public bool HideSeason0 { get; set; }

        /// <summary>
        ///     Initialises the settings to default values.
        /// </summary>
        private void SetDefault()
        {
            HideLocked = false;
            HideMissingSeasons = false;
            HideNotYetAired = false;
            HidePart2 = false;
            HideSeason0 = false;
        }
    }
}
