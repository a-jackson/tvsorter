// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ScanManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Handles the scanning of files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TVSorter.Data;
using TVSorter.Model;
using TVSorter.Repostitory;
using TVSorter.Storage;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    /// <summary>
    ///     Handles the scanning of files.
    /// </summary>
    public class ScanManager : IScanManager
    {
        /// <summary>
        ///     The data provider to use.
        /// </summary>
        private readonly IDataProvider dataProvider;

        /// <summary>
        ///     The current settings of the system.
        /// </summary>
        private readonly Settings settings;

        /// <summary>
        ///     The storage provider to use.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     The TV show repository.
        /// </summary>
        private readonly ITvShowRepository tvShowRepository;

        /// <summary>
        ///     The list of the TV shows.
        /// </summary>
        private readonly List<TvShow> tvShows;

        /// <summary>
        ///     Indicating whether the file counts are being refreshed or not.
        /// </summary>
        private bool refreshingFileCounts;

        /// <summary>
        ///     The list of unsuccessful matches.
        /// </summary>
        private List<string> unsuccessfulMatches;

        /// <summary>
        ///     The list of shows that have been updated as part of the search.
        /// </summary>
        private List<TvShow> updatedShows;

        /// <summary>
        ///     Initialises a new instance of the <see cref="ScanManager" /> class.
        /// </summary>
        /// <param name="storageProvider">
        ///     The storage provider to use.
        /// </param>
        /// <param name="dataProvider">
        ///     The data provider to use.
        /// </param>
        /// <param name="tvShowRepository">
        ///     The TV show repository.
        /// </param>
        public ScanManager(
            IStorageProvider storageProvider,
            IDataProvider dataProvider,
            ITvShowRepository tvShowRepository)
        {
            this.storageProvider = storageProvider;
            this.dataProvider = dataProvider;
            settings = storageProvider.Settings;
            this.tvShowRepository = tvShowRepository;
            tvShows = tvShowRepository.GetTvShows().ToList();
            this.tvShowRepository.TvShowAdded += TvShowAdded;
            this.tvShowRepository.TvShowRemoved += TvShowRemoved;
            this.tvShowRepository.TvShowChanged += TvShowChanged;
        }

        /// <summary>
        ///     Refresh the specified sub directory.
        /// </summary>
        /// <param name="subDirectory">
        ///     The sub directory to refresh.
        /// </param>
        /// <returns>
        ///     The list of files identified during the refresh operation.
        /// </returns>
        public List<FileResult> Refresh(string subDirectory)
        {
            var root = new DirectoryInfoWrap(string.Concat(settings.SourceDirectory, subDirectory));
            return Refresh(root);
        }

        /// <summary>
        ///     Searches for files in the output directory to set the file counts.
        /// </summary>
        public void RefreshFileCounts()
        {
            RefreshFileCounts(settings.DestinationDirectories.Select(x => new DirectoryInfoWrap(x)));
        }

        /// <summary>
        ///     Resets the show of the specified result.
        /// </summary>
        /// <param name="result">
        ///     The result to modify.
        /// </param>
        /// <param name="show">
        ///     The show to set the result to.
        /// </param>
        public void ResetShow(FileResult result, TvShow show)
        {
            result.Show = show;
            var firstMatch = GetMatches(result.InputFile).FirstOrDefault();
            if (firstMatch == null)
            {
                result.Episode = null;
            }
            else
            {
                result.Episodes = ProcessEpisode(firstMatch, show).ToList();
                result.Episode = result.Episodes.FirstOrDefault();
            }
        }

        /// <summary>
        ///     Searches for new TVShows.
        /// </summary>
        /// <param name="directories">
        ///     The directories to search in.
        /// </param>
        /// <returns>
        ///     The ambiguous results of the search for user selection.
        /// </returns>
        public Dictionary<string, List<TvShow>> SearchNewShows(IEnumerable<IDirectoryInfo> directories)
        {
            var showDirs = new List<string>();
            var existingShows = tvShowRepository.GetTvShows().Select(x => x.FolderName).ToList();

            // Add all of dirInfo's subdirectories where they don't already exist and
            // there isn't already a tv show with it as a folder name.
            foreach (var dirInfo in directories)
            {
                showDirs.AddRange(
                    from dir in dirInfo.GetDirectories()
                    where !showDirs.Contains(dir.Name) && !existingShows.Contains(dir.Name)
                    select dir.Name);
            }

            // Sort the directories so the show's are added alphabetically.
            showDirs.Sort();

            var searchResults = new Dictionary<string, List<TvShow>>();
            foreach (var showName in showDirs)
            {
                // Search for each of the shows using the directory name as the show name.
                var results = tvShowRepository.SearchShow(showName);

                // Any with only one result should be saved or where the first result is an exact match.
                if ((results.Count == 1) ||
                    ((results.Count > 1) &&
                     results[0].Name.Equals(showName, StringComparison.InvariantCultureIgnoreCase)))
                {
                    var show = tvShowRepository.FromSearchResult(results[0]);
                    tvShowRepository.Save(show);
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
        ///     Searches a destination folder for files.
        ///     This is intended to be called by the file manager.
        /// </summary>
        /// <param name="destination">
        ///     The destination directory to search.
        /// </param>
        /// <returns>
        ///     The collection of matched files.
        /// </returns>
        public IEnumerable<FileResult> SearchDestinationFolder(IDirectoryInfo destination)
        {
            unsuccessfulMatches = new List<string>();
            updatedShows = new List<TvShow>();
            return ProcessDirectory(destination, false, true);
        }

        /// <summary>
        ///     Refresh the specified directory.
        /// </summary>
        /// <param name="directoryInfo">
        ///     The directory to refresh.
        /// </param>
        /// <returns>
        ///     The list of files identified during the refresh operation.
        /// </returns>
        internal List<FileResult> Refresh(IDirectoryInfo directoryInfo)
        {
            unsuccessfulMatches = new List<string>();
            updatedShows = new List<TvShow>();
            return ProcessDirectory(directoryInfo).ToList();
        }

        /// <summary>
        ///     Searches for files in specified directories to set the file counts.
        /// </summary>
        /// <param name="directories">
        ///     The directories to search.
        /// </param>
        internal void RefreshFileCounts(IEnumerable<IDirectoryInfo> directories)
        {
            unsuccessfulMatches = new List<string>();
            updatedShows = new List<TvShow>();
            refreshingFileCounts = true;

            // Search all the destination directories for positive matches.
            var files = directories.SelectMany(dir => ProcessDirectory(dir, true));
            var matchedFiles = files.Where(x => !x.Incomplete).ToList();
            var unmatchedFiles = files.Where(x => x.Incomplete).ToList();

            // Log the unmatched files.
            if (unmatchedFiles.Any())
            {
                Logger.OnLogMessage(this, "Unable to match {0} files", LogType.Error, unmatchedFiles.Count);
                foreach (var result in unmatchedFiles)
                {
                    Logger.OnLogMessage(
                        this,
                        "Unable to match: " + result.InputFile.FullName.Truncate(),
                        LogType.Error);
                }
            }

            Logger.OnLogMessage(this, "Updating file counts...", LogType.Info);

            // Get all of the matched episodes.
            var matchedEpsiodes = matchedFiles.SelectMany(x => x.Episodes).ToList();

            // Get all of the shows and episodes.
            var shows = tvShowRepository.GetTvShows().ToList();
            var allEpisodes = shows.SelectMany(x => x.Episodes);

            // Update the file count of the episodes.
            foreach (var episode in allEpisodes)
            {
                episode.FileCount = matchedEpsiodes.Count(x => episode.Equals(x));
            }

            Logger.OnLogMessage(this, "Saving file counts...", LogType.Info);

            // Save all the shows.
            shows.Save(storageProvider);
        }

        /// <summary>
        ///     Gets the matches for the specified file.
        /// </summary>
        /// <param name="file">
        ///     The file to match.
        /// </param>
        /// <returns>
        ///     The collection of matches.
        /// </returns>
        private IEnumerable<Match> GetMatches(IFileInfo file)
        {
            foreach (var regex in settings.RegularExpressions)
            {
                var match = Regex.Match(file.Name, regex, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    yield return match;
                }
            }
        }

        /// <summary>
        ///     Attempts to match the file name to to a show in storage.
        /// </summary>
        /// <param name="fileName">
        ///     The file name of the episode.
        /// </param>
        /// <param name="matchIndex">
        ///     The index that the season and episode were identified in the file name.
        /// </param>
        /// <param name="showName">
        ///     The name of the show as seen by the program.
        /// </param>
        /// <param name="ignoreShowUpdate">
        ///     A value indicating whether the settings to update and lock shows should be ignored.
        /// </param>
        /// <returns>
        ///     The TV show that was found.
        /// </returns>
        private TvShow MatchShow(string fileName, int matchIndex, out string showName, bool ignoreShowUpdate)
        {
            if (matchIndex == 0)
            {
                // A season and episode number has been found but there is no show name.
                showName = string.Empty;
                return null;
            }

            showName = fileName.Substring(0, matchIndex - 1);

            // Replace any spacer characters with spaces
            showName = showName.RemoveSpacerChars();

            var names = new[]
            {
                showName,
                showName.RemoveSpacerChars(),
                showName.AlphaNumericOnly(),
                showName.RemoveSpecialChars(),
                showName.GetFileSafeName()
            }.Distinct();

            var show = tvShows.FirstOrDefault(
                x => x.GetShowNames().Intersect(names, StringComparer.InvariantCultureIgnoreCase).Any());

            if (settings.AddUnmatchedShows &&
                (show == null) &&
                !ignoreShowUpdate &&
                !unsuccessfulMatches.Contains(showName))
            {
                Logger.OnLogMessage(this, "Attempting to add show {0}", LogType.Info, showName);

                // Attempt to add the show.
                var results = tvShowRepository.SearchShow(showName);
                if (results.Count == 0)
                {
                    Logger.OnLogMessage(this, "Show not found.", LogType.Error);
                    unsuccessfulMatches.Add(showName);
                    return null;
                }

                // If there is only 1 result, of the name of the first result is a perfect match
                // then use it as the show.
                if ((results.Count == 1) ||
                    results[0].Name.Equals(showName, StringComparison.InvariantCultureIgnoreCase))
                {
                    show = ProcessResult(showName, results[0]);
                    updatedShows.Add(show);
                }
            }

            // If Unlock matched shows is on and the show is locked.
            if (!refreshingFileCounts &&
                settings.UnlockMatchedShows &&
                (show != null) &&
                show.Locked &&
                !ignoreShowUpdate &&
                !updatedShows.Contains(show))
            {
                // Unlock and update the show.
                show.Locked = false;
                tvShowRepository.Save(show);
                tvShowRepository.Update(show);
                updatedShows.Add(show);
            }

            return show;
        }

        /// <summary>
        ///     Processes the specified directory looking for episodes.
        /// </summary>
        /// <param name="directory">
        ///     The directory to search.
        /// </param>
        /// <param name="overrideRecurse">
        ///     A value indicating whether to override the setting to recursively search subdirectories.
        /// </param>
        /// <param name="ignoreShowUpdate">
        ///     A value indicating whether the settings for updating and locked shows should be ignored.
        /// </param>
        /// <returns>
        ///     The list of identified files.
        /// </returns>
        private IEnumerable<FileResult> ProcessDirectory(
            IDirectoryInfo directory,
            bool overrideRecurse = false,
            bool ignoreShowUpdate = false)
        {
            // Get the files where the extension is in the list of extensions.
            var files = directory.GetFiles().Where(file => settings.FileExtensions.Contains(file.Extension));

            foreach (var result in files.Select(info => ProcessFile(info, ignoreShowUpdate)))
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
                Logger.OnLogMessage(this, "Scanned directory: {0}", LogType.Info, directory.FullName.Truncate());
            }

            if (!settings.RecurseSubdirectories && !overrideRecurse)
            {
                yield break;
            }

            var dirs = directory.GetDirectories()
                .Where(
                    d => !d.DirectoryAttributes.HasFlag(FileAttributes.System) &&
                         !settings.IgnoredDirectories.Contains(d.FullName))
                .ToArray();
            foreach (var result in dirs.SelectMany(dir => ProcessDirectory(dir, overrideRecurse, ignoreShowUpdate)))
            {
                yield return result;
            }
        }

        /// <summary>
        ///     Gets the necessary info from the file name and return the episode object
        /// </summary>
        /// <param name="match">
        ///     The regular expression's match
        /// </param>
        /// <param name="show">
        ///     The show.
        /// </param>
        /// <returns>
        ///     The episode objects that have been matched.
        /// </returns>
        private IEnumerable<Episode> ProcessEpisode(Match match, TvShow show)
        {
            var season = match.Groups["S"];
            var episode = match.Groups["E"];
            var year = match.Groups["Y"];
            var month = match.Groups["M"];
            var day = match.Groups["D"];

            if (show == null)
            {
                yield break;
            }

            // Determine if the match was a season/episode or a date.
            if (season.Success && episode.Success)
            {
                var seasonNum = int.Parse(season.ToString());

                // There is the possibility of multiple episodes being matches
                foreach (Capture episodeGroup in match.Groups["E"].Captures)
                {
                    var episodeNum = int.Parse(episodeGroup.ToString());
                    yield return show.Episodes.FirstOrDefault(
                        x => (x.EpisodeNumber == episodeNum) && (x.SeasonNumber == seasonNum));
                }
            }
            else if (year.Success && month.Success && day.Success)
            {
                var date = DateTime.Parse(string.Concat(year, "-", month, "-", day));
                yield return show.Episodes.FirstOrDefault(x => x.FirstAir.Equals(date));
            }
            else
            {
                throw new Exception("Invalid regular expression.");
            }
        }

        /// <summary>
        ///     Processes the specified file, overriding the show and episode search.
        /// </summary>
        /// <param name="file">
        ///     The file to process.
        /// </param>
        /// <param name="ignoreShowUpdate">
        ///     A value indicating whether the settings to update and lock shows should be ignored.
        /// </param>
        /// <returns>
        ///     The results of the file process.
        /// </returns>
        private FileResult ProcessFile(IFileInfo file, bool ignoreShowUpdate)
        {
            // Attempt to match to a regular express
            var matches = GetMatches(file);
            FileResult emptyResult = null;

            var showname = string.Empty;

            // Try to resolve an result
            foreach (var match in matches)
            {
                var show = MatchShow(file.Name, match.Index, out showname, ignoreShowUpdate);
                Episode episode = null;
                IList<Episode> episodes = null;

                if (show != null)
                {
                    episodes = ProcessEpisode(match, show).ToList();
                    if (episodes.Count > 0)
                    {
                        episode = episodes.First();
                    }

                    return new FileResult
                    {
                        Episode = episode,
                        Episodes = episodes,
                        InputFile = file,
                        Show = show,
                        ShowName = showname
                    };
                }

                emptyResult = new FileResult
                {
                    Episode = episode,
                    Episodes = episodes,
                    InputFile = file,
                    Show = show,
                    ShowName = showname
                };
            }

            return emptyResult;
        }

        /// <summary>
        ///     Processes a TVShow result
        /// </summary>
        /// <param name="showName">
        ///     The name of the show.
        /// </param>
        /// <param name="result">
        ///     The matched result.
        /// </param>
        /// <returns>
        ///     The matched TV Show.
        /// </returns>
        private TvShow ProcessResult(string showName, TvShow result)
        {
            // See if we already have the show under the same TVDB.
            var show = tvShows.FirstOrDefault(x => x.Equals(result));
            if (show != null)
            {
                Logger.OnLogMessage(
                    this,
                    "{0} matched as {1}. Adding alternate name.",
                    LogType.Info,
                    showName,
                    show.Name);
                show.AlternateNames.Add(showName);
                tvShowRepository.Save(show);
                return show;
            }

            Logger.OnLogMessage(this, "Matched {0} as a new show. Adding and updating...", LogType.Info, showName);

            // Doesn't exist with the same TVDB. Add a new show and update.
            show = tvShowRepository.FromSearchResult(result);
            tvShowRepository.Save(show);
            tvShowRepository.Update(show);
            tvShows.Add(show);
            return show;
        }

        /// <summary>
        ///     Handles a TV show being changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void TvShowChanged(object sender, TvShowEventArgs e)
        {
            if (tvShows.Contains(e.TvShow))
            {
                tvShows.Remove(e.TvShow);
            }

            tvShows.Add(e.TvShow);
        }

        /// <summary>
        ///     Handles a TV show being added.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void TvShowAdded(object sender, TvShowEventArgs e)
        {
            tvShows.Add(e.TvShow);
        }

        /// <summary>
        ///     Handles a TV show being removed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The arguments of the event.</param>
        private void TvShowRemoved(object sender, TvShowEventArgs e)
        {
            if (tvShows.Contains(e.TvShow))
            {
                tvShows.Remove(e.TvShow);
            }
        }
    }
}
