using System;
using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questore.Models;

namespace Questore.Controllers
{
    public class QuestController : Controller
    {
        private readonly IQuestDAO _questDao;
        private readonly ISession _session;

        private Student ActiveStudent => JsonConvert.DeserializeObject<Student>(_session.GetString("user"));


        public QuestController(IServiceProvider services)
        {
            _questDao = new QuestDAO();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

        }

        public IActionResult Index()
        {
            var quests = _questDao.GetQuests().ToList();
            return View(quests);
        }

        public IActionResult Claim(int id)
        {
            _questDao.ClaimQuest(id, ActiveStudent.Id);
            //UPDATE SESSION
            return RedirectToAction("Index", "Profile");
        }
    }
}