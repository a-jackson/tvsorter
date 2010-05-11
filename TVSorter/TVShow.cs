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
        }

        public TVShow(long databaseId, string tvdvId, string name, long updateTime, 
            bool useDefaultFormat, string customFormat, string folderName,string banner,string altNames)
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
        }
        /// <summary>
        /// Create a TVShow and attempt to fill in missing data from the database
        /// </summary>
        /// <param name="showName">The show object to create</param>
        public TVShow(string showName)
        {
            Database database = new Database();
            Dictionary<string, object> results;
            //See if the show is in the database. Check the name against the show name, folder name 
            //and all alt names
            if ((long)database.ExecuteScalar("Select Count(*) From Shows Where name Like \"" + showName 
                + "\" Or folder_name Like \""+showName
                + "\" Or Upper(\""+showName+"\") In (Select Trim(Upper(alt_name)) From AltNames"
                + " Where AltNames.show_id = Shows.id);") > 0)
            {
                //Get the first show that matches
                results =
                    database.ExecuteResults("Select * From Shows Where name Like \"" + showName
                + "\" Or folder_name Like \"" + showName
                + "\" Or Upper(\"" + showName + "\") In (Select Trim(Upper(alt_name)) From AltNames"
                + " Where AltNames.show_id = Shows.id);")[0];
                //Set the values using the results in the database
                _databaseId = (long)results["id"];
                _tvdbid = (string)results["tvdb_id"];
                _name = (string)results["name"];
                _updateTime = (long)results["update_time"];
                _useDefaultFormat = ((long)results["use_default_format"] == 1 ? true : false);
                _customFormat = (string)results["custom_format"];
                _folderName = (string)results["folder_name"];
                _banner = (string)results["banner"];
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

        public override string ToString()
        {
            return _name;
        }

        public string Name
        {
            get { return _name; }
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
            get { return _folderName; }
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
    }
}
