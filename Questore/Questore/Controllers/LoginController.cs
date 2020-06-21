using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Questore.Controllers
{
    public class LoginController : Controller
    {
        private Authentication _authentication;

        private ISession _session;

        public LoginController(IServiceProvider services)
        {
            _authentication = new Authentication();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Login login)
        {
            if (!ModelState.IsValid)
                return View(login);

            var user = _authentication.Authenticate(login);
            if (user != null)
            {
                _session.SetString("user", JsonSerializer.Serialize(user));
                return RedirectToAction("index", "quest");
            }

            ModelState.AddModelError("", "Username/password not found");
            return View(login);
        }

        public IActionResult Logout()
        {
            _session.Remove("user");
            return RedirectToAction("Index");
        }
    }
}