// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Handles the scanning of files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Files
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using TVSorter.Data;
    using TVSorter.Storage;
    using TVSorter.Wrappers;

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
        /// The list of files identified during the refresh operation. 
        /// </returns>
        public List<FileResult> Refresh(string subDirectory)
        {
            var root = new DirectoryInfoWrap(string.Concat(this.settings.SourceDirectory, subDirectory));
            return this.Refresh(root);
        }

        /// <summary>
        /// Searches for files in the output directory to set the file counts.
        /// </summary>
        public void RefreshFileCounts()
        {
            this.RefreshFileCounts(this.settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x)));
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

            result.Episodes = this.ProcessEpisode(match, show).ToList();
            result.Episode = result.Episodes.FirstOrDefault();
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
        /// <param name="directories">
        /// The directories to search.
        /// </param>
        /// <returns>
        /// The ambiguous results of the search for user selection.
        /// </returns>
        internal static Dictionary<string, List<TvShow>> SearchNewShows(
            IStorageProvider storageProvider, IDataProvider dataProvider, IEnumerable<IDirectoryInfo> directories)
        {
            Settings settings = Settings.LoadSettings(storageProvider);
            var showDirs = new List<string>();
            List<string> existingShows = TvShow.GetTvShows(storageProvider).Select(x => x.FolderName).ToList();
            foreach (IDirectoryInfo dirInfo in directories)
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
        /// Refresh the specified directory.
        /// </summary>
        /// <param name="directoryInfo">
        /// The directory to refresh.
        /// </param>
        /// <returns>
        /// The list of files identified during the refresh operation.
        /// </returns>
        internal List<FileResult> Refresh(IDirectoryInfo directoryInfo)
        {
            return this.ProcessDirectory(directoryInfo).ToList();
        }

        /// <summary>
        /// Searches for files in specified directories to set the file counts.
        /// </summary>
        /// <param name="directories">
        /// The directories to search.
        /// </param>
        internal void RefreshFileCounts(IEnumerable<IDirectoryInfo> directories)
        {
            // Search all the destination directories for positive matches.
            // Incomplete matches are filtered out.
            var matchedFiles =
                directories.SelectMany(directory => this.ProcessDirectory(directory, true)).Where(x => !x.Incomplete).
                    ToList();

            // Get all of the matched episodes.
            var matchedEpsiodes = matchedFiles.SelectMany(x => x.Episodes).ToList();


            // Get all of the shows and episodes.
            var shows = TvShow.GetTvShows(this.provider).ToList();
            var allEpisodes = shows.SelectMany(x => x.Episodes);

            // Update the file count of the episodes.
            foreach (var episode in allEpisodes)
            {
                episode.FileCount = matchedEpsiodes.Count(x => episode.Equals(x));
            }

            // Save all the shows.
            shows.Save(this.provider);
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
        private Match GetFirstMatch(IFileInfo file)
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
        private IEnumerable<FileResult> ProcessDirectory(IDirectoryInfo directory, bool overrideRecurse = false)
        {
            // Get the files where the extension is in the list of extensions.
            var files = directory.GetFiles().Where(file => this.settings.FileExtensions.Contains(file.Extension));

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

            IDirectoryInfo[] dirs = directory.GetDirectories();
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
        /// The episode objects that have been matched.
        /// </returns>
        private IEnumerable<Episode> ProcessEpisode(Match match, TvShow show)
        {
            Group season = match.Groups["S"];
            Group episode = match.Groups["E"];
            Group year = match.Groups["Y"];
            Group month = match.Groups["M"];
            Group day = match.Groups["D"];

            if (show == null)
            {
                yield break;
            }

            // Determine if the match was a season/episode or a date.
            if (season.Success && episode.Success)
            {
                int seasonNum = int.Parse(season.ToString());

                // There is the possibility of multiple episodes being matches
                foreach (Capture episodeGroup in match.Groups["E"].Captures)
                {
                    int episodeNum = int.Parse(episodeGroup.ToString());
                    yield return
                        show.Episodes.FirstOrDefault(x => x.EpisodeNumber == episodeNum && x.SeasonNumber == seasonNum);
                }
            }
            else if (year.Success && month.Success && day.Success)
            {
                DateTime date = DateTime.Parse(string.Concat(year, "-", month, "-", day));
                yield return show.Episodes.FirstOrDefault(x => x.FirstAir.Equals(date));
            }
            else
            {
                throw new Exception("Invalid regular expression.");
            }
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
        private FileResult ProcessFile(IFileInfo file)
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
            IList<Episode> episodes = null;

            if (show != null)
            {
                episodes = this.ProcessEpisode(firstMatch, show).ToList();
                if (episodes.Count > 0)
                {
                    episode = episodes.First();
                }
            }

            return new FileResult
                {
                   Episode = episode, Episodes = episodes, InputFile = file, Show = show, ShowName = showname 
                };
        }

        #endregion
    }
}