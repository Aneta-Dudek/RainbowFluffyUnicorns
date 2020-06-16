using Microsoft.AspNetCore.Mvc;

namespace Questore.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

    }
}
