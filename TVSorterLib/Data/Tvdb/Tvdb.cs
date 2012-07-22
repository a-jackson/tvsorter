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

    using TVSorter.Model;

    #endregion

    /// <summary>
    /// Manages accessing show data from the TVDB.
    /// </summary>
    internal class Tvdb : IDataProvider
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
        ///   The tvdb process.
        /// </summary>
        private readonly TvdbProcess tvdbProcess;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Tvdb"/> class.
        /// </summary>
        public Tvdb()
        {
            this.tvdbProcess = new TvdbProcess();
            this.tvdbProcess.BannerDownloadRequired += this.BannerDownloadRequired;
        }

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
            StringReader result = TvdbDownload.SearchShow(name);
            return this.tvdbProcess.ProcessSearch(result, name);
        }

        /// <summary>
        /// Updates the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to update. 
        /// </param>
        public void UpdateShow(TvShow show)
        {
            DateTime serverTime = TvdbDownload.GetServerTime();
            string xmlFile = TvdbDownload.DownloadShowEpisodes(show);
            try
            {
                this.tvdbProcess.ProcessShow(show, xmlFile, serverTime);
            }
            catch (Exception e)
            {
                Logger.OnLogMessage(this, "Error parsing XML for {0}. {1}", show.Name, e.Message);
            }
        }

        /// <summary>
        /// Updates the collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
        /// </param>
        public void UpdateShows(IList<TvShow> shows)
        {
            DateTime firstUpdate = shows.Min(x => x.LastUpdated);
            StringReader updates = TvdbDownload.DownloadUpdates(firstUpdate);
            List<string> updateIds = this.tvdbProcess.ProcessUpdates(updates).ToList();
            DateTime serverTime = TvdbDownload.GetServerTime();
            foreach (TvShow show in shows)
            {
                if (updateIds.Contains(show.TvdbId))
                {
                    Logger.OnLogMessage(this, "No updates for {0}", show.Name);
                    show.LastUpdated = serverTime;
                }
                else
                {
                    this.tvdbProcess.ProcessShow(show, TvdbDownload.DownloadShowEpisodes(show), serverTime);
                }
            }
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
            TvdbDownload.DownloadBanner(e.Show);
        }

        #endregion
    }
}