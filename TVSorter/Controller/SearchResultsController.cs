// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="SearchResultsController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The controller for displaying search results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.ComponentModel;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     The controller for displaying search results.
    /// </summary>
    public class SearchResultsController : ShowSearchController
    {
        /// <summary>
        ///     The folder name that was searched for.
        /// </summary>
        private string folderName;

        /// <summary>
        ///     Initialises a new instance of the <see cref="SearchResultsController" /> class.
        /// </summary>
        /// <param name="tvShowRepository">The TV Show Repository.</param>
        public SearchResultsController(ITvShowRepository tvShowRepository)
            : base(tvShowRepository)
        {
        }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            base.Initialise(view);
            CloseButtonText = "Skip";
        }

        /// <summary>
        ///     Selects the specified index as the desired result.
        /// </summary>
        /// <param name="index">
        ///     The index to select.
        /// </param>
        public override void Select(int index)
        {
            SearchResults[index].FolderName = folderName;
            base.Select(index);
        }

        /// <summary>
        ///     Sets the results for the specified search term.
        /// </summary>
        /// <param name="name">
        ///     The name of the show searched for.
        /// </param>
        /// <param name="searchResults">
        ///     The list of possible results.
        /// </param>
        public void SetResults(string name, IList<TvShow> searchResults)
        {
            folderName = name;
            Title = "Search Results: " + name;
            SearchResults = new BindingList<TvShow>(searchResults);
        }
    }
}
