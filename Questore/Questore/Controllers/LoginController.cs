﻿using System;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;

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
            _session.SetString("user", JsonSerializer.Serialize(user));

            return RedirectToAction("index", "quest");
        }
    }
}