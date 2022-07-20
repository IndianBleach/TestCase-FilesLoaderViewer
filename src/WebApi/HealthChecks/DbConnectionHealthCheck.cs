using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.HealthChecks
{
    public class DbConnectionHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public DbConnectionHealthCheck(string sqlConnectionString)
        {
            _connectionString = sqlConnectionString;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            try
            {                
                connection.Open();                

                return Task.FromResult(
                    HealthCheckResult.Healthy("Database connection: healthy"));
            }
            catch (Exception exp)
            {
                return Task.FromResult(
                HealthCheckResult.Unhealthy($"Database connection: unhealthy ({exp.Message})"));
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
