using Microsoft.Extensions.Configuration;
using Npgsql;
using Questore.Data.Dtos;
using Questore.Data.Interfaces;

namespace Questore.Data.Persistence
{
    public class DetailsDAO : DefaultDAO, IDetailsDAO
    {
        public DetailsDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public void AddDetail(DetailDto detailDto)
        {
            using var connection = Connection;
            var query = "INSERT INTO detail (name, content, student_id)" +
                        $"VALUES ('{detailDto.Name}','{detailDto.Content}', {detailDto.StudentId})";

            using var command = new NpgsqlCommand(query, connection);
            command.ExecuteNonQuery();
        }

        public void DeleteDetail(int id)
        {
            using var connection = Connection;
            var query = "DELETE FROM detail " +
                        $"WHERE id = {id};";
            using var command = new NpgsqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
