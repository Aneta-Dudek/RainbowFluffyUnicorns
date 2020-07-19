using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;

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
