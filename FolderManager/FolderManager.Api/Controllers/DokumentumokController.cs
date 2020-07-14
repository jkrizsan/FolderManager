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

        //[HttpGet("{fileName}")]
        //public string Get(string fileName)
        //{
        //    var filePath = Path.Combine(GetFolderPath(), fileName); //@$"{GetFolderPath()}\\{fileName}";
        //    byte[] b = System.IO.File.ReadAllBytes(filePath);
        //    return "data:image/png;base64," + Convert.ToBase64String(b);
        //}


        //[HttpGet("{fileName}")]
        //public HttpResponseMessage Get(string fileName)
        //{

        //    var filePath = Path.Combine(GetFolderPath(), fileName);
        //    var dataBytes = System.IO.File.ReadAllBytes(filePath);
        //    //adding bytes to memory stream   
        //    var dataStream = new MemoryStream(dataBytes);

        //    HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK);
        //    httpResponseMessage.Content = new StreamContent(dataStream);
        //    httpResponseMessage.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
        //    httpResponseMessage.Content.Headers.ContentDisposition.FileName = filePath;
        //    httpResponseMessage.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("Base64");

        //    return httpResponseMessage;
        //}

        [HttpPost]
        public async Task<IActionResult> Post(IFormFile file)
        {
            var stream = file.OpenReadStream();
            var name = file.FileName;
            var filePath = Path.Combine(GetFolderPath(), file.FileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return null; //null just to make error free
        }

        //[HttpPost]
        //public FileContentResult Post([FromBody] UploadFileDto model)
        //{
        //    //Depending on if you want the byte array or a memory stream, you can use the below. 
        //    var imageDataByteArray = Convert.FromBase64String(model.FileData);

        //    //When creating a stream, you need to reset the position, without it you will see that you always write files with a 0 byte length. 
        //    var imageDataStream = new MemoryStream(imageDataByteArray);
        //    imageDataStream.Position = 0;

        //    //Go and do something with the actual data.
        //    //_customerImageService.Upload([...])

        //    //For the purpose of the demo, we return a file so we can ensure it was uploaded correctly. 
        //    //But otherwise you can just return a 204 etc. 
        //    return File(imageDataByteArray, "txt");
        //}



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
