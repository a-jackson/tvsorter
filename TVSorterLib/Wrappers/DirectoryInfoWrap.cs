// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DirectoryInfoWrap.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A wrapper for the DirectoryInfo class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Wrappers
{
    using System.IO;

    /// <summary>
    /// A wrapper for the DirectoryInfo class.
    /// </summary>
    public class DirectoryInfoWrap : IDirectoryInfo
    {
        #region Fields

        /// <summary>
        /// The directory info being wrapper.
        /// </summary>
        private readonly DirectoryInfo directoryInfo;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoWrap"/> class.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public DirectoryInfoWrap(string path)
        {
            this.directoryInfo = new DirectoryInfo(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DirectoryInfoWrap"/> class.
        /// </summary>
        /// <param name="directoryInfo">
        /// The directory info.
        /// </param>
        public DirectoryInfoWrap(DirectoryInfo directoryInfo)
        {
            this.directoryInfo = directoryInfo;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets a value indicating whether the directory exists.
        /// </summary>
        public bool Exists
        {
            get
            {
                return this.directoryInfo.Exists;
            }
        }

        /// <summary>
        /// Gets the full path of the directory.
        /// </summary>
        public string FullName
        {
            get
            {
                return this.directoryInfo.FullName;
            }
        }

        /// <summary>
        /// Gets the name of the directory.
        /// </summary>
        public string Name
        {
            get
            {
                return this.directoryInfo.Name;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Creates the directory.
        /// </summary>
        public void Create()
        {
            this.directoryInfo.Create();
        }

        /// <summary>
        /// Creates a file under this directory.
        /// </summary>
        /// <param name="name">
        /// The name of the file to create.
        /// </param>
        /// <returns>
        /// The IFileInfo for the new file.
        /// </returns>
        public IFileInfo CreateFile(string name)
        {
            return new FileInfoWrap(this.FullName + Path.DirectorySeparatorChar + name);
        }

        /// <summary>
        /// Deletes the directory.
        /// </summary>
        /// <param name="recursive">
        /// A value indicating whether the deletion should be recursive.
        /// </param>
        public void Delete(bool recursive)
        {
            this.directoryInfo.Delete(recursive);
        }

        /// <summary>
        /// Gets the sub directories of this directory.
        /// </summary>
        /// <returns>The collection of sub directories.</returns>
        public IDirectoryInfo[] GetDirectories()
        {
            return ConvertDirectoryInfoToIDirectoryInfo(this.directoryInfo.GetDirectories());
        }

        /// <summary>
        /// Gets a sub file of the directory at the specified path.
        /// </summary>
        /// <param name="outputPath">
        /// The path toget the file for.
        /// </param>
        /// <returns>
        /// The IFileInfo for the file.
        /// </returns>
        public IFileInfo GetFile(string outputPath)
        {
            return new FileInfoWrap(string.Concat(this.FullName, Path.DirectorySeparatorChar, outputPath));
        }

        /// <summary>
        /// Gets the files of this directory.
        /// </summary>
        /// <returns>The collection of files.</returns>
        public IFileInfo[] GetFiles()
        {
            return FileInfoWrap.ConvertFileInfoToIFileInfoArray(this.directoryInfo.GetFiles());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts an array of DirectoryInfo into an array of IDirectoryInfo.
        /// </summary>
        /// <param name="directoryInfos">
        /// The DirectoryInfo objects to convert.
        /// </param>
        /// <returns>
        /// The array of IDirectoryInfo.
        /// </returns>
        private static IDirectoryInfo[] ConvertDirectoryInfoToIDirectoryInfo(DirectoryInfo[] directoryInfos)
        {
            var array = new IDirectoryInfo[directoryInfos.Length];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = new DirectoryInfoWrap(directoryInfos[i]);
            }

            return array;
        }

        #endregion
    }
}