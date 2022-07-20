using Core.DTOs;
using Core.Interfaces;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

            else return BadRequest(HttpStatusCode.NotFound);
            
        }

        [HttpGet("getlink")]
        public async Task<IActionResult> GetFileLink([FromQuery]string? file)
        {
            string? res = await _linkService.CreateFileLinkOrDefaultAsync(file);

            if (string.IsNullOrEmpty(res)) 
                return BadRequest(HttpStatusCode.BadRequest);

            else return Ok(res);
        }

        [HttpGet("l/{fileLink}")]
        public async Task<IActionResult> DownloadByLink([FromRoute] string fileLink)
        {
            DownloadFileDto? file = await _fileService.GetFileByLinkOrNullAsync(fileLink);

            if (file != null) 
                return PhysicalFile(file.FullPath, file.Type);

            return BadRequest(HttpStatusCode.BadRequest);
        }

        [HttpGet("all")]
        public async Task<IActionResult> ViewAll()
            => Ok(await _fileService.GetAllAsync());

        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = long.MaxValue)]
        [HttpPost("upload")]
        [Produces("application/json")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            FileDto? fileDto = await _fileService.UploadAsync(file);

            if (fileDto != null) return Json(fileDto);

            return BadRequest(HttpStatusCode.BadRequest);
        }
    }
}
