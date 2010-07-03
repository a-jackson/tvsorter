using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text;
using System.Threading;

namespace TVSorter
{
    /// <summary>
    /// A class containing methods for accessing The TVDB API
    /// </summary>
    public class TVDB
    {
        private static TVDB instance;
        public static TVDB Instance
        {
            get { return instance ?? (instance = new TVDB()); }
        }
        //The program's API key and other paths
        const string ApiKey = "D4DCAEBFCA5A6BC1";
        const string SiteAddress = "http://www.thetvdb.com/";
        const string ApiLoc = "/api/" + ApiKey + "/";
        const string MirrorsAddress = SiteAddress + ApiLoc + "/mirrors.xml";
        const string TimeAddress = SiteAddress + "/api/Updates.php?type=none";
        static Database _database = new Database();

        //Events
        public event Increment Increment = delegate { };
        public event ProgressError Abort = delegate { };

        private string MirrorAddress;
        private long ServerTime;

        private bool EnvironmentSet = false;

        private TVDB()
        {
            SetEnvironment();
        }

        public void SetEvents(Increment inc, ProgressError error)
        {
            Increment += inc;
            Abort += error;
        }

        public void ClearEvents(Increment inc, ProgressError error)
        {
            Increment -= inc;
            Abort -= error;
        }

        /// <summary>
        /// Sets up the environment for getting data from The TVDB
        /// </summary>
        private void SetEnvironment()
        {
            if (EnvironmentSet)
            {
                UpdateServerTime();
                return;
            }
            //Create the appdata folder if needed
            if (!Directory.Exists("Data"))
                Directory.CreateDirectory("Data");

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
        private void UpdateServerTime()
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
        public List<TVShow> SearchShow(string name)
        {
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
        public TVShow GetShow(int showid)
        {
            XmlDocument searchDoc = new XmlDocument();
            string seriesAddress = MirrorAddress + ApiLoc + "/series/" + showid + "/en.xml";
            searchDoc.Load(seriesAddress);
            XmlNodeList series = searchDoc.GetElementsByTagName("Series")[0].ChildNodes;
            TVShow show = new TVShow(showid.ToString(), series[15].InnerText,
                ServerTime, series[19].InnerText, series[15].InnerText);
            return show;
        }

        /// <summary>
        /// Downloads the banner for the specified show
        /// </summary>
        /// <param name="show">The show to download the banner for</param>
        private void DownloadShowBanner(TVShow show)
        {
            if (show.Banner == "" || File.Exists("Data\\"+show.TvdbId))
                return;
            WebClient client = new WebClient();
            string address = MirrorAddress + "/banners/" + show.Banner;
            string save = "Data\\" + show.TvdbId;
            client.DownloadFile(address, save);
        }

        public void UpdateShows(bool force, params TVShow[] shows)
        {
            new Thread(new ThreadStart(delegate()
            {
                try
                {
                    Log.Add("Update started");
                    foreach (TVShow show in shows)
                    {
                        UpdateShow(show, force);
                        Increment();
                    }
                    Log.Add("Update complete");
                }
                catch (Exception ex)
                {
                    Abort(ex.Message);
                    Log.Add("Update failed: " + ex.Message);
                }
            })).Start();
        }

        /// <summary>
        /// Updates a show's episode data in the database
        /// </summary>
        /// <param name="show">The show to update</param>
        /// <param name="force">If the update should be forced rather
        /// than checking if there are any updates</param>
        private void UpdateShow(TVShow show, bool force)
        {
            if (show.Locked)
            {
                return;
            }
            UpdateServerTime();
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
                    show.UpdateTime = ServerTime;
                    show.SaveToDatabase();
                    Log.Add("No updates for " + show.Name + " refreshed update time");
                    return;
                }
            }
            string showAddress = MirrorAddress + ApiLoc + "/series/" + show.TvdbId +
                "/all/en.xml";
            XmlDocument showDoc = new XmlDocument();
            showDoc.Load(showAddress);
            //Download the banner if necessary
            XmlNode banner = showDoc.GetElementsByTagName("banner")[0];
            if (banner.InnerText != show.Banner)
            {
                show.Banner = banner.InnerText;
                //Delete the old one and get the new one
                File.Delete("Data\\" + show.TvdbId);
                DownloadShowBanner(show);
            }
            //Get all the episodes and add to the database
            XmlNodeList episodes = showDoc.GetElementsByTagName("Episode");
            ProcessXMLFile(show, episodes);
        }

        private void ProcessXMLFile(TVShow show, XmlNodeList episodes)
        {
            StringBuilder query = new StringBuilder();
            //Clear all episode data for this show            
            query.Append("Delete From Episodes Where show_id = " + show.DatabaseId + ";");         
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
                    first_air = TVShow.ConvertToUnixTimestamp
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
            }
            //Get the current number of episodes
            long currEps = (long)_database.ExecuteScalar("Select Count(*) From Episodes Where show_id = " + show.DatabaseId);
            //Execute the query
            long eps = _database.ExecuteQuery("Begin;" + query.ToString() + "Commit;");
            //Eps includes the delete query. Need to remove that many from it.
            eps -= currEps;
            //Refresh the update time
            UpdateServerTime();
            show.UpdateTime = ServerTime;
            show.SaveToDatabase();
            Log.Add("Updated " + show.Name + " has " + eps + " episodes");
        }


        /// <summary>
        /// Searches the output directory for new shows
        /// <returns>A Dictionary of the results, won't be populated until the thread is completed</returns>
        /// </summary>
        public SortedDictionary<string, List<TVShow>> SearchNewShows()
        {
            SortedDictionary<string, List<TVShow>> shows = new SortedDictionary<string, List<TVShow>>();
            Thread addShows = new Thread(new ThreadStart(delegate()
            {
                foreach (string dir in Directory.GetDirectories(Settings.OutputDir))
                {
                    //Check if the show has already been added.
                    string name = dir.Substring(dir.LastIndexOf('\\') + 1).Replace("\"", "\"\"");
                    if ((long)_database.ExecuteScalar("SELECT COUNT(*) FROM shows WHERE folder_name=\""
                        + name + "\"") == 0)
                    {
                        try
                        {
                            //Search for the show and add the results
                            List<TVShow> results = TVDB.Instance.SearchShow(name);
                            shows.Add(name, results);
                        }
                        catch
                        {
                            Abort("The TVDB is down, unable to search for shows.");
                            return;
                        }
                    }
                    Increment();
                }
            }));
            addShows.Start();
            return shows;
        }
    }
}
