using System.Xml;
using System.Xml.Linq;
using TVSorter.Model;

namespace TVSorter.Storage
{
    public class Version2Migration : IStorageMigration
    {
        public int MigratesToVersion => 2;

        public void Migrate(XElement root)
        {
            var settingsNode = root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid.");
            }

            // Update the first regualar expression to use the one with dual episode support
            var regularExpression = settingsNode.Element("RegularExpression".GetElementName());
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            var regex = regularExpression.Element("RegEx".GetElementName());

            // If it is the old text then replace with the new text
            if ((regex != null) && regex.Value.Equals("s(?<S>[0-9]+)e(?<E>[0-9]+)"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+";
            }

            // Add the new missing episode settings node.
            settingsNode.AddAfterSelf(new MissingEpisodeSettings().ToXml());
        }
    }
}
