using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Questore.Models;
using Questore.Persistence;

namespace Questore.Controllers
{
    public class HomeController : Controller
    {
        private readonly IQuestDAO _questDao;

        public HomeController()
        {
            _questDao = new QuestDAO();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Quests()
        {
            var quests = _questDao.GetQuests().ToList();
            return View(quests);
        }

        public IActionResult Artifacts()
        {
            return View();
        }

    }
}
