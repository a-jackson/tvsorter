using System.IO;

namespace TVSorter.Data.TvdbV2
{
    public interface IStreamWriter
    {
        void WriteStream(Stream stream, string path);
    }
}
