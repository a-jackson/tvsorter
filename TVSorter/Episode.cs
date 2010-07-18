using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace TVSorter
{
    /// <summary>
    /// Contains the data for a specific episode
    /// </summary>
    public class Episode
    {
        /// <summary>
        /// The source of the data that defined which episode it was.
        /// Required for reidentification if the show is changed by the user
        /// </summary>
        private enum OriginalData { Episode, Date, All };

        /// <summary>
        /// An example used for the output format examples
        /// </summary>
        public static Episode ExampleEpiosde = new Episode(
            new TVShow("","The IT Crowd",0,"", "The IT Crowd"),
            1, 1, "Yesterday's Jam",
            new DateTime(2006, 2, 3), new FileInfo("file.avi"));

        private static DateTime NullDate = new DateTime(1,1,1);

        private TVShow _show;
        private long _episode;
        private long _season;
        private string _episodeName;
        private DateTime _firstAir;
        private FileInfo _fileInfo;
        private OriginalData _origData;

        public Episode(TVShow show, int episode, int season, 
            string episodeName, DateTime firstAir, FileInfo fileInfo)
        {
            _show = show;
            _episode = episode;
            _season = season;
            _episodeName = episodeName;
            _firstAir = firstAir;
            _fileInfo = fileInfo;
            _origData = OriginalData.All; 
        }

        public Episode(string showName, int episode, int season, FileInfo fileInfo)
        {
            _show = TVShow.GetTVShow(showName); ;
            _episode = episode;
            _season = season;
            _fileInfo = fileInfo;
            _firstAir = NullDate;
            _origData = OriginalData.Episode;
            GetEpisodeInfo();
        }

        public Episode(string showName, DateTime date, FileInfo fileInfo)
        {
            _show = TVShow.GetTVShow(showName);
            _firstAir = date;
            _fileInfo = fileInfo;
            _episode = -1;
            _season = -1;
            _origData = OriginalData.Date;
            GetEpisodeInfo();
        }

        /// <summary>
        /// Fills in the blanks with data from the database
        /// </summary>
        public void GetEpisodeInfo()
        {
            Database database = new Database();
            //If the show has no database ID then it couldn't be identified
            //from the available data so can't look up the episode
            if (_show == null || _show.DatabaseId == -1)
            {
                _episodeName = "";
                return;//Show unknown
            }
            else if (_origData == OriginalData.Episode) //Have the season/episode number
            {
                //use season/episode numbers to get rest
                try
                {
                    //Attempt to find the episode in the database to get the name and date
                    Dictionary<string, object> results = database.ExecuteResults(
                        "Select * From Episodes Where show_id = "
                        + _show.DatabaseId + " And season_num = "
                        + SeasonNum + " And episode_num = " + EpisodeNum + ";")[0];
                    _episodeName = (string)results["episode_name"];
                    _firstAir = TVShow.ConvertFromUnixTimestamp((long)results["first_air"]);
                }
                catch
                {
                    //Failed to find it
                    _firstAir = NullDate;
                    _episodeName = "";
                }
            }
            else if (_origData == OriginalData.Date) //Have the date
            {
                //use date to get rest
                try
                {
                    //Convert to a timestamp as is stored in the database
                    long firstAir = TVShow.ConvertToUnixTimestamp(_firstAir);
                    //Attempt to find an episode with the same airdate
                    Dictionary<string, object> results = database.ExecuteResults(
                        "Select * From Episodes Where show_id = "
                        + _show.DatabaseId + " And first_air = "
                        + firstAir + ";")[0];
                    //Get the name, season and episode number
                    _episodeName = (string)results["episode_name"];
                    _season = (long)results["season_num"];
                    _episode = (long)results["episode_num"];
                }
                catch
                {
                    //Failed, no name
                    _episodeName = "";
                }
            }
            else
            {
                //Not enough data to look anything up
                _episodeName = "";
            }
        }

        public TVShow Show
        {
            get { return _show; }
            //After the show has changed, refresh the episode info
            set { _show = value; GetEpisodeInfo(); }
        }

        public string EpisodeName
        {
            get { return (_episodeName ?? "").Trim(); }
        }
        public long EpisodeNum
        {
            get { return _episode; }
        }
        public long SeasonNum
        {
            get { return _season; }
        }
        public FileInfo FileInfo
        {
            get { return _fileInfo; }
        }
        public bool IsComplete
        {
            get { return (_show != null && _show.DatabaseId != -1 && EpisodeName != ""); }
        }

        /// <summary>
        /// Gets the formatted output path for the current episode, uses the appropriate format.
        /// </summary>
        /// <returns>The formatted output path for the episode</returns>
        public string FormatOutputPath()
        {
            //If there is no show, it's not in the database, or the show uses the default format
            //then use the default format
            if (_show == null || _show.DatabaseId == -1 || _show.UseDefaultFormat)
            {
                return FormatOutputPath(Settings.FileNameFormat);
            }
            else
            {
                //Use the custom format for the show
                return FormatOutputPath(_show.CustomFormat);
            }
        }

        /// <summary>
        /// Formats the output path to the specified format
        /// </summary>
        /// <param name="format">The format string to use</param>
        /// <returns>The formatted string</returns>
        public string FormatOutputPath(string format)
        {
                        //Set any instances of {Ext} and {FName}
            //They have no arg so the regexp doesn't match them
            if (_fileInfo != null)
            {
                format = format.Replace("{Ext}", _fileInfo.Extension);
            }
            if (_show != null)
            {
                format = format.Replace("{FName}", _show.FolderName);
            }
            Regex regExp = new Regex("{([a-zA-Z]+)\\(([^\\)}]+)\\)}");
            //Search for matches and make sure that the data exists
            foreach (Match match in regExp.Matches(format))
            {
                string type = match.Groups[1].Value;
                if ((type == "SName" && _show.Name == "") ||
                    (type == "EName" && _episodeName == ""))
                {
                    int start = match.Index - 1;
                    int end = match.Index + match.Length;
                    if (start >= 0 && end < format.Length - 1)
                    {
                        char[] separatorChars = new char[] { '-', ':', '_', '.', ',' };
                        if (format[start] == format[end] && separatorChars.Contains(format[start]))
                        {
                            format = format.Remove(start, 1);
                        }
                    }
                }
            }

            //Search for the variables and replace them in the string. This is searching for
            //{Var(Arg)}
            format = regExp.Replace(format, delegate(Match match)
            {
                string type = match.Groups[1].Value;
                string arg = match.Groups[2].Value;
                if (type == "SName") //Show name
                {
                    if (_show != null)
                    {
                        return FormatName(_show.Name, arg);
                    }
                    return "";
                }
                else if (type == "EName") //Episode name
                {
                    return FormatName(_episodeName, arg);
                }
                else if (type == "ENum") //Episode number
                {
                    try
                    {
                        return FormatNum(arg,_episode);
                    }
                    catch
                    {
                        return match.Value;
                    }
                }
                else if (type == "SNum") //Season number
                {
                    try
                    {
                        return FormatNum(arg, _season);
                    }
                    catch
                    {
                        return match.Value;
                    }
                }
                else if (type == "Date") //First air date
                {
                    try
                    {
                        return _firstAir.ToString(arg);
                    }
                    catch
                    {
                        return match.Value;
                    }
                }
                return match.Value;
            });
            //Replace : with .
            format = format.Replace(':','.');
            //Remove any characters that can't be in a filename
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                if (ch.Equals(Path.DirectorySeparatorChar))
                    continue;
                format = format.Replace(ch.ToString(), "");
            }
            return format;
        }

        /// <summary>
        /// Format the numbers for output
        /// </summary>
        /// <param name="arg">The variable's argument, the length of the output num</param>
        /// <param name="num">The number to format</param>
        /// <returns>The formatted string</returns>
        private string FormatNum(string arg, long num)
        {
            int length = int.Parse(arg);
            string zeros = "";
            if (length == 0)
                zeros = "0";
            else
                for (int i = 0; i < length; i++)
                    zeros += "0";
            return String.Format("{0:" + zeros + "}", num);
        }

        /// <summary>
        /// Formatted the name of an episode or show
        /// </summary>
        /// <param name="name">The name of the episode/show</param>
        /// <param name="arg">The variable's argument, should be the separator char</param>
        /// <returns>The formatted string</returns>
        private string FormatName(string name, string arg)
        {
            //Break into separate words
            string[] parts = name.Split(' ');
            //Characters that can be used as separators, if there is one of these at the start
            //or end of a part then it won't put the arg character next to it. This is to avoid
            //outputs like Day.1.12.00.-.1.00 for a 24 episode since .-. looks silly.
            //Or names with titles in like Dr. Smith would be Mr..Smith - equally silly.
            string[] separatorChars = new string[] { "-", ":", "_", ".", "," };
            //The new name to be returned
            string newName = parts[0];
            //Loop through each part appending it with the appropriate separator
            for (int i = 1; i < parts.Length; i++)
            {
                bool prefix = true;
                //Check for the separator chars at the start of this part of the end of the next,
                //no extra one should be added
                foreach (string sep in separatorChars)
                {
                    if (parts[i].StartsWith(sep) || parts[i - 1].EndsWith(sep))
                    {
                        prefix = false;
                        break;
                    }
                }
                //Add the arg character
                if (prefix)
                {
                    newName += arg;
                }
                //Add the part
                newName += parts[i];
            }
            //If the string ends with a separator character then remove it
            foreach (string sep in separatorChars)
            {
                if (newName.EndsWith(sep))
                    newName = newName.Substring(0, newName.Length - 1);
            }
            return newName;
        }
    }
}
