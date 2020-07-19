using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using Questore.Data.Interfaces;
using Questore.Data.Models;
using System.Collections.Generic;

namespace Questore.Data.Persistence
{
    public class ArtifactDAO : DefaultDAO, IArtifactDAO
    {
        private readonly string _table = "artifact";

        public ArtifactDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public ICollection<Artifact> GetArtifacts()
        {
            using var connection = Connection;
            var query = $"SELECT id, name, description, image_url, price " +
                        $"FROM {_table};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var artifacts = new List<Artifact>();

            while (reader.Read())
            {
                var artifact = ProvideOneArtifact(reader);

                artifacts.Add(artifact);
            }

            return artifacts;
        }

        public Artifact GetArtifact(int id)
        {
            using var connection = Connection;
            var query = $"SELECT id, name, description, image_url, price " +
                        $"FROM {_table} " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var artifact = new Artifact();

            while (reader.Read())
            {
                artifact = ProvideOneArtifact(reader);
            }

            return artifact;
        }

        public void AddArtifact(Artifact artifact)
        {
            using var connection = Connection;
            var query = $"INSERT INTO {_table}(name, description, image_url, category_id, price) " +
                        $"VALUES (@name, @description, @image_url, @category_id, @price);";
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = artifact.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = artifact.Description;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = "default_url";
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = 1;
            command.Parameters.Add("price", NpgsqlDbType.Integer).Value = artifact.Price;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateArtifact(int id, Artifact updatedArtifact)
        {
            using var connection = Connection;
            var query = $"UPDATE {_table} " +
                        $"SET name = @name, " +
                        $"description = @description, " +
                        $"image_url = @image_url, " +
                        $"category_id = @category_id, " +
                        $"price = @price " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = updatedArtifact.Name;
            command.Parameters.Add("@description", NpgsqlDbType.Varchar).Value = updatedArtifact.Description;
            command.Parameters.Add("@image_url", NpgsqlDbType.Varchar).Value = "test_url";
            command.Parameters.Add("@category_id", NpgsqlDbType.Integer).Value = 1;
            command.Parameters.Add("@price", NpgsqlDbType.Integer).Value = updatedArtifact.Price;
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UseArtifact(int id)
        {
            using var connection = Connection;
            var query = $"UPDATE student_{_table} " +
                        $"SET is_used = @is_used " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("@is_used", NpgsqlDbType.Boolean).Value = true;
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void BuyArtifact(int artifactId, int studentId)
        {
            if (IsArtifactAffordable(artifactId, studentId))
            {
                ClaimArtifact(artifactId, studentId);
                UpdateCoolcoinsAfterPurchase(artifactId, studentId);
            }
        }

        private void ClaimArtifact(int artifactId, int studentId)
        {
            using var connection = Connection;
            var query = $"INSERT INTO student_{_table}(student_id, artifact_id) " +
                        $"VALUES (@studentId, @artifactId);";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("studentId", NpgsqlDbType.Integer).Value = studentId;
            command.Parameters.Add("artifactId", NpgsqlDbType.Integer).Value = artifactId;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private void UpdateCoolcoinsAfterPurchase(int artifactId, int studentId)
        {
            using var connection = Connection;
            var query = $"UPDATE student " +
                        $"SET coolcoins = student.coolcoins - artifact.price " +
                        $"FROM artifact " +
                        $"WHERE artifact.id = @artifactId AND student.id = @studentId;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("studentId", NpgsqlDbType.Integer).Value = studentId;
            command.Parameters.Add("artifactId", NpgsqlDbType.Integer).Value = artifactId;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private bool IsArtifactAffordable(int artifactId, int studentId)
        {
            using var connection = Connection;
            var query = $"SELECT artifact.id " +
                        $"FROM {_table}, student " +
                        $"WHERE student.id = @studentId AND student.coolcoins >= artifact.price;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("studentId", NpgsqlDbType.Integer).Value = studentId;
            var reader = command.ExecuteReader();

            var artifactIds = new List<int>();

            while (reader.Read())
            {
                artifactIds.Add(reader.GetInt32((int)DBUtilities.ArtifactEnum.Id));
            }

            if (artifactIds.Contains(artifactId)) return true;

            return false;
        }

        public void DeleteArtifact(int id)
        {
            using var connection = Connection;
            var query = $"DELETE FROM {_table}" +
                        $" WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteStudentArtifact(int id)
        {
            using var connection = Connection;
            var query = $"DELETE FROM student_{_table}" +
                        $" WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private Artifact ProvideOneArtifact(NpgsqlDataReader reader)
        {
            var artifact = new Artifact()
            {
                Id = reader.GetInt32((int)DBUtilities.ArtifactEnum.Id),
                Name = reader.GetString((int)DBUtilities.ArtifactEnum.Name),
                Description = reader.GetString((int)DBUtilities.ArtifactEnum.Description),
                ImageUrl = reader.GetString((int)DBUtilities.ArtifactEnum.ImageUrl),
                Price = reader.GetInt32((int)DBUtilities.ArtifactEnum.Price)
            };

            artifact.Category = GetArtifactCategory(artifact.Id);

            return artifact;
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
    }
}