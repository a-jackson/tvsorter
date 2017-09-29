// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IFileResultManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The interface for the file result manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TVSorter.Model;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    /// <summary>
    ///     The interface for the file result manager.
    /// </summary>
    public interface IFileResultManager
    {
        /// <summary>
        ///     Formats the output path of the episode.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to format the path for.
        /// </param>
        /// <returns>
        ///     The formatted output path.
        /// </returns>
        string FormatOutputPath(FileResult fileResult);

        /// <summary>
        ///     Formats the output path of the episode.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to format the string for.
        /// </param>
        /// <param name="formatString">
        ///     The format string to use.
        /// </param>
        /// <returns>
        ///     The formatted output path.
        /// </returns>
        string FormatOutputPath(FileResult fileResult, string formatString);

        /// <summary>
        ///     Gets the full path of the file for the specified destination.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to get the path for.
        /// </param>
        /// <param name="destination">
        ///     The destination directory.
        /// </param>
        /// <returns>
        ///     The full path of the file.
        /// </returns>
        IFileInfo GetFullPath(FileResult fileResult, IDirectoryInfo destination);
    }
}
