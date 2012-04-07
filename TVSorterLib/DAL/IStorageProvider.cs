// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IStorageProvider.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface to the storage provider.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The public interface to the storage provider.
    /// </summary>
    public interface IStorageProvider
    {
        #region Public Events

        /// <summary>
        ///   Occurs when the settings are chanaged.
        /// </summary>
        event EventHandler SettingsChanged;

        /// <summary>
        ///   Occurs when a new show is added.
        /// </summary>
        event EventHandler<ShowEventArgs> ShowAdded;

        /// <summary>
        ///   Occurs when a show is removed.
        /// </summary>
        event EventHandler<ShowEventArgs> ShowRemoved;

        /// <summary>
        ///   Occurs when a show is updated.
        /// </summary>
        event EventHandler<ShowEventArgs> ShowUpdated;

        #endregion

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
        /// Loads the episodes for the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to get episodes for. 
        /// </param>
        /// <returns>
        /// The list of episodes for the show. 
        /// </returns>
        List<Episode> LoadEpisodes(TvShow show);

        /// <summary>
        /// Reads the settings from the XML file.
        /// </summary>
        /// <returns>
        /// The settings object that represents the settings in the file. 
        /// </returns>
        Settings LoadSettings();

        /// <summary>
        /// Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        /// The list of TV shows. 
        /// </returns>
        List<TvShow> LoadTvShows();

        /// <summary>
        /// Removes the specified show from the storage.
        /// </summary>
        /// <param name="show">
        /// The show to remove. 
        /// </param>
        void RemoveShow(TvShow show);

        /// <summary>
        /// Saves the specified collection of episodes for the specified show. Updates them if they already exist or adds if they do not.
        /// </summary>
        /// <param name="show">
        /// The show the episodes are for. 
        /// </param>
        /// <param name="episodes">
        /// The collection of episodes to save. 
        /// </param>
        /// <param name="retainFileCount">
        /// A value indicating whether the file count in the XML should be retained. 
        /// </param>
        void SaveEpisodes(TvShow show, IEnumerable<Episode> episodes, bool retainFileCount);

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
        /// Saves the specified collection of shows. Updates them if they already exist or adds if they do not.
        /// </summary>
        /// <param name="shows">
        /// The collection of shows to save. 
        /// </param>
        void SaveShow(IEnumerable<TvShow> shows);

        /// <summary>
        /// Gets the number of episodes in every season.
        /// </summary>
        /// <returns>
        /// The number of episodes in each season.
        /// </returns>
        Dictionary<TvShow, Dictionary<int, int>> SeasonEpisodeCount();

        /// <summary>
        /// Updates the episode.
        /// </summary>
        /// <param name="episode">
        /// The episode to update. 
        /// </param>
        void UpdateEpisode(Episode episode);

        #endregion
    }
}