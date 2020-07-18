using Microsoft.AspNetCore.Mvc;
using Questore.Services.Interfaces;

namespace Questore.Controllers
{
    public class ArtifactController : Controller
    {
        private readonly IArtifactService _artifactService;

        public ArtifactController(IArtifactService artifactService)
        {
            _artifactService = artifactService;
        }
        public IActionResult Index()
        {
            var artifacts = _artifactService.MarkAffordableArtifacts();
            return View(artifacts);
        }

        [HttpPost]
        public IActionResult Use(int id)
        {
            _artifactService.UseArtifact(id);
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public IActionResult Buy(int id)
        {
            _artifactService.BuyArtifact(id);
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult DeleteStudentArtifact(int id)
        {
            _artifactService.DeleteStudentArtifact(id);
            return RedirectToAction("Index", "Profile");
        }
    }
}