// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="UpdateShowsCompletedEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event arguments for a update shows complete event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The event arguments for a update shows complete event.
    /// </summary>
    public class UpdateShowsCompletedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateShowsCompletedEventArgs"/> class. Initialises a new instance of the <see cref="UpdateShowsCompletedEventArgs"/> class.
        /// </summary>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        public UpdateShowsCompletedEventArgs(object userState)
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