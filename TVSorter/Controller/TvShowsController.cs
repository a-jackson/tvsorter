// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShowsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the TV Shows tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    using Files;
    using Repostitory;
    using Storage;
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;

    using TVSorter.Model;
    using TVSorter.View;
    using Wrappers;
    #endregion

    /// <summary>
    /// The controller for the TV Shows tab.
    /// </summary>
    internal class TvShowsController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   The selected show.
        /// </summary>
        private TvShow selectedShow;

        /// <summary>
        ///   The shows.
        /// </summary>
        private BindingList<TvShow> shows;

        /// <summary>
        ///   The tv view.
        /// </summary>
        private IView tvView;

        /// <summary>
        /// The collection of destination directories.
        /// </summary>
        private BindingList<string> destinationDirectories;

        /// <summary>
        /// The settings.
        /// </summary>
        private Model.Settings settings;

        private IStorageProvider storageProvider;
        private ITvShowRepository tvShowRepository;
        private IScanManager scanManager;

        #endregion

        public TvShowsController(IStorageProvider storageProvider, ITvShowRepository tvShowRepository, IScanManager scanManager)
        {
            this.storageProvider = storageProvider;
            this.tvShowRepository = tvShowRepository;
            this.scanManager = scanManager;
        }

        #region Public Events

        /// <summary>
        ///   The search shows complete.
        /// </summary>
        public event EventHandler SearchShowsComplete;

        /// <summary>
        /// Occurs when a show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> ShowChanged;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets SearchResults.
        /// </summary>
        public Dictionary<string, List<TvShow>> SearchResults { get; private set; }

        /// <summary>
        ///   Gets SelectedShow.
        /// </summary>
        public TvShow SelectedShow
        {
            get
            {
                return this.selectedShow;
            }

            private set
            {
                this.selectedShow = value;
                this.OnPropertyChanged("SelectedShow");
            }
        }

        /// <summary>
        ///   Gets Shows.
        /// </summary>
        public BindingList<TvShow> Shows
        {
            get
            {
                return this.shows;
            }

            private set
            {
                this.shows = value;
                this.OnPropertyChanged("Shows");
            }
        }

        /// <summary>
        /// Gets the destination directories.
        /// </summary>
        public BindingList<string> DestinationDirectories
        {
            get
            {
                return this.destinationDirectories;
            }

            private set
            {
                this.destinationDirectories = value;
                this.OnPropertyChanged("DestinationDirectories");
            }
        }

        #endregion

        #region Public Methods and Operators
        
        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.tvView = view;
            this.settings = storageProvider.Settings;
            this.Shows = new BindingList<TvShow>(tvShowRepository.GetTvShows().ToList());
            this.DestinationDirectories = new BindingList<string>(this.settings.DestinationDirectories);
            this.Shows.ListChanged += (sender, e) => this.OnPropertyChanged("Shows");
            this.TvShowSelected(0);
            tvShowRepository.TvShowAdded += this.OnTvShowAdded;
            tvShowRepository.TvShowChanged += this.OnTvShowChanged;
            tvShowRepository.TvShowRemoved += this.OnTvShowRemoved;
            storageProvider.SettingsSaved += this.OnSettingsChanged;
        }

        /// <summary>
        /// Removes the selected show.
        /// </summary>
        public void RemoveSelectedShow()
        {
            if (this.SelectedShow == null)
            {
                return;
            }

            tvShowRepository.Delete(this.SelectedShow);
            this.Shows.Remove(this.SelectedShow);
            this.SelectedShow = null;
        }

        /// <summary>
        /// Resets the last updated date of the selected show.
        /// </summary>
        public void ResetLastUpdated()
        {
            if (this.SelectedShow == null)
            {
                return;
            }

            this.SelectedShow.LastUpdated = new DateTime(1970, 1, 1);
            tvShowRepository.Save(this.SelectedShow);
            this.OnPropertyChanged("SelectedShow");
        }

        /// <summary>
        /// Saves the selceted show.
        /// </summary>
        public void SaveSelectedShow()
        {
            if (this.SelectedShow == null)
            {
                return;
            }

            tvShowRepository.Save(this.SelectedShow);
        }

        /// <summary>
        /// Searches for new shows.
        /// </summary>
        public void SearchShows()
        {
            var task = new BackgroundTask(
                () =>
                    {
                        this.SearchResults = scanManager.SearchNewShows(settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x)));
                        this.OnSearchShowsComplete();
                    });
            task.Start();

            this.tvView.StartTaskProgress(task, "Searching shows");
        }

        /// <summary>
        /// Changes the selection of the TV show.
        /// </summary>
        /// <param name="newIndex">
        /// The new index. 
        /// </param>
        public void TvShowSelected(int newIndex)
        {
            if (newIndex < this.Shows.Count && newIndex >= 0 && this.Shows.Count > 0)
            {
                this.SelectedShow = this.Shows[newIndex];
            }
            else
            {
                this.SelectedShow = null;
            }
        }

        /// <summary>
        /// Updates all shows.
        /// </summary>
        public void UpdateAllShows()
        {
            var task = new BackgroundTask(
                () =>
                    {
                        // Only update the unlocked shows.
                        List<TvShow> unlockedShows = this.Shows.Where(x => !x.Locked).ToList();
                        tvShowRepository.UpdateShows(unlockedShows);
                        this.OnPropertyChanged("SelectedShow");
                    });
            task.Start();

            this.tvView.StartTaskProgress(task, "Updating All Shows");
        }

        /// <summary>
        /// Updates the selected show.
        /// </summary>
        public void UpdateSelectedShow()
        {
            var task = new BackgroundTask(
                () =>
                    {
                        if (this.SelectedShow == null)
                        {
                            return;
                        }

                        tvShowRepository.Update(this.SelectedShow);
                        this.OnPropertyChanged("SelectedShow");
                    });
            task.Start();
            this.tvView.StartTaskProgress(task, "Updating " + this.SelectedShow.Name);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises an on search shows complete event.
        /// </summary>
        private void OnSearchShowsComplete()
        {
            if (this.SearchShowsComplete != null)
            {
                this.SearchShowsComplete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the TVShow's OnTvShowAdded event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowAdded(object sender, TvShowEventArgs e)
        {
            this.Shows.Add(e.TvShow);
        }

        /// <summary>
        /// Handles the TVShow's OnTvShowChanged event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowChanged(object sender, TvShowEventArgs e)
        {
            int index = this.Shows.IndexOf(e.TvShow);
            if (index != -1)
            {
                this.Shows[index] = e.TvShow;
                if (e.TvShow.Equals(this.SelectedShow))
                {
                    this.SelectedShow = e.TvShow;
                }
            }
            else
            {
                this.Shows.Add(e.TvShow);
            }

            if (this.ShowChanged != null)
            {
                this.ShowChanged(sender, e);
            }
        }

        /// <summary>
        /// Handles the TVShow's OnTvShowRemoved event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnTvShowRemoved(object sender, TvShowEventArgs e)
        {
            this.Shows.Remove(e.TvShow);
        }

        /// <summary>
        /// Handles the settings changing.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void OnSettingsChanged(object sender, EventArgs e)
        {
            this.DestinationDirectories = new BindingList<string>(this.settings.DestinationDirectories);
        }

        #endregion
    }
}