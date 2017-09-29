// --------------------------------------------------------------------------------------------------------------------
// <copyright file="XElementExtensions.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Provides extensions methods for the XElement class.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TVSorter.Storage
{
    /// <summary>
    ///     Provides extensions methods for the XElement class.
    /// </summary>
    internal static class XElementExtensions
    {
        /// <summary>
        ///     Gets the value of the specified attribute from the specified element.
        /// </summary>
        /// <param name="element">
        ///     The element to read the attribute from.
        /// </param>
        /// <param name="name">
        ///     The name of the attribute.
        /// </param>
        /// <param name="defaultValue">
        ///     The value to return if the attribute doesn't exist.
        /// </param>
        /// <returns>
        ///     The value of the specified attribute or default value if it does not exist.
        /// </returns>
        public static string GetAttribute(this XElement element, string name, string defaultValue = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var attribute = element.Attribute(name);
            return attribute != null ? attribute.Value : defaultValue;
        }

        /// <summary>
        ///     Gets the text from child element of the specified element. Returns defaultValue if it doesn't exist.
        /// </summary>
        /// <param name="element">
        ///     The element to read.
        /// </param>
        /// <param name="name">
        ///     The name of the child element.
        /// </param>
        /// <param name="defaultValue">
        ///     The default value to return if it doesn't exist.
        /// </param>
        /// <returns>
        ///     The child element's inner text.
        /// </returns>
        public static string GetElementText(this XElement element, XName name, string defaultValue = null)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            var childElement = element.Element(name);
            return childElement != null ? childElement.Value : defaultValue;
        }

        /// <summary>
        ///     Gets the text from child element of the specified element. Returns defaultValue if it doesn't exist.
        /// </summary>
        /// <param name="element">
        ///     The element to read.
        /// </param>
        /// <param name="name">
        ///     The name of the child element.
        /// </param>
        /// <returns>
        ///     The child element's inner text.
        /// </returns>
        public static IEnumerable<string> GetElementsText(this XElement element, XName name)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            return element.Elements(name).Select(childElement => childElement.Value);
        }
    }
}
