// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ProgressChangedEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for the process changed event.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The event args for the process changed event.
    /// </summary>
    public class ProgressChangedEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressChangedEventArgs"/> class.
        /// </summary>
        /// <param name="maxValue">
        /// The max value. 
        /// </param>
        /// <param name="value">
        /// The value. 
        /// </param>
        /// <param name="userState">
        /// The user state. 
        /// </param>
        public ProgressChangedEventArgs(int maxValue, int value, object userState)
        {
            this.MaxValue = maxValue;
            this.Value = value;
            this.UserState = userState;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the max value of the progress.
        /// </summary>
        public int MaxValue { get; private set; }

        /// <summary>
        ///   Gets the user's state object.
        /// </summary>
        public object UserState { get; private set; }

        /// <summary>
        ///   Gets the value of the progress.
        /// </summary>
        public int Value { get; private set; }

        #endregion
    }
}