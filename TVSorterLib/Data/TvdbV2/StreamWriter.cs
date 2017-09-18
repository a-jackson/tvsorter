using System.IO;

namespace TVSorter.Data.TvdbV2
{
    public class StreamWriter : IStreamWriter
    {
        public void WriteStream(Stream stream, string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));
            using (var fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
    }
}
