using System.IO;

namespace TVSorter.Storage
{
    public class TextReaderProvider : ITextReaderProvider
    {
        public TextReader GetTextReader(string path) => File.OpenText(path);

        public bool CanGetTextReader(string path) => File.Exists(path);
    }
}
