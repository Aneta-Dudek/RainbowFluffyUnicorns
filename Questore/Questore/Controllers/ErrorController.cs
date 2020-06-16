using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Questore.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {

        [Route("/Home/Error")]
        public IActionResult Error() => RedirectToAction("Index", "Home");
    }
}