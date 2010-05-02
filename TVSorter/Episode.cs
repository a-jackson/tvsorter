using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace TVSorter
{
    public class Episode
    {
        private enum OriginalData { Episode, Date, All };

        public static Episode ExampleEpiosde = new Episode(
            new TVShow("","The IT Crowd",0,"", "The IT Crowd"),
            1, 1, "Yesterday's Jam",
            new DateTime(2006, 2, 3), new FileInfo("C:\\file.avi"));

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
            _show = new TVShow(showName);
            _episode = episode;
            _season = season;
            _fileInfo = fileInfo;
            _firstAir = NullDate;
            _origData = OriginalData.Episode;
            GetEpisodeInfo();
        }

        public Episode(string showName, DateTime date, FileInfo fileInfo)
        {
            _show = new TVShow(showName);
            _firstAir = date;
            _fileInfo = fileInfo;
            _episode = -1;
            _season = -1;
            _origData = OriginalData.Date;
            GetEpisodeInfo();
        }

        public void GetEpisodeInfo()
        {
            Database database = new Database();
            if (_show.DatabaseId == -1)
            {
                _episodeName = "";
                return;//Show unknown
            }
            else if (_origData == OriginalData.Episode)
            {
                //use season/episode numbers to get rest
                try
                {
                    Dictionary<string, object> results = database.ExecuteResults("Select * From Episodes Where show_id = "
                        + _show.DatabaseId + " And season_num = "
                        + Season + " And episode_num = " + Epiosde + ";")[0];
                    _episodeName = (string)results["episode_name"];
                    _firstAir = frmMain.ConvertFromUnixTimestamp((long)results["first_air"]);
                }
                catch
                {
                    _firstAir = NullDate;
                    _episodeName = "";
                }
            }
            else if (_origData == OriginalData.Date)
            {
                //use date to get rest
                try
                {
                    long firstAir = frmMain.ConvertToUnixTimestamp(_firstAir);
                    Dictionary<string, object> results = database.ExecuteResults("Select * From Episodes Where show_id = "
                        + _show.DatabaseId + " And first_air = "
                        + firstAir + ";")[0];
                    _episodeName = (string)results["episode_name"];
                    _season = (long)results["season_num"];
                    _episode = (long)results["episode_num"];
                }
                catch
                {
                    _episodeName = "";
                }
            }
            else
            {
                _episodeName = "";
            }
        }

        public TVShow Show
        {
            get { return _show; }
            set { _show = value; GetEpisodeInfo(); }
        }

        public string EpisodeName
        {
            get { return _episodeName; }
        }
        public long Epiosde
        {
            get { return _episode; }
        }
        public long Season
        {
            get { return _season; }
        }
        public FileInfo FileInfo
        {
            get { return _fileInfo; }
        }

        public string FormatOutputPath()
        {
            if (_show == null || _show.DatabaseId == -1 || _show.UseDefaultFormat)
            {
                return FormatOutputPath(Properties.Settings.Default.FileNameFormat);
            }
            else
            {
                return FormatOutputPath(_show.CustomFormat);
            }
        }

        public string FormatOutputPath(string format)
        {
            format = Regex.Replace(format, "{([a-zA-Z]+)\\(([^\\)}]+)\\)}", delegate(Match match)
            {
                string type = match.Groups[1].Value;
                string arg = match.Groups[2].Value;
                if (type == "SName")
                {
                    if (_show != null)
                    {
                        return FormatName(_show.Name, arg);
                    }
                    return "";
                }
                else if (type == "EName")
                {
                    return FormatName(_episodeName, arg);
                }
                else if (type == "ENum")
                {
                    try
                    {
                        int length = int.Parse(arg);
                        string zeros = "";
                        if (length == 0)
                            zeros = "0";
                        else
                            for (int i = 0; i < length; i++)
                                zeros += "0";
                        return String.Format("{0:" + zeros + "}", _episode);
                    }
                    catch
                    {
                        return match.Value;
                    }
                }
                else if (type == "SNum")
                {
                    try
                    {
                        int length = int.Parse(arg);
                        string zeros = "";
                        if (length == 0)
                            zeros = "0";
                        else
                            for (int i = 0; i < length; i++)
                                zeros += "0";
                        return String.Format("{0:" + zeros + "}", _season);
                    }
                    catch
                    {
                        return match.Value;
                    }
                }
                else if (type == "Date")
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
            if (_fileInfo != null)
            {
                format = format.Replace("{Ext}", _fileInfo.Extension);
            }
            if (_show != null)
            {
                format = format.Replace("{FName}", _show.FolderName);
            }
            //Replace : with .
            format = format.Replace(':','.');
            //Remove any characters that can't be in a filename
            foreach (char ch in Path.GetInvalidFileNameChars())
            {
                if (ch.Equals('\\'))
                    continue;
                format = format.Replace(ch.ToString(), "");
            }
            return format;
        }

        private string FormatName(string episodeName, string arg)
        {
            string[] parts = episodeName.Split(' ');
            string[] separatorChars = new string[] { "-", ":", "_", ".", "," };
            string newName = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                bool prefix = true;
                foreach (string sep in separatorChars)
                {
                    if (parts[i].StartsWith(sep) || parts[i - 1].EndsWith(sep))
                    {
                        prefix = false;
                        break;
                    }
                }
                if (prefix)
                {
                    newName += arg;
                }
                newName += parts[i];
            }
            foreach (string sep in separatorChars)
            {
                if (newName.EndsWith(sep))
                    newName = newName.Substring(0, newName.Length - 1);
            }
            return newName;
        }
    }
}
