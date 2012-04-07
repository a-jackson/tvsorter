// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for handle show searches.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Controller
{
    #region Using Directives

    using System.ComponentModel;

    using TVSorter.DAL;
    using TVSorter.Types;
    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for handle show searches.
    /// </summary>
    public abstract class ShowSearchController : ControllerBase
    {
        #region Constants and Fields

        /// <summary>
        ///   The text for the close button.
        /// </summary>
        private string closeButtonText;

        /// <summary>
        ///   The data provider.
        /// </summary>
        private IDataProvider dataProvider;

        /// <summary>
        ///   The list of results.
        /// </summary>
        private BindingList<TvShow> searchResults;

        /// <summary>
        ///   The storage provider.
        /// </summary>
        private IStorageProvider storageProvider;

        /// <summary>
        ///   The title of the form.
        /// </summary>
        private string title;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets the close button text.
        /// </summary>
        public string CloseButtonText
        {
            get
            {
                return this.closeButtonText;
            }

            protected set
            {
                this.closeButtonText = value;
                this.OnPropertyChanged("CloseButtonText");
            }
        }

        /// <summary>
        ///   Gets or sets SearchResults.
        /// </summary>
        public BindingList<TvShow> SearchResults
        {
            get
            {
                return this.searchResults;
            }

            protected set
            {
                this.searchResults = value;
                this.OnPropertyChanged("SearchResults");
            }
        }

        /// <summary>
        ///   Gets or sets the title of the form.
        /// </summary>
        public string Title
        {
            get
            {
                return this.title;
            }

            protected set
            {
                this.title = value;
                this.OnPropertyChanged("Title");
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.dataProvider = Factory.DataProvider;
            this.storageProvider = Factory.StorageProvider;
        }

        /// <summary>
        /// Searches for the specified name.
        /// </summary>
        /// <param name="name">
        /// The name to seach for. 
        /// </param>
        public void Search(string name)
        {
            this.SearchResults = new BindingList<TvShow>(this.dataProvider.SearchShow(name));
        }

        /// <summary>
        /// Selects the specified index as the chosen result.
        /// </summary>
        /// <param name="index">
        /// The index to select. 
        /// </param>
        public virtual void Select(int index)
        {
            this.storageProvider.SaveShow(this.SearchResults[index]);
        }

        #endregion
    }
}