// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileOperationEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for a file operation.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The event args for a file operation.
    /// </summary>
    public class FileOperationEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileOperationEventArgs"/> class.
        /// </summary>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        public FileOperationEventArgs(object userState)
        {
            this.UserState = userState;
            this.Successful = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileOperationEventArgs"/> class.
        /// </summary>
        /// <param name="e">
        /// The exception that occured during the operation. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        public FileOperationEventArgs(Exception e, object userState)
        {
            this.Error = e;
            this.UserState = userState;
            this.Successful = false;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the error of the operation.
        /// </summary>
        public Exception Error { get; private set; }

        /// <summary>
        ///   Gets a value indicating whether the operation was sucessful.
        /// </summary>
        public bool Successful { get; private set; }

        /// <summary>
        ///   Gets the user's state object.
        /// </summary>
        public object UserState { get; private set; }

        #endregion
    }
}