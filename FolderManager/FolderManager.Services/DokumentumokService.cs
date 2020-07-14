using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

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
                return Directory.GetFiles(folderPath).Select(x => Path.GetFileName(x));
            }

            throw new DirectoryNotFoundException($"{folderPath} does not exists!");
        }
    }
}
