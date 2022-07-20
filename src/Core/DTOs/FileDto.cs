using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class FileDto
    {
        public string Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long Size { get; set; }

        public FileDto(string id,
            string path,
            string name,
            string type,
            long size)
        {
            Id = id;
            Path = path;
            Name = name;
            Type = type;
            Size = size;
        }

        public FileDto()
        {
        }
    }
}
