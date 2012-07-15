// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateXml.cs" company="TVSorter">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Validates XML files.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter.Storage
{
    using System.Xml;
    using System.Xml.Schema;

    /// <summary>
    /// Validates XML files.
    /// </summary>
    internal static class ValidateXml
    {
        #region Methods

        /// <summary>
        /// Validates the specified XML file.
        /// </summary>
        /// <param name="xmlFile">
        /// The XML file to validate.
        /// </param>
        internal static void Validate(string xmlFile)
        {
            var settings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
            settings.ValidationEventHandler +=
                (sender, args) => { throw new XmlSchemaException(args.Message, args.Exception); };

            using (XmlReader reader = XmlReader.Create(xmlFile, settings))
            {
                while (reader.Read())
                {
                }
            }
        }

        #endregion
    }
}