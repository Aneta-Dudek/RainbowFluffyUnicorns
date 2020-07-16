using System.Collections.Generic;
using Questore.Data.Models;

namespace Questore.Services
{
    public interface IQuestService
    {
        public IEnumerable<Quest> GetQuests();
        public void ClaimQuest(int id);
    }
}