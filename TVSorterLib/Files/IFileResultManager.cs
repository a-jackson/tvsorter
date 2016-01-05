using TVSorter.Model;
using TVSorter.Wrappers;

namespace TVSorter.Files
{
    public interface IFileResultManager
    {
        string FormatOutputPath(FileResult fileResult);
        string FormatOutputPath(FileResult fileResult, string format);
        IFileInfo GetFullPath(FileResult fileResult, IDirectoryInfo destination);
    }
}