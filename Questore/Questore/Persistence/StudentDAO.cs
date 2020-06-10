using Npgsql;
using NpgsqlTypes;
using Questore.Models;
using System.Collections.Generic;

namespace Questore.Persistence
{
    public class StudentDAO : IStudentDAO
    {
        private readonly DBConnection _connection;

        private readonly string _table = "student";

        private enum StudentEnum { Id, FirstName, LastName, Email, Coolcoins, Experience, ImageUrl, TitleId }
        private enum ArtifactEnum { Id, Name, Description, ImageUrl, Price }
        private enum ClassEnum { Id, Name, DateStarted, ImageUrl }
        private enum TeamEnum { Id, Name, ImageUrl }
        private enum CategoryEnum { Id, Name }
        private enum TitleEnum { Id, Name, Threshold }

        public StudentDAO()
        {
            _connection = new DBConnection();
        }

        public IEnumerable<Student> GetStudents()
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id, first_name, last_name, email, coolcoins, experience, image_url, title_id " +
                              $"FROM {_table};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var students = new List<Student>();

            while (reader.Read())
            {
                var student = ProvideOneStudent(reader);

                AssignStudentDetails(student);

                students.Add(student);
            }

            return students;
        }

        public Student GetStudent(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id, first_name, last_name, email, coolcoins, experience, image_url, title_id " +
                              $"FROM {_table}" +
                              $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var student = new Student();

            while (reader.Read())
            {
                student = ProvideOneStudent(reader);

                AssignStudentDetails(student);
            }

            return student;
        }

        //Jakiś pomysł jak to nazwać?
        private void AssignStudentDetails(Student student)
        {
            student.Title = GetStudentTitle(student.Id);
            student.Artifacts = GetStudentArtifacts(student.Id);
            student.Classes = GetStudentClasses(student.Id);
            student.Teams = GetStudentTeams(student.Id);
        }

        public void AddStudent(Student student)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();
            var query = $"INSERT INTO {_table}(first_name, last_name, email, password, coolcoins, experience, image_url, title_id) " +
                              $"VALUES (@first_name, @last_name, @email, @password, @coolcoins, @experience, @image_url, @title_id);";
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("first_name", NpgsqlDbType.Varchar).Value = student.FirstName;
            command.Parameters.Add("last_name", NpgsqlDbType.Varchar).Value = student.LastName;
            command.Parameters.Add("email", NpgsqlDbType.Varchar).Value = student.Email;
            command.Parameters.Add("password", NpgsqlDbType.Varchar).Value = student.Password;
            command.Parameters.Add("coolcoins", NpgsqlDbType.Integer).Value = student.Coolcoins;
            command.Parameters.Add("experience", NpgsqlDbType.Integer).Value = student.Experience;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = student.ImageUrl;
            command.Parameters.Add("title_id", NpgsqlDbType.Integer).Value = GetStudentTitleIdByExperience(student.Experience);

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateStudent(int id, Student updatedStudent)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"UPDATE {_table} " +
                        $"SET first_name = '{updatedStudent.FirstName}', " +
                             $"last_name = '{updatedStudent.LastName}', " +
                             $"email = '{updatedStudent.Email}', " +
                             $"password = '{updatedStudent.Password}', " +
                             $"coolcoins = {updatedStudent.Coolcoins}, " +
                             $"experience = {updatedStudent.Experience}, " +
                             $"image_url = '{updatedStudent.ImageUrl}', " +
                             $"title_id = {GetStudentTitleIdByExperience(updatedStudent.Experience)} " +
                        $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("first_name", NpgsqlDbType.Varchar).Value = updatedStudent.FirstName;
            command.Parameters.Add("last_name", NpgsqlDbType.Varchar).Value = updatedStudent.LastName;
            command.Parameters.Add("email", NpgsqlDbType.Varchar).Value = updatedStudent.Email;
            command.Parameters.Add("password", NpgsqlDbType.Varchar).Value = updatedStudent.Password;
            command.Parameters.Add("coolcoins", NpgsqlDbType.Integer).Value = updatedStudent.Coolcoins;
            command.Parameters.Add("experience", NpgsqlDbType.Integer).Value = updatedStudent.Experience;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = updatedStudent.ImageUrl;
            command.Parameters.Add("title_id", NpgsqlDbType.Integer).Value = GetStudentTitleIdByExperience(updatedStudent.Experience);

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteStudent(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"DELETE FROM {_table}" +
                              $" WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private Student ProvideOneStudent(NpgsqlDataReader reader)
        {
            var student = new Student()
            {
                Id = reader.GetInt32((int)StudentEnum.Id),
                FirstName = reader.GetString((int)StudentEnum.FirstName),
                LastName = reader.GetString((int)StudentEnum.LastName),
                Email = reader.GetString((int)StudentEnum.Email),
                Coolcoins = reader.GetInt32((int)StudentEnum.Coolcoins),
                Experience = reader.GetInt32((int)StudentEnum.Experience),
                ImageUrl = reader.GetString((int)StudentEnum.ImageUrl)
            };

            return student;
        }

        private Artifact ProvideOneArtifact(NpgsqlDataReader reader)
        {
            var artifact = new Artifact()
            {
                Id = reader.GetInt32((int)ArtifactEnum.Id),
                Name = reader.GetString((int)ArtifactEnum.Name),
                Description = reader.GetString((int)ArtifactEnum.Description),
                ImageUrl = reader.GetString((int)ArtifactEnum.ImageUrl),
                Price = reader.GetInt32((int)ArtifactEnum.Price)
            };

            artifact.Category = GetArtifactCategory(artifact.Id);

            return artifact;
        }

        private Class ProvideOneClass(NpgsqlDataReader reader)
        {
            var newClass = new Class()
            {
                Id = reader.GetInt32((int)ClassEnum.Id),
                Name = reader.GetString((int)ClassEnum.Name),
                DateStarted = reader.GetDateTime((int)ClassEnum.DateStarted),
                ImageUrl = reader.GetString((int)ClassEnum.ImageUrl)
            };

            return newClass;
        }

        private Team ProvideOneTeam(NpgsqlDataReader reader)
        {
            var team = new Team()
            {
                Id = reader.GetInt32((int)TeamEnum.Id),
                Name = reader.GetString((int)TeamEnum.Name),
                ImageUrl = reader.GetString((int)TeamEnum.ImageUrl)
            };

            return team;
        }

        private IEnumerable<Artifact> GetStudentArtifacts(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT artifact.id, artifact.name, artifact.description, artifact.image_url, artifact.price " +
                              $"FROM artifact " +
                              $"INNER JOIN student_artifact ON artifact.id = student_artifact.artifact_id " +
                              $"WHERE student_artifact.student_id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var artifacts = new List<Artifact>();

            while (reader.Read())
            {
                artifacts.Add(ProvideOneArtifact(reader));
            }

            return artifacts;
        }

        private Category GetArtifactCategory(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT category.id, category.name " +
                              $"FROM category " +
                              $"INNER JOIN artifact on artifact.category_id = category.id " +
                              $"WHERE artifact.id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var category = new Category();

            while (reader.Read())
            {
                category.Id = reader.GetInt32((int)CategoryEnum.Id);
                category.Name = reader.GetString((int)CategoryEnum.Name);
            }

            return category;
        }
        private IEnumerable<Class> GetStudentClasses(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT class.id, class.name, class.date_started, class.image_url " +
                              $"FROM class " +
                              $"INNER JOIN student_class ON class.id = student_class.class_id " +
                              $"WHERE student_class.student_id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var classes = new List<Class>();

            while (reader.Read())
            {
                classes.Add(ProvideOneClass(reader));
            }

            return classes;
        }

        private IEnumerable<Team> GetStudentTeams(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT team.id, team.name, team.image_url " +
                              $"FROM team " +
                              $"INNER JOIN student_team ON team.id = student_team.team_id " +
                              $"WHERE student_team.student_id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var teams = new List<Team>();

            while (reader.Read())
            {
                teams.Add(ProvideOneTeam(reader));
            }

            return teams;
        }

        private Title GetStudentTitle(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT title.id, title.name, title.threshold " +
                        $"FROM title " +
                        $"INNER JOIN student on student.title_id = title.id " +
                        $"WHERE student.id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var title = new Title();

            while (reader.Read())
            {
                title.Id = reader.GetInt32((int)TitleEnum.Id);
                title.Name = reader.GetString((int)TitleEnum.Name);
                title.Threshold = reader.GetInt32((int)TitleEnum.Id);
            }

            return title;
        }

        private int GetStudentTitleIdByExperience(int experience)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            //QUERY nie działa podobnie jak milion innych, które próbowałem typu:
            //SELECT id, MIN(threshold) FROM (SELECT id, threshold FROM title WHERE threshold > 200) s GROUP BY id;

            var query = $"SELECT id FROM title GROUP BY id HAVING MIN (threshold) > {experience};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var studentId = 0;

            while (reader.Read())
            {
                studentId = reader.GetInt32((int)TitleEnum.Id);
            }

            return studentId;
        }
    }
}