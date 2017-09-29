namespace TVSorter.Storage
{
    public interface IXmlValidator
    {
        /// <summary>
        ///     Validates the XML file against the specified schema.
        /// </summary>
        /// <param name="schemaVersion">
        ///     The schema to validate against.
        /// </param>
        /// <param name="xmlFilePath">The path of the file to validate.</param>
        void ValidateXml(int schemaVersion, string xmlFilePath);
    }
}
