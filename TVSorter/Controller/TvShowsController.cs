// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShowsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the TV Shows tab.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TVSorter.Files;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.Storage;
using TVSorter.View;
using TVSorter.Wrappers;
using Settings = TVSorter.Model.Settings;

namespace TVSorter.Controller
{
    /// <summary>
    ///     The controller for the TV Shows tab.
    /// </summary>
    internal class TvShowsController : ControllerBase
    {
        /// <summary>
        ///     The scan manager.
        /// </summary>
        private readonly IScanManager scanManager;

        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     The TV show repository.
        /// </summary>
        private readonly ITvShowRepository tvShowRepository;

        /// <summary>
        ///     The collection of destination directories.
        /// </summary>
        private BindingList<string> destinationDirectories;

        /// <summary>
        ///     The selected show.
        /// </summary>
        private TvShow selectedShow;

        /// <summary>
        ///     The settings.
        /// </summary>
        private Settings settings;

        /// <summary>
        ///     The shows.
        /// </summary>
        private BindingList<TvShow> shows;

        /// <summary>
        ///     The TV view.
        /// </summary>
        private IView tvView;

        /// <summary>
        ///     Initialises a new instance of the <see cref="TvShowsController" /> class.
        /// </summary>
        /// <param name="storageProvider">The storage provider.</param>
        /// <param name="tvShowRepository">The TV show repository.</param>
        /// <param name="scanManager">The scan manager.</param>
        public TvShowsController(
            IStorageProvider storageProvider,
            ITvShowRepository tvShowRepository,
            IScanManager scanManager)
        {
            this.storageProvider = storageProvider;
            this.tvShowRepository = tvShowRepository;
            this.scanManager = scanManager;
        }

        /// <summary>
        ///     Gets SearchResults.
        /// </summary>
        public Dictionary<string, List<TvShow>> SearchResults { get; private set; }

        /// <summary>
        ///     Gets SelectedShow.
        /// </summary>
        public TvShow SelectedShow
        {
            get => selectedShow;

            private set
            {
                selectedShow = value;
                OnPropertyChanged("SelectedShow");
            }
        }

        /// <summary>
        ///     Gets Shows.
        /// </summary>
        public BindingList<TvShow> Shows
        {
            get => shows;

            private set
            {
                shows = value;
                OnPropertyChanged("Shows");
            }
        }

        /// <summary>
        ///     Gets the destination directories.
        /// </summary>
        public BindingList<string> DestinationDirectories
        {
            get => destinationDirectories;

            private set
            {
                destinationDirectories = value;
                OnPropertyChanged("DestinationDirectories");
            }
        }

        /// <summary>
        ///     The search shows complete.
        /// </summary>
        public event EventHandler SearchShowsComplete;

        /// <summary>
        ///     Occurs when a show changes.
        /// </summary>
        public event EventHandler<TvShowEventArgs> ShowChanged;

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            tvView = view;
            settings = storageProvider.Settings;
            Shows = new BindingList<TvShow>(tvShowRepository.GetTvShows().ToList());
            DestinationDirectories = new BindingList<string>(settings.DestinationDirectories);
            Shows.ListChanged += (sender, e) => OnPropertyChanged("Shows");
            TvShowSelected(0);
            tvShowRepository.TvShowAdded += OnTvShowAdded;
            tvShowRepository.TvShowChanged += OnTvShowChanged;
            tvShowRepository.TvShowRemoved += OnTvShowRemoved;
            storageProvider.SettingsSaved += OnSettingsChanged;
        }

        /// <summary>
        ///     Removes the selected show.
        /// </summary>
        public void RemoveSelectedShow()
        {
            if (SelectedShow == null)
            {
                return;
            }

            tvShowRepository.Delete(SelectedShow);
            Shows.Remove(SelectedShow);
            SelectedShow = null;
        }

        /// <summary>
        ///     Resets the last updated date of the selected show.
        /// </summary>
        public void ResetLastUpdated()
        {
            if (SelectedShow == null)
            {
                return;
            }

            SelectedShow.LastUpdated = new DateTime(1970, 1, 1);
            tvShowRepository.Save(SelectedShow);
            OnPropertyChanged("SelectedShow");
        }

        /// <summary>
        ///     Saves the selected show.
        /// </summary>
        public void SaveSelectedShow()
        {
            if (SelectedShow == null)
            {
                return;
            }

            tvShowRepository.Save(SelectedShow);
        }

        /// <summary>
        ///     Searches for new shows.
        /// </summary>
        public void SearchShows()
        {
            var task = new BackgroundTask(
                () =>
                {
                    SearchResults = scanManager.SearchNewShows(
                        settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x)));
                    OnSearchShowsComplete();
                });
            task.Start();

            tvView.StartTaskProgress(task, "Searching shows");
        }

        /// <summary>
        ///     Changes the selection of the TV show.
        /// </summary>
        /// <param name="newIndex">
        ///     The new index.
        /// </param>
        public void TvShowSelected(int newIndex)
        {
            if (newIndex < Shows.Count && newIndex >= 0 && Shows.Count > 0)
            {
                SelectedShow = Shows[newIndex];
            }
            else
            {
                SelectedShow = null;
            }
        }

        /// <summary>
        ///     Updates all shows.
        /// </summary>
        public void UpdateAllShows()
        {
            var task = new BackgroundTask(
                () =>
                {
                    // Only update the unlocked shows.
                    var unlockedShows = Shows.Where(x => !x.Locked).ToList();
                    tvShowRepository.UpdateShows(unlockedShows);
                    OnPropertyChanged("SelectedShow");
                });
            task.Start();

            tvView.StartTaskProgress(task, "Updating All Shows");
        }

        /// <summary>
        ///     Updates the selected show.
        /// </summary>
        public void UpdateSelectedShow()
        {
            var task = new BackgroundTask(
                () =>
                {
                    if (SelectedShow == null)
                    {
                        return;
                    }

                    tvShowRepository.Update(SelectedShow);
                    OnPropertyChanged("SelectedShow");
                });
            task.Start();
            tvView.StartTaskProgress(task, "Updating " + SelectedShow.Name);
        }

        /// <summary>
        ///     Raises an on search shows complete event.
        /// </summary>
        private void OnSearchShowsComplete()
        {
            if (SearchShowsComplete != null)
            {
                SearchShowsComplete(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Handles the TVShow's OnTvShowAdded event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowAdded(object sender, TvShowEventArgs e)
        {
            Shows.Add(e.TvShow);
        }

        /// <summary>
        ///     Handles the TVShow's OnTvShowChanged event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowChanged(object sender, TvShowEventArgs e)
        {
            var index = Shows.IndexOf(e.TvShow);
            if (index != -1)
            {
                Shows[index] = e.TvShow;
                if (e.TvShow.Equals(SelectedShow))
                {
                    SelectedShow = e.TvShow;
                }
            }
            else
            {
                Shows.Add(e.TvShow);
            }

            if (ShowChanged != null)
            {
                ShowChanged(sender, e);
            }
        }

        /// <summary>
        ///     Handles the TVShow's OnTvShowRemoved event.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnTvShowRemoved(object sender, TvShowEventArgs e)
        {
            Shows.Remove(e.TvShow);
        }

        /// <summary>
        ///     Handles the settings changing.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void OnSettingsChanged(object sender, EventArgs e)
        {
            DestinationDirectories = new BindingList<string>(settings.DestinationDirectories);
        }
    }
}
