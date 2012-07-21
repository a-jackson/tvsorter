// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="TvdbDownload.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Downloads the XML files from the TVDB.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Data.Tvdb
{
    #region Using Directives

    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Xml.Linq;

    using TVSorter.Model;

    #endregion

    /// <summary>
    /// Downloads the XML files from the TVDB.
    /// </summary>
    internal class TvdbDownload
    {
        #region Constants

        /// <summary>
        ///   The TVDB API key.
        /// </summary>
        private const string ApiKey = "D4DCAEBFCA5A6BC1";

        /// <summary>
        ///   The addess of the api on the site.
        /// </summary>
        private const string ApiLoc = "/api/" + ApiKey + "/";

        /// <summary>
        ///   The address to donwload mirrors from.
        /// </summary>
        private const string MirrorsAddress = SiteAddress + ApiLoc + "/mirrors.xml";

        /// <summary>
        ///   The number of times to attempt to retry a download.
        /// </summary>
        private const int RetryCount = 4;

        /// <summary>
        ///   The address of the TVDB.
        /// </summary>
        private const string SiteAddress = "http://www.thetvdb.com/";

        /// <summary>
        ///   The address to download the server time from.
        /// </summary>
        private const string TimeAddress = SiteAddress + "/api/Updates.php?type=none";

        #endregion

        #region Static Fields

        /// <summary>
        ///   The task that determines the mirror.
        /// </summary>
        private static readonly Task<string> MirrorTask;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes static members of the <see cref="TvdbDownload"/> class. 
        /// </summary>
        /// <exception cref="Exception">
        /// Throws an exception if a mirror cannot be found.
        /// </exception>
        static TvdbDownload()
        {
            // Initialise the class on a background thread.
            MirrorTask = Task<string>.Factory.StartNew(InitialiseMirror);
        }

        #endregion

        #region Properties

        /// <summary>
        ///   Gets the mirror to use.
        /// </summary>
        private static string Mirror
        {
            get
            {
                if (MirrorTask.Result == null)
                {
                    throw new Exception("Unable to determine mirror.");
                }

                return MirrorTask.Result;
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Downloads the banner for the specified show.
        /// </summary>
        /// <param name="tvShow">
        /// The show to download the banner for. 
        /// </param>
        public static void DownloadBanner(TvShow tvShow)
        {
            var webClient = new WebClient();
            string bannerAddress = Mirror + "/banners/" + tvShow.Banner;
            string saveAddress = Tvdb.ImageDirectory + tvShow.TvdbId + ".jpg";

            if (!Directory.Exists(Tvdb.ImageDirectory))
            {
                Directory.CreateDirectory(Tvdb.ImageDirectory);
            }

            if (File.Exists(saveAddress))
            {
                File.Delete(saveAddress);
            }

            webClient.DownloadFile(new Uri(bannerAddress), saveAddress);
        }

        /// <summary>
        /// Downloads the data for the specified show and returns the path to the file.
        /// </summary>
        /// <param name="show">
        /// The show to download data for. 
        /// </param>
        /// <returns>
        /// The path to the downloaded file. 
        /// </returns>
        public static string DownloadShowEpisodes(TvShow show)
        {
            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    var webClient = new WebClient();
                    string showAddress = Mirror + ApiLoc + "/series/" + show.TvdbId + "/all/en.xml";
                    string savePath = Tvdb.CacheDirectory + show.TvdbId + ".xml";

                    if (!Directory.Exists(Tvdb.CacheDirectory))
                    {
                        Directory.CreateDirectory(Tvdb.CacheDirectory);
                    }

                    if (File.Exists(savePath))
                    {
                        File.Delete(savePath);
                    }

                    webClient.DownloadFile(showAddress, savePath);
                    return savePath;
                }
                catch (WebException)
                {
                    // Suppress any exceptions so the download can be retried.
                }
            }

            throw new Exception("Unable to download show data for " + show.Name);
        }

        /// <summary>
        /// Downloads the shows that have been updated since the specified time.
        /// </summary>
        /// <param name="time">
        /// The time to get updates from. 
        /// </param>
        /// <returns>
        /// The XML with the updates. 
        /// </returns>
        public static StringReader DownloadUpdates(DateTime time)
        {
            var timestamp = (int)(time - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
            string updatesUrl = SiteAddress + "/api/Updates.php?type=all&time=" + timestamp;
            return DownloadXml(updatesUrl);
        }

        /// <summary>
        /// Gets the server time from the TVDB.
        /// </summary>
        /// <returns>
        /// The time of the server. 
        /// </returns>
        public static DateTime GetServerTime()
        {
            StringReader timeXml = DownloadXml(TimeAddress);
            XDocument time = XDocument.Load(timeXml);
            string timestamp = time.Descendants("Time").First().Value;

            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(long.Parse(timestamp));
        }

        /// <summary>
        /// Searches for the specified show.
        /// </summary>
        /// <param name="name">
        /// The name of the show to search. 
        /// </param>
        /// <returns>
        /// A string reader of the XML downloaded. 
        /// </returns>
        public static StringReader SearchShow(string name)
        {
            return DownloadXml(Mirror + "/api/GetSeries.php?seriesname=" + name);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Downloads the specified XML file.
        /// </summary>
        /// <param name="url">
        /// The XML to download. 
        /// </param>
        /// <returns>
        /// A stringreader of the XML file. 
        /// </returns>
        private static StringReader DownloadXml(string url)
        {
            var client = new WebClient();
            string xml = client.DownloadString(url);
            return new StringReader(xml);
        }

        /// <summary>
        /// Initialises the mirror to use.
        /// </summary>
        /// <returns>
        /// The selected mirror URL. Null if one cannot be determined. 
        /// </returns>
        private static string InitialiseMirror()
        {
            // Download the mirrors 
            StringReader mirrorString = DownloadXml(MirrorsAddress);
            XDocument mirrors = XDocument.Load(mirrorString);

            // Randomly select one that has a typemask of 7 
            // meaning all data is available at that mirror.
            var r = new Random();
            return (from mirrorElement in mirrors.Descendants("Mirror")
                    let typeMask = mirrorElement.Element("typemask")
                    where typeMask != null && typeMask.Value == "7"
                    orderby r.Next()
                    let mirrorPath = mirrorElement.Element("mirrorpath")
                    where mirrorPath != null
                    select mirrorPath.Value).FirstOrDefault();
        }

        #endregion
    }
}