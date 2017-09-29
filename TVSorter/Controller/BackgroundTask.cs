// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BackgroundTask.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A task that runs in the background
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSorter.Controller
{
    /// <summary>
    ///     A task that runs in the background
    /// </summary>
    public class BackgroundTask : IProgressTask
    {
        /// <summary>
        ///     The task that should be run.
        /// </summary>
        private readonly Action action;

        /// <summary>
        ///     Initialises a new instance of the <see cref="BackgroundTask" /> class.
        /// </summary>
        /// <param name="action">
        ///     The method to run.
        /// </param>
        public BackgroundTask(Action action)
        {
            this.action = action;
        }

        /// <summary>
        ///     Occurs when the task is complete.
        /// </summary>
        public event EventHandler TaskComplete;

        /// <summary>
        ///     Starts the task.
        /// </summary>
        public void Start()
        {
            var task = Task.Factory.StartNew(
                () =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                });
            task.ContinueWith(
                delegate
                {
                    if (TaskComplete != null)
                    {
                        TaskComplete(this, EventArgs.Empty);
                    }
                });
        }
    }
}
