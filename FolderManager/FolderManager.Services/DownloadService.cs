using FolderManager.Data.DTOs;
using FolderManager.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FolderManager.Services
{
    public class DownloadService : IDownloadService
    {

        private IHttpHandler client;

        private string baseUrl = "https://localhost:44307/api/";
        private string fileServiceName = "dokumentumok";

        public DownloadService(IHttpHandler client)
        {
            this.client = client;
        }

        public async Task<bool> Download(string curFileName)
        {

            var response = await client.GetAsync($"{baseUrl}{fileServiceName}/{curFileName}");
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }
            using (var stream = response.Content.ReadAsStreamAsync())
            {
                var fileInfo = new FileInfo(curFileName);
                using (var fileStream = fileInfo.Create())
                {
                    await stream.Result.CopyToAsync(fileStream);
                }
            }
            return true;
        }

        public async Task<List<string>> DownloadFileNames()
        {
            try
            {
                var response = await client.GetAsync($"{baseUrl}{fileServiceName}");
                string apiResponse = await response.Content.ReadAsStringAsync();
                var res = JsonConvert.DeserializeObject<FileListDto>(apiResponse);
                return res.Files.ToList();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> Upload(string fileName)
        {
            using (var form = new MultipartFormDataContent())
            {
                using (var fs = File.OpenRead(fileName))
                {
                    using (var streamContent = new StreamContent(fs))
                    {
                        using (var fileContent = new ByteArrayContent(await streamContent.ReadAsByteArrayAsync()))
                        {
                            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");

                            form.Add(fileContent, "file", Path.GetFileName(fileName));
                            HttpResponseMessage response =
                                await client.PostAsync($"{baseUrl}{fileServiceName}", form);
                            if (!response.IsSuccessStatusCode)
                            {
                                return false;

                            }
                        }
                    }
                }
            }

            return true;
        }
    }
}
