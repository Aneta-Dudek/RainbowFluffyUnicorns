using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Questore.Models;
using Questore.Persistence;
using System;
using Questore.Dtos;

namespace Questore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStudentDAO _studentDao;

        private readonly IDetailsDAO _detailsDao;

        private readonly ISession _session;

        private Student ActiveStudent => JsonConvert.DeserializeObject<Student>(_session.GetString("user"));


        public ProfileController(IServiceProvider services, IStudentDAO studentDao, IDetailsDAO detailsDao)
        {
            _studentDao = studentDao;
            _detailsDao = detailsDao;
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }

        [HttpPost]
        public IActionResult AddDetail(DetailDto detailDto)
        {
            _detailsDao.AddDetail(detailDto);
            return RedirectToAction("Index");
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