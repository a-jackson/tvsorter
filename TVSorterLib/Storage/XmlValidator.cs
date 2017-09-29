using System.Xml;
using System.Xml.Schema;

namespace TVSorter.Storage
{
    public class XmlValidator : IXmlValidator
    {
        /// <summary>
        ///     The file path of the XSD file. Contains the version number of the XML file.
        /// </summary>
        private const string XsdFile = "TVSorter-{0}.0.xsd";

        private readonly ITextReaderProvider textReaderProvider;

        public XmlValidator(ITextReaderProvider textReaderProvider)
        {
            this.textReaderProvider = textReaderProvider;
        }

        /// <summary>
        ///     Validates the XML file against the specified schema.
        /// </summary>
        /// <param name="schemaVersion">
        ///     The schema to validate against.
        /// </param>
        /// <param name="xmlFilePath">The path of the file to validate.</param>
        public void ValidateXml(int schemaVersion, string xmlFilePath)
        {
            if (VerifyFilesExist(schemaVersion, xmlFilePath, out var schemaFile))
            {
                return;
            }

            var xmlSettings = CreateXmlReaderSettings(schemaFile);

            ValidateXml(xmlFilePath, xmlSettings);
        }

        private bool VerifyFilesExist(int schemaVersion, string xmlFilePath, out string schemaFile)
        {
            if (!textReaderProvider.CanGetTextReader(xmlFilePath))
            {
                throw new XmlSchemaValidationException("File does not exist");
            }

            schemaFile = GetFileName(schemaVersion);

            return !textReaderProvider.CanGetTextReader(schemaFile);
        }

        private static XmlReaderSettings CreateXmlReaderSettings(string schemaFile)
        {
            // Get the schemas to validate against.
            var xmlSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };

            xmlSettings.Schemas.Add(Xml.XmlNamespace.NamespaceName, schemaFile);
            xmlSettings.ValidationEventHandler += (sender, args)
                => throw new XmlSchemaValidationException(args.Message, args.Exception);
            return xmlSettings;
        }

        private void ValidateXml(string xmlFilePath, XmlReaderSettings xmlSettings)
        {
            using (var reader = XmlReader.Create(textReaderProvider.GetTextReader(xmlFilePath), xmlSettings))
            {
                while (reader.Read())
                {
                }
            }
        }

        private string GetFileName(int schemaVersion)
        {
            return string.Format(XsdFile, schemaVersion);
        }
    }
}
