// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface of a file scan manager.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The public interface of a file scan manager.
    /// </summary>
    public interface IScanManager
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a log message occurs.
        /// </summary>
        event EventHandler<LogMessageEventArgs> LogMessage;

        /// <summary>
        ///   Occurs when the progress of an operation changes.
        /// </summary>
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        ///   Occurs when a refresh operation is completed.
        /// </summary>
        event EventHandler<RefreshCompleteEventArgs> RefreshComplete;

        /// <summary>
        ///   Occurs when a refresh file counts operation is completed.
        /// </summary>
        event EventHandler<RefreshFileCountsCompleteEventArgs> RefreshFileCountsComplete;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Refresh the specified sub directory.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to refresh. 
        /// </param>
        /// <returns>
        /// The list of files indentified during the refresh operation. 
        /// </returns>
        List<FileResult> Refresh(string subDirectory);

        /// <summary>
        /// Refreshes the specified sub directory asynchronously.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to refresh. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        void RefreshAsync(string subDirectory, object userState);

        /// <summary>
        /// Searches for files in the output directory to set the file counts asynchronously.
        /// </summary>
        /// <param name="userState">
        /// The user's state oject. 
        /// </param>
        void RefreshFileCountsAsync(object userState);

        /// <summary>
        /// Resets the episode of the specified file.
        /// </summary>
        /// <param name="result">
        /// The file to reset. 
        /// </param>
        void ResetEpisode(FileResult result);

        /// <summary>
        /// Resets the episode of the specified file.
        /// </summary>
        /// <param name="result">
        /// The file to reset. 
        /// </param>
        /// <param name="seasonNum">
        /// The season number to set it to. 
        /// </param>
        /// <param name="episodeNum">
        /// The episode number to set it to. 
        /// </param>
        void ResetEpsiode(FileResult result, int seasonNum, int episodeNum);

        #endregion
    }
}