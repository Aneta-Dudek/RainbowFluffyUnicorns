using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Text.Json;

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
        public IActionResult Index(Login login)
        {
            var user = _authentication.Authenticate(login);
            if (user == null)
                return RedirectToAction("Index");

            _session.SetString("user", JsonSerializer.Serialize(user));

            return RedirectToAction("index", "quest");
        }

        public IActionResult Logout()
        {
            _session.Remove("user");
            return RedirectToAction("Index");
        }
    }
}