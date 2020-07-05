using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Linq;
using System.Text.Json;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly IQuestDAO _questDao;

        private readonly ISession _session;

        private readonly IStudentDAO _student;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));


        public QuestController(IServiceProvider services, IQuestDAO questDao, IStudentDAO studentDao)
        {
            _questDao = questDao;
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _student = studentDao;
        }

        public IActionResult Index()
        {
            var quests = _questDao.GetQuests().ToList();
            return View(quests);
        }

        public IActionResult Claim(int id)
        {
            _questDao.ClaimQuest(id, ActiveStudent.Id);
            var updatedStudent = _student.GetStudent(ActiveStudent.Id);
            if (updatedStudent == null)
                return RedirectToAction("Index");
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
            return RedirectToAction("Index", "Profile");
        }
    }
}