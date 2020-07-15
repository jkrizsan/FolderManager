using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FolderManager.Services
{

    public class DokumentumokService : IDokumentumokService
    {
        public IConfiguration Configuration { get; }

        public DokumentumokService(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }

        public IEnumerable<string> GetAllFileNameFromFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath).Select(x => Path.GetFileName(x));
            }

            throw new DirectoryNotFoundException($"{folderPath} does not exists!");
        }

        public async Task<string> SaveFileFromPost(IFormFile file)
        {
            try
            {
                var filePath = Path.Combine(GetFolderPath(), file.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return "";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }


        private string GetFolderPath()
            => Configuration.GetSection("FolderConfig").GetSection("Path").Value;

    }
}
