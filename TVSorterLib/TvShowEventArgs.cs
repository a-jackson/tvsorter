// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TvShowEventArgs.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Arguments for a TV Show related event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System;

    using TVSorter.Model;

    /// <summary>
    /// Arguments for a TV Show related event.
    /// </summary>
    public class TvShowEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TvShowEventArgs"/> class. 
        /// </summary>
        /// <param name="tvShow">
        /// The TV Show for the event.
        /// </param>
        public TvShowEventArgs(TvShow tvShow)
        {
            this.TvShow = tvShow;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the TV Show for the event.
        /// </summary>
        public TvShow TvShow { get; private set; }

        #endregion
    }
}