using GymManagementSystem.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementSystem.BLL.Services.Classes
{
    public class AttachementServices : IAttachementServices
    {
        public AttachementServices(ILogger<AttachementServices> logger,IWebHostEnvironment env) 
        {
            this.logger = logger;
            this.env = env;
        }
        private readonly long MaxFileSize = 10 * 1024 * 1024; // 10 MB
        private readonly string[] AllowedFileTypes = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachementServices> logger;
        private readonly IWebHostEnvironment env;

        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName)) return false;
            try
            {
                var filePath = Path.Combine(env.ContentRootPath, folderName,fileName);
                if (File.Exists(filePath)) return false;
                
                File.Delete(filePath);
                return true;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while deleting the file {FileName} from folder {FolderName}.", fileName, folderName);
                return false;
            }
           

        }

        public (Stream stream, string contentType)? GetFile(string fileName, string folderName)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadAsync(Stream fileStream, string fileName, string folderName, CancellationToken ct = default)
        {
            if (fileStream is null || !fileStream.CanRead) return null;
            if(fileStream.Length==0) return null;
            if(fileStream.Length > MaxFileSize)
            {
                logger.LogWarning("File size exceeds the maximum limit of {MaxFileSize} bytes.", MaxFileSize);
                return null;
            }
            var extension = Path.GetExtension(fileName);
            if(string.IsNullOrEmpty(extension) || !AllowedFileTypes.Contains(extension))
            {
                logger.LogWarning("Reject wrong extention file ");
                return null;
            }

            var UploadFolderPath = Path.Combine(env.ContentRootPath, folderName);
            Directory.CreateDirectory(UploadFolderPath);
            var storageFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(UploadFolderPath, storageFileName);
            try
            {
                await using var fs = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None) ;
                
                    await fileStream.CopyToAsync(fs);
                return storageFileName;


            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while uploading the file.");
                return null;
            }

        }
    }
}
