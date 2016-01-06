// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Extension methods for the string type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Extension methods for the string type.
    /// </summary>
    public static class StringExtensions
    {
        #region Static Fields

        /// <summary>
        /// A collecting of characters not to be used in file names.
        /// </summary>
        internal static readonly char[] InvalidFilenameChars =
            {
                '\u0000', '\u0022', '\u003C', '\u003E', '\u007C', '\u0000',
                '\u0001', '\u0002', '\u0003', '\u0004', '\u0005', '\u0006',
                '\u0007', '\u0008', '\u0009', '\u000A', '\u000B', '\u000C',
                '\u000D', '\u000E', '\u000F', '\u0010', '\u0011', '\u0012',
                '\u0013', '\u0014', '\u0015', '\u0016', '\u0017', '\u0018',
                '\u0019', '\u001A', '\u001B', '\u001C', '\u001D', '\u001E',
                '\u001F', '\u003A', '\u002A', '\u003F', '\u005C', '\u002F'
            };

        /// <summary>
        ///   An array of characters that can be used as spaces.
        /// </summary>
        private static readonly char[] SpacerChars = new[] { '.', '_', '-' };

        /// <summary>
        /// An array of special characters to be removed.
        /// </summary>
        private static readonly char[] SpecialChars = new[] { '?', ':', ';', ',', '#', '~', '(', ')', '&', '%', '$' };

        #endregion

        #region Methods

        /// <summary>
        /// Returns a string that is exactly the length specified, either truncating or padding the specified string.
        /// </summary>
        /// <param name="str">The string to format.</param>
        /// <param name="length">The target length.</param>
        /// <returns>The formatting string.</returns>
        public static string FormatLength(this string str, int length)
        {
            string format = "{0,-" + length + "}";
            string arg = str.Length > length ? str.Substring(0, length) : str;
            return string.Format(format, arg);
        }

        /// <summary>
        /// Returns a string that is truncated to the specified length if it is longer.
        /// </summary>
        /// <param name="str">The string to truncate.</param>
        /// <param name="length">The length to truncate to.</param>
        /// <returns>The truncated string.</returns>
        public static string Truncate(this string str, int length = 30)
        {
            return str.Length > length ? str.Substring(0, length - 4) + "..." : str;
        }

        /// <summary>
        /// Strips any characters that can't be in a file name from the specified string.
        /// </summary>
        /// <param name="str">
        /// The string to edit.
        /// </param>
        /// <returns>
        /// The file name safe string.
        /// </returns>
        internal static string GetFileSafeName(this string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str", "The string cannot be null.");
            }

            return new string(str.Where(x => !InvalidFilenameChars.Contains(x)).ToArray());
        }

        /// <summary>
        /// Removes the spacer chars from the specified string.
        /// </summary>
        /// <param name="str">
        /// The string to process.
        /// </param>
        /// <returns>
        /// The processed string.
        /// </returns>
        internal static string RemoveSpacerChars(this string str)
        {
            return SpacerChars.Aggregate(str, (current, ch) => current.Replace(ch, ' '));
        }

        /// <summary>
        /// Removes special characters from the string.
        /// </summary>
        /// <param name="str">
        /// The string to process.
        /// </param>
        /// <returns>
        /// The processed string.
        /// </returns>
        internal static string RemoveSpecialChars(this string str)
        {
            return SpecialChars.Aggregate(str.RemoveSpacerChars(), (current, ch) => current.Replace(ch, '\0'));
        }

        /// <summary>
        /// Removes special characters and spaces from the string.
        /// </summary>
        /// <param name="str">The string to process.</param>
        /// <returns>The processed string.</returns>
        internal static string AlphaNumericOnly(this string str)
        {
            return new string(str.Where(char.IsLetterOrDigit).ToArray());
        }

        #endregion
    }
}