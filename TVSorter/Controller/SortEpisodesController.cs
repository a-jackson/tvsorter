// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SortEpisodesController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the sort episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using TVSorter.View;

    using Settings = TVSorter.Settings;

    #endregion

    /// <summary>
    /// The controller for the sort episodes tab.
    /// </summary>
    public class SortEpisodesController : ControllerBase
    {
        #region Fields

        /// <summary>
        /// The file searcher.
        /// </summary>
        private FileSearch fileSearch;

        /// <summary>
        ///   The last subdirectory scanned.
        /// </summary>
        private string lastSubdirectoryScanned;

        /// <summary>
        ///   The sort view.
        /// </summary>
        private IView sortView;

        /// <summary>
        ///   The sub directories.
        /// </summary>
        private BindingList<string> subDirectories;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets Results.
        /// </summary>
        public List<FileResult> Results
        {
            get
            {
                return this.fileSearch.Results;
            }
        }

        /// <summary>
        ///   Gets Settings.
        /// </summary>
        public Settings Settings
        {
            get
            {
                return new Settings();
            }
        }

        /// <summary>
        ///   Gets Shows.
        /// </summary>
        public List<TvShow> Shows
        {
            get
            {
                return TvShow.GetTvShows().ToList();
            }
        }

        /// <summary>
        ///   Gets SubDirectories.
        /// </summary>
        public BindingList<string> SubDirectories
        {
            get
            {
                return this.subDirectories;
            }

            private set
            {
                this.subDirectories = value;
                this.OnPropertyChanged("SubDirectories");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Copies the episodes at the specified indices.
        /// </summary>
        public void CopyEpisodes()
        {
            var task = new BackgroundTask(() => this.fileSearch.Copy());
            task.Start();
            this.sortView.StartTaskProgress(task, "Copying Episodes");
        }

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.lastSubdirectoryScanned = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);
            this.fileSearch = new FileSearch();

            this.sortView = view;

            this.LoadSettings();
        }

        /// <summary>
        /// Moves the episodes at the specified indices.
        /// </summary>
        public void MoveEpisodes()
        {
            var task = new BackgroundTask(() => this.fileSearch.Move());
            task.Start();
            this.sortView.StartTaskProgress(task, "Moving Episodes");
            this.ScanEpisodes(this.lastSubdirectoryScanned);
        }

        /// <summary>
        /// Scans the specified subdirectory for settings.
        /// </summary>
        /// <param name="subdirectory">
        /// The subdirectory. 
        /// </param>
        public void ScanEpisodes(string subdirectory)
        {
            var task = new BackgroundTask(
                () =>
                    {
                        this.fileSearch.Search(subdirectory);
                        this.OnPropertyChanged("Results");
                    });
            task.Start();
            this.sortView.StartTaskProgress(task, "Scanning episodes");
        }

        /// <summary>
        /// Sets the episode number of the specified indices.
        /// </summary>
        /// <param name="indices">
        /// The indices to set. 
        /// </param>
        /// <param name="episodeNum">
        /// The episode Num. 
        /// </param>
        public void SetEpisodeNum(IEnumerable<int> indices, int episodeNum)
        {
            throw new NotImplementedException();
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                if (result.Episode == null)
                {
                    continue;
                }

                // this.scanner.ResetEpsiode(result, result.Episode.SeasonNumber, episodeNum);
            }
        }

        /// <summary>
        /// Sets the season number of the specified indices.
        /// </summary>
        /// <param name="indices">
        /// The indices. 
        /// </param>
        /// <param name="seasonNum">
        /// The season num. 
        /// </param>
        public void SetSeasonNum(IEnumerable<int> indices, int seasonNum)
        {
            throw new NotImplementedException();
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                if (result.Episode == null)
                {
                    continue;
                }

                // this.scanner.ResetEpsiode(result, seasonNum, result.Episode.EpisodeNumber);
            }
        }

        /// <summary>
        /// Sets the show of the specified indices.
        /// </summary>
        /// <param name="indices">
        /// The indices to set. 
        /// </param>
        /// <param name="show">
        /// The show to set them to. 
        /// </param>
        public void SetShow(IEnumerable<int> indices, TvShow show)
        {
            throw new NotImplementedException();
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                result.Show = show;

                // this.scanner.ResetEpisode(result);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            this.SubDirectories = this.LoadSubDirectories();
        }

        /// <summary>
        /// Loads the subdirectories of the input folder.
        /// </summary>
        /// <returns>
        /// A list of the subdirectories. 
        /// </returns>
        private BindingList<string> LoadSubDirectories()
        {
            var dirs = new BindingList<string>();

            try
            {
                var source = new DirectoryInfo(this.Settings.SourceDirectory);
                if (source.Exists)
                {
                    dirs.Add(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture));
                    foreach (DirectoryInfo subDir in source.GetDirectories())
                    {
                        dirs.Add(string.Concat(Path.DirectorySeparatorChar, subDir.Name));
                    }
                }
            }
            catch
            {
            }

            return dirs;
        }

        #endregion
    }
}