using System.Xml.Linq;

namespace TVSorter.Storage
{
    public interface IXmlMigration
    {
        void MigrateIfRequired(XDocument document, string xmlFile);
    }
}
