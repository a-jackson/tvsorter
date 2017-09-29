// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Extenstions to the IEnumerable interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using TVSorter.Model;
using TVSorter.Storage;

namespace TVSorter
{
    /// <summary>
    ///     Extensions to the IEnumerable interface.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        ///     Saves a collection of TVShows.
        /// </summary>
        /// <param name="shows">
        ///     The shows to save.
        /// </param>
        /// <param name="provider">
        ///     The provider to save the shows with.
        /// </param>
        internal static void Save(this IEnumerable<TvShow> shows, IStorageProvider provider)
        {
            provider.SaveShows(shows);
        }
    }
}
