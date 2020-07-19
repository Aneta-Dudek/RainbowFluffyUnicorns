﻿using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Questore.Data.Persistence
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
