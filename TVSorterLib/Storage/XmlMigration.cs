using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;

namespace TVSorter.Storage
{
    public class XmlMigration : IXmlMigration
    {
        internal const int XmlVersion = 5;

        private readonly IEnumerable<IStorageMigration> storageMigrations;
        private readonly IXmlValidator xmlValidator;

        public XmlMigration(IEnumerable<IStorageMigration> storageMigrations, IXmlValidator xmlValidator)
        {
            this.storageMigrations = storageMigrations;
            this.xmlValidator = xmlValidator;
        }

        public void MigrateIfRequired(XDocument document, string xmlFile)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var root = document.Root;
            if (document.Root == null)
            {
                throw new XmlSchemaValidationException("The XML file is invalid.");
            }

            var version = int.Parse(document.Root.GetAttribute("version", "0"));
            if (version == 0)
            {
                // Verison 0 is the XML before it was verisoned from 1.0b.
                // Add the version and namespace declaration so it will match verison 1.
                document.Root.Add(new XAttribute("version", 1));

                foreach (var element in document.Descendants())
                {
                    element.Name = element.Name.LocalName.GetElementName();
                }

                version = 1;
                document.Save(xmlFile);
            }

            // Check that the XML is up to date.
            if (version < XmlVersion)
            {
                // Ensure that the file is valid for it's version
                xmlValidator.ValidateXml(version, xmlFile);

                var migrations = storageMigrations.Where(x => x.MigratesToVersion > version)
                    .OrderBy(x => x.MigratesToVersion);

                foreach (var migration in migrations)
                {
                    migration.Migrate(root);
                }

                var versionAttribute = root.Attribute("version");
                if (versionAttribute != null)
                {
                    versionAttribute.Value = XmlVersion.ToString(CultureInfo.InvariantCulture);
                }

                SanitizeXml(document.Root);
                document.Save(xmlFile);
            }

            xmlValidator.ValidateXml(XmlVersion, xmlFile);
        }

        /// <summary>
        ///     Sanitizes the XML file of empty namespaces
        /// </summary>
        /// <param name="root">
        ///     The root element to sanitize.
        /// </param>
        private void SanitizeXml(XElement root)
        {
            // If we have an empty namespace...
            foreach (var node in root.Descendants())
            {
                if (node.Name.NamespaceName == string.Empty)
                {
                    // Remove the xmlns='' attribute. Note the use of
                    // Attributes rather than Attribute, in case the
                    // attribute doesn't exist (which it might not if we'd
                    // created the document "manually" instead of loading
                    // it from a file.)
                    node.Attributes("xmlns").Remove();

                    // Inherit the parent namespace instead
                    if (node.Parent != null)
                    {
                        node.Name = node.Parent.Name.Namespace + node.Name.LocalName;
                    }
                }
            }
        }
    }
}
