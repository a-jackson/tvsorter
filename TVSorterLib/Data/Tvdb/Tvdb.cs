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

    #endregion

    /// <summary>
    /// Manages accessing show data from the TVDB.
    /// </summary>
    internal class Tvdb
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
        /// Updates the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to update. 
        /// </param>
        public void UpdateShow(TvShow show)
        {
            DateTime serverTime = this.tvdbDownload.GetServerTime();
            string xmlFile = this.tvdbDownload.DownloadShowEpisodes(show);
            this.tvdbProcess.ProcessShow(show, xmlFile, serverTime);
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
            this.tvdbDownload.DownloadBanner(e.Show);
        }

        #endregion
    }
}