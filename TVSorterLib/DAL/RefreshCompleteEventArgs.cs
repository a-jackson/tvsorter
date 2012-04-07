// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="RefreshCompleteEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for the refresh complete event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The event args for the refresh complete event.
    /// </summary>
    public class RefreshCompleteEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshCompleteEventArgs"/> class.
        /// </summary>
        /// <param name="results">
        /// The results. 
        /// </param>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        public RefreshCompleteEventArgs(List<FileResult> results, object userState)
        {
            this.Results = results;
            this.UserState = userState;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the results of the refresh.
        /// </summary>
        public List<FileResult> Results { get; private set; }

        /// <summary>
        ///   Gets the user's state object.
        /// </summary>
        public object UserState { get; private set; }

        #endregion
    }
}