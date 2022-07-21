using Application.Models;
using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Net;
using WebApi.Controllers.Common;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]    
    public class FilesController : ApiController
    {
        private readonly IFileService _fileService;
        private readonly IFileLinkService _linkService;

        public FilesController(IFileService fileService, 
            IFileLinkService linkService)
        {
            _fileService = fileService;
            _linkService = linkService;
        }

        [HttpGet("download/{fileId}")]
        public async Task<IActionResult> DownloadById([FromRoute]string fileId)
        {
            DownloadFileDto? file = await _fileService.GetFileByIdOrNullAsync(fileId);

            if (file != null) 
                return PhysicalFile(file.FullPath, file.Type);

            else return BadRequest(ApiResult<DownloadFileDto>
                .Failed(file, new List<string>() { "File not found"}));
            
        }

        [HttpPost("getlink")]
        public async Task<IActionResult> GetFileLink([FromQuery]string? file)
        {
            string? res = await _linkService.CreateFileLinkOrDefaultAsync(file);

            if (string.IsNullOrEmpty(res)) 
                return BadRequest(ApiResult<string>
                .Failed(res, new List<string>() { "File not found", "Something went wrong" }));

            else return Ok(ApiResult<string>
                .SuccessOk(res));
        }

        [HttpGet("l/{fileLink}")]        
        public async Task<IActionResult> DownloadByLink([FromRoute] string fileLink)
        {
            DownloadFileDto? file = await _fileService.GetFileByLinkOrNullAsync(fileLink);

            if (file != null) 
                return PhysicalFile(file.FullPath, file.Type);

            return BadRequest(ApiResult<DownloadFileDto>
                .Failed(file, new List<string>() { "Link is incorrect", "Link has been already used" }));
        }

        [HttpGet("all")]
        public async Task<IActionResult> ViewAll()
            => Ok(ApiResult<ICollection<FileDto>>
                .SuccessOk(await _fileService.GetAllAsync()));

        [HttpPost("upload")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm]IFormFile file)
        {
            FileDto? fileDto = await _fileService.UploadAsync(file);

            if (fileDto != null) return Ok(ApiResult<FileDto>
                .SuccessOk(fileDto));

            return BadRequest(ApiResult<FileDto>
                .Failed(fileDto, new List<string>() { "Some errors with upload file", "Check file limit size or format" }));
        }
    }
}
