// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Handles log messages from the library.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System;

    /// <summary>
    /// Handles log messages from the library.
    /// </summary>
    public static class Logger
    {
        #region Public Events

        /// <summary>
        /// Occurs when there is a log message.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs> LogMessage;

        #endregion

        #region Methods

        /// <summary>
        /// Fires a log message.
        /// </summary>
        /// <param name="sender">
        /// The sender of the message.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        internal static void OnLogMessage(object sender, string message)
        {
            if (LogMessage != null)
            {
                LogMessage(sender, new LogMessageEventArgs(message, DateTime.Now));
            }
        }

        #endregion
    }
}