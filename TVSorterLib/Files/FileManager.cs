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

    using TVSorter.Data;
    using TVSorter.Model;
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
        /// A instance of scan manager.
        /// </summary>
        private readonly ScanManager scanManager;

        /// <summary>
        ///   The settings.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        /// The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileManager"/> class. Initialises a new instance of the <see cref="FileManager"/> class.
        /// </summary>
        /// <param name="storageProvider">
        /// The storage provider.
        /// </param>
        /// <param name="dataProvider">
        /// The data provider.
        /// </param>
        internal FileManager(IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            this.storageProvider = storageProvider;
            this.settings = Settings.LoadSettings(storageProvider);
            this.scanManager = new ScanManager(this.storageProvider, dataProvider);
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
        /// Handles the rename and overwrite of the file.
        /// </summary>
        /// <param name="file">
        /// The file being processed.
        /// </param>
        /// <param name="destination">
        /// The destination directory.
        /// </param>
        /// <param name="destinationInfo">
        /// The destination file.
        /// </param>
        /// <returns>
        /// A value indicating whether the ProcessFile operation should continue or not.
        /// </returns>
        private bool HandleRenameAndOverwrite(
            FileResult file, IDirectoryInfo destination, ref IFileInfo destinationInfo)
        {
            // If the directory didn't exist then check it for the episode.
            bool containsOverwriteKeyword = file.ContainsKeyword(this.settings.OverwriteKeywords);

            // Rename the file that is already in the destination if it exists under a different name.
            if (this.settings.RenameIfExists || containsOverwriteKeyword)
            {
                // Get the files that are already in the destination directory.
                List<FileResult> results =
                    this.scanManager.SearchDestinationFolder(destinationInfo.Directory).Where(
                        x => !x.Episodes.Where((t, i) => !file.Episodes[i].Equals(t)).Any()).ToList();

                // If the episode already exists.
                if (results.Count > 0)
                {
                    if (containsOverwriteKeyword)
                    {
                        foreach (FileResult result in results)
                        {
                            result.InputFile.Delete();
                            foreach (Episode episode in result.Episodes)
                            {
                                episode.FileCount--;
                                episode.Save(this.storageProvider);
                            }
                        }
                    }
                    else if (this.settings.RenameIfExists)
                    {
                        // Can't rename more than 1 file to the same thing.
                        // Also don't rename if the file name is already the same.
                        if (results.Count == 1 && !results[0].InputFile.Name.Equals(destinationInfo.Name))
                        {
                            results[0].InputFile.MoveTo(destinationInfo.FullName);
                            Logger.OnLogMessage(
                                this, "Renamed {0} to {1}", results[0].InputFile.Name, destinationInfo.Name);

                            return false;
                        }
                    }
                }

                // Refresh the destination info as it may have changed.
                destinationInfo = file.GetFullPath(destination, this.storageProvider);
            }

            return true;
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

            IFileInfo destinationInfo = file.GetFullPath(destination, this.storageProvider);
            if (destinationInfo.Directory != null && !destinationInfo.Directory.Exists)
            {
                destinationInfo.Directory.Create();
            }
            else
            {
                if (!this.HandleRenameAndOverwrite(file, destination, ref destinationInfo))
                {
                    return;
                }
            }

            if (destinationInfo.Exists)
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
            file.Episode.Save(this.storageProvider);
        }

        #endregion
    }
}