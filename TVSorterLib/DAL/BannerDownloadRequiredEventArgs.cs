// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="BannerDownloadRequiredEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for the banner download required event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The event args for the banner download required event.
    /// </summary>
    public class BannerDownloadRequiredEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BannerDownloadRequiredEventArgs"/> class.
        /// </summary>
        /// <param name="show">
        /// The show. 
        /// </param>
        /// <param name="newBanner">
        /// The new banner. 
        /// </param>
        public BannerDownloadRequiredEventArgs(TvShow show, string newBanner)
        {
            this.Show = show;
            this.NewBanner = newBanner;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the new banner to download.
        /// </summary>
        public string NewBanner { get; private set; }

        /// <summary>
        ///   Gets the show that the event pertains to.
        /// </summary>
        public TvShow Show { get; private set; }

        #endregion
    }
}