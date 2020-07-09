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
    public class QuestController : BaseController
    {
        private readonly IQuestDAO _questDao;

        private readonly IStudentDAO _student;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));


        public QuestController(IServiceProvider services, IQuestDAO questDao, IStudentDAO studentDao) :
            base(services)
        {
            _questDao = questDao;
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
            return RedirectToAction("Index", "Profile");
        }
    }
}