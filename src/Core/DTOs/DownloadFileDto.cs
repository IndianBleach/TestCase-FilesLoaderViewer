using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class DownloadFileDto
    {
        public string Type { get; set; }
        public string FullPath { get; set; }

        public DownloadFileDto(string type, string fullPath)
        {
            Type = type;
            FullPath = fullPath;
        }

        public DownloadFileDto()
        {
        }
    }
}
