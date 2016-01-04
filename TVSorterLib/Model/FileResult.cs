// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileResult.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using TVSorter.Storage;
    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// The file result.
    /// </summary>
    public class FileResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult"/> class.
        /// </summary>
        internal FileResult()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets an example result.
        /// </summary>
        public static FileResult Example
        {
            get
            {
                var result = new FileResult
                    {
                        Episode =
                            new Episode
                                {
                                    EpisodeNumber = 1, 
                                    SeasonNumber = 1, 
                                    FirstAir = DateTime.Today, 
                                    Name = "Episode Name"
                                }, 
                        Show = new TvShow { Name = "Show Name", FolderName = "Show Name", UseCustomFormat = false }, 
                        InputFile = new FileInfoWrap("Show Name.S01E01.avi")
                    };
                result.Episodes = new List<Episode> { result.Episode };
                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the result is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        ///   Gets the file's episode match or first episode if there is more than one.
        /// </summary>
        public Episode Episode { get; internal set; }

        /// <summary>
        /// Gets the files's episodes matches.
        /// </summary>
        public IList<Episode> Episodes { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the result is incomplete.
        /// </summary>
        public bool Incomplete
        {
            get
            {
                return this.Show == null | this.Episode == null;
            }
        }

        /// <summary>
        ///   Gets the file.
        /// </summary>
        public IFileInfo InputFile { get; internal set; }

        /// <summary>
        ///   Gets the file's show.
        /// </summary>
        public TvShow Show { get; internal set; }

        /// <summary>
        /// Gets the name of the show as seen by the program.
        /// </summary>
        public string ShowName { get; internal set; }

        #endregion
        
        #region Methods

        /// <summary>
        /// Determines the file's name contains any of the specified keywords.
        /// </summary>
        /// <param name="keywords">
        /// The keywords to check for.
        /// </param>
        /// <returns>
        /// A value indicating whether the file name contains the keywords.
        /// </returns>
        internal bool ContainsKeyword(IEnumerable<string> keywords)
        {
            return keywords.Any(keyword => this.InputFile.Name.ToLower().Contains(keyword.ToLower()));
        }
        
        #endregion
    }
}