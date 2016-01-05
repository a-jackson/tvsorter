// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="AddShowController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for adding new shows.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    using Repostitory;
    #region Using Directives

    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for adding new shows.
    /// </summary>
    public class AddShowController : ShowSearchController
    {
        public AddShowController(ITvShowRepository tvShowRepository) : base(tvShowRepository)
        {
        }

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
            this.Title = "Add Show";
            this.CloseButtonText = "Close";
        }

        #endregion
    }
}