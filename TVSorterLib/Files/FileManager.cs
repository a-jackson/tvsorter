// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Files
{
    #region Using Directives

    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    using TVSorter.Storage;
    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// The file manager.
    /// </summary>
    internal class FileManager
    {
        #region Fields

        /// <summary>
        /// The storage provider.
        /// </summary>
        private readonly IStorageProvider provider;

        /// <summary>
        ///   The settings.
        /// </summary>
        private readonly Settings settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class. Initialises a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        internal FileManager(IStorageProvider provider)
        {
            this.provider = provider;
            this.settings = Settings.LoadSettings(provider);
        }

        #endregion

        #region Enums

        /// <summary>
        /// The types of sorting operations possible.
        /// </summary>
        internal enum SortType
        {
            /// <summary>
            ///   Indicates a move operation.
            /// </summary>
            Move, 

            /// <summary>
            ///   Indicates a copy operation.
            /// </summary>
            Copy
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Performs a copy file operation.
        /// </summary>
        /// <param name="files">
        /// The files to copy. 
        /// </param>
        public void CopyFile(IEnumerable<FileResult> files)
        {
            string destination = this.settings.DestinationDirectory;
            this.ProcessFiles(files, SortType.Copy, new DirectoryInfoWrap(destination));
        }

        /// <summary>
        /// Performs a move file operation on the specified files.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        public void MoveFile(IEnumerable<FileResult> files)
        {
            string destination = this.settings.DestinationDirectory;
            this.ProcessFiles(files, SortType.Move, new DirectoryInfoWrap(destination));
        }

        #endregion

        #region Methods

        /// <summary>
        /// Processes the specified list of files.
        /// </summary>
        /// <param name="files">
        /// The list of files to process. 
        /// </param>
        /// <param name="type">
        /// The operation to perform on the files. 
        /// </param>
        /// <param name="destination">
        /// The destination to process the files into.
        /// </param>
        internal void ProcessFiles(IEnumerable<FileResult> files, SortType type, IDirectoryInfo destination)
        {
            foreach (FileResult file in files)
            {
                this.ProcessFile(type, file, destination);
            }
        }

        /// <summary>
        /// Deletes the subdirectories of the file if they are empty.
        /// </summary>
        /// <param name="file">
        /// The file to check.
        /// </param>
        private void DeleteEmptySubdirectories(FileResult file)
        {
            // If no files exist in the directory
            if (file.InputFile.Directory != null && !file.InputFile.Directory.GetFiles().Any()
                && !file.InputFile.Directory.GetDirectories().Any())
            {
                // If this isn't the source directory
                if (
                    !file.InputFile.Directory.FullName.TrimEnd(Path.DirectorySeparatorChar).Equals(
                        this.settings.SourceDirectory.TrimEnd(Path.DirectorySeparatorChar)))
                {
                    file.InputFile.Directory.Delete(true);
                    Logger.OnLogMessage(this, "Delete directory: {0}", file.InputFile.DirectoryName);
                }
            }
        }

        /// <summary>
        /// Processes a single file.
        /// </summary>
        /// <param name="type">
        /// The sort type to use.
        /// </param>
        /// <param name="file">
        /// The file to process.
        /// </param>
        /// <param name="destination">
        /// The destination to process the files to.
        /// </param>
        private void ProcessFile(SortType type, FileResult file, IDirectoryInfo destination)
        {
            if (file.Incomplete)
            {
                Logger.OnLogMessage(this, "Skipping {0}. Not enough information.", file.InputFile.Name);
                return;
            }

            IFileInfo destinationInfo = file.GetFullPath(destination);
            if (destinationInfo.Directory != null && !destinationInfo.Directory.Exists)
            {
                destinationInfo.Directory.Create();
            }

            // If the file name contains any overwrite keywords and the destination already exists.
            if (file.ContainsKeyword(this.settings.OverwriteKeywords) && destinationInfo.Exists)
            {
                destinationInfo.Delete();
                file.Episode.FileCount--;
            }
            else if (destinationInfo.Exists)
            {
                Logger.OnLogMessage(this, "Skipping {0}. Already exists.", destinationInfo.Name);
                return;
            }

            switch (type)
            {
                case SortType.Move:
                    file.InputFile.MoveTo(destinationInfo.FullName);
                    Logger.OnLogMessage(this, "Moved {0}", file.InputFile.Name);
                    if (this.settings.DeleteEmptySubdirectories)
                    {
                        this.DeleteEmptySubdirectories(file);
                    }

                    break;
                case SortType.Copy:
                    file.InputFile.CopyTo(destinationInfo.FullName);
                    Logger.OnLogMessage(this, "Copied {0}", file.InputFile.Name);
                    break;
            }

            file.Episode.FileCount++;
            file.Episode.Save(this.provider);
        }

        #endregion
    }
}