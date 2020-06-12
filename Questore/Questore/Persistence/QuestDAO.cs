using System.Collections.Generic;
using Npgsql;
using NpgsqlTypes;
using Questore.Models;

namespace Questore.Persistence
{
    public class QuestDAO : IQuestDAO
    {
        private readonly DBConnection _connection;

        private readonly string _table = "quest";

        public QuestDAO()
        {
            _connection = new DBConnection();
        }

        public IEnumerable<Quest> GetQuests()
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id, name, description, reward, image_url " +
                        $"FROM {_table};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var quests = new List<Quest>();

            while (reader.Read())
            {
                var quest = ProvideOneQuest(reader);

                quests.Add(quest);
            }

            return quests;
        }

        public Quest GetQuest(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT id, name, description, reward, image_url " +
                              $"FROM {_table} " +
                              $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);
            var reader = command.ExecuteReader();

            var quest = new Quest();

            while (reader.Read())
            {
                quest = ProvideOneQuest(reader);
            }

            return quest;
        }

        public void AddQuest(Quest quest)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();
            var query = $"INSERT INTO {_table}(name, description, reward, image_url, category_id) " +
                        $"VALUES (@name, @description, @reward, @image_url, @category_id);";
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = quest.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = quest.Description;
            command.Parameters.Add("reward", NpgsqlDbType.Integer).Value = quest.Reward;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = quest.ImageUrl;
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = quest.Category.Id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateQuest(int id, Quest updatedQuest)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"UPDATE {_table} " +
                        $"SET name = '{updatedQuest.Name}', " +
                        $"description = '{updatedQuest.Description}', " +
                        $"reward = {updatedQuest.Reward}, " +
                        $"image_url = '{updatedQuest.ImageUrl}', " +
                        $"category_id = {updatedQuest.Category.Id} " +
                        $"WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = updatedQuest.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = updatedQuest.Description;
            command.Parameters.Add("reward", NpgsqlDbType.Integer).Value = updatedQuest.Reward;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = updatedQuest.ImageUrl;
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = updatedQuest.Category.Id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteQuest(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"DELETE FROM {_table}" +
                        $" WHERE id = {id};";

            using var command = new NpgsqlCommand(query, connection);

            command.Prepare();
            command.ExecuteNonQuery();
        }

        private Quest ProvideOneQuest(NpgsqlDataReader reader)
        {
            var quest = new Quest()
            {
                Id = reader.GetInt32((int)DBUtilities.QuestEnum.Id),
                Name = reader.GetString((int)DBUtilities.QuestEnum.Name),
                Description = reader.GetString((int)DBUtilities.QuestEnum.Description),
                Reward = reader.GetInt32((int)DBUtilities.QuestEnum.Reward),
                ImageUrl = reader.GetString((int)DBUtilities.QuestEnum.ImageUrl)
            };

            quest.Category = GetQuestCategory(quest.Id);

            return quest;
        }

        private Category GetQuestCategory(int id)
        {
            using NpgsqlConnection connection = _connection.GetOpenConnectionObject();

            var query = $"SELECT category_quest.id, category_quest.name " +
                        $"FROM category_quest " +
                        $"INNER JOIN quest on quest.category_id = category_quest.id " +
                        $"WHERE quest.id = {id};";

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