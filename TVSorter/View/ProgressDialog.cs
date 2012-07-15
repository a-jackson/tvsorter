// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ProgressDialog.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The dialog showing the progress bar.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.View
{
    #region Using Directives

    using System;
    using System.Windows.Forms;

    using TVSorter.Controller;

    #endregion

    /// <summary>
    /// The dialog showing the progress bar.
    /// </summary>
    public partial class ProgressDialog : Form
    {
        #region Fields

        /// <summary>
        ///   That task that the dialog is showing the progress for.
        /// </summary>
        private readonly IProgressTask progressTask;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressDialog"/> class.
        /// </summary>
        /// <param name="task">
        /// The task. 
        /// </param>
        public ProgressDialog(IProgressTask task)
        {
            this.InitializeComponent();
            this.progressTask = task;
            this.progressTask.TaskComplete += this.OnProgressTaskOnTaskComplete;
            Logger.LogMessage += this.OnLogMessage;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the receipt of a log message.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="e">
        /// The arguments of the event.
        /// </param>
        private void OnLogMessage(object sender, LogMessageEventArgs e)
        {
            this.Invoke(new Action(() => this.log.TopIndex = this.log.Items.Add(e.ToString())));
        }

        /// <summary>
        /// Handles the completion of the task.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnProgressTaskOnTaskComplete(object sender, EventArgs e)
        {
            this.progressTask.TaskComplete -= this.OnProgressTaskOnTaskComplete;
            Logger.LogMessage -= this.OnLogMessage;
            this.Invoke(new Action(this.Close));
        }

        #endregion
    }
}