// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStorageProvider.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Repsents an object that can provide storage for the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TVSorter.Model;

namespace TVSorter.Storage
{
    /// <summary>
    ///     Represents an object that can provide storage for the program.
    /// </summary>
    public interface IStorageProvider
    {
        /// <summary>
        ///     Gets the settings object.
        /// </summary>
        Settings Settings { get; }

        /// <summary>
        ///     Gets the missing episode settings.
        /// </summary>
        MissingEpisodeSettings MissingEpisodeSettings { get; }

        /// <summary>
        ///     Occurs when the settings are saved.
        /// </summary>
        event EventHandler SettingsSaved;

        /// <summary>
        ///     Occurs when a TV Show is added.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        ///     Occurs when a TV Show changes.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        ///     Occurs when a TV Show is removed.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowRemoved;

        /// <summary>
        ///     Gets the episodes that have more than 1 file grouped by show.
        /// </summary>
        /// <returns>
        ///     The collection of episodes that are duplicated.
        /// </returns>
        IEnumerable<Episode> GetDuplicateEpisodes();

        /// <summary>
        ///     Gets the episodes that are missing grouped by show.
        /// </summary>
        /// <returns>
        ///     The collection of episodes that are missing.
        /// </returns>
        IEnumerable<Episode> GetMissingEpisodes();

        /// <summary>
        ///     Loads all the TVShows from the XML file.
        /// </summary>
        /// <returns>
        ///     The list of TV shows.
        /// </returns>
        IEnumerable<TvShow> LoadTvShows();

        /// <summary>
        ///     Removes the specified show from the storage.
        /// </summary>
        /// <param name="show">
        ///     The show to remove.
        /// </param>
        void RemoveShow(TvShow show);

        /// <summary>
        ///     Saves the specified episode.
        /// </summary>
        /// <param name="episode">
        ///     The episode to save.
        /// </param>
        void SaveEpisode(Episode episode);

        /// <summary>
        ///     Saves the missing episode settings into the XML file.
        /// </summary>
        void SaveMissingEpisodeSettings();

        /// <summary>
        ///     Saves the specified settings into the XML file.
        /// </summary>
        void SaveSettings();

        /// <summary>
        ///     Saves the specified show. Updates if it already exists and adds if it doesn't
        /// </summary>
        /// <param name="show">
        ///     The show to save.
        /// </param>
        void SaveShow(TvShow show);

        /// <summary>
        ///     Saves a collection of shows.
        /// </summary>
        /// <param name="shows">
        ///     The shows to save.
        /// </param>
        void SaveShows(IEnumerable<TvShow> shows);

        /// <summary>
        ///     Reads the settings from the XML file.
        /// </summary>
        /// <returns>
        ///     The settings that have been loaded.
        /// </returns>
        Settings LoadSettings();

        /// <summary>
        ///     Loads the missing episode settings from the XML file.
        /// </summary>
        /// <returns>
        ///     The settings that have been loaded.
        /// </returns>
        MissingEpisodeSettings LoadMissingEpisodeSettings();
    }
}
