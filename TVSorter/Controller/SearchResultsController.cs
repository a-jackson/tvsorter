// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SearchResultsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for displaying search results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    #region Using Directives

    using System.Collections.Generic;
    using System.ComponentModel;

    using TVSorter.Model;
    using TVSorter.View;

    #endregion

    /// <summary>
    /// The controller for displaying search results.
    /// </summary>
    public class SearchResultsController : ShowSearchController
    {
        #region Fields

        /// <summary>
        ///   The folder name that was searched for.
        /// </summary>
        private string folderName;

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
            base.Initialise(view);
            this.CloseButtonText = "Skip";
        }

        /// <summary>
        /// Selects the specified index as the desired result.
        /// </summary>
        /// <param name="index">
        /// The index to select. 
        /// </param>
        public override void Select(int index)
        {
            this.SearchResults[index].FolderName = this.folderName;
            base.Select(index);
        }

        /// <summary>
        /// Sets the results for the specified search term.
        /// </summary>
        /// <param name="name">
        /// The name of the show searched for. 
        /// </param>
        /// <param name="searchResults">
        /// The list of possible results. 
        /// </param>
        public void SetResults(string name, IList<TvShow> searchResults)
        {
            this.folderName = name;
            this.Title = "Search Results: " + name;
            this.SearchResults = new BindingList<TvShow>(searchResults);
        }

        #endregion
    }
}