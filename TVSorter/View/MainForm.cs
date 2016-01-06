// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="MainForm.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The main form of the program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Windows.Forms;

    using Files;
    using Repostitory;
    using Version = TVSorter.Version;

    #endregion

    /// <summary>
    /// The main form of the program.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constructors and Destructors

        /// <summary>
        ///   Initialises a new instance of the <see cref="MainForm" /> class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();

            var storageProvider = new Storage.Xml();
            var dataProvider = new Data.Tvdb.Tvdb(storageProvider);
            var tvshowRepository = new TvShowRepository(storageProvider, dataProvider);
            var scanManager = new ScanManager(storageProvider, dataProvider, tvshowRepository);
            var fileResultManager = new FileResultManager(storageProvider);
            var fileManager = new FileManager(storageProvider, dataProvider, scanManager, fileResultManager);
            var fileSearch = new FileSearch(storageProvider, dataProvider, scanManager, fileManager);

            missingDuplicateEpisodes.StorageProvider = storageProvider;
            missingDuplicateEpisodes.FileSearch = fileSearch;

            settings.StorageProvider = storageProvider;
            settings.FileResultManager = fileResultManager;

            sortEpisodes.StorageProvider = storageProvider;
            sortEpisodes.FileSearch = fileSearch;
            sortEpisodes.TvShowRepository = tvshowRepository;
            sortEpisodes.FileResultManager = fileResultManager;

            tvShows.StorageProvider = storageProvider;
            tvShows.TvShowRepository = tvshowRepository;
            tvShows.ScanManager = scanManager;
            tvShows.FileResultManager = fileResultManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the load event for the form.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void MainFormLoad(object sender, EventArgs e)
        {
            this.Text = "TV Sorter " + Version.CurrentVersion;
        }

        #endregion
    }
}