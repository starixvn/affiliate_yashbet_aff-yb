using Fanex.Affiliate.Admin.Core.DTO;
using System.Drawing;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public interface ISystemFile
    {
        void UploadFile(Bitmap bitmap, string filePath);

        void MoveFile(string serverPath, string newFilePath, string oldFilePath);

        void DeleteFile(string serverPath, string filePath);

        ResultInfo CreateFolder(string serverPath, string folder);

        ResultInfo CheckExistedFile(string serverPath, string filePath);

        string GetExtension(string fileName);
    }
}
