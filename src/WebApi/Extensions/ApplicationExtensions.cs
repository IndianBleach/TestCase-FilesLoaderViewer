namespace WebApi.Extensions
{
    internal static class ApplicationExtensions
    {
        internal static void ConfigureFilesDirectoryPath(this WebApplication app, string webRootPath)
            => app.Configuration["uploadDirPath"] = webRootPath + "../Data/Storage/";
    }
}
