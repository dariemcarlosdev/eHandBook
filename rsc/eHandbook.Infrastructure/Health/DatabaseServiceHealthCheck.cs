using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace eHandbook.Infrastructure.Health
{
    /// <summary>
    /// Checking DataBase Conection Health
    /// </summary>
    public class DatabaseServiceCustomHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        private readonly ILogger _logger;

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public DatabaseServiceCustomHealthCheck(IConfiguration configuration, ILogger<DatabaseServiceCustomHealthCheck> logger)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _logger = logger;
        }



        /// <inheritdoc/>
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);

                await sqlConnection.OpenAsync(cancellationToken);

                using var command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT 1";

                await command.ExecuteScalarAsync(cancellationToken);

                return HealthCheckResult.Healthy(description: "DataBase Status healthy.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return HealthCheckResult.Unhealthy(description: "DataBase is Unhealthy:" + e.Message, exception: e);
            }
        }
    }
}
