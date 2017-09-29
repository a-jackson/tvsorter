// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="AddShowController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for adding new shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using TVSorter.Repostitory;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Controller for adding new shows.
    /// </summary>
    public class AddShowController : ShowSearchController
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="AddShowController" /> class.
        /// </summary>
        /// <param name="tvShowRepository">The TV Show Repository.</param>
        public AddShowController(ITvShowRepository tvShowRepository)
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
            Title = "Add Show";
            CloseButtonText = "Close";
        }
    }
}
