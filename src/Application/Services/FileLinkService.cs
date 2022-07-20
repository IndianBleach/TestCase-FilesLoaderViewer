using Core.Interfaces;
using Dapper;
using Data.Context;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class FileLinkService : IFileLinkService
    {
        private readonly IDbContextService _ctx;
        private readonly IConfiguration _config;

        public FileLinkService(IDbContextService ctx,
            IConfiguration configuration)
        {
            _ctx = ctx;
            _config = configuration;
        }
        
        public async Task<string?> CreateFileLinkOrDefaultAsync(string? fileId)
        {
            if (string.IsNullOrEmpty(fileId)) 
                return null;

            string fileExist = await _ctx.MakeFirstOrDefaultAsync<string>("select Id from Files where Id = @fileId",
                new { fileId });

            if (string.IsNullOrEmpty(fileExist)) 
                return null;

            StringBuilder builder = new();
            Enumerable
               .Range(65, 26)
                .Select(e => ((char)e).ToString())
                .Concat(Enumerable.Range(97, 26).Select(e => ((char)e).ToString()))
                .Concat(Enumerable.Range(0, 10).Select(e => e.ToString()))
                .OrderBy(e => Guid.NewGuid())
                .Take(16)
                .ToList().ForEach(e => builder.Append(e));

            string query = @"
                use FileX;
                begin tran
                if exists (select * from FileLinks where FileId = @fileId)
                begin
                   update FileLinks set Link = @link
                   where FileId = @fileId
                end
                else
                begin
                   insert into FileLinks
                   values (@link, @fileId)
                end
                commit tran";

            await _ctx.ExecuteAsync(query, new { link = builder.ToString(), fileId });

            return _config["profiles:WebApi:applicationUrl"] + "/api/files/l/" + builder.ToString();
        }
    }
}
