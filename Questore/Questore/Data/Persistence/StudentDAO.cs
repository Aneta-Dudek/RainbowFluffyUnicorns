using Npgsql;
using NpgsqlTypes;
using Questore.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Questore.Persistence
{
    public class StudentDAO : DefaultDAO, IStudentDAO
    {
        private readonly string _table = "student";

        public StudentDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Student> GetStudents()
        {
            using var connection = Connection;
            var query = $"SELECT id, first_name, last_name, coolcoins, experience, image_url, photo_id, credentials " +
                        $"FROM {_table};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var students = new List<Student>();

            while (reader.Read())
            {
                var student = ProvideOneStudent(reader);

                AssignStudentDetails(student);

                var email = GetStudentEmail(student.CredentialsId);
                student.Email = email;

                students.Add(student);
            }
            return students;
        }

        public Student GetStudent(int id)
        {
            using var connection = Connection;
            var query = $"SELECT id, first_name, last_name, coolcoins, experience, image_url, photo_id, credentials " +
                        $"FROM {_table} " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var student = new Student();

            while (reader.Read())
            {
                student = ProvideOneStudent(reader);
                var email = GetStudentEmail(student.CredentialsId);
                student.Email = email;

                AssignStudentDetails(student);
            }
            return student;
        }

        private void AssignStudentDetails(Student student)
        {
            student.Title = GetStudentTitleByExperience(student.Experience);
            student.Artifacts = GetStudentArtifacts(student.Id);
            student.Classes = GetStudentClasses(student.Id);
            student.Teams = GetStudentTeams(student.Id);
            student.Details = GetStudentDetails(student.Id);
        }

        public void AddStudent(Student student)
        {
            using var connection = Connection;
            var query = $"INSERT INTO {_table}(first_name, last_name, coolcoins, experience, image_url, photo_id, credentials) " +
                        $"VALUES (@first_name, @last_name, @coolcoins, @experience, @image_url, @photo_id, @credentials);";
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("first_name", NpgsqlDbType.Varchar).Value = student.FirstName;
            command.Parameters.Add("last_name", NpgsqlDbType.Varchar).Value = student.LastName;
            command.Parameters.Add("password", NpgsqlDbType.Varchar).Value = student.Password;
            command.Parameters.Add("coolcoins", NpgsqlDbType.Integer).Value = student.Coolcoins;
            command.Parameters.Add("experience", NpgsqlDbType.Integer).Value = student.Experience;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = "default_url";
            command.Parameters.Add("credentials", NpgsqlDbType.Integer).Value = student.CredentialsId;
            command.Parameters.Add("photo_id", NpgsqlDbType.Varchar).Value = "default";

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateStudent(int id, Student updatedStudent)
        {
            using var connection = Connection;
            var query = $"UPDATE {_table} " +
                        $"SET first_name = @first_name, " +
                        $"last_name = @last_name, " +
                        $"photo_id = @photo_id, " +
                        $"coolcoins = @coolcoins, " +
                        $"experience = @experience, " +
                        $"image_url = @image_url " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("first_name", NpgsqlDbType.Varchar).Value = updatedStudent.FirstName;
            command.Parameters.Add("last_name", NpgsqlDbType.Varchar).Value = updatedStudent.LastName;
            command.Parameters.Add("coolcoins", NpgsqlDbType.Integer).Value = updatedStudent.Coolcoins;
            command.Parameters.Add("experience", NpgsqlDbType.Integer).Value = updatedStudent.Experience;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = updatedStudent.ImageUrl;
            command.Parameters.Add("photo_id", NpgsqlDbType.Varchar).Value = updatedStudent.PhotoId;
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteStudent(int id)
        {
            using var connection = Connection;
            var query = $"DELETE FROM {_table}" +
                        $" WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private Student ProvideOneStudent(NpgsqlDataReader reader)
        {
            var student = new Student()
            {
                Id = reader.GetInt32((int)DBUtilities.StudentEnum.Id),
                FirstName = reader.GetString((int)DBUtilities.StudentEnum.FirstName),
                LastName = reader.GetString((int)DBUtilities.StudentEnum.LastName),
                Coolcoins = reader.GetInt32((int)DBUtilities.StudentEnum.Coolcoins),
                Experience = reader.GetInt32((int)DBUtilities.StudentEnum.Experience),
                ImageUrl = reader.GetString((int)DBUtilities.StudentEnum.ImageUrl),
                PhotoId = reader.GetString((int)DBUtilities.StudentEnum.PublicImageId),
                CredentialsId = reader.GetInt32((int)DBUtilities.StudentEnum.Credentials)
            };

            return student;
        }

        private string GetStudentEmail(int id)
        {
            using var connection = Connection;
            var query = "SELECT email FROM credential" +
                        $" WHERE id = {id};";

            using var commandStudent = new NpgsqlCommand(query, connection);
            var reader = commandStudent.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                var studentEmail = reader.GetString(0);
                return studentEmail;
            }

            return null;
        }

        private Artifact ProvideOneStudentArtifact(NpgsqlDataReader reader)
        {
            var artifact = new Artifact()
            {
                Id = reader.GetInt32((int)DBUtilities.ArtifactEnum.Id),
                Name = reader.GetString((int)DBUtilities.ArtifactEnum.Name),
                Description = reader.GetString((int)DBUtilities.ArtifactEnum.Description),
                ImageUrl = reader.GetString((int)DBUtilities.ArtifactEnum.ImageUrl),
                Price = reader.GetInt32((int)DBUtilities.ArtifactEnum.Price),
                IsUsed = reader.GetBoolean((int)DBUtilities.ArtifactEnum.IsUsed),
                StudentArtifactId = reader.GetInt32((int)DBUtilities.ArtifactEnum.StudentArtifactId)
            };

            artifact.Category = GetArtifactCategory(artifact.Id);

            return artifact;
        }

        private Class ProvideOneClass(NpgsqlDataReader reader)
        {
            var newClass = new Class()
            {
                Id = reader.GetInt32((int)DBUtilities.ClassEnum.Id),
                Name = reader.GetString((int)DBUtilities.ClassEnum.Name),
                DateStarted = reader.GetDateTime((int)DBUtilities.ClassEnum.DateStarted),
                ImageUrl = reader.GetString((int)DBUtilities.ClassEnum.ImageUrl)
            };

            return newClass;
        }

        private Team ProvideOneTeam(NpgsqlDataReader reader)
        {
            var team = new Team()
            {
                Id = reader.GetInt32((int)DBUtilities.TeamEnum.Id),
                Name = reader.GetString((int)DBUtilities.TeamEnum.Name),
                ImageUrl = reader.GetString((int)DBUtilities.TeamEnum.ImageUrl)
            };

            return team;
        }

        private Title ProvideOneTitle(NpgsqlDataReader reader)
        {
            var title = new Title()
            {
                Id = reader.GetInt32((int)DBUtilities.TitleEnum.Id),
                Name = reader.GetString((int)DBUtilities.TitleEnum.Name),
                Threshold = reader.GetInt32((int)DBUtilities.TitleEnum.Threshold)
            };

            return title;
        }

        private StudentDetail ProvideOneDetail(NpgsqlDataReader reader)
        {
            var detail = new StudentDetail()
            {
                Id = reader.GetInt32((int)DBUtilities.StudentDetailEnum.Id),
                Name = reader.GetString((int)DBUtilities.StudentDetailEnum.Name),
                Content = reader.GetString((int)DBUtilities.StudentDetailEnum.Content)
            };

            return detail;
        }

        private IEnumerable<Artifact> GetStudentArtifacts(int id)
        {
            using var connection = Connection;
            var query = $"SELECT artifact.id, artifact.name, artifact.description, artifact.image_url, artifact.price, student_artifact.is_used, student_artifact.id " +
                        $"FROM artifact " +
                        $"INNER JOIN student_artifact ON artifact.id = student_artifact.artifact_id " +
                        $"WHERE student_artifact.student_id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var artifacts = new List<Artifact>();

            while (reader.Read())
            {
                artifacts.Add(ProvideOneStudentArtifact(reader));
            }

            return artifacts;
        }

        private Category GetArtifactCategory(int id)
        {
            using var connection = Connection;
            var query = $"SELECT category_artifact.id, category_artifact.name " +
                        $"FROM category_artifact " +
                        $"INNER JOIN artifact on artifact.category_id = category_artifact.id " +
                        $"WHERE artifact.id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var category = new Category();

            while (reader.Read())
            {
                category.Id = reader.GetInt32((int)DBUtilities.CategoryEnum.Id);
                category.Name = reader.GetString((int)DBUtilities.CategoryEnum.Name);
            }

            return category;
        }
        private IEnumerable<Class> GetStudentClasses(int id)
        {
            using var connection = Connection;
            var query = $"SELECT class.id, class.name, class.date_started, class.image_url " +
                        $"FROM class " +
                        $"INNER JOIN student_class ON class.id = student_class.class_id " +
                        $"WHERE student_class.student_id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
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
            using var connection = Connection;
            var query = $"SELECT team.id, team.name, team.image_url " +
                        $"FROM team " +
                        $"INNER JOIN student_team ON team.id = student_team.team_id " +
                        $"WHERE student_team.student_id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var teams = new List<Team>();

            while (reader.Read())
            {
                teams.Add(ProvideOneTeam(reader));
            }

            return teams;
        }

        private IEnumerable<StudentDetail> GetStudentDetails(int id)
        {
            using var connection = Connection;
            var query = $"SELECT detail.id, detail.name, detail.content " +
                        $"FROM detail " +
                        $"INNER JOIN student ON detail.student_id = student.id " +
                        $"WHERE student.id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var details = new List<StudentDetail>();

            while (reader.Read())
            {
                details.Add(ProvideOneDetail(reader));
            }

            return details;
        }

        private Title GetStudentTitleByExperience(int experience)
        {
            using var connection = Connection;
            var query = $"SELECT id, name, threshold FROM title WHERE threshold >= @experience;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("experience", NpgsqlDbType.Integer).Value = experience;
            var reader = command.ExecuteReader();

            var studentTitles = new List<Title>();

            while (reader.Read())
            {
                studentTitles.Add(ProvideOneTitle(reader));
            }

            Title studentTitle = studentTitles.OrderBy(t => t.Threshold).FirstOrDefault();

            return studentTitle;
        }
    }
}