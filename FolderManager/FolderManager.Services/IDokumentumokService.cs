using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace FolderManager.Services
{
    public interface IDokumentumokService
    {
        IEnumerable<string> GetAllFileNameFromFolder(string folderPath);

        Task<string> SaveFileFromPost(IFormFile file);
    }
}
