using Application.Services;
using Core.Interfaces;
using Data.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Extensions
{
    public static class ConfigurationServicesExtensions
    {
        public static void ConfigureServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddTransient<IFileService, FileService>();

            services.AddTransient<IFileLinkService, FileLinkService>();

            services.AddSingleton<IDbContextService>(new ApplicationDbContextService(
                config.GetConnectionString("MSSQLConnectionString")));
        }
    }
}
