using Ninject.Modules;
using TVSorter.Controller;
using TVSorter.View;

namespace TVSorter
{
    public class InterfaceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AddShowController>().ToSelf();
            Bind<LogController>().ToSelf();
            Bind<MissingDuplicateController>().ToSelf();
            Bind<SearchResultsController>().ToSelf();
            Bind<SettingsController>().ToSelf();
            Bind<ShowSearchController>().ToSelf();
            Bind<SortEpisodesController>().ToSelf();
            Bind<TvShowsController>().ToSelf();

            Bind<MainForm>().ToSelf();
            Bind<SortEpisodes>().ToSelf();
            Bind<TvShows>().ToSelf();
            Bind<Settings>().ToSelf();
            Bind<Log>().ToSelf();
            Bind<MissingDuplicateEpisodes>().ToSelf();

            Bind<FormatBuilder>().ToSelf();
            Bind<ListDialog>().ToSelf();
            Bind<NumberInputDialog>().ToSelf();
            Bind<ProgressDialog>().ToSelf();
            Bind<ShowSearchDialog>().ToSelf();
            Bind<ShowSelectDialog>().ToSelf();
        }
    }
}
