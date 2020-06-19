using System.Collections.Generic;
using Questore.Models;

namespace Questore.Persistence
{
    public interface IQuestDAO
    {
        IEnumerable<Quest> GetQuests();
        Quest GetQuest(int id);
        void AddQuest(Quest quest);
        void UpdateQuest(int id, Quest updatedQuest);
        void DeleteQuest(int id);
        void ClaimQuest(int questId, int studentId);
    }
}