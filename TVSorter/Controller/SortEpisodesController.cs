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

    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Forms;

    using TVSorter.Files;
    using TVSorter.Scanning;
    using TVSorter.Storage;
    using TVSorter.Types;
    using TVSorter.View;

    using ProgressChangedEventArgs = TVSorter.ProgressChangedEventArgs;
    using Settings = TVSorter.Types.Settings;

    #endregion

    /// <summary>
    /// The controller for the sort episodes tab.
    /// </summary>
    public class SortEpisodesController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   The file manager.
        /// </summary>
        private IFileManager fileManager;

        /// <summary>
        ///   The last subdirectory scanned.
        /// </summary>
        private string lastSubdirectoryScanned;

        /// <summary>
        ///   The results.
        /// </summary>
        private List<FileResult> results;

        /// <summary>
        ///   The scanner.
        /// </summary>
        private IScanManager scanner;

        /// <summary>
        ///   The settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///   The sort view.
        /// </summary>
        private IView sortView;

        /// <summary>
        ///   The storage.
        /// </summary>
        private IStorageProvider storage;

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
                return this.results;
            }

            private set
            {
                this.results = value;
                this.OnPropertyChanged("Results");
            }
        }

        /// <summary>
        ///   Gets Settings.
        /// </summary>
        public Settings Settings
        {
            get
            {
                return this.settings;
            }

            private set
            {
                this.settings = value;
                this.OnPropertyChanged("Settings");
            }
        }

        /// <summary>
        ///   Gets Shows.
        /// </summary>
        public List<TvShow> Shows
        {
            get
            {
                return this.storage.LoadTvShows();
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
        /// <param name="indices">
        /// The selected indices. 
        /// </param>
        public void CopyEpisodes(IEnumerable<int> indices)
        {
            List<FileResult> checkedResults = indices.Select(index => this.Results[index]).ToList();
            this.MaxValue = checkedResults.Count;
            this.Value = 0;
            this.fileManager.CopyFileAsync(checkedResults, this);
            this.sortView.StartTaskProgress(this, "Copying Episodes");
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

            this.sortView = view;

            this.storage = Factory.StorageProvider;
            this.scanner = Factory.Scanner;
            this.fileManager = Factory.FileManager;

            this.scanner.RefreshComplete += this.OnRefreshComplete;
            this.scanner.ProgressChanged += this.OnProgressChanged;
            this.storage.SettingsChanged += (sender, args) => this.LoadSettings();

            this.fileManager.CopyFileComplete += this.OnFileOperationComplete;
            this.fileManager.MoveFileComplete += this.OnFileOperationComplete;
            this.fileManager.ProgressChanged += this.OnProgressChanged;

            this.LoadSettings();
        }

        /// <summary>
        /// Moves the episodes at the specified indices.
        /// </summary>
        /// <param name="indices">
        /// The selected indices. 
        /// </param>
        public void MoveEpisodes(IEnumerable<int> indices)
        {
            List<FileResult> checkedResults = indices.Select(index => this.Results[index]).ToList();
            this.MaxValue = checkedResults.Count;
            this.Value = 0;
            this.fileManager.MoveFileAsync(checkedResults, this);
            this.sortView.StartTaskProgress(this, "Moving Episodes");
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
            this.MaxValue = 1;
            this.Value = 1;
            this.lastSubdirectoryScanned = subdirectory;
            this.scanner.RefreshAsync(subdirectory, this);
            this.sortView.StartTaskProgress(this, "Scanning episodes");
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
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                if (result.Episode == null)
                {
                    continue;
                }

                this.scanner.ResetEpsiode(result, result.Episode.SeasonNumber, episodeNum);
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
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                if (result.Episode == null)
                {
                    continue;
                }

                this.scanner.ResetEpsiode(result, seasonNum, result.Episode.EpisodeNumber);
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
            IEnumerable<FileResult> checkedResults = indices.Select(index => this.Results[index]);
            foreach (FileResult result in checkedResults)
            {
                result.Show = show;
                this.scanner.ResetEpisode(result);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            this.Settings = this.storage.LoadSettings();
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
                var source = new DirectoryInfo(this.settings.SourceDirectory);
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

        /// <summary>
        /// Handles a file operation completing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnFileOperationComplete(object sender, FileOperationEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.OnTaskComplete();

            if (!e.Successful)
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        /// <summary>
        /// Handles the process changed event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.MaxValue = e.MaxValue;
            this.Value = e.Value;
            this.OnProgressChanged();
        }

        /// <summary>
        /// Handles a scanner refresh complete event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnRefreshComplete(object sender, RefreshCompleteEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.Results = e.Results;
            this.OnTaskComplete();
        }

        #endregion
    }
}