using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;

namespace TVSorter
{
    /// <summary>
    /// A class containing methods for accessing The TVDB API
    /// </summary>
    public class TVDB
    {
        //The program's API key and other paths
        const string ApiKey = "D4DCAEBFCA5A6BC1";
        const string SiteAddress = "http://www.thetvdb.com/";
        const string ApiLoc = "/api/" + ApiKey + "/";
        const string MirrorsAddress = SiteAddress + ApiLoc + "/mirrors.xml";
        const string TimeAddress = SiteAddress + "/api/Updates.php?type=none";
        static Database _database = new Database();

        static string AppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TVSorter2\\";
        static string MirrorAddress;
        public static long ServerTime;

        static bool EnvironmentSet = false;

        /// <summary>
        /// Sets up the environment for getting data from The TVDB
        /// </summary>
        private static void SetEnvironment()
        {
            if (EnvironmentSet)
            {
                UpdateServerTime();
                return;
            }
            //Create the appdata folder if needed
            if (!Directory.Exists(AppData))
                Directory.CreateDirectory(AppData);

            //Set the mirror address
                XmlDocument mirrorDoc = new XmlDocument();
                mirrorDoc.Load(MirrorsAddress);
                string address = mirrorDoc.GetElementsByTagName("mirrorpath")[0].InnerText;
                MirrorAddress = address;

            //Get time since last update of cached data
            UpdateServerTime();

            EnvironmentSet = true;
        }

        /// <summary>
        /// Updates the ServerTime variable with the current time of The TVDB server
        /// </summary>
        private static void UpdateServerTime()
        {
                XmlDocument timeDoc = new XmlDocument();
                timeDoc.Load(TimeAddress);
                ServerTime = long.Parse(timeDoc.GetElementsByTagName("Time")[0].InnerText);
        }

        /// <summary>
        /// Searches The TVDB for the specified show and returns the results in a list
        /// </summary>
        /// <param name="name">The name of the show to search for</param>
        /// <returns>The possible results</returns>
        public static List<TVShow> SearchShow(string name)
        {
            SetEnvironment();
            XmlDocument searchDoc = new XmlDocument();
            searchDoc.Load(MirrorAddress + "/api/GetSeries.php?seriesname=" + name);
            XmlNodeList seriess = searchDoc.GetElementsByTagName("Series");
            List<TVShow> results = new List<TVShow>();
            //For each show found, add the result to the list
            foreach (XmlNode series in seriess)
            {
                int id = int.Parse(series.ChildNodes[0].InnerText);
                string showName = series.ChildNodes[2].InnerText;
                string banner = series.ChildNodes[3].InnerText;
                long updatetime = -1;
                results.Add(new TVShow(id.ToString(), showName, updatetime, banner, name));
            }
            return results;
        }

        /// <summary>
        /// Get a show's data from its TVDB ID number
        /// </summary>
        /// <param name="showid">The ID to search for</param>
        /// <returns>A TVShow object representing the show</returns>
        public static TVShow GetShow(int showid)
        {
            SetEnvironment();
            XmlDocument searchDoc = new XmlDocument();
            string seriesAddress = MirrorAddress + ApiLoc + "/series/" + showid + "/en.xml";
            searchDoc.Load(seriesAddress);
            XmlNodeList series = searchDoc.GetElementsByTagName("Series")[0].ChildNodes;
            TVShow show = new TVShow(showid.ToString(), series[15].InnerText,
                -1, series[19].InnerText, series[15].InnerText);
            return show;
        }

        /// <summary>
        /// Downloads the banner for the specified show
        /// </summary>
        /// <param name="show">The show to download the banner for</param>
        private static void DownloadShowBanner(TVShow show)
        {
            SetEnvironment();
            if (show.Banner == "")
                return;
            WebClient client = new WebClient();
            string address = MirrorAddress + "/banners/" + show.Banner;
            string save = AppData + show.Banner.Replace('/', '\\');
            Directory.CreateDirectory(save.Substring(0, save.LastIndexOf('\\')));
            if (!File.Exists(save))
                client.DownloadFile(address, save);
        }

        /// <summary>
        /// Updates a show's episode data in the database
        /// </summary>
        /// <param name="show">The show to update</param>
        /// <param name="force">If the update should be forced rather
        /// than checking if there are any updates</param>
        public static void UpdateShow(TVShow show, bool force)
        {
            if (show.Locked)
            {
                Log.Add(show.Name + " is locked. Skipping.");
                return;
            }
            SetEnvironment();
            //Download banner.
            DownloadShowBanner(show);
            //Not being forced to update and has updated in the last month
            //check if it needs it.
            if (!force && ServerTime - show.UpdateTime < 2419200)
            {
                //Check if it needs updating - get the time of the show's update
                XmlDocument updates = new XmlDocument();
                updates.Load("http://www.thetvdb.com/api/Updates.php?time=" +
                    show.UpdateTime + "&type=series");
                XmlNodeList series = updates.GetElementsByTagName("Series");
                bool shouldUpdate = false;
                //Search for the show
                foreach (XmlNode seriesId in series)
                {
                    string id = seriesId.InnerText;
                    if (id.Equals(show.TvdbId))
                    {
                        shouldUpdate = true;
                        break;
                    }
                }
                //If the show doesn't need updating, refresh its update time and return
                if (!shouldUpdate)
                {
                    string show_query = "Update Shows Set update_time = " + ServerTime + " Where id = " +
                        show.DatabaseId + ";";
                    _database.ExecuteQuery(show_query);
                    show.UpdateTime = ServerTime;
                    Log.Add("No updates for " + show.Name + " refreshed update time");
                    return;
                }
            }
            //Clear all episode data for this show
            _database.ExecuteQuery("Delete From Episodes Where show_id = " + show.DatabaseId + ";");
            string showAddress = MirrorAddress + ApiLoc + "/series/" + show.TvdbId +
                "/all/en.xml";
            XmlDocument showDoc = new XmlDocument();
            showDoc.Load(showAddress);
            //Download the banner if necessary
            XmlNode banner = showDoc.GetElementsByTagName("banner")[0];
            if (banner.InnerText != show.Banner)
            {
                show.Banner = banner.InnerText;
                DownloadShowBanner(show);
            }
            //Get all the episodes and add to the database
            XmlNodeList episodes = showDoc.GetElementsByTagName("Episode");
            StringBuilder query = new StringBuilder();
            int count = 0;
            int eps = 0;
            foreach (XmlNode episode in episodes)
            {
                //Get each piece of data from the XML
                string tvdb_id = episode.ChildNodes[0].InnerText;
                string episode_name = episode.ChildNodes[9].InnerText;
                string date = episode.ChildNodes[11].InnerText;
                long first_air;
                if (date.Length > 0)
                {
                    //Convert date to timestamp
                    string[] dateParts = date.Split('-');
                    int year = int.Parse(dateParts[0]);
                    int month = int.Parse(dateParts[1]);
                    int day = int.Parse(dateParts[2]);
                    first_air = frmMain.ConvertToUnixTimestamp
                        (new DateTime(year, month, day));
                }
                else
                {
                    first_air = 0;
                }
                int episode_num;
                int season_num;
                if (show.UseDvdOrder)
                {
                    string dvdE = episode.ChildNodes[5].InnerText;
                    string dvdS = episode.ChildNodes[6].InnerText;
                    if (dvdE == "" || dvdS == "")
                    {
                        dvdE = episode.ChildNodes[10].InnerText;
                        dvdS = episode.ChildNodes[18].InnerText;
                    }
                    episode_num = (int)float.Parse(dvdE);
                    season_num = (int)float.Parse(dvdS);
                }
                else
                {
                    episode_num = int.Parse(episode.ChildNodes[10].InnerText);
                    season_num = int.Parse(episode.ChildNodes[18].InnerText);
                }
                //Build the query
                query.Append("Insert Into Episodes (show_id, tvdb_id, episode_num, season_num," +
                    "first_air, episode_name) Values (" +
                    show.DatabaseId + ", '" +
                    tvdb_id + "', " +
                    episode_num + ", " +
                    season_num + ", " +
                    first_air + ", \"" +
                    episode_name.Replace("\"", "\"\"") + "\");");
                count++;
                //Only run 1000 eps at a time
                if (count == 1000)
                {
                    eps += _database.ExecuteQuery("Begin;"+query.ToString()+"Commit;");
                    count = 0;
                    query.Remove(0, query.Length);
                }
            }
            if (query.Length > 0)
            {
                //Execute the query
                eps += _database.ExecuteQuery("Begin;" + query.ToString() + "Commit;");
            }
            //Refresh the update time
            string showquery = "Update Shows Set update_time = " + ServerTime +
                ", banner = '" + show.Banner + "' Where id = " +
                show.DatabaseId + ";";
            show.UpdateTime = ServerTime;
            _database.ExecuteQuery(showquery);
            Log.Add("Updated " + show.Name + " has " + eps + " episodes");
        }
    }
}
