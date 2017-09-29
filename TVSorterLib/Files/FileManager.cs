// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using System.Linq;
using TVSorter.Data;
using TVSorter.Model;
using TVSorter.Storage;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    /// <summary>
    ///     The file manager.
    /// </summary>
    public class FileManager : IFileManager
    {
        /// <summary>
        ///     The file result manager.
        /// </summary>
        private readonly IFileResultManager fileResultManager;

        /// <summary>
        ///     A instance of scan manager.
        /// </summary>
        private readonly IScanManager scanManager;

        /// <summary>
        ///     The settings.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileManager" /> class. Initialises a new instance of the
        ///     <see cref="FileManager" /> class.
        /// </summary>
        /// <param name="storageProvider">
        ///     The storage provider.
        /// </param>
        /// <param name="dataProvider">
        ///     The data provider.
        /// </param>
        /// <param name="scanManager">
        ///     The scan manager.
        /// </param>
        /// <param name="fileResultManager">
        ///     The file result manager.
        /// </param>
        public FileManager(
            IStorageProvider storageProvider,
            IDataProvider dataProvider,
            IScanManager scanManager,
            IFileResultManager fileResultManager)
        {
            this.storageProvider = storageProvider;
            settings = storageProvider.Settings;
            this.scanManager = scanManager;
            this.fileResultManager = fileResultManager;
        }

        /// <summary>
        ///     Performs a copy file operation.
        /// </summary>
        /// <param name="files">
        ///     The files to copy.
        /// </param>
        public void CopyFile(IEnumerable<FileResult> files)
        {
            var destination = settings.DefaultDestinationDirectory;
            ProcessFiles(files, SortType.Copy, new DirectoryInfoWrap(destination));
        }

        /// <summary>
        ///     Performs a move file operation on the specified files.
        /// </summary>
        /// <param name="files">
        ///     The files to move.
        /// </param>
        public void MoveFile(IEnumerable<FileResult> files)
        {
            var destination = settings.DefaultDestinationDirectory;
            ProcessFiles(files, SortType.Move, new DirectoryInfoWrap(destination));
        }

        /// <summary>
        ///     Processes the specified list of files.
        /// </summary>
        /// <param name="files">
        ///     The list of files to process.
        /// </param>
        /// <param name="type">
        ///     The operation to perform on the files.
        /// </param>
        /// <param name="defaultDestination">
        ///     The destination to process the files into.
        /// </param>
        internal void ProcessFiles(IEnumerable<FileResult> files, SortType type, IDirectoryInfo defaultDestination)
        {
            foreach (var file in files)
            {
                if (file.Incomplete)
                {
                    Logger.OnLogMessage(
                        this,
                        "Skipping {0}. Not enough information.",
                        LogType.Error,
                        file.InputFile.Name.Truncate());
                    continue;
                }

                var showDestination = defaultDestination;
                if (file.Show.UseCustomDestination)
                {
                    showDestination = file.Show.GetCustomDestinationDirectory();
                }

                ProcessFile(type, file, showDestination);
            }
        }

        /// <summary>
        ///     Deletes the subdirectories of the file if they are empty.
        /// </summary>
        /// <param name="file">
        ///     The file to check.
        /// </param>
        private void DeleteEmptySubdirectories(FileResult file)
        {
            // If no files exist in the directory
            if (file.InputFile.Directory != null &&
                !file.InputFile.Directory.GetFiles().Any() &&
                !file.InputFile.Directory.GetDirectories().Any())
            {
                if (!file.InputFile.Directory.FullName.TrimEnd(Path.DirectorySeparatorChar)
                    .Equals(settings.SourceDirectory.TrimEnd(Path.DirectorySeparatorChar)))
                {
                    file.InputFile.Directory.Delete(true);
                    Logger.OnLogMessage(
                        this,
                        "Delete directory: {0}",
                        LogType.Info,
                        file.InputFile.DirectoryName.Truncate());
                }
            }
        }

        /// <summary>
        ///     Handles the rename and overwrite of the file.
        /// </summary>
        /// <param name="file">
        ///     The file being processed.
        /// </param>
        /// <param name="destination">
        ///     The destination directory.
        /// </param>
        /// <param name="destinationInfo">
        ///     The destination file.
        /// </param>
        /// <returns>
        ///     A value indicating whether the ProcessFile operation should continue or not.
        /// </returns>
        private bool HandleRenameAndOverwrite(
            FileResult file,
            IDirectoryInfo destination,
            ref IFileInfo destinationInfo)
        {
            // If the directory didn't exist then check it for the episode.
            var containsOverwriteKeyword = file.ContainsKeyword(settings.OverwriteKeywords);

            // Rename the file that is already in the destination if it exists under a different name.
            if (settings.RenameIfExists || containsOverwriteKeyword)
            {
                // Get the files that are already in the destination directory.
                var results = scanManager.SearchDestinationFolder(destinationInfo.Directory)
                    .Where(x => x.Episodes != null && !x.Episodes.Where((t, i) => !file.Episodes[i].Equals(t)).Any())
                    .ToList();

                // If the episode already exists.
                if (results.Count > 0)
                {
                    if (containsOverwriteKeyword)
                    {
                        foreach (var result in results)
                        {
                            result.InputFile.Delete();
                            foreach (var episode in result.Episodes)
                            {
                                episode.FileCount--;
                                episode.Save(storageProvider);
                            }
                        }
                    }
                    else if (settings.RenameIfExists &&
                             results[0].InputFile.Extension.Equals(destinationInfo.Extension))
                    {
                        // Can't rename more than 1 file to the same thing.
                        // Also don't rename if the file name is already the same.
                        var currentName = results[0].InputFile.Name;
                        var newName = destinationInfo.Name;

                        if (results.Count == 1 && !currentName.Equals(newName))
                        {
                            var originalName = results[0].InputFile.Name;
                            results[0].InputFile.MoveTo(destinationInfo.FullName);
                            Logger.OnLogMessage(
                                this,
                                "Renamed {0} to {1}",
                                LogType.Info,
                                originalName.Truncate(30),
                                destinationInfo.Name.Truncate(30));

                            return false;
                        }
                    }
                }

                // Refresh the destination info as it may have changed.
                destinationInfo = fileResultManager.GetFullPath(file, destination);
            }

            return true;
        }

        /// <summary>
        ///     Processes a single file.
        /// </summary>
        /// <param name="type">
        ///     The sort type to use.
        /// </param>
        /// <param name="file">
        ///     The file to process.
        /// </param>
        /// <param name="destination">
        ///     The destination to process the files to.
        /// </param>
        private void ProcessFile(SortType type, FileResult file, IDirectoryInfo destination)
        {
            var destinationInfo = fileResultManager.GetFullPath(file, destination);
            if (destinationInfo.Directory != null && !destinationInfo.Directory.Exists)
            {
                destinationInfo.Directory.Create();
            }
            else
            {
                if (!HandleRenameAndOverwrite(file, destination, ref destinationInfo))
                {
                    return;
                }
            }

            if (destinationInfo.Exists)
            {
                Logger.OnLogMessage(
                    this,
                    "Skipping {0}. Already exists.",
                    LogType.Info,
                    destinationInfo.Name.Truncate());
                return;
            }

            switch (type)
            {
                case SortType.Move:
                    file.InputFile.MoveTo(destinationInfo.FullName);
                    Logger.OnLogMessage(this, "Moved {0}", LogType.Info, file.InputFile.Name.Truncate());
                    if (settings.DeleteEmptySubdirectories)
                    {
                        DeleteEmptySubdirectories(file);
                    }

                    break;
                case SortType.Copy:
                    file.InputFile.CopyTo(destinationInfo.FullName);
                    Logger.OnLogMessage(this, "Copied {0}", LogType.Info, file.InputFile.Name.Truncate());
                    break;
            }

            foreach (var episode in file.Episodes)
            {
                episode.FileCount++;
                episode.Save(storageProvider);
            }
        }

        /// <summary>
        ///     The types of sorting operations possible.
        /// </summary>
        internal enum SortType
        {
            /// <summary>
            ///     Indicates a move operation.
            /// </summary>
            Move,

            /// <summary>
            ///     Indicates a copy operation.
            /// </summary>
            Copy
        }
    }
}
