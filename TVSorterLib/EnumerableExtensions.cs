// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System.Collections.Generic;

    using TVSorter.Storage;

    /// <summary>
    /// Extenstions to the IEnumerable interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        #region Methods

        /// <summary>
        /// Saves a collection of TVShows.
        /// </summary>
        /// <param name="shows">
        /// The shows to save.
        /// </param>
        internal static void Save(this IEnumerable<TvShow> shows)
        {
            var xml = new Xml();
            xml.SaveShows(shows);
        }

        #endregion
    }
}