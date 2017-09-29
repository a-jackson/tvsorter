using System.IO;

namespace TVSorter.Storage
{
    public interface ITextReaderProvider
    {
        TextReader GetTextReader(string path);

        bool CanGetTextReader(string path);
    }
}
