// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IFileManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The public interface of the file manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Files
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The public interface of the file manager.
    /// </summary>
    public interface IFileManager
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a copy file operation completes.
        /// </summary>
        event EventHandler<FileOperationEventArgs> CopyFileComplete;

        /// <summary>
        ///   Occurs when a log message occurs.
        /// </summary>
        event EventHandler<LogMessageEventArgs> LogMessage;

        /// <summary>
        ///   Occurs when a move file operation completes.
        /// </summary>
        event EventHandler<FileOperationEventArgs> MoveFileComplete;

        /// <summary>
        ///   Occurs when progress changing on a running operation.
        /// </summary>
        event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Performs a copy file operation.
        /// </summary>
        /// <param name="files">
        /// The files to copy. 
        /// </param>
        void CopyFile(List<FileResult> files);

        /// <summary>
        /// Performs a copy file operation on the specified files asynchronously.
        /// </summary>
        /// <param name="files">
        /// The files to copy. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        void CopyFileAsync(List<FileResult> files, object userState);

        /// <summary>
        /// Performs a move file operation on the specified files.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        void MoveFile(List<FileResult> files);

        /// <summary>
        /// Performs a move file operation on the specfied files asynchronously.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        void MoveFileAsync(List<FileResult> files, object userState);

        #endregion
    }
}