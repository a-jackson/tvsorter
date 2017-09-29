// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IView.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface of a view.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using TVSorter.Controller;

namespace TVSorter.View
{
    /// <summary>
    ///     The public interface of a view.
    /// </summary>
    public interface IView : ISynchronizeInvoke
    {
        /// <summary>
        ///     Starts the progress indication for the specified Project Task.
        /// </summary>
        /// <param name="task">
        ///     The task.
        /// </param>
        /// <param name="taskName">
        ///     The task name.
        /// </param>
        void StartTaskProgress(IProgressTask task, string taskName);
    }
}
