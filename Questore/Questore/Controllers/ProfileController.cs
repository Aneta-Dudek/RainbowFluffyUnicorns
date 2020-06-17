using Microsoft.AspNetCore.Mvc;
using Questore.Persistence;

namespace Questore.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IStudentDAO _studentDao;

        public ProfileController()
        {
            _studentDao = new StudentDAO();

        }

        public IActionResult Index()
        {
            var student = _studentDao.GetStudent(1);
            return View(student);
        }
    }
}