using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FolderManager.Api.DTOs;
using FolderManager.Services;
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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IDokumentumokService dokumentumokService;
        public IConfiguration Configuration { get; }


        private string GetFolderPath()
            => Configuration.GetSection("FolderConfig").GetSection("Path").Value;

        public DokumentumokController(ILogger<DokumentumokController> logger, IDokumentumokService dokumentumokService,
            IConfiguration Configuration)
        {
            this.dokumentumokService = dokumentumokService;
            this.Configuration = Configuration;
        }

        [HttpGet("{fileName}")]
        public string Get(string fileName)
        {
            var filePath = Path.Combine(GetFolderPath(), fileName); //@$"{GetFolderPath()}\\{fileName}";
            byte[] b = System.IO.File.ReadAllBytes(filePath);
            return "data:image/png;base64," + Convert.ToBase64String(b);
        }

        [HttpGet]
        public string GetAll()
        {
            AllFileResponseDto response = new AllFileResponseDto();
            //IEnumerable<string> response = null;
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
