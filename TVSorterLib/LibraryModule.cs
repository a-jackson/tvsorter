using Ninject.Modules;
using TVSorter.Data;
using TVSorter.Data.TvdbV2;
using TVSorter.Files;
using TVSorter.Repostitory;
using TVSorter.Storage;

namespace TVSorter
{
    public class LibraryModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IStorageProvider>().To<Xml>().InSingletonScope();
            Bind<IStorageMigration>().To<Version2Migration>().InSingletonScope();
            Bind<IStorageMigration>().To<Version3Migration>().InSingletonScope();
            Bind<IStorageMigration>().To<Version4Migration>().InSingletonScope();
            Bind<IStorageMigration>().To<Version5Migration>().InSingletonScope();
            Bind<IXmlValidator>().To<XmlValidator>().InSingletonScope();
            Bind<IXmlMigration>().To<XmlMigration>().InSingletonScope();
            Bind<ITextReaderProvider>().To<TextReaderProvider>();

            Bind<IDataProvider>().To<TvdbV2>().InSingletonScope();
            Bind<IScanManager>().To<ScanManager>().InSingletonScope();
            Bind<IFileResultManager>().To<FileResultManager>().InSingletonScope();
            Bind<IFileManager>().To<FileManager>().InSingletonScope();
            Bind<IFileSearch>().To<FileSearch>().InSingletonScope();
            Bind<ITvShowRepository>().To<TvShowRepository>().InSingletonScope();
            Bind<IStreamWriter>().To<StreamWriter>().InSingletonScope();
        }
    }
}
