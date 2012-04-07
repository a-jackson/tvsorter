// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowUpdatedEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for the Show update event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The event args for the Show update event.
    /// </summary>
    public class ShowUpdatedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="show">
        /// The show. 
        /// </param>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        public ShowUpdatedEventArgs(TvShow show, object userState)
        {
            this.Show = show;
            this.UserState = userState;
            this.Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowUpdatedEventArgs"/> class.
        /// </summary>
        /// <param name="show">
        /// The show. 
        /// </param>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        /// <param name="error">
        /// The error. 
        /// </param>
        public ShowUpdatedEventArgs(TvShow show, object userState, Exception error)
        {
            this.Show = show;
            this.UserState = userState;
            this.Error = error;
            this.Success = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the error of the operation.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        ///   Gets the show that was updated.
        /// </summary>
        public TvShow Show { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the operation was a success.
        /// </summary>
        public bool Success { get; private set; }

        /// <summary>
        ///   Gets the user's state object.
        /// </summary>
        public object UserState { get; private set; }

        #endregion
    }
}