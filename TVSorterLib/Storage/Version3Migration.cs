using System.Xml;
using System.Xml.Linq;

namespace TVSorter.Storage
{
    public class Version3Migration : IStorageMigration
    {
        public int MigratesToVersion => 3;

        public void Migrate(XElement root)
        {
            var settingsNode = root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("XML is not valid");
            }

            settingsNode.Add(new XAttribute("addunmatchedshows", false));
            settingsNode.Add(new XAttribute("unlockmatchedshows", false));
            settingsNode.Add(new XAttribute("lockshowsnonewepisodes", false));
        }
    }
}
