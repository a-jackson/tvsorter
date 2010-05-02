using System;
using System.Xml;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace TVSorter
{
    public class TVDB
    {

        const string ApiKey = "D4DCAEBFCA5A6BC1";
        const string SiteAddress = "http://www.thetvdb.com/";
        const string ApiLoc = "/api/" + ApiKey + "/";
        const string MirrorsAddress = SiteAddress + ApiLoc + "/mirrors.xml";
        const string TimeAddress = SiteAddress + "/api/Updates.php?type=none";
        static Database _database = new Database();

        static string AppData = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\TVSorter2\\";
        static string ShowsIDFile = AppData + "ShowsIDs.dat";
        static string MirrorAddress;
        public static long ServerTime;

        static bool EnvironmentSet = false;

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

        private static void UpdateServerTime()
        {
                XmlDocument timeDoc = new XmlDocument();
                timeDoc.Load(TimeAddress);
                ServerTime = long.Parse(timeDoc.GetElementsByTagName("Time")[0].InnerText);
        }

        public static List<TVShow> SearchShow(string name)
        {
            SetEnvironment();
            XmlDocument searchDoc = new XmlDocument();
            searchDoc.Load(MirrorAddress + "/api/GetSeries.php?seriesname=" + name);
            XmlNodeList seriess = searchDoc.GetElementsByTagName("Series");
            List<TVShow> results = new List<TVShow>();
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

        public static void UpdateShow(TVShow show, bool force)
        {
            SetEnvironment();
            //Download banner.
            DownloadShowBanner(show);
            if (!force && ServerTime - show.UpdateTime < 2419200)
            {
                //Check if it needs updating
                XmlDocument updates = new XmlDocument();
                updates.Load("http://www.thetvdb.com/api/Updates.php?time=" +
                    show.UpdateTime + "&type=series");
                XmlNodeList series = updates.GetElementsByTagName("Series");
                bool shouldUpdate = false;
                foreach (XmlNode seriesId in series)
                {
                    string id = seriesId.InnerText;
                    if (id.Equals(show.TvdbId))
                    {
                        shouldUpdate = true;
                        break;
                    }
                }
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
            XmlNode banner = showDoc.GetElementsByTagName("banner")[0];
            if (banner.InnerText != show.Banner)
            {
                show.Banner = banner.InnerText;
                DownloadShowBanner(show);
            }
            XmlNodeList episodes = showDoc.GetElementsByTagName("Episode");
            string query = "";
            foreach (XmlNode episode in episodes)
            {
                string tvdb_id = episode.ChildNodes[0].InnerText;
                string episode_name = episode.ChildNodes[9].InnerText;
                int episode_num = int.Parse(episode.ChildNodes[10].InnerText);
                string date = episode.ChildNodes[11].InnerText;
                int season_num = int.Parse(episode.ChildNodes[18].InnerText);
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
                query += "Insert Into Episodes (show_id, tvdb_id, episode_num, season_num," +
                    "first_air, episode_name) Values (" +
                    show.DatabaseId + ", '" +
                    tvdb_id + "', " +
                    episode_num + ", " +
                    season_num + ", " +
                    first_air + ", \"" +
                    episode_name.Replace("\"", "\"\"") + "\");";  
            }
            int eps = _database.ExecuteQuery(query);
            string showquery = "Update Shows Set update_time = " + ServerTime +
                ", banner = '" + show.Banner + "' Where id = " +
                show.DatabaseId + ";";
            show.UpdateTime = ServerTime;
            _database.ExecuteQuery(showquery);
            Log.Add("Updated " + show.Name + " has " + eps + " episodes");
        }
    }
}
