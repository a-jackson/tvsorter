// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="RefreshFileCountsCompleteEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for the RefreshFileCountsComplete event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Scanning
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The event args for the RefreshFileCountsComplete event.
    /// </summary>
    public class RefreshFileCountsCompleteEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshFileCountsCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        public RefreshFileCountsCompleteEventArgs(object userState)
        {
            this.UserState = userState;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the user's state object.
        /// </summary>
        public object UserState { get; private set; }

        #endregion
    }
}