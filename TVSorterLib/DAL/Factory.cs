// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Factory.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Factory for retrieving instances of DAL types.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    /// <summary>
    /// Factory for retrieving instances of DAL types.
    /// </summary>
    public static class Factory
    {
        #region Constants and Fields

        /// <summary>
        ///   The data provider.
        /// </summary>
        private static IDataProvider dataProvider;

        /// <summary>
        ///   The file manager.
        /// </summary>
        private static IFileManager fileManager;

        /// <summary>
        ///   The scan manager.
        /// </summary>
        private static IScanManager scanner;

        /// <summary>
        ///   The storage provider.
        /// </summary>
        private static IStorageProvider storageProvider;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the data provider.
        /// </summary>
        public static IDataProvider DataProvider
        {
            get
            {
                return dataProvider ?? (dataProvider = new Tvdb());
            }
        }

        /// <summary>
        ///   Gets the file manager.
        /// </summary>
        public static IFileManager FileManager
        {
            get
            {
                return fileManager ?? (fileManager = new FileManager());
            }
        }

        /// <summary>
        ///   Gets the scan manager.
        /// </summary>
        public static IScanManager Scanner
        {
            get
            {
                return scanner ?? (scanner = new ScanManager());
            }
        }

        /// <summary>
        ///   Gets the storage provider.
        /// </summary>
        public static IStorageProvider StorageProvider
        {
            get
            {
                return storageProvider ?? (storageProvider = new Xml());
            }
        }

        #endregion
    }
}