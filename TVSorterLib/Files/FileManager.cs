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

    #endregion

    /// <summary>
    /// The file manager.
    /// </summary>
    internal class FileManager
    {
        #region Fields

        /// <summary>
        ///   The settings.
        /// </summary>
        private readonly Settings settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="FileManager" /> class. Initialises a new instance of the <see
        ///    cref="FileManager" /> class.
        /// </summary>
        public FileManager()
        {
            this.settings = new Settings();
        }

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
            this.ProcessFiles(files, SortType.Copy);
        }

        /// <summary>
        /// Performs a move file operation on the specified files.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        public void MoveFile(List<FileResult> files)
        {
            this.ProcessFiles(files, SortType.Move);
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
        private void ProcessFiles(IEnumerable<FileResult> files, SortType type)
        {
            foreach (FileResult file in files)
            {
                FileResult file1 = file;
                if (file1.Incomplete)
                {
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
                IEnumerable<string> destinationFiles = from dest in this.settings.DestinationDirectories
                                                       select
                                                           string.Format(
                                                               "{0}{1}{2}", 
                                                               dest, 
                                                               Path.DirectorySeparatorChar, 
                                                               file1.OutputPath);
                bool continueFile = true;

                // Check if the destination exists in any of the destinations
                foreach (bool any in
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
                    continue;
                }

                switch (type)
                {
                    case SortType.Move:
                        File.Move(file1.InputFile.FullName, destination);
                        Logger.OnLogMessage(this, "Moved {0}", file1.InputFile.Name);
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
                                    Logger.OnLogMessage(this, "Delete directory: {0}", file1.InputFile.DirectoryName);
                                }
                            }
                        }

                        break;
                    case SortType.Copy:
                        File.Copy(file1.InputFile.FullName, destination);
                        Logger.OnLogMessage(this, "Copied {0}", file1.InputFile.Name);
                        break;
                }

                file.Episode.FileCount++;
                file.Episode.Save();
            }
        }

        #endregion
    }
}