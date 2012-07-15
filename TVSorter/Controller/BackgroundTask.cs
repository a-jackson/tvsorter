// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundTask.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A task that runs in the background
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// A task that runs in the background
    /// </summary>
    public class BackgroundTask : IProgressTask
    {
        #region Fields

        /// <summary>
        /// The task that should be run.
        /// </summary>
        private readonly Action action;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BackgroundTask"/> class.
        /// </summary>
        /// <param name="action">
        /// The method to run.
        /// </param>
        public BackgroundTask(Action action)
        {
            this.action = action;
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the task is complete.
        /// </summary>
        public event EventHandler TaskComplete;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Starts the task.
        /// </summary>
        public void Start()
        {
            Task task = Task.Factory.StartNew(this.action);
            task.ContinueWith(delegate { this.TaskComplete(this, EventArgs.Empty); });
        }

        #endregion
    }
}