using System;

namespace TVSorter
{
    /// <summary>
    /// Provide extensions for DateTime Purposes.
    /// </summary>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Validates incoming string as a valid datetime.
        /// </summary>
        /// <param name="dateInString">date string to check.</param>
        /// <returns>Parse result.</returns>
        public static bool ValidateTime(this string dateInString)
        {
            if (DateTime.TryParse(dateInString, out DateTime temp))
            {
                return true;
            }

            return false;
        }
    }
}
