// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Handles the scanning of files.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.DAL
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using TVSorter.Types;

    #endregion

    /// <summary>
    /// Handles the scanning of files.
    /// </summary>
    internal class ScanManager : DalBase, IScanManager
    {
        #region Constants and Fields

        /// <summary>
        ///   An array of characters that can be used as spaces.
        /// </summary>
        private static readonly char[] SpacerChars = new[] { '.', '_', '-' };

        /// <summary>
        ///   The list of shows from storage.
        /// </summary>
        private readonly List<TvShow> shows;

        /// <summary>
        ///   The storage provider.
        /// </summary>
        private readonly IStorageProvider storage;

        /// <summary>
        ///   The current settings of the system.
        /// </summary>
        private Settings settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ScanManager" /> class. Initialises a new instance of the <see
        ///    cref="ScanManager" /> class.
        /// </summary>
        public ScanManager()
        {
            this.storage = Factory.StorageProvider;
            this.shows = this.storage.LoadTvShows();
            this.settings = this.storage.LoadSettings();
            this.storage.SettingsChanged += (sender, args) => this.settings = this.storage.LoadSettings();
            this.storage.ShowAdded += (sender, args) => this.shows.Add(args.Show);
            this.storage.ShowRemoved += (sender, args) => this.shows.Remove(args.Show);
            this.storage.ShowUpdated += (sender, args) =>
                {
                    this.shows.Remove(args.Show);
                    this.shows.Add(args.Show);
                };
        }

        #endregion

        #region Public Events

        /// <summary>
        ///   Occurs when the progress of an operation changes.
        /// </summary>
        public event EventHandler<ProgressChangedEventArgs> ProgressChanged;

        /// <summary>
        ///   Occurs when a refresh operation is completed.
        /// </summary>
        public event EventHandler<RefreshCompleteEventArgs> RefreshComplete;

        /// <summary>
        ///   Occurs when a refresh file counts operation is completed.
        /// </summary>
        public event EventHandler<RefreshFileCountsCompleteEventArgs> RefreshFileCountsComplete;

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Refresh the specified sub directory.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to refresh. 
        /// </param>
        /// <returns>
        /// The list of files indentified during the refresh operation. 
        /// </returns>
        public List<FileResult> Refresh(string subDirectory)
        {
            return this.Refresh(subDirectory, null);
        } 

        /// <summary>
        /// Refresh the specified sub directory.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to refresh. 
        /// </param>
        /// <param name="userState">
        /// The user State.
        /// </param>
        /// <returns>
        /// The list of files indentified during the refresh operation. 
        /// </returns>
        private List<FileResult> Refresh(string subDirectory, object userState)
        {
            var root = new DirectoryInfo(string.Concat(this.settings.SourceDirectory, subDirectory));
            int max = 0;
            int value = 0;
            var results = this.ProcessDirectory(root, ref max, ref value, userState);
            this.OnLogMessage("Refresh complete. Found {0} files", results.Count);
            return results;
        }

        /// <summary>
        /// Refreshes the specified sub directory asynchronously.
        /// </summary>
        /// <param name="subDirectory">
        /// The sub directory to refresh. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        public void RefreshAsync(string subDirectory, object userState)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        var results = this.Refresh(subDirectory, userState);
                        this.OnRefreshComplete(results, userState);
                    });
        }

        /// <summary>
        /// Searches for files in the output directory to set the file counts asynchronously.
        /// </summary>
        /// <param name="userState">
        /// The user's state oject. 
        /// </param>
        public void RefreshFileCountsAsync(object userState)
        {
            Task.Factory.StartNew(
                () =>
                    {
                        this.RefreshFileCounts(userState);
                        this.OnRefreshFileCountsComplete(userState);
                    });
        }

        /// <summary>
        /// Resets the episode of the specified file.
        /// </summary>
        /// <param name="result">
        /// The file to reset. 
        /// </param>
        public void ResetEpisode(FileResult result)
        {
            FileResult newResult = this.ProcessFile(result.InputFile, result.Show, null);
            result.Episode = newResult.Episode;
            result.OutputPath = newResult.OutputPath;
        }

        /// <summary>
        /// Resets the episode of the specified file.
        /// </summary>
        /// <param name="result">
        /// The file to reset. 
        /// </param>
        /// <param name="seasonNum">
        /// The season number to set it to. 
        /// </param>
        /// <param name="episodeNum">
        /// The episode number to set it to. 
        /// </param>
        public void ResetEpsiode(FileResult result, int seasonNum, int episodeNum)
        {
            List<Episode> episodes = this.storage.LoadEpisodes(result.Show);
            var eps = from ep in episodes where ep.EpisodeNumber == episodeNum && ep.SeasonNumber == seasonNum select ep;
            var episode = eps.FirstOrDefault();
            FileResult newResult = this.ProcessFile(result.InputFile, result.Show, episode);
            result.Episode = episode;
            result.OutputPath = newResult.OutputPath;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Formatted the name of an episode or show
        /// </summary>
        /// <param name="name">
        /// The name of the episode/show 
        /// </param>
        /// <param name="arg">
        /// The variable's argument, should be the separator char 
        /// </param>
        /// <returns>
        /// The formatted string 
        /// </returns>
        private string FormatName(string name, string arg)
        {
            // Break into separate words
            string[] parts = name.Split(' ');

            // Characters that can be used as separators, if there is one of these at the start
            // or end of a part then it won't put the arg character next to it. This is to avoid
            // outputs like Day.1.12.00.-.1.00 for a 24 episode since .-. looks silly.
            // Or names with titles in like Dr. Smith would be Mr..Smith - equally silly.
            var separatorChars = new[] { "-", ":", "_", ".", "," };

            // The new name to be returned
            string[] newName = { parts[0] };

            // Loop through each part appending it with the appropriate separator
            for (int i = 1; i < parts.Length; i++)
            {
                bool prefix = separatorChars.All(sep => !parts[i].StartsWith(sep) && !parts[i - 1].EndsWith(sep));

                // Check for the separator chars at the start of this part of the end of the next,
                // no extra one should be added
                // Add the arg character
                if (prefix)
                {
                    newName[0] += arg;
                }

                // Add the part
                newName[0] += parts[i];
            }

            // If the string ends with a separator character then remove it
            foreach (var sep in separatorChars)
            {
                if (newName[0].EndsWith(sep))
                {
                    newName[0] = newName[0].Substring(0, newName[0].Length - 1);
                }
            }

            return newName[0];
        }

        /// <summary>
        /// Format the numbers for output
        /// </summary>
        /// <param name="arg">
        /// The variable's argument, the length of the output num 
        /// </param>
        /// <param name="num">
        /// The number to format 
        /// </param>
        /// <returns>
        /// The formatted string 
        /// </returns>
        private string FormatNum(string arg, long num)
        {
            int length = int.Parse(arg);
            string zeros = "{0:";
            if (length == 0)
            {
                zeros += "0";
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    zeros += "0";
                }
            }

            zeros += "}";
            return string.Format(zeros, num);
        }

        /// <summary>
        /// Formats the output path of the episode.
        /// </summary>
        /// <param name="show">
        /// The show of the episode. 
        /// </param>
        /// <param name="episode">
        /// The episode. 
        /// </param>
        /// <param name="file">
        /// The file. 
        /// </param>
        /// <returns>
        /// The formatted output path. 
        /// </returns>
        private string FormatOutputPath(TvShow show, Episode episode, FileInfo file)
        {
            if (show == null || episode == null || file == null)
            {
                return string.Empty;
            }

            string formatString = show.UseCustomFormat ? show.CustomFormat : this.settings.DefaultOutputFormat;

            formatString = formatString.Replace("{Ext}", file.Extension);
            formatString = formatString.Replace("{FName}", show.FolderName);

            var regExp = new Regex(@"{([a-zA-Z]+)\(([^\)}]+)\)}");

            formatString = regExp.Replace(
                formatString, 
                match =>
                    {
                        string type = match.Groups[1].Value;
                        string arg = match.Groups[2].Value;
                        switch (type)
                        {
                            case "SName":
                                return this.FormatName(show.Name, arg);
                            case "EName":
                                return this.FormatName(episode.Name, arg);
                            case "ENum":
                                try
                                {
                                    return this.FormatNum(arg, episode.EpisodeNumber);
                                }
                                catch
                                {
                                    return match.Value;
                                }

                            case "SNum":
                                try
                                {
                                    return this.FormatNum(arg, episode.SeasonNumber);
                                }
                                catch
                                {
                                    return match.Value;
                                }

                            case "Date":
                                try
                                {
                                    return episode.FirstAir.ToString(arg);
                                }
                                catch
                                {
                                    return match.Value;
                                }
                        }

                        return match.Value;
                    });

            // Replace : with .
            formatString = formatString.Replace(':', '.');

            // Remove any characters that can't be in a filename
            return
                Path.GetInvalidFileNameChars().Where(ch => !ch.Equals(Path.DirectorySeparatorChar)).Aggregate(
                    formatString, 
                    (current, ch) => current.Replace(ch.ToString(CultureInfo.InvariantCulture), string.Empty));
        }

        /// <summary>
        /// Attempts to match the file name to to a show in storage.
        /// </summary>
        /// <param name="fileName">
        /// The file name of the episode. 
        /// </param>
        /// <param name="matchIndex">
        /// The index that the season and epsiode were identified in the file name. 
        /// </param>
        /// <returns>
        /// The TV show that was found. 
        /// </returns>
        private TvShow MatchShow(string fileName, int matchIndex)
        {
            string showName = fileName.Substring(0, matchIndex - 1);

            // Replace any spacer characters with spaces
            showName = SpacerChars.Aggregate(showName, (current, ch) => current.Replace(ch, ' '));

            // Remove any duplicate spaces
            showName =
                showName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Aggregate(
                    (x, y) => string.Concat(x, " ", y));

            // Match the show's name against the name, folder name and any alternate names.
            // fileSafeNmae removes any invalid file name chars from the show's name to match against as well.
            return (from show in this.shows
                    let fileSafeName = new string(show.Name.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray())
                    where show.Name.Equals(showName, StringComparison.InvariantCultureIgnoreCase)
                        || show.FolderName.Equals(showName, StringComparison.InvariantCultureIgnoreCase)
                        || fileSafeName.Equals(showName, StringComparison.InvariantCultureIgnoreCase)
                        ||
                        (show.AlternateNames != null
                         && show.AlternateNames.Contains(showName, StringComparer.InvariantCultureIgnoreCase))
                    select show).FirstOrDefault();
        }

        /// <summary>
        /// Raises a progress event.
        /// </summary>
        /// <param name="max">
        /// The max value of the progress. 
        /// </param>
        /// <param name="value">
        /// The current value of the progress. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnProgressChanged(int max, int value, object userState)
        {
            if (this.ProgressChanged != null)
            {
                this.ProgressChanged(this, new ProgressChangedEventArgs(max, value, userState));
            }
        }

        /// <summary>
        /// Raises a refresh complete event.
        /// </summary>
        /// <param name="results">
        /// The results of the operation. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnRefreshComplete(List<FileResult> results, object userState)
        {
            if (this.RefreshComplete != null)
            {
                this.RefreshComplete(this, new RefreshCompleteEventArgs(results, userState));
            }
        }

        /// <summary>
        /// Raises a refresh file counts complete event.
        /// </summary>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        private void OnRefreshFileCountsComplete(object userState)
        {
            if (this.RefreshFileCountsComplete != null)
            {
                this.RefreshFileCountsComplete(this, new RefreshFileCountsCompleteEventArgs(userState));
            }
        }

        /// <summary>
        /// Processes the specified directory looking for episodes.
        /// </summary>
        /// <param name="directory">
        /// The directory to search. 
        /// </param>
        /// <param name="max">
        /// The max value of the progress indication. 
        /// </param>
        /// <param name="value">
        /// The current value of te progress indication. 
        /// </param>
        /// <param name="userState">
        /// The user's state object. 
        /// </param>
        /// <param name="overrideRecurse">
        /// A value indicating whether the setting for recurse subdirectories should be overriden. 
        /// </param>
        /// <returns>
        /// The list of identified files. 
        /// </returns>
        private List<FileResult> ProcessDirectory(
            DirectoryInfo directory, ref int max, ref int value, object userState, bool overrideRecurse = false)
        {
            var results = new List<FileResult>();

            // Get the files where the extension is in the list of extensions.
            var files =
                directory.GetFiles().Where(file => this.settings.FileExtensions.Contains(file.Extension)).ToList();
            max += files.Count;

            foreach (var result in files.Select(this.ProcessFile))
            {
                if (result != null)
                {
                    results.Add(result);
                }

                value++;
                this.OnProgressChanged(max, value, userState);
            }

            if (this.settings.RecurseSubdirectories || overrideRecurse)
            {
                DirectoryInfo[] dirs = directory.GetDirectories();
                foreach (var dir in dirs)
                {
                    results.AddRange(this.ProcessDirectory(dir, ref max, ref value, userState, overrideRecurse));
                }
            }

            return results;
        }

        /// <summary>
        /// Gets the necessary info from the file name and return the episode object
        /// </summary>
        /// <param name="match">
        /// The regexp match 
        /// </param>
        /// <param name="show">
        /// The show. 
        /// </param>
        /// <returns>
        /// The episode object 
        /// </returns>
        private Episode ProcessEpisode(Match match, TvShow show)
        {
            // Determine if the match was a season/episode or a date.
            Group season = match.Groups["S"];
            Group episode = match.Groups["E"];
            Group year = match.Groups["Y"];
            Group month = match.Groups["M"];
            Group day = match.Groups["D"];

            if (show == null)
            {
                return null;
            }

            var episodes = this.storage.LoadEpisodes(show);

            Episode ep;

            if (season.Success && episode.Success)
            {
                int seasonNum = int.Parse(season.ToString());
                int episodeNum = int.Parse(episode.ToString());
                ep = episodes.FirstOrDefault(x => x.EpisodeNumber == episodeNum && x.SeasonNumber == seasonNum);
            }
            else if (year.Success && month.Success && day.Success)
            {
                DateTime date = DateTime.Parse(string.Concat(year, "-", month, "-", day));
                ep = episodes.FirstOrDefault(x => x.FirstAir.Equals(date));
            }
            else
            {
                throw new Exception("Invalid regular expression.");
            }

            return ep;
        }

        /// <summary>
        /// Processes the specified file.
        /// </summary>
        /// <param name="file">
        /// The file to process. 
        /// </param>
        /// <returns>
        /// The results of the file process. 
        /// </returns>
        private FileResult ProcessFile(FileInfo file)
        {
            return this.ProcessFile(file, null, null);
        }

        /// <summary>
        /// Processes the specified file, overriding the show and episode search.
        /// </summary>
        /// <param name="file">
        /// The file to process. 
        /// </param>
        /// <param name="show">
        /// The show to use - null if based on file name. 
        /// </param>
        /// <param name="episode">
        /// The episode to use - null if based on file name. 
        /// </param>
        /// <returns>
        /// The results of the file process. 
        /// </returns>
        private FileResult ProcessFile(FileInfo file, TvShow show, Episode episode)
        {
            // Attempt to match to a regular express
            Match firstMatch = (from regex in this.settings.RegularExpressions
                                let match = Regex.Match(file.Name, regex, RegexOptions.IgnoreCase)
                                where match.Success
                                select match).FirstOrDefault();

            if (firstMatch == null)
            {
                return null;
            }

            if (show == null)
            {
                show = this.MatchShow(file.Name, firstMatch.Index);
            }

            if (episode == null)
            {
                episode = this.ProcessEpisode(firstMatch, show);
            }

            var output = this.FormatOutputPath(show, episode, file);

            var incomplete = string.IsNullOrWhiteSpace(output) || string.IsNullOrWhiteSpace(episode.Name);

            return new FileResult
                {
                    Episode = episode, 
                    InputFile = file, 
                    Show = show, 
                    OutputPath = output,
                    Incomplete = incomplete
                };
        }

        /// <summary>
        /// Searches for files in the output directory to set the file counts.
        /// </summary>
        /// <param name="userState">
        /// The user's state oject. 
        /// </param>
        private void RefreshFileCounts(object userState)
        {
            var directories = this.settings.DestinationDirectories;
            int max = 1;
            int value = 0;
            var results = new List<FileResult>();

            // Search all the destination directories.
            foreach (var directory in directories)
            {
                var files = this.ProcessDirectory(new DirectoryInfo(directory), ref max, ref value, userState, true);
                results.AddRange(files);
            }

            // Group the results by episode.
            var episodeGroups = results.GroupBy(x => x.Episode);
            foreach (var group in episodeGroups)
            {
                if (group.Key != null)
                {
                    // Set the file count to the size of the group.
                    var count = group.Count();
                    foreach (var ep in group)
                    {
                        ep.Episode.FileCount = count;
                    }
                }
            }

            // Group the result by show for saving them.
            var showGroups = results.GroupBy(x => x.Show);
            foreach (var group in showGroups)
            {
                if (group.Key != null)
                {
                    // Save the episodes for each show.
                    this.storage.SaveEpisodes(group.Key, group.Select(x => x.Episode), false);
                }
            }
        }

        #endregion
    }
}