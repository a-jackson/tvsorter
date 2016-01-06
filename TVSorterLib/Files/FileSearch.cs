// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSearch.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Searches the files and presents the results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Files
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using Data;
    using Model;
    using Storage;

    #endregion

    /// <summary>
    /// Searches the files and presents the results.
    /// </summary>
    public class FileSearch : IFileSearch
    {
        #region Fields

        /// <summary>
        /// The storage provider.
        /// </summary>
        private IStorageProvider storageProvider;

        /// <summary>
        /// The data provider.
        /// </summary>
        private IDataProvider dataProvider;

        /// <summary>
        /// The file manager.
        /// </summary>
        private IFileManager fileManager;

        /// <summary>
        /// The scan manager.
        /// </summary>
        private IScanManager scanManager;
        
        #endregion
        
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FileSearch"/> class.
        /// </summary>
        /// <param name="storageProvider">
        /// The storage provider.
        /// </param>
        /// <param name="dataProvider">
        /// The data provider.
        /// </param>
        /// <param name="scanManager">
        /// The scan manager.
        /// </param>
        /// <param name="fileManager">
        /// The file manager.
        /// </param>
        public FileSearch(IStorageProvider storageProvider, IDataProvider dataProvider, IScanManager scanManager, IFileManager fileManager)
        {
            this.Results = new List<FileResult>();
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;
            this.scanManager = scanManager;
            this.fileManager = fileManager;
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
        public void RefreshFileCounts()
        {
            this.scanManager.RefreshFileCounts();
        }

        /// <summary>
        /// Copies the checked file results.
        /// </summary>
        public void Copy()
        {
            this.fileManager.CopyFile(this.Results.Where(x => x.Checked).ToList());
        }

        /// <summary>
        /// Moves the checked file results.
        /// </summary>
        public void Move()
        {
            this.fileManager.MoveFile(this.Results.Where(x => x.Checked).ToList());
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

            this.Results = this.scanManager.Refresh(subDirectory);
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
                var episode =
                    result.Show.Episodes.FirstOrDefault(
                        x => x.SeasonNumber == seasonNumber && x.EpisodeNumber == episodeNumber);
                result.Episode = episode;
                result.Episodes = new List<Episode>() { episode };
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
            foreach (FileResult result in this.Results.Where(x => x.Checked))
            {
                this.scanManager.ResetShow(result, show);
            }
        }

        #endregion
    }
}