// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Logger.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Handles log messages from the library.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace TVSorter
{
    /// <summary>
    ///     Handles log messages from the library.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        ///     Occurs when there is a log message.
        /// </summary>
        public static event EventHandler<LogMessageEventArgs> LogMessage;

        /// <summary>
        ///     Fires a log message.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the message.
        /// </param>
        /// <param name="message">
        ///     The message.
        /// </param>
        /// <param name="type">
        ///     The type of the message.
        /// </param>
        /// <param name="args">
        ///     The string format args.
        /// </param>
        internal static void OnLogMessage(object sender, string message, LogType type, params object[] args)
        {
            message = string.Format(message, args);
            if (LogMessage != null)
            {
                LogMessage(sender, new LogMessageEventArgs(message, DateTime.Now, type));
            }
        }
    }
}
