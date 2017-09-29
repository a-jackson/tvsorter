// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The interface for scan manager
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using TVSorter.Model;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    /// <summary>
    ///     The interface for scan manager
    /// </summary>
    public interface IScanManager
    {
        /// <summary>
        ///     Refresh the specified sub directory.
        /// </summary>
        /// <param name="subDirectory">
        ///     The sub directory to refresh.
        /// </param>
        /// <returns>
        ///     The list of files identified during the refresh operation.
        /// </returns>
        List<FileResult> Refresh(string subDirectory);

        /// <summary>
        ///     Searches for files in the output directory to set the file counts.
        /// </summary>
        void RefreshFileCounts();

        /// <summary>
        ///     Resets the show of the specified result.
        /// </summary>
        /// <param name="result">
        ///     The result to modify.
        /// </param>
        /// <param name="show">
        ///     The show to set the result to.
        /// </param>
        void ResetShow(FileResult result, TvShow show);

        /// <summary>
        ///     Searches a destination folder for files.
        ///     This is intended to be called by the file manager.
        /// </summary>
        /// <param name="destination">
        ///     The destination directory to search.
        /// </param>
        /// <returns>
        ///     The collection of matched files.
        /// </returns>
        IEnumerable<FileResult> SearchDestinationFolder(IDirectoryInfo destination);

        /// <summary>
        ///     Searches for new TVShows.
        /// </summary>
        /// <param name="directories">
        ///     The directories to search.
        /// </param>
        /// <returns>
        ///     The ambiguous results of the search for user selection.
        /// </returns>
        Dictionary<string, List<TvShow>> SearchNewShows(IEnumerable<IDirectoryInfo> directories);
    }
}
