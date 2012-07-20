// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="FileResult.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The file result.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    using TVSorter.Wrappers;

    #endregion

    /// <summary>
    /// The file result.
    /// </summary>
    public class FileResult
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FileResult"/> class.
        /// </summary>
        internal FileResult()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the result is checked.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        ///   Gets the file's episode.
        /// </summary>
        public Episode Episode { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the result is incomplete.
        /// </summary>
        public bool Incomplete
        {
            get
            {
                return this.Show == null | this.Episode == null;
            }
        }

        /// <summary>
        ///   Gets the file.
        /// </summary>
        public IFileInfo InputFile { get; internal set; }

        /// <summary>
        ///   Gets the OutputPath.
        /// </summary>
        public string OutputPath
        {
            get
            {
                return this.FormatOutputPath();
            }
        }

        /// <summary>
        ///   Gets the file's show.
        /// </summary>
        public TvShow Show { get; internal set; }

        /// <summary>
        /// Gets the name of the show as seen by the program.
        /// </summary>
        public string ShowName { get; internal set; }

        #endregion

        #region Methods

        /// <summary>
        /// Determines the file's name contains any of the specified keywords.
        /// </summary>
        /// <param name="keywords">
        /// The keywords to check for.
        /// </param>
        /// <returns>
        /// A value indicating whether the file name contains the keywords.
        /// </returns>
        internal bool ContainsKeyword(IEnumerable<string> keywords)
        {
            return keywords.Any(keyword => this.InputFile.Name.ToLower().Contains(keyword.ToLower()));
        }

        /// <summary>
        /// Gets the full path of the file for the specified destination.
        /// </summary>
        /// <param name="destination">
        /// The destination directory.
        /// </param>
        /// <returns>
        /// The full path of the file.
        /// </returns>
        internal IFileInfo GetFullPath(IDirectoryInfo destination)
        {
            if (this.Incomplete)
            {
                throw new InvalidOperationException("There is not enough data to get the file path.");
            }

            return destination.GetFile(this.OutputPath);
        }

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
            foreach (string sep in separatorChars)
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
        /// <returns>
        /// The formatted output path. 
        /// </returns>
        private string FormatOutputPath()
        {
            if (this.Show == null || this.Episode == null || this.InputFile == null)
            {
                return string.Empty;
            }

            // TODO
            Settings settings = Settings.LoadSettings();

            string formatString = this.Show.UseCustomFormat ? this.Show.CustomFormat : settings.DefaultOutputFormat;

            formatString = formatString.Replace("{Ext}", this.InputFile.Extension);
            formatString = formatString.Replace("{FName}", this.Show.FolderName);

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
                                return this.FormatName(this.Show.Name, arg);
                            case "EName":
                                return this.FormatName(this.Episode.Name, arg);
                            case "ENum":
                                try
                                {
                                    return this.FormatNum(arg, this.Episode.EpisodeNumber);
                                }
                                catch
                                {
                                    return match.Value;
                                }

                            case "SNum":
                                try
                                {
                                    return this.FormatNum(arg, this.Episode.SeasonNumber);
                                }
                                catch
                                {
                                    return match.Value;
                                }

                            case "Date":
                                try
                                {
                                    return this.Episode.FirstAir.ToString(arg);
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

        #endregion
    }
}