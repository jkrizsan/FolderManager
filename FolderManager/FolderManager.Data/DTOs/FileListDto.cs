using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderManager.Data.DTOs
{
    public class FileListDto
    {
        public IEnumerable<string> Files { get; set; }
        public string ErrorMessage { get; set; }
    }
}
