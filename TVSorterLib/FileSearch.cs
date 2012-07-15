// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSearch.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Searches the files and presents the results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using TVSorter.Files;

    /// <summary>
    /// Searches the files and presents the results.
    /// </summary>
    public class FileSearch
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSearch"/> class.
        /// </summary>
        public FileSearch()
        {
            this.Results = new List<FileResult>();
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the results of the search.
        /// </summary>
        public List<FileResult> Results { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Copies the checked file results.
        /// </summary>
        public void Copy()
        {
            var fileManager = new FileManager();
            fileManager.CopyFile(this.Results.Where(x => x.Checked).ToList());
        }

        /// <summary>
        /// Moves the checked file results.
        /// </summary>
        public void Move()
        {
            var fileManager = new FileManager();
            fileManager.MoveFile(this.Results.Where(x => x.Checked).ToList());
        }

        /// <summary>
        /// Performs a search.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to search.
        /// </param>
        public void Search(string subDirectory)
        {
            if (subDirectory == null)
            {
                subDirectory = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            }

            var scanManager = new ScanManager();
            this.Results = scanManager.Refresh(subDirectory);
            Logger.OnLogMessage(this, "Scan complete. Found {0} files.", this.Results.Count);
        }

        /// <summary>
        /// Updates the file counts of all the episodes.
        /// </summary>
        public void UpdateFileCounts()
        {
            var scanManager = new ScanManager();
            scanManager.RefreshFileCounts();
        }

        #endregion
    }
}