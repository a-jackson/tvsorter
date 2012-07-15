// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Episode.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The episode.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Xml.Linq;

    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// The episode.
    /// </summary>
    public class Episode : IEquatable<Episode>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Episode"/> class.
        /// </summary>
        internal Episode()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Episode"/> class from XML.
        /// </summary>
        /// <param name="element">
        /// The episode element.
        /// </param>
        internal Episode(XElement element)
        {
            FromXml(element, this);
        }

        #region Public Properties

        /// <summary>
        ///   Gets or sets EpisodeNumber.
        /// </summary>
        public int EpisodeNumber { get; set; }

        /// <summary>
        ///   Gets or sets FileCount.
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        ///   Gets or sets FirstAir.
        /// </summary>
        public DateTime FirstAir { get; set; }

        /// <summary>
        ///   Gets or sets Name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///   Gets or sets SeasonNumber.
        /// </summary>
        public int SeasonNumber { get; set; }

        /// <summary>
        /// Gets or sets the episodes show.
        /// </summary>
        public TvShow Show { get; set; }

        /// <summary>
        ///   Gets or sets TvdbId.
        /// </summary>
        public string TvdbId { get; set; }

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

            if (!(obj is Episode))
            {
                return false;
            }

            return this.Equals((Episode)obj);
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

        /// <summary>
        /// Saves the episode.
        /// </summary>
        public void Save()
        {
            var xml = new Xml();
            xml.SaveEpisode(this);
        }

        /// <summary>
        /// Loads the episode from XML.
        /// </summary>
        /// <param name="episodeNode">The Episode element to load.</param>
        /// <param name="episode">The episode to load into.</param>
        internal static void FromXml(XElement episodeNode, Episode episode)
        {
            episode.Name = episodeNode.GetAttribute("name", string.Empty);
            episode.TvdbId = episodeNode.GetAttribute("tvdbid", string.Empty);
            episode.SeasonNumber = int.Parse(episodeNode.GetAttribute("seasonnum", "-1"));
            episode.EpisodeNumber = int.Parse(episodeNode.GetAttribute("episodenum", "-1"));
            episode.FirstAir = DateTime.Parse(episodeNode.GetAttribute("firstair", "1970-01-01 00:00:00"));
            episode.FileCount = int.Parse(episodeNode.GetAttribute("filecount", "0"));
        }

        /// <summary>
        /// Converts the episode to XML.
        /// </summary>
        /// <returns>The XElement.</returns>
        internal XElement ToXml()
        {
            var episodeElement = new XElement(
                Xml.GetName("Episode"),
                new XAttribute("name", this.Name),
                new XAttribute("tvdbid", this.TvdbId),
                new XAttribute("seasonnum", this.SeasonNumber),
                new XAttribute("episodenum", this.EpisodeNumber),
                new XAttribute("firstair", this.FirstAir),
                new XAttribute("filecount", this.FileCount));
            return episodeElement;
        }

        #endregion
    }
}