// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumerableExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Extenstions to the IEnumerable interface.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System.Collections.Generic;

    using TVSorter.Data;
    using TVSorter.Model;
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
        /// <param name="provider">
        /// The provider to save the shows with. 
        /// </param>
        internal static void Save(this IEnumerable<TvShow> shows, IStorageProvider provider)
        {
            provider.SaveShows(shows);
        }

        /// <summary>
        /// Updates the collection of TVShows.
        /// </summary>
        /// <param name="shows">
        /// The collection of shows to update.
        /// </param>
        /// <param name="storageProvider">
        /// The storage provider to use
        /// </param>
        /// <param name="dataProvider">
        /// The data provider to use.
        /// </param>
        internal static void Update(
            this IList<TvShow> shows, IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            foreach (var show in dataProvider.UpdateShows(shows, storageProvider))
            {
                show.LockIfNoEpisodes(storageProvider);
                show.Save(storageProvider);
            }
        }

        #endregion
    }
}