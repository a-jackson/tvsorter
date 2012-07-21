// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A class for getting instances of the default providers.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using TVSorter.Data;
    using TVSorter.Data.Tvdb;
    using TVSorter.Storage;

    /// <summary>
    /// A class for getting instances of the default providers.
    /// </summary>
    internal static class Factory
    {
        #region Static Fields

        /// <summary>
        /// The data provider for the system.
        /// </summary>
        private static IDataProvider dataProvider;

        /// <summary>
        /// The storage provider for the system.
        /// </summary>
        private static IStorageProvider storageProvider;

        #endregion

        #region Properties

        /// <summary>
        /// Gets a new instance of the default data provider.
        /// </summary>
        internal static IDataProvider DataProvider
        {
            get
            {
                return dataProvider ?? (dataProvider = new Tvdb());
            }
        }

        /// <summary>
        /// Gets a new instance of the default storage provider.
        /// </summary>
        internal static IStorageProvider StorageProvider
        {
            get
            {
                return storageProvider ?? (storageProvider = new Xml());
            }
        }

        #endregion
    }
}