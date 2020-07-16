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
using FolderManager.Services.Interfaces;
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

        public DokumentumokController(IDokumentumokService dokumentumokService)
        {
            this.dokumentumokService = dokumentumokService;
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
        public async Task<IActionResult> Get(string fileName)
        {
            if (fileName == null)
            {
                return Content("filename not present");
            }

            var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           dokumentumokService.GetFolderPath(), fileName);

            var memory = await dokumentumokService.GetFileInMemoryStream(path);

            var contentType =  dokumentumokService.GetContentType(path);

            return File(memory, contentType, Path.GetFileName(path));
        }

        

            [HttpGet]
        public string GetAll()
        {
            FileListDto response = new FileListDto();

            try
            {
                response.Files = dokumentumokService.GetAllFileNameFromFolder(dokumentumokService.GetFolderPath());
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
