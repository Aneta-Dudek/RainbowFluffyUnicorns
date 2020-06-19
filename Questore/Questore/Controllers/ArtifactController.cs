using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Questore.Controllers
{
    public class ArtifactController : Controller
    {
        private readonly IArtifactDAO _artifactDao;
        private readonly ISession _session;
        private Student ActiveStudent => JsonConvert.DeserializeObject<Student>(_session.GetString("user"));

        public ArtifactController(IServiceProvider services)
        {
            _artifactDao = new ArtifactDAO();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }
        public IActionResult Index()
        {
            var artifacts = _artifactDao.GetArtifacts().ToList();
            CheckAffordability(artifacts);
            return View(artifacts);
        }

        private void CheckAffordability(List<Artifact> artifacts)
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
            //UPDATE SESSION
            return RedirectToAction("Index", "Profile");
        }
    }
}