using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using NpgsqlTypes;
using Questore.Data.Interfaces;
using Questore.Data.Models;

namespace Questore.Data.Persistence
{
    public class QuestDAO : DefaultDAO, IQuestDAO
    {
        private readonly string _table = "quest";

        public QuestDAO(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<Quest> GetQuests()
        {
            using var connection = Connection;
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
            using var connection = Connection;
            var query = $"SELECT id, name, description, reward, image_url " +
                        $"FROM {_table} " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

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
            using var connection = Connection;
            var query = $"INSERT INTO {_table}(name, description, reward, image_url, category_id) " +
                        $"VALUES (@name, @description, @reward, @image_url, @category_id);";
            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = quest.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = quest.Description;
            command.Parameters.Add("reward", NpgsqlDbType.Integer).Value = quest.Reward;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = "default_url";
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = 1;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void UpdateQuest(int id, Quest updatedQuest)
        {
            using var connection = Connection;
            var query = $"UPDATE {_table} " +
                        $"SET name = @name, " +
                        $"description = @description, " +
                        $"reward = @reward, " +
                        $"image_url = @image_url, " +
                        $"category_id = @category_id " +
                        $"WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);

            command.Parameters.Add("name", NpgsqlDbType.Varchar).Value = updatedQuest.Name;
            command.Parameters.Add("description", NpgsqlDbType.Varchar).Value = updatedQuest.Description;
            command.Parameters.Add("reward", NpgsqlDbType.Integer).Value = updatedQuest.Reward;
            command.Parameters.Add("image_url", NpgsqlDbType.Varchar).Value = "default_url";
            command.Parameters.Add("category_id", NpgsqlDbType.Integer).Value = 1;
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void DeleteQuest(int id)
        {
            using var connection = Connection;
            var query = $"DELETE FROM {_table}" +
                        $" WHERE id = @id;";

            using var command = new NpgsqlCommand(query, connection);
            command.Parameters.Add("id", NpgsqlDbType.Integer).Value = id;

            command.Prepare();
            command.ExecuteNonQuery();
        }

        public void ClaimQuest(int questId, int studentId)
        {
            var query = $"UPDATE student " +
                        $"SET coolcoins = student.coolcoins + quest.reward, " +
                        $"experience = student.experience + quest.reward " +
                        $"FROM quest " +
                        $"WHERE quest.id = @questId AND student.id = @studentId;";

            using var command = new NpgsqlCommand(query, Connection);
            command.Parameters.Add("questId", NpgsqlDbType.Integer).Value = questId;
            command.Parameters.Add("studentId", NpgsqlDbType.Integer).Value = studentId;

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
            using var connection = Connection;
            var query = $"SELECT category_quest.id, category_quest.name " +
                        $"FROM category_quest " +
                        $"INNER JOIN quest on quest.category_id = category_quest.id " +
                        $"WHERE quest.id = @id;";

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