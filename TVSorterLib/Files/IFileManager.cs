// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="IFileManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The interface for the file manager.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Files
{
    #region Using Directives

    using System.Collections.Generic;

    using Model;
    
    #endregion

    /// <summary>
    /// The interface for the file manager.
    /// </summary>
    public interface IFileManager
    {
        /// <summary>
        /// Performs a copy file operation.
        /// </summary>
        /// <param name="files">
        /// The files to copy. 
        /// </param>
        void CopyFile(IEnumerable<FileResult> files);

        /// <summary>
        /// Performs a move file operation.
        /// </summary>
        /// <param name="files">
        /// The files to move. 
        /// </param>
        void MoveFile(IEnumerable<FileResult> files);
    }
}