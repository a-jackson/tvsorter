// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryInfoWrap.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace TVSorter.Wrappers
{
    /// <summary>
    ///     A wrapper for the DirectoryInfo class.
    /// </summary>
    public class DirectoryInfoWrap : IDirectoryInfo
    {
        /// <summary>
        ///     The directory info being wrapper.
        /// </summary>
        private readonly DirectoryInfo directoryInfo;

        /// <summary>
        ///     Initialises a new instance of the <see cref="DirectoryInfoWrap" /> class.
        /// </summary>
        /// <param name="path">
        ///     The path.
        /// </param>
        public DirectoryInfoWrap(string path)
        {
            directoryInfo = new DirectoryInfo(path);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="DirectoryInfoWrap" /> class.
        /// </summary>
        /// <param name="directoryInfo">
        ///     The directory info.
        /// </param>
        public DirectoryInfoWrap(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        /// <summary>
        ///     Gets the value of the directory attributes.
        ///     This to avoid System folders
        /// </summary>
        public FileAttributes DirectoryAttributes => directoryInfo.Attributes;

        /// <summary>
        ///     Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists => directoryInfo.Exists;

        /// <summary>
        ///     Gets the full path of the directory.
        /// </summary>
        public string FullName => directoryInfo.FullName;

        /// <summary>
        ///     Gets the name of the directory.
        /// </summary>
        public string Name => directoryInfo.Name;

        /// <summary>
        ///     Creates the directory.
        /// </summary>
        public void Create()
        {
            directoryInfo.Create();
        }

        /// <summary>
        ///     Creates a file under this directory.
        /// </summary>
        /// <param name="name">
        ///     The name of the file to create.
        /// </param>
        /// <returns>
        ///     The IFileInfo for the new file.
        /// </returns>
        public IFileInfo CreateFile(string name)
        {
            return new FileInfoWrap(FullName + Path.DirectorySeparatorChar + name);
        }

        /// <summary>
        ///     Deletes the directory.
        /// </summary>
        /// <param name="recursive">
        ///     A value indicating whether the deletion should be recursive.
        /// </param>
        public void Delete(bool recursive)
        {
            directoryInfo.Delete(recursive);
        }

        /// <summary>
        ///     Gets the sub directories of this directory.
        /// </summary>
        /// <returns>The collection of sub directories.</returns>
        public IDirectoryInfo[] GetDirectories()
        {
            return ConvertDirectoryInfoToIDirectoryInfo(directoryInfo.GetDirectories());
        }

        /// <summary>
        ///     Gets a sub file of the directory at the specified path.
        /// </summary>
        /// <param name="outputPath">
        ///     The path to get the file for.
        /// </param>
        /// <returns>
        ///     The IFileInfo for the file.
        /// </returns>
        public IFileInfo GetFile(string outputPath)
        {
            return new FileInfoWrap(string.Concat(FullName, Path.DirectorySeparatorChar, outputPath));
        }

        /// <summary>
        ///     Gets the files of this directory.
        /// </summary>
        /// <returns>The collection of files.</returns>
        public IFileInfo[] GetFiles()
        {
            return FileInfoWrap.ConvertFileInfoToIFileInfoArray(directoryInfo.GetFiles());
        }

        /// <summary>
        ///     Converts an array of DirectoryInfo into an array of IDirectoryInfo.
        /// </summary>
        /// <param name="directoryInfos">
        ///     The DirectoryInfo objects to convert.
        /// </param>
        /// <returns>
        ///     The array of IDirectoryInfo.
        /// </returns>
        private static IDirectoryInfo[] ConvertDirectoryInfoToIDirectoryInfo(DirectoryInfo[] directoryInfos)
        {
            var array = new IDirectoryInfo[directoryInfos.Length];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new DirectoryInfoWrap(directoryInfos[i]);
            }

            return array;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        ///     A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return directoryInfo.ToString();
        }
    }
}
