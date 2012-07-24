// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="LogMessageEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The log message event args.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The log message event args.
    /// </summary>
    public class LogMessageEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessageEventArgs"/> class. Initialises a new instance of the <see cref="LogMessageEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        ///   The message. 
        /// </param>
        /// <param name="time">
        ///   The time. 
        /// </param>
        /// <param name="type"> The type of the log message.</param>
        public LogMessageEventArgs(string message, DateTime time, LogType type)
        {
            this.LogTime = time;
            this.Type = type;
            this.LogMessage = message;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the log message of the event.
        /// </summary>
        public string LogMessage { get; private set; }

        /// <summary>
        ///   Gets the time the event was raised.
        /// </summary>
        public DateTime LogTime { get; private set; }

        /// <summary>
        /// Gets the type of the log message.
        /// </summary>
        public LogType Type { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/> .
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/> . 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString()
        {
            return string.Format("{0:HH:mm:ss}: {1}", this.LogTime, this.LogMessage);
        }

        #endregion
    }
}