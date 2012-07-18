// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Factory.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
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
        #region Properties

        /// <summary>
        /// Gets a new instance of the default data provider.
        /// </summary>
        internal static IDataProvider DataProvider
        {
            get
            {
                return new Tvdb();
            }
        }

        /// <summary>
        /// Gets a new instance of the default storage provider.
        /// </summary>
        internal static IStorageProvider StorageProvider
        {
            get
            {
                return new Xml();
            }
        }

        #endregion
    }
}