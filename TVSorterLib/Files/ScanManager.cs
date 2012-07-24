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
    using TVSorter.Model;
    using TVSorter.Storage;
    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// Handles the scanning of files.
    /// </summary>
    internal class ScanManager
    {
        #region Fields

        /// <summary>
        /// The data provider to use.
        /// </summary>
        private readonly IDataProvider dataProvider;

        /// <summary>
        ///   The current settings of the system.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        /// The storage provider to use.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        /// The list of the tvshows.
        /// </summary>
        private readonly List<TvShow> tvShows;

        /// <summary>
        /// Indicating whether the file counts are being refreshed or not.
        /// </summary>
        private bool refreshingFileCounts;

        /// <summary>
        /// The list of unsuccessful matches.
        /// </summary>
        private List<string> unsuccessfulMatches;

        /// <summary>
        /// The list of shows that have been updated as part of the search.
        /// </summary>
        private List<TvShow> updatedShows;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanManager"/> class.
        /// </summary>
        /// <param name="storageProvider">
        /// The storage provider to use.
        /// </param>
        /// <param name="dataProvider">
        /// The data provider to use. 
        /// </param>
        internal ScanManager(IStorageProvider storageProvider, IDataProvider dataProvider)
        {
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;
            this.settings = Settings.LoadSettings(storageProvider);
            this.tvShows = TvShow.GetTvShows(storageProvider).ToList();
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

                // Any with only one result should be saved or where the first result is an exact match.
                if (results.Count == 1
                    ||
                    (results.Count > 1 && results[0].Name.Equals(showName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    TvShow show = TvShow.FromSearchResult(results[0]);
                    show.Save(storageProvider);
                    Logger.OnLogMessage(show, "Found show {0}", LogType.Info, show.Name);
                }
                else
                {
                    // Any 0 or more than 1 result should be added to the dictionary for user selection.
                    searchResults.Add(showName, results);
                    Logger.OnLogMessage(results, "Found {0} results for {1}", LogType.Info, results.Count, showName);
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
            this.unsuccessfulMatches = new List<string>();
            this.updatedShows = new List<TvShow>();
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
            this.unsuccessfulMatches = new List<string>();
            this.updatedShows = new List<TvShow>();
            this.refreshingFileCounts = true;

            // Search all the destination directories for positive matches.
            // Incomplete matches are filtered out.
            List<FileResult> matchedFiles =
                directories.SelectMany(dir => this.ProcessDirectory(dir, true)).Where(x => !x.Incomplete).ToList();

            Logger.OnLogMessage(this, "Updating file counts...", LogType.Info);

            // Get all of the matched episodes.
            List<Episode> matchedEpsiodes = matchedFiles.SelectMany(x => x.Episodes).ToList();

            // Get all of the shows and episodes.
            List<TvShow> shows = TvShow.GetTvShows(this.storageProvider).ToList();
            IEnumerable<Episode> allEpisodes = shows.SelectMany(x => x.Episodes);

            // Update the file count of the episodes.
            foreach (Episode episode in allEpisodes)
            {
                episode.FileCount = matchedEpsiodes.Count(x => episode.Equals(x));
            }

            Logger.OnLogMessage(this, "Saving file counts...", LogType.Info);

            // Save all the shows.
            shows.Save(this.storageProvider);
        }

        /// <summary>
        /// Searches a destination folder for files.
        /// This is intended to be called by the file manager.
        /// </summary>
        /// <param name="destination">
        /// The destination directory to search.
        /// </param>
        /// <returns>
        /// The collection of matched files.
        /// </returns>
        internal IEnumerable<FileResult> SearchDestinationFolder(IDirectoryInfo destination)
        {
            this.unsuccessfulMatches = new List<string>();
            this.updatedShows = new List<TvShow>();
            return this.ProcessDirectory(destination, false, true);
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
        /// <param name="ignoreShowUpdate">
        /// A value indicating whether the settings to update and lock shows should be ignored.
        /// </param>
        /// <returns>
        /// The TV show that was found. 
        /// </returns>
        private TvShow MatchShow(string fileName, int matchIndex, out string showName, bool ignoreShowUpdate)
        {
            showName = fileName.Substring(0, matchIndex - 1);

            // Replace any spacer characters with spaces
            showName = showName.RemoveSpacerChars();

            var names = new[]
                {
                    showName, showName.RemoveSpacerChars(), showName.AlphaNumericOnly(), showName.RemoveSpecialChars(),
                    showName.GetFileSafeName()
                }.Distinct();

            TvShow show =
                this.tvShows.FirstOrDefault(
                    x => x.GetShowNames().Intersect(names, StringComparer.InvariantCultureIgnoreCase).Any());

            if (this.settings.AddUnmatchedShows && show == null && !ignoreShowUpdate
                && !this.unsuccessfulMatches.Contains(showName))
            {
                Logger.OnLogMessage(this, "Attempting to add show {0}", LogType.Info, showName);

                // Attempt to add the show.
                List<TvShow> results = TvShow.SearchShow(showName, this.dataProvider);
                if (results.Count == 0)
                {
                    Logger.OnLogMessage(this, "Show not found.", LogType.Error);
                    this.unsuccessfulMatches.Add(showName);
                    return null;
                }

                // If there is only 1 result, of the name of the first result is a perfect match
                // then use it as the show.
                if (results.Count == 1 || results[0].Name.Equals(showName, StringComparison.InvariantCultureIgnoreCase))
                {
                    show = this.ProcessResult(showName, results[0]);
                    this.updatedShows.Add(show);
                }
            }

            // If Unlock matched shows is on and the show is locked.
            if (!this.refreshingFileCounts && this.settings.UnlockMatchedShows && show != null && show.Locked
                && !ignoreShowUpdate && !this.updatedShows.Contains(show))
            {
                // Unlock and update the show.
                show.Locked = false;
                show.Save(this.storageProvider);
                show.Update(this.dataProvider, this.storageProvider);
                this.updatedShows.Add(show);
            }

            return show;
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
        /// <param name="ignoreShowUpdate">
        /// A value indicating whether the settings for updating and locked shows should be ignored.
        /// </param>
        /// <returns>
        /// The list of identified files. 
        /// </returns>
        private IEnumerable<FileResult> ProcessDirectory(
            IDirectoryInfo directory, bool overrideRecurse = false, bool ignoreShowUpdate = false)
        {
            // Get the files where the extension is in the list of extensions.
            IEnumerable<IFileInfo> files =
                directory.GetFiles().Where(file => this.settings.FileExtensions.Contains(file.Extension));

            foreach (FileResult result in files.Select(info => this.ProcessFile(info, ignoreShowUpdate)))
            {
                if (result != null)
                {
                    yield return result;
                }
            }

            // If ignore show update is on then this is being run from the FileManger as part
            // of a move or copy operation. It should add to log that it is scanning the directory.
            if (!ignoreShowUpdate)
            {
                Logger.OnLogMessage(this, "Scanned directory: {0}", LogType.Info, directory.FullName);
            }

            if (!this.settings.RecurseSubdirectories && !overrideRecurse)
            {
                yield break;
            }

            IDirectoryInfo[] dirs = directory.GetDirectories();
            foreach (FileResult result in
                dirs.SelectMany(dir => this.ProcessDirectory(dir, overrideRecurse, ignoreShowUpdate)))
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
        /// <param name="ignoreShowUpdate">
        /// A value indicating whether the settings to update and lock shows should be ignored.
        /// </param>
        /// <returns>
        /// The results of the file process. 
        /// </returns>
        private FileResult ProcessFile(IFileInfo file, bool ignoreShowUpdate)
        {
            // Attempt to match to a regular express
            Match firstMatch = this.GetFirstMatch(file);

            if (firstMatch == null)
            {
                return null;
            }

            string showname;

            TvShow show = this.MatchShow(file.Name, firstMatch.Index, out showname, ignoreShowUpdate);

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

        /// <summary>
        /// Processes a TVShow result
        /// </summary>
        /// <param name="showName">
        /// The name of the show.
        /// </param>
        /// <param name="result">
        /// The matched result.
        /// </param>
        /// <returns>
        /// The matched TV Show.
        /// </returns>
        private TvShow ProcessResult(string showName, TvShow result)
        {
            // See if we already have the show under the same TVDB.
            TvShow show = this.tvShows.FirstOrDefault(x => x.Equals(result));
            if (show != null)
            {
                Logger.OnLogMessage(this, "{0} matched as {1}. Adding alternate name.", LogType.Info, showName, show.Name);
                show.AlternateNames.Add(showName);
                show.Save(this.storageProvider);
                return show;
            }

            Logger.OnLogMessage(this, "Matched {0} as a new show. Adding and updating...", LogType.Info, showName);

            // Doesn't exist with the same TVDB. Add a new show and update.
            show = TvShow.FromSearchResult(result);
            show.Save(this.storageProvider);
            show.Update(this.dataProvider, this.storageProvider);
            this.tvShows.Add(show);
            return show;
        }

        #endregion
    }
}