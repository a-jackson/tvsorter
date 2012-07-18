// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageProvider.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Repsents an object that can provide storage for the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Storage
{
    using System.Collections.Generic;

    /// <summary>
    /// Repsents an object that can provide storage for the program.
    /// </summary>
    public interface IStorageProvider
    {
        #region Public Methods and Operators

        /// <summary>
        /// Gets the episodes that have more than 1 file grouped by show.
        /// </summary>
        /// <returns>
        /// The collection of episodes that are duplicated. 
        /// </returns>
        IEnumerable<Episode> GetDuplicateEpisodes();

        /// <summary>
        /// Gets the episodes that are missing grouped by show.
        /// </summary>
        /// <returns>
        /// The collection of episodes that are missing. 
        /// </returns>
        IEnumerable<Episode> GetMissingEpisodes();

        /// <summary>
        /// Loads the missing episode settings from the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to load into.
        /// </param>
        void LoadMissingEpisodeSettings(MissingEpisodeSettings settings);

        /// <summary>
        /// Reads the settings from the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to set from the XML.
        /// </param>
        void LoadSettings(Settings settings);

        /// <summary>
        /// Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        /// The list of TV shows. 
        /// </returns>
        IEnumerable<TvShow> LoadTvShows();

        /// <summary>
        /// Removes the specified show from the storage.
        /// </summary>
        /// <param name="show">
        /// The show to remove. 
        /// </param>
        void RemoveShow(TvShow show);

        /// <summary>
        /// Saves the specified episode.
        /// </summary>
        /// <param name="episode">
        /// The episode to save.
        /// </param>
        void SaveEpisode(Episode episode);

        /// <summary>
        /// Saves the missing episode settings into the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to save.
        /// </param>
        void SaveMissingEpisodeSettings(MissingEpisodeSettings settings);

        /// <summary>
        /// Saves the specified settings into the XML file.
        /// </summary>
        /// <param name="settings">
        /// The settings to save. 
        /// </param>
        void SaveSettings(Settings settings);

        /// <summary>
        /// Saves the specified show. Updates if it already exists and adds if it doesn't
        /// </summary>
        /// <param name="show">
        /// The show to save. 
        /// </param>
        void SaveShow(TvShow show);

        /// <summary>
        /// Saves a collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to save.
        /// </param>
        void SaveShows(IEnumerable<TvShow> shows);

        #endregion
    }
}