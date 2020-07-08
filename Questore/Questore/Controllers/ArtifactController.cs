using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Questore.Controllers
{
    public class ArtifactController : Controller
    {
        private readonly IArtifactDAO _artifactDao;

        private readonly ISession _session;

        private readonly IStudentDAO _studentDao;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public ArtifactController(IServiceProvider services, IArtifactDAO artifactDao, IStudentDAO studentDao)
        {
            _artifactDao = artifactDao;
            _studentDao = studentDao;
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }
        public IActionResult Index()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            CheckAffordability(artifacts);
            return View(artifacts);
        }

        private void CheckAffordability(IEnumerable<Artifact> artifacts)
        {
            foreach (var artifact in artifacts)
            {
                artifact.IsAffordable = ActiveStudent.Coolcoins >= artifact.Price;
            }
        }

        [HttpPost]
        public IActionResult Use(int id)
        {
            _artifactDao.UseArtifact(id);
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public IActionResult Buy(int id)
        {
            _artifactDao.BuyArtifact(id, ActiveStudent.Id);
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult DeleteStudentArtifact(int id)
        {
            _artifactDao.DeleteStudentArtifact(id);
            return RedirectToAction("Index", "Profile");
        }
    }
}