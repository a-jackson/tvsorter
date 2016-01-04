using System.Collections.Generic;
using TVSorter.Model;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    public interface IScanManager
    {
        List<FileResult> Refresh(string subDirectory);
        void RefreshFileCounts();
        void ResetShow(FileResult result, TvShow show);
        IEnumerable<FileResult> SearchDestinationFolder(IDirectoryInfo destination);
        Dictionary<string, List<TvShow>> SearchNewShows();
    }
}