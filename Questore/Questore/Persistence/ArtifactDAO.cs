using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using Questore.Models;

namespace Questore.Persistence
{
    public class ArtifactDAO : IArtifactDAO
    {
        private readonly DBConnection _connection;

        private readonly string _table = "artifact";

        public ArtifactDAO()
        {
            _connection = new DBConnection();
        }
        public IEnumerable<Artifact> GetArtifacts()
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

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
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id, name, description, image_url, price " +
                        $"FROM {_table} " +
                        $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);
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
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();
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
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"UPDATE {_table} " +
                        $"SET name = '{updatedArtifact.Name}', " +
                        $"description = '{updatedArtifact.Description}', " +
                        $"image_url = 'default_url', " +
                        $"category_id = 1, " +
                        $"price = {updatedArtifact.Price} " +
                        $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = updatedArtifact.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = updatedArtifact.Description;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = "default_url";
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = 1;
            command.Parameters.Add("price", NpgsqlDbType.Integer).Value = updatedArtifact.Price;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteArtifact(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"DELETE FROM {_table}" +
                        $" WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

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
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT category_artifact.id, category_artifact.name " +
                        $"FROM category_artifact " +
                        $"INNER JOIN artifact on artifact.category_id = category_artifact.id " +
                        $"WHERE artifact.id = {id};";

            using var command = new NpgsqlCommand(query, connection);
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