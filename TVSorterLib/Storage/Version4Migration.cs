using System.Xml;
using System.Xml.Linq;

namespace TVSorter.Storage
{
    public class Version4Migration : IStorageMigration
    {
        public int MigratesToVersion => 4;

        public void Migrate(XElement root)
        {
            var settingsNode = root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            var selectedDestination = string.Empty;
            var destinations = settingsNode.Descendants("Destination".GetElementName());
            foreach (var destination in destinations)
            {
                if (bool.Parse(destination.GetAttribute("selected", "false")))
                {
                    selectedDestination = destination.Value;
                }

                destination.RemoveAttributes();
            }

            settingsNode.Add(new XAttribute("defaultdestinationdir", selectedDestination));

            // Update the first regualar expression
            var regularExpression = settingsNode.Element("RegularExpression".GetElementName());
            if (regularExpression == null)
            {
                throw new XmlException("XML is not valid.");
            }

            var regex = regularExpression.Element("RegEx".GetElementName());

            // If it is the old text then replace with the new text
            if ((regex != null) && regex.Value.Equals("s(?<S>[0-9]+)e((?<E>[0-9]+)-{0,1})+"))
            {
                regex.Value = "s(?<S>[0-9]+)e((?<E>[0-9]+)[e-]{0,1})+";
            }

            foreach (var show in root.Descendants("Show".GetElementName()))
            {
                show.Add(new XAttribute("usecustomdestination", false));
                show.Add(new XAttribute("customdestinationdir", string.Empty));
            }
        }
    }
}
