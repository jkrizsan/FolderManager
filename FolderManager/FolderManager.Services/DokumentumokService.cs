using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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


        public string GetFolderPath()
                    => Configuration.GetSection("FolderConfig").GetSection("Path").Value;

        public async Task<MemoryStream> GetFileInMemoryStream(string path)
        {
            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return memory;
        }

        public string GetContentType(string path)
        {
            var types = GetMimeTypes();
            var ext = Path.GetExtension(path).ToLowerInvariant();
            return types[ext];
        }

        private Dictionary<string, string> GetMimeTypes()
        {
            return new Dictionary<string, string>
            {
                {".txt", "text/plain"},
                {".pdf", "application/pdf"},
                {".doc", "application/vnd.ms-word"},
                {".docx", "application/vnd.ms-word"},
                {".xls", "application/vnd.ms-excel"},
                {".xlsx", "application/vnd.openxmlformats  officedocument.spreadsheetml.sheet"},
                {".png", "image/png"},
                {".jpg", "image/jpeg"},
                {".jpeg", "image/jpeg"},
                {".gif", "image/gif"},
                {".csv", "text/csv"}
            };
        }

        

    }
}
