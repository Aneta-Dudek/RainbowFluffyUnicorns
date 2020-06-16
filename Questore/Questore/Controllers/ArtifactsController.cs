using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;

namespace Questore.Controllers
{
    public class ArtifactsController : Controller
    {
        private readonly IArtifactDAO _artifactDao;

        public ArtifactsController()
        {
            _artifactDao = new ArtifactDAO();
        }
        public IActionResult Index()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            return View(artifacts);
        }
    }
}