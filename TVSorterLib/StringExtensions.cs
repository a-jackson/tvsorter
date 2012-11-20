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

            return new string(str.Where(x => !Path.GetInvalidFileNameChars().Contains(x)).ToArray());
        }

        /// <summary>
        /// Removes the spacer chars from the specfied string.
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