using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FolderManager.Data.DTOs;
using FolderManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FolderManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DokumentumokController : ControllerBase
    {
        private readonly IDokumentumokService dokumentumokService;
        public IConfiguration Configuration { get; }


        private string GetFolderPath()
            => Configuration.GetSection("FolderConfig").GetSection("Path").Value;

        public DokumentumokController(IDokumentumokService dokumentumokService, IConfiguration Configuration)
        {
            this.dokumentumokService = dokumentumokService;
            this.Configuration = Configuration;
        }

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var resPonse = await dokumentumokService.SaveFileFromPost(file);

            if (resPonse.Equals(string.Empty))
            {
                return Content("Ok");
            }

            return Content(resPonse);
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get(string filename)
        {
            if (filename == null)
                return Content("filename not present");

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           GetFolderPath(), filename);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;
            return File(memory, GetContentType(path), Path.GetFileName(path));
        }

        private string GetContentType(string path)
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

            [HttpGet]
        public string GetAll()
        {
            FileListDto response = new FileListDto();

            try
            {
                response.Files = dokumentumokService.GetAllFileNameFromFolder(GetFolderPath());
            }
            catch (DirectoryNotFoundException aEx)
            {
                response.ErrorMessage = $"{nameof(DirectoryNotFoundException)}, message: {aEx.Message}";
            }
            catch (Exception ex)
            {

                response.ErrorMessage = $"Unknow exception, message: {ex.Message}";
            }

            return JsonConvert.SerializeObject(response);
        }
    }
}
