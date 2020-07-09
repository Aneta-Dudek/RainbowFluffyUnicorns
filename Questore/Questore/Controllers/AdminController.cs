using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

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
