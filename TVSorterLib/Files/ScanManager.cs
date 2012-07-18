// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Files
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using TVSorter.Data;
    using TVSorter.Storage;

    #endregion

    /// <summary>
    /// Handles the scanning of files.
    /// </summary>
    internal class ScanManager
    {
        #region Static Fields

        /// <summary>
        ///   An array of characters that can be used as spaces.
        /// </summary>
        private static readonly char[] SpacerChars = new[] { '.', '_', '-' };

        #endregion

        #region Fields

        /// <summary>
        /// The storage provider to use.
        /// </summary>
        private readonly IStorageProvider provider;

        /// <summary>
        ///   The current settings of the system.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        /// The list of the tvshows.
        /// </summary>
        private readonly List<TvShow> tvShows;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanManager"/> class.
        /// </summary>
        /// <param name="provider">
        /// The provider.
        /// </param>
        internal ScanManager(IStorageProvider provider)
        {
            this.provider = provider;
            this.settings = Settings.LoadSettings(provider);
            this.tvShows = TvShow.GetTvShows(provider).ToList();
        }

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
            var root = new DirectoryInfo(string.Concat(this.settings.SourceDirectory, subDirectory));
            return this.ProcessDirectory(root).ToList();
        }

        /// <summary>
        /// Searches for files in the output directory to set the file counts.
        /// </summary>
        public void RefreshFileCounts()
        {
            List<string> directories = this.settings.DestinationDirectories;
            var results = new List<FileResult>();

            // Search all the destination directories.
            foreach (string directory in directories)
            {
                List<FileResult> files = this.ProcessDirectory(new DirectoryInfo(directory), true).ToList();
                results.AddRange(files);
            }

            // Group the results by episode.
            IEnumerable<IGrouping<Episode, FileResult>> episodeGroups = results.GroupBy(x => x.Episode);
            foreach (var group in episodeGroups)
            {
                if (group.Key != null)
                {
                    // Set the file count to the size of the group.
                    int count = group.Count();
                    foreach (FileResult ep in group)
                    {
                        ep.Episode.FileCount = count;
                    }
                }
            }

            IEnumerable<TvShow> shows = from result in results
                                        group result by result.Show
                                        into showGroup where showGroup.Key != null select showGroup.Key;

            shows.Save(this.provider);
        }

        /// <summary>
        /// Resets the show of the specified result.
        /// </summary>
        /// <param name="result">
        /// The result to modify.
        /// </param>
        /// <param name="show">
        /// The show to set the result to.
        /// </param>
        public void ResetShow(FileResult result, TvShow show)
        {
            result.Show = show;
            Match match = this.GetFirstMatch(result.InputFile);
            if (match == null)
            {
                result.Episode = null;
            }

            result.Episode = this.ProcessEpisode(match, show);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Searches for new TVShows.
        /// </summary>
        /// <param name="storageProvider">
        /// The stroage provider
        /// </param>
        /// <param name="dataProvider">
        /// The data provider. 
        /// </param>
        /// <returns>
        /// The ambiguous results of the search for user selection.
        /// </returns>
        internal static Dictionary<string, List<TvShow>> SearchNewShows(
            IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            Settings settings = Settings.LoadSettings(storageProvider);
            var showDirs = new List<string>();
            List<string> existingShows = TvShow.GetTvShows(storageProvider).Select(x => x.FolderName).ToList();
            foreach (DirectoryInfo dirInfo in settings.GetDestinationDirectories())
            {
                // Add all of dirInfo's subdirectories where they don't already exist and 
                // there isn't already a tv show with it as a folder name.
                showDirs.AddRange(
                    from dir in dirInfo.GetDirectories()
                    where !showDirs.Contains(dir.Name) && !existingShows.Contains(dir.Name)
                    select dir.Name);
            }

            // Sort the directories so the show's are added alphabetically.
            showDirs.Sort();

            var searchResults = new Dictionary<string, List<TvShow>>();
            foreach (string showName in showDirs)
            {
                // Search for each of the shows using the directory name as the show name.
                List<TvShow> results = TvShow.SearchShow(showName, dataProvider);

                // Any with only one result should be saved.
                if (results.Count == 1)
                {
                    TvShow show = TvShow.FromSearchResult(results[0]);
                    show.Save(storageProvider);
                    Logger.OnLogMessage(show, "Found show {0}", show.Name);
                }
                else
                {
                    // Any 0 or more than 1 result should be added to the dictionary for user selection.
                    searchResults.Add(showName, results);
                    Logger.OnLogMessage(results, "Found {0} results for {1}", results.Count, showName);
                }
            }

            return searchResults;
        }

        /// <summary>
        /// Gets the first match for the specified file.
        /// </summary>
        /// <param name="file">
        /// The file to match.
        /// </param>
        /// <returns>
        /// The first succcessful match. Null if none.
        /// </returns>
        private Match GetFirstMatch(FileInfo file)
        {
            return (from regex in this.settings.RegularExpressions
                    let match = Regex.Match(file.Name, regex, RegexOptions.IgnoreCase)
                    where match.Success
                    select match).FirstOrDefault();
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
        /// <param name="showName">
        /// The name of the show as seen by the program. 
        /// </param>
        /// <returns>
        /// The TV show that was found. 
        /// </returns>
        private TvShow MatchShow(string fileName, int matchIndex, out string showName)
        {
            showName = fileName.Substring(0, matchIndex - 1);

            // Replace any spacer characters with spaces
            showName = SpacerChars.Aggregate(showName, (current, ch) => current.Replace(ch, ' '));

            string name = showName;
            return
                this.tvShows.FirstOrDefault(
                    x => x.GetShowNames().Contains(name, StringComparer.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Processes the specified directory looking for episodes.
        /// </summary>
        /// <param name="directory">
        /// The directory to search. 
        /// </param>
        /// <param name="overrideRecurse">
        /// A value indicating whether the setting for recurse subdirectories should be overriden. 
        /// </param>
        /// <returns>
        /// The list of identified files. 
        /// </returns>
        private IEnumerable<FileResult> ProcessDirectory(DirectoryInfo directory, bool overrideRecurse = false)
        {
            // Get the files where the extension is in the list of extensions.
            List<FileInfo> files =
                directory.GetFiles().Where(file => this.settings.FileExtensions.Contains(file.Extension)).ToList();

            foreach (FileResult result in files.Select(this.ProcessFile))
            {
                if (result != null)
                {
                    yield return result;
                }
            }

            Logger.OnLogMessage(this, "Scanned directory: {0}", directory.FullName);

            if (!this.settings.RecurseSubdirectories && !overrideRecurse)
            {
                yield break;
            }

            DirectoryInfo[] dirs = directory.GetDirectories();
            foreach (FileResult result in dirs.SelectMany(dir => this.ProcessDirectory(dir, overrideRecurse)))
            {
                yield return result;
            }
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
            Group season = match.Groups["S"];
            Group episode = match.Groups["E"];
            Group year = match.Groups["Y"];
            Group month = match.Groups["M"];
            Group day = match.Groups["D"];

            if (show == null)
            {
                return null;
            }

            Episode ep;

            // Determine if the match was a season/episode or a date.
            if (season.Success && episode.Success)
            {
                int seasonNum = int.Parse(season.ToString());
                int episodeNum = int.Parse(episode.ToString());
                ep = show.Episodes.FirstOrDefault(x => x.EpisodeNumber == episodeNum && x.SeasonNumber == seasonNum);
            }
            else if (year.Success && month.Success && day.Success)
            {
                DateTime date = DateTime.Parse(string.Concat(year, "-", month, "-", day));
                ep = show.Episodes.FirstOrDefault(x => x.FirstAir.Equals(date));
            }
            else
            {
                throw new Exception("Invalid regular expression.");
            }

            return ep;
        }

        /// <summary>
        /// Processes the specified file, overriding the show and episode search.
        /// </summary>
        /// <param name="file">
        /// The file to process. 
        /// </param>
        /// <returns>
        /// The results of the file process. 
        /// </returns>
        private FileResult ProcessFile(FileInfo file)
        {
            // Attempt to match to a regular express
            Match firstMatch = this.GetFirstMatch(file);

            if (firstMatch == null)
            {
                return null;
            }

            string showname;

            TvShow show = this.MatchShow(file.Name, firstMatch.Index, out showname);

            Episode episode = null;

            if (show != null)
            {
                episode = this.ProcessEpisode(firstMatch, show);
            }

            return new FileResult { Episode = episode, InputFile = file, Show = show, ShowName = showname };
        }

        #endregion
    }
}