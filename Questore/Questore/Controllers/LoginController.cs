using Microsoft.AspNetCore.Mvc;

namespace Questore.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}