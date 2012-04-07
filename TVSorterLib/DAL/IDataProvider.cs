// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IDataProvider.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface of a data provider.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The public interface of a data provider.
    /// </summary>
    public interface IDataProvider
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a log message occurs.
        /// </summary>
        event EventHandler<LogMessageEventArgs> LogMessage;

        /// <summary>
        ///   Occurs when a show is updated.
        /// </summary>
        event EventHandler<ShowUpdatedEventArgs> ShowUpdated;

        /// <summary>
        ///   Occurs when an update shows operation is completed.
        /// </summary>
        event EventHandler<UpdateShowsCompletedEventArgs> UpdateShowsCompleted;

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
        List<TvShow> SearchShow(string name);

        /// <summary>
        /// Updates the specified collection of shows.
        /// </summary>
        /// <param name="shows">
        ///   The shows to update. 
        /// </param>
        void UpdateShows(IList<TvShow> shows);

        /// <summary>
        /// Updates the specified collection of shows asynchronously.
        /// </summary>
        /// <param name="shows">
        ///   The shows to update. 
        /// </param>
        /// <param name="userState">
        ///   A state object that will be returned in all events. 
        /// </param>
        void UpdateShowsAsync(IList<TvShow> shows, object userState);

        #endregion
    }
}