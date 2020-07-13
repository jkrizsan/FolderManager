using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FolderManager.Api.DTOs
{
    public class AllFileResponseDto
    {
        public IEnumerable<string> Files { get; set; }
        public string ErrorMessage { get; set; }
    }
}
