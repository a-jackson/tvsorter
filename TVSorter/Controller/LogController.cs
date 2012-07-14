// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="LogController.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Controller for the Log.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Controller
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.View;

    #endregion

    /// <summary>
    /// Controller for the Log.
    /// </summary>
    public class LogController : ControllerBase
    {
        #region Fields

        /// <summary>
        ///   A value indicating whether the controller has been initialised.
        /// </summary>
        private bool initialised;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="LogController" /> class.
        /// </summary>
        public LogController()
        {
            this.Log = new List<string>();
            Factory.DataProvider.LogMessage += this.OnLogMessageReceived;
            Factory.Scanner.LogMessage += this.OnLogMessageReceived;
            Factory.FileManager.LogMessage += this.OnLogMessageReceived;
            this.initialised = false;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when a log message is received.
        /// </summary>
        public event EventHandler<LogMessageEventArgs> LogMessageReceived;

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the list of log messages.
        /// </summary>
        public List<string> Log { get; private set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Initialises the controller.
        /// </summary>
        /// <param name="view">
        /// The view the controller is for. 
        /// </param>
        public override void Initialise(IView view)
        {
            this.initialised = true;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises a log message received event.
        /// </summary>
        /// <param name="e">
        /// The event args. 
        /// </param>
        private void OnLogMessageReceived(LogMessageEventArgs e)
        {
            if (this.LogMessageReceived != null)
            {
                this.LogMessageReceived(this, e);
            }
        }

        /// <summary>
        /// Handles a log message being recieved.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnLogMessageReceived(object sender, LogMessageEventArgs e)
        {
            // Cache the messages in a list until the controller has been initialised.
            // Then the message can be sent directly to the view.
            if (!this.initialised)
            {
                this.Log.Add(e.ToString());
            }
            else
            {
                this.OnLogMessageReceived(e);
            }
        }

        #endregion
    }
}