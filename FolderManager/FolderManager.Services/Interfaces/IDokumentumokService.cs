using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FolderManager.Services.Interfaces
{
    public interface IDokumentumokService
    {
        IEnumerable<string> GetAllFileNameFromFolder(string folderPath);

        Task<string> SaveFileFromPost(IFormFile file);

        Task<MemoryStream> GetFileInMemoryStream(string path);

        string GetContentType(string path);

        string GetFolderPath();

    }
}
