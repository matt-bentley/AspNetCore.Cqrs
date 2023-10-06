using AspNetCore.Cqrs.Infrastructure.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using System.Data;

namespace AspNetCore.Cqrs.Infrastructure.Repositories
{
    public sealed class DapperContext
    {
        private readonly string? _connectionString;

        public DapperContext(IOptions<DatabaseSettings> options)
        {
            _connectionString = options.Value.SqlConnectionString;
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
