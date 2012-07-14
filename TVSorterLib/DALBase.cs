// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="DALBase.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Defines the DALBase type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// Defines the DALBase type.
    /// </summary>
    public abstract class DalBase
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a log message is raised.
        /// </summary>
        public event EventHandler<LogMessageEventArgs> LogMessage;

        #endregion

        #region Methods

        /// <summary>
        /// Raises a log message event.
        /// </summary>
        /// <param name="message">
        /// The log message. 
        /// </param>
        /// <param name="args">
        /// The string format arguments of the log message. 
        /// </param>
        protected void OnLogMessage(string message, params object[] args)
        {
            message = string.Format(message, args);
            if (this.LogMessage != null)
            {
                this.LogMessage(this, new LogMessageEventArgs(message, DateTime.Now));
            }
        }

        #endregion
    }
}