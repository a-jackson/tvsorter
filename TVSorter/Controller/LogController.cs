// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="LogController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for the Log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using TVSorter.View;

namespace TVSorter.Controller
{
    /// <summary>
    ///     Controller for the Log.
    /// </summary>
    public class LogController : ControllerBase
    {
        /// <summary>
        ///     A value indicating whether the controller has been initialised.
        /// </summary>
        private bool initialised;

        /// <summary>
        ///     Initialises a new instance of the <see cref="LogController" /> class.
        /// </summary>
        public LogController()
        {
            Log = new List<LogMessageEventArgs>();
            Logger.LogMessage += OnLogMessageReceived;
            initialised = false;
        }

        /// <summary>
        ///     Gets the list of log messages.
        /// </summary>
        public List<LogMessageEventArgs> Log { get; }

        /// <summary>
        ///     Occurs when a log message is received.
        /// </summary>
        public event EventHandler<LogMessageEventArgs> LogMessageReceived;

        /// <summary>
        ///     Initialises the controller.
        /// </summary>
        /// <param name="view">
        ///     The view the controller is for.
        /// </param>
        public override void Initialise(IView view)
        {
            initialised = true;
        }

        /// <summary>
        ///     Raises a log message received event.
        /// </summary>
        /// <param name="e">
        ///     The event args.
        /// </param>
        private void OnLogMessageReceived(LogMessageEventArgs e)
        {
            if (LogMessageReceived != null)
            {
                LogMessageReceived(this, e);
            }
        }

        /// <summary>
        ///     Handles a log message being received.
        /// </summary>
        /// <param name="sender">
        ///     The sender of the event.
        /// </param>
        /// <param name="e">
        ///     The arguments of the event.
        /// </param>
        private void OnLogMessageReceived(object sender, LogMessageEventArgs e)
        {
            // Cache the messages in a list until the controller has been initialised.
            // Then the message can be sent directly to the view.
            if (!initialised)
            {
                Log.Add(e);
            }
            else
            {
                OnLogMessageReceived(e);
            }
        }
    }
}
