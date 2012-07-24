// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSearch.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Searches the files and presents the results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
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
        /// Refreshes the file counts of the episodes.
        /// </summary>
        public static void RefreshFileCounts()
        {
            var scanManager = new ScanManager(Factory.StorageProvider, Factory.DataProvider);
            scanManager.RefreshFileCounts();
        }

        /// <summary>
        /// Copies the checked file results.
        /// </summary>
        public void Copy()
        {
            var fileManager = new FileManager(Factory.StorageProvider, Factory.DataProvider);
            fileManager.CopyFile(this.Results.Where(x => x.Checked).ToList());
        }

        /// <summary>
        /// Moves the checked file results.
        /// </summary>
        public void Move()
        {
            var fileManager = new FileManager(Factory.StorageProvider, Factory.DataProvider);
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

            var scanManager = new ScanManager(Factory.StorageProvider, Factory.DataProvider);
            this.Results = scanManager.Refresh(subDirectory);
            Logger.OnLogMessage(this, "Scan complete. Found {0} files.", LogType.Info, this.Results.Count);
        }

        /// <summary>
        /// Sets the episode of the checked results.
        /// </summary>
        /// <param name="seasonNumber">
        /// The season number.
        /// </param>
        /// <param name="episodeNumber">
        /// The episode number.
        /// </param>
        public void SetEpisode(int seasonNumber, int episodeNumber)
        {
            foreach (FileResult result in this.Results.Where(x => x.Checked && x.Show != null))
            {
                result.Episode =
                    result.Show.Episodes.FirstOrDefault(
                        x => x.SeasonNumber == seasonNumber && x.EpisodeNumber == episodeNumber);
            }
        }

        /// <summary>
        /// Sets the show of the checked results.
        /// </summary>
        /// <param name="show">
        /// The show to set them to.
        /// </param>
        public void SetShow(TvShow show)
        {
            var scanManager = new ScanManager(Factory.StorageProvider, Factory.DataProvider);
            foreach (FileResult result in this.Results.Where(x => x.Checked))
            {
                scanManager.ResetShow(result, show);
            }
        }

        #endregion
    }
}