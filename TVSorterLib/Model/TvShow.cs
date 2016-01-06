// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvShow.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The tv show.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Model
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TVSorter.Data;
    using TVSorter.Files;
    using TVSorter.Storage;
    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// The tv show.
    /// </summary>
    public class TvShow : IEquatable<TvShow>
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TvShow"/> class.
        /// </summary>
        internal TvShow()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///   Gets or sets AlternateNames.
        /// </summary>
        public List<string> AlternateNames { get; set; }

        /// <summary>
        ///   Gets or sets Banner.
        /// </summary>
        public string Banner { get; set; }

        /// <summary>
        ///   Gets or sets CustomFormat.
        /// </summary>
        public string CustomFormat { get; set; }

        /// <summary>
        /// Gets the episodes of the show.
        /// </summary>
        public List<Episode> Episodes { get; internal set; }

        /// <summary>
        ///   Gets or sets FolderName.
        /// </summary>
        public string FolderName { get; set; }

        /// <summary>
        ///   Gets or sets LastUpdated.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Locked.
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets the TVDB ID.
        /// </summary>
        public string TvdbId { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to Use Custom Format.
        /// </summary>
        public bool UseCustomFormat { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether to use DVD Order.
        /// </summary>
        public bool UseDvdOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use a custom destination.
        /// </summary>
        public bool UseCustomDestination { get; set; }

        /// <summary>
        /// Gets or sets the custom output destination directory to use.
        /// </summary>
        public string CustomDestinationDir { get; set; }

        #endregion

        #region Public Methods and Operators
      
        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> .
        /// </summary>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"/> is equal to the current <see cref="T:System.Object"/> ; otherwise, false. 
        /// </returns>
        /// <param name="obj">
        /// The <see cref="T:System.Object"/> to compare with the current <see cref="T:System.Object"/> . 
        /// </param>
        /// <filterpriority>2</filterpriority>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is TvShow))
            {
                return false;
            }

            return this.Equals((TvShow)obj);
        }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false. 
        /// </returns>
        /// <param name="other">
        /// An object to compare with this object. 
        /// </param>
        public bool Equals(TvShow other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.TvdbId, this.TvdbId);
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/> . 
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return this.TvdbId != null ? this.TvdbId.GetHashCode() : 0;
        }
        
        #endregion

        #region Methods
        
        /// <summary>
        /// Gets all the possible names of this show.
        /// </summary>
        /// <returns>The possible names of the show.</returns>
        internal IEnumerable<string> GetShowNames()
        {
            return this.GetNames(new[] { this.Name, this.FolderName }.Concat(this.AlternateNames)).Distinct();
        }

        /// <summary>
        /// Gets the custom destination directory for the show.
        /// </summary>
        /// <returns>The IDirectoryInfo the show's custom directory.</returns>
        internal IDirectoryInfo GetCustomDestinationDirectory()
        {
            // TODO: This needs to be able to return a substituted one for the tests
            return new DirectoryInfoWrap(this.CustomDestinationDir);
        }

        /// <summary>
        /// Initialises the show with default values.
        /// </summary>
        internal void InitialiseDefaultData()
        {
            this.AlternateNames = new List<string>();
            this.Banner = string.Empty;
            this.CustomFormat = string.Empty;
            this.LastUpdated = DateTime.MinValue;
            this.Locked = false;
            this.UseCustomFormat = false;
            this.UseDvdOrder = false;
            this.UseCustomDestination = false;
            this.CustomDestinationDir = string.Empty;
        }
        
        /// <summary>
        /// Gets the names from the specified collection of names.
        /// </summary>
        /// <param name="names">
        /// The names to convert.
        /// </param>
        /// <returns>
        /// The complete list of names including string processing.
        /// </returns>
        private IEnumerable<string> GetNames(IEnumerable<string> names)
        {
            foreach (string name in names.Where(name => name != null))
            {
                yield return name;
                yield return name.GetFileSafeName();
                yield return name.RemoveSpacerChars();
                yield return name.RemoveSpecialChars();
                yield return name.AlphaNumericOnly();
            }
        }

        #endregion
    }
}