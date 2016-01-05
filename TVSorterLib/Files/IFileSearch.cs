using System.Collections.Generic;
using TVSorter.Model;

namespace TVSorter.Files
{
    public interface IFileSearch
    {
        List<FileResult> Results { get; }

        void Copy();
        void Move();
        void RefreshFileCounts();
        void Search(string subDirectory);
        void SetEpisode(int seasonNumber, int episodeNumber);
        void SetShow(TvShow show);
    }
}