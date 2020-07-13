using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace FolderManager.Services
{

    public class DokumentumokService : IDokumentumokService
    {
        public DokumentumokService()
        {

        }

        public IEnumerable<string> GetAllFileNameFromFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                return Directory.GetFiles(folderPath);
            }

            throw new DirectoryNotFoundException($"{folderPath} does not exists!");
        }
    }
}
