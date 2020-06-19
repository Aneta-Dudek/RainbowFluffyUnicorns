﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Questore.Models;

namespace Questore.Persistence
{
    public class Authentication
    {
        private readonly DBConnection _connection;

        private readonly IStudentDAO _studentDao;

        private readonly string _table = "student";

        public Authentication()
        {
            _connection = new DBConnection();

            _studentDao = new StudentDAO();
        }

        public Student Authenticate(Login login)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id " +
                        $"FROM {_table} " +
                        $"WHERE email = '{login.Identifier}' AND password = '{login.Password}';";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            reader.Read();
            var id = reader.GetInt32(0);


            var student = _studentDao.GetStudent(id);

            return student;
        }

    }
}
