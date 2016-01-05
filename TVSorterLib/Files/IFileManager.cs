using System.Collections.Generic;
using TVSorter.Model;

namespace TVSorter.Files
{
    public interface IFileManager
    {
        void CopyFile(IEnumerable<FileResult> files);
        void MoveFile(IEnumerable<FileResult> files);
    }
}