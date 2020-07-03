using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Questore.Persistence
{
    public class DefaultDAO
    {
        private readonly IConfiguration _configuration;

        private NpgsqlConnection _connection;

        protected NpgsqlConnection Connection
        {
            get
            {
                _connection = new NpgsqlConnection(_configuration.GetConnectionString("DefaultConnection"));
                _connection.Open();
                return _connection;
            }
        }

        public DefaultDAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
