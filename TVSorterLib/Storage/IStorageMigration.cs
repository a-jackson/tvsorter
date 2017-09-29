using System.Xml.Linq;

namespace TVSorter.Storage
{
    public interface IStorageMigration
    {
        int MigratesToVersion { get; }

        void Migrate(XElement root);
    }
}
