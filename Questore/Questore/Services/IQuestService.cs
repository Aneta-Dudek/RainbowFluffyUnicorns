using Questore.Models;
using System.Collections.Generic;

namespace Questore.Services
{
    public interface IQuestService
    {
        public IEnumerable<Quest> GetQuests();
        public void ClaimQuest(int id);
    }
}