using System;
using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly IQuestDAO _questDao;

        private readonly ISession _session;

        private readonly IStudentDAO _student;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));


        public QuestController(IServiceProvider services)
        {
            _questDao = new QuestDAO();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            _student = new StudentDAO();
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
            _session.SetString("user", JsonSerializer.Serialize(updatedStudent));
            return RedirectToAction("Index", "Profile");
        }
    }
}