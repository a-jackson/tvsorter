using System;
using System.Collections.Generic;
using TVSorter.Model;

namespace TVSorter.Repostitory
{
    public interface ITvShowRepository
    {
        event EventHandler<TvShowEventArgs> TvShowAdded;
        event EventHandler<TvShowEventArgs> TvShowChanged;
        event EventHandler<TvShowEventArgs> TvShowRemoved;

        void Delete(TvShow show);
        TvShow FromSearchResult(TvShow searchResult);
        IEnumerable<TvShow> GetTvShows();
        void Save(TvShow show);
        List<TvShow> SearchShow(string name);
        void Update(TvShow show);
        void UpdateShows(IList<TvShow> shows);
    }
}