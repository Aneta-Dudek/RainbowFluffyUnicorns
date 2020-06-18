using Microsoft.AspNetCore.Mvc;
using Questore.Models;
using Questore.Persistence;

namespace Questore.Controllers
{
    public class LoginController : Controller
    {
        private Authentication _authentication;

        public LoginController()
        {
            _authentication = new Authentication();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Login login)
        {
            var user = _authentication.Authenticate(login);
            return RedirectToAction("index", "quests");
        }
    }
}