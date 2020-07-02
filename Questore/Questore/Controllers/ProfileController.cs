using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questore.Models;
using Questore.Persistence;
using System;

namespace Questore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStudentDAO _studentDao;
        private readonly ISession _session;
        private Student ActiveStudent => JsonConvert.DeserializeObject<Student>(_session.GetString("user"));


        public ProfileController(IServiceProvider services)
        {
            _studentDao = new StudentDAO();
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        public IActionResult Index()
        {
            var student = _studentDao.GetStudent(ActiveStudent.Id);
            if (student == null)
                return RedirectToAction("Logout", "Login");
            return View(student);
        }
    }
}