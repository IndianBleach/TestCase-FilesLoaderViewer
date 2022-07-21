using Core.Interfaces;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Context
{
    public static class ApplicationContextSeed
    {
        public static async Task SeedDatabaseAsync(IDbContextService dbService)
        {
            int? dbExist = await dbService.MakeFirstOrDefaultAsync<int>
            ("SELECT count(*) FROM Sys.Tables");

            if (!dbExist.HasValue || dbExist.Value == 0)
            {
                string query = @"
                create table Files 
                (
                    Id nvarchar(200) primary key,
                    Name nvarchar(128) not null,
                    Path nvarchar(max) not null,
                    Size decimal  not null,
                    Type nvarchar(max) not null    
                )
                create table FileLinks
                (
                    Id int identity(1,1) primary key,
                    Link nvarchar(max) not null,
                    FileId nvarchar(200) foreign key references Files(Id) not null,    
                )
                ";

                await dbService.ExecuteAsync(query);
            }
        }
    }
}
