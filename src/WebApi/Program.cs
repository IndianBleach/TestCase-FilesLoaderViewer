using Application.Extensions;
using Core.Interfaces;
using Data.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Extensions;
using WebApi.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"));

builder.Services.AddControllers();

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy(
//        policy =>
//        {
//            policy.WithOrigins("https://localhost:7191", "https://localhost:7145");
//        });
//});

builder.Services.AddHealthChecks().AddTypeActivatedCheck<DbConnectionHealthCheck>(
        "test",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "testdb" },
        args: new object[] { builder.Configuration.GetConnectionString("MSSQLConnectionString") });

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

//app.UseCors();

app.ConfigureFilesDirectoryPath(builder.Environment.WebRootPath);

app.UseHttpsRedirection();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await ApplicationContextSeed.SeedDatabaseAsync(
        scope.ServiceProvider.GetRequiredService<IDbContextService>());
}

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/api");
});


await app.RunAsync();
