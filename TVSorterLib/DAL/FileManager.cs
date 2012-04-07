// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file manager.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The file manager.
    /// </summary>
    internal class FileManager : DalBase, IFileManager
    {
        #region Constants and Fields

        /// <summary>
        ///   The storage.
        /// </summary>
        private readonly IStorageProvider storage;

        /// <summary>
        ///   The settings.
        /// </summary>
        private Settings settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="FileManager" /> class. Initialises a new instance of the <see
        ///    cref="FileManager" /> class.
        /// </summary>
        public FileManager()
        {
            this.storage = Factory.StorageProvider;
            this.storage.SettingsChanged += (sender, args) => this.settings = this.storage.LoadSettings();
            this.settings = this.storage.LoadSettings();
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when a copy file operation completes.
        /// </summary>
        public event EventHandler<FileOperationEventArgs> CopyFileComplete;

        /// <summary>
        ///   Occurs when a move file operation completes.
        /// </summary>
        public event EventHandler<FileOperationEventArgs> MoveFileComplete;

        /// <summary>
        ///   Occurs when progress changing on a running operation.
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        #endregion

        #region Enums

        /// <summary>
        /// The types of sorting operations possible.
        /// </summary>
        private enum SortType
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
        public void CopyFile(List<FileResult> files)
        {
            this.ProcessFiles(files, SortType.Copy, null);
        }

        /// <summary>
        /// Performs a copy file operation on the specified files asynchronously.
        /// </summary>
        /// <param name="files">
        /// The files to copy. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        public void CopyFileAsync(List<FileResult> files, object userState)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        try
                        {
                            this.ProcessFiles(files, SortType.Copy, userState);
                            this.OnCopyFileComplete(userState);
                        }
                        catch (Exception e)
                        {
                            this.OnCopyFileComplete(e, userState);
                        }
                    });
        }

        /// <summary>
        /// Performs a move file operation on the specified files.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        public void MoveFile(List<FileResult> files)
        {
            this.ProcessFiles(files, SortType.Move, null);
        }

        /// <summary>
        /// Performs a move file operation on the specfied files asynchronously.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        public void MoveFileAsync(List<FileResult> files, object userState)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        try
                        {
                            this.ProcessFiles(files, SortType.Move, userState);
                            this.OnMoveFileComplete(userState);
                        }
                        catch (Exception e)
                        {
                            this.OnMoveFileComplete(e, userState);
                        }
                    });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises a copy file complete event.
        /// </summary>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnCopyFileComplete(object userState)
        {
            if (this.CopyFileComplete != null)
            {
                this.CopyFileComplete(this, new FileOperationEventArgs(userState));
            }
        }

        /// <summary>
        /// Raises a copy file complete event with an error.
        /// </summary>
        /// <param name="e">
        /// The error. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnCopyFileComplete(Exception e, object userState)
        {
            if (this.CopyFileComplete != null)
            {
                this.CopyFileComplete(this, new FileOperationEventArgs(e, userState));
            }
        }

        /// <summary>
        /// Raises a move file complete event.
        /// </summary>
        /// <param name="userState">
        /// The user's state object 
        /// </param>
        private void OnMoveFileComplete(object userState)
        {
            if (this.MoveFileComplete != null)
            {
                this.MoveFileComplete(this, new FileOperationEventArgs(userState));
            }
        }

        /// <summary>
        /// Raises a move file complete event with an error.
        /// </summary>
        /// <param name="e">
        /// The error. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnMoveFileComplete(Exception e, object userState)
        {
            if (this.MoveFileComplete != null)
            {
                this.MoveFileComplete(this, new FileOperationEventArgs(e, userState));
            }
        }

        /// <summary>
        /// Raises a progress changed event.
        /// </summary>
        /// <param name="max">
        /// The maximum value of the progress. 
        /// </param>
        /// <param name="value">
        /// The current value of the progress. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnProgressChanaged(int max, int value, object userState)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, new ProgressChangedEventArgs(max, value, userState));
            }
        }

        /// <summary>
        /// Processes the specified list of files.
        /// </summary>
        /// <param name="files">
        /// The list of files to process. 
        /// </param>
        /// <param name="type">
        /// The operation to perform on the files. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void ProcessFiles(List<FileResult> files, SortType type, object userState)
        {
            int value = 0;
            int max = files.Count;
            foreach (var file in files)
            {
                var file1 = file;
                if (file1.Incomplete)
                {
                    this.OnLogMessage("Skipped {0}. - Not enough information.", file1.InputFile.Name);
                    continue;
                }

                string destination = string.Format(
                    "{0}{1}{2}", this.settings.DestinationDirectory, Path.DirectorySeparatorChar, file1.OutputPath);

                var destinationInfo = new FileInfo(destination);
                if (destinationInfo.Directory != null && !destinationInfo.Directory.Exists)
                {
                    destinationInfo.Directory.Create();
                }

                // Get the path for all the destination directories.
                var destinationFiles = from dest in this.settings.DestinationDirectories
                                       select
                                           string.Format(
                                               "{0}{1}{2}", dest, Path.DirectorySeparatorChar, file1.OutputPath);
                bool continueFile = true;

                // Check if the destination exists in any of the destinations
                foreach (var any in
                    destinationFiles.Where(File.Exists).Select(
                        dest =>
                        this.settings.OverwriteKeywords.Any(
                            keyword => file1 != null && file1.InputFile.Name.ToLower().Contains(keyword.ToLower()))))
                {
                    if (any)
                    {
                        destinationInfo.Delete();
                        break;
                    }

                    continueFile = false;
                }

                if (!continueFile)
                {
                    this.OnLogMessage("Skiped {0} - Already exists.", file1.InputFile.Name);
                    continue;
                }

                switch (type)
                {
                    case SortType.Move:
                        File.Move(file1.InputFile.FullName, destination);
                        if (this.settings.DeleteEmptySubdirectories)
                        {
                            // If no files exist in the directory
                            if (file1.InputFile.Directory != null && !file1.InputFile.Directory.GetFiles().Any())
                            {
                                // If this isn't the source directory
                                if (
                                    !file1.InputFile.Directory.FullName.TrimEnd(Path.DirectorySeparatorChar).Equals(
                                        this.settings.SourceDirectory.TrimEnd(Path.DirectorySeparatorChar)))
                                {
                                    file1.InputFile.Directory.Delete(true);
                                }
                            }
                        }

                        this.OnLogMessage("Moved {0} to {1}", file1.InputFile.Name, destinationInfo.Name);
                        break;
                    case SortType.Copy:
                        this.OnLogMessage("Copying {0} to {1}", file1.InputFile.Name, destinationInfo.Name);
                        try
                        {
                            File.Copy(file1.InputFile.FullName, destination);
                        }
                        catch
                        {
                            this.OnLogMessage("Failed to copy {0}", file1.InputFile.Name);
                        }
                        this.OnLogMessage("Copied {0} to {1}", file1.InputFile.Name, destinationInfo.Name);
                        break;
                }

                file.Episode.FileCount++;
                this.storage.UpdateEpisode(file.Episode);

                value++;
                this.OnProgressChanaged(max, value, userState);
            }
        }

        #endregion
    }
}