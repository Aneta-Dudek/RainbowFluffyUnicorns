using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Questore.Models;
using Questore.Persistence;
using System;
using System.Text.Json;
using Questore.Dtos;
using Questore.ModelState;
using Questore.ViewModel;

namespace Questore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStudentDAO _studentDao;

        private readonly IDetailsDAO _detailsDao;

        private readonly ISession _session;

        private Student ActiveStudent => JsonSerializer.Deserialize<Student>(_session.GetString("user"));


        public ProfileController(IServiceProvider services, IStudentDAO studentDao, IDetailsDAO detailsDao)
        {
            _studentDao = studentDao;
            _detailsDao = detailsDao;
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }


        public IActionResult DeleteStudentDetail(int id)
        {
            _detailsDao.DeleteDetail(id);
            return RedirectToAction("Index");
        }


        [HttpPost]
        [ExportModelState]
        public IActionResult AddDetail(DetailDto detail)
        {

            if (!ModelState.IsValid)
            {
                TempData["Detail"] = JsonSerializer.Serialize(detail);
                return RedirectToAction("Index");
            }

            detail.StudentId = ActiveStudent.Id;
            _detailsDao.AddDetail(detail);
            return RedirectToAction("Index");
        }

        [ImportModelState]
        public IActionResult Index()
        {
            var student = _studentDao.GetStudent(ActiveStudent.Id);
            if (student == null)
                return RedirectToAction("Logout", "Login");

            var detail = TempData.ContainsKey("Detail")
                ? JsonSerializer.Deserialize<DetailDto>(TempData["Detail"].ToString())
                : null;

            var profile = new Profile
            {
                Detail = detail,
                Student = student
            };

            return View(profile);
        }
    }
}