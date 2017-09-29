// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IFileSearch.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The interface for the file searcher.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using TVSorter.Model;

namespace TVSorter.Files
{
    /// <summary>
    ///     The interface for the file searcher.
    /// </summary>
    public interface IFileSearch
    {
        /// <summary>
        ///     Gets the result of the search.
        /// </summary>
        List<FileResult> Results { get; }

        /// <summary>
        ///     Copies the checked file results.
        /// </summary>
        void Copy();

        /// <summary>
        ///     Moves the checked file results.
        /// </summary>
        void Move();

        /// <summary>
        ///     Refreshes the file counts of the episodes.
        /// </summary>
        void RefreshFileCounts();

        /// <summary>
        ///     Performs a search.
        /// </summary>
        /// <param name="subDirectory">
        ///     The sub directory to search.
        /// </param>
        void Search(string subDirectory);

        /// <summary>
        ///     Sets the episode of the checked results.
        /// </summary>
        /// <param name="seasonNumber">
        ///     The season number.
        /// </param>
        /// <param name="episodeNumber">
        ///     The episode number.
        /// </param>
        void SetEpisode(int seasonNumber, int episodeNumber);

        /// <summary>
        ///     Sets the show of the checked results.
        /// </summary>
        /// <param name="show">
        ///     The show to set them to.
        /// </param>
        void SetShow(TvShow show);
    }
}
