// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IFileInfo.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   An interface for the FileInfo
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Wrappers
{
    /// <summary>
    ///     An interface for the FileInfo
    /// </summary>
    public interface IFileInfo
    {
        /// <summary>
        ///     Gets the directory for this file.
        /// </summary>
        IDirectoryInfo Directory { get; }

        /// <summary>
        ///     Gets the directory name for this file.
        /// </summary>
        string DirectoryName { get; }

        /// <summary>
        ///     Gets a value indicating whether the file exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        ///     Gets the file's extension.
        /// </summary>
        string Extension { get; }

        /// <summary>
        ///     Gets the full name of the file.
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///     Copies the file to the specified location.
        /// </summary>
        /// <param name="destFileName">
        ///     The destination to copy to.
        /// </param>
        /// <returns>
        ///     The file info for the destination.
        /// </returns>
        IFileInfo CopyTo(string destFileName);

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        void Delete();

        /// <summary>
        ///     Moves the file to the specified location.
        /// </summary>
        /// <param name="destFileName">
        ///     The destination to move to.
        /// </param>
        void MoveTo(string destFileName);

        /// <summary>
        ///     Writes all the specified text into the file, creating it if it doesn't exist.
        /// </summary>
        /// <param name="text">
        ///     The text to write.
        /// </param>
        void WriteAllText(string text);
    }
}
