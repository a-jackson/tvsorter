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
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

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
        ///   The current settings of the system.
        /// </summary>
        private readonly Settings settings;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///   Initializes a new instance of the <see cref="ScanManager" /> class. Initialises a new instance of the <see
        ///    cref="ScanManager" /> class.
        /// </summary>
        public ScanManager()
        {
            this.settings = new Settings();
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
            // TODO Fix this method.
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

            // Group the result by show for saving them.
            IEnumerable<IGrouping<TvShow, FileResult>> showGroups = results.GroupBy(x => x.Show);
            foreach (var group in showGroups)
            {
                if (group.Key != null)
                {
                    // Save the episodes for each show.
                    // this.storage.SaveEpisodes(group.Key, group.Select(x => x.Episode), false);
                }
            }
        }

        #endregion

        #region Methods

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

            try
            {
                return new TvShow(showName, true);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception)
            {
                return null;
            }
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
            Match firstMatch = (from regex in this.settings.RegularExpressions
                                let match = Regex.Match(file.Name, regex, RegexOptions.IgnoreCase)
                                where match.Success
                                select match).FirstOrDefault();

            if (firstMatch == null)
            {
                return null;
            }

            TvShow show = this.MatchShow(file.Name, firstMatch.Index);

            Episode episode = null;

            if (show != null)
            {
                episode = this.ProcessEpisode(firstMatch, show);
            }

            return new FileResult
                {
                   Episode = episode, InputFile = file, Show = show, Incomplete = show == null || episode == null 
                };
        }

        #endregion
    }
}