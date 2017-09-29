// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Episode.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The episode.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using TVSorter.Storage;

namespace TVSorter.Model
{
    /// <summary>
    ///     The episode.
    /// </summary>
    public class Episode : IEquatable<Episode>
    {
        /// <summary>
        ///     Initialises a new instance of the <see cref="Episode" /> class.
        /// </summary>
        internal Episode()
        {
        }

        /// <summary>
        ///     Gets or sets EpisodeNumber.
        /// </summary>
        public int EpisodeNumber { get; set; }

        /// <summary>
        ///     Gets or sets FileCount.
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        ///     Gets or sets FirstAir.
        /// </summary>
        public DateTime FirstAir { get; set; }

        /// <summary>
        ///     Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets SeasonNumber.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        ///     Gets or sets the episodes show.
        /// </summary>
        public TvShow Show { get; set; }

        /// <summary>
        ///     Gets or sets the TVDB ID.
        /// </summary>
        public string TvdbId { get; set; }

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">
        ///     An object to compare with this object.
        /// </param>
        public bool Equals(Episode other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(other.TvdbId, TvdbId);
        }

        /// <summary>
        ///     Saves the episode.
        /// </summary>
        /// <param name="provider">
        ///     The provider to save the episode to.
        /// </param>
        internal void Save(IStorageProvider provider)
        {
            provider.SaveEpisode(this);
        }

        /// <summary>
        ///     Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        ///     <see cref="T:System.Object" /> .
        /// </summary>
        /// <returns>
        ///     true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" /> ;
        ///     otherwise, false.
        /// </returns>
        /// <param name="obj">
        ///     The <see cref="T:System.Object" /> to compare with the current <see cref="T:System.Object" /> .
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

            if (!(obj is Episode))
            {
                return false;
            }

            return Equals((Episode)obj);
        }

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        ///     A hash code for the current <see cref="T:System.Object" /> .
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            return TvdbId != null ? TvdbId.GetHashCode() : 0;
        }
    }
}
