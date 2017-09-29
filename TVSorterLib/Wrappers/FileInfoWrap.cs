// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileInfoWrap.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.IO;

namespace TVSorter.Wrappers
{
    /// <summary>
    ///     Wrapper for the FileInfo class.
    /// </summary>
    public class FileInfoWrap : IFileInfo
    {
        /// <summary>
        ///     The file info object that is being wrapped.
        /// </summary>
        private readonly FileInfo fileInfo;

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileInfoWrap" /> class.
        /// </summary>
        /// <param name="fileName">
        ///     The file name.
        /// </param>
        public FileInfoWrap(string fileName)
        {
            fileInfo = new FileInfo(fileName);
        }

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileInfoWrap" /> class.
        /// </summary>
        /// <param name="fileInfo">
        ///     The file info.
        /// </param>
        public FileInfoWrap(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        /// <summary>
        ///     Gets the directory for this file.
        /// </summary>
        public IDirectoryInfo Directory => new DirectoryInfoWrap(fileInfo.Directory);

        /// <summary>
        ///     Gets the directory name for this file.
        /// </summary>
        public string DirectoryName => fileInfo.DirectoryName;

        /// <summary>
        ///     Gets a value indicating whether the file exists.
        /// </summary>
        public bool Exists => fileInfo.Exists;

        /// <summary>
        ///     Gets the file's extension.
        /// </summary>
        public string Extension => fileInfo.Extension;

        /// <summary>
        ///     Gets the full name of the file.
        /// </summary>
        public string FullName => fileInfo.FullName;

        /// <summary>
        ///     Gets the name of the file.
        /// </summary>
        public string Name => fileInfo.Name;

        /// <summary>
        ///     Copies the file to the specified location.
        /// </summary>
        /// <param name="destFileName">
        ///     The destination to copy to.
        /// </param>
        /// <returns>
        ///     The file info for the destination.
        /// </returns>
        public IFileInfo CopyTo(string destFileName)
        {
            return new FileInfoWrap(fileInfo.CopyTo(destFileName));
        }

        /// <summary>
        ///     Deletes the file.
        /// </summary>
        public void Delete()
        {
            fileInfo.Delete();
        }

        /// <summary>
        ///     Moves the file to the specified location.
        /// </summary>
        /// <param name="destFileName">
        ///     The destination to move to.
        /// </param>
        public void MoveTo(string destFileName)
        {
            fileInfo.MoveTo(destFileName);
        }

        /// <summary>
        ///     Writes all the specified text into the file, creating it if it doesn't exist.
        /// </summary>
        /// <param name="text">
        ///     The text to write.
        /// </param>
        public void WriteAllText(string text)
        {
            File.WriteAllText(fileInfo.FullName, text);
        }

        /// <summary>
        ///     Converts a FileInfo array into an IFileInfo array.
        /// </summary>
        /// <param name="fileInfos">
        ///     The FileInfo array to convert.
        /// </param>
        /// <returns>
        ///     The IFileInfo array.
        /// </returns>
        internal static IFileInfo[] ConvertFileInfoToIFileInfoArray(FileInfo[] fileInfos)
        {
            var array = new IFileInfo[fileInfos.Length];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = new FileInfoWrap(fileInfos[i]);
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
            return fileInfo.ToString();
        }
    }
}
