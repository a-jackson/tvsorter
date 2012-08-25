// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileInfoWrap.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Wrappers
{
    using System.IO;

    /// <summary>
    /// Wrapper for the FileInfo class.
    /// </summary>
    public class FileInfoWrap : IFileInfo
    {
        #region Fields

        /// <summary>
        /// The file info object that is being wrapped.
        /// </summary>
        private readonly FileInfo fileInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoWrap"/> class.
        /// </summary>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        public FileInfoWrap(string fileName)
        {
            this.fileInfo = new FileInfo(fileName);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileInfoWrap"/> class.
        /// </summary>
        /// <param name="fileInfo">
        /// The file info.
        /// </param>
        public FileInfoWrap(FileInfo fileInfo)
        {
            this.fileInfo = fileInfo;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the directory for this file.
        /// </summary>
        public IDirectoryInfo Directory
        {
            get
            {
                return new DirectoryInfoWrap(this.fileInfo.Directory);
            }
        }

        /// <summary>
        /// Gets the directory name for this file.
        /// </summary>
        public string DirectoryName
        {
            get
            {
                return this.fileInfo.DirectoryName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the file exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return this.fileInfo.Exists;
            }
        }

        /// <summary>
        /// Gets the file's extension.
        /// </summary>
        public string Extension
        {
            get
            {
                return this.fileInfo.Extension;
            }
        }

        /// <summary>
        /// Gets the full name of the file.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.fileInfo.FullName;
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        public string Name
        {
            get
            {
                return this.fileInfo.Name;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Copies the file to the specified location.
        /// </summary>
        /// <param name="destFileName">
        /// The destination to copy to.
        /// </param>
        /// <returns>
        /// The file info for the destination.
        /// </returns>
        public IFileInfo CopyTo(string destFileName)
        {
            return new FileInfoWrap(this.fileInfo.CopyTo(destFileName));
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        public void Delete()
        {
            this.fileInfo.Delete();
        }

        /// <summary>
        /// Moves the file to the specfied location.
        /// </summary>
        /// <param name="destFileName">
        /// The destination to move to.
        /// </param>
        public void MoveTo(string destFileName)
        {
            this.fileInfo.MoveTo(destFileName);
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return this.fileInfo.ToString();
        }

        /// <summary>
        /// Writes all the specified text into the file, creating it if it doesn't exist.
        /// </summary>
        /// <param name="text">
        /// The text to write.
        /// </param>
        public void WriteAllText(string text)
        {
            File.WriteAllText(this.fileInfo.FullName, text);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts a FileInfo array into an IFileInfo array.
        /// </summary>
        /// <param name="fileInfos">
        /// The FileInfo array to convert.
        /// </param>
        /// <returns>
        /// The IFileInfo array.
        /// </returns>
        internal static IFileInfo[] ConvertFileInfoToIFileInfoArray(FileInfo[] fileInfos)
        {
            var array = new IFileInfo[fileInfos.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new FileInfoWrap(fileInfos[i]);
            }

            return array;
        }

        #endregion
    }
}