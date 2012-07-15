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
        /// The message. 
        /// </param>
        /// <param name="time">
        /// The time. 
        /// </param>
        public LogMessageEventArgs(string message, DateTime time)
        {
            this.LogTime = time;
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