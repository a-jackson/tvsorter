// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for handle show searches.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    #region Using Directives

    using System.ComponentModel;

    using TVSorter.Model;
    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for handle show searches.
    /// </summary>
    public abstract class ShowSearchController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   The text for the close button.
        /// </summary>
        private string closeButtonText;

        /// <summary>
        ///   The list of results.
        /// </summary>
        private BindingList<TvShow> searchResults;

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
        }

        /// <summary>
        /// Searches for the specified name.
        /// </summary>
        /// <param name="name">
        /// The name to seach for. 
        /// </param>
        public void Search(string name)
        {
            this.SearchResults = new BindingList<TvShow>(TvShow.SearchShow(name));
        }

        /// <summary>
        /// Selects the specified index as the chosen result.
        /// </summary>
        /// <param name="index">
        /// The index to select. 
        /// </param>
        public virtual void Select(int index)
        {
            TvShow show = TvShow.FromSearchResult(this.SearchResults[index]);
            show.Save();
        }

        #endregion
    }
}