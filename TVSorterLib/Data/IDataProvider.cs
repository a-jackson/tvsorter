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

        #endregion
    }
}