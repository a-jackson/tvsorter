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
                throw new ArgumentNullException(nameof(element));
            }

            var attribute = element.Attribute(name);
            return attribute?.Value ?? defaultValue;
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
                throw new ArgumentNullException(nameof(element));
            }

            return element.Elements(name).Select(childElement => childElement.Value);
        }

        public static XName GetElementName(this string elementName)
        {
            return Xml.XmlNamespace + elementName;
        }
    }
}
