using Microsoft.AspNetCore.Mvc;
using Questore.Services;
using System.Linq;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly QuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = (QuestService)questService;
        }

        public IActionResult Index()
        {
            var quests = _questService.GetQuests().ToList();
            return View(quests);
        }

        public IActionResult Claim(int id)
        {
            _questService.ClaimQuest(id);
            return RedirectToAction("Index", "Profile");
        }
    }
}