using Application.Extensions;
using Core.Interfaces;
using Data.Context;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebApi.Extensions;
using WebApi.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "Properties", "launchSettings.json"));

builder.Services.AddControllers();

builder.Services.AddHealthChecks().AddTypeActivatedCheck<DbConnectionHealthCheck>(
        "test",
        failureStatus: HealthStatus.Degraded,
        tags: new[] { "example" },
        args: new object[] { builder.Configuration.GetConnectionString("MSSQLConnectionString") });

builder.Services.AddEndpointsApiExplorer();

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.ConfigureFilesDirectoryPath(builder.Environment.WebRootPath);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    await DatabaseEnsureCreated.CreateDbWithValuesAsync(
        scope.ServiceProvider.GetRequiredService<IDbContextService>());
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/api");
});


await app.RunAsync();
