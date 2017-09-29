// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for handle show searches.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Controller for handle show searches.
    /// </summary>
    public abstract class ShowSearchController : ControllerBase
    {
        /// <summary>
        ///     The TV show repository.
        /// </summary>
        private readonly ITvShowRepository tvShowRepository;

        /// <summary>
        ///     The text for the close button.
        /// </summary>
        private string closeButtonText;

        /// <summary>
        ///     The list of results.
        /// </summary>
        private BindingList<TvShow> searchResults;

        /// <summary>
        ///     The title of the form.
        /// </summary>
        private string title;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ShowSearchController" /> class.
        /// </summary>
        /// <param name="tvShowRepository">The TV Show Repository.</param>
        public ShowSearchController(ITvShowRepository tvShowRepository)
        {
            this.tvShowRepository = tvShowRepository;
        }

        /// <summary>
        ///     Gets or sets the close button text.
        /// </summary>
        public string CloseButtonText
        {
            get => closeButtonText;

            protected set
            {
                closeButtonText = value;
                OnPropertyChanged("CloseButtonText");
            }
        }

        /// <summary>
        ///     Gets or sets SearchResults.
        /// </summary>
        public BindingList<TvShow> SearchResults
        {
            get => searchResults;

            protected set
            {
                searchResults = value;
                OnPropertyChanged("SearchResults");
            }
        }

        /// <summary>
        ///     Gets or sets the title of the form.
        /// </summary>
        public string Title
        {
            get => title;

            protected set
            {
                title = value;
                OnPropertyChanged("Title");
            }
        }

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
        }

        /// <summary>
        ///     Searches for the specified name.
        /// </summary>
        /// <param name="name">
        ///     The name to search for.
        /// </param>
        public void Search(string name)
        {
            SearchResults = new BindingList<TvShow>(tvShowRepository.SearchShow(name));
        }

        /// <summary>
        ///     Selects the specified index as the chosen result.
        /// </summary>
        /// <param name="index">
        ///     The index to select.
        /// </param>
        public virtual void Select(int index)
        {
            var show = tvShowRepository.FromSearchResult(SearchResults[index]);
            tvShowRepository.Save(show);
        }
    }
}
