// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDirectoryInfo.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   An interface for the directory info.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Wrappers
{
    /// <summary>
    /// An interface for the directory info.
    /// </summary>
    public interface IDirectoryInfo
    {
        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// Gets the full path of the directory.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        string Name { get; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates the directory.
        /// </summary>
        void Create();

        /// <summary>
        /// Creates a file under this directory.
        /// </summary>
        /// <param name="name">
        /// The name of the file to create.
        /// </param>
        /// <returns>
        /// The IFileInfo for the new file.
        /// </returns>
        IFileInfo CreateFile(string name);

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">
        /// A value indicating whether the deletion should be recursive.
        /// </param>
        void Delete(bool recursive);

        /// <summary>
        /// Gets the sub directories of this directory.
        /// </summary>
        /// <returns>The collection of sub directories.</returns>
        IDirectoryInfo[] GetDirectories();

        /// <summary>
        /// Gets a sub file of the directory at the specified path.
        /// </summary>
        /// <param name="outputPath">
        /// The path toget the file for.
        /// </param>
        /// <returns>
        /// The IFileInfo for the file.
        /// </returns>
        IFileInfo GetFile(string outputPath);

        /// <summary>
        /// Gets the files of this directory.
        /// </summary>
        /// <returns>The collection of files.</returns>
        IFileInfo[] GetFiles();

        #endregion
    }
}