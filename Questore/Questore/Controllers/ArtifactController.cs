using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;
using Questore.Models;

namespace Questore.Controllers
{
    public class ArtifactController : Controller
    {
        private readonly IArtifactDAO _artifactDao;

        public ArtifactController()
        {
            _artifactDao = new ArtifactDAO();
        }
        public IActionResult Index()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            return View(artifacts);
        }

        [HttpPost]
        public IActionResult Use(int id)
        {
            _artifactDao.UseArtifact(id);
            return RedirectToActionPermanent("Index", "Profile");
        }
    }
}