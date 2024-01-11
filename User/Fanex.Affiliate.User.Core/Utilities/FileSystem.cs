using System.IO;

namespace Fanex.Affiliate.User.Core.Utilities
{
    public class FileSystem : IFileSystem
    {
        public bool Exists(string path)
        {
            return File.Exists(path);
        }
    }
}
