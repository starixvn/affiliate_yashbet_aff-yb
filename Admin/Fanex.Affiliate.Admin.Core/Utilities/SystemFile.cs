using Fanex.Affiliate.Admin.Core.Constants;
using Fanex.Affiliate.Admin.Core.DTO;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Fanex.Affiliate.Admin.Core.Utilities
{
    public class SystemFile : ISystemFile
    {
        public void DeleteFile(string serverPath, string filePath)
        {
            try
            {
                var fullPath = Path.Combine(serverPath, filePath);
                var file = new FileInfo(fullPath);
                if (file.Exists)
                {
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        public void MoveFile(string serverPath, string newFilePath, string oldFilePath)
        {
            try
            {
                var oldFullPath = Path.Combine(serverPath, oldFilePath);
                var newFullPath = Path.Combine(serverPath, newFilePath);

                if (File.Exists(oldFullPath))
                {
                    File.Move(oldFullPath, newFullPath);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        public void UploadFile(Bitmap bitmap, string filePath)
        {
            try
            {
                var extension = GetExtension(filePath);
                if (string.Equals(extension, ".gif", StringComparison.CurrentCultureIgnoreCase))
                {
                    bitmap.Save(filePath, ImageFormat.Gif);
                }
                else
                {
                    var isPng = string.Equals(extension, ".png", StringComparison.CurrentCultureIgnoreCase);
                    bitmap.Save(filePath, isPng ? ImageFormat.Png : ImageFormat.Jpeg);
                }
            }
            catch (Exception ex)
            {
                LogHelper.WriteError(ex);
            }
        }

        public ResultInfo CreateFolder(string serverPath, string folder)
        {
            if (!Directory.Exists(Path.Combine(serverPath, folder)))
            {
                try
                {
                    Directory.CreateDirectory(serverPath + folder);
                }
                catch (Exception ex)
                {
                    LogHelper.WriteError(ex);
                    return new ResultInfo
                    {
                        Code = ErrCode.ErrCanNotAccessFolder,
                        Message = "Can not access folder."
                    };
                }
            }

            return new ResultInfo();
        }

        public ResultInfo CheckExistedFile(string serverPath, string filePath)
        {
            var newFullPath = Path.Combine(serverPath, filePath);
            if (File.Exists(newFullPath))
            {
                return new ResultInfo
                {
                    Code = ErrCode.ErrTheFileNameAlreadyExists,
                    Message = "This file name belong to an existing banner. Please change the file name."
                };
            }

            return new ResultInfo();
        }

        public string GetExtension(string fileName)
        {
            return Path.GetExtension(fileName);
        }
    }
}
