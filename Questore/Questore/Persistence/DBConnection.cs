using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Questore.Persistence
{
    public class DBConnection
    {
        private readonly IConfiguration _config;

        public DBConnection(IConfiguration configuration)
        {
            _config = configuration;
        }

        public NpgsqlConnection GetOpenConnectionObject()
        {
            var connection = new NpgsqlConnection(_config.GetConnectionString("DefaultConnection"));
            connection.Open();
            return connection;
        }
    }
}