using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Questore.Data.Dtos;
using Questore.Data.Interfaces;
using Questore.Data.Persistence;

namespace Questore.Controllers
{
    public class LoginController : BaseController
    {
        private Authentication _authentication;

        public LoginController(IServiceProvider services, IAdminDAO adminDao, IConfiguration configuration, IStudentDAO studentDao) 
            : base(services)
        {
            _authentication = new Authentication(studentDao, adminDao, configuration);
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
            {
                ModelState.AddModelError("", "Username/password not found");
                return View(login);
            }

            if (user.Role == "student")
            {
                _session.SetString("user", JsonSerializer.Serialize<object>(user));
                return RedirectToAction("index", "quest");
            } 
            else if (user.Role == "admin")
            {
                _session.SetString("user", JsonSerializer.Serialize<object>(user));
                return RedirectToAction("Index", "Admin");
            }

            return View(login);
        }

        public IActionResult Logout()
        {
            _session.Remove("user");
            return RedirectToAction("Index");
        }
    }
}