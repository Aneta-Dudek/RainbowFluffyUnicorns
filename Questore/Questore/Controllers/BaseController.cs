using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Questore.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly ISession _session;

        protected BaseController(IServiceProvider services)
        {
            _session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
        }
    }
}
