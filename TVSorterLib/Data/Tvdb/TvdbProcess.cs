// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvdbProcess.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The tvdb process.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Data.Tvdb
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;

    #endregion

    /// <summary>
    /// The tvdb process.
    /// </summary>
    internal class TvdbProcess
    {
        #region Public Events

        /// <summary>
        ///   Occurs when a banner download is required.
        /// </summary>
        public event EventHandler<BannerDownloadRequiredEventArgs> BannerDownloadRequired;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Processes the results of a search.
        /// </summary>
        /// <param name="xml">
        /// The xml that has been downloaded. 
        /// </param>
        /// <param name="name">
        /// The name of the show that has been searched. 
        /// </param>
        /// <returns>
        /// The list of results. 
        /// </returns>
        public List<TvShow> ProcessSearch(TextReader xml, string name)
        {
            XDocument doc = XDocument.Load(xml);
            return
                doc.Descendants("Series").Select(
                    series =>
                    new TvShow
                        {
                            Name = this.GetElementValue(series, "SeriesName", string.Empty), 
                            TvdbId = this.GetElementValue(series, "seriesid", string.Empty), 
                            FolderName = name
                        }).ToList();
        }

        /// <summary>
        /// Processes a cached XML file to extract all the episodes and save them.
        /// </summary>
        /// <param name="show">
        /// The show to process. 
        /// </param>
        /// <param name="filePath">
        /// The path to its XML file. 
        /// </param>
        /// <param name="serverTime">
        /// The time of the server. 
        /// </param>
        public void ProcessShow(TvShow show, string filePath, DateTime serverTime)
        {
            if (show == null)
            {
                throw new ArgumentNullException("show");
            }

            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("No file can be found at the specified file path.");
            }

            XDocument doc = XDocument.Load(filePath);
            XElement data = doc.Root;
            if (data == null)
            {
                throw new Exception("XML is invalid");
            }

            XElement series = data.Element("Series");
            if (series == null)
            {
                throw new Exception("XML is invalid");
            }

            XElement banner = series.Element("banner");

            // Check that the banner exists in the file, and that the file doesn't exist or the banner is different to the downloaded one.
            if (banner != null
                && (!File.Exists(Tvdb.ImageDirectory + show.TvdbId + ".jpg") || banner.Value != show.Banner))
            {
                this.OnBannerDownloadRequired(show, banner.Value);
            }

            string episodeElement = "EpisodeNumber";
            string seasonElement = "SeasonNumber";

            if (show.UseDvdOrder)
            {
                episodeElement = "DVD_episodenumber";
                seasonElement = "DVD_season";
            }

            List<Episode> newEpisodes = (from episode in data.Descendants("Episode")
                                         let episodeNum = this.GetElementValue(episode, episodeElement, "-1")
                                         let seasonNum = this.GetElementValue(episode, seasonElement, "-1")
                                         select
                                             new Episode
                                                 {
                                                     TvdbId = this.GetElementValue(episode, "id", string.Empty), 
                                                     EpisodeNumber = this.ParseInt(episodeNum), 
                                                     SeasonNumber = this.ParseInt(seasonNum), 
                                                     FirstAir =
                                                         DateTime.Parse(
                                                             this.GetElementValue(episode, "FirstAired", "1970-01-01")), 
                                                     Name = this.GetElementValue(episode, "EpisodeName", string.Empty), 
                                                     Show = show, 
                                                 }).ToList();

            // Copy the episodes filecount across
            if (show.Episodes != null)
            {
                foreach (Episode episode in newEpisodes)
                {
                    Episode currentEpisode = show.Episodes.FirstOrDefault(x => x.Equals(episode));
                    if (currentEpisode != null)
                    {
                        episode.FileCount = currentEpisode.FileCount;
                    }
                }
            }

            show.Episodes = newEpisodes;
            show.LastUpdated = serverTime;

            Logger.OnLogMessage(this, "Updated show {0}. Has {1} episodes.", show.Name, newEpisodes.Count);
        }

        /// <summary>
        /// Processes the updates XML to get the list of IDs that have updated.
        /// </summary>
        /// <param name="xml">
        /// The XML to process. 
        /// </param>
        /// <returns>
        /// The collection of TVDB IDs that have updates. 
        /// </returns>
        public IEnumerable<string> ProcessUpdates(StringReader xml)
        {
            XDocument doc = XDocument.Load(xml);

            return doc.Descendants("Series").Select(x => x.Value);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value from the specified child element.
        /// </summary>
        /// <param name="element">
        /// The element. 
        /// </param>
        /// <param name="child">
        /// The name of the child. 
        /// </param>
        /// <param name="defaultValue">
        /// The default value. 
        /// </param>
        /// <returns>
        /// The value of the element. 
        /// </returns>
        private string GetElementValue(XElement element, string child, string defaultValue)
        {
            if (element == null || string.IsNullOrWhiteSpace(child))
            {
                return defaultValue;
            }

            XElement childElement = element.Element(child);
            return childElement == null || string.IsNullOrWhiteSpace(childElement.Value)
                       ? defaultValue
                       : childElement.Value;
        }

        /// <summary>
        /// Raises a banner download required event.
        /// </summary>
        /// <param name="show">
        /// The show that requires a banner. 
        /// </param>
        /// <param name="newBanner">
        /// The new banner to download. 
        /// </param>
        private void OnBannerDownloadRequired(TvShow show, string newBanner)
        {
            if (this.BannerDownloadRequired != null)
            {
                this.BannerDownloadRequired(this, new BannerDownloadRequiredEventArgs(show, newBanner));
            }
        }

        /// <summary>
        /// Parses an int and returns the default value if unsuccessful.
        /// </summary>
        /// <param name="str">
        /// The string to parse. 
        /// </param>
        /// <param name="defaultValue">
        /// The default value to return. 
        /// </param>
        /// <returns>
        /// The parsed int. 
        /// </returns>
        private int ParseInt(string str, int defaultValue = -1)
        {
            int num;
            if (int.TryParse(str, out num))
            {
                return num;
            }

            return defaultValue;
        }

        #endregion
    }
}