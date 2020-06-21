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

        private readonly IStudentDAO _student;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));

        public ArtifactController(IServiceProvider services)
        {
            _artifactDao = new ArtifactDAO();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _student = new StudentDAO();
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
            var updatedStudent = _student.GetStudent(ActiveStudent.Id);
            if (updatedStudent == null)
                return RedirectToAction("Index");
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
            return RedirectToAction("Index", "Profile");
        }

        [HttpPost]
        public IActionResult Buy(int id)
        {
            _artifactDao.BuyArtifact(id, ActiveStudent.Id);
            var updatedStudent = _student.GetStudent(ActiveStudent.Id);
            if (updatedStudent == null)
                return RedirectToAction("Index");
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
            return RedirectToAction("Index", "Profile");
        }

        public IActionResult DeleteStudentArtifact(int id)
        {
            _artifactDao.DeleteStudentArtifact(id);
            var updatedStudent = _student.GetStudent(ActiveStudent.Id);
            if (updatedStudent == null)
                return RedirectToAction("Index");
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
            return RedirectToAction("Index", "Profile");
        }
    }
}