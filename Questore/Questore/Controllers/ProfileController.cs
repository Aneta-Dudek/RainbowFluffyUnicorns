using Microsoft.AspNetCore.Mvc;

namespace Questore.Controllers
{
    public class ProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}