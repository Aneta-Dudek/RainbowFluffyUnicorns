using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using Questore.Data.Interfaces;
using Questore.Data.Models;

namespace Questore.Data.Persistence
{
    public class AdminDAO : DefaultDAO, IAdminDAO
    {
        private readonly string _table = "admin";

        public AdminDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public Admin GetAdmin(int id)
        {
            using var connection = Connection;
            var query = $"SELECT id, firstname, lastname, image_url, photo_id, credentials " +
                        $"FROM {_table} " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;
            var reader = command.ExecuteReader();

            var admin = new Admin();

            while (reader.Read())
            {
                admin = ProvideOneAdmin(reader);
                admin.Email = GetAdminEmail(admin.CredentialsId);
            }

            return admin;
        }

        private Admin ProvideOneAdmin(NpgsqlDataReader reader)
        {
            var admin = new Admin()
            {
                Id = reader.GetInt32((int)DBUtilities.AdminEnum.Id),
                FirstName = reader.GetString((int)DBUtilities.AdminEnum.FirstName),
                LastName = reader.GetString((int)DBUtilities.AdminEnum.LastName),
                ImageUrl = reader.GetString((int)DBUtilities.AdminEnum.ImageUrl),
                PhotoId = reader.GetString((int)DBUtilities.AdminEnum.PublicImageId),
                CredentialsId = reader.GetInt32((int)DBUtilities.AdminEnum.Credentials)
            };
            return admin;
        }

        private string GetAdminEmail(int id)
        {
            using var connection = Connection;
            var query = "SELECT email FROM credential" +
                        $" WHERE id = {id};";

            using var commandStudent = new NpgsqlCommand(query, connection);
            var reader = commandStudent.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();
                return reader.GetString(0);
            }

            return null;
        }
    }
}
