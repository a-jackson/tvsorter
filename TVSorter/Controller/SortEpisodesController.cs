// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SortEpisodesController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the sort episodes tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.Storage;
using TVSorter.View;
using Settings = TVSorter.Model.Settings;

namespace TVSorter.Controller
{
    /// <summary>
    ///     The controller for the sort episodes tab.
    /// </summary>
    public class SortEpisodesController : ControllerBase
    {
        /// <summary>
        ///     The file searcher.
        /// </summary>
        private readonly IFileSearch fileSearch;

        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     The TV show repository.
        /// </summary>
        private readonly ITvShowRepository tvShowRepository;

        /// <summary>
        ///     The last subdirectory scanned.
        /// </summary>
        private string lastSubdirectoryScanned;

        /// <summary>
        ///     The system settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///     The sort view.
        /// </summary>
        private IView sortView;

        /// <summary>
        ///     The sub directories.
        /// </summary>
        private BindingList<string> subDirectories;

        /// <summary>
        ///     Initialises a new instance of the <see cref="SortEpisodesController" /> class.
        /// </summary>
        /// <param name="tvShowRepository">The TV show repository.</param>
        /// <param name="fileSearch">The file searcher.</param>
        /// <param name="storageProvider">The storage provider.</param>
        public SortEpisodesController(
            ITvShowRepository tvShowRepository,
            IFileSearch fileSearch,
            IStorageProvider storageProvider)
        {
            this.tvShowRepository = tvShowRepository;
            this.fileSearch = fileSearch;
            this.storageProvider = storageProvider;
        }

        /// <summary>
        ///     Gets Results.
        /// </summary>
        public List<FileResult> Results => fileSearch.Results;

        /// <summary>
        ///     Gets Shows.
        /// </summary>
        public List<TvShow> Shows => tvShowRepository.GetTvShows().ToList();

        /// <summary>
        ///     Gets SubDirectories.
        /// </summary>
        public BindingList<string> SubDirectories
        {
            get => subDirectories;

            private set
            {
                subDirectories = value;
                OnPropertyChanged("SubDirectories");
            }
        }

        /// <summary>
        ///     Copies the episodes at the specified indices.
        /// </summary>
        public void CopyEpisodes()
        {
            var task = new BackgroundTask(() => fileSearch.Copy());
            task.Start();
            sortView.StartTaskProgress(task, "Copying Episodes");
        }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            lastSubdirectoryScanned = Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture);

            sortView = view;

            LoadSettings();
        }

        /// <summary>
        ///     Moves the episodes at the specified indices.
        /// </summary>
        public void MoveEpisodes()
        {
            var task = new BackgroundTask(() => fileSearch.Move());
            task.Start();
            sortView.StartTaskProgress(task, "Moving Episodes");
            ScanEpisodes(lastSubdirectoryScanned);
        }

        /// <summary>
        ///     Scans the specified subdirectory for settings.
        /// </summary>
        /// <param name="subdirectory">
        ///     The subdirectory.
        /// </param>
        public void ScanEpisodes(string subdirectory)
        {
            var task = new BackgroundTask(
                () =>
                {
                    fileSearch.Search(subdirectory);
                    OnPropertyChanged("Results");
                });
            task.Start();
            sortView.StartTaskProgress(task, "Scanning episodes");
        }

        /// <summary>
        ///     Sets the episode of the checked results.
        /// </summary>
        /// <param name="seasonNum">
        ///     The season number.
        /// </param>
        /// <param name="episodeNum">
        ///     The episode number.
        /// </param>
        public void SetEpisode(int seasonNum, int episodeNum)
        {
            fileSearch.SetEpisode(seasonNum, episodeNum);
        }

        /// <summary>
        ///     Sets the show of the specified indices.
        /// </summary>
        /// <param name="show">
        ///     The show to set them to.
        /// </param>
        public void SetShow(TvShow show)
        {
            fileSearch.SetShow(show);
        }

        /// <summary>
        ///     Loads the settings.
        /// </summary>
        private void LoadSettings()
        {
            settings = storageProvider.Settings;
            storageProvider.SettingsSaved += (sender, e) => { SubDirectories = LoadSubDirectories(); };
            SubDirectories = LoadSubDirectories();
        }

        /// <summary>
        ///     Loads the subdirectories of the input folder.
        /// </summary>
        /// <returns>
        ///     A list of the subdirectories.
        /// </returns>
        private BindingList<string> LoadSubDirectories()
        {
            var dirs = new BindingList<string>();

            try
            {
                var source = new DirectoryInfo(settings.SourceDirectory);
                if (source.Exists)
                {
                    dirs.Add(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture));
                    foreach (var subDir in source.GetDirectories())
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
    }
}
