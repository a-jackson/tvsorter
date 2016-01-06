// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ITvShowRepository.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The interface for the TV Show Repository.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Repostitory
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using Model;
    
    #endregion

    /// <summary>
    /// The interface for the TV show repository.
    /// </summary>
    public interface ITvShowRepository
    {
        /// <summary>
        /// Raised when a new TV Show is added.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowAdded;

        /// <summary>
        /// Raised when an existing TV show is changed.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowChanged;

        /// <summary>
        /// Raised when a TV show is removed.
        /// </summary>
        event EventHandler<TvShowEventArgs> TvShowRemoved;

        /// <summary>
        /// Deletes the specified show.
        /// </summary>
        /// <param name="show">The show to delete.</param>
        void Delete(TvShow show);

        /// <summary>
        /// Creates a TV show from the specified search result TV show.
        /// </summary>
        /// <param name="searchResult">The search result.</param>
        /// <returns>A TV show object derived from the search result.</returns>
        TvShow FromSearchResult(TvShow searchResult);

        /// <summary>
        /// Gets a collection of all the TV Shows.
        /// </summary>
        /// <returns>The collection of TV shows.</returns>
        IEnumerable<TvShow> GetTvShows();

        /// <summary>
        /// Saves the specified TV Show.
        /// </summary>
        /// <param name="show">The show to save.</param>
        void Save(TvShow show);

        /// <summary>
        /// Searches for shows with the specified name.
        /// </summary>
        /// <param name="name">The name of the show to search.</param>
        /// <returns>The collection of search results.</returns>
        List<TvShow> SearchShow(string name);

        /// <summary>
        /// Updates the data of the specified show.
        /// </summary>
        /// <param name="show">The show to update.</param>
        void Update(TvShow show);

        /// <summary>
        /// Updates the specified collection of show's data.
        /// </summary>
        /// <param name="shows">The collection of shows to update.</param>
        void UpdateShows(IList<TvShow> shows);
    }
}