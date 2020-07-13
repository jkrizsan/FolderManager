using System;
using System.Collections.Generic;
using System.Text;

namespace FolderManager.Services
{
    public interface IDokumentumokService
    {
        IEnumerable<string> GetAllFileNameFromFolder(string folderPath);
    }
}
