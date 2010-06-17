using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TVSorter
{
    /// <summary>
    /// Class representing a TV Show
    /// </summary>
    public class TVShow
    {
        private long _databaseId;
        private string _tvdbid;
        private string _name;
        private long _updateTime;
        private bool _useDefaultFormat;
        private string _customFormat;
        private string _folderName;
        private string _banner;
        private string _altNames;
        private bool _locked;
        private bool _useDvdOrder;

        public TVShow(string id, string name, long updatetime,string banner,string folderName)
        {
            _databaseId = -1;
            _tvdbid = id;
            _name = name;
            _updateTime = updatetime;
            _useDefaultFormat = true;
            _customFormat = "";
            _folderName = folderName;
            _banner = banner;
            _altNames = "";
            _locked = false;
            _useDvdOrder = false;
        }

        public TVShow(long databaseId, string tvdvId, string name, long updateTime, 
            bool useDefaultFormat, string customFormat, string folderName,string banner,
            string altNames, bool locked, bool dvdOrder)
        {
            _databaseId = databaseId;
            _tvdbid = tvdvId;
            _name = name;
            _updateTime = updateTime;
            _useDefaultFormat = useDefaultFormat;
            _customFormat = customFormat;
            _folderName = folderName;
            _banner = banner;
            _altNames = altNames;
            _locked = locked;
            _useDvdOrder = dvdOrder;
        }
        /// <summary>
        /// Create a TVShow and attempt to fill in missing data from the database
        /// </summary>
        /// <param name="showName">The show object to create</param>
        public TVShow(string showName)
        {
            Database database = new Database();
            List<Dictionary<string, object>> results;
            //See if the show is in the database. Check the name against the show name, folder name 
            //and all alt names
            results = database.ExecuteResults("Select * From Shows Where name Like \"" + showName
                + "\" Or folder_name Like \"" + showName
                + "\" Or Upper(\"" + showName + "\") In (Select Trim(Upper(alt_name)) From AltNames"
                + " Where AltNames.show_id = Shows.id);");
            if (results.Count > 0)
            {
                //Get the first show that matches
                Dictionary<string, object> result = results[0];
                //Set the values using the results in the database
                _databaseId = (long)result["id"];
                _tvdbid = (string)result["tvdb_id"];
                _name = (string)result["name"];
                _updateTime = (long)result["update_time"];
                _useDefaultFormat = ((long)result["use_default_format"] == 1 ? true : false);
                _customFormat = (string)result["custom_format"];
                _folderName = (string)result["folder_name"];
                _banner = (string)result["banner"];
                _locked = ((long)result["show_locked"] == 1 ? true : false);
                _useDvdOrder = ((long)result["use_dvd_order"] == 1 ? true : false);
                //Get the altnames for this show
                List<Dictionary<string, object>> altName = database.ExecuteResults
                    ("Select * From AltNames Where show_id = "
                    + _databaseId + ";");
                _altNames = "";
                foreach (Dictionary<string, object> altNameRow in altName)
                    _altNames += (string)altNameRow["alt_name"] + ",";
                if (_altNames.EndsWith(","))
                    _altNames = _altNames.Substring(0, _altNames.Length - 1);
            }
            else
            {
                _name = showName;
                _databaseId = -1;
            }
        }

        /// <summary>
        /// Saves the show to the database, if it exists it updates, else inserts
        /// </summary>
        public void SaveToDatabase()
        {
            Database database = new Database();
            if (DatabaseId != -1)
            {
                string query = "Update Shows Set " +
                "use_default_format = " + (UseDefaultFormat ? 1 : 0) +
                ", custom_format = \"" + CustomFormat.Replace("\"", "\"\"") +
                "\", folder_name = \"" + FolderName.Replace("\"", "\"\"") +
                "\", show_locked = " + (Locked ? 1 : 0) +
                ", use_dvd_order = " + (UseDvdOrder ? 1 : 0) +
                ", update_time = " + UpdateTime +
                ", banner = '" + Banner + "'" +
                " Where id = " + DatabaseId + ";";
                database.ExecuteQuery(query);
                //Process alternate names - delete existing ones and add everything again
                database.ExecuteQuery("Delete From AltNames Where show_id = " + DatabaseId + ";");
                string[] altNames = AltNames.Split(',');
                if (altNames.Length > 0)
                {
                    string altNameQuery = "";
                    foreach (string altName in altNames)
                    {
                        if (altName.Length > 0)
                        {
                            altNameQuery += "Insert Into AltNames (show_id, alt_name) Values ("
                                + DatabaseId + ", \""
                                + altName.Replace("\"", "\"\"") + "\");";
                        }
                    }
                    database.ExecuteQuery(altNameQuery);
                }
            }
            else //New show
            {
                string query = "Insert Into Shows " +
                       "(tvdb_id, name, update_time, use_default_format, custom_format, folder_name, banner) " +
                       "Values (\"" + 
                       TvdbId + "\", \"" + 
                       Name.Replace("\"", "\"\"") + "\", " +
                       UpdateTime + ", " +
                       (UseDefaultFormat ? 1 : 0) + ", \"" +
                       CustomFormat + "\", \"" + 
                       FolderName + "\", \"" +
                       Banner + "\");";
                database.ExecuteQuery(query);
            }
        }

        public override string ToString()
        {
            return (_name ?? "").Trim();
        }

        public string Name
        {
            get { return (_name ?? "").Trim(); }
        }
        public string TvdbId
        {
            get { return _tvdbid; }
        }
        public string CustomFormat
        {
            get { return _customFormat; }
            set { _customFormat = value; }
        }
        public long UpdateTime
        {
            get { return _updateTime; }
            set { _updateTime = value; }
        }
        public bool UseDefaultFormat
        {
            get { return _useDefaultFormat; }
            set { _useDefaultFormat = value; }
        }
        public long DatabaseId
        {
            get { return _databaseId; }
        }
        public string FolderName
        {
            get { return (_folderName ?? "").Trim(); }
            set { _folderName = value; }
        }
        public string Banner
        {
            get { return _banner; }
            set { _banner = value; }
        }
        public string AltNames
        {
            get { return _altNames; }
            set { _altNames = value; }
        }
        public bool Locked
        {
            get { return _locked; }
            set { _locked = value; }
        }
        public bool UseDvdOrder
        {
            get { return _useDvdOrder; }
            set { _useDvdOrder = value; }
        }

        /// <summary>
        /// Returns a list of all the shows in the database
        /// </summary>
        /// <returns></returns>
        public static List<TVShow> GetAllShows()
        {
            Database database = new Database();
            //Get all the tv shows
            List<Dictionary<string, object>> shows =
                database.ExecuteResults("SELECT * FROM shows ORDER BY name");
            List<TVShow> showList = new List<TVShow>();
            foreach (Dictionary<string, object> row in shows)
            {
                List<Dictionary<string, object>> altName =
                    database.ExecuteResults("Select * From AltNames Where show_id = "
                    + (long)row["id"] + ";");
                string altNames = "";
                foreach (Dictionary<string, object> altNameRow in altName)
                    altNames += (string)altNameRow["alt_name"] + ",";
                if (altNames.EndsWith(","))
                    altNames = altNames.Substring(0, altNames.Length - 1);
                //Create the TVShow object with the data in the database
                TVShow tvshow = new TVShow((long)row["id"],
                    (string)row["tvdb_id"], (string)row["name"],
                    (long)row["update_time"],
                    ((long)row["use_default_format"] == 1 ? true : false),
                    (string)row["custom_format"],
                    (string)row["folder_name"],
                    (string)row["banner"],
                    altNames,
                    ((long)row["show_locked"] == 1 ? true : false),
                    ((long)row["use_dvd_order"] == 1 ? true : false));
                showList.Add(tvshow);
            }
            return showList;
        }
    }
}
