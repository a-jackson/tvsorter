// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileResult.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.Types
{
    #region Using Directives

    using System.IO;

    #endregion

    /// <summary>
    /// The file result.
    /// </summary>
    public class FileResult
    {
        #region Public Properties

        /// <summary>
        ///   Gets or sets Episode.
        /// </summary>
        public Episode Episode { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the result is incomplete.
        /// </summary>
        public bool Incomplete { get; set; }

        /// <summary>
        ///   Gets or sets InputFile.
        /// </summary>
        public FileInfo InputFile { get; set; }

        /// <summary>
        ///   Gets or sets OutputPath.
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        ///   Gets or sets Show.
        /// </summary>
        public TvShow Show { get; set; }

        #endregion
    }
}