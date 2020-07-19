using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Questore.Data.Dtos;
using Questore.Data.Interfaces;
using Questore.Data.Models;

namespace Questore.Data.Persistence
{
    public class Authentication : DefaultDAO, IAuthentication
    {
        private readonly IStudentDAO _studentDao;
        private readonly IAdminDAO _adminDao;
        private readonly string _table = "student";
        private readonly string _userTb = "credential";

        public Authentication(IStudentDAO studentDao, IAdminDAO adminDao, IConfiguration configuration) : base(configuration)
        {
            _studentDao = studentDao;
            _adminDao = adminDao;
        }

        public User Authenticate(Login login)
        {
            var id = GetCredentialsId(login);
            if (id == 0)
                return null;

            User user = GetStudent(id);

            if (user != null)
            {
                return user;
            }

            return GetAdmin(id);
        }

        private int GetCredentialsId(Login login)
        {
            using var connection = Connection;
            var id = 0;
            var query = "SELECT id " +
                        $"FROM {_userTb} " +
                        $"WHERE email = '{login.Email}' AND password = '{login.Password}';";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                id = reader.GetInt32(0);
            }

            return id;
        }

        private Student GetStudent(int id)
        {
            var query = "SELECT id " +
                        $"FROM {_table} " +
                        $"WHERE credentials = '{id}'";

            using var connection = Connection;
            using var commandStudent = new NpgsqlCommand(query, connection);
            var reader = commandStudent.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var studentId = reader.GetInt32(0);
                var student = _studentDao.GetStudent(studentId);
                return student;
            }

            return null;
        }

        private Admin GetAdmin(int id)
        {
            var query = "SELECT id " +
                        "FROM admin " +
                        $"WHERE credentials = '{id}'";

            using var connection = Connection;
            using var commandStudent = new NpgsqlCommand(query, connection);
            var reader = commandStudent.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var adminId = reader.GetInt32(0);
                var admin = _adminDao.GetAdmin(adminId);
                return admin;
            }

            return null;
        }
    }
}
