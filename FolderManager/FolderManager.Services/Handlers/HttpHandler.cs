using FolderManager.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace FolderManager.Services.Handlers
{
    public class HttpHandler : IHttpHandler
    {
        private HttpClient client;

        public HttpHandler()
        {
            client = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await client.PostAsync(url, content);
        }
    }
}
