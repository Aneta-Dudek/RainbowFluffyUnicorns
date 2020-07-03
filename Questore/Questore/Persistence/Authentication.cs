using Microsoft.Extensions.Configuration;
using Npgsql;
 using Questore.Dtos;
 using Questore.Models;

namespace Questore.Persistence
{
    public class Authentication : DefaultDAO
    {
        private readonly IStudentDAO _studentDao;

        private readonly string _table = "student";

        public Authentication(IStudentDAO studentDao, IConfiguration configuration) : base(configuration)
        {
            _studentDao = studentDao;
        }

        public Student Authenticate(Login login)
        {
            var connection = Connection;
            var query = $"SELECT id " +
                        $"FROM {_table} " +
                        $"WHERE email = '{login.Email}' AND password = '{login.Password}';";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var id = reader.GetInt32(0);
                var student = _studentDao.GetStudent(id);
                return student;
            }

            return null;
        }
    }
}
