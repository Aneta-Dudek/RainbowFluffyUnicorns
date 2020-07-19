using Microsoft.AspNetCore.Mvc;
using System;

namespace Questore.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IServiceProvider services) :
            base(services)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
