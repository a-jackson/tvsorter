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

    using TVSorter.Model;
    using TVSorter.View;

    using Settings = TVSorter.Model.Settings;

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
        /// The system settings.
        /// </summary>
        private Settings settings;

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
        /// Sets the episode of the checked results.
        /// </summary>
        /// <param name="seasonNum">
        /// The season number.
        /// </param>
        /// <param name="episodeNum">
        /// The episode number.
        /// </param>
        public void SetEpisode(int seasonNum, int episodeNum)
        {
            this.fileSearch.SetEpisode(seasonNum, episodeNum);
        }

        /// <summary>
        /// Sets the show of the specified indices.
        /// </summary>
        /// <param name="show">
        /// The show to set them to. 
        /// </param>
        public void SetShow(TvShow show)
        {
            this.fileSearch.SetShow(show);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            this.settings = Settings.LoadSettings();
            this.settings.SettingsChanged += (sender, e) => { this.SubDirectories = this.LoadSubDirectories(); };
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

        #endregion
    }
}