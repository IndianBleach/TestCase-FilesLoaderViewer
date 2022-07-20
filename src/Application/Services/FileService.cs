using Core.DTOs;
using Core.Enums;
using Core.Interfaces;
using Dapper;
using Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileService : IFileService
    {
        private IDbContextService _ctx;
        private string _webPath;

        public FileService(IDbContextService ctx,
            IConfiguration config)
        {
            _ctx = ctx;
            _webPath = config["uploadDirPath"];
        }

        private static string GetFileFullPath(string fileLocalPath)
        {
            string loc = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\"));

            return loc + @"Data\Storage" + fileLocalPath;
        }

        public async Task<DownloadFileDto?> GetFileByIdOrNullAsync(string fileId)
        {
            string selectQuery = @"                
                select CONCAT(Files.Path, Files.Name) as FullPath, Type from Files
                    where Files.Id = @fileId";

            DownloadFileDto? file = await _ctx.MakeFirstOrDefaultAsync<DownloadFileDto?>(selectQuery, new { fileId });

            if (file == null) return null;

            file.FullPath = GetFileFullPath(file.FullPath);

            return file;
        }

        public async Task<DownloadFileDto?> GetFileByLinkOrNullAsync(string link)
        {
            string selectQuery = @"                
                select CONCAT(Files.Path, Files.Name) as FullPath, Type from FileLinks
                    inner join Files on Files.Id = FileLinks.FileId
                    where FileLinks.Link = @link;

                delete FileLinks
                where FileLinks.Link = @link";

            DownloadFileDto? file = await _ctx
                .MakeFirstOrDefaultAsync<DownloadFileDto?>(selectQuery, new { link });

            if (file == null) return null;

            file.FullPath = GetFileFullPath(file.FullPath);

            return file;
        }

        public async Task<ICollection<FileDto>> GetAllAsync()
        {
            string selectQuery = @"select id, name, path, size, type from Files";

            IEnumerable<FileDto> files = await _ctx.MakeQueryAsync<FileDto>(selectQuery);

            return files.ToList();
        }

        private static string GetFileFolderBaseOnFormat(IFormFile file)
        {
            string[] formats = new[] { "audio", "video", "application", "image" };

            string path = @"\";

            bool finded = false;
            for (int i = 0; i < formats.Length; i++)
            {
                if (!finded && file.ContentType.StartsWith(formats[i]))
                {
                    path += formats[i] + @"\";
                    finded = true;
                }
            }

            if (!finded) path += @"other\";

            return path;
        }

        public async Task<FileDto?> UploadAsync(IFormFile file)
        {
            string path = GetFileFolderBaseOnFormat(file);

            using FileStream fileStream = new(_webPath + path + file.FileName, FileMode.Create);
            await file.CopyToAsync(fileStream);

            string insertQuery = @"insert into Files values(@id, @name, @url, @size, @type)";

            FileInfo info = new(_webPath + path + file.FileName);

            string id = Guid.NewGuid().ToString();

            await _ctx.ExecuteAsync(insertQuery, new
            { id, name = file.FileName, url = path, size = info.Length, type = file.ContentType });

            return new FileDto(id, path, file.FileName, file.ContentType, info.Length);
        }
    }
}
