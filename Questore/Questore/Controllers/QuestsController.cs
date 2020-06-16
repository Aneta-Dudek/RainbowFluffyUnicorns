using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;

namespace Questore.Controllers
{
    public class QuestsController : Controller
    {
        private readonly IQuestDAO _questDao;

        public QuestsController()
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