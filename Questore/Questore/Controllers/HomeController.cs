using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;

namespace Questore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestDAO _questDao;

        private readonly IArtifactDAO _artifactDao;

        public HomeController()
        {
            _questDao = new QuestDAO();
            _artifactDao = new ArtifactDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Quests()
        {
            var quests = _questDao.GetQuests().ToList();
            return View(quests);
        }

        public IActionResult Artifacts()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            return View(artifacts);
        }

    }
}
