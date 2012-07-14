// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Tvdb.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Manages accessing show data from the TVDB.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Data.Tvdb
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using TVSorter.Storage;
    using TVSorter.Types;

    #endregion

    /// <summary>
    /// Manages accessing show data from the TVDB.
    /// </summary>
    internal class Tvdb : DalBase, IDataProvider
    {
        #region Static Fields

        /// <summary>
        ///   The directory to cache data in.
        /// </summary>
        public static readonly string CacheDirectory = "Cache" + Path.DirectorySeparatorChar;

        /// <summary>
        ///   The directory to download images to.
        /// </summary>
        public static readonly string ImageDirectory = "Images" + Path.DirectorySeparatorChar;

        #endregion

        #region Fields

        /// <summary>
        ///   The tvdb download.
        /// </summary>
        private readonly TvdbDownload tvdbDownload;

        /// <summary>
        ///   The tvdb process.
        /// </summary>
        private readonly TvdbProcess tvdbProcess;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="Tvdb" /> class. Initialises a new instance of the <see cref="Tvdb" /> class.
        /// </summary>
        public Tvdb()
        {
            this.tvdbDownload = new TvdbDownload();
            this.tvdbProcess = new TvdbProcess();
            this.tvdbProcess.BannerDownloadRequired += this.BannerDownloadRequired;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when a show is updated.
        /// </summary>
        public event EventHandler<ShowUpdatedEventArgs> ShowUpdated;

        /// <summary>
        ///   Occurs when an update shows operation is completed.
        /// </summary>
        public event EventHandler<UpdateShowsCompletedEventArgs> UpdateShowsCompleted;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Search for a show.
        /// </summary>
        /// <param name="name">
        /// The name to search for. 
        /// </param>
        /// <returns>
        /// The list of results. 
        /// </returns>
        public List<TvShow> SearchShow(string name)
        {
            StringReader result = this.tvdbDownload.SearchShow(name);
            return this.tvdbProcess.ProcessSearch(result, name);
        }

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update. 
        /// </param>
        public void UpdateShows(IList<TvShow> shows)
        {
            var state = new object();
            this.UpdateShowsCompleted += (sender, e) =>
                {
                    if (e.UserState != state)
                    {
                        return;
                    }

                    lock (state)
                    {
                        Monitor.Pulse(state);
                    }
                };
            this.UpdateShowsAsync(shows, state);
            lock (state)
            {
                Monitor.Wait(state);
            }
        }

        /// <summary>
        /// Updates the specified collection of shows asynchronously.
        /// </summary>
        /// <param name="shows">
        /// The shows to update. 
        /// </param>
        /// <param name="userState">
        /// A state object that will be returned in all events. 
        /// </param>
        public void UpdateShowsAsync(IList<TvShow> shows, object userState)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        // Determine which shows require an update.
                        // Get the earliest update time.
                        DateTime updateTime = shows.Select(x => x.LastUpdated).OrderBy(x => x).First();

                        List<string> updateIds;

                        // If the update time is greater than 30 days old then update all shows.
                        if (DateTime.Now - updateTime > TimeSpan.FromDays(30))
                        {
                            updateIds = shows.Select(x => x.TvdbId).ToList();
                        }
                        else
                        {
                            StringReader updates = this.tvdbDownload.DownloadUpdates(updateTime);
                            updateIds = this.tvdbProcess.ProcessUpdates(updates).ToList();

                            // The updates list only shows 1000 updates so data may be missing if 
                            // there are 1000 ids in the list. Update all shows in this case.
                            if (updateIds.Count == 1000)
                            {
                                updateIds = shows.Select(x => x.TvdbId).ToList();
                            }
                        }

                        DateTime serverTime = this.tvdbDownload.GetServerTime();

                        var taskList = new List<Task>();

                        foreach (TvShow show in shows)
                        {
                            TvShow show1 = show;

                            // If the show has no updates then skip it and continue.
                            if (!updateIds.Contains(show1.TvdbId))
                            {
                                this.OnLogMessage("{0} has no updates.", show1.Name);
                                this.OnShowUpdated(show1, userState);

                                // Show has been updated so update it's time.
                                // Use the lock to prevent concurrent writes to storage.
                                lock (this)
                                {
                                    show1.LastUpdated = serverTime;
                                    Factory.StorageProvider.SaveShow(show1);
                                }

                                continue;
                            }

                            var downloadTask = new Task<string>(() => this.tvdbDownload.DownloadShowEpisodes(show1));
                            downloadTask.Start();

                            Task process = downloadTask.ContinueWith(
                                xmlFile =>
                                    {
                                        if (!xmlFile.IsFaulted)
                                        {
                                            try
                                            {
                                                lock (this)
                                                {
                                                    this.tvdbProcess.ProcessShow(
                                                        show1, xmlFile.Result, this.OnLogMessage, serverTime);
                                                }

                                                this.OnShowUpdated(show1, userState);
                                            }
                                            catch (Exception e)
                                            {
                                                this.OnShowUpdated(show1, userState, e);
                                            }
                                        }
                                        else
                                        {
                                            this.OnShowUpdated(show1, userState, xmlFile.Exception);
                                        }
                                    });
                            taskList.Add(process);
                        }

                        if (taskList.Count > 0)
                        {
                            Task.Factory.ContinueWhenAll(
                                taskList.ToArray(), obj => this.OnUpdateShowsCompleted(userState));
                        }
                        else
                        {
                            // There were no tasks so complete now.
                            this.OnUpdateShowsCompleted(userState);
                        }
                    });
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles a Banner Download Required event.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void BannerDownloadRequired(object sender, BannerDownloadRequiredEventArgs e)
        {
            e.Show.Banner = e.NewBanner;
            lock (this)
            {
                Factory.StorageProvider.SaveShow(e.Show);
            }

            this.tvdbDownload.DownloadBanner(e.Show);
        }

        /// <summary>
        /// Raises a show updated event.
        /// </summary>
        /// <param name="show">
        /// The show that was updated. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnShowUpdated(TvShow show, object userState)
        {
            if (this.ShowUpdated != null)
            {
                this.ShowUpdated(this, new ShowUpdatedEventArgs(show, userState));
            }
        }

        /// <summary>
        /// Raises a show updated event with an error.
        /// </summary>
        /// <param name="show">
        /// The show that was updated. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        /// <param name="error">
        /// The error that occured during the operation. 
        /// </param>
        private void OnShowUpdated(TvShow show, object userState, Exception error)
        {
            if (this.ShowUpdated != null)
            {
                this.ShowUpdated(this, new ShowUpdatedEventArgs(show, userState, error));
            }
        }

        /// <summary>
        /// Raises a show update completed event.
        /// </summary>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnUpdateShowsCompleted(object userState)
        {
            if (this.UpdateShowsCompleted != null)
            {
                this.UpdateShowsCompleted(this, new UpdateShowsCompletedEventArgs(userState));
            }
        }

        #endregion
    }
}