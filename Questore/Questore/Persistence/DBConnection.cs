using Npgsql;

namespace Questore.Persistence
{
    public class DBConnection
    {
        private const string Host = "kandula.db.elephantsql.com";
        private const string Username = "hfyfzvrh";
        private const string Password = "M7N7RnAE2hgq0LNoIdRLEjFS536vflMA";
        private const string Database = "hfyfzvrh";

        private string GetConnectionString()
        {
            return $"Host={Host};Username={Username};Password={Password};Database={Database}";
        }
        public NpgsqlConnection GetOpenConnectionObject()
        {
            var connection = new NpgsqlConnection(GetConnectionString());
            connection.Open();
            return connection;
        }
    }
}