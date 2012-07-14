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
            this.progressTask.ProgressChanged += this.OnProgressTaskOnProgressChanged;
            this.progressTask.TaskComplete += this.OnProgressTaskOnTaskComplete;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles the task progress changing.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void OnProgressTaskOnProgressChanged(object sender, EventArgs e)
        {
            this.Invoke(
                new Action(
                    () =>
                        {
                            this.taskProgress.Maximum = this.progressTask.MaxValue;
                            this.taskProgress.Value = this.progressTask.Value;
                        }));
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
            this.progressTask.ProgressChanged -= this.OnProgressTaskOnProgressChanged;
            this.progressTask.TaskComplete -= this.OnProgressTaskOnTaskComplete;
            this.Invoke(new Action(this.Close));
        }

        /// <summary>
        /// Handles the Progress dialog loading.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private void ProgressDialogLoad(object sender, EventArgs e)
        {
            this.taskProgress.Maximum = this.progressTask.MaxValue;
            this.taskProgress.Value = 0;
        }

        #endregion
    }
}