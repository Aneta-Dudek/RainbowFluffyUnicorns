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

        private readonly string _table = "student";

        private readonly string _userTb = "credential";

        public Authentication(IStudentDAO studentDao, IConfiguration configuration) : base(configuration)
        {
            _studentDao = studentDao;
        }

        public User Authenticate(Login login)
        {
            var id = GetCredentialsId(login);
            if (id == 0)
                return null;

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
    }
}
