// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IProgressTask.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface of the progress task.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Controller
{
    #region Using Directives

    using System;

    #endregion

    /// <summary>
    /// The public interface of the progress task.
    /// </summary>
    public interface IProgressTask
    {
        #region Public Events

        /// <summary>
        ///   Occurs when the task is complete.
        /// </summary>
        event EventHandler TaskComplete;

        #endregion
    }
}