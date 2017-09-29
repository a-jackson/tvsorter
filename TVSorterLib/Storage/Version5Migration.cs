using System.Xml;
using System.Xml.Linq;

namespace TVSorter.Storage
{
    public class Version5Migration : IStorageMigration
    {
        public int MigratesToVersion => 5;

        public void Migrate(XElement root)
        {
            var settingsNode = root.Element("Settings".GetElementName());
            if (settingsNode == null)
            {
                throw new XmlException("Xml is not valid");
            }

            settingsNode.FirstNode.AddAfterSelf(new XElement("IgnoredDirectories", new object[] { }));
        }
    }
}
