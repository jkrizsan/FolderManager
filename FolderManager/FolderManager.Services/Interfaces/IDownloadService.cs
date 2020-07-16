using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FolderManager.Services.Interfaces
{
    public interface IDownloadService
    {
        Task<bool> Download(string curFileName);
        Task<bool> Upload(string fileName);
        Task<List<string>> DownloadFileNames();
    }
}
