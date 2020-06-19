using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly IQuestDAO _questDao;

        public QuestController()
        {
            _questDao = new QuestDAO();
        }

        public IActionResult Index()
        {
            var quests = _questDao.GetQuests().ToList();
            return View(quests);
        }
    }
}