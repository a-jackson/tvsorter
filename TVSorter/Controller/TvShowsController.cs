// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShowsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for the TV Shows tab.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Controller
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    using TVSorter.DAL;
    using TVSorter.Types;
    using TVSorter.View;

    using Settings = TVSorter.Types.Settings;

    #endregion

    /// <summary>
    /// The controller for the TV Shows tab.
    /// </summary>
    internal class TvShowsController : ControllerBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The data provider.
        /// </summary>
        private IDataProvider data;

        /// <summary>
        ///   The storage.
        /// </summary>
        private IStorageProvider storage;

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

        #endregion

        #region Public Events

        /// <summary>
        ///   The search shows complete.
        /// </summary>
        public event EventHandler SearchShowsComplete;

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

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The create nfo files.
        /// </summary>
        public void CreateNfoFiles()
        {
            this.MaxValue = this.Shows.Count;
            this.Value = 0;
            Task.Factory.StartNew(
                delegate
                    {
                        foreach (var show in this.Shows)
                        {
                            TvShow show1 = show;
                            string url = string.Format("http://thetvdb.com/?tab=series&id={0}&lid=7", show1.TvdbId);
                            IEnumerable<string> files =
                                from destination in this.storage.LoadSettings().DestinationDirectories
                                where
                                    Directory.Exists(
                                        string.Concat(destination, Path.DirectorySeparatorChar, show1.FolderName))
                                select
                                    string.Format(
                                        "{0}{2}{1}{2}tvshow.nfo", 
                                        destination, 
                                        show1.FolderName, 
                                        Path.DirectorySeparatorChar);
                            foreach (var file in files.Where(x => !File.Exists(x)))
                            {
                                File.WriteAllText(file, url);
                            }

                            this.Value++;
                            this.OnProgressChanged();
                        }

                        this.OnTaskComplete();
                    });
            this.tvView.StartTaskProgress(this, "Creating .nfo files.");
        }

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.tvView = view;
            this.storage = Factory.StorageProvider;
            this.data = Factory.DataProvider;
            this.data.ShowUpdated += this.ShowUpdated;
            this.data.UpdateShowsCompleted += this.UpdateShowsCompleted;
            this.storage.ShowAdded += this.ShowAdded;
            this.storage.ShowRemoved += this.ShowRemoved;

            try
            {
                this.Shows = new BindingList<TvShow>(this.storage.LoadTvShows());
                this.TvShowSelected(0);
            } 
            catch (Exception)
            {  
            }
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
                this.storage.RemoveShow(this.SelectedShow);
          
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
                this.storage.SaveShow(this.SelectedShow);
        }

        /// <summary>
        /// Searches for new shows.
        /// </summary>
        public void SearchShows()
        {
            Settings settings = this.storage.LoadSettings();
            var showDirs = new List<string>();
            foreach (var dirInfo in
                from dir in settings.DestinationDirectories where Directory.Exists(dir) select new DirectoryInfo(dir))
            {
                showDirs.AddRange(
                    from dir in dirInfo.GetDirectories()
                    where !showDirs.Contains(dir.Name) && !this.Shows.Select(x => x.FolderName).Contains(dir.Name)
                    select dir.Name);
            }

            this.SearchResults = new Dictionary<string, List<TvShow>>();
            this.MaxValue = showDirs.Count;
            this.Value = 0;
            Task.Factory.StartNew(
                delegate
                    {
                        foreach (var showName in showDirs)
                        {
                            List<TvShow> results = this.data.SearchShow(showName);
                            if (results.Count == 1)
                            {
                                this.storage.SaveShow(results[0]);
                            }
                            else
                            {
                                this.SearchResults.Add(showName, results);
                            }

                            this.Value++;
                            this.OnProgressChanged();
                        }

                        this.OnSearchShowsComplete();
                        this.OnTaskComplete();
                    });
            this.tvView.StartTaskProgress(this, "Searching shows");
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
            // Only update the unlocked shows.
            List<TvShow> unlockedShows = this.Shows.Where(x => !x.Locked).ToList();
            this.MaxValue = unlockedShows.Count;
            this.Value = 0;
            this.data.UpdateShowsAsync(unlockedShows, this);
            this.tvView.StartTaskProgress(this, "Updating All Shows");
        }

        /// <summary>
        /// Updates the selected show.
        /// </summary>
        public void UpdateSelectedShow()
        {
            if (this.SelectedShow == null)
            {
                return;
            }
            this.MaxValue = 1;
            this.Value = 0;
            this.data.UpdateShowsAsync(new List<TvShow> { this.SelectedShow }, this);
            this.tvView.StartTaskProgress(this, "Updating " + this.SelectedShow.Name);
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
        /// Handles a new show being added.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ShowAdded(object sender, ShowEventArgs e)
        {
            if (this.tvView.InvokeRequired)
            {
                this.tvView.Invoke(new Action<object, ShowEventArgs>(this.ShowAdded), new[] { sender, e });
            }
            else
            {
                this.Shows.Add(e.Show);
            }
        }

        /// <summary>
        /// Handles a show being removed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ShowRemoved(object sender, ShowEventArgs e)
        {
            if (this.tvView.InvokeRequired)
            {
                this.tvView.Invoke(new Action<object, ShowEventArgs>(this.ShowRemoved), new[] { sender, e });
            }
            else
            {
                this.Shows.Remove(e.Show);
            }
        }

        /// <summary>
        /// Handles a show being updated.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ShowUpdated(object sender, ShowUpdatedEventArgs e)
        {
            if (e.UserState != this)
            {
                return;
            }

            this.Value++;
            this.OnProgressChanged();

            if (!e.Success)
            {
                MessageBox.Show(e.Error.Message);
            }
        }

        /// <summary>
        /// The update shows completed.
        /// </summary>
        /// <param name="sender">
        /// The sender. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void UpdateShowsCompleted(object sender, UpdateShowsCompletedEventArgs e)
        {
            if (e.UserState != this)
        {
                return;
            }

            this.OnTaskComplete();

            // Refresh the selected show.
            this.OnPropertyChanged("SelectedShow");
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
            this.storage.SaveShow(this.SelectedShow);
            this.OnPropertyChanged("SelectedShow");
        }
        #endregion
    }
}