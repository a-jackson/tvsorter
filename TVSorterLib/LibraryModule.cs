using Ninject.Modules;
using TVSorter.Data;
using TVSorter.Data.Tvdb;
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
            Bind<IDataProvider>().To<Tvdb>().InSingletonScope();
            Bind<IScanManager>().To<ScanManager>().InSingletonScope();
            Bind<IFileResultManager>().To<FileResultManager>().InSingletonScope();
            Bind<IFileManager>().To<FileManager>().InSingletonScope();
            Bind<IFileSearch>().To<FileSearch>().InSingletonScope();
            Bind<ITvShowRepository>().To<TvShowRepository>().InSingletonScope();
        }
    }
}
