using Core.DTOs;
using Core.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IFileService
    {
        Task<DownloadFileDto?> GetFileByIdOrNullAsync(string fileId);
        Task<DownloadFileDto?> GetFileByLinkOrNullAsync(string link);        
        Task<FileDto?> UploadAsync(IFormFile file);
        Task<ICollection<FileDto>> GetAllAsync();       
    }
}
