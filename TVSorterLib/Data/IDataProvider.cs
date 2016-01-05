// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDataProvider.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Interface for getting show data.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Data
{
    using System.Collections.Generic;

    using TVSorter.Model;
    using TVSorter.Storage;

    /// <summary>
    /// Interface for getting show data.
    /// </summary>
    public interface IDataProvider
    {
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
        /// Updates the specified show.
        /// </summary>
        /// <param name="show">
        /// The show to update. 
        /// </param>
        void UpdateShow(TvShow show);

        /// <summary>
        /// Updates the collection of shows.
        /// </summary>
        /// <param name="shows">
        /// The shows to update.
        /// </param>
        /// <returns>
        /// The collection of TVShows that have been updated.
        /// </returns>
        IEnumerable<TvShow> UpdateShows(IList<TvShow> shows);

        #endregion
    }
}