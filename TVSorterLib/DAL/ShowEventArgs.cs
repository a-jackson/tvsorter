// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowEventArgs.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The event args for a show event.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// The event args for a show event.
    /// </summary>
    public class ShowEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowEventArgs"/> class.
        /// </summary>
        /// <param name="show">
        /// The show. 
        /// </param>
        public ShowEventArgs(TvShow show)
        {
            this.Show = show;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets the show the event is for.
        /// </summary>
        public TvShow Show { get; private set; }

        #endregion
    }
}