using Microsoft.AspNetCore.Mvc;
using Questore.Services;
using System.Linq;
using Questore.Services.Interfaces;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly IQuestService _questService;

        public QuestController(IQuestService questService)
        {
            _questService = questService;
        }

        public IActionResult Index()
        {
            var quests = _questService.GetQuests();
            return View(quests);
        }

        public IActionResult Claim(int id)
        {
            _questService.ClaimQuest(id);
            return RedirectToAction("Index", "Profile");
        }
    }
}