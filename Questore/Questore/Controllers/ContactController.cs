using Microsoft.AspNetCore.Mvc;

namespace Questore.Controllers
{
    public class ContactController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}