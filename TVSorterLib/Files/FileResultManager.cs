// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileResultManager.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The manager for handling File Results.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TVSorter.Model;
using TVSorter.Storage;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    /// <summary>
    ///     The manager for handling File Results.
    /// </summary>
    public class FileResultManager : IFileResultManager
    {
        /// <summary>
        ///     The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        ///     Initialises a new instance of the <see cref="FileResultManager" /> class.
        /// </summary>
        /// <param name="storageProvider">
        ///     The storage provider.
        /// </param>
        public FileResultManager(IStorageProvider storageProvider)
        {
            this.storageProvider = storageProvider;
        }

        /// <summary>
        ///     Gets the full path of the file for the specified destination.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to get the path for.
        /// </param>
        /// <param name="destination">
        ///     The destination directory.
        /// </param>
        /// <returns>
        ///     The full path of the file.
        /// </returns>
        public IFileInfo GetFullPath(FileResult fileResult, IDirectoryInfo destination)
        {
            if (fileResult.Incomplete)
            {
                throw new InvalidOperationException("There is not enough data to get the file path.");
            }

            return destination.GetFile(FormatOutputPath(fileResult));
        }

        /// <summary>
        ///     Formats the output path of the episode.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to format the path for.
        /// </param>
        /// <returns>
        ///     The formatted output path.
        /// </returns>
        public string FormatOutputPath(FileResult fileResult)
        {
            var formatString = GetEffectiveFormat(fileResult);
            return FormatOutputPath(fileResult, formatString);
        }

        /// <summary>
        ///     Formats the output path of the episode.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to format the string for.
        /// </param>
        /// <param name="formatString">
        ///     The format string to use.
        /// </param>
        /// <returns>
        ///     The formatted output path.
        /// </returns>
        public string FormatOutputPath(FileResult fileResult, string formatString)
        {
            if (fileResult.Show == null || fileResult.Episode == null || fileResult.InputFile == null)
            {
                return string.Empty;
            }

            // Replace the extension and folder name. (Format codes with no parameters)
            formatString = formatString.Replace("{Ext}", fileResult.InputFile.Extension);
            formatString = formatString.Replace("{FName}", fileResult.Show.FolderName);

            // Identify the other format codes and their parameters.
            var regExp = new Regex(@"{([a-zA-Z]+)\(([^\)}]+)\)}");

            // Replace the matches with the appropriate strings.
            formatString = regExp.Replace(formatString, match => ProcessMatch(match, fileResult));

            // Replace : with .
            formatString = formatString.Replace(':', '.');

            // Get the invalid characters for a file name, not including the DirectorySeparatorChar.
            var invalidChars = StringExtensions.InvalidFilenameChars.Where(x => !x.Equals(Path.DirectorySeparatorChar));

            // Remove any characters that can't be in a filename from the formatString.
            return invalidChars.Aggregate(
                formatString,
                (current, ch) => current.Replace(ch.ToString(CultureInfo.InvariantCulture), string.Empty));
        }

        /// <summary>
        ///     Gets the format string that corresponds the specified file result's show.
        ///     The show could have a custom format, if not, uses the default.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to get the format of.
        /// </param>
        /// <returns>
        ///     The format string.
        /// </returns>
        private string GetEffectiveFormat(FileResult fileResult)
        {
            if (fileResult.Show == null)
            {
                return string.Empty;
            }

            if (fileResult.Show.UseCustomFormat)
            {
                return fileResult.Show.CustomFormat;
            }

            return storageProvider.Settings.DefaultOutputFormat;
        }

        /// <summary>
        ///     Processes a single match of a format code.
        /// </summary>
        /// <param name="match">
        ///     The matched format code.
        /// </param>
        /// <param name="fileResult">
        ///     The file result to process.
        /// </param>
        /// <returns>
        ///     The string to replace the format code with.
        /// </returns>
        private string ProcessMatch(Match match, FileResult fileResult)
        {
            var type = match.Groups[1].Value;
            var arg = match.Groups[2].Value;
            switch (type)
            {
                case "SName":
                    return FormatName(fileResult.Show.Name, arg);
                case "EName":
                    var name = GetEpisodeName(fileResult);
                    return FormatName(name, arg);
                case "ENum":
                    try
                    {
                        return FormatNum(arg, fileResult.Episodes.Select(x => x.EpisodeNumber).ToArray());
                    }
                    catch
                    {
                        return match.Value;
                    }

                case "SNum":
                    try
                    {
                        return FormatNum(arg, fileResult.Episodes.Select(x => x.SeasonNumber).ToArray());
                    }
                    catch
                    {
                        return match.Value;
                    }

                case "Date":
                    try
                    {
                        return fileResult.Episode.FirstAir.ToString(arg);
                    }
                    catch
                    {
                        return match.Value;
                    }
            }

            return match.Value;
        }

        /// <summary>
        ///     Formatted the name of an episode or show
        /// </summary>
        /// <param name="name">
        ///     The name of the episode/show
        /// </param>
        /// <param name="arg">
        ///     The variable's argument, should be the separator char
        /// </param>
        /// <returns>
        ///     The formatted string
        /// </returns>
        private string FormatName(string name, string arg)
        {
            // Break into separate words
            var parts = name.Split(' ');

            // Characters that can be used as separators, if there is one of these at the start
            // or end of a part then it won't put the arg character next to it. This is to avoid
            // outputs like Day.1.12.00.-.1.00 for a 24 episode since .-. looks silly.
            // Or names with titles in like Mr. Smith would be Mr..Smith - equally silly.
            var separatorChars = new[] { '-', ':', '_', '.', ',' };

            // The new name to be returned
            var newName = parts[0];

            // Loop through each part appending it with the appropriate separator
            for (var i = 1; i < parts.Length; i++)
            {
                // Check for the separator chars at the start of this part of the end of the next.
                var prefix = separatorChars.Select(x => x.ToString(CultureInfo.InvariantCulture))
                    .All(sep => !parts[i].StartsWith(sep) && !parts[i - 1].EndsWith(sep));

                // If there is already a prefix then don't add another
                // Add the arg character
                if (prefix)
                {
                    newName += arg;
                }

                // Add the part
                newName += parts[i];
            }

            // If the string ends with a separator character then remove it
            newName = newName.TrimEnd(separatorChars);

            return newName;
        }

        /// <summary>
        ///     Format the numbers for output
        /// </summary>
        /// <param name="arg">
        ///     The variable's argument, the length of the output number.
        /// </param>
        /// <param name="num">
        ///     The numbers to format
        /// </param>
        /// <returns>
        ///     The formatted string
        /// </returns>
        private string FormatNum(string arg, params int[] num)
        {
            if (num.Length == 0)
            {
                throw new ArgumentException("You must specify at least one number to format.");
            }

            var length = int.Parse(arg);

            // Build the format string for the specfied number of digits.
            var zeros = "{0:";
            if (length == 0)
            {
                zeros += "0";
            }
            else
            {
                for (var i = 0; i < length; i++)
                {
                    zeros += "0";
                }
            }

            zeros += "}";

            return num.Distinct().Select(x => string.Format(zeros, x)).Aggregate((x, y) => x + "-" + y);
        }

        /// <summary>
        ///     Gets the name of the episode. Used if there is more than one episode.
        /// </summary>
        /// <param name="fileResult">
        ///     The file result to get the episode name for.
        /// </param>
        /// <returns>The name of the episode.</returns>
        private string GetEpisodeName(FileResult fileResult)
        {
            // If there is only one episode on the match then return it.
            if (fileResult.Episodes.Count == 1)
            {
                return fileResult.Episode.Name;
            }

            // If the episode names are the same except for a part number at the end then
            // use the same name and just list the part numbers at the end.
            var regex = new Regex(@"\(([0-9]+)\)");
            var episodeNames = (from episode in fileResult.Episodes
                let match = regex.Match(episode.Name)
                where match.Success
                select new { Name = episode.Name.Substring(0, match.Index).Trim(), Part = match.Groups[1] }).ToList();

            // If all the episode had a number in brackets at the end of them.
            if (episodeNames.Count == fileResult.Episodes.Count)
            {
                // Ensure that the all the episode names before the number were the same.
                var firstName = episodeNames.First().Name;
                if (episodeNames.All(x => x.Name == firstName))
                {
                    return firstName +
                           " (" +
                           episodeNames.Select(x => x.Part.ToString()).Aggregate((x, y) => x + "-" + y) +
                           ")";
                }
            }

            // The names didn't have numbers or didn't match.
            // Just concatenate all the names together separated with -.
            return fileResult.Episodes.Select(x => x.Name).Aggregate((x, y) => x + "-" + y);
        }
    }
}
