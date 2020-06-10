using Npgsql;
using Questore.Models;
using System.Collections.Generic;

namespace Questore.Persistence
{
    public class StudentDAO : IStudentDAO
    {
        private readonly DBConnection _connection;
        private readonly string _table = "student";

        public StudentDAO()
        {
            _connection = new DBConnection();
        }
        public IEnumerable<Student> GetStudents()
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();
            var query = $"SELECT * FROM {_table};";
            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var students = new List<Student>();

            while (reader.Read())
            {
                var student = provideOneStudent(reader);

                student.Artifacts = GetStudentArtifacts(student.Id);
                student.Classes = GetStudentClasses(student.Id);
                student.Teams = GetStudentTeams(student.Id);

                students.Add(student);
            }

            return students;
        }

        private Student provideOneStudent(NpgsqlDataReader reader)
        {
            var student = new Student()
            {
                Id = reader.GetInt32(0),
                FirstName = reader.GetString(1),
                LastName = reader.GetString(2),
                Email = reader.GetString(3),
                Coolcoins = reader.GetInt32(4),
                Experience = reader.GetInt32(5),
                ImageUrl = reader.GetString(6)
            };

            return student;
        }

        public IEnumerable<Artifact> GetStudentArtifacts(int id)
        {

        }

        public IEnumerable<Class> GetStudentClasses(int id)
        {

        }
        public IEnumerable<Team> GetStudentTeams(int id)
        {

        }
        public Student GetStudent(int id)
    {
        throw new System.NotImplementedException();
    }

    public void AddStudent(Student student)
    {
        using NpgsqlConnection connection = _connection.GetOpenConnectionObject();
        var query = $"INSERT INTO {_table} VALUES (@ );";
        using var command = new NpgsqlCommand(query, connection);
    }

    public void UpdateStudent(int id, Student updatedStudent)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteStudent(int id)
    {
        throw new System.NotImplementedException();
    }
}
}